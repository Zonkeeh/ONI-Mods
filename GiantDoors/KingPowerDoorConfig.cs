using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace GiantDoors
{
    public class KingPowerDoorConfig : IBuildingConfig
    {
        public static string ID = "KingPowerDoor";
        public static string DisplayName = "King Sized Airlock";
        public static string Description = "A powered airlock fit for keeping in king flactulance.";
        public static string Effect = "Airlock used to provide a better solution for high rooms/room seperation.";

        public override BuildingDef CreateBuildingDef()
        {
            string id = ID;
            int width = 1;
            int height = 4;
            string anim = "door_king_powered_kanim";
            int hitpoints = 200;
            float construction_time = 90f;
            float[] construct_mass = { 400f };
            string[] construct_mat = TUNING.MATERIALS.REFINED_METALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Tile;
            EffectorValues none = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construct_mass, construct_mat, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 1f);
            buildingDef.Overheatable = false;
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 240f;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.IsFoundation = true;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.TileLayer = ObjectLayer.FoundationTile;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R90;
            buildingDef.SceneLayer = Grid.SceneLayer.Building;
            buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            buildingDef.TileLayer = ObjectLayer.FoundationTile;
            SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.unpoweredAnimSpeed = ConfigChecker.PoweredSpeedUnpowered;
            door.poweredAnimSpeed = ConfigChecker.PoweredSpeed;
            door.doorOpeningSoundEventName = "BunkerDoor_opening";
            door.doorClosingSoundEventName = "BunkerDoor_closing";
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 20f;
            Object.DestroyImmediate((Object)go.GetComponent<BuildingEnabledButton>());
            go.GetComponent<AccessControl>().controlEnabled = true;
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
            go.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;
        }
    }
}
