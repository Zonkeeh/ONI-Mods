using System;
using STRINGS;
using Harmony;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;
using System.Collections.Generic;

namespace AdvancedSpaceScanner
{
    public class AdvancedSpaceScannerConfig : IBuildingConfig
    {
        public static string ID = "AdvancedSpaceScanner";
        public static string DisplayName = (string) AdvancedSpaceScannerStrings.BUILDINGS.PREFABS.ADVANCEDSPACESCANNER.NAME;
        public static string Description = (string) AdvancedSpaceScannerStrings.BUILDINGS.PREFABS.ADVANCEDSPACESCANNER.DESC;
        public static string Effect = (string) AdvancedSpaceScannerStrings.BUILDINGS.PREFABS.ADVANCEDSPACESCANNER.EFFECT;

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
            int width = 8;
            int height = 5;
            string anim = "advanced_space_scanner_kanim";
            int hitpoints = 100;
            float construction_time = 30f;
            float[] con_mass = {this.refined_mass, this.glass_mass};
            string[] con_mat = { "RefinedMetal" , "Glass"};
            float melting_point = 320000f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            EffectorValues decor = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
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
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>() {
                LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_INACTIVE, true)
            };
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGetDef<CometDetector.Def>();
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        }
    }

}

