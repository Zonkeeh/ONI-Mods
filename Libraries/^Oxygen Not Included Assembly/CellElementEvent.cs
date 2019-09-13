// Decompiled with JetBrains decompiler
// Type: CellElementEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

public class CellElementEvent : CellEvent
{
  public CellElementEvent(string id, string reason, bool is_send, bool enable_logging = true)
    : base(id, reason, is_send, enable_logging)
  {
  }

  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void Log(int cell, SimHashes element, int callback_id)
  {
    if (!this.enableLogging)
      return;
    CellEventInstance ev = new CellEventInstance(cell, (int) element, 0, (CellEvent) this);
    CellEventLogger.Instance.Add(ev);
  }

  public override string GetDescription(EventInstanceBase ev)
  {
    return this.GetMessagePrefix() + "Element=" + ((SimHashes) (ev as CellEventInstance).data).ToString() + " (" + this.reason + ")";
  }
}
