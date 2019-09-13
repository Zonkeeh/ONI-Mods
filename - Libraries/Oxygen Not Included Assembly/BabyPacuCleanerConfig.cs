// Decompiled with JetBrains decompiler
// Type: BabyPacuCleanerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPacuCleanerConfig : IEntityConfig
{
  public const string ID = "PacuCleanerBaby";

  public GameObject CreatePrefab()
  {
    GameObject pacu = PacuCleanerConfig.CreatePacu("PacuCleanerBaby", (string) CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME, (string) CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.DESC, "baby_pacu_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(pacu, (Tag) "PacuCleaner", (string) null);
    return pacu;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
