// Decompiled with JetBrains decompiler
// Type: CellSolidFilterEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

public class CellSolidFilterEvent : CellEvent
{
  public CellSolidFilterEvent(string id, bool enable_logging = true)
    : base(id, "filtered", false, enable_logging)
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
    return this.GetMessagePrefix() + "Filtered Solid Event solid=" + (ev as CellEventInstance).data.ToString();
  }
}
