// Decompiled with JetBrains decompiler
// Type: BabyHatchMetalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyHatchMetalConfig : IEntityConfig
{
  public const string ID = "HatchMetalBaby";

  public GameObject CreatePrefab()
  {
    GameObject hatch = HatchMetalConfig.CreateHatch("HatchMetalBaby", (string) CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.NAME, (string) CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.DESC, "baby_hatch_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(hatch, (Tag) "HatchMetal", (string) null);
    return hatch;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
