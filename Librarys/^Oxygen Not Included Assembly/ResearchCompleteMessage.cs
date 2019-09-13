// Decompiled with JetBrains decompiler
// Type: ResearchCompleteMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;

public class ResearchCompleteMessage : Message
{
  [Serialize]
  private ResourceRef<Tech> tech = new ResourceRef<Tech>();

  public ResearchCompleteMessage()
  {
  }

  public ResearchCompleteMessage(Tech tech)
  {
    this.tech.Set(tech);
  }

  public override string GetSound()
  {
    return "AI_Notification_ResearchComplete";
  }

  public override string GetMessageBody()
  {
    Tech tech = this.tech.Get();
    string empty = string.Empty;
    for (int index = 0; index < tech.unlockedItems.Count; ++index)
    {
      if (index != 0)
        empty += ", ";
      empty += tech.unlockedItems[index].Name;
    }
    return string.Format((string) MISC.NOTIFICATIONS.RESEARCHCOMPLETE.MESSAGEBODY, (object) tech.Name, (object) empty);
  }

  public override string GetTitle()
  {
    return (string) MISC.NOTIFICATIONS.RESEARCHCOMPLETE.NAME;
  }

  public override string GetTooltip()
  {
    Tech tech = this.tech.Get();
    return string.Format((string) MISC.NOTIFICATIONS.RESEARCHCOMPLETE.TOOLTIP, (object) tech.Name);
  }

  public override bool IsValid()
  {
    return this.tech.Get() != null;
  }
}
