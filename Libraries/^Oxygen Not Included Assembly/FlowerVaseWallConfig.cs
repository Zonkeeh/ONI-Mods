// Decompiled with JetBrains decompiler
// Type: FlowerVaseWallConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class FlowerVaseWallConfig : IBuildingConfig
{
  public const string ID = "FlowerVaseWall";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "FlowerVaseWall";
    int width = 1;
    int height = 1;
    string anim = "flowervase_wall_kanim";
    int hitpoints = 10;
    float construction_time = 10f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnWall;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Decor.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Storage>();
    Prioritizable.AddRef(go);
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.AddDepositTag(GameTags.DecorSeed);
    plantablePlot.occupyingObjectVisualOffset = new Vector3(0.0f, -0.25f, 0.0f);
    go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
