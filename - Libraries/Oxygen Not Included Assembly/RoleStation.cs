// Decompiled with JetBrains decompiler
// Type: RoleStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class RoleStation : Workable, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<RoleStation> OnUpdateDelegate = new EventSystem.IntraObjectHandler<RoleStation>((System.Action<RoleStation, object>) ((component, data) => component.UpdateSkillPointAvailableStatusItem(data)));
  private List<int> subscriptions = new List<int>();
  private Chore chore;
  [MyCmpAdd]
  private Notifier notifier;
  [MyCmpAdd]
  private Operational operational;
  private RoleStation.RoleStationSM.Instance smi;
  private Guid skillPointAvailableStatusItem;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.RoleStations.Add(this);
    this.smi = new RoleStation.RoleStationSM.Instance(this);
    this.smi.StartSM();
    this.SetWorkTime(7.53f);
    this.resetProgressOnStop = true;
    this.subscriptions.Add(this.Subscribe<RoleStation>(-1523247426, RoleStation.OnUpdateDelegate));
    this.subscriptions.Add(this.Subscribe<RoleStation>(1505456302, RoleStation.OnUpdateDelegate));
    this.UpdateSkillPointAvailableStatusItem((object) null);
  }

  protected override void OnStopWork(Worker worker)
  {
    Telepad.StatesInstance smi = this.GetSMI<Telepad.StatesInstance>();
    smi.sm.idlePortal.Trigger(smi);
  }

  private void UpdateSkillPointAvailableStatusItem(object data = null)
  {
    IEnumerator enumerator = Components.MinionResumes.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        MinionResume current = (MinionResume) enumerator.Current;
        if (current.TotalSkillPointsGained - current.SkillsMastered > 0)
        {
          if (!(this.skillPointAvailableStatusItem == Guid.Empty))
            return;
          this.skillPointAvailableStatusItem = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, (object) null);
          return;
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, false);
    this.skillPointAvailableStatusItem = Guid.Empty;
  }

  private Chore CreateWorkChore()
  {
    return (Chore) new WorkChore<RoleStation>(Db.Get().ChoreTypes.LearnSkill, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, false, true, Assets.GetAnim((HashedString) "anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    worker.GetComponent<MinionResume>().SkillLearned();
  }

  private void OnSelectRolesClick()
  {
    DetailsScreen.Instance.Show(false);
    ManagementMenu.Instance.ToggleSkills();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    foreach (int subscription in this.subscriptions)
      Game.Instance.Unsubscribe(subscription);
    Components.RoleStations.Remove(this);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return new List<Descriptor>();
  }

  public class RoleStationSM : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation>
  {
    public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State unoperational;
    public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (StateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.operational.ToggleChore((Func<RoleStation.RoleStationSM.Instance, Chore>) (smi => smi.master.CreateWorkChore()), this.unoperational);
    }

    public class Instance : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.GameInstance
    {
      public Instance(RoleStation master)
        : base(master)
      {
      }
    }
  }
}
