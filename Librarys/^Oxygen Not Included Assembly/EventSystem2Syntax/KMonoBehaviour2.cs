// Decompiled with JetBrains decompiler
// Type: EventSystem2Syntax.KMonoBehaviour2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace EventSystem2Syntax
{
  internal class KMonoBehaviour2
  {
    protected virtual void OnPrefabInit()
    {
    }

    public void Subscribe(int evt, Action<object> cb)
    {
    }

    public void Trigger(int evt, object data)
    {
    }

    public void Subscribe<ListenerType, EventType>(Action<ListenerType, EventType> cb) where EventType : IEventData
    {
    }

    public void Trigger<EventType>(EventType evt) where EventType : IEventData
    {
    }
  }
}
