// Decompiled with JetBrains decompiler
// Type: SuitLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SuitLockerConfig : IBuildingConfig
{
  public const string ID = "SuitLocker";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SuitLocker";
    int width = 1;
    int height = 3;
    string anim = "changingarea_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    }, refinedMetals, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "SuitLocker");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<SuitLocker>().OutfitTags = new Tag[1]
    {
      GameTags.AtmoSuit
    };
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.capacityKG = 200f;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("SuitLocker"),
      new Tag("SuitMarker")
    };
    go.AddOrGet<Storage>().capacityKg = 400f;
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
