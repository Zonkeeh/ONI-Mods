using System;
using System.Collections.Generic;
using Database;
using Harmony;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using Zolibrary.Utilities;

namespace DuplicantLifecycle
{
    public class DuplicantLifeCyclePatches
    {
        public static Config config;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("DuplicantLifeCycle", "1.0.0");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                DuplicantLifeCyclePatches.config = cm.LoadConfig<Config>(new Config());
            }
        }
        

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Trait trait = Db.Get().CreateTrait(
                    id: (string) DuplicantLifecycleStrings.ID, 
                    name: (string) DuplicantLifecycleStrings.NAME, 
                    description: (string) DuplicantLifecycleStrings.DESC, 
                    group_name: null, 
                    should_save: true, 
                    disabled_chore_groups: null, 
                    positive_trait: true, 
                    is_valid_starter_trait: false
                    );
                trait.OnAddTrait = (go => go.FindOrAddUnityComponent<Aging>());
                trait.ExtendedTooltip = (() => string.Format(DuplicantLifecycleStrings.EXTENDED_DESC, new[] { 1000f, 120f, 70f, 20f }));
            }
        }

        [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
        public class OnSpawn_Patch
        {
            public static void Postfix(GameObject __instance)
            {
                Trait agingTrait = Db.Get().traits.TryGet((string)DuplicantLifecycleStrings.ID);

                if (agingTrait == null)
                    LogManager.LogDebug("Aging trait cannot be found: " + DuplicantLifecycleStrings.ID);
                else
                    __instance.gameObject.GetComponent<Traits>().Add(agingTrait);
            }
        }

        [HarmonyPatch(typeof(CarePackageContainer),"GetSpawnableDescription")]
        public class CarePackageContainer_GetSpawnableDescription_Patch
        {
            public static void Postfix(string __result) => LogManager.LogDebug(__result);
        }

        [HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
        public static class MinionStartingStats_GenerateTraits_Patch
        {
            public static void Postfix(MinionStartingStats __instance)
            {
                Trait agingTrait = Db.Get().traits.TryGet((string) DuplicantLifecycleStrings.ID);
                if (agingTrait == null)
                    LogManager.LogDebug("Aging trait cannot be found: " + DuplicantLifecycleStrings.ID);
                else if (!__instance.Traits.Contains(agingTrait))
                    __instance.Traits.Add(agingTrait);
            }
        }
    }
}
