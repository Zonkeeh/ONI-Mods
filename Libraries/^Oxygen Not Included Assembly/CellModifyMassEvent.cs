// Decompiled with JetBrains decompiler
// Type: CellModifyMassEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

public class CellModifyMassEvent : CellEvent
{
  public CellModifyMassEvent(string id, string reason, bool enable_logging = false)
    : base(id, reason, true, enable_logging)
  {
  }

  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void Log(int cell, SimHashes element, float amount)
  {
    if (!this.enableLogging)
      return;
    CellEventInstance ev = new CellEventInstance(cell, (int) element, (int) ((double) amount * 1000.0), (CellEvent) this);
    CellEventLogger.Instance.Add(ev);
  }

  public override string GetDescription(EventInstanceBase ev)
  {
    CellEventInstance cellEventInstance = ev as CellEventInstance;
    return this.GetMessagePrefix() + "Element=" + ((SimHashes) cellEventInstance.data).ToString() + ", Mass=" + (object) (float) ((double) cellEventInstance.data2 / 1000.0) + " (" + this.reason + ")";
  }
}
