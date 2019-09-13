// Decompiled with JetBrains decompiler
// Type: ChoreDriver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ChoreDriver : StateMachineComponent<ChoreDriver.StatesInstance>
{
  [MyCmpAdd]
  private User user;
  private Chore.Precondition.Context context;

  public Chore GetCurrentChore()
  {
    return this.smi.GetCurrentChore();
  }

  public bool HasChore()
  {
    return this.smi.GetCurrentChore() != null;
  }

  public void StopChore()
  {
    this.smi.sm.stop.Trigger(this.smi);
  }

  public void SetChore(Chore.Precondition.Context context)
  {
    Chore currentChore = this.smi.GetCurrentChore();
    if (currentChore == context.chore)
      return;
    this.StopChore();
    if (context.chore.IsValid())
    {
      context.chore.PrepareChore(ref context);
      this.context = context;
      this.smi.sm.nextChore.Set(context.chore, this.smi);
    }
    else
    {
      string str1 = "Null";
      string str2 = "Null";
      if (currentChore != null)
        str1 = currentChore.GetType().Name;
      if (context.chore != null)
        str2 = context.chore.GetType().Name;
      Debug.LogWarning((object) ("Stopping chore " + str1 + " to start " + str2 + " but stopping the first chore cancelled the second one."));
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.GameInstance
  {
    public StatesInstance(ChoreDriver master)
      : base(master)
    {
      this.GetComponent<ChoreConsumer>().choreRulesChanged += new System.Action(this.OnChoreRulesChanged);
    }

    public void BeginChore()
    {
      Chore chore = this.smi.sm.currentChore.Set(this.GetNextChore(), this.smi);
      if (chore != null && chore.IsPreemptable && (UnityEngine.Object) chore.driver != (UnityEngine.Object) null)
        chore.Fail("Preemption!");
      this.smi.sm.nextChore.Set((Chore) null, this.smi);
      chore.onExit += new System.Action<Chore>(this.OnChoreExit);
      chore.Begin(this.master.context);
      this.Trigger(-1988963660, (object) chore);
    }

    public void EndChore(string reason)
    {
      if (this.GetCurrentChore() == null)
        return;
      Chore currentChore = this.GetCurrentChore();
      this.smi.sm.currentChore.Set((Chore) null, this.smi);
      currentChore.onExit -= new System.Action<Chore>(this.OnChoreExit);
      currentChore.Fail(reason);
      this.Trigger(1745615042, (object) currentChore);
    }

    private void OnChoreExit(Chore chore)
    {
      this.smi.sm.stop.Trigger(this.smi);
    }

    public Chore GetNextChore()
    {
      return this.smi.sm.nextChore.Get(this.smi);
    }

    public Chore GetCurrentChore()
    {
      return this.smi.sm.currentChore.Get(this.smi);
    }

    private void OnChoreRulesChanged()
    {
      Chore currentChore = this.GetCurrentChore();
      if (currentChore == null || this.GetComponent<ChoreConsumer>().IsPermittedOrEnabled(currentChore.choreType, currentChore))
        return;
      this.EndChore("Permissions changed");
    }
  }

  public class States : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver>
  {
    public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> currentChore;
    public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> nextChore;
    public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Signal stop;
    public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State nochore;
    public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State haschore;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.nochore;
      this.saveHistory = true;
      this.nochore.Update((System.Action<ChoreDriver.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.master.HasTag(GameTags.Minion) || smi.master.HasTag(GameTags.Dead))
          return;
        ReportManager.Instance.ReportValue(ReportManager.ReportType.WorkTime, dt, string.Format((string) UI.ENDOFDAYREPORT.NOTES.TIME_SPENT, (object) DUPLICANTS.CHORES.THINKING.NAME), smi.master.GetProperName());
      }), UpdateRate.SIM_200ms, false).ParamTransition<Chore>((StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Parameter<Chore>) this.nextChore, this.haschore, (StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Parameter<Chore>.Callback) ((smi, next_chore) => next_chore != null));
      this.haschore.Enter("BeginChore", (StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State.Callback) (smi => smi.BeginChore())).Update((System.Action<ChoreDriver.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.master.HasTag(GameTags.Minion) || smi.master.HasTag(GameTags.Dead))
          return;
        Chore chore = this.currentChore.Get(smi);
        if (chore == null)
          return;
        if (smi.master.GetComponent<Navigator>().IsMoving())
        {
          ReportManager.Instance.ReportValue(ReportManager.ReportType.TravelTime, dt, GameUtil.GetChoreName(chore, (object) null), smi.master.GetProperName());
        }
        else
        {
          ReportManager.ReportType reportType1 = chore.GetReportType();
          Workable workable = smi.master.GetComponent<Worker>().workable;
          if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
          {
            ReportManager.ReportType reportType2 = workable.GetReportType();
            if (reportType1 != reportType2)
              reportType1 = reportType2;
          }
          ReportManager.Instance.ReportValue(reportType1, dt, string.Format((string) UI.ENDOFDAYREPORT.NOTES.WORK_TIME, (object) GameUtil.GetChoreName(chore, (object) null)), smi.master.GetProperName());
        }
      }), UpdateRate.SIM_200ms, false).Exit("EndChore", (StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State.Callback) (smi => smi.EndChore("ChoreDriver.SignalStop"))).OnSignal(this.stop, this.nochore);
    }
  }
}
