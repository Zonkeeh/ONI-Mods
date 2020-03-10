using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace BuildablePoiDoors
{
    public class BuildablePOIInternalDoorConfig : IBuildingConfig
    {
        public static string ID = "BuildablePOIInternalDoor";
        public static string DisplayName = "Futuristic Door";
        public static string Description = "A door from the future! (or the past)";
        public static string Effect = "Buildable form of the POI internal door.";

        public override BuildingDef CreateBuildingDef()
        {
            float[] construct_mass = { 200f };
            string[] construct_mat = { "RefinedMetal"};

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 2, "door_poi_internal_kanim", 
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
            buildingDef.SceneLayer = Grid.SceneLayer.Building;
            buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Door door = go.AddOrGet<Door>();
            door.unpoweredAnimSpeed = 1.2f;
            door.doorType = Door.DoorType.Internal;
            door.doorOpeningSoundEventName = "Open_DoorInternal";
            door.doorClosingSoundEventName = "Close_DoorInternal";
            go.AddOrGet<AccessControl>().controlEnabled = true;
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 3f;
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            Object.DestroyImmediate((Object)go.GetComponent<BuildingEnabledButton>());
        }
    }
}
