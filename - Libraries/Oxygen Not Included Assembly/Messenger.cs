// Decompiled with JetBrains decompiler
// Type: Messenger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;

public class Messenger : KMonoBehaviour
{
  [Serialize]
  private SerializedList<Message> messages = new SerializedList<Message>();
  public static Messenger Instance;

  public int Count
  {
    get
    {
      return this.messages.Count;
    }
  }

  public IEnumerator<Message> GetEnumerator()
  {
    return this.messages.GetEnumerator();
  }

  public static void DestroyInstance()
  {
    Messenger.Instance = (Messenger) null;
  }

  public SerializedList<Message> Messages
  {
    get
    {
      return this.messages;
    }
  }

  protected override void OnPrefabInit()
  {
    Messenger.Instance = this;
  }

  protected override void OnSpawn()
  {
    int idx = 0;
    while (idx < this.messages.Count)
    {
      if (this.messages[idx].IsValid())
        ++idx;
      else
        this.messages.RemoveAt(idx);
    }
    this.Trigger(-599791736, (object) null);
  }

  public void QueueMessage(Message message)
  {
    this.messages.Add(message);
    this.Trigger(1558809273, (object) message);
  }

  public Message DequeueMessage()
  {
    Message message = (Message) null;
    if (this.messages.Count > 0)
    {
      message = this.messages[0];
      this.messages.RemoveAt(0);
    }
    return message;
  }

  public void ClearAllMessages()
  {
    for (int idx = this.messages.Count - 1; idx >= 0; --idx)
      this.messages.RemoveAt(idx);
  }

  public void RemoveMessage(Message m)
  {
    this.messages.Remove(m);
    this.Trigger(-599791736, (object) null);
  }
}
