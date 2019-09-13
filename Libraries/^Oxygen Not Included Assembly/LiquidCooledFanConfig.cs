// Decompiled with JetBrains decompiler
// Type: LiquidCooledFanConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidCooledFanConfig : IBuildingConfig
{
  public const string ID = "LiquidCooledFan";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidCooledFan";
    int width = 2;
    int height = 2;
    string anim = "fanliquid_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR2_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR2_2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2_1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2_2, 0.2f);
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Temperature.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.Deprecated = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage1 = go.AddComponent<Storage>();
    Storage storage2 = go.AddComponent<Storage>();
    storage2.capacityKg = 100f;
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    float num1 = 2426.72f;
    float num2 = 0.01f;
    LiquidCooledFan liquidCooledFan = go.AddOrGet<LiquidCooledFan>();
    liquidCooledFan.gasStorage = storage1;
    liquidCooledFan.liquidStorage = storage2;
    liquidCooledFan.waterKGConsumedPerKJ = (float) (1.0 / ((double) num1 * (double) num2));
    liquidCooledFan.coolingKilowatts = 80f;
    liquidCooledFan.minCooledTemperature = 290f;
    liquidCooledFan.minEnvironmentMass = 0.25f;
    liquidCooledFan.minCoolingRange = new Vector2I(-2, 0);
    liquidCooledFan.maxCoolingRange = new Vector2I(2, 4);
    ManualDeliveryKG manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg.requestedItemTag = new Tag("Water");
    manualDeliveryKg.capacity = 500f;
    manualDeliveryKg.refillMass = 50f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
    elementConsumer.storeOnConsume = true;
    elementConsumer.storage = storage1;
    elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer.consumptionRadius = (byte) 8;
    elementConsumer.EnableConsumption(true);
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f);
    elementConsumer.showDescriptor = false;
    LiquidCooledFanWorkable cooledFanWorkable = go.AddOrGet<LiquidCooledFanWorkable>();
    cooledFanWorkable.SetWorkTime(20f);
    cooledFanWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_liquidfan_kanim")
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
