// Decompiled with JetBrains decompiler
// Type: RocketEngine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;

[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance>, IEffectDescriptor
{
  public float exhaustEmitRate = 50f;
  public float exhaustTemperature = 1500f;
  public SimHashes exhaustElement = SimHashes.CarbonDioxide;
  public float efficiency = 1f;
  public bool requireOxidizer = true;
  public bool mainEngine = true;
  public SpawnFXHashes explosionEffectHash;
  public Tag fuelTag;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    if (!this.mainEngine)
      return;
    this.GetComponent<RocketModule>().AddLaunchCondition((RocketLaunchCondition) new RequireAttachedComponent(this.gameObject.GetComponent<AttachableBuilding>(), typeof (FuelTank), (string) UI.STARMAP.COMPONENT.FUEL_TANK));
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return (List<Descriptor>) null;
  }

  public class StatesInstance : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.GameInstance
  {
    public StatesInstance(RocketEngine smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine>
  {
    public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State idle;
    public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burning;
    public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burnComplete;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, (StateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.Transition.ConditionCallback) null);
      this.burning.EventTransition(GameHashes.LandRocket, this.burnComplete, (StateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.Transition.ConditionCallback) null).PlayAnim("launch_pre").QueueAnim("launch_loop", true, (Func<RocketEngine.StatesInstance, string>) null).Update((System.Action<RocketEngine.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
        if (Grid.IsValidCell(cell))
          SimMessages.EmitMass(cell, (byte) ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, (byte) 0, 0, -1);
        int num1 = 10;
        for (int index = 1; index < num1; ++index)
        {
          int num2 = Grid.OffsetCell(cell, -1, -index);
          int num3 = Grid.OffsetCell(cell, 0, -index);
          int num4 = Grid.OffsetCell(cell, 1, -index);
          if (Grid.IsValidCell(num2))
            SimMessages.ModifyEnergy(num2, smi.master.exhaustTemperature / (float) (index + 1), 3200f, SimMessages.EnergySourceID.Burner);
          if (Grid.IsValidCell(num3))
            SimMessages.ModifyEnergy(num3, smi.master.exhaustTemperature / (float) index, 3200f, SimMessages.EnergySourceID.Burner);
          if (Grid.IsValidCell(num4))
            SimMessages.ModifyEnergy(num4, smi.master.exhaustTemperature / (float) (index + 1), 3200f, SimMessages.EnergySourceID.Burner);
        }
      }), UpdateRate.SIM_200ms, false);
      this.burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, (StateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.Transition.ConditionCallback) null);
    }
  }
}
