// Decompiled with JetBrains decompiler
// Type: FishOvercrowdingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FishOvercrowdingMonitor : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>
{
  public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State satisfied;
  public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State overcrowded;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (FishOvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FishOvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Register);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback fMgCache0 = FishOvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State state = root.Enter(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (FishOvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FishOvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Unregister);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback fMgCache1 = FishOvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache1;
    state.Exit(fMgCache1);
    this.satisfied.DoNothing();
    this.overcrowded.DoNothing();
  }

  private static void Register(FishOvercrowdingMonitor.Instance smi)
  {
    FishOvercrowingManager.Instance.Add(smi);
  }

  private static void Unregister(FishOvercrowdingMonitor.Instance smi)
  {
    FishOvercrowingManager instance = FishOvercrowingManager.Instance;
    if ((Object) instance == (Object) null)
      return;
    instance.Remove(smi);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.GameInstance
  {
    public int cellCount;
    public int fishCount;

    public Instance(IStateMachineTarget master, FishOvercrowdingMonitor.Def def)
      : base(master, def)
    {
    }

    public void SetOvercrowdingInfo(int cell_count, int fish_count)
    {
      this.cellCount = cell_count;
      this.fishCount = fish_count;
    }
  }
}
