// Decompiled with JetBrains decompiler
// Type: OilFloaterDecorBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class OilFloaterDecorBabyConfig : IEntityConfig
{
  public const string ID = "OilfloaterDecorBaby";

  public GameObject CreatePrefab()
  {
    GameObject oilFloater = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecorBaby", (string) CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.NAME, (string) CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.DESC, "baby_oilfloater_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(oilFloater, (Tag) "OilfloaterDecor", (string) null);
    return oilFloater;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
