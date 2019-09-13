// Decompiled with JetBrains decompiler
// Type: WoodGasGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WoodGasGeneratorConfig : IBuildingConfig
{
  public const string ID = "WoodGasGenerator";
  private const float BRANCHES_PER_GENERATOR = 8f;
  public const float CONSUMPTION_RATE = 1.2f;
  private const float WOOD_PER_REFILL = 360f;
  private const SimHashes EXHAUST_ELEMENT_GAS = SimHashes.CarbonDioxide;
  private const SimHashes EXHAUST_ELEMENT_GAS2 = SimHashes.Syngas;
  public const float CO2_EXHAUST_RATE = 0.17f;
  private const int WIDTH = 2;
  private const int HEIGHT = 2;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "WoodGasGenerator";
    int width = 2;
    int height = 2;
    string anim = "generatorwood_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, allMetals, 2400f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, tieR5, 0.2f);
    buildingDef.GeneratorWattageRating = 300f;
    buildingDef.GeneratorBaseCapacity = 20000f;
    buildingDef.ExhaustKilowattsWhenActive = 8f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerOutputOffset = new CellOffset(0, 0);
    return buildingDef;
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
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<LoopingSounds>();
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.showInUI = true;
    float max_stored_input_mass = 720f;
    go.AddOrGet<LoopingSounds>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = WoodLogConfig.TAG;
    manualDeliveryKg.capacity = 360f;
    manualDeliveryKg.refillMass = 180f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
    energyGenerator.powerDistributionOrder = 8;
    energyGenerator.hasMeter = true;
    energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(WoodLogConfig.TAG, 1.2f, max_stored_input_mass, SimHashes.CarbonDioxide, 0.17f, false, new CellOffset(0, 1), 383.15f);
    Tinkerable.MakePowerTinkerable(go);
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
