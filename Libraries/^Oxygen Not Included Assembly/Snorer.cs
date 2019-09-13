// Decompiled with JetBrains decompiler
// Type: Snorer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

[SkipSaveFileSerialization]
public class Snorer : StateMachineComponent<Snorer.StatesInstance>
{
  private static readonly HashedString HeadHash = (HashedString) "snapTo_mouth";
  private static readonly EventSystem.IntraObjectHandler<Snorer> OnDeathDelegate = new EventSystem.IntraObjectHandler<Snorer>((System.Action<Snorer, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Snorer> OnRevivedDelegate = new EventSystem.IntraObjectHandler<Snorer>((System.Action<Snorer, object>) ((component, data) => component.OnRevived(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<Snorer>(1623392196, Snorer.OnDeathDelegate);
    this.Subscribe<Snorer>(-1117766961, Snorer.OnRevivedDelegate);
  }

  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  private void OnDeath(object data)
  {
    this.enabled = false;
  }

  private void OnRevived(object data)
  {
    this.enabled = true;
  }

  public void ModifyTrait(Trait t)
  {
  }

  public class StatesInstance : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.GameInstance
  {
    private SchedulerHandle snoreHandle;
    private KBatchedAnimController snoreEffect;
    private KBatchedAnimController snoreBGEffect;
    private const float BGEmissionRadius = 3f;

    public StatesInstance(Snorer master)
      : base(master)
    {
    }

    public bool IsSleeping()
    {
      StaminaMonitor.Instance smi = this.master.GetSMI<StaminaMonitor.Instance>();
      if (smi != null)
        return smi.IsSleeping();
      return false;
    }

    public void StartSmallSnore()
    {
      this.snoreHandle = GameScheduler.Instance.Schedule("snorelines", 2f, new System.Action<object>(this.StartSmallSnoreInternal), (object) null, (SchedulerGroup) null);
    }

    private void StartSmallSnoreInternal(object data)
    {
      this.snoreHandle.ClearScheduler();
      bool symbolVisible;
      Matrix4x4 symbolTransform = this.smi.master.GetComponent<KBatchedAnimController>().GetSymbolTransform(Snorer.HeadHash, out symbolVisible);
      if (!symbolVisible)
        return;
      Vector3 column = (Vector3) symbolTransform.GetColumn(3);
      column.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
      this.snoreEffect = FXHelpers.CreateEffect("snore_fx_kanim", column, (Transform) null, false, Grid.SceneLayer.Front, false);
      this.snoreEffect.destroyOnAnimComplete = true;
      this.snoreEffect.Play((HashedString) "snore", KAnim.PlayMode.Loop, 1f, 0.0f);
    }

    public void StopSmallSnore()
    {
      this.snoreHandle.ClearScheduler();
      if ((UnityEngine.Object) this.snoreEffect != (UnityEngine.Object) null)
        this.snoreEffect.PlayMode = KAnim.PlayMode.Once;
      this.snoreEffect = (KBatchedAnimController) null;
    }

    public void StartSnoreBGEffect()
    {
      AcousticDisturbance.Emit((object) this.smi.master.gameObject, 3);
    }

    public void StopSnoreBGEffect()
    {
    }
  }

  public class States : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer>
  {
    public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State idle;
    public Snorer.States.SleepStates sleeping;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.Transition((GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State) this.sleeping, (StateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.Transition.ConditionCallback) (smi => smi.IsSleeping()), UpdateRate.SIM_200ms);
      this.sleeping.DefaultState(this.sleeping.quiet).Enter((StateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State.Callback) (smi => smi.StartSmallSnore())).Exit((StateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State.Callback) (smi => smi.StopSmallSnore())).Transition(this.idle, (StateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.Transition.ConditionCallback) (smi => !smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping()), UpdateRate.SIM_200ms);
      this.sleeping.quiet.Enter("ScheduleNextSnore", (StateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State.Callback) (smi => smi.ScheduleGoTo(this.GetNewInterval(), (StateMachine.BaseState) this.sleeping.snoring)));
      this.sleeping.snoring.Enter((StateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State.Callback) (smi => smi.StartSnoreBGEffect())).ToggleExpression(Db.Get().Expressions.Relief, (Func<Snorer.StatesInstance, bool>) null).ScheduleGoTo(3f, (StateMachine.BaseState) this.sleeping.quiet).Exit((StateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State.Callback) (smi => smi.StopSnoreBGEffect()));
    }

    private float GetNewInterval()
    {
      return Mathf.Min(Mathf.Max(Util.GaussianRandom(5f, 1f), 3f), 10f);
    }

    public class SleepStates : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State
    {
      public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State quiet;
      public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State snoring;
    }
  }

  private struct CellInfo
  {
    public int cell;
    public int depth;

    public override int GetHashCode()
    {
      return this.cell;
    }

    public override bool Equals(object obj)
    {
      return this.cell == ((Snorer.CellInfo) obj).cell;
    }
  }
}
