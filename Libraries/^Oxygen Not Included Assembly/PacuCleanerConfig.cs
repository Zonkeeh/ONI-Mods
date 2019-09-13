// Decompiled with JetBrains decompiler
// Type: PacuCleanerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PacuCleanerConfig : IEntityConfig
{
  public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
  public const string ID = "PacuCleaner";
  public const string BASE_TRAIT_ID = "PacuCleanerBaseTrait";
  public const string EGG_ID = "PacuCleanerEgg";
  public const float POLLUTED_WATER_CONVERTED_PER_CYCLE = 120f;
  public const SimHashes INPUT_ELEMENT = SimHashes.DirtyWater;
  public const SimHashes OUTPUT_ELEMENT = SimHashes.Water;
  public const int EGG_SORT_ORDER = 501;

  public static GameObject CreatePacu(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuCleanerBaseTrait", name, desc, anim_file, is_baby, "glp_", 243.15f, 278.15f), PacuTuning.PEN_SIZE_PER_CREATURE, 25f);
    if (!is_baby)
    {
      wildCreature.AddComponent<Storage>().capacityKg = 10f;
      ElementConsumer elementConsumer = (ElementConsumer) wildCreature.AddOrGet<PassiveElementConsumer>();
      elementConsumer.elementToConsume = SimHashes.DirtyWater;
      elementConsumer.consumptionRate = 0.2f;
      elementConsumer.capacityKG = 10f;
      elementConsumer.consumptionRadius = (byte) 3;
      elementConsumer.showInStatusPanel = true;
      elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
      elementConsumer.isRequired = false;
      elementConsumer.storeOnConsume = true;
      elementConsumer.showDescriptor = false;
      wildCreature.AddOrGet<UpdateElementConsumerPosition>();
      BubbleSpawner bubbleSpawner = wildCreature.AddComponent<BubbleSpawner>();
      bubbleSpawner.element = SimHashes.Water;
      bubbleSpawner.emitMass = 2f;
      bubbleSpawner.emitVariance = 0.5f;
      bubbleSpawner.initialVelocity = (Vector2) new Vector2f(0, 1);
      ElementConverter elementConverter = wildCreature.AddOrGet<ElementConverter>();
      elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
      {
        new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.2f)
      };
      elementConverter.outputElements = new ElementConverter.OutputElement[1]
      {
        new ElementConverter.OutputElement(0.2f, SimHashes.Water, 0.0f, true, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
      };
    }
    return wildCreature;
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuCleanerConfig.CreatePacu("PacuCleaner", (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.NAME, (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, 25f), "PacuCleanerEgg", (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuCleanerBaby", 15f, 5f, PacuTuning.EGG_CHANCES_CLEANER, 501, false, true, false, 0.75f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    ElementConsumer component = inst.GetComponent<ElementConsumer>();
    if (!((Object) component != (Object) null))
      return;
    component.EnableConsumption(true);
  }
}
