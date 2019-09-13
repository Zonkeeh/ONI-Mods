// Decompiled with JetBrains decompiler
// Type: OwnableSlotInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

[DebuggerDisplay("{slot.Id}")]
public class OwnableSlotInstance : AssignableSlotInstance
{
  public OwnableSlotInstance(Assignables assignables, OwnableSlot slot)
    : base(assignables, (AssignableSlot) slot)
  {
  }
}
