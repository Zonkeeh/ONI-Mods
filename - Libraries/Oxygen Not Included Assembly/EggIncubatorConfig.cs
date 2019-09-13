// Decompiled with JetBrains decompiler
// Type: EggIncubatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class EggIncubatorConfig : IBuildingConfig
{
  public static readonly List<Storage.StoredItemModifier> IncubatorStorage = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Preserve
  };
  public const string ID = "EggIncubator";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "EggIncubator";
    int width = 2;
    int height = 3;
    string anim = "incubator_kanim";
    int hitpoints = 30;
    float construction_time = 120f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.OverheatTemperature = 363.15f;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(EggIncubatorConfig.IncubatorStorage);
    EggIncubator eggIncubator = go.AddOrGet<EggIncubator>();
    eggIncubator.AddDepositTag(GameTags.Egg);
    eggIncubator.SetWorkTime(5f);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
