// Decompiled with JetBrains decompiler
// Type: OxyRockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class OxyRockConfig : IOreConfig
{
  public SimHashes ElementID
  {
    get
    {
      return SimHashes.OxyRock;
    }
  }

  public SimHashes SublimeElementID
  {
    get
    {
      return SimHashes.Oxygen;
    }
  }

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID, (List<Tag>) null);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
    sublimates.info = new Sublimates.Info(0.01f, 0.005f, 1.8f, 0.7f, this.SublimeElementID, byte.MaxValue, 0);
    return solidOreEntity;
  }
}
