using V1Mod.SkillStates;
using V1Mod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace V1Mod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(ReflectPunch));

            entityStates.Add(typeof(Coin));

            entityStates.Add(typeof(Slide2));

            entityStates.Add(typeof(Tesla));
        }
    }
}