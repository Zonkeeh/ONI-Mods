// Decompiled with JetBrains decompiler
// Type: NearbyCreatureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class NearbyCreatureMonitor : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Update("UpdateNearbyCreatures", (System.Action<NearbyCreatureMonitor.Instance, float>) ((smi, dt) => smi.UpdateNearbyCreatures(dt)), UpdateRate.SIM_1000ms, false);
  }

  public class Instance : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public event System.Action<float, List<KPrefabID>> OnUpdateNearbyCreatures;

    public void UpdateNearbyCreatures(float dt)
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.gameObject));
      if (cavityForCell == null)
        return;
      this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures);
    }
  }
}
