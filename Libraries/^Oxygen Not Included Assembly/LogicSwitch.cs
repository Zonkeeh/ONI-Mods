// Decompiled with JetBrains decompiler
// Type: LogicSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections;
using System.Diagnostics;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicSwitch : Switch
{
  public static readonly HashedString PORT_ID = (HashedString) nameof (LogicSwitch);
  private System.Action firstFrameCallback;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateVisualization();
    this.UpdateLogicCircuit();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  protected override void Toggle()
  {
    base.Toggle();
    this.UpdateVisualization();
    this.UpdateLogicCircuit();
  }

  private void UpdateVisualization()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (!this.switchedOn ? "on_pst" : "on_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
    component.Queue((HashedString) (!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !this.switchedOn ? 0 : 1);
  }

  protected override void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.LogicSwitchStatusInactive : Db.Get().BuildingStatusItems.LogicSwitchStatusActive, (object) null);
  }

  public void SetFirstFrameCallback(System.Action ffCb)
  {
    this.firstFrameCallback = ffCb;
    this.StartCoroutine(this.RunCallback());
  }

  [DebuggerHidden]
  private IEnumerator RunCallback()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LogicSwitch.\u003CRunCallback\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
