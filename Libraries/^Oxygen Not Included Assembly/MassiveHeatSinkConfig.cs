// Decompiled with JetBrains decompiler
// Type: MassiveHeatSinkConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MassiveHeatSinkConfig : IBuildingConfig
{
  public const string ID = "MassiveHeatSink";
  private const float CONSUMPTION_RATE = 0.01f;
  private const float STORAGE_CAPACITY = 0.09999999f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "MassiveHeatSink";
    int width = 4;
    int height = 4;
    string anim = "massiveheatsink_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    float[] tieR5_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 2400f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5_1, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, tieR5_2, 0.2f);
    buildingDef.ExhaustKilowattsWhenActive = -16f;
    buildingDef.SelfHeatKilowattsWhenActive = -64f;
    buildingDef.Floodable = true;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.InputConduitType = ConduitType.Gas;
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<MassiveHeatSink>();
    go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 100f;
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Iron);
    component.Temperature = 294.15f;
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<Storage>().capacityKg = 0.09999999f;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Hydrogen);
    conduitConsumer.capacityKG = 0.09999999f;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    go.AddOrGet<ElementConverter>().consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag, 0.01f)
    };
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
