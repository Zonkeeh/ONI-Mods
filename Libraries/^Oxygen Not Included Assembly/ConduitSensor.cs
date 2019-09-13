// Decompiled with JetBrains decompiler
// Type: ConduitSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class ConduitSensor : Switch
{
  protected static readonly HashedString[] ON_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pre",
    (HashedString) "on"
  };
  protected static readonly HashedString[] OFF_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pst",
    (HashedString) "off"
  };
  public ConduitType conduitType;
  protected bool wasOn;
  protected KBatchedAnimController animController;

  protected abstract void ConduitUpdate(float dt);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
    Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
  }

  protected override void OnCleanUp()
  {
    Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    base.OnCleanUp();
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState(false);
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !this.switchedOn ? 0 : 1);
  }

  protected virtual void UpdateVisualState(bool force = false)
  {
    if (this.wasOn == this.switchedOn && !force)
      return;
    this.wasOn = this.switchedOn;
    if (this.switchedOn)
      this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
    else
      this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
  }
}
