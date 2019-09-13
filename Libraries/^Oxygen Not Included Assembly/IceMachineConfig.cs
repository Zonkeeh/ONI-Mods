// Decompiled with JetBrains decompiler
// Type: IceMachineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class IceMachineConfig : IBuildingConfig
{
  private float energyConsumption = 60f;
  public const string ID = "IceMachine";
  private const float WATER_STORAGE = 30f;
  private const float ICE_STORAGE = 150f;
  private const float WATER_INPUT_RATE = 0.5f;
  private const float ICE_OUTPUT_RATE = 0.5f;
  private const float ICE_PER_LOAD = 30f;
  private const float TARGET_ICE_TEMP = 253.15f;
  private const float KDTU_TRANSFER_RATE = 20f;
  private const float THERMAL_CONSERVATION = 0.8f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "IceMachine";
    int width = 2;
    int height = 3;
    string anim = "freezerator_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = this.energyConsumption;
    buildingDef.ExhaustKilowattsWhenActive = 4f;
    buildingDef.SelfHeatKilowattsWhenActive = 12f;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Temperature.ID;
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    storage.showInUI = true;
    storage.capacityKg = 30f;
    Storage iceStorage = go.AddComponent<Storage>();
    iceStorage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    iceStorage.showInUI = true;
    iceStorage.capacityKg = 150f;
    iceStorage.allowItemRemoval = true;
    iceStorage.ignoreSourcePriority = true;
    iceStorage.allowUIItemRemoval = true;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    IceMachine iceMachine = go.AddOrGet<IceMachine>();
    iceMachine.SetStorages(storage, iceStorage);
    iceMachine.targetTemperature = 253.15f;
    iceMachine.heatRemovalRate = 20f;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = GameTags.Water;
    manualDeliveryKg.capacity = 30f;
    manualDeliveryKg.refillMass = 6f;
    manualDeliveryKg.minimumMass = 10f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
