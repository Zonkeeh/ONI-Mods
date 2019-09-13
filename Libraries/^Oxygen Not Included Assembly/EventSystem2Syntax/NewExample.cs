// Decompiled with JetBrains decompiler
// Type: EventSystem2Syntax.NewExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace EventSystem2Syntax
{
  internal class NewExample : KMonoBehaviour2
  {
    protected override void OnPrefabInit()
    {
      // ISSUE: reference to a compiler-generated field
      if (NewExample.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        NewExample.\u003C\u003Ef__mg\u0024cache0 = new Action<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.OnObjectDestroyed);
      }
      // ISSUE: reference to a compiler-generated field
      this.Subscribe<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.\u003C\u003Ef__mg\u0024cache0);
      this.Trigger<NewExample.ObjectDestroyedEvent>(new NewExample.ObjectDestroyedEvent()
      {
        parameter = false
      });
    }

    private static void OnObjectDestroyed(NewExample example, NewExample.ObjectDestroyedEvent evt)
    {
    }

    private struct ObjectDestroyedEvent : IEventData
    {
      public bool parameter;
    }
  }
}
