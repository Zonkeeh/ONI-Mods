// Decompiled with JetBrains decompiler
// Type: MoveToLocationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class MoveToLocationMonitor : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance>
{
  public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.State moving;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.DoNothing();
    this.moving.ToggleChore((Func<MoveToLocationMonitor.Instance, Chore>) (smi => (Chore) new MoveChore(smi.master, Db.Get().ChoreTypes.MoveTo, (Func<MoveChore.StatesInstance, int>) (smii => smi.targetCell), false)), this.satisfied);
  }

  public class Instance : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public int targetCell;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      master.Subscribe(493375141, new System.Action<object>(this.OnRefreshUserMenu));
    }

    private void OnRefreshUserMenu(object data)
    {
      Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_control", (string) UI.USERMENUACTIONS.MOVETOLOCATION.NAME, new System.Action(this.OnClickMoveToLocation), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.MOVETOLOCATION.TOOLTIP, true), 0.2f);
    }

    private void OnClickMoveToLocation()
    {
      MoveToLocationTool.Instance.Activate(this.GetComponent<Navigator>());
    }

    public void MoveToLocation(int cell)
    {
      this.targetCell = cell;
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.satisfied);
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.moving);
    }

    public override void StopSM(string reason)
    {
      this.master.Unsubscribe(493375141, new System.Action<object>(this.OnRefreshUserMenu));
      base.StopSM(reason);
    }
  }
}
