using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace BuildablePoiDoors
{
    public class BuildablePOIFacilityDoorConfig : IBuildingConfig
    {
        public static string ID = "BuildablePOIFacilityDoor";
        public static string DisplayName = "Facility Door";
        public static string Description = "A door fit for kings... or just some tall people...";
        public static string Effect = "Buildable form of the POI facility door.";

        public override BuildingDef CreateBuildingDef()
        {
            float[] construct_mass = { 300f, 100f };
            string[] construct_mat = { "RefinedMetal", "Plastic"};

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 2, 3, "door_facility_kanim", 
                100, 60f, construct_mass, construct_mat, 1800f, BuildLocationRule.Anywhere, 
                BUILDINGS.DECOR.BONUS.TIER1, NOISE_POLLUTION.NONE, 0.2f);
            buildingDef.Overheatable = false;
            buildingDef.Repairable = false;
            buildingDef.Floodable = false;
            buildingDef.Invincible = false;
            buildingDef.IsFoundation = true;
            buildingDef.TileLayer = ObjectLayer.FoundationTile;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R90;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_gear_LP", NOISE_POLLUTION.NOISY.TIER1);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_open", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_close", NOISE_POLLUTION.NOISY.TIER2);
            buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Door door = go.AddOrGet<Door>();
            door.unpoweredAnimSpeed = 1.2f;
            door.doorType = Door.DoorType.ManualPressure;
            door.hasComplexUserControls = true;
            door.doorOpeningSoundEventName = "Open_DoorInternal";
            door.doorClosingSoundEventName = "Close_DoorInternal";
            go.AddOrGet<AccessControl>().controlEnabled = true;
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 10f;
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
            go.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;
            go.AddOrGet<Unsealable>();
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            Object.DestroyImmediate((Object)go.GetComponent<BuildingEnabledButton>());
        }
    }
}
