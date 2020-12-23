using System;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace CritterProofDoors
{
    public class CritterProofManualPressureDoorConfig : IBuildingConfig
    {
        public static string ID = "CritterProofManualPressureDoor";
        public static string DisplayName = "Critter Proof Manual Airlock";
        public static string Description = UI.FormatAsLink("Critters","CREATURES") + " are obviously scared of the noise this door makes...";
        public static string Effect = "Unpassable by " + UI.FormatAsLink("Critters", "CREATURES") + ", even when open.\n\nActs like a Manual Airlock to Duplicants, " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " & " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + ".";

        public static void Setup()
        {
            BuildingUtils.AddStrings(ID, DisplayName, Description, Effect);
            if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                return;
            else if (CritterPathingPatches.config.EnableCritterProofManualAirlock)
            {
                BuildingUtils.AddToPlanning("Base", ID, "ManualPressureDoor");
                BuildingUtils.AddToTechnology("AnimalControl", ID);
            }
        }

        public override BuildingDef CreateBuildingDef()
        {
            string anim = "critter_proof_manual_kanim";

            if (!CritterPathingPatches.config.UseCustomTextures)
                anim = "door_manual_kanim";

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 2, 
                anim, 100, 90f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 
                1600f, BuildLocationRule.Tile, TUNING.BUILDINGS.DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE, 1f);
            buildingDef.Overheatable = false;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.IsFoundation = true;
            buildingDef.TileLayer = ObjectLayer.FoundationTile;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R90;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_gear_LP", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_open", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_close", NOISE_POLLUTION.NOISY.TIER2);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.unpoweredAnimSpeed = 1f;
            door.doorType = Door.DoorType.ManualPressure;
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            CopyBuildingSettings copyBuildingSettings = go.AddOrGet<CopyBuildingSettings>();
            copyBuildingSettings.copyGroupTag = GameTags.Door;
            Workable workable = go.AddOrGet<Workable>();
            workable.workTime = 10f;
            UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            AccessControl component = go.GetComponent<AccessControl>();
            component.controlEnabled = true;
            KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
            component2.initialAnim = "closed";
        }

    }
}
