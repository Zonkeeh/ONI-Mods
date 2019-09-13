// Decompiled with JetBrains decompiler
// Type: BabyPacuTropicalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPacuTropicalConfig : IEntityConfig
{
  public const string ID = "PacuTropicalBaby";

  public GameObject CreatePrefab()
  {
    GameObject pacu = PacuTropicalConfig.CreatePacu("PacuTropicalBaby", (string) CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.NAME, (string) CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.DESC, "baby_pacu_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(pacu, (Tag) "PacuTropical", (string) null);
    return pacu;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
