// Decompiled with JetBrains decompiler
// Type: TargetMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

public abstract class TargetMessage : Message
{
  [Serialize]
  private MessageTarget target;

  protected TargetMessage()
  {
  }

  public TargetMessage(KPrefabID prefab_id)
  {
    this.target = new MessageTarget(prefab_id);
  }

  public MessageTarget GetTarget()
  {
    return this.target;
  }

  public override void OnCleanUp()
  {
    this.target.OnCleanUp();
  }
}
