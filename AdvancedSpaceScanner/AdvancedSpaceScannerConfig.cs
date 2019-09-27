using System;
using STRINGS;
using Harmony;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;

namespace AdvancedSpaceScanner
{
    public class AdvancedSpaceScannerConfig : IBuildingConfig
    {
        public static string ID = "AdvancedSpaceScanner";
        public static string DisplayName = "Advanced Space Scanner";
        public static string Description = "Networks of many scanners will scan more efficiently than one on its own.";
        public static string Effect = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to its automation circuit when it detects incoming objects from space.\n\nCan be configured to detect incoming meteor showers or returning space rockets.\n\nBunkered to protect from meteor damage.";

        private float refined_mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0];
        private float glass_mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0];


        private void CheckValidConfigVars()
        {
            if (AdvancedSpaceScannerPatches.Config.RefinedMetalCost >= 1)
                refined_mass = AdvancedSpaceScannerPatches.Config.RefinedMetalCost;
            else
                LogManager.LogException("RefinedMaterialMass was set to an invalid integer: " + AdvancedSpaceScannerPatches.Config.RefinedMetalCost, new ArgumentOutOfRangeException("RefinedConstructionMass:" + AdvancedSpaceScannerPatches.Config.RefinedMetalCost));

            if (AdvancedSpaceScannerPatches.Config.GlassCost >= 1)
                glass_mass = AdvancedSpaceScannerPatches.Config.GlassCost;
            else
                LogManager.LogException("GlassCost was set to an invalid integer: " + AdvancedSpaceScannerPatches.Config.GlassCost, new ArgumentOutOfRangeException("GlassConstructionMass:" + AdvancedSpaceScannerPatches.Config.GlassCost));
        }

        public override BuildingDef CreateBuildingDef()
        {
            CheckValidConfigVars();

            string id = AdvancedSpaceScannerConfig.ID;
            int width = 10;
            int height = 6;
            string anim = "advanced_space_scanner_kanim";
            int hitpoints = 100;
            float construction_time = 30f;
            float[] con_mass = {this.refined_mass, this.glass_mass};
            string[] con_mat = { "RefinedMetal" , "Glass"};
            float melting_point = 3200f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            EffectorValues decor = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
            EffectorValues noise = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, con_mass, con_mat, melting_point, build_location_rule, decor , noise);
            buildingDef.Overheatable = AdvancedSpaceScannerPatches.Config.IsOverheatable;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.Invincible = true;
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 240f;
            buildingDef.ViewMode = OverlayModes.Logic.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.SceneLayer = Grid.SceneLayer.Building;
            SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
            SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, AdvancedSpaceScannerConfig.ID);
            return buildingDef;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
            GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
            go.AddOrGetDef<AdvancedSpaceScanner.Def>();
        }
    }

}

