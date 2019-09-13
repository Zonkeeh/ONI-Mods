// Decompiled with JetBrains decompiler
// Type: BottleEmptierGasConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BottleEmptierGasConfig : IBuildingConfig
{
  public const string ID = "BottleEmptierGas";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "BottleEmptierGas";
    int width = 1;
    int height = 3;
    string anim = "gas_emptying_station_kanim";
    int hitpoints = 30;
    float construction_time = 60f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tieR1, 0.2f);
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
    storage.storageFilters = STORAGEFILTERS.GASES;
    storage.showInUI = true;
    storage.showDescriptor = true;
    storage.capacityKg = 200f;
    go.AddOrGet<TreeFilterable>();
    go.AddOrGet<BottleEmptier>().emptyRate = 0.25f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
