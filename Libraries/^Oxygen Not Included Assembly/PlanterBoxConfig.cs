// Decompiled with JetBrains decompiler
// Type: PlanterBoxConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class PlanterBoxConfig : IBuildingConfig
{
  public const string ID = "PlanterBox";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "PlanterBox";
    int width = 1;
    int height = 1;
    string anim = "planterbox_kanim";
    int hitpoints = 10;
    float construction_time = 3f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] farmable = MATERIALS.FARMABLE;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage = go.AddOrGet<Storage>();
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.AddDepositTag(GameTags.CropSeed);
    plantablePlot.SetFertilizationFlags(true, false);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
    BuildingTemplates.CreateDefaultStorage(go, false);
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<PlanterBox>();
    go.AddOrGet<AnimTileable>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
