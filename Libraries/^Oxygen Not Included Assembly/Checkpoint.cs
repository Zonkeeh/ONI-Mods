// Decompiled with JetBrains decompiler
// Type: Checkpoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class Checkpoint : StateMachineComponent<Checkpoint.SMInstance>
{
  public static readonly HashedString PORT_ID = (HashedString) nameof (Checkpoint);
  private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>((System.Action<Checkpoint, object>) ((component, data) => component.OnLogicValueChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>((System.Action<Checkpoint, object>) ((component, data) => component.OnOperationalChanged(data)));
  private bool statusDirty = true;
  [MyCmpReq]
  public Operational operational;
  [MyCmpReq]
  private KSelectable selectable;
  private static StatusItem infoStatusItem_Logic;
  private Checkpoint.CheckpointReactable reactable;
  private bool hasLogicWire;
  private bool hasInputHigh;
  private bool redLight;

  private bool RedLightDesiredState
  {
    get
    {
      if (this.hasLogicWire && !this.hasInputHigh)
        return this.operational.IsOperational;
      return false;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Checkpoint>(-801688580, Checkpoint.OnLogicValueChangedDelegate);
    this.Subscribe<Checkpoint>(-592767678, Checkpoint.OnOperationalChangedDelegate);
    this.smi.StartSM();
    if (Checkpoint.infoStatusItem_Logic == null)
    {
      Checkpoint.infoStatusItem_Logic = new StatusItem("CheckpointLogic", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      StatusItem infoStatusItemLogic = Checkpoint.infoStatusItem_Logic;
      // ISSUE: reference to a compiler-generated field
      if (Checkpoint.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Checkpoint.\u003C\u003Ef__mg\u0024cache0 = new Func<string, object, string>(Checkpoint.ResolveInfoStatusItem_Logic);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, object, string> fMgCache0 = Checkpoint.\u003C\u003Ef__mg\u0024cache0;
      infoStatusItemLogic.resolveStringCallback = fMgCache0;
    }
    this.Refresh(this.redLight);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.ClearReactable();
  }

  public void RefreshLight()
  {
    if (this.redLight != this.RedLightDesiredState)
    {
      this.Refresh(this.RedLightDesiredState);
      this.statusDirty = true;
    }
    if (!this.statusDirty)
      return;
    this.RefreshStatusItem();
  }

  private LogicCircuitNetwork GetNetwork()
  {
    return Game.Instance.logicCircuitManager.GetNetworkForCell(this.GetComponent<LogicPorts>().GetPortCell(Checkpoint.PORT_ID));
  }

  private static string ResolveInfoStatusItem_Logic(string format_str, object data)
  {
    return (string) (!((Checkpoint) data).RedLight ? BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_OPEN : BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_CLOSED);
  }

  private void CreateNewReactable()
  {
    if (this.reactable != null)
      return;
    this.reactable = new Checkpoint.CheckpointReactable(this);
  }

  private void OrphanReactable()
  {
    this.reactable = (Checkpoint.CheckpointReactable) null;
  }

  private void ClearReactable()
  {
    if (this.reactable == null)
      return;
    this.reactable.Cleanup();
    this.reactable = (Checkpoint.CheckpointReactable) null;
  }

  public bool RedLight
  {
    get
    {
      return this.redLight;
    }
  }

  private void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (!(logicValueChanged.portID == Checkpoint.PORT_ID))
      return;
    this.hasInputHigh = logicValueChanged.newValue > 0;
    this.hasLogicWire = this.GetNetwork() != null;
    this.statusDirty = true;
  }

  private void OnOperationalChanged(object data)
  {
    this.statusDirty = true;
  }

  private void RefreshStatusItem()
  {
    bool on = this.operational.IsOperational && this.hasLogicWire;
    this.selectable.ToggleStatusItem(Checkpoint.infoStatusItem_Logic, on, (object) this);
    this.statusDirty = false;
  }

  private void Refresh(bool redLightState)
  {
    this.redLight = redLightState;
    this.operational.SetActive(this.operational.IsOperational && this.redLight, false);
    this.smi.sm.redLight.Set(this.redLight, this.smi);
    if (this.redLight)
      this.CreateNewReactable();
    else
      this.ClearReactable();
  }

  private class CheckpointReactable : Reactable
  {
    private Checkpoint checkpoint;
    private Navigator reactor_navigator;
    private bool rotated;

    public CheckpointReactable(Checkpoint checkpoint)
      : base(checkpoint.gameObject, (HashedString) nameof (CheckpointReactable), Db.Get().ChoreTypes.Checkpoint, 1, 1, false, 0.0f, 0.0f, float.PositiveInfinity)
    {
      this.checkpoint = checkpoint;
      this.rotated = this.gameObject.GetComponent<Rotatable>().IsRotated;
      this.preventChoreInterruption = false;
    }

    public override bool InternalCanBegin(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
        return false;
      if ((UnityEngine.Object) this.checkpoint == (UnityEngine.Object) null)
      {
        this.Cleanup();
        return false;
      }
      if (!this.checkpoint.RedLight)
        return false;
      if (this.rotated)
        return transition.x < 0;
      return transition.x > 0;
    }

    protected override void InternalBegin()
    {
      this.reactor_navigator = this.reactor.GetComponent<Navigator>();
      KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
      component.AddAnimOverrides(Assets.GetAnim((HashedString) "anim_idle_distracted_kanim"), 1f);
      component.Play((HashedString) "idle_pre", KAnim.PlayMode.Once, 1f, 0.0f);
      component.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop, 1f, 0.0f);
      this.checkpoint.OrphanReactable();
      this.checkpoint.CreateNewReactable();
    }

    public override void Update(float dt)
    {
      if ((UnityEngine.Object) this.checkpoint == (UnityEngine.Object) null || !this.checkpoint.RedLight || (UnityEngine.Object) this.reactor_navigator == (UnityEngine.Object) null)
      {
        this.Cleanup();
      }
      else
      {
        this.reactor_navigator.AdvancePath(false);
        if (!this.reactor_navigator.path.IsValid())
        {
          this.Cleanup();
        }
        else
        {
          NavGrid.Transition nextTransition = this.reactor_navigator.GetNextTransition();
          if (!this.rotated ? nextTransition.x > (sbyte) 0 : nextTransition.x < (sbyte) 0)
            return;
          this.Cleanup();
        }
      }
    }

    protected override void InternalEnd()
    {
      if (!((UnityEngine.Object) this.reactor != (UnityEngine.Object) null))
        return;
      this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim((HashedString) "anim_idle_distracted_kanim"));
    }

    protected override void InternalCleanup()
    {
    }
  }

  public class SMInstance : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.GameInstance
  {
    public SMInstance(Checkpoint master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint>
  {
    public StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.BoolParameter redLight;
    public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State stop;
    public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State go;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.go;
      this.root.Update("RefreshLight", (System.Action<Checkpoint.SMInstance, float>) ((smi, dt) => smi.master.RefreshLight()), UpdateRate.SIM_200ms, false);
      this.stop.ParamTransition<bool>((StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.Parameter<bool>) this.redLight, this.go, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsFalse).PlayAnim("red_light");
      this.go.ParamTransition<bool>((StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.Parameter<bool>) this.redLight, this.stop, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsTrue).PlayAnim("green_light");
    }
  }
}
