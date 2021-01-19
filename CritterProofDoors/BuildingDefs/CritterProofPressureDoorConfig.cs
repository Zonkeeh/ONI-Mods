using System;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace CritterProofDoors
{
    public class CritterProofPressureDoorConfig : IBuildingConfig
    {
        public static string ID = "CritterProofPressureDoor";
        public static string DisplayName = "Critter Proof Mechanized Airlock";
        public static string Description = UI.FormatAsLink("Critters","CREATURES") + " are obviously scared of getting crushed.";
        public static string Effect = "Unpassable by " + UI.FormatAsLink("Critters", "CREATURES") + ", even when open.\n\nActs like a Mechanized Airlock to Duplicants, " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " & " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + ".";

        public static void Setup()
        {
            BuildingUtils.AddStrings(ID, DisplayName, Description, Effect);
            if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                return;
            else if (CritterPathingPatches.config.EnableCritterProofMechanisedAirlock)
            {
                BuildingUtils.AddToPlanning("Base", ID, "PressureDoor");
                BuildingUtils.AddToTechnology("AnimalControl", ID);
            }
        }

        public override BuildingDef CreateBuildingDef()
        {
            string anim = "critter_proof_external_kanim";

            if (!CritterPathingPatches.config.UseCustomTextures)
                anim = "door_external_kanim";

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 2,
                anim, 100, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS,
                1600f, BuildLocationRule.Tile, TUNING.BUILDINGS.DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE, 1f);
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
            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
            buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.unpoweredAnimSpeed = 0.9f;
            door.poweredAnimSpeed = 6f;
            door.doorClosingSoundEventName = "MechanizedAirlock_closing";
            door.doorOpeningSoundEventName = "MechanizedAirlock_opening";
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            CopyBuildingSettings copyBuildingSettings = go.AddOrGet<CopyBuildingSettings>();
            copyBuildingSettings.copyGroupTag = GameTags.Door;
            Workable workable = go.AddOrGet<Workable>();
            workable.workTime = 10f;
            UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
            AccessControl component = go.GetComponent<AccessControl>();
            component.controlEnabled = true;
            KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
            component2.initialAnim = "closed";
        }

    }
}
