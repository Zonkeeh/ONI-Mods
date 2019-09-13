// Decompiled with JetBrains decompiler
// Type: BabyHatchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyHatchConfig : IEntityConfig
{
  public const string ID = "HatchBaby";

  public GameObject CreatePrefab()
  {
    GameObject hatch = HatchConfig.CreateHatch("HatchBaby", (string) CREATURES.SPECIES.HATCH.BABY.NAME, (string) CREATURES.SPECIES.HATCH.BABY.DESC, "baby_hatch_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(hatch, (Tag) "Hatch", (string) null);
    return hatch;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
