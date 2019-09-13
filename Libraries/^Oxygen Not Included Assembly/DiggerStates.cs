// Decompiled with JetBrains decompiler
// Type: DiggerStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class DiggerStates : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>
{
  public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State move;
  public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State hide;
  public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State behaviourcomplete;

  private static float GetHideDuration()
  {
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SaveGame.Instance.GetComponent<SeasonManager>() != (UnityEngine.Object) null)
      return SaveGame.Instance.GetComponent<SeasonManager>().GetBombardmentDuration();
    return 0.0f;
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.move;
    this.move.MoveTo((Func<DiggerStates.Instance, int>) (smi => smi.GetTunnelCell()), this.hide, this.behaviourcomplete, false);
    this.hide.ScheduleGoTo(DiggerStates.GetHideDuration(), (StateMachine.BaseState) this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Tunnel, false);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.GameInstance
  {
    public Instance(Chore<DiggerStates.Instance> chore, DiggerStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Tunnel);
    }

    public int GetTunnelCell()
    {
      DiggerMonitor.Instance smi = this.smi.GetSMI<DiggerMonitor.Instance>();
      if (smi != null)
        return smi.lastDigCell;
      return -1;
    }
  }
}
