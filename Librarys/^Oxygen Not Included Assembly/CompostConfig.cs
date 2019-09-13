// Decompiled with JetBrains decompiler
// Type: CompostConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CompostConfig : IBuildingConfig
{
  public static readonly Tag COMPOST_TAG = GameTags.Compostable;
  public const string ID = "Compost";
  public const float SAND_INPUT_PER_SECOND = 0.1f;
  public const float FERTILIZER_OUTPUT_PER_SECOND = 0.1f;
  public const float FERTILIZER_OUTPUT_TEMP = 348.15f;
  public const float INPUT_CAPACITY = 300f;
  private const SimHashes OUTPUT_ELEMENT = SimHashes.Dirt;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Compost";
    int width = 2;
    int height = 2;
    string anim = "compost_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, none, 0.2f);
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    SoundEventVolumeCache.instance.AddVolume("anim_interacts_compost_kanim", "Compost_shovel_in", NOISE_POLLUTION.NOISY.TIER2);
    SoundEventVolumeCache.instance.AddVolume("anim_interacts_compost_kanim", "Compost_shovel_out", NOISE_POLLUTION.NOISY.TIER2);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 2000f;
    go.AddOrGet<Compost>().simulatedInternalTemperature = 348.15f;
    CompostWorkable compostWorkable = go.AddOrGet<CompostWorkable>();
    compostWorkable.workTime = 20f;
    compostWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_compost_kanim")
    };
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(CompostConfig.COMPOST_TAG, 0.1f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.1f, SimHashes.Dirt, 348.15f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
    };
    ElementDropper elementDropper = go.AddComponent<ElementDropper>();
    elementDropper.emitMass = 10f;
    elementDropper.emitTag = SimHashes.Dirt.CreateTag();
    elementDropper.emitOffset = new Vector3(0.5f, 1f, 0.0f);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = CompostConfig.COMPOST_TAG;
    manualDeliveryKg.capacity = 300f;
    manualDeliveryKg.refillMass = 60f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
    Prioritizable.AddRef(go);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
