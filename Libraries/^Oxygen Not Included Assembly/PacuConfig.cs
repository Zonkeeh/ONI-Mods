// Decompiled with JetBrains decompiler
// Type: PacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class PacuConfig : IEntityConfig
{
  public const string ID = "Pacu";
  public const string BASE_TRAIT_ID = "PacuBaseTrait";
  public const string EGG_ID = "PacuEgg";
  public const int EGG_SORT_ORDER = 500;

  public static GameObject CreatePacu(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    return EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, (string) null, 273.15f, 333.15f), PacuTuning.PEN_SIZE_PER_CREATURE, 25f);
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(PacuConfig.CreatePacu("Pacu", (string) CREATURES.SPECIES.PACU.NAME, (string) CREATURES.SPECIES.PACU.DESC, "pacu_kanim", false), "PacuEgg", (string) CREATURES.SPECIES.PACU.EGG_NAME, (string) CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuBaby", 15f, 5f, PacuTuning.EGG_CHANCES_BASE, 500, false, true, false, 0.75f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
    prefab.AddOrGet<LoopingSounds>();
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
