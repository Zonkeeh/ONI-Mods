// Decompiled with JetBrains decompiler
// Type: DirtyWaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DirtyWaterConfig : IOreConfig
{
  public SimHashes ElementID
  {
    get
    {
      return SimHashes.DirtyWater;
    }
  }

  public SimHashes SublimeElementID
  {
    get
    {
      return SimHashes.ContaminatedOxygen;
    }
  }

  public GameObject CreatePrefab()
  {
    GameObject liquidOreEntity = EntityTemplates.CreateLiquidOreEntity(this.ElementID, (List<Tag>) null);
    Sublimates sublimates = liquidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
    sublimates.info = new Sublimates.Info(4E-05f, 0.025f, 1.8f, 1f, this.SublimeElementID, byte.MaxValue, 0);
    return liquidOreEntity;
  }
}
