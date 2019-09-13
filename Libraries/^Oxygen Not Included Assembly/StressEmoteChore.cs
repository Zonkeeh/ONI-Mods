// Decompiled with JetBrains decompiler
// Type: StressEmoteChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class StressEmoteChore : Chore<StressEmoteChore.StatesInstance>
{
  private Func<StatusItem> getStatusItem;

  public StressEmoteChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    HashedString emote_kanim,
    HashedString[] emote_anims,
    KAnim.PlayMode play_mode,
    Func<StatusItem> get_status_item)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.AddPrecondition(ChorePreconditions.instance.IsMoving, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.NotInTube, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsAwake, (object) null);
    this.getStatusItem = get_status_item;
    this.smi = new StressEmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, play_mode);
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
      return "StressEmoteChore<" + (object) this.smi.emoteKAnim + ">";
    return "StressEmoteChore<" + (object) this.smi.emoteAnims[0] + ">";
  }

  public class StatesInstance : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.GameInstance
  {
    public KAnim.PlayMode mode = KAnim.PlayMode.Once;
    public HashedString[] emoteAnims;
    public HashedString emoteKAnim;

    public StatesInstance(
      StressEmoteChore master,
      GameObject emoter,
      HashedString emote_kanim,
      HashedString[] emote_anims,
      KAnim.PlayMode mode)
      : base(master)
    {
      this.emoteKAnim = emote_kanim;
      this.emoteAnims = emote_anims;
      this.mode = mode;
      this.sm.emoter.Set(emoter, this.smi);
    }
  }

  public class States : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore>
  {
    public StateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.TargetParameter emoter;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.Target(this.emoter);
      this.root.ToggleAnims((Func<StressEmoteChore.StatesInstance, HashedString>) (smi => smi.emoteKAnim)).ToggleThought(Db.Get().Thoughts.Unhappy, (Func<StressEmoteChore.StatesInstance, bool>) null).PlayAnims((Func<StressEmoteChore.StatesInstance, HashedString[]>) (smi => smi.emoteAnims), (Func<StressEmoteChore.StatesInstance, KAnim.PlayMode>) (smi => smi.mode)).OnAnimQueueComplete((GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.State) null);
    }
  }
}
