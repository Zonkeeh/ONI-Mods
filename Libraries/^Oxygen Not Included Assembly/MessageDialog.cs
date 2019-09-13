// Decompiled with JetBrains decompiler
// Type: MessageDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class MessageDialog : KMonoBehaviour
{
  public virtual bool CanDontShowAgain
  {
    get
    {
      return false;
    }
  }

  public abstract bool CanDisplay(Message message);

  public abstract void SetMessage(Message message);

  public abstract void OnClickAction();

  public virtual void OnDontShowAgain()
  {
  }
}
