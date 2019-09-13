// Decompiled with JetBrains decompiler
// Type: AlgaeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AlgaeConfig : IOreConfig
{
  public SimHashes ElementID
  {
    get
    {
      return SimHashes.Algae;
    }
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.CreateSolidOreEntity(this.ElementID, new List<Tag>()
    {
      GameTags.Life
    });
  }
}
