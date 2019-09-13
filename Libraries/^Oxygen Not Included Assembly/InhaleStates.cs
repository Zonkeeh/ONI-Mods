// Decompiled with JetBrains decompiler
// Type: InhaleStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class InhaleStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
  public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State goingtoeat;
  public InhaleStates.InhalingStates inhaling;
  public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete;
  public StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.IntParameter targetCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtoeat;
    this.root.Enter("SetTarget", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi)));
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State state1 = this.goingtoeat.MoveTo((Func<InhaleStates.Instance, int>) (smi => this.targetCell.Get(smi)), (GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State) this.inhaling, (GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State) null, false);
    string name1 = (string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty1 = string.Empty;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state1.ToggleStatusItem(name2, tooltip2, empty1, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay1, 0, (Func<string, InhaleStates.Instance, string>) null, (Func<string, InhaleStates.Instance, string>) null, category1);
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State state2 = this.inhaling.DefaultState(this.inhaling.pre);
    string name3 = (string) CREATURES.STATUSITEMS.INHALING.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.INHALING.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    string name4 = name3;
    string tooltip4 = tooltip3;
    string empty2 = string.Empty;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state2.ToggleStatusItem(name4, tooltip4, empty2, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay2, 0, (Func<string, InhaleStates.Instance, string>) null, (Func<string, InhaleStates.Instance, string>) null, category2);
    this.inhaling.pre.PlayAnim("inhale_pre").QueueAnim("inhale_loop", true, (Func<InhaleStates.Instance, string>) null).Update("Consume", (System.Action<InhaleStates.Instance, float>) ((smi, dt) => smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().Consume(dt)), UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ElementNoLongerAvailable, this.inhaling.pst, (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback) null).Enter("StartInhaleSound", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => smi.StartInhaleSound())).Exit("StopInhaleSound", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => smi.StopInhaleSound())).ScheduleGoTo((Func<InhaleStates.Instance, float>) (smi => smi.def.inhaleTime), (StateMachine.BaseState) this.inhaling.pst);
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst = this.inhaling.pst;
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full = this.inhaling.full;
    // ISSUE: reference to a compiler-generated field
    if (InhaleStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      InhaleStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback fMgCache0 = InhaleStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State state3 = pst.Transition(full, fMgCache0, UpdateRate.SIM_200ms);
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete = this.behaviourcomplete;
    // ISSUE: reference to a compiler-generated field
    if (InhaleStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      InhaleStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback condition = GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Not(InhaleStates.\u003C\u003Ef__mg\u0024cache1);
    state3.Transition(behaviourcomplete, condition, UpdateRate.SIM_200ms);
    this.inhaling.full.QueueAnim("inhale_pst", false, (Func<InhaleStates.Instance, string>) null).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.WantsToEat, false);
  }

  private static bool IsFull(InhaleStates.Instance smi)
  {
    CreatureCalorieMonitor.Instance smi1 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
    if (smi1 != null)
      return (double) smi1.stomach.GetFullness() >= 1.0;
    return false;
  }

  public class Def : StateMachine.BaseDef
  {
    public float inhaleTime = 3f;
    public string inhaleSound;
  }

  public class Instance : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.GameInstance
  {
    public string inhaleSound;

    public Instance(Chore<InhaleStates.Instance> chore, InhaleStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToEat);
      this.inhaleSound = GlobalAssets.GetSound(def.inhaleSound, false);
    }

    public void StartInhaleSound()
    {
      LoopingSounds component = this.GetComponent<LoopingSounds>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.StartSound(this.smi.inhaleSound);
    }

    public void StopInhaleSound()
    {
      LoopingSounds component = this.GetComponent<LoopingSounds>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.StopSound(this.smi.inhaleSound);
    }
  }

  public class InhalingStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State
  {
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pre;
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst;
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full;
  }
}
