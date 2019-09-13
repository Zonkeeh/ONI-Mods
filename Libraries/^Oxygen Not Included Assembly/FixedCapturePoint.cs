// Decompiled with JetBrains decompiler
// Type: FixedCapturePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
  private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;
  public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State unoperational;
  public FixedCapturePoint.OperationalState operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.operational;
    this.serializable = true;
    this.unoperational.TagTransition(GameTags.Operational, (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.operational, false);
    this.operational.DefaultState(this.operational.manual).TagTransition(GameTags.Operational, this.unoperational, true);
    this.operational.manual.ParamTransition<bool>((StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Parameter<bool>) this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
    this.operational.automated.ParamTransition<bool>((StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Parameter<bool>) this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((Func<FixedCapturePoint.Instance, Chore>) (smi => smi.CreateChore()), this.unoperational, this.unoperational).Update("FindFixedCapturable", (System.Action<FixedCapturePoint.Instance, float>) ((smi, dt) => smi.FindFixedCapturable()), UpdateRate.SIM_1000ms, false);
  }

  public class Def : StateMachine.BaseDef
  {
    public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = (Func<FixedCapturePoint.Instance, int>) (smi =>
    {
      int cell = Grid.PosToCell((StateMachine.Instance) smi);
      Navigator component = smi.targetCapturable.GetComponent<Navigator>();
      if (Grid.IsValidCell(cell - 1) && component.CanReach(cell - 1))
        return cell - 1;
      if (Grid.IsValidCell(cell + 1) && component.CanReach(cell + 1))
        return cell + 1;
      return cell;
    });
    public Func<GameObject, FixedCapturePoint.Instance, bool> isCreatureEligibleToBeCapturedCb;
  }

  public class OperationalState : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
  {
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class Instance : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance, ICheckboxControl
  {
    public Instance(IStateMachineTarget master, FixedCapturePoint.Def def)
      : base(master, def)
    {
      this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettings));
    }

    public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

    public bool shouldCreatureGoGetCaptured { get; private set; }

    private void OnCopySettings(object data)
    {
      GameObject go = (GameObject) data;
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
        return;
      FixedCapturePoint.Instance smi = go.GetSMI<FixedCapturePoint.Instance>();
      if (smi == null)
        return;
      this.sm.automated.Set(this.sm.automated.Get(smi), this);
    }

    public Chore CreateChore()
    {
      this.FindFixedCapturable();
      return (Chore) new FixedCaptureChore(this.GetComponent<KPrefabID>());
    }

    public bool IsCreatureAvailableForFixedCapture()
    {
      if (this.targetCapturable.IsNullOrStopped())
        return false;
      int cell = Grid.PosToCell(this.transform.GetPosition());
      return FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, Game.Instance.roomProber.GetCavityForCell(cell), cell);
    }

    public void SetRancherIsAvailableForCapturing()
    {
      this.shouldCreatureGoGetCaptured = true;
    }

    public void ClearRancherIsAvailableForCapturing()
    {
      this.shouldCreatureGoGetCaptured = false;
    }

    private static bool CanCapturableBeCapturedAtCapturePoint(
      FixedCapturableMonitor.Instance capturable,
      FixedCapturePoint.Instance capture_point,
      CavityInfo capture_cavity_info,
      int capture_cell)
    {
      if (!capturable.IsRunning() || capturable.HasTag(GameTags.Creatures.Bagged) || capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped() || (capture_point.def.isCreatureEligibleToBeCapturedCb != null && !capture_point.def.isCreatureEligibleToBeCapturedCb(capturable.gameObject, capture_point) || !capturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>()))
        return false;
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(capturable.transform.GetPosition()));
      if (cavityForCell == null || cavityForCell != capture_cavity_info || capturable.GetComponent<Navigator>().GetNavigationCost(capture_cell) == -1)
        return false;
      TreeFilterable component1 = capture_point.GetComponent<TreeFilterable>();
      IUserControlledCapacity component2 = capture_point.GetComponent<IUserControlledCapacity>();
      return !component1.ContainsTag(capturable.GetComponent<KPrefabID>().PrefabTag) || (double) component2.AmountStored > (double) component2.UserMaxCapacity;
    }

    public void FindFixedCapturable()
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
      if (cavityForCell == null)
      {
        this.ResetCapturePoint();
      }
      else
      {
        if (!this.targetCapturable.IsNullOrStopped() && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, cell))
          this.ResetCapturePoint();
        if (!this.targetCapturable.IsNullOrStopped())
          return;
        FixedCapturePoint.Instance.CapturableIterator iterator = new FixedCapturePoint.Instance.CapturableIterator(this, cavityForCell, cell);
        GameScenePartitioner.Instance.Iterate<FixedCapturePoint.Instance.CapturableIterator>(cavityForCell.minX, cavityForCell.minY, cavityForCell.maxX - cavityForCell.minX + 1, cavityForCell.maxY - cavityForCell.minY + 1, GameScenePartitioner.Instance.collisionLayer, ref iterator);
        iterator.Cleanup();
        this.targetCapturable = iterator.result;
        if (this.targetCapturable.IsNullOrStopped())
          return;
        this.targetCapturable.targetCapturePoint = this;
      }
    }

    public void ResetCapturePoint()
    {
      this.Trigger(643180843, (object) null);
      if (this.targetCapturable.IsNullOrStopped())
        return;
      this.targetCapturable.targetCapturePoint = (FixedCapturePoint.Instance) null;
      this.targetCapturable.Trigger(1034952693, (object) null);
      this.targetCapturable = (FixedCapturableMonitor.Instance) null;
    }

    string ICheckboxControl.CheckboxTitleKey
    {
      get
      {
        return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;
      }
    }

    string ICheckboxControl.CheckboxLabel
    {
      get
      {
        return (string) UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;
      }
    }

    string ICheckboxControl.CheckboxTooltip
    {
      get
      {
        return (string) UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;
      }
    }

    bool ICheckboxControl.GetCheckboxValue()
    {
      return this.sm.automated.Get(this);
    }

    void ICheckboxControl.SetCheckboxValue(bool value)
    {
      this.sm.automated.Set(value, this);
    }

    private struct CapturableIterator : GameScenePartitioner.Iterator
    {
      private CavityInfo captureCavityInfo;
      private int captureCell;
      private FixedCapturePoint.Instance capturePoint;

      public CapturableIterator(
        FixedCapturePoint.Instance capture_point,
        CavityInfo capture_cavity_info,
        int capture_cell)
      {
        this.capturePoint = capture_point;
        this.captureCavityInfo = capture_cavity_info;
        this.captureCell = capture_cell;
        this.result = (FixedCapturableMonitor.Instance) null;
      }

      public FixedCapturableMonitor.Instance result { get; private set; }

      public void Iterate(object target_obj)
      {
        KMonoBehaviour cmp = target_obj as KMonoBehaviour;
        if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
          return;
        FixedCapturableMonitor.Instance smi = cmp.GetSMI<FixedCapturableMonitor.Instance>();
        if (smi == null || !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(smi, this.capturePoint, this.captureCavityInfo, this.captureCell))
          return;
        this.result = smi;
      }

      public void Cleanup()
      {
      }
    }
  }
}
