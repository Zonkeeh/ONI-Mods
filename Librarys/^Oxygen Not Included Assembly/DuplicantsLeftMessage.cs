// Decompiled with JetBrains decompiler
// Type: DuplicantsLeftMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class DuplicantsLeftMessage : Message
{
  public override string GetSound()
  {
    return string.Empty;
  }

  public override string GetTitle()
  {
    return (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;
  }

  public override string GetMessageBody()
  {
    return (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;
  }

  public override string GetTooltip()
  {
    return (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
  }
}
