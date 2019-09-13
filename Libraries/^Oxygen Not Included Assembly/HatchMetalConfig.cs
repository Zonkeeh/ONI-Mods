// Decompiled with JetBrains decompiler
// Type: HatchMetalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

[EntityConfigOrder(1)]
public class HatchMetalConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 100f;
  private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchMetalConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = 10f;
  public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 3;
  public static readonly TagBits METAL_ORE_TAGS = new TagBits(new Tag[4]
  {
    SimHashes.Cuprite.CreateTag(),
    SimHashes.GoldAmalgam.CreateTag(),
    SimHashes.IronOre.CreateTag(),
    SimHashes.Wolframite.CreateTag()
  });
  public const string ID = "HatchMetal";
  public const string BASE_TRAIT_ID = "HatchMetalBaseTrait";
  public const string EGG_ID = "HatchMetalEgg";

  public static GameObject CreateHatch(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchMetalBaseTrait", is_baby, "mtl_"), HatchTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("HatchMetalBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 400f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
    List<Diet.Info> diet_infos = BaseHatchConfig.MetalDiet(GameTags.Metal, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, (string) null, 0.0f);
    return BaseHatchConfig.SetupDiet(wildCreature, diet_infos, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, HatchMetalConfig.MIN_POOP_SIZE_IN_KG);
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(HatchMetalConfig.CreateHatch("HatchMetal", (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.NAME, (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "hatch_kanim", false), "HatchMetalEgg", (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchMetalBaby", 60f, 20f, HatchTuning.EGG_CHANCES_METAL, HatchMetalConfig.EGG_SORT_ORDER, true, false, true, 1f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
