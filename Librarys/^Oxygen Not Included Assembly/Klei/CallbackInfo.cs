// Decompiled with JetBrains decompiler
// Type: Klei.CallbackInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei
{
  public struct CallbackInfo
  {
    private HandleVector<Game.CallbackInfo>.Handle handle;

    public CallbackInfo(HandleVector<Game.CallbackInfo>.Handle h)
    {
      this.handle = h;
    }

    public void Release()
    {
      if (!this.handle.IsValid())
        return;
      Game.CallbackInfo callbackInfo = Game.Instance.callbackManager.GetItem(this.handle);
      System.Action cb = callbackInfo.cb;
      if (!callbackInfo.manuallyRelease)
        Game.Instance.callbackManager.Release(this.handle);
      cb();
    }
  }
}
