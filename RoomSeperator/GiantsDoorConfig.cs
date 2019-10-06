using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace GiantsDoor
{
    public class GiantsDoorConfig : IBuildingConfig
    {
        public static string ID = "GiantsDoor";
        public static string DisplayName = "King Sized Door";
        public static string Description = "A door fit for kings, giants... or just extra tall rooms.";
        public static string Effect = "Door used to provide a better solution for high rooms/room seperation.";

        

        public override BuildingDef CreateBuildingDef()
        {
            float[] construct_mass = { 400f };
            string[] construct_mat = TUNING.MATERIALS.REFINED_MINERALS;

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 4, "door_seperator_kanim",
                100, 60f, construct_mass, construct_mat, 1800f, BuildLocationRule.Anywhere,
                BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NONE, 0.2f);
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
            return buildingDef;
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
            door.unpoweredAnimSpeed = 2.2f;
            door.doorType = Door.DoorType.Internal;
            door.hasComplexUserControls = true;
            door.doorOpeningSoundEventName = "BunkerDoor_opening";
            door.doorClosingSoundEventName = "BunkerDoor_closing";
            go.AddOrGet<AccessControl>().controlEnabled = true;
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 10f;
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
            go.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;
            go.AddOrGet<Unsealable>();
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
            Object.DestroyImmediate((Object)go.GetComponent<BuildingEnabledButton>());
        }
    }
}
