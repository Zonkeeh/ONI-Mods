// Decompiled with JetBrains decompiler
// Type: EODReportMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

public class EODReportMessage : Message
{
  [Serialize]
  private int day;
  [Serialize]
  private string title;
  [Serialize]
  private string tooltip;

  public EODReportMessage(string title, string tooltip)
  {
    this.day = GameUtil.GetCurrentCycle();
    this.title = title;
    this.tooltip = tooltip;
  }

  public EODReportMessage()
  {
  }

  public override string GetSound()
  {
    return (string) null;
  }

  public override string GetMessageBody()
  {
    return string.Empty;
  }

  public override string GetTooltip()
  {
    return this.tooltip;
  }

  public override string GetTitle()
  {
    return this.title;
  }

  public void OpenReport()
  {
    ManagementMenu.Instance.OpenReports(this.day);
  }

  public override bool ShowDialog()
  {
    return false;
  }

  public override void OnClick()
  {
    this.OpenReport();
  }
}
