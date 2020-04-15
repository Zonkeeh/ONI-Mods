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
                BUILDINGS.DECOR.BONUS.TIER1, NOISE_POLLUTION.NONE, 0.2f);
            buildingDef.Overheatable = false;
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 120f;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.IsFoundation = true;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.TileLayer = ObjectLayer.FoundationTile;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R90;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
            return buildingDef;
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
            Object.DestroyImmediate((Object)go.GetComponent<BuildingEnabledButton>());
            go.GetComponent<AccessControl>().controlEnabled = true;
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
        }
    }
}
