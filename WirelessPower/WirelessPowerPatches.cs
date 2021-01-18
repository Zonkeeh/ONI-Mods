#define UsesDLC
using Harmony;
using PeterHan.PLib.UI;
using UnityEngine;
using WirelessPower.BuildingDefs;
using WirelessPower.Components.Sidescreens;
using WirelessPower.Manager;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace WirelessPower
{
	public static class WirelessPowerPatches
	{
		public static Operational.Flag WirelessConnectionFlag;
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogManager.SetModInfo("WirelessPower", "1.0.2");
				LogManager.LogInit();
				WirelessPowerPatches.WirelessConnectionFlag = new Operational.Flag("wireless_grid_connection", Operational.Flag.Type.Requirement);
				WirelessPowerGrid.CreateInstance();
			}
		}

		[HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
		public static class DetailsScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				PUIUtils.AddSideScreenContent<WirelessPowerReceiverSideScreen>((GameObject)null);
				PUIUtils.AddSideScreenContent<WirelessPowerSenderSideScreen>((GameObject)null);
			}
		}

		[HarmonyPatch(typeof(Game), "OnPrefabInit")]
		public static class Game_OnPrefabInit_Patch
		{
			public static void Postfix(PauseScreen __instance) => WirelessPowerGrid.Instance.ClearGrid();
		}

		[HarmonyPatch(typeof(Game), "OnLoadLevel")]
		public static class Game_OnLoadLevel_Patch
		{
			public static void Postfix(PauseScreen __instance) => WirelessPowerGrid.Instance.ClearGrid();
		}

		public static void RegisterBuildings()
		{
			BuildingUtils.AddStrings(WirelessSenderConfig.ID, WirelessSenderConfig.Name, WirelessSenderConfig.Description, WirelessSenderConfig.Effect);
			BuildingUtils.AddToPlanning("Power", WirelessSenderConfig.ID, "BatterySmart");
			BuildingUtils.AddToTechnology("PrettyGoodConductors", WirelessSenderConfig.ID);

			BuildingUtils.AddStrings(WirelessReceiverConfig.ID, WirelessReceiverConfig.Name, WirelessReceiverConfig.Description, WirelessReceiverConfig.Effect);
			BuildingUtils.AddToPlanning("Power", WirelessReceiverConfig.ID, "BatterySmart");
			BuildingUtils.AddToTechnology("PrettyGoodConductors", WirelessReceiverConfig.ID);

			BuildingUtils.AddStrings(WirelessBatteryConfig.ID, WirelessBatteryConfig.Name, WirelessBatteryConfig.Description, WirelessBatteryConfig.Effect);
			BuildingUtils.AddToPlanning("Power", WirelessBatteryConfig.ID, "BatterySmart");
			BuildingUtils.AddToTechnology("PrettyGoodConductors", WirelessBatteryConfig.ID);
		}

#if UsesDLC
		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public class Db_Initialise_Patch
		{
			public static void Postfix() => WirelessPowerPatches.RegisterBuildings();
		}
#else
		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix() => WirelessPowerPatches.RegisterBuildings();
		}
#endif
	}
}
