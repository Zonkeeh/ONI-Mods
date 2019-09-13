// Decompiled with JetBrains decompiler
// Type: EmoteChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class EmoteChore : Chore<EmoteChore.StatesInstance>
{
  private Func<StatusItem> getStatusItem;
  private SelfEmoteReactable reactable;

  public EmoteChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    HashedString[] emote_anims,
    Func<StatusItem> get_status_item = null)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new EmoteChore.StatesInstance(this, target.gameObject, (HashedString) ((string) null), emote_anims, KAnim.PlayMode.Once, false);
    this.getStatusItem = get_status_item;
  }

  public EmoteChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    HashedString emote_kanim,
    HashedString[] emote_anims,
    KAnim.PlayMode play_mode,
    bool flip_x = false)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, play_mode, flip_x);
  }

  public EmoteChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    HashedString emote_kanim,
    HashedString[] emote_anims,
    Func<StatusItem> get_status_item)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, KAnim.PlayMode.Once, false);
    this.getStatusItem = get_status_item;
  }

  protected override StatusItem GetStatusItem()
  {
    if (this.getStatusItem != null)
      return this.getStatusItem();
    return base.GetStatusItem();
  }

  public override string ToString()
  {
    if (this.smi.emoteKAnim.IsValid)
      return "EmoteChore<" + (object) this.smi.emoteKAnim + ">";
    return "EmoteChore<" + (object) this.smi.emoteAnims[0] + ">";
  }

  public void PairReactable(SelfEmoteReactable reactable)
  {
    this.reactable = reactable;
  }

  protected new virtual void End(string reason)
  {
    if (this.reactable != null)
    {
      this.reactable.PairEmote((EmoteChore) null);
      this.reactable.Cleanup();
      this.reactable = (SelfEmoteReactable) null;
    }
    base.End(reason);
  }

  public class StatesInstance : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.GameInstance
  {
    public KAnim.PlayMode mode = KAnim.PlayMode.Once;
    public HashedString[] emoteAnims;
    public HashedString emoteKAnim;

    public StatesInstance(
      EmoteChore master,
      GameObject emoter,
      HashedString emote_kanim,
      HashedString[] emote_anims,
      KAnim.PlayMode mode,
      bool flip_x)
      : base(master)
    {
      this.emoteKAnim = emote_kanim;
      this.emoteAnims = emote_anims;
      this.mode = mode;
      this.sm.emoter.Set(emoter, this.smi);
    }
  }

  public class States : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore>
  {
    public StateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.TargetParameter emoter;
    public GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.State finish;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.Target(this.emoter);
      this.root.ToggleAnims((Func<EmoteChore.StatesInstance, HashedString>) (smi => smi.emoteKAnim)).PlayAnims((Func<EmoteChore.StatesInstance, HashedString[]>) (smi => smi.emoteAnims), (Func<EmoteChore.StatesInstance, KAnim.PlayMode>) (smi => smi.mode)).ScheduleGoTo(10f, (StateMachine.BaseState) this.finish).OnAnimQueueComplete(this.finish);
      this.finish.ReturnSuccess();
    }
  }
}
