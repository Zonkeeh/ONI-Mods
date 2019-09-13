// Decompiled with JetBrains decompiler
// Type: PuftOxyliteConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class PuftOxyliteConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 50f;
  private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftOxyliteConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = 25f;
  public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 2;
  public const string ID = "PuftOxylite";
  public const string BASE_TRAIT_ID = "PuftOxyliteBaseTrait";
  public const string EGG_ID = "PuftOxyliteEgg";
  public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;
  public const SimHashes EMIT_ELEMENT = SimHashes.OxyRock;

  public static GameObject CreatePuftOxylite(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePuftConfig.BasePuft(id, name, desc, "PuftOxyliteBaseTrait", anim_file, is_baby, "com_", 303.15f, 338.15f), PuftTuning.PEN_SIZE_PER_CREATURE, 75f);
    Trait trait = Db.Get().CreateTrait("PuftOxyliteBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
    GameObject go = BasePuftConfig.SetupDiet(wildCreature, SimHashes.Oxygen.CreateTag(), SimHashes.OxyRock.CreateTag(), PuftOxyliteConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, (string) null, 0.0f, PuftOxyliteConfig.MIN_POOP_SIZE_IN_KG);
    go.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      SimHashes.OxyRock.CreateTag()
    };
    return go;
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(PuftOxyliteConfig.CreatePuftOxylite("PuftOxylite", (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.NAME, (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "puft_kanim", false), "PuftOxyliteEgg", (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftOxyliteBaby", 45f, 15f, PuftTuning.EGG_CHANCES_OXYLITE, PuftOxyliteConfig.EGG_SORT_ORDER, true, false, true, 1f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    BasePuftConfig.OnSpawn(inst);
  }
}
