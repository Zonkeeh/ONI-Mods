// Decompiled with JetBrains decompiler
// Type: BleachStoneConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BleachStoneConfig : IOreConfig
{
  public SimHashes ElementID
  {
    get
    {
      return SimHashes.BleachStone;
    }
  }

  public SimHashes SublimeElementID
  {
    get
    {
      return SimHashes.ChlorineGas;
    }
  }

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID, (List<Tag>) null);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.BleachStoneEmissionBubbles;
    sublimates.info = new Sublimates.Info(0.0002f, 0.0025f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
    return solidOreEntity;
  }
}
