using System;
using RoR2;
using EntityStates;
using EntityStates.Mage;

namespace V1Mod.SkillStates
{
	// Token: 0x02000A9A RID: 2714
	public class JumpDashStart : GenericCharacterMain
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.jetpackStateMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Jet");
		}

		public override void ProcessJump()
		{
			base.ProcessJump();
			if (this.hasCharacterMotor && this.hasInputBank && base.isAuthority)
            {
                object obj = base.inputBank.jump.down && base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded;
                bool flag = this.jetpackStateMachine.state.GetType() == typeof(JetpackOn);
                object obj2 = obj;
                if (obj2 != null && !flag)
					{
                    this.jetpackStateMachine.SetNextState(new JetpackOn());
                }
                if (obj2 == null || !flag)
                {
                    return;
                }
                this.jetpackStateMachine.SetNextState(new Idle());
            }
        }
		public override void OnExit()
		{
			if (base.isAuthority && this.jetpackStateMachine)
			{
				this.jetpackStateMachine.SetNextState(new Idle());
			}
			base.OnExit();
		}
		private EntityStateMachine jetpackStateMachine;
	}
}
