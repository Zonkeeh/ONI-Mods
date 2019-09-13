// Decompiled with JetBrains decompiler
// Type: LightBugPurpleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LightBugPurpleConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 1f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPurpleConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 2;
  public const string ID = "LightBugPurple";
  public const string BASE_TRAIT_ID = "LightBugPurpleBaseTrait";
  public const string EGG_ID = "LightBugPurpleEgg";

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPurpleBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PURPLE, TUNING.DECOR.BONUS.TIER6, is_baby, "prp_");
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE, 25f);
    Trait trait = Db.Get().CreateTrait("LightBugPurpleBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
    return BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>()
    {
      TagManager.Create("FriedMushroom"),
      TagManager.Create("GrilledPrickleFruit"),
      TagManager.Create(SpiceNutConfig.ID),
      TagManager.Create("SpiceBread"),
      SimHashes.Phosphorite.CreateTag()
    }, Tag.Invalid, LightBugPurpleConfig.CALORIES_PER_KG_OF_ORE);
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugPurpleConfig.CreateLightBug("LightBugPurple", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, "LightBugPurpleEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugPurpleBaby", 15f, 5f, LightBugTuning.EGG_CHANCES_PURPLE, LightBugPurpleConfig.EGG_SORT_ORDER, true, false, true, 1f);
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
