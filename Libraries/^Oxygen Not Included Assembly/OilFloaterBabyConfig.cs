// Decompiled with JetBrains decompiler
// Type: OilFloaterBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class OilFloaterBabyConfig : IEntityConfig
{
  public const string ID = "OilfloaterBaby";

  public GameObject CreatePrefab()
  {
    GameObject oilFloater = OilFloaterConfig.CreateOilFloater("OilfloaterBaby", (string) CREATURES.SPECIES.OILFLOATER.BABY.NAME, (string) CREATURES.SPECIES.OILFLOATER.BABY.DESC, "baby_oilfloater_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(oilFloater, (Tag) "Oilfloater", (string) null);
    return oilFloater;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
