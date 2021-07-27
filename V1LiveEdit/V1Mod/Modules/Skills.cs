using System;
using System.Collections.Generic;
using EntityStates;
using V1Mod.Modules.Misc;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace V1Mod.Modules
	///what did it cost? Everything.
{
	internal static class Skills
	{
		internal static void CreateSkillFamilies(GameObject targetPrefab)
		{
			foreach (GenericSkill obj in targetPrefab.GetComponentsInChildren<GenericSkill>())
			{
                UnityEngine.Object.DestroyImmediate(obj);
			}
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			V1PassiveController V1PassiveController = targetPrefab.AddComponent<V1PassiveController>();

			V1PassiveController.passiveSkill = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily skillFamily5 = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily5.variants = new SkillFamily.Variant[0];
			V1PassiveController.passiveSkill._skillFamily = skillFamily5;

			component.primary = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[0];
			component.primary._skillFamily = skillFamily;

			component.secondary = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily skillFamily2 = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily2.variants = new SkillFamily.Variant[0];
			component.secondary._skillFamily = skillFamily2;

			component.utility = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily skillFamily3 = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily3.variants = new SkillFamily.Variant[0];
			component.utility._skillFamily = skillFamily3;

			component.special = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily skillFamily4 = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily4.variants = new SkillFamily.Variant[0];
			component.special._skillFamily = skillFamily4;

			Skills.skillFamilies.Add(skillFamily5);
			Skills.skillFamilies.Add(skillFamily);
			Skills.skillFamilies.Add(skillFamily2);
			Skills.skillFamilies.Add(skillFamily3);
			Skills.skillFamilies.Add(skillFamily4);

		}


		internal static void AddPassiveSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				Skills.AddPassiveSkill(targetPrefab, skillDef);
			}
		}

		internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef)
		{
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.primary.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
			skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		internal static void AddPrimarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				Skills.AddPrimarySkill(targetPrefab, skillDef);
			}
		}

		internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
		{
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.secondary.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
			skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				Skills.AddSecondarySkill(targetPrefab, skillDef);
			}
		}

		internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
		{
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.utility.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
			skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				Skills.AddUtilitySkill(targetPrefab, skillDef);
			}
		}

		internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
		{
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.special.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
			skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				Skills.AddSpecialSkill(targetPrefab, skillDef);
			}
		}

		internal static void AddPassiveSkill(GameObject targetPrefab, SkillDef skillDef)
		{
			V1PassiveController component = targetPrefab.GetComponent<V1PassiveController>();
			SkillFamily skillFamily = component.passiveSkill.skillFamily;
			Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
			skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}
		//Rob's generic skill creator. MUST KEEP!
		internal static SkillDef CreatePrimarySkillDef(SerializableEntityStateType state, string stateMachine, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile)
		{
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.skillName = skillNameToken;
			skillDef.skillNameToken = skillNameToken;
			skillDef.skillDescriptionToken = skillDescriptionToken;
			skillDef.icon = skillIcon;
			skillDef.activationState = state;
			skillDef.activationStateMachineName = stateMachine;
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.forceSprintDuringState = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.resetCooldownTimerOnUse = false;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.cancelSprintingOnActivation = !agile;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 0;
			skillDef.stockToConsume = 0;
			if (agile)
			{
				skillDef.keywordTokens = new string[]
				{
					"KEYWORD_AGILE"
				};
			}
			Skills.skillDefs.Add(skillDef);
			return skillDef;
		}
		internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
		{
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.skillName = skillDefInfo.skillName;
			skillDef.skillNameToken = skillDefInfo.skillNameToken;
			skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
			skillDef.icon = skillDefInfo.skillIcon;
			skillDef.activationState = skillDefInfo.activationState;
			skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
			skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
			skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
			skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
			skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
			skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
			skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
			skillDef.interruptPriority = skillDefInfo.interruptPriority;
			skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
			skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
			skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
			skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
			skillDef.rechargeStock = skillDefInfo.rechargeStock;
			skillDef.requiredStock = skillDefInfo.requiredStock;
			skillDef.stockToConsume = skillDefInfo.stockToConsume;
			skillDef.keywordTokens = skillDefInfo.keywordTokens;
			Skills.skillDefs.Add(skillDef);
			return skillDef;
		}
		internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
		internal static List<SkillDef> skillDefs = new List<SkillDef>();
	}
}

internal class SkillDefInfo
{
	public string skillName;
	public string skillNameToken;
	public string skillDescriptionToken;
	public Sprite skillIcon;
	public SerializableEntityStateType activationState;
	public string activationStateMachineName;
	public int baseMaxStock;
	public float baseRechargeInterval;
	public bool beginSkillCooldownOnSkillEnd;
	public bool canceledFromSprinting;
	public bool forceSprintDuringState;
	public bool fullRestockOnAssign;
	public InterruptPriority interruptPriority;
	public bool resetCooldownTimerOnUse;
	public bool isCombatSkill;
	public bool mustKeyPress;
	public bool cancelSprintingOnActivation;
	public int rechargeStock;
	public int requiredStock;
	public int stockToConsume;
	public string[] keywordTokens;
}

internal class SkillDefPassiveInfo
{
	public string skillName;
	public string skillNameToken;
	public string skillDescriptionToken;
	public Sprite defIcon;
	public Sprite fireIcon;
	public Sprite lightningIcon;
	public Sprite iceIcon;
	public SerializableEntityStateType activationState;
	public string activationStateMachineName;
	public int baseMaxStock;
	public float baseRechargeInterval;
	public bool beginSkillCooldownOnSkillEnd;
	public bool canceledFromSprinting;
	public bool forceSprintDuringState;
	public bool fullRestockOnAssign;
	public InterruptPriority interruptPriority;
	public bool resetCooldownTimerOnUse;
	public bool isCombatSkill;
	public bool mustKeyPress;
	public bool cancelSprintingOnActivation;
	public int rechargeStock;
	public int requiredStock;
	public int stockToConsume;
	public string[] keywordTokens;
}