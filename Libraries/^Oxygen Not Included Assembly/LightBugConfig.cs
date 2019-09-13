// Decompiled with JetBrains decompiler
// Type: LightBugConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LightBugConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 0.166f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = 100;
  public const string ID = "LightBug";
  public const string BASE_TRAIT_ID = "LightBugBaseTrait";
  public const string EGG_ID = "LightBugEgg";

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBaseTrait", LIGHT2D.LIGHTBUG_COLOR, TUNING.DECOR.BONUS.TIER4, is_baby, (string) null);
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE, 25f);
    Trait trait = Db.Get().CreateTrait("LightBugBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
    return BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>()
    {
      TagManager.Create(PrickleFruitConfig.ID),
      TagManager.Create("GrilledPrickleFruit"),
      SimHashes.Phosphorite.CreateTag()
    }, Tag.Invalid, LightBugConfig.CALORIES_PER_KG_OF_ORE);
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugConfig.CreateLightBug("LightBug", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, "LightBugEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBaby", 15f, 5f, LightBugTuning.EGG_CHANCES_BASE, LightBugConfig.EGG_SORT_ORDER, true, false, true, 1f);
    lightBug.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
    return lightBug;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    BaseLightBugConfig.SetupLoopingSounds(inst);
  }
}
