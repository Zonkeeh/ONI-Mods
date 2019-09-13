// Decompiled with JetBrains decompiler
// Type: SelfEmoteReactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SelfEmoteReactable : EmoteReactable
{
  private EmoteChore emote;

  public SelfEmoteReactable(
    GameObject gameObject,
    HashedString id,
    ChoreType chore_type,
    HashedString animset,
    float min_reactable_time = 0.0f,
    float min_reactor_time = 20f,
    float max_trigger_time = float.PositiveInfinity)
    : base(gameObject, id, chore_type, animset, 3, 3, min_reactable_time, min_reactor_time, max_trigger_time)
  {
  }

  public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
  {
    if ((Object) reactor == (Object) null)
      return false;
    Navigator component = reactor.GetComponent<Navigator>();
    if ((Object) component == (Object) null || !component.IsMoving())
      return false;
    return (Object) this.gameObject == (Object) reactor;
  }

  public void PairEmote(EmoteChore emote)
  {
    this.emote = emote;
  }

  protected override void InternalEnd()
  {
    if (this.emote != null && (Object) this.emote.driver != (Object) null)
    {
      this.emote.PairReactable((SelfEmoteReactable) null);
      this.emote.Cancel("Reactable ended");
      this.emote = (EmoteChore) null;
    }
    base.InternalEnd();
  }
}
