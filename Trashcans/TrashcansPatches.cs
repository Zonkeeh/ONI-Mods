#define UsesDLC
using Harmony;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace Trashcans
{
    public class TrashcansPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Trashcans", "1.0.2");
                LogManager.LogInit();
            }
        }

        public static void RegisterBuildings()
        {
            BuildingUtils.AddStrings(GasTrashcanConfig.ID, GasTrashcanConfig.Name, GasTrashcanConfig.Description, GasTrashcanConfig.Effect);
            BuildingUtils.AddToPlanning("Base", GasTrashcanConfig.ID, "GasReservoir");
            BuildingUtils.AddToTechnology("SmartStorage", GasTrashcanConfig.ID);

            BuildingUtils.AddStrings(LiquidTrashcanConfig.ID, LiquidTrashcanConfig.Name, LiquidTrashcanConfig.Description, LiquidTrashcanConfig.Effect);
            BuildingUtils.AddToPlanning("Base", LiquidTrashcanConfig.ID, "LiquidReservoir");
            BuildingUtils.AddToTechnology("SmartStorage", LiquidTrashcanConfig.ID);

            BuildingUtils.AddStrings(SolidTrashcanConfig.ID, SolidTrashcanConfig.Name, SolidTrashcanConfig.Description, SolidTrashcanConfig.Effect);
            BuildingUtils.AddToPlanning("Base", SolidTrashcanConfig.ID, "StorageLockerSmart");
            BuildingUtils.AddToTechnology("SmartStorage", SolidTrashcanConfig.ID);
        }

#if UsesDLC
		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public class Db_Initialise_Patch
		{
			public static void Postfix() => TrashcansPatches.RegisterBuildings();
		}
#else
        [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix() => TrashcansPatches.RegisterBuildings();
        }
#endif
    }
}
