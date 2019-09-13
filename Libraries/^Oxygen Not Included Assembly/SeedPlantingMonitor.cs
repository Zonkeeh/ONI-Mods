// Decompiled with JetBrains decompiler
// Type: SeedPlantingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SeedPlantingMonitor : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.State root = this.root;
    Tag wantsToPlantSeed = GameTags.Creatures.WantsToPlantSeed;
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.Transition.ConditionCallback(SeedPlantingMonitor.ShouldSearchForSeeds);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.Transition.ConditionCallback fMgCache0 = SeedPlantingMonitor.\u003C\u003Ef__mg\u0024cache0;
    System.Action<SeedPlantingMonitor.Instance> on_complete = (System.Action<SeedPlantingMonitor.Instance>) (smi => smi.RefreshSearchTime());
    root.ToggleBehaviour(wantsToPlantSeed, fMgCache0, on_complete);
  }

  public static bool ShouldSearchForSeeds(SeedPlantingMonitor.Instance smi)
  {
    return (double) Time.time >= (double) smi.nextSearchTime;
  }

  public class Def : StateMachine.BaseDef
  {
    public float searchMinInterval = 60f;
    public float searchMaxInterval = 300f;
  }

  public class Instance : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.GameInstance
  {
    public float nextSearchTime;

    public Instance(IStateMachineTarget master, SeedPlantingMonitor.Def def)
      : base(master, def)
    {
      this.RefreshSearchTime();
    }

    public void RefreshSearchTime()
    {
      this.nextSearchTime = Time.time + Mathf.Lerp(this.def.searchMinInterval, this.def.searchMaxInterval, UnityEngine.Random.value);
    }
  }
}
