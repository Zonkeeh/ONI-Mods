using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace GiantDoors
{
    public class KingDoorConfig : IBuildingConfig
    {
        public static string ID = "KingDoor";
        public static string DisplayName = "King Sized Door";
        public static string Description = "A door fit for kings, giants... or just extra tall rooms.";
        public static string Effect = "Door used to provide a better solution for high rooms/room seperation.";

        

        public override BuildingDef CreateBuildingDef()
        {
            float[] construct_mass = { 400f };
            string[] construct_mat = TUNING.MATERIALS.ALL_METALS;

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 4, "door_king_kanim",
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
            buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Door door = go.AddOrGet<Door>();
            door.unpoweredAnimSpeed = ConfigChecker.NormalSpeed;
            door.poweredAnimSpeed = ConfigChecker.NormalSpeed;
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
            Object.DestroyImmediate((Object)go.GetComponent<BuildingEnabledButton>());
        }
    }
}
