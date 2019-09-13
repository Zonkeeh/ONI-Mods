// Decompiled with JetBrains decompiler
// Type: Flatulence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

[SkipSaveFileSerialization]
public class Flatulence : StateMachineComponent<Flatulence.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<Flatulence> OnDeathDelegate = new EventSystem.IntraObjectHandler<Flatulence>((System.Action<Flatulence, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Flatulence> OnRevivedDelegate = new EventSystem.IntraObjectHandler<Flatulence>((System.Action<Flatulence, object>) ((component, data) => component.OnRevived(data)));
  private static readonly HashedString[] WorkLoopAnims = new HashedString[3]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop",
    (HashedString) "working_pst"
  };
  private const float EmitMass = 0.1f;
  private const SimHashes EmitElement = SimHashes.Methane;
  private const float EmissionRadius = 1.5f;
  private const float MaxDistanceSq = 2.25f;

  protected override void OnPrefabInit()
  {
    this.Subscribe<Flatulence>(1623392196, Flatulence.OnDeathDelegate);
    this.Subscribe<Flatulence>(-1117766961, Flatulence.OnRevivedDelegate);
  }

  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  private void Emit(object data)
  {
    GameObject gameObject = (GameObject) data;
    float temperature = Db.Get().Amounts.Temperature.Lookup((Component) this).value;
    Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
    if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
    {
      equippable.GetComponent<Storage>().AddGasChunk(SimHashes.Methane, 0.1f, temperature, byte.MaxValue, 0, false, true);
    }
    else
    {
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
            minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
          }
        }
      }
      SimMessages.AddRemoveSubstance(Grid.PosToCell(gameObject.transform.GetPosition()), SimHashes.Methane, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.1f, temperature, byte.MaxValue, 0, true, -1);
      KBatchedAnimController effect = FXHelpers.CreateEffect("odor_fx_kanim", gameObject.transform.GetPosition(), gameObject.transform, true, Grid.SceneLayer.Front, false);
      effect.Play(Flatulence.WorkLoopAnims, KAnim.PlayMode.Once);
      effect.destroyOnAnimComplete = true;
    }
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

  public class StatesInstance : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.GameInstance
  {
    public StatesInstance(Flatulence master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence>
  {
    public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State idle;
    public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State emit;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.Enter("ScheduleNextFart", (StateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State.Callback) (smi => smi.ScheduleGoTo(this.GetNewInterval(), (StateMachine.BaseState) this.emit)));
      this.emit.Enter("Fart", (StateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State.Callback) (smi => smi.master.Emit((object) smi.master.gameObject))).ToggleExpression(Db.Get().Expressions.Relief, (Func<Flatulence.StatesInstance, bool>) null).ScheduleGoTo(3f, (StateMachine.BaseState) this.idle);
    }

    private float GetNewInterval()
    {
      return Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.FLATULENCE_EMIT_INTERVAL_MAX - TRAITS.FLATULENCE_EMIT_INTERVAL_MIN, 1f), TRAITS.FLATULENCE_EMIT_INTERVAL_MIN), TRAITS.FLATULENCE_EMIT_INTERVAL_MAX);
    }
  }
}
