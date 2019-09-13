// Decompiled with JetBrains decompiler
// Type: BabyHatchVeggieConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyHatchVeggieConfig : IEntityConfig
{
  public const string ID = "HatchVeggieBaby";

  public GameObject CreatePrefab()
  {
    GameObject hatch = HatchVeggieConfig.CreateHatch("HatchVeggieBaby", (string) CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.NAME, (string) CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.DESC, "baby_hatch_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(hatch, (Tag) "HatchVeggie", (string) null);
    return hatch;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
