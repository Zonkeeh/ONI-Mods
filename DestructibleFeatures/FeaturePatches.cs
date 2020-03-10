using Harmony;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using System;

namespace DestructibleFeatures
{
    public class FeaturePatches
    {
        public static Config config;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Destructable Features", "1.0.4");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                FeaturePatches.config = cm.LoadConfig<Config>(new Config());
            }
        }

        [HarmonyPatch(typeof(Studyable), "OnPrefabInit")]
        public static class Studyable_OnPrefabInit_Patch
        {
            public static void Postfix(Studyable __instance)
            {
                int aTime = config.AnaylsisTime;
                if (aTime <= 0 || aTime > 10000)
                    LogManager.LogException("Anaylsis time is invalid (less than 0 or greater then 10000) in the config: " + aTime, 
                        new ArgumentException("AnaylsisTime:" + aTime));
                else if(aTime != 3600)
                    __instance.SetWorkTime((float)aTime);

                DestructibleWorkable destWorkable = __instance.gameObject.AddOrGet<DestructibleWorkable>();
            }
        }

        [HarmonyPatch(typeof(Studyable), "OnCompleteWork")]
        public static class Studyable_OnCompleteWork_Patch
        {
            public static void Postfix(Studyable __instance)
            {
                if (DetailsScreen.Instance != null)
                    DetailsScreen.Instance.Show(false);

                DestructibleWorkable destWorkable = __instance.gameObject.AddOrGet<DestructibleWorkable>();

                int dTime = config.DeconstructTime;
                if (dTime <= 0 || dTime > 10000)
                    LogManager.LogException("Deconstruct time is invalid (less than 0 or greater then 10000) in the config: " + dTime,
                        new ArgumentException("DeconstructTime:" + dTime));
                else if (dTime != 1800)
                    destWorkable.SetWorkTime(dTime);

                UnityEngine.Component.Destroy(__instance);
            }
        }
    }
}
