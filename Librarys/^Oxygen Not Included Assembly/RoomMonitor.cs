// Decompiled with JetBrains decompiler
// Type: RoomMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
  public Room currentRoom;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (RoomMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RoomMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback(RoomMonitor.UpdateRoomType);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache0 = RoomMonitor.\u003C\u003Ef__mg\u0024cache0;
    root.EventHandler(GameHashes.PathAdvanced, fMgCache0);
  }

  private static void UpdateRoomType(RoomMonitor.Instance smi)
  {
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
    if (roomOfGameObject == smi.sm.currentRoom)
      return;
    smi.sm.currentRoom = roomOfGameObject;
    roomOfGameObject?.cavity.OnEnter((object) smi.master.gameObject);
  }

  public class Instance : GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Navigator navigator;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.navigator = this.GetComponent<Navigator>();
    }

    public Room GetCurrentRoomType()
    {
      return this.sm.currentRoom;
    }
  }
}
