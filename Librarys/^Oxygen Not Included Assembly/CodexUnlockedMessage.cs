// Decompiled with JetBrains decompiler
// Type: CodexUnlockedMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class CodexUnlockedMessage : Message
{
  private string unlockMessage;
  private string lockId;

  public CodexUnlockedMessage()
  {
  }

  public CodexUnlockedMessage(string lock_id, string unlock_message)
  {
    this.lockId = lock_id;
    this.unlockMessage = unlock_message;
  }

  public string GetLockId()
  {
    return this.lockId;
  }

  public override string GetSound()
  {
    return "AI_Notification_ResearchComplete";
  }

  public override string GetMessageBody()
  {
    return UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);
  }

  public override string GetTitle()
  {
    return (string) UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;
  }

  public override string GetTooltip()
  {
    return this.GetMessageBody();
  }

  public override bool IsValid()
  {
    return true;
  }
}
