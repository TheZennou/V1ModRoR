using R2API;
using System;

namespace V1Mod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            string str = "ZARKO_V1_BODY_";
            string text = "V1 is a high risk, high reward super mobile ranged survivor.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            text = text + "< ! > The Pistol is a fast and forgiving weapon, while the Revolver is more powerful, but more punishing.." + Environment.NewLine + Environment.NewLine;
            text = text + "< ! > Use Coin Toss to deal massive damage to the nearest enemy, kills reduce it's cooldown." + Environment.NewLine + Environment.NewLine;
            text = text + "< ! > Slide along the ground for massive speed." + Environment.NewLine + Environment.NewLine;
            text = text + "< ! > Tesla Railcannon can be used to chunk large foes, or pierce enemies for crowd control." + Environment.NewLine + Environment.NewLine;
            string text2 = "..and so it left, destined for a much greater challenge.";
            string text3 = "..and so it vanished, becoming inanimate once more";
            LanguageAPI.Add(str + "NAME", "V1");
            LanguageAPI.Add(str + "DESCRIPTION", text);
            LanguageAPI.Add(str + "SUBTITLE", "Bloodstained War Machine");
            LanguageAPI.Add(str + "LORE", "The employee went about his routine, checking the stock of the UES Safe Travels. That's when he noticed it, a broken down combat robot hidden behind some boxes, it was clearly labeled 'V1' on it's chest.\n\nNo Tracking Number, no Shipping Details, this Thing wasn’t UES.\n\nThe crew knew about the planet's status, it’s anomalous effets, it’s elusivity on the Starmaps, this Thing could be dangerous. They threw it out of the airlock, and as it barreled towards the planet’s surface, nothing. No subroutines, No Backup Power,  No Self-Preservation.\n\nIt hit the planet's surface, although not with much force, the planet's low gravity combined with the robot’s low weight made sure of that. It lay there in the grass, unmoving.\n\nA few days later a rescue ship descended down unto the grassy plains, a squadron of commandos were met with the full brunt of the planet's fury. Blood everywhere, from Human and Beast alike. \n\nBlood seeped into the Robot’s core, it started to stir to life.\n\nSTATUS UPDATE:\n\nMACHINE ID:                        V1\nLOCATION:                           UNKNOWN\nCURRENT OBJECTIVE:           ESCAPE\n\n<color=#dc0000>MANKIND IS DEAD.\nBLOOD IS FUEL.\nHELL IS FULL.</color>");
            LanguageAPI.Add(str + "OUTRO_FLAVOR", text2);
            LanguageAPI.Add(str + "OUTRO_FAILURE", text3);

            #region V1
            string prefix = V1Plugin.developerPrefix + "_V1_BODY_";

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "V2");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_OVERCLOCK_NAME", "Overclock");
            LanguageAPI.Add(prefix + "PASSIVE_OVERCLOCK_DESCRIPTION", "Allows V1 to carry items while still maintaining <style=cIsUtility>top speed</style>.");
            LanguageAPI.Add(prefix + "PASSIVE_BLOODISFUEL_NAME", "BLOOD IS FUEL");
            LanguageAPI.Add(prefix + "PASSIVE_BLOODISFUEL_DESCRIPTION", "V1 may no longer carry items or equipment, gain increased <style=cIsHealing>blood healing</style> and <style=cIsDamage>damage</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_NAME", "Pistol");
            LanguageAPI.Add(prefix + "PRIMARY_PISTOL_DESCRIPTION", Helpers.agilePrefix + $"Fire a pistol for <style=cIsDamage>{100f * StaticValues.pistolDamageCoefficient}% damage</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_REVOLVER_NAME", "Revolver");
            LanguageAPI.Add(prefix + "PRIMARY_REVOLVER_DESCRIPTION", Helpers.agilePrefix + $"Fire a powerful blast from your Revolver dealing <style=cIsDamage>{100f * StaticValues.revolverDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Coin Toss");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Toss a coin into the air, then shoot it dealing <style=cIsDamage>{100f * StaticValues.flipDamageCoefficient}% damage</style> to the nearest enemy. Kills reduce it's cooldown.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_NAME", "Slide");
            LanguageAPI.Add(prefix + "UTILITY_SLIDE_DESCRIPTION", "Slide along the ground for an unlimited amount of time, <style=cIsUtility>You can fire while using this skill.</style>");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_WHIPLASH_NAME", "Whiplash");
            LanguageAPI.Add(prefix + "UTILITY_WHIPLASH_DESCRIPTION", "Fire a strong grappling hook to<style=cIsUtility> pull yourself to enemies, or pull enemies to yourself.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_TESLA_NAME", "Tesla Railcannon");
            LanguageAPI.Add(prefix + "SPECIAL_TESLA_DESCRIPTION", Helpers.agilePrefix + $"Charge up and fire the Tesla Railcannon for <style=cIsDamage>{100f * StaticValues.teslaDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "V1: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As V1, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "V1: V2");
            #endregion
            #endregion
        }
    }
}