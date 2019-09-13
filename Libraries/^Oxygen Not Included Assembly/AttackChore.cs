// Decompiled with JetBrains decompiler
// Type: AttackChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class AttackChore : Chore<AttackChore.StatesInstance>
{
  private MultitoolController.Instance multiTool;

  public AttackChore(IStateMachineTarget target, GameObject enemy)
    : base(Db.Get().ChoreTypes.Attack, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new AttackChore.StatesInstance(this);
    this.smi.sm.attackTarget.Set(enemy, this.smi);
  }

  protected override void OnStateMachineStop(string reason, StateMachine.Status status)
  {
    this.CleanUpMultitool();
    base.OnStateMachineStop(reason, status);
  }

  public string GetHitAnim()
  {
    Workable component = this.smi.sm.attackTarget.Get(this.smi).gameObject.GetComponent<Workable>();
    if ((bool) ((UnityEngine.Object) component))
      return MultitoolController.GetAnimationStrings(component, this.gameObject.GetComponent<Worker>(), "hit")[1].Replace("_loop", string.Empty);
    return "hit";
  }

  public void OnTargetMoved(object data)
  {
    int cell1 = Grid.PosToCell(this.smi.master.gameObject);
    if ((UnityEngine.Object) this.smi.sm.attackTarget.Get(this.smi) == (UnityEngine.Object) null)
    {
      this.CleanUpMultitool();
    }
    else
    {
      if (this.smi.GetCurrentState() == this.smi.sm.attack)
      {
        int cell2 = Grid.PosToCell(this.smi.sm.attackTarget.Get(this.smi).gameObject);
        IApproachable component = this.smi.sm.attackTarget.Get(this.smi).gameObject.GetComponent<IApproachable>();
        if (component != null)
        {
          CellOffset[] offsets = component.GetOffsets();
          if (cell1 == cell2 || !Grid.IsCellOffsetOf(cell1, cell2, offsets))
          {
            if (this.multiTool != null)
              this.CleanUpMultitool();
            this.smi.GoTo((StateMachine.BaseState) this.smi.sm.approachtarget);
          }
        }
        else
          Debug.Log((object) "has no approachable");
      }
      if (this.multiTool == null)
        return;
      this.multiTool.UpdateHitEffectTarget();
    }
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.attacker.Set(context.consumerState.gameObject, this.smi);
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    this.CleanUpMultitool();
    base.End(reason);
  }

  public void OnTargetDestroyed(object data)
  {
    this.Fail("target destroyed");
  }

  private void CleanUpMultitool()
  {
    if (this.smi.master.multiTool == null)
      return;
    this.multiTool.DestroyHitEffect();
    this.multiTool.StopSM("attack complete");
    this.multiTool = (MultitoolController.Instance) null;
  }

  public class StatesInstance : GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.GameInstance
  {
    public StatesInstance(AttackChore master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore>
  {
    public StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.TargetParameter attackTarget;
    public StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.TargetParameter attacker;
    public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.ApproachSubState<RangedAttackable> approachtarget;
    public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State attack;
    public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approachtarget;
      this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fighting, (Func<AttackChore.StatesInstance, object>) (smi => (object) smi.master.gameObject)).EventHandler(GameHashes.TargetLost, (StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State.Callback) (smi => smi.master.Fail("target lost"))).Enter((StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State.Callback) (smi => smi.master.GetComponent<Weapon>().Configure(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0.0f)));
      this.approachtarget.InitializeStates(this.attacker, this.attackTarget, this.attack, (GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State) null, (CellOffset[]) null, NavigationTactics.Range_3_ProhibitOverlap).Enter((StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State.Callback) (smi =>
      {
        smi.master.CleanUpMultitool();
        smi.master.Trigger(1039067354, (object) this.attackTarget.Get(smi));
        Health component = this.attackTarget.Get(smi).GetComponent<Health>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && !component.IsDefeated())
          return;
        smi.StopSM("target defeated");
      }));
      this.attack.Target(this.attacker).Enter((StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State.Callback) (smi =>
      {
        this.attackTarget.Get(smi).Subscribe(1088554450, new System.Action<object>(smi.master.OnTargetMoved));
        if (this.attackTarget != null && smi.master.multiTool == null)
        {
          smi.master.multiTool = new MultitoolController.Instance(this.attackTarget.Get(smi).GetComponent<Workable>(), smi.master.GetComponent<Worker>(), (HashedString) "attack", Assets.GetPrefab((Tag) EffectConfigs.AttackSplashId));
          smi.master.multiTool.StartSM();
        }
        this.attackTarget.Get(smi).Subscribe(1969584890, new System.Action<object>(smi.master.OnTargetDestroyed));
        smi.ScheduleGoTo(0.5f, (StateMachine.BaseState) this.success);
      })).Update((System.Action<AttackChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.multiTool == null)
          return;
        smi.master.multiTool.UpdateHitEffectTarget();
      }), UpdateRate.SIM_200ms, false).Exit((StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) this.attackTarget.Get(smi) != (UnityEngine.Object) null))
          return;
        this.attackTarget.Get(smi).Unsubscribe(1088554450, new System.Action<object>(smi.master.OnTargetMoved));
      }));
      this.success.Enter("finishAttack", (StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State.Callback) (smi =>
      {
        if ((UnityEngine.Object) this.attackTarget.Get(smi) != (UnityEngine.Object) null)
        {
          Transform transform = this.attackTarget.Get(smi).transform;
          Weapon component1 = this.attacker.Get(smi).gameObject.GetComponent<Weapon>();
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          {
            component1.AttackTarget(transform.gameObject);
            Health component2 = this.attackTarget.Get(smi).GetComponent<Health>();
            if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
              return;
            if (!component2.IsDefeated())
            {
              smi.GoTo((StateMachine.BaseState) this.attack);
            }
            else
            {
              smi.master.CleanUpMultitool();
              smi.StopSM("target defeated");
            }
          }
          else
          {
            smi.master.CleanUpMultitool();
            smi.StopSM("no weapon");
          }
        }
        else
        {
          smi.master.CleanUpMultitool();
          smi.StopSM("no target");
        }
      })).ReturnSuccess();
    }
  }
}
