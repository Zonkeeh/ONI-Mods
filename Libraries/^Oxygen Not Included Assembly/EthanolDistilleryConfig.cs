// Decompiled with JetBrains decompiler
// Type: EthanolDistilleryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class EthanolDistilleryConfig : IBuildingConfig
{
  public const string ID = "EthanolDistillery";
  public const float ORGANICS_CONSUME_PER_SECOND = 1f;
  public const float ORGANICS_STORAGE_AMOUNT = 600f;
  public const float ETHANOL_RATE = 0.5f;
  public const float SOLID_WASTE_RATE = 0.3333333f;
  public const float CO2_WASTE_RATE = 0.1666667f;
  public const float OUTPUT_TEMPERATURE = 346.5f;
  public const float WASTE_OUTPUT_TEMPERATURE = 366.5f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "EthanolDistillery";
    int width = 4;
    int height = 3;
    string anim = "ethanoldistillery_kanim";
    int hitpoints = 100;
    float construction_time = 30f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR5, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.PowerInputOffset = new CellOffset(2, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(-1, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.Ethanol
    };
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1000f;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = WoodLogConfig.TAG;
    manualDeliveryKg.capacity = 600f;
    manualDeliveryKg.refillMass = 150f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(WoodLogConfig.TAG, 1f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[3]
    {
      new ElementConverter.OutputElement(0.5f, SimHashes.Ethanol, 346.5f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.3333333f, SimHashes.ToxicSand, 366.5f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.1666667f, SimHashes.CarbonDioxide, 366.5f, false, false, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
    };
    AlgaeDistillery algaeDistillery = go.AddOrGet<AlgaeDistillery>();
    algaeDistillery.emitMass = 20f;
    algaeDistillery.emitTag = new Tag("ToxicSand");
    algaeDistillery.emitOffset = new Vector3(2f, 1f);
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
