// Decompiled with JetBrains decompiler
// Type: MooConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MooConfig : IEntityConfig
{
  public static Tag POOP_ELEMENT = SimHashes.Methane.CreateTag();
  private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 2f;
  private static float CALORIES_PER_DAY_OF_PLANT_EATEN = MooTuning.STANDARD_CALORIES_PER_CYCLE / MooConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;
  private static float KG_POOP_PER_DAY_OF_PLANT = 5f;
  private static float MIN_POOP_SIZE_IN_KG = 1.5f;
  private static float MIN_POOP_SIZE_IN_CALORIES = MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * MooConfig.MIN_POOP_SIZE_IN_KG / MooConfig.KG_POOP_PER_DAY_OF_PLANT;
  public const string ID = "Moo";
  public const string BASE_TRAIT_ID = "MooBaseTrait";
  public const SimHashes CONSUME_ELEMENT = SimHashes.Carbon;

  public static GameObject CreateMoo(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject gameObject = BaseMooConfig.BaseMoo(id, name, (string) CREATURES.SPECIES.MOO.DESC, "MooBaseTrait", anim_file, is_baby, (string) null);
    EntityTemplates.ExtendEntityToWildCreature(gameObject, MooTuning.PEN_SIZE_PER_CREATURE, 75f);
    Trait trait = Db.Get().CreateTrait("MooBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) MooTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
        "GasGrass".ToTag()
      }, MooConfig.POOP_ELEMENT, MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, MooConfig.KG_POOP_PER_DAY_OF_PLANT, (string) null, 0.0f, false, false)
    });
    CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = MooConfig.MIN_POOP_SIZE_IN_CALORIES;
    gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return gameObject;
  }

  public GameObject CreatePrefab()
  {
    return MooConfig.CreateMoo("Moo", (string) CREATURES.SPECIES.MOO.NAME, (string) CREATURES.SPECIES.MOO.DESC, "gassy_moo_kanim", false);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    BaseMooConfig.OnSpawn(inst);
  }
}
