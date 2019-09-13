// Decompiled with JetBrains decompiler
// Type: EventSystem2Syntax.OldExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace EventSystem2Syntax
{
  internal class OldExample : KMonoBehaviour2
  {
    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.Subscribe(0, new System.Action<object>(this.OnObjectDestroyed));
      this.Trigger(0, (object) false);
    }

    private void OnObjectDestroyed(object data)
    {
      Debug.Log((object) (bool) data);
    }
  }
}
