// Decompiled with JetBrains decompiler
// Type: LightBugPinkBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LightBugPinkBabyConfig : IEntityConfig
{
  public const string ID = "LightBugPinkBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugPinkConfig.CreateLightBug("LightBugPinkBaby", (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBugPink", (string) null);
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
