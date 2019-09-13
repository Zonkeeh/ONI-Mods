// Decompiled with JetBrains decompiler
// Type: LiquidReservoirConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidReservoirConfig : IBuildingConfig
{
  public const string ID = "LiquidReservoir";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
  private const int WIDTH = 2;
  private const int HEIGHT = 3;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidReservoir", 2, 3, "liquidreservoir_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.UtilityInputOffset = new CellOffset(1, 2);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Reservoir>();
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
    defaultStorage.showDescriptor = true;
    defaultStorage.allowItemRemoval = false;
    defaultStorage.storageFilters = STORAGEFILTERS.LIQUIDS;
    defaultStorage.capacityKg = 5000f;
    defaultStorage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.ignoreMinMassCheck = true;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.alwaysConsume = true;
    conduitConsumer.capacityKG = defaultStorage.capacityKg;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = (SimHashes[]) null;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<StorageController.Def>();
  }
}
