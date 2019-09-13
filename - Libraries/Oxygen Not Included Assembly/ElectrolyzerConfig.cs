// Decompiled with JetBrains decompiler
// Type: ElectrolyzerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ElectrolyzerConfig : IBuildingConfig
{
  public const string ID = "Electrolyzer";
  public const float WATER2OXYGEN_RATIO = 0.888f;
  public const float OXYGEN_TEMPERATURE = 343.15f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Electrolyzer";
    int width = 2;
    int height = 2;
    string anim = "electrolyzer_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3_1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR3_2, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
    electrolyzer.maxMass = 1.8f;
    electrolyzer.hasMeter = true;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 2f;
    storage.showInUI = true;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(new Tag("Water"), 1f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(0.888f, SimHashes.Oxygen, 343.15f, false, false, 0.0f, 1f, 1f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.112f, SimHashes.Hydrogen, 343.15f, false, false, 0.0f, 1f, 1f, byte.MaxValue, 0)
    };
    Prioritizable.AddRef(go);
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
