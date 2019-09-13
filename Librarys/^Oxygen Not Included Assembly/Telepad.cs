// Decompiled with JetBrains decompiler
// Type: Telepad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
  public static readonly HashedString[] PortalBirthAnim = new HashedString[1]
  {
    (HashedString) "portalbirth"
  };
  [MyCmpReq]
  private KSelectable selectable;
  private MeterController meter;
  private const float MAX_IMMIGRATION_TIME = 120f;
  private const int NUM_METER_NOTCHES = 8;
  private List<MinionStartingStats> minionStats;
  public float startingSkillPoints;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<Deconstructable>().allowDeconstruction = false;
    int x = 0;
    int y = 0;
    Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) this), out x, out y);
    if (x == 0)
      Debug.LogError((object) ("Headquarters spawned at: (" + x.ToString() + "," + y.ToString() + ")"));
    if (!((UnityEngine.Object) GameUtil.GetTelepad() != (UnityEngine.Object) null))
      return;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, string.Format((string) BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.UNIQUE_POPTEXT, (object) this.GetProperName()), (Transform) null, this.transform.GetPosition(), 1.5f, false, false);
    Util.KDestroyGameObject(this.gameObject);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Telepads.Add(this);
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
    this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    Components.Telepads.Remove(this);
    base.OnCleanUp();
  }

  public void Update()
  {
    if (this.smi.IsColonyLost())
      return;
    if (Immigration.Instance.ImmigrantsAvailable)
    {
      this.smi.sm.openPortal.Trigger(this.smi);
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, (object) this);
    }
    else
    {
      this.smi.sm.closePortal.Trigger(this.smi);
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, (object) this);
    }
    if ((double) this.GetTimeRemaining() >= -120.0)
      return;
    Messenger.Instance.QueueMessage((Message) new DuplicantsLeftMessage());
    Immigration.Instance.EndImmigration();
  }

  public void RejectAll()
  {
    Immigration.Instance.EndImmigration();
    this.smi.sm.closePortal.Trigger(this.smi);
  }

  public void OnAcceptDelivery(ITelepadDeliverable delivery)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    Immigration.Instance.EndImmigration();
    GameObject go = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
    MinionIdentity component1 = go.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), string.Format((string) UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, (object) DUPLICANTS.CHORES.NOT_EXISTING_TASK), go.GetProperName());
      foreach (Component component2 in Components.LiveMinionIdentities.Items)
        component2.GetComponent<Effects>().Add("NewCrewArrival", true);
      MinionResume component3 = component1.GetComponent<MinionResume>();
      for (int index = 0; (double) index < (double) this.startingSkillPoints; ++index)
        component3.ForceAddSkillPoint();
    }
    this.smi.sm.closePortal.Trigger(this.smi);
  }

  public float GetTimeRemaining()
  {
    return Immigration.Instance.GetTimeRemaining();
  }

  public class StatesInstance : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance
  {
    public StatesInstance(Telepad master)
      : base(master)
    {
    }

    public bool IsColonyLost()
    {
      if ((UnityEngine.Object) GameFlowManager.Instance != (UnityEngine.Object) null)
        return GameFlowManager.Instance.IsGameOver();
      return false;
    }

    public void UpdateMeter()
    {
      this.master.meter.SetPositionPercent(Mathf.Clamp01((float) (1.0 - (double) Immigration.Instance.GetTimeRemaining() / (double) Immigration.Instance.GetTotalWaitTime())));
    }
  }

  public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
  {
    private static readonly HashedString[] workingAnims = new HashedString[2]
    {
      (HashedString) "working_loop",
      (HashedString) "working_pst"
    };
    public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;
    public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;
    public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.serializable = true;
      this.root.OnSignal(this.idlePortal, this.resetToIdle);
      this.resetToIdle.GoTo(this.idle);
      this.idle.Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.UpdateMeter())).Update("TelepadMeter", (System.Action<Telepad.StatesInstance, float>) ((smi, dt) => smi.UpdateMeter()), UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OperationalChanged, this.unoperational, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
      this.unoperational.PlayAnim("idle").Enter("StopImmigration", (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi =>
      {
        Immigration.Instance.Stop();
        smi.master.meter.SetPositionPercent(0.0f);
      })).Exit("StartImmigration", (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => Immigration.Instance.Restart())).EventTransition(GameHashes.OperationalChanged, this.idle, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.opening.Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.master.meter.SetPositionPercent(1f))).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
      this.open.OnSignal(this.closePortal, this.close).Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.master.meter.SetPositionPercent(1f))).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => smi.IsColonyLost()), UpdateRate.SIM_200ms).EventTransition(GameHashes.OperationalChanged, this.close, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.close.Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.master.meter.SetPositionPercent(0.0f))).PlayAnims((Func<Telepad.StatesInstance, HashedString[]>) (smi => Telepad.States.workingAnims), KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
    }
  }
}
