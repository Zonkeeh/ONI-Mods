using System;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace CritterProofDoors
{
    public class CritterProofDoorConfig : IBuildingConfig
    {
        public static string ID = "CritterProofDoor";
        public static string DisplayName = "Critter Proof Door";
        public static string Description = UI.FormatAsLink("Critters","CREATURES") + " must hate the sight of this...";
        public static string Effect = "Unpassable by " + UI.FormatAsLink("Critters", "CREATURES") + ", even when open.\n\nActs like a Pneumatic Door to Duplicants, " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " & " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + ". However, whilst " + UI.FormatAsLink("Critters", "CREATURES") + " won't walk through pneumatic doors on auto they won't walk through these critter proof doors, even when open.";

        public static void Setup()
        {
            BuildingUtils.AddStrings(ID, DisplayName, Description, Effect);
            if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                return;
            else if (CritterPathingPatches.config.EnablePneumaticCritterProofDoor)
            {
                BuildingUtils.AddToPlanning("Base", ID, "Door");
                BuildingUtils.AddToTechnology("Ranching", ID);
            }
        }

        public override BuildingDef CreateBuildingDef()
        {
            string anim = "critter_proof_internal_kanim";

            if (!CritterPathingPatches.config.UseCustomTextures)
                anim = "door_internal_kanim";

            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 2,
                anim, 100, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.REFINED_METALS,
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
            SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.unpoweredAnimSpeed = 1f;
            door.doorType = Door.DoorType.Internal;
            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();
            Prioritizable.AddRef(go);
            CopyBuildingSettings copyBuildingSettings = go.AddOrGet<CopyBuildingSettings>();
            copyBuildingSettings.copyGroupTag = GameTags.Door;
            Workable workable = go.AddOrGet<Workable>();
            workable.workTime = 5f;
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
