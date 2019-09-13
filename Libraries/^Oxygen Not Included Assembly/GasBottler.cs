// Decompiled with JetBrains decompiler
// Type: GasBottler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GasBottler : Workable
{
  public Storage storage;
  private GasBottler.Controller.Instance smi;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new GasBottler.Controller.Instance(this);
    this.smi.StartSM();
    this.UpdateStoredItemState();
  }

  protected override void OnCleanUp()
  {
    if (this.smi != null)
      this.smi.StopSM(nameof (OnCleanUp));
    base.OnCleanUp();
  }

  private void UpdateStoredItemState()
  {
    this.storage.allowItemRemoval = this.smi != null && this.smi.GetCurrentState() == this.smi.sm.ready;
    foreach (GameObject go in this.storage.items)
    {
      if ((Object) go != (Object) null)
        go.Trigger(-778359855, (object) this.storage);
    }
  }

  private class Controller : GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler>
  {
    public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State empty;
    public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State filling;
    public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State ready;
    public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State pickup;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.empty.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.filling, (StateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.Transition.ConditionCallback) (smi => smi.master.storage.IsFull()));
      this.filling.PlayAnim("working").OnAnimQueueComplete(this.ready);
      this.ready.EventTransition(GameHashes.OnStorageChange, this.pickup, (StateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.Transition.ConditionCallback) (smi => !smi.master.storage.IsFull())).Enter((StateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State.Callback) (smi =>
      {
        smi.master.storage.allowItemRemoval = true;
        foreach (GameObject go in smi.master.storage.items)
          go.Trigger(-778359855, (object) smi.master.storage);
      })).Exit((StateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State.Callback) (smi =>
      {
        smi.master.storage.allowItemRemoval = false;
        foreach (GameObject go in smi.master.storage.items)
          go.Trigger(-778359855, (object) smi.master.storage);
      }));
      this.pickup.PlayAnim("pick_up").OnAnimQueueComplete(this.empty);
    }

    public class Instance : GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.GameInstance
    {
      public Instance(GasBottler master)
        : base(master)
      {
      }
    }
  }
}
