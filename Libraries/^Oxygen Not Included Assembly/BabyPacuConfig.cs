// Decompiled with JetBrains decompiler
// Type: BabyPacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPacuConfig : IEntityConfig
{
  public const string ID = "PacuBaby";

  public GameObject CreatePrefab()
  {
    GameObject pacu = PacuConfig.CreatePacu("PacuBaby", (string) CREATURES.SPECIES.PACU.BABY.NAME, (string) CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(pacu, (Tag) "Pacu", (string) null);
    return pacu;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
