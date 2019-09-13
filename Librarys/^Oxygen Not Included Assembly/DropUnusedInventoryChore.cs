// Decompiled with JetBrains decompiler
// Type: DropUnusedInventoryChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DropUnusedInventoryChore : Chore<DropUnusedInventoryChore.StatesInstance>
{
  public DropUnusedInventoryChore(ChoreType chore_type, IStateMachineTarget target)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new DropUnusedInventoryChore.StatesInstance(this);
  }

  public class StatesInstance : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.GameInstance
  {
    public StatesInstance(DropUnusedInventoryChore master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore>
  {
    public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State dropping;
    public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.dropping;
      this.dropping.Enter((StateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State.Callback) (smi => smi.GetComponent<Storage>().DropAll(false, false, new Vector3(), true))).GoTo(this.success);
      this.success.ReturnSuccess();
    }
  }
}
