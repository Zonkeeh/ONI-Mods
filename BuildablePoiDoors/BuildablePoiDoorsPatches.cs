#define UsesDLC
using Harmony;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace BuildablePoiDoors
{
    public class BuildablePOIDoorsPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Buildable POI Doors", "1.0.5");
                LogManager.LogInit();
            }
        }

        public static void RegisterBuildings()
        {
            BuildingUtils.AddToPlanning("Base", BuildablePOIFacilityDoorConfig.ID, "ManualPressureDoor");
            BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOIFacilityDoorConfig.ID);

            BuildingUtils.AddToPlanning("Base", BuildablePOISecurityDoorConfig.ID, "PressureDoor");
            BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOISecurityDoorConfig.ID);

            BuildingUtils.AddToPlanning("Base", BuildablePOIInternalDoorConfig.ID, "Door");
            BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOIInternalDoorConfig.ID);

            LocString.CreateLocStringKeys(typeof(BuildablePOIDoorsStrings.BUILDINGS));
        }

#if UsesDLC
        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class Db_Initialise_Patch
        {
            public static void Postfix() => BuildablePOIDoorsPatches.RegisterBuildings();
        }
#else
        [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix() => BuildablePOIDoorsPatches.RegisterBuildings();
        }
#endif
    }
}
