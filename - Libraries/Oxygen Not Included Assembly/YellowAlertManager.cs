// Decompiled with JetBrains decompiler
// Type: YellowAlertManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class YellowAlertManager : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance>
{
  public StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter();
  public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on;
  public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on_pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.serializable = true;
    this.off.ParamTransition<bool>((StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isOn, this.on, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsTrue);
    this.on.Enter("EnterEvent", (StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State.Callback) (smi => Game.Instance.Trigger(-741654735, (object) null))).Exit("ExitEvent", (StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State.Callback) (smi => Game.Instance.Trigger(-2062778933, (object) null))).Enter("EnableVignette", (StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State.Callback) (smi => Vignette.Instance.SetColor(new Color(1f, 1f, 0.0f, 0.1f)))).Exit("DisableVignette", (StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State.Callback) (smi => Vignette.Instance.Reset())).Enter("Sounds", (StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State.Callback) (smi => KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false)))).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), (Func<YellowAlertManager.Instance, bool>) null, true, true, true).ToggleNotification((Func<YellowAlertManager.Instance, Notification>) (smi => smi.notification)).ParamTransition<bool>((StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isOn, this.off, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsFalse);
    this.on_pst.Enter("Sounds", (StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State.Callback) (smi => KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false))));
  }

  public class Instance : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Notification notification = new Notification((string) MISC.NOTIFICATIONS.YELLOWALERT.NAME, NotificationType.Bad, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.YELLOWALERT.TOOLTIP), (object) null, false, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
    private static YellowAlertManager.Instance instance;
    private bool hasTopPriorityChore;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      YellowAlertManager.Instance.instance = this;
    }

    public static void DestroyInstance()
    {
      YellowAlertManager.Instance.instance = (YellowAlertManager.Instance) null;
    }

    public static YellowAlertManager.Instance Get()
    {
      return YellowAlertManager.Instance.instance;
    }

    public bool IsOn()
    {
      return this.sm.isOn.Get(this.smi);
    }

    public void HasTopPriorityChore(bool on)
    {
      this.hasTopPriorityChore = on;
      this.Refresh();
    }

    private void Refresh()
    {
      this.sm.isOn.Set(this.hasTopPriorityChore, this.smi);
    }
  }
}
