// Decompiled with JetBrains decompiler
// Type: BabyMoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyMoleConfig : IEntityConfig
{
  public const string ID = "MoleBaby";

  public GameObject CreatePrefab()
  {
    GameObject mole = MoleConfig.CreateMole("MoleBaby", (string) CREATURES.SPECIES.MOLE.BABY.NAME, (string) CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(mole, (Tag) "Mole", (string) null);
    return mole;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    MoleConfig.SetSpawnNavType(inst);
  }
}
