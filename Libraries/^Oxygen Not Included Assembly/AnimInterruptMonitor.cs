// Decompiled with JetBrains decompiler
// Type: AnimInterruptMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class AnimInterruptMonitor : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.State root = this.root;
    Tag playInterruptAnim = GameTags.Creatures.Behaviours.PlayInterruptAnim;
    // ISSUE: reference to a compiler-generated field
    if (AnimInterruptMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AnimInterruptMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback fMgCache0 = AnimInterruptMonitor.\u003C\u003Ef__mg\u0024cache0;
    // ISSUE: reference to a compiler-generated field
    if (AnimInterruptMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AnimInterruptMonitor.\u003C\u003Ef__mg\u0024cache1 = new System.Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<AnimInterruptMonitor.Instance> fMgCache1 = AnimInterruptMonitor.\u003C\u003Ef__mg\u0024cache1;
    root.ToggleBehaviour(playInterruptAnim, fMgCache0, fMgCache1);
  }

  private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi)
  {
    return smi.anim.IsValid;
  }

  private static void ClearAnim(AnimInterruptMonitor.Instance smi)
  {
    smi.anim = HashedString.Invalid;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance
  {
    public HashedString anim;

    public Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def)
      : base(master, def)
    {
    }

    public void PlayAnim(HashedString anim)
    {
      this.anim = anim;
      this.GetComponent<CreatureBrain>().UpdateBrain();
    }
  }
}
