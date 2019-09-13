// Decompiled with JetBrains decompiler
// Type: BabySquirrelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabySquirrelConfig : IEntityConfig
{
  public const string ID = "SquirrelBaby";

  public GameObject CreatePrefab()
  {
    GameObject squirrel = SquirrelConfig.CreateSquirrel("SquirrelBaby", (string) CREATURES.SPECIES.SQUIRREL.BABY.NAME, (string) CREATURES.SPECIES.SQUIRREL.BABY.DESC, "baby_squirrel_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(squirrel, (Tag) "Squirrel", (string) null);
    return squirrel;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
