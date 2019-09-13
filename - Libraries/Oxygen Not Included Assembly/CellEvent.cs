// Decompiled with JetBrains decompiler
// Type: CellEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellEvent : EventBase
{
  public string reason;
  public bool isSend;
  public bool enableLogging;

  public CellEvent(string id, string reason, bool is_send, bool enable_logging = true)
    : base(id)
  {
    this.reason = reason;
    this.isSend = is_send;
    this.enableLogging = enable_logging;
  }

  public string GetMessagePrefix()
  {
    return this.isSend ? ">>>: " : "<<<: ";
  }
}
