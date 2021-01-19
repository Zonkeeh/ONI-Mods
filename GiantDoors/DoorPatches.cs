#define UsesDLC
using Harmony;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace GiantDoors
{
    public class DoorPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("GiantDoors", "1.0.3");
                LogManager.LogInit();
            }
        }

        public static void RegisterBuildings()
        {
            BuildingUtils.AddStrings(KingDoorConfig.ID, KingDoorConfig.DisplayName, KingDoorConfig.Description, KingDoorConfig.Effect);
            BuildingUtils.AddToPlanning("Base", KingDoorConfig.ID, "Door");
            BuildingUtils.AddToTechnology("Jobs", KingDoorConfig.ID);

            BuildingUtils.AddStrings(KingPowerDoorConfig.ID, KingPowerDoorConfig.DisplayName, KingPowerDoorConfig.Description, KingPowerDoorConfig.Effect);
            BuildingUtils.AddToPlanning("Base", KingPowerDoorConfig.ID, "PressureDoor");
            BuildingUtils.AddToTechnology("RefinedObjects", KingPowerDoorConfig.ID);
        }

#if UsesDLC
        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Initialise_Patch
        {
            public static void Postfix() => DoorPatches.RegisterBuildings();
        }
#else
        [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix() => DoorPatches.RegisterBuildings();
        }
#endif
    }
}
