// Decompiled with JetBrains decompiler
// Type: OilFloaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class OilFloaterConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 20f;
  private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = 0.5f;
  public static int EGG_SORT_ORDER = 400;
  public const string ID = "Oilfloater";
  public const string BASE_TRAIT_ID = "OilfloaterBaseTrait";
  public const string EGG_ID = "OilfloaterEgg";
  public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;
  public const SimHashes EMIT_ELEMENT = SimHashes.CrudeOil;

  public static GameObject CreateOilFloater(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterBaseTrait", 323.15f, 413.15f, is_baby, (string) null);
    EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("OilfloaterBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
    return BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.CrudeOil.CreateTag(), OilFloaterConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, (string) null, 0.0f, OilFloaterConfig.MIN_POOP_SIZE_IN_KG);
  }

  public GameObject CreatePrefab()
  {
    GameObject oilFloater = OilFloaterConfig.CreateOilFloater("Oilfloater", (string) STRINGS.CREATURES.SPECIES.OILFLOATER.NAME, (string) STRINGS.CREATURES.SPECIES.OILFLOATER.DESC, "oilfloater_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(oilFloater, "OilfloaterEgg", (string) STRINGS.CREATURES.SPECIES.OILFLOATER.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.OILFLOATER.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterBaby", 60f, 20f, OilFloaterTuning.EGG_CHANCES_BASE, OilFloaterConfig.EGG_SORT_ORDER, true, false, true, 1f);
    return oilFloater;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
