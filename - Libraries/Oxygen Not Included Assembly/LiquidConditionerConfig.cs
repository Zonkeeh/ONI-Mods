// Decompiled with JetBrains decompiler
// Type: LiquidConditionerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LiquidConditionerConfig : IBuildingConfig
{
  private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };
  public const string ID = "LiquidConditioner";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidConditioner";
    int width = 2;
    int height = 2;
    string anim = "liquidconditioner_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    float[] tieR6 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER6;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR6, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.EnergyConsumptionWhenActive = 1200f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.OverheatTemperature = 398.15f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    AirConditioner airConditioner = go.AddOrGet<AirConditioner>();
    airConditioner.temperatureDelta = -14f;
    airConditioner.maxEnvironmentDelta = -50f;
    airConditioner.isLiquidConditioner = true;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 10f;
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
    defaultStorage.showInUI = true;
    defaultStorage.capacityKg = 2f * conduitConsumer.consumptionRate;
    defaultStorage.SetDefaultStoredItemModifiers(LiquidConditionerConfig.StoredItemModifiers);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
