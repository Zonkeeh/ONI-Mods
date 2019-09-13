// Decompiled with JetBrains decompiler
// Type: Stinky
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

[SkipSaveFileSerialization]
public class Stinky : StateMachineComponent<Stinky.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<Stinky> OnDeathDelegate = new EventSystem.IntraObjectHandler<Stinky>((System.Action<Stinky, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Stinky> OnRevivedDelegate = new EventSystem.IntraObjectHandler<Stinky>((System.Action<Stinky, object>) ((component, data) => component.OnRevived(data)));
  private static readonly HashedString[] WorkLoopAnims = new HashedString[3]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop",
    (HashedString) "working_pst"
  };
  private const float EmitMass = 0.0025f;
  private const SimHashes EmitElement = SimHashes.ContaminatedOxygen;
  private const float EmissionRadius = 1.5f;
  private const float MaxDistanceSq = 2.25f;
  private KBatchedAnimController stinkyController;

  protected override void OnPrefabInit()
  {
    this.Subscribe<Stinky>(1623392196, Stinky.OnDeathDelegate);
    this.Subscribe<Stinky>(-1117766961, Stinky.OnRevivedDelegate);
  }

  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  private void Emit(object data)
  {
    GameObject gameObject = (GameObject) data;
    Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
    Vector2 position1 = (Vector2) gameObject.transform.GetPosition();
    for (int index = 0; index < minionIdentities.Count; ++index)
    {
      MinionIdentity minionIdentity = minionIdentities[index];
      if ((UnityEngine.Object) minionIdentity.gameObject != (UnityEngine.Object) gameObject.gameObject)
      {
        Vector2 position2 = (Vector2) minionIdentity.transform.GetPosition();
        if ((double) Vector2.SqrMagnitude(position1 - position2) <= 2.25)
        {
          minionIdentity.Trigger(508119890, (object) Strings.Get("STRINGS.DUPLICANTS.DISEASES.PUTRIDODOUR.CRINGE_EFFECT").String);
          minionIdentity.GetComponent<Effects>().Add("SmelledStinky", true);
          minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
        }
      }
    }
    int cell = Grid.PosToCell(gameObject.transform.GetPosition());
    float temperature = Db.Get().Amounts.Temperature.Lookup((Component) this).value;
    SimMessages.AddRemoveSubstance(cell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.0025f, temperature, byte.MaxValue, 0, true, -1);
    KFMOD.PlayOneShot(GlobalAssets.GetSound("Dupe_Flatulence", false), this.transform.GetPosition());
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

  public class StatesInstance : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.GameInstance
  {
    public StatesInstance(Stinky master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky>
  {
    public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State idle;
    public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State emit;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.Enter((StateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State.Callback) (smi =>
      {
        KBatchedAnimController effect = FXHelpers.CreateEffect("odor_fx_kanim", smi.master.gameObject.transform.GetPosition(), smi.master.gameObject.transform, true, Grid.SceneLayer.Front, false);
        effect.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
        smi.master.stinkyController = effect;
      })).Update("StinkyFX", (System.Action<Stinky.StatesInstance, float>) ((smi, dt) =>
      {
        if (!((UnityEngine.Object) smi.master.stinkyController != (UnityEngine.Object) null))
          return;
        smi.master.stinkyController.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
      }), UpdateRate.SIM_4000ms, false);
      this.idle.Enter("ScheduleNextFart", (StateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State.Callback) (smi => smi.ScheduleGoTo(this.GetNewInterval(), (StateMachine.BaseState) this.emit)));
      this.emit.Enter("Fart", (StateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State.Callback) (smi => smi.master.Emit((object) smi.master.gameObject))).ToggleExpression(Db.Get().Expressions.Relief, (Func<Stinky.StatesInstance, bool>) null).ScheduleGoTo(3f, (StateMachine.BaseState) this.idle);
    }

    private float GetNewInterval()
    {
      return Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.STINKY_EMIT_INTERVAL_MAX - TRAITS.STINKY_EMIT_INTERVAL_MIN, 1f), TRAITS.STINKY_EMIT_INTERVAL_MIN), TRAITS.STINKY_EMIT_INTERVAL_MAX);
    }
  }
}
