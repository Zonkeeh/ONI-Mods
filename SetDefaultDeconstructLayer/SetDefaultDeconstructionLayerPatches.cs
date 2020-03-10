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
        private static string selectedLayer = "ALL";
        public static Config config;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Set Default Deconstruction Layer", "1.0.2");
                LogManager.LogInit();

                ConfigManager cm = new ConfigManager();
                SetDefaultDeconstructionLayerPatches.config = cm.LoadConfig<Config>(new Config());

                SetDefaultDeconstructionLayerPatches.selectedLayer = 
                    SetDefaultDeconstructionLayerPatches.GetWidgetString(config.SelectedLayer);
            }
        }

        [HarmonyPatch(typeof(ToolParameterMenu), "PopulateMenu")]
        public static class ToolParameterMenu_PopulateMenu_Patch
        {
            public static void Postfix(ref ToolParameterMenu __instance, ref Dictionary<string, GameObject> ___widgets)
            {
                InterfaceTool at = PlayerController.Instance.ActiveTool;

                if (SetDefaultDeconstructionLayerPatches.config.DefaultEveryTime)
                    SetDefaultDeconstructionLayerPatches.hasStarted = false;

                if (at.name.Equals("DeconstructTool") && !hasStarted
                    && !SetDefaultDeconstructionLayerPatches.selectedLayer.Equals("ALL"))
                {
                    Traverse.Create(__instance).Method("ChangeToSetting", new[] { SetDefaultDeconstructionLayerPatches.selectedLayer }).GetValue();
                    Traverse.Create(__instance).Method("OnChange").GetValue();
                    SetDefaultDeconstructionLayerPatches.hasStarted = true;
                }
            }
        }

        [HarmonyPatch(typeof(PauseScreen), "OnQuit")]
        public static class PauseScreen_OnQuit_Patch
        {
            public static void Postfix() => SetDefaultDeconstructionLayerPatches.hasStarted = false;
        }

        private static string GetWidgetString(string config_string)
        {
            switch (config_string)
            {
                case "All": return "ALL";
                case "Power Wires": return "WIRES";
                case "Liquid Pipes": return "LIQUIDPIPES";
                case "Gas Pipes": return "GASPIPES";
                case "Conveyor Rails": return "SOLIDCONDUITS";
                case "Buildings": return "BUILDINGS";
                case "Automation": return "LOGIC";
                case "Background Buildings": return "BACKWALL";
                default: LogManager.LogWarning("The selected layer in config is invalid:" + config_string);
                    return "ALL";
            }
        }
    }
}
