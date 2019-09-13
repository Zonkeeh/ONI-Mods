// Decompiled with JetBrains decompiler
// Type: WaterCoolerChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using System;
using TUNING;

public class WaterCoolerChore : Chore<WaterCoolerChore.StatesInstance>, IWorkerPrioritizable
{
  public int basePriority = RELAXATION.PRIORITY.TIER2;
  public string specificEffect = "Socialized";
  public string trackingEffect = "RecentlySocialized";

  public WaterCoolerChore(
    IStateMachineTarget master,
    Workable chat_workable,
    System.Action<Chore> on_complete = null,
    System.Action<Chore> on_begin = null,
    System.Action<Chore> on_end = null)
    : base(Db.Get().ChoreTypes.Relax, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
  {
    this.smi = new WaterCoolerChore.StatesInstance(this);
    this.smi.sm.chitchatlocator.Set((KMonoBehaviour) chat_workable, this.smi);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) chat_workable);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Recreation);
    this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) this);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.drinker.Set(context.consumerState.gameObject, this.smi);
    base.Begin(context);
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }

  public class States : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore>
  {
    public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter drinker;
    public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter chitchatlocator;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<WaterCooler> drink_move;
    public WaterCoolerChore.States.DrinkStates drink;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<IApproachable> chat_move;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State chat;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.drink_move;
      this.Target(this.drinker);
      this.drink_move.InitializeStates(this.drinker, this.masterTarget, (GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) this.drink, (GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) null, (CellOffset[]) null, (NavTactic) null);
      this.drink.ToggleAnims("anim_interacts_watercooler_kanim", 0.0f).DefaultState(this.drink.drink);
      this.drink.drink.Face(this.masterTarget, 0.5f).PlayAnim("working_pre").QueueAnim("working_loop", false, (Func<WaterCoolerChore.StatesInstance, string>) null).OnAnimQueueComplete(this.drink.post);
      this.drink.post.Enter("Drink", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.Drink)).PlayAnim("working_pst").OnAnimQueueComplete((GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) this.chat_move);
      this.chat_move.InitializeStates(this.drinker, this.chitchatlocator, this.chat, (GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) null, (CellOffset[]) null, (NavTactic) null);
      this.chat.ToggleWork<SocialGatheringPointWorkable>(this.chitchatlocator, this.success, (GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) null, (Func<WaterCoolerChore.StatesInstance, bool>) null);
      this.success.ReturnSuccess();
    }

    private void Drink(WaterCoolerChore.StatesInstance smi)
    {
      Storage storage = this.masterTarget.Get<Storage>(smi);
      Worker cmp = this.stateTarget.Get<Worker>(smi);
      SimUtil.DiseaseInfo disease_info;
      float aggregate_temperature;
      storage.ConsumeAndGetDisease(GameTags.Water, 1f, out disease_info, out aggregate_temperature);
      cmp.GetSMI<GermExposureMonitor.Instance>()?.TryInjectDisease(disease_info.idx, disease_info.count, GameTags.Water, Sickness.InfectionVector.Digestion);
      Effects component = cmp.GetComponent<Effects>();
      if (string.IsNullOrEmpty(smi.master.trackingEffect))
        return;
      component.Add(smi.master.trackingEffect, true);
    }

    public class DrinkStates : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State
    {
      public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State drink;
      public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State post;
    }
  }

  public class StatesInstance : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.GameInstance
  {
    public StatesInstance(WaterCoolerChore master)
      : base(master)
    {
    }
  }
}
