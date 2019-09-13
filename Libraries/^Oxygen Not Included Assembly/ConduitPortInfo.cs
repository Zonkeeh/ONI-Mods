// Decompiled with JetBrains decompiler
// Type: ConduitPortInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class ConduitPortInfo
{
  public ConduitType conduitType;
  public CellOffset offset;

  public ConduitPortInfo(ConduitType type, CellOffset offset)
  {
    this.conduitType = type;
    this.offset = offset;
  }
}
