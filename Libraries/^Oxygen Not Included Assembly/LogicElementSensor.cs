// Decompiled with JetBrains decompiler
// Type: LogicElementSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicElementSensor : Switch, ISaveLoadable, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<LogicElementSensor> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicElementSensor>((System.Action<LogicElementSensor, object>) ((component, data) => component.OnOperationalChanged(data)));
  public Element.State desiredState = Element.State.Gas;
  private bool[] samples = new bool[8];
  private byte desiredElementIdx = byte.MaxValue;
  private bool wasOn;
  private const int WINDOW_SIZE = 8;
  private int sampleIdx;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<Filterable>().onFilterChanged += new System.Action<Tag>(this.OnElementSelected);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
    this.Subscribe<LogicElementSensor>(-592767678, LogicElementSensor.OnOperationalChangedDelegate);
  }

  public void Sim200ms(float dt)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (this.sampleIdx < 8)
    {
      this.samples[this.sampleIdx] = (int) Grid.ElementIdx[cell] == (int) this.desiredElementIdx;
      ++this.sampleIdx;
    }
    else
    {
      this.sampleIdx = 0;
      bool flag = true;
      foreach (bool sample in this.samples)
        flag = sample && flag;
      if (this.IsSwitchedOn == flag)
        return;
      this.Toggle();
    }
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState(false);
  }

  private void UpdateLogicCircuit()
  {
    bool flag = this.switchedOn && this.GetComponent<Operational>().IsOperational;
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !flag ? 0 : 1);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (this.wasOn == this.switchedOn && !force)
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (!this.switchedOn ? "on_pst" : "on_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
    component.Queue((HashedString) (!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private void OnElementSelected(Tag element_tag)
  {
    if (!element_tag.IsValid)
      return;
    Element element = ElementLoader.GetElement(element_tag);
    bool on = true;
    if (element != null)
    {
      this.desiredElementIdx = (byte) ElementLoader.GetElementIndex(element.id);
      on = element.id == SimHashes.Void || element.id == SimHashes.Vacuum;
    }
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, (object) null);
  }

  private void OnOperationalChanged(object data)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState(false);
  }

  protected override void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusInactive : Db.Get().BuildingStatusItems.LogicSensorStatusActive, (object) null);
  }
}
