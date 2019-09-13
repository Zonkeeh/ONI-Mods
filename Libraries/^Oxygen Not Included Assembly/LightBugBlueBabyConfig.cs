// Decompiled with JetBrains decompiler
// Type: LightBugBlueBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LightBugBlueBabyConfig : IEntityConfig
{
  public const string ID = "LightBugBlueBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugBlueConfig.CreateLightBug("LightBugBlueBaby", (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBugBlue", (string) null);
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
