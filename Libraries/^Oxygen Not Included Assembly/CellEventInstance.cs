// Decompiled with JetBrains decompiler
// Type: CellEventInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
  [Serialize]
  public int cell;
  [Serialize]
  public int data;
  [Serialize]
  public int data2;

  public CellEventInstance(int cell, int data, int data2, CellEvent ev)
    : base((EventBase) ev)
  {
    this.cell = cell;
    this.data = data;
    this.data2 = data2;
  }
}
