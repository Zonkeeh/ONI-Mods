using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace BuildablePoiDoors
{
    public class BuildablePOISecurityDoorConfig : IBuildingConfig
    {
        public static string ID = "BuildablePOISecurityDoor";
        public static string DisplayName = "Security Door";
        public static string Description = "A secure door?!?";
        public static string Effect = "Buildable form of the POI Security door.";

        public override BuildingDef CreateBuildingDef()
        {
            float[] construct_mass = { 400f };
            string[] construct_mat = { "RefinedMetal"};

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 2, "door_poi_kanim", 
                100, 60f, construct_mass, construct_mat, 1800f, BuildLocationRule.Anywhere, 
                BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NONE, 0.2f);
            buildingDef.Overheatable = false;
            buildingDef.Repairable = false;
            buildingDef.Floodable = false;
            buildingDef.Invincible = false;
            buildingDef.IsFoundation = true;
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 120f;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.TileLayer = ObjectLayer.FoundationTile;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R90;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_gear_LP", NOISE_POLLUTION.NOISY.TIER1);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_open", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_close", NOISE_POLLUTION.NOISY.TIER2);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.doorType = Door.DoorType.Pressure;
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            go.AddOrGet<Workable>().workTime = 5f;
            go.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.unpoweredAnimSpeed = 1f;
            door.poweredAnimSpeed = 6f;
            door.doorClosingSoundEventName = "MechanizedAirlock_closing";
            door.doorOpeningSoundEventName = "MechanizedAirlock_opening";
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 5f;
            GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
            Object.DestroyImmediate((Object)go.GetComponent<BuildingEnabledButton>());
            go.GetComponent<AccessControl>().controlEnabled = true;
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
        }
    }
}
