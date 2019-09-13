// Decompiled with JetBrains decompiler
// Type: Approachable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
public class Approachable : KMonoBehaviour, IApproachable
{
  public CellOffset[] GetOffsets()
  {
    return OffsetGroups.Use;
  }

  public int GetCell()
  {
    return Grid.PosToCell((KMonoBehaviour) this);
  }

  Transform IApproachable.get_transform()
  {
    return this.transform;
  }
}
