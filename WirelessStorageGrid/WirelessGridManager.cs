// Decompiled with JetBrains decompiler
// Type: WirelessAutomation.WirelessAutomationManager
// Assembly: WirelessAutomation-merged, Version=2019.9.5.0, Culture=neutral, PublicKeyToken=null
// MVID: C4EBA218-73FA-4D36-8F30-4D91E9958487
// Assembly location: C:\Users\Isaac\Documents\Klei\OxygenNotIncluded\mods\Steam\1718226085\WirelessAutomation.dll

using KSerialization;
using Harmony;

namespace WirelessStorageGrid
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class WirelessGridManager
    {
        public static string SliderTooltipKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TOOLTIP";
        public static string SliderTooltip = "Select channel to tune in the device";
        public static string SliderTitleKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TITLE";
        public static string SliderTitle = "Channel";

        public static UtilityNetworkManager<DataCircuitNetwork, DataWire> dataCircuitSystem;
        public static DataCircuitManager dataCircuitManager;


        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public static class Game_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                WirelessGridManager.dataCircuitSystem = new UtilityNetworkManager<DataCircuitNetwork, DataWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
                WirelessGridManager.dataCircuitManager = new DataCircuitManager(WirelessGridManager.dataCircuitSystem);
            }
        }

        [HarmonyPatch(typeof(Game), "StepTheSim")]
        public static class Game_StepTheSim_Patch
        {
            public static void Postfix(ref float dt)
            {
                if ((double)dt > 0.0)
                {
                    if (WirelessGridManager.dataCircuitManager != null)
                        WirelessGridManager.dataCircuitManager.Sim200ms(dt);
                }
            }
        }

    }
}
