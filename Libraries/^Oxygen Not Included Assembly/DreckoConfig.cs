// Decompiled with JetBrains decompiler
// Type: DreckoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class DreckoConfig : IEntityConfig
{
  public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();
  public static Tag EMIT_ELEMENT = (Tag) BasicFabricConfig.ID;
  private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 2f;
  private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;
  private static float KG_POOP_PER_DAY_OF_PLANT = 5f;
  private static float MIN_POOP_SIZE_IN_KG = 1.5f;
  private static float MIN_POOP_SIZE_IN_CALORIES = DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoConfig.MIN_POOP_SIZE_IN_KG / DreckoConfig.KG_POOP_PER_DAY_OF_PLANT;
  public static float SCALE_GROWTH_TIME_IN_CYCLES = 8f;
  public static float FIBER_PER_CYCLE = 0.25f;
  public static int EGG_SORT_ORDER = 800;
  public const string ID = "Drecko";
  public const string BASE_TRAIT_ID = "DreckoBaseTrait";
  public const string EGG_ID = "DreckoEgg";

  public static GameObject CreateDrecko(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoBaseTrait", is_baby, "fbr_", 308.15f, 363.15f), DreckoTuning.PEN_SIZE_PER_CREATURE, 150f);
    Trait trait = Db.Get().CreateTrait("DreckoBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
        "SpiceVine".ToTag(),
        SwampLilyConfig.ID.ToTag(),
        "BasicSingleHarvestPlant".ToTag()
      }, DreckoConfig.POOP_ELEMENT, DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoConfig.KG_POOP_PER_DAY_OF_PLANT, (string) null, 0.0f, false, true)
    });
    CreatureCalorieMonitor.Def def1 = wildCreature.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def1.diet = diet;
    def1.minPoopSizeInCalories = DreckoConfig.MIN_POOP_SIZE_IN_CALORIES;
    wildCreature.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    ScaleGrowthMonitor.Def def2 = wildCreature.AddOrGetDef<ScaleGrowthMonitor.Def>();
    def2.defaultGrowthRate = (float) (1.0 / (double) DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600.0);
    def2.dropMass = DreckoConfig.FIBER_PER_CYCLE * DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES;
    def2.itemDroppedOnShear = DreckoConfig.EMIT_ELEMENT;
    def2.levelCount = 6;
    def2.targetAtmosphere = SimHashes.Hydrogen;
    return wildCreature;
  }

  public virtual GameObject CreatePrefab()
  {
    GameObject drecko = DreckoConfig.CreateDrecko("Drecko", (string) CREATURES.SPECIES.DRECKO.NAME, (string) CREATURES.SPECIES.DRECKO.DESC, "drecko_kanim", false);
    string eggId = "DreckoEgg";
    string eggName = (string) CREATURES.SPECIES.DRECKO.EGG_NAME;
    string desc = (string) CREATURES.SPECIES.DRECKO.DESC;
    string egg_anim = "egg_drecko_kanim";
    float eggMass = DreckoTuning.EGG_MASS;
    string baby_id = "DreckoBaby";
    float fertility_cycles = 90f;
    float incubation_cycles = 30f;
    int eggSortOrder = DreckoConfig.EGG_SORT_ORDER;
    return EntityTemplates.ExtendEntityToFertileCreature(drecko, eggId, eggName, desc, egg_anim, eggMass, baby_id, fertility_cycles, incubation_cycles, DreckoTuning.EGG_CHANCES_BASE, eggSortOrder, true, false, true, 1f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
