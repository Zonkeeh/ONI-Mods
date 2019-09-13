// Decompiled with JetBrains decompiler
// Type: MineralDeoxidizerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MineralDeoxidizerConfig : IBuildingConfig
{
  public const string ID = "MineralDeoxidizer";
  private const float ALGAE_BURN_RATE = 0.55f;
  private const float ALGAE_STORAGE = 330f;
  private const float OXYGEN_GENERATION_RATE = 0.5f;
  private const float OXYGEN_TEMPERATURE = 303.15f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "MineralDeoxidizer";
    int width = 1;
    int height = 2;
    string anim = "mineraldeoxidizer_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3_1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR3_2, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.Breakable = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
    electrolyzer.maxMass = 1.8f;
    electrolyzer.hasMeter = false;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 330f;
    storage.showInUI = true;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(new Tag("Algae"), 0.55f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.5f, SimHashes.Oxygen, 303.15f, false, false, 0.0f, 1f, 1f, byte.MaxValue, 0)
    };
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = new Tag("Algae");
    manualDeliveryKg.capacity = 330f;
    manualDeliveryKg.refillMass = 132f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
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
