using TUNING;
using UnityEngine;
using Zolibrary.Logging;

namespace ShineBugLights
{
    public class ShineLightConfig : IBuildingConfig
    {
        public const string ID = "ShineBugLight";
        public const string DisplayName = "Shine Bug Light";
        public const string Description = "ShineBugLight";
        public const string Effect = "ShineBugLight";

        public override BuildingDef CreateBuildingDef()
        {
            int width = 1;
            int height = 1;
            string anim = "shine_light_kanim";
            int hitpoints = 10;
            float construction_time = 10f; 
            float[] cons_mass = new float[] { 50f, 1f };
            string[] cons_mat = new string[] { "Metal", "Glass" };
            float melting_point = 800f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFoundationRotatable;
            EffectorValues none = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, cons_mass, cons_mat, melting_point, build_location_rule, new EffectorValues() { amount = 2, radius = 4 }, none, 0.2f);
            buildingDef.ViewMode = OverlayModes.Light.ID;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.AudioCategory = "Metal";
            return buildingDef;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
            lightShapePreview.lux = 1800;
            lightShapePreview.radius = 8f;
            lightShapePreview.shape = LightShape.Circle;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LoopingSounds>();
            Light2D light2D = go.AddOrGet<Light2D>();
            light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
            light2D.Color = LIGHT2D.CEILINGLIGHT_COLOR;
            light2D.Range = 8f;
            light2D.Angle = 2.6f;
            light2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
            light2D.Offset = LIGHT2D.CEILINGLIGHT_OFFSET;
            light2D.shape = LightShape.Circle;
            light2D.drawOverlay = true;
            light2D.Lux = 1800;
            go.AddOrGetDef<LightController.Def>();
        }
    }
}
