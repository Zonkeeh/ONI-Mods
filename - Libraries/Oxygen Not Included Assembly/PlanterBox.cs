// Decompiled with JetBrains decompiler
// Type: PlanterBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
public class PlanterBox : StateMachineComponent<PlanterBox.SMInstance>
{
  [MyCmpReq]
  private PlantablePlot plantablePlot;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class SMInstance : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.GameInstance
  {
    public SMInstance(PlanterBox master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox>
  {
    public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State empty;
    public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State full;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (StateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.Transition.ConditionCallback) (smi => (Object) smi.master.plantablePlot.Occupant != (Object) null)).PlayAnim("off");
      this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (StateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.Transition.ConditionCallback) (smi => (Object) smi.master.plantablePlot.Occupant == (Object) null)).PlayAnim("on");
    }
  }
}
