// Decompiled with JetBrains decompiler
// Type: DesalinatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class DesalinatorConfig : IBuildingConfig
{
  public const string ID = "Desalinator";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
  private const float INPUT_RATE = 5f;
  private const float SALT_WATER_TO_SALT_OUTPUT_RATE = 0.35f;
  private const float SALT_WATER_TO_CLEAN_WATER_OUTPUT_RATE = 4.65f;
  private const float BRINE_TO_SALT_OUTPUT_RATE = 1.5f;
  private const float BRINE_TO_CLEAN_WATER_OUTPUT_RATE = 3.5f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Desalinator";
    int width = 4;
    int height = 3;
    string anim = "desalinator_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tieR1, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.showInUI = true;
    go.AddOrGet<Desalinator>().maxSalt = 945f;
    ElementConverter elementConverter1 = go.AddComponent<ElementConverter>();
    elementConverter1.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(new Tag("SaltWater"), 5f)
    };
    elementConverter1.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(4.65f, SimHashes.Water, 0.0f, false, true, 0.0f, 0.5f, 0.75f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.35f, SimHashes.Salt, 0.0f, false, true, 0.0f, 0.5f, 0.25f, byte.MaxValue, 0)
    };
    ElementConverter elementConverter2 = go.AddComponent<ElementConverter>();
    elementConverter2.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(new Tag("Brine"), 5f)
    };
    elementConverter2.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(3.5f, SimHashes.Water, 313.15f, false, true, 0.0f, 0.5f, 0.75f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(1.5f, SimHashes.Salt, 313.15f, false, true, 0.0f, 0.5f, 0.25f, byte.MaxValue, 0)
    };
    DesalinatorWorkableEmpty desalinatorWorkableEmpty = go.AddOrGet<DesalinatorWorkableEmpty>();
    desalinatorWorkableEmpty.workTime = 90f;
    desalinatorWorkableEmpty.workLayer = Grid.SceneLayer.BuildingFront;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityKG = 20f;
    conduitConsumer.capacityTag = GameTags.AnyWater;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.invertElementFilter = true;
    conduitDispenser.elementFilter = new SimHashes[2]
    {
      SimHashes.SaltWater,
      SimHashes.Brine
    };
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
  }
}
