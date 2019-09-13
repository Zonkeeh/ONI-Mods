// Decompiled with JetBrains decompiler
// Type: BottleEmptierConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BottleEmptierConfig : IBuildingConfig
{
  public const string ID = "BottleEmptier";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "BottleEmptier";
    int width = 1;
    int height = 3;
    string anim = "liquidator_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.storageFilters = STORAGEFILTERS.LIQUIDS;
    storage.showInUI = true;
    storage.showDescriptor = true;
    storage.capacityKg = 200f;
    go.AddOrGet<TreeFilterable>();
    go.AddOrGet<BottleEmptier>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
