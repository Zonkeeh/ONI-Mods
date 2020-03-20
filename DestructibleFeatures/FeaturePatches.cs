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
                LogManager.SetModInfo("Destructable Features", "1.0.5.2");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                FeaturePatches.config = cm.LoadConfig<Config>(new Config());
            }
        }

        [HarmonyPatch(typeof(Geyser), "OnCmpEnable")]
        public static class Geyser_OnCmpEnable_Patch
        {
            public static void Postfix(Geyser __instance)
            {
                if (__instance.GetType() != typeof(Geyser))
                {
                    DestructibleWorkable destWorkable = __instance.GetComponent<DestructibleWorkable>();

                    if((UnityEngine.Object)destWorkable != (UnityEngine.Object)null)
                        UnityEngine.Object.Destroy(__instance.GetComponent<DestructibleWorkable>());

                    return;
                }
                else
                {
                    DestructibleWorkable destWorkable = __instance.FindOrAddComponent<DestructibleWorkable>();

                    int dTime = config.DeconstructTime;
                    if (dTime <= 0 || dTime > 10000)
                        LogManager.LogException("Deconstruct time is invalid (less than 0 or greater then 10000) in the config: " + dTime,
                            new ArgumentException("DeconstructTime:" + dTime));
                    else if (dTime != 1800)
                        destWorkable.SetWorkTime(dTime);
                }
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
                    else if (aTime != 3600)
                    __instance.SetWorkTime(aTime);
            }
        }

        [HarmonyPatch(typeof(SingleButtonSideScreen), "SetTarget")]
        public static class SingleButtonSideScreen_SetTarget_Patch
        {
            public static void Postfix(SingleButtonSideScreen __instance, GameObject new_target, ref ISidescreenButtonControl ___target)
            {
                if ((UnityEngine.Object)new_target == (UnityEngine.Object)null)
                    Debug.LogError((object)"Invalid gameObject received");
                else
                {
                    var buttonControl = new_target.GetComponent<ISidescreenButtonControl>();

                    if (buttonControl == null || !(buttonControl is Studyable))
                        return;
                    else if (((Studyable)buttonControl).Studied)
                    {
                        DestructibleWorkable destWorkable = ((Studyable)buttonControl).gameObject.GetComponent<DestructibleWorkable>();

                        if ((UnityEngine.Object)destWorkable == (UnityEngine.Object)null)
                            return;
                        else
                        {
                            ___target = destWorkable;
                            Traverse.Create(__instance).Method("Refresh").GetValue();
                        }
                    }
                }
            }
        }
    }
}
