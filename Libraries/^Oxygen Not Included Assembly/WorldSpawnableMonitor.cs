// Decompiled with JetBrains decompiler
// Type: WorldSpawnableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class WorldSpawnableMonitor : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
  }

  public class Def : StateMachine.BaseDef
  {
    public Func<int, int> adjustSpawnLocationCb;
  }

  public class Instance : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, WorldSpawnableMonitor.Def def)
      : base(master, def)
    {
    }
  }
}
