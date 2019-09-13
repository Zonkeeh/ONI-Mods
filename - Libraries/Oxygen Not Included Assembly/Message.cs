// Decompiled with JetBrains decompiler
// Type: Message
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
  public abstract string GetTitle();

  public abstract string GetSound();

  public abstract string GetMessageBody();

  public abstract string GetTooltip();

  public virtual bool ShowDialog()
  {
    return true;
  }

  public virtual void OnCleanUp()
  {
  }

  public virtual bool IsValid()
  {
    return true;
  }

  public virtual bool PlayNotificationSound()
  {
    return true;
  }

  public virtual void OnClick()
  {
  }
}
