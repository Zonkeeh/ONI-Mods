// Decompiled with JetBrains decompiler
// Type: SlimeMoldConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SlimeMoldConfig : IOreConfig
{
  public SimHashes ElementID
  {
    get
    {
      return SimHashes.SlimeMold;
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
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID, (List<Tag>) null);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
    sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0.0f, this.SublimeElementID, byte.MaxValue, 0);
    return solidOreEntity;
  }
}
