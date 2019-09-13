// Decompiled with JetBrains decompiler
// Type: AchievementEarnedMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class AchievementEarnedMessage : Message
{
  public override bool ShowDialog()
  {
    return false;
  }

  public override string GetSound()
  {
    return "AI_Notification_ResearchComplete";
  }

  public override string GetMessageBody()
  {
    return string.Empty;
  }

  public override string GetTitle()
  {
    return (string) MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.NAME;
  }

  public override string GetTooltip()
  {
    return (string) MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.TOOLTIP;
  }

  public override bool IsValid()
  {
    return true;
  }

  public override void OnClick()
  {
    RetireColonyUtility.SaveColonySummaryData();
    MainMenu.ActivateRetiredColoniesScreen(PauseScreen.Instance.transform.parent.gameObject, SaveGame.Instance.BaseName, (string[]) null);
  }
}
