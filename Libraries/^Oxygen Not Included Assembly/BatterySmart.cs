// Decompiled with JetBrains decompiler
// Type: BatterySmart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class BatterySmart : Battery, IActivationRangeTarget
{
  public static readonly HashedString PORT_ID = (HashedString) "BatterySmartLogicPort";
  private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BatterySmart>((System.Action<BatterySmart, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<BatterySmart>((System.Action<BatterySmart, object>) ((component, data) => component.OnLogicValueChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<BatterySmart> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<BatterySmart>((System.Action<BatterySmart, object>) ((component, data) => component.UpdateLogicCircuit(data)));
  [Serialize]
  private int deactivateValue = 100;
  [Serialize]
  private int activateValue;
  [Serialize]
  private bool activated;
  [MyCmpGet]
  private LogicPorts logicPorts;
  private MeterController logicMeter;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<BatterySmart>(-905833192, BatterySmart.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    BatterySmart component = ((GameObject) data).GetComponent<BatterySmart>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.ActivateValue = component.ActivateValue;
    this.DeactivateValue = component.DeactivateValue;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.CreateLogicMeter();
    this.Subscribe<BatterySmart>(-801688580, BatterySmart.OnLogicValueChangedDelegate);
    this.Subscribe<BatterySmart>(-592767678, BatterySmart.UpdateLogicCircuitDelegate);
  }

  private void CreateLogicMeter()
  {
    this.logicMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[0]);
  }

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    this.UpdateLogicCircuit((object) null);
  }

  private void UpdateLogicCircuit(object data)
  {
    float num = (float) Mathf.RoundToInt(this.PercentFull * 100f);
    if (this.activated)
    {
      if ((double) num >= (double) this.deactivateValue)
        this.activated = false;
    }
    else if ((double) num <= (double) this.activateValue)
      this.activated = true;
    bool isOperational = this.operational.IsOperational;
    bool flag = this.activated && isOperational;
    this.logicPorts.SendSignal(BatterySmart.PORT_ID, !flag ? 0 : 1);
  }

  private void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (!(logicValueChanged.portID == BatterySmart.PORT_ID))
      return;
    this.SetLogicMeter(logicValueChanged.newValue > 0);
  }

  public void SetLogicMeter(bool on)
  {
    if (this.logicMeter == null)
      return;
    this.logicMeter.SetPositionPercent(!on ? 0.0f : 1f);
  }

  public float ActivateValue
  {
    get
    {
      return (float) this.deactivateValue;
    }
    set
    {
      this.deactivateValue = (int) value;
      this.UpdateLogicCircuit((object) null);
    }
  }

  public float DeactivateValue
  {
    get
    {
      return (float) this.activateValue;
    }
    set
    {
      this.activateValue = (int) value;
      this.UpdateLogicCircuit((object) null);
    }
  }

  public float MinValue
  {
    get
    {
      return 0.0f;
    }
  }

  public float MaxValue
  {
    get
    {
      return 100f;
    }
  }

  public bool UseWholeNumbers
  {
    get
    {
      return true;
    }
  }

  public string ActivateTooltip
  {
    get
    {
      return (string) BUILDINGS.PREFABS.BATTERYSMART.DEACTIVATE_TOOLTIP;
    }
  }

  public string DeactivateTooltip
  {
    get
    {
      return (string) BUILDINGS.PREFABS.BATTERYSMART.ACTIVATE_TOOLTIP;
    }
  }

  public string ActivationRangeTitleText
  {
    get
    {
      return (string) BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_TITLE;
    }
  }

  public string ActivateSliderLabelText
  {
    get
    {
      return (string) BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_DEACTIVATE;
    }
  }

  public string DeactivateSliderLabelText
  {
    get
    {
      return (string) BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_ACTIVATE;
    }
  }
}
