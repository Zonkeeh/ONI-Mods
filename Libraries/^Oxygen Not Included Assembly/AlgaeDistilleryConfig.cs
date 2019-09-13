// Decompiled with JetBrains decompiler
// Type: AlgaeDistilleryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class AlgaeDistilleryConfig : IBuildingConfig
{
  public const string ID = "AlgaeDistillery";
  public const float INPUT_SLIME_PER_SECOND = 0.6f;
  public const float ALGAE_PER_SECOND = 0.2f;
  public const float DIRTY_WATER_PER_SECOND = 0.4f;
  public const float OUTPUT_TEMP = 303.15f;
  public const float REFILL_RATE = 2400f;
  public const float ALGAE_STORAGE_AMOUNT = 480f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "AlgaeDistillery";
    int width = 3;
    int height = 4;
    string anim = "algae_distillery_kanim";
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
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    AlgaeDistillery algaeDistillery = go.AddOrGet<AlgaeDistillery>();
    algaeDistillery.emitTag = new Tag("Algae");
    algaeDistillery.emitMass = 30f;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.DirtyWater
    };
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1000f;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.showInUI = true;
    Tag tag = SimHashes.SlimeMold.CreateTag();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = tag;
    manualDeliveryKg.refillMass = 120f;
    manualDeliveryKg.capacity = 480f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(tag, 0.6f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(0.2f, SimHashes.Algae, 303.15f, false, true, 0.0f, 1f, 1f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.4f, SimHashes.DirtyWater, 303.15f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
    };
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
