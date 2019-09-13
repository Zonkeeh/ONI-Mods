// Decompiled with JetBrains decompiler
// Type: GameTagExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class GameTagExtensions
{
  public static GameObject Prefab(this Tag tag)
  {
    return Assets.GetPrefab(tag);
  }

  public static string ProperName(this Tag tag)
  {
    return TagManager.GetProperName(tag);
  }

  public static Tag Create(SimHashes id)
  {
    return TagManager.Create(id.ToString());
  }

  public static Tag CreateTag(this SimHashes id)
  {
    return TagManager.Create(id.ToString());
  }
}
