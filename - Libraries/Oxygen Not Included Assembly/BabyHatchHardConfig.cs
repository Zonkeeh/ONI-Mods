// Decompiled with JetBrains decompiler
// Type: BabyHatchHardConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyHatchHardConfig : IEntityConfig
{
  public const string ID = "HatchHardBaby";

  public GameObject CreatePrefab()
  {
    GameObject hatch = HatchHardConfig.CreateHatch("HatchHardBaby", (string) CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.NAME, (string) CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.DESC, "baby_hatch_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(hatch, (Tag) "HatchHard", (string) null);
    return hatch;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
