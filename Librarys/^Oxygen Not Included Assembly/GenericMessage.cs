// Decompiled with JetBrains decompiler
// Type: GenericMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

public class GenericMessage : Message
{
  [Serialize]
  private string title;
  [Serialize]
  private string tooltip;
  [Serialize]
  private string body;

  public GenericMessage(string _title, string _body, string _tooltip)
  {
    this.title = _title;
    this.body = _body;
    this.tooltip = _tooltip;
  }

  public GenericMessage()
  {
  }

  public override string GetSound()
  {
    return (string) null;
  }

  public override string GetMessageBody()
  {
    return this.body;
  }

  public override string GetTooltip()
  {
    return this.tooltip;
  }

  public override string GetTitle()
  {
    return this.title;
  }
}
