// Decompiled with JetBrains decompiler
// Type: ConversationType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ConversationType
{
  public string id;
  public string target;

  public virtual void NewTarget(MinionIdentity speaker)
  {
  }

  public virtual Conversation.Topic GetNextTopic(
    MinionIdentity speaker,
    Conversation.Topic lastTopic)
  {
    return (Conversation.Topic) null;
  }

  public virtual Sprite GetSprite(string topic)
  {
    return (Sprite) null;
  }
}
