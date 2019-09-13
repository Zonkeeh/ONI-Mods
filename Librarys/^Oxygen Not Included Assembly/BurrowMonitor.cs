// Decompiled with JetBrains decompiler
// Type: BurrowMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BurrowMonitor : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>
{
  public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State openair;
  public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State entombed;
  public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State burrowcomplete;
  public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State exitburrowcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.openair;
    this.openair.ToggleBehaviour(GameTags.Creatures.WantsToEnterBurrow, (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.Transition.ConditionCallback) (smi =>
    {
      if (smi.ShouldBurrow())
        return (double) smi.timeinstate > (double) smi.def.minimumAwakeTime;
      return false;
    }), (System.Action<BurrowMonitor.Instance>) (smi => smi.BurrowComplete())).Transition(this.entombed, (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsEntombed()), UpdateRate.SIM_200ms).Enter("SetFallAnim", (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State.Callback) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().anim = "fall")).Enter("SetCollider", (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State.Callback) (smi => smi.SetCollider(true)));
    this.entombed.Enter("SetFallAnim", (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State.Callback) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().anim = "dormant_pre")).Enter("SetCollider", (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State.Callback) (smi => smi.SetCollider(false))).Transition(this.openair, (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsEntombed()), UpdateRate.SIM_200ms).ToggleBehaviour(GameTags.Creatures.Burrowed, (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsEntombed()), (System.Action<BurrowMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.openair))).ToggleBehaviour(GameTags.Creatures.WantsToExitBurrow, (StateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.Transition.ConditionCallback) (smi =>
    {
      if (smi.EmergeIsClear())
        return GameClock.Instance.IsNighttime();
      return false;
    }), (System.Action<BurrowMonitor.Instance>) (smi => smi.ExitBurrowComplete()));
  }

  public class Def : StateMachine.BaseDef
  {
    public float burrowHardnessLimit = 20f;
    public float minimumAwakeTime = 24f;
    public Vector2 moundColliderSize = (Vector2) new Vector2f(1f, 1.5f);
    public Vector2 moundColliderOffset = new Vector2(0.0f, 0.75f);
  }

  public class Instance : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.GameInstance
  {
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    public Instance(IStateMachineTarget master, BurrowMonitor.Def def)
      : base(master, def)
    {
      KBoxCollider2D component = master.GetComponent<KBoxCollider2D>();
      this.originalColliderSize = component.size;
      this.originalColliderOffset = component.offset;
    }

    public bool EmergeIsClear()
    {
      int cell = Grid.PosToCell(this.gameObject);
      if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(Grid.CellAbove(cell)))
        return false;
      int index = Grid.CellAbove(cell);
      return !Grid.Solid[index] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.9f);
    }

    public bool ShouldBurrow()
    {
      return !GameClock.Instance.IsNighttime() && this.CanBurrowInto(Grid.CellBelow(Grid.PosToCell(this.gameObject)));
    }

    public bool CanBurrowInto(int cell)
    {
      return Grid.IsValidCell(cell) && Grid.Solid[cell] && (!Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f) && !((UnityEngine.Object) Grid.Objects[cell, 1] != (UnityEngine.Object) null)) && ((double) Grid.Element[cell].hardness <= (double) this.def.burrowHardnessLimit && !Grid.Foundation[cell]);
    }

    public bool IsEntombed()
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this.smi);
      if (Grid.IsValidCell(cell))
        return Grid.Solid[cell];
      return false;
    }

    public void ExitBurrowComplete()
    {
      this.smi.GetComponent<KBatchedAnimController>().Play((HashedString) "idle_loop", KAnim.PlayMode.Once, 1f, 0.0f);
      this.GoTo((StateMachine.BaseState) this.sm.openair);
    }

    public void BurrowComplete()
    {
      this.smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellBelow(Grid.PosToCell(this.transform.GetPosition())), Grid.SceneLayer.Creatures));
      this.smi.GetComponent<KBatchedAnimController>().Play((HashedString) "idle_mound", KAnim.PlayMode.Once, 1f, 0.0f);
      this.GoTo((StateMachine.BaseState) this.sm.entombed);
    }

    public void SetCollider(bool original_size)
    {
      KBoxCollider2D component = this.master.GetComponent<KBoxCollider2D>();
      if (original_size)
      {
        component.size = this.originalColliderSize;
        component.offset = this.originalColliderOffset;
      }
      else
      {
        component.size = this.def.moundColliderSize;
        component.offset = this.def.moundColliderOffset;
      }
    }
  }
}
