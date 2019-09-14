using Harmony;
using System;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using System.Collections.Generic;

namespace SetDefaultDeconstructionLayer
{
    public class SetDefaultDeconstructionLayerPatches
    {
        public static bool hasStarted = false;
        public static Config config;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Set Default Deconstruction Layer", "1.0.0");
                LogManager.LogInit();

                ConfigManager cm = new ConfigManager();
                SetDefaultDeconstructionLayerPatches.config = cm.LoadConfig<Config>(new Config());
            }
        }

        [HarmonyPatch(typeof(ToolParameterMenu), "PopulateMenu")]
        public static class ToolParameterMenu_PopulateMenu_Patch
        {
            public static void Postfix(ref KeyValuePair<string, GameObject>[] ___widgets)
            {
                InterfaceTool at = PlayerController.Instance.ActiveTool;


                if (config.DefaultEveryTime)
                    hasStarted = false;

                foreach (KeyValuePair<string, GameObject> widget in ___widgets)
                    LogManager.LogWarning(widget.Key);


                    //if (at.GetProperName().ToLowerInvariant().Contains("deconstruct") && !hasStarted)



                
            }
        }
    }
}
