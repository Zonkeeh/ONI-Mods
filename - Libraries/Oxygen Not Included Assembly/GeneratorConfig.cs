// Decompiled with JetBrains decompiler
// Type: GeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GeneratorConfig : IBuildingConfig
{
  public const string ID = "Generator";
  private const float COAL_BURN_RATE = 1f;
  private const float COAL_CAPACITY = 600f;
  public const float CO2_OUTPUT_TEMPERATURE = 383.15f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Generator";
    int width = 3;
    int height = 3;
    string anim = "generatorphos_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    float[] tieR5_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 2400f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5_1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tieR5_2, 0.2f);
    buildingDef.GeneratorWattageRating = 600f;
    buildingDef.GeneratorBaseCapacity = 20000f;
    buildingDef.ExhaustKilowattsWhenActive = 8f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
    energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(SimHashes.Carbon.CreateTag(), 1f, 600f, SimHashes.CarbonDioxide, 0.02f, false, new CellOffset(1, 2), 383.15f);
    energyGenerator.meterOffset = Meter.Offset.Behind;
    energyGenerator.SetSliderValue(50f, 0);
    energyGenerator.powerDistributionOrder = 9;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 600f;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = new Tag("Coal");
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.refillMass = 100f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
    Tinkerable.MakePowerTinkerable(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
