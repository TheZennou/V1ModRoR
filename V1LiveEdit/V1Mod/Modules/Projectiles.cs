using System;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using EntityStates.Loader;

namespace V1Mod.Modules
{
	internal static class Projectiles
	{
		internal static GameObject bombPrefab;
		internal static GameObject Hook;

		internal static void RegisterProjectiles()
		{
			// only separating into separate methods for my sanity
			CreateBomb();

			AddProjectile(bombPrefab);
		}

		internal static void AddProjectile(GameObject projectileToAdd)
		{
			Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
		}

		private static void CreateBomb()
		{
			bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

			ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
			InitializeImpactExplosion(bombImpactExplosion);

			bombImpactExplosion.blastRadius = 16f;
			bombImpactExplosion.destroyOnEnemy = true;
			bombImpactExplosion.lifetime = 12f;
			bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
			//bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
			bombImpactExplosion.timerAfterImpact = true;
			bombImpactExplosion.lifetimeAfterImpact = 0.1f;

			ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
			if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("HenryBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
			bombController.startSound = "";
		}

		private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
		{
			projectileImpactExplosion.blastDamageCoefficient = 1f;
			projectileImpactExplosion.blastProcCoefficient = 1f;
			projectileImpactExplosion.blastRadius = 1f;
			projectileImpactExplosion.bonusBlastForce = Vector3.zero;
			projectileImpactExplosion.childrenCount = 0;
			projectileImpactExplosion.childrenDamageCoefficient = 0f;
			projectileImpactExplosion.childrenProjectilePrefab = null;
			projectileImpactExplosion.destroyOnEnemy = false;
			projectileImpactExplosion.destroyOnWorld = false;
			projectileImpactExplosion.explosionSoundString = "";
			projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
			projectileImpactExplosion.fireChildren = false;
			projectileImpactExplosion.impactEffect = null;
			projectileImpactExplosion.lifetime = 0f;
			projectileImpactExplosion.lifetimeAfterImpact = 0f;
			projectileImpactExplosion.lifetimeExpiredSoundString = "";
			projectileImpactExplosion.lifetimeRandomOffset = 0f;
			projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
			projectileImpactExplosion.timerAfterImpact = false;

			projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
		}

		private static GameObject CreateGhostPrefab(string ghostName)
		{
			GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
			if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
			if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

			Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

			return ghostPrefab;
		}

		private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
		{
			GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
			return newPrefab;
		}
	}

	namespace V1Mod.HookProjectile
	{
		[RequireComponent(typeof(EntityStateMachine))]
		[RequireComponent(typeof(ProjectileController))]
		[RequireComponent(typeof(ProjectileStickOnImpact))]
		[RequireComponent(typeof(ProjectileSimple))]
		public class ProjectileHookController : MonoBehaviour
		{
			// Token: 0x06002925 RID: 10533 RVA: 0x000A6B1C File Offset: 0x000A4D1C
			private void Awake()
			{
				this.projectileStickOnImpactController = base.GetComponent<ProjectileStickOnImpact>();
				this.projectileController = base.GetComponent<ProjectileController>();
				this.projectileSimple = base.GetComponent<ProjectileSimple>();
				this.resolvedOwnerHookStateType = this.ownerHookStateType.stateType;
				if (this.ropeEndTransform)
				{
					this.soundID = Util.PlaySound(this.enterSoundString, this.ropeEndTransform.gameObject);
				}
			}

			// Token: 0x06002926 RID: 10534 RVA: 0x000A6B88 File Offset: 0x000A4D88
			private void FixedUpdate()
			{
				if (this.ropeEndTransform)
				{
					float in_value = Util.Remap((this.ropeEndTransform.transform.position - base.transform.position).magnitude, this.minHookDistancePitchModifier, this.maxHookDistancePitchModifier, 0f, 100f);
					AkSoundEngine.SetRTPCValueByPlayingID(this.hookDistanceRTPCstring, in_value, this.soundID);
				}
			}

			// Token: 0x06002927 RID: 10535 RVA: 0x000A6BFC File Offset: 0x000A4DFC
			private void AssignHookReferenceToBodyStateMachine()
			{
				FireHook Hook;
				if (this.owner.stateMachine && (Hook = (this.owner.stateMachine.state as FireHook)) != null)
				{
					Hook.SetHookReference(base.gameObject);
				}
				Transform modelTransform = this.owner.gameObject.GetComponent<ModelLocator>().modelTransform;
				if (modelTransform)
				{
					ChildLocator component = modelTransform.GetComponent<ChildLocator>();
					if (component)
					{
						Transform transform = component.FindChild(this.muzzleStringOnBody);
						if (transform)
						{
							this.ropeEndTransform.SetParent(transform, false);
						}
					}
				}
			}

			// Token: 0x06002928 RID: 10536 RVA: 0x000A6C8F File Offset: 0x000A4E8F
			private void Start()
			{
				this.owner = new ProjectileHookController.OwnerInfo(this.projectileController.owner);
				this.AssignHookReferenceToBodyStateMachine();
			}

			// Token: 0x06002929 RID: 10537 RVA: 0x000A6CB0 File Offset: 0x000A4EB0
			private void OnDestroy()
			{
				if (this.ropeEndTransform)
				{
					Util.PlaySound(this.exitSoundString, this.ropeEndTransform.gameObject);
					UnityEngine.Object.Destroy(this.ropeEndTransform.gameObject);
					return;
				}
				AkSoundEngine.StopPlayingID(this.soundID);
			}

			// Token: 0x0600292A RID: 10538 RVA: 0x000A6CFD File Offset: 0x000A4EFD
			private bool OwnerIsInFiringState()
			{
				return this.owner.stateMachine && this.owner.stateMachine.state.GetType() == this.resolvedOwnerHookStateType;
			}

			// Token: 0x0400238D RID: 9101
			private ProjectileController projectileController;

			// Token: 0x0400238E RID: 9102
			private ProjectileStickOnImpact projectileStickOnImpactController;

			// Token: 0x0400238F RID: 9103
			private ProjectileSimple projectileSimple;

			// Token: 0x04002390 RID: 9104
			public SerializableEntityStateType ownerHookStateType;

			// Token: 0x04002391 RID: 9105
			public float acceleration;

			// Token: 0x04002392 RID: 9106
			public float lookAcceleration = 4f;

			// Token: 0x04002393 RID: 9107
			public float lookAccelerationRampUpDuration = 0.25f;

			// Token: 0x04002394 RID: 9108
			public float initialLookImpulse = 5f;

			// Token: 0x04002395 RID: 9109
			public float initiallMoveImpulse = 5f;

			// Token: 0x04002396 RID: 9110
			public float moveAcceleration = 4f;

			// Token: 0x04002397 RID: 9111
			public string enterSoundString;

			// Token: 0x04002398 RID: 9112
			public string exitSoundString;

			// Token: 0x04002399 RID: 9113
			public string hookDistanceRTPCstring;

			// Token: 0x0400239A RID: 9114
			public float minHookDistancePitchModifier;

			// Token: 0x0400239B RID: 9115
			public float maxHookDistancePitchModifier;

			// Token: 0x0400239C RID: 9116
			public AnimationCurve lookAccelerationRampUpCurve;

			// Token: 0x0400239D RID: 9117
			public Transform ropeEndTransform;

			// Token: 0x0400239E RID: 9118
			public string muzzleStringOnBody = "MuzzleLeft";

			// Token: 0x0400239F RID: 9119
			[Tooltip("The minimum distance the hook can be from the target before it detaches.")]
			public float nearBreakDistance;

			// Token: 0x040023A0 RID: 9120
			[Tooltip("The maximum distance this hook can travel.")]
			public float maxTravelDistance;

			// Token: 0x040023A1 RID: 9121
			public float escapeForceMultiplier = 2f;

			// Token: 0x040023A2 RID: 9122
			public float normalOffset = 1f;

			// Token: 0x040023A3 RID: 9123
			public float yankMassLimit;

			// Token: 0x040023A4 RID: 9124
			private Type resolvedOwnerHookStateType;

			// Token: 0x040023A5 RID: 9125
			private ProjectileHookController.OwnerInfo owner;

			// Token: 0x040023A6 RID: 9126
			private bool didStick;

			// Token: 0x040023A7 RID: 9127
			private uint soundID;

			// Token: 0x020006A0 RID: 1696
			private struct OwnerInfo
			{
				// Token: 0x0600292C RID: 10540 RVA: 0x000A6DA0 File Offset: 0x000A4FA0
				public OwnerInfo(GameObject ownerGameObject)
				{
					this = default(ProjectileHookController.OwnerInfo);
					this.gameObject = ownerGameObject;
					if (this.gameObject)
					{
						this.characterBody = this.gameObject.GetComponent<CharacterBody>();
						this.characterMotor = this.gameObject.GetComponent<CharacterMotor>();
						this.rigidbody = this.gameObject.GetComponent<Rigidbody>();
						this.hasEffectiveAuthority = Util.HasEffectiveAuthority(this.gameObject);
						EntityStateMachine[] components = this.gameObject.GetComponents<EntityStateMachine>();
						for (int i = 0; i < components.Length; i++)
						{
							if (components[i].customName == "Hook")
							{
								this.stateMachine = components[i];
								return;
							}
						}
					}
				}
				public readonly GameObject gameObject;

				public readonly CharacterBody characterBody;

				public readonly CharacterMotor characterMotor;

				public readonly Rigidbody rigidbody;

				public readonly EntityStateMachine stateMachine;
				public readonly bool hasEffectiveAuthority;
			}
			private class BaseState : EntityStates.BaseState
			{
				private bool ownerValid;

				protected bool GetOwnerValid()
				{
					return ownerValid;
				}

				private void SetOwnerValid(bool value)
				{
					ownerValid = value;
				}

				protected ref ProjectileHookController.OwnerInfo owner
				{
					get
					{
						return ref this.grappleController.owner;
					}
				}

				private void UpdatePositions()
				{
					this.aimOrigin = this.grappleController.owner.characterBody.aimOrigin;
					this.position = base.transform.position + base.transform.up * this.grappleController.normalOffset;
				}


				public override void OnEnter()
				{
					base.OnEnter();
					this.grappleController = base.GetComponent<ProjectileHookController>();
					this.SetOwnerValid((this.grappleController && this.grappleController.owner.gameObject));
					if (this.GetOwnerValid())
					{
						this.UpdatePositions();
					}
				}

				// Token: 0x06002932 RID: 10546 RVA: 0x000A6F18 File Offset: 0x000A5118
				public override void FixedUpdate()
				{
					base.FixedUpdate();
					if (this.GetOwnerValid())
					{
						this.SetOwnerValid(this.GetOwnerValid() & this.grappleController.owner.gameObject);
						if (this.GetOwnerValid())
						{
							this.UpdatePositions();
							this.FixedUpdateBehavior();
						}
					}
					if (NetworkServer.active && !this.GetOwnerValid())
					{
						this.SetOwnerValid(false);
						EntityState.Destroy(base.gameObject);
						return;
					}
				}

				// Token: 0x06002933 RID: 10547 RVA: 0x000A6F8B File Offset: 0x000A518B
				protected virtual void FixedUpdateBehavior()
				{
					if (base.isAuthority && !this.grappleController.OwnerIsInFiringState())
					{
						this.outer.SetNextState(new ProjectileHookController.ReturnState());
						return;
					}
				}

				// Token: 0x06002934 RID: 10548 RVA: 0x000A6FB4 File Offset: 0x000A51B4
				protected Ray GetOwnerAimRay()
				{
					if (!this.owner.characterBody)
					{
						return default(Ray);
					}
					return this.owner.characterBody.inputBank.GetAimRay();
				}

				// Token: 0x040023AE RID: 9134
				protected ProjectileHookController grappleController;

				// Token: 0x040023B0 RID: 9136
				protected Vector3 aimOrigin;

				// Token: 0x040023B1 RID: 9137
				protected Vector3 position;
			}

			// Token: 0x020006A2 RID: 1698
			private class FlyState : ProjectileHookController.BaseState
			{
				// Token: 0x06002936 RID: 10550 RVA: 0x000A6FF2 File Offset: 0x000A51F2
				public override void OnEnter()
				{
					base.OnEnter();
					this.duration = this.grappleController.maxTravelDistance / this.grappleController.GetComponent<ProjectileSimple>().velocity;
				}

				// Token: 0x06002937 RID: 10551 RVA: 0x000A701C File Offset: 0x000A521C
				protected override void FixedUpdateBehavior()
				{
					base.FixedUpdateBehavior();
					if (base.isAuthority)
					{
						if (this.grappleController.projectileStickOnImpactController.stuck)
						{
							EntityState entityState = null;
							if (this.grappleController.projectileStickOnImpactController.stuckBody)
							{
								Rigidbody component = this.grappleController.projectileStickOnImpactController.stuckBody.GetComponent<Rigidbody>();
								if (component && component.mass < this.grappleController.yankMassLimit)
								{
									CharacterBody component2 = component.GetComponent<CharacterBody>();
									if (!component2 || !component2.isPlayerControlled || component2.teamComponent.teamIndex != base.projectileController.teamFilter.teamIndex || FriendlyFireManager.ShouldDirectHitProceed(component2.healthComponent, base.projectileController.teamFilter.teamIndex))
									{
										entityState = new ProjectileHookController.YankState();
									}
								}
							}
							if (entityState == null)
							{
								entityState = new ProjectileHookController.GripState();
							}
							this.DeductOwnerStock();
							this.outer.SetNextState(entityState);
							return;
						}
						if (this.duration <= base.fixedAge)
						{
							this.outer.SetNextState(new ProjectileHookController.ReturnState());
							return;
						}
					}
				}

				// Token: 0x06002938 RID: 10552 RVA: 0x000A7134 File Offset: 0x000A5334
				private void DeductOwnerStock()
				{
					if (base.GetOwnerValid() && base.owner.hasEffectiveAuthority)
					{
						SkillLocator component = base.owner.gameObject.GetComponent<SkillLocator>();
						if (component)
						{
							GenericSkill secondary = component.secondary;
							if (secondary)
							{
								secondary.DeductStock(1);
							}
						}
					}
				}

				// Token: 0x040023B2 RID: 9138
				private float duration;
			}

			// Token: 0x020006A3 RID: 1699
			private class BaseGripState : ProjectileHookController.BaseState
			{
				// Token: 0x0600293A RID: 10554 RVA: 0x000A718D File Offset: 0x000A538D
				public override void OnEnter()
				{
					base.OnEnter();
					this.currentDistance = Vector3.Distance(this.aimOrigin, this.position);
				}

				// Token: 0x0600293B RID: 10555 RVA: 0x000A71AC File Offset: 0x000A53AC
				protected override void FixedUpdateBehavior()
				{
					base.FixedUpdateBehavior();
					this.currentDistance = Vector3.Distance(this.aimOrigin, this.position);
					if (base.isAuthority)
					{
						bool flag = !this.grappleController.projectileStickOnImpactController.stuck;
						bool flag2 = this.currentDistance < this.grappleController.nearBreakDistance;
						bool flag3 = !this.grappleController.OwnerIsInFiringState();
						bool flag4;
						if (base.owner.stateMachine)
						{
							BaseSkillState baseSkillState = base.owner.stateMachine.state as BaseSkillState;
							flag4 = (baseSkillState == null || !baseSkillState.IsKeyDownAuthority());
						}
						else
						{
							flag4 = true;
						}
						if (flag4 || flag3 || flag2 || flag)
						{
							this.outer.SetNextState(new ProjectileHookController.ReturnState());
							return;
						}
					}
				}

				// Token: 0x040023B3 RID: 9139
				protected float currentDistance;
			}

			// Token: 0x020006A4 RID: 1700
			private class GripState : ProjectileHookController.BaseGripState
			{
				// Token: 0x0600293D RID: 10557 RVA: 0x000A726C File Offset: 0x000A546C
				private void DeductStockIfStruckNonPylon()
				{
					GameObject victim = this.grappleController.projectileStickOnImpactController.victim;
					if (victim)
					{
						GameObject gameObject = victim;
						EntityLocator component = gameObject.GetComponent<EntityLocator>();
						if (component)
						{
							gameObject = component.entity;
						}
						gameObject.GetComponent<ProjectileController>();
					}
				}

				// Token: 0x0600293E RID: 10558 RVA: 0x000A72B8 File Offset: 0x000A54B8
				public override void OnEnter()
				{
					base.OnEnter();
					this.lastDistance = Vector3.Distance(this.aimOrigin, this.position);
					if (base.GetOwnerValid())
					{
						this.grappleController.didStick = true;
						if (base.owner.characterMotor)
						{
							Vector3 direction = base.GetOwnerAimRay().direction;
							Vector3 vector = base.owner.characterMotor.velocity;
							vector = ((Vector3.Dot(vector, direction) < 0f) ? Vector3.zero : Vector3.Project(vector, direction));
							vector += direction * this.grappleController.initialLookImpulse;
							vector += base.owner.characterMotor.moveDirection * this.grappleController.initiallMoveImpulse;
							base.owner.characterMotor.velocity = vector;
						}
					}
				}

				// Token: 0x0600293F RID: 10559 RVA: 0x000A739C File Offset: 0x000A559C
				protected override void FixedUpdateBehavior()
				{
					base.FixedUpdateBehavior();
					float num = this.grappleController.acceleration;
					if (this.currentDistance > this.lastDistance)
					{
						num *= this.grappleController.escapeForceMultiplier;
					}
					this.lastDistance = this.currentDistance;
					if (base.owner.hasEffectiveAuthority && base.owner.characterMotor && base.owner.characterBody)
					{
						Ray ownerAimRay = base.GetOwnerAimRay();
						Vector3 normalized = (base.transform.position - base.owner.characterBody.aimOrigin).normalized;
						Vector3 a = normalized * num;
						float time = Mathf.Clamp01(base.fixedAge / this.grappleController.lookAccelerationRampUpDuration);
						float num2 = this.grappleController.lookAccelerationRampUpCurve.Evaluate(time);
						float num3 = Util.Remap(Vector3.Dot(ownerAimRay.direction, normalized), -1f, 1f, 1f, 0f);
						a += ownerAimRay.direction * (this.grappleController.lookAcceleration * num2 * num3);
						a += base.owner.characterMotor.moveDirection * this.grappleController.moveAcceleration;
						base.owner.characterMotor.ApplyForce(a * (base.owner.characterMotor.mass * Time.fixedDeltaTime), true, true);
					}
				}

				// Token: 0x040023B4 RID: 9140
				private float lastDistance;
			}

			// Token: 0x020006A5 RID: 1701
			private class YankState : ProjectileHookController.BaseGripState
			{
				// Token: 0x06002941 RID: 10561 RVA: 0x000A752C File Offset: 0x000A572C
				public override void OnEnter()
				{
					base.OnEnter();
					this.stuckBody = this.grappleController.projectileStickOnImpactController.stuckBody;
				}

				// Token: 0x06002942 RID: 10562 RVA: 0x000A754C File Offset: 0x000A574C
				protected override void FixedUpdateBehavior()
				{
					base.FixedUpdateBehavior();
					if (this.stuckBody)
					{
						if (Util.HasEffectiveAuthority(this.stuckBody.gameObject))
						{
							Vector3 a = this.aimOrigin - this.position;
							IDisplacementReceiver component = this.stuckBody.GetComponent<IDisplacementReceiver>();
							if ((Component)component && base.fixedAge >= ProjectileHookController.YankState.delayBeforeYanking)
							{
								component.AddDisplacement(a * (ProjectileHookController.YankState.yankSpeed * Time.fixedDeltaTime));
							}
						}
						if (base.owner.hasEffectiveAuthority && base.owner.characterMotor && base.fixedAge < ProjectileHookController.YankState.hoverTimeLimit)
						{
							Vector3 velocity = base.owner.characterMotor.velocity;
							if (velocity.y < 0f)
							{
								velocity.y = 0f;
								base.owner.characterMotor.velocity = velocity;
							}
						}
					}
				}

				// Token: 0x040023B5 RID: 9141
				public static float yankSpeed;

				// Token: 0x040023B6 RID: 9142
				public static float delayBeforeYanking;

				// Token: 0x040023B7 RID: 9143
				public static float hoverTimeLimit = 0.5f;

				// Token: 0x040023B8 RID: 9144
				private CharacterBody stuckBody;
			}

			// Token: 0x020006A6 RID: 1702
			private class ReturnState : ProjectileHookController.BaseState
			{
				// Token: 0x06002945 RID: 10565 RVA: 0x000A7644 File Offset: 0x000A5844
				public override void OnEnter()
				{
					base.OnEnter();
					if (base.GetOwnerValid())
					{
						this.returnSpeed = this.grappleController.projectileSimple.velocity;
						this.returnSpeedAcceleration = this.returnSpeed * 2f;
					}
					if (NetworkServer.active && this.grappleController)
					{
						this.grappleController.projectileStickOnImpactController.Detach();
						this.grappleController.projectileStickOnImpactController.ignoreCharacters = true;
						this.grappleController.projectileStickOnImpactController.ignoreWorld = true;
					}
					Collider component = base.GetComponent<Collider>();
					if (component)
					{
						component.enabled = false;
					}
				}

				// Token: 0x06002946 RID: 10566 RVA: 0x000A76E4 File Offset: 0x000A58E4
				protected override void FixedUpdateBehavior()
				{
					base.FixedUpdateBehavior();
					if (base.rigidbody)
					{
						this.returnSpeed += this.returnSpeedAcceleration * Time.fixedDeltaTime;
						base.rigidbody.velocity = (this.aimOrigin - this.position).normalized * this.returnSpeed;
						if (NetworkServer.active)
						{
							Vector3 endPosition = this.position + base.rigidbody.velocity * Time.fixedDeltaTime;
							if (HGMath.Overshoots(this.position, endPosition, this.aimOrigin))
							{
								EntityState.Destroy(base.gameObject);
								return;
							}
						}
					}
				}

				// Token: 0x040023B9 RID: 9145
				private float returnSpeedAcceleration = 240f;

				// Token: 0x040023BA RID: 9146
				private float returnSpeed;
			}
		}
	}
}