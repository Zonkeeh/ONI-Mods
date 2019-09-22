using System;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;

namespace RibbedFirePole
{
    public class RibbedFirePoleConfig : IBuildingConfig
    {
        public static string ID = "RibbedFirePole";
        public static string DisplayName = "Ribbed Fire Pole";
        public static string Description = "A slippery fire pole with drilled climing notches.";
        public static string Effect = "All the perks in one. Fast climbing and sliding.";


        public override BuildingDef CreateBuildingDef()
        {
            string id = ID;
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

            float up_speed = 1.2f;
            float down_speed = 4.5f;

            float configUp = RibbedFirePolePatches.config.ClimbSpeed;
            float configDown = RibbedFirePolePatches.config.FallSpeed;

            if (float.IsNaN(configUp) || configUp <= 0f || configUp > 10f)
                LogManager.LogException("ClimbSpeed in config is not valid (not a float, or is < 0 or > 10)", new ArgumentException("ClimbingSpeed: " + configUp));
            else
                up_speed = configUp;

            if (float.IsNaN(configDown) || configDown <= 0f || configDown > 10f)
                LogManager.LogException("Falling in config is not valid (not a float, or is < 0 or > 10)", new ArgumentException("FallingSpeed: " + configDown));
            else
                down_speed = configDown;

            ladder.isPole = true;
            ladder.upwardsMovementSpeedMultiplier = up_speed;
            ladder.downwardsMovementSpeedMultiplier = down_speed;
            go.AddOrGet<AnimTileable>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            BuildingTemplates.DoPostConfigure(go);
        }
    }
}