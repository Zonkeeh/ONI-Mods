// Decompiled with JetBrains decompiler
// Type: OilFloaterHighTempBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class OilFloaterHighTempBabyConfig : IEntityConfig
{
  public const string ID = "OilfloaterHighTempBaby";

  public GameObject CreatePrefab()
  {
    GameObject oilFloater = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTempBaby", (string) CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.NAME, (string) CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.DESC, "baby_oilfloater_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(oilFloater, (Tag) "OilfloaterHighTemp", (string) null);
    return oilFloater;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
