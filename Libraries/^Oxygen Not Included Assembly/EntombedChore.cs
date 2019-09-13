// Decompiled with JetBrains decompiler
// Type: EntombedChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EntombedChore : Chore<EntombedChore.StatesInstance>
{
  public EntombedChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.Entombed, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new EntombedChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.GameInstance
  {
    public StatesInstance(EntombedChore master, GameObject entombable)
      : base(master)
    {
      this.sm.entombable.Set(entombable, this.smi);
    }

    public void UpdateFaceEntombed()
    {
      int cell = Grid.CellAbove(Grid.PosToCell(this.transform.GetPosition()));
      this.sm.isFaceEntombed.Set(Grid.IsValidCell(cell) && Grid.Solid[cell], this.smi);
    }
  }

  public class States : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore>
  {
    public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.BoolParameter isFaceEntombed;
    public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.TargetParameter entombable;
    public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedface;
    public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedbody;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.entombedbody;
      this.Target(this.entombable);
      this.root.ToggleAnims("anim_emotes_default_kanim", 0.0f).Update("IsFaceEntombed", (System.Action<EntombedChore.StatesInstance, float>) ((smi, dt) => smi.UpdateFaceEntombed()), UpdateRate.SIM_200ms, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.EntombedChore, (object) null);
      this.entombedface.PlayAnim("entombed_ceiling", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.Parameter<bool>) this.isFaceEntombed, this.entombedbody, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsFalse);
      this.entombedbody.PlayAnim("entombed_floor", KAnim.PlayMode.Loop).StopMoving().ParamTransition<bool>((StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.Parameter<bool>) this.isFaceEntombed, this.entombedface, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsTrue);
    }
  }
}
