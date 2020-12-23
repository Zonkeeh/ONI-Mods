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

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddToPlanning("Base", BuildablePOIFacilityDoorConfig.ID, "ManualPressureDoor");
                BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOIFacilityDoorConfig.ID);

                BuildingUtils.AddToPlanning("Base", BuildablePOISecurityDoorConfig.ID, "PressureDoor");
                BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOISecurityDoorConfig.ID);

                BuildingUtils.AddToPlanning("Base", BuildablePOIInternalDoorConfig.ID, "Door");
                BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOIInternalDoorConfig.ID);

                LocString.CreateLocStringKeys(typeof(BuildablePOIDoorsStrings.BUILDINGS));
            }
        }
    }
}
