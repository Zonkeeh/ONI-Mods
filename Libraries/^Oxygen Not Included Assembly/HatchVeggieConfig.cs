// Decompiled with JetBrains decompiler
// Type: HatchVeggieConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

[EntityConfigOrder(1)]
public class HatchVeggieConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 140f;
  private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchVeggieConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = 50f;
  public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 1;
  public const string ID = "HatchVeggie";
  public const string BASE_TRAIT_ID = "HatchVeggieBaseTrait";
  public const string EGG_ID = "HatchVeggieEgg";
  private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

  public static GameObject CreateHatch(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchVeggieBaseTrait", is_baby, "veg_"), HatchTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("HatchVeggieBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
    List<Diet.Info> diet_infos = BaseHatchConfig.VeggieDiet(SimHashes.Carbon.CreateTag(), HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, (string) null, 0.0f);
    diet_infos.AddRange((IEnumerable<Diet.Info>) BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, (string) null, 0.0f));
    return BaseHatchConfig.SetupDiet(wildCreature, diet_infos, HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, HatchVeggieConfig.MIN_POOP_SIZE_IN_KG);
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(HatchVeggieConfig.CreateHatch("HatchVeggie", (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.NAME, (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.DESC, "hatch_kanim", false), "HatchVeggieEgg", (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchVeggieBaby", 60f, 20f, HatchTuning.EGG_CHANCES_VEGGIE, HatchVeggieConfig.EGG_SORT_ORDER, true, false, true, 1f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
