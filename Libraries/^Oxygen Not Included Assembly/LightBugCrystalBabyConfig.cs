// Decompiled with JetBrains decompiler
// Type: LightBugCrystalBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LightBugCrystalBabyConfig : IEntityConfig
{
  public const string ID = "LightBugCrystalBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugCrystalConfig.CreateLightBug("LightBugCrystalBaby", (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBugCrystal", (string) null);
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
