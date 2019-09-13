using System;
using TUNING;
using UnityEngine;

namespace RibbedFilePole
{
    public class RibbedFilePoleConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            string id = "RibbedFirePole";
            int width = 1;
            int height = 1;
            string anim = "ribbed_firepole_kanim";
            int hitpoints = 100;
            float construction_time = 15f;
            float[] cons_mass = new float[] { 125f, 25f};
            string[] cons_mat = new string[] { "Steel", "Plastic" };
            float melting_point = 2400f;
            BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
            EffectorValues none = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, cons_mass, cons_mat, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);

            BuildingTemplates.CreateLadderDef(buildingDef);
            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.Entombable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "small";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.DragBuild = true;
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            Ladder ladder = go.AddOrGet<Ladder>();
            ladder.isPole = true;
            ladder.upwardsMovementSpeedMultiplier = 1.2f;
            ladder.downwardsMovementSpeedMultiplier = 4.5f;
            go.AddOrGet<AnimTileable>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            BuildingTemplates.DoPostConfigure(go);
        }

        public const string Id = "RibbedFirePole";

        public const string DisplayName = "Ribbed Fire Pole";

        public const string Description = "A slippery file pole with drilled climing notches.";

        public const string Effect = "All the perks in one. Fast climbing and sliding.";
    }
}