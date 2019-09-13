// Decompiled with JetBrains decompiler
// Type: CrabConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

[EntityConfigOrder(1)]
public class CrabConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 140f;
  private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = 25f;
  public static int EGG_SORT_ORDER = 0;
  public const string ID = "Crab";
  public const string BASE_TRAIT_ID = "CrabBaseTrait";
  public const string EGG_ID = "CrabEgg";
  private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

  public static GameObject CreateCrab(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby,
    string deathDropID)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabBaseTrait", is_baby, (string) null, deathDropID), CrabTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("CrabBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
    List<Diet.Info> diet_infos = BaseCrabConfig.BasicDiet(SimHashes.Sand.CreateTag(), CrabConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, (string) null, 0.0f);
    return BaseCrabConfig.SetupDiet(wildCreature, diet_infos, CrabConfig.CALORIES_PER_KG_OF_ORE, CrabConfig.MIN_POOP_SIZE_IN_KG);
  }

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(CrabConfig.CreateCrab("Crab", (string) STRINGS.CREATURES.SPECIES.CRAB.NAME, (string) STRINGS.CREATURES.SPECIES.CRAB.DESC, "pincher_kanim", false, "CrabShell"), "CrabEgg", (string) STRINGS.CREATURES.SPECIES.CRAB.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.CRAB.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabBaby", 60f, 20f, CrabTuning.EGG_CHANCES_BASE, CrabConfig.EGG_SORT_ORDER, true, false, true, 1f);
    fertileCreature.AddOrGetDef<EggProtectionMonitor.Def>().allyTags = new Tag[1]
    {
      GameTags.Creatures.CrabFriend
    };
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
