// Decompiled with JetBrains decompiler
// Type: CellSolidEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

public class CellSolidEvent : CellEvent
{
  public CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true)
    : base(id, reason, is_send, enable_logging)
  {
  }

  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void Log(int cell, bool solid)
  {
    if (!this.enableLogging)
      return;
    CellEventInstance ev = new CellEventInstance(cell, !solid ? 0 : 1, 0, (CellEvent) this);
    CellEventLogger.Instance.Add(ev);
  }

  public override string GetDescription(EventInstanceBase ev)
  {
    if ((ev as CellEventInstance).data == 1)
      return this.GetMessagePrefix() + "Solid=true (" + this.reason + ")";
    return this.GetMessagePrefix() + "Solid=false (" + this.reason + ")";
  }
}
