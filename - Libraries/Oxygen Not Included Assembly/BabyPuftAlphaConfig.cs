// Decompiled with JetBrains decompiler
// Type: BabyPuftAlphaConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPuftAlphaConfig : IEntityConfig
{
  public const string ID = "PuftAlphaBaby";

  public GameObject CreatePrefab()
  {
    GameObject puftAlpha = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", (string) CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, (string) CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(puftAlpha, (Tag) "PuftAlpha", (string) null);
    return puftAlpha;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    BasePuftConfig.OnSpawn(inst);
  }
}
