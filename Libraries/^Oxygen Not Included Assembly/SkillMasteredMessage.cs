// Decompiled with JetBrains decompiler
// Type: SkillMasteredMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;

public class SkillMasteredMessage : Message
{
  [Serialize]
  private string minionName;

  public SkillMasteredMessage()
  {
  }

  public SkillMasteredMessage(MinionResume resume)
  {
    this.minionName = resume.GetProperName();
  }

  public override string GetSound()
  {
    return "AI_Notification_ResearchComplete";
  }

  public override string GetMessageBody()
  {
    Debug.Assert(this.minionName != null);
    string str = string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, (object) this.minionName);
    return string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, (object) str);
  }

  public override string GetTitle()
  {
    return (string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME;
  }

  public override string GetTooltip()
  {
    return string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP, (object) string.Empty);
  }

  public override bool IsValid()
  {
    return this.minionName != null;
  }
}
