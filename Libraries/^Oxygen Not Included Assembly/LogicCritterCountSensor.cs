// Decompiled with JetBrains decompiler
// Type: LogicCritterCountSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCritterCountSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<LogicCritterCountSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCritterCountSensor>((System.Action<LogicCritterCountSensor, object>) ((component, data) => component.OnCopySettings(data)));
  private bool countEggs = true;
  [Serialize]
  public bool activateOnGreaterThan = true;
  private bool wasOn;
  [Serialize]
  public int countThreshold;
  private int currentCount;
  private KSelectable selectable;
  private Guid roomStatusGUID;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.selectable = this.GetComponent<KSelectable>();
    this.Subscribe<LogicCritterCountSensor>(-905833192, LogicCritterCountSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicCritterCountSensor component = ((GameObject) data).GetComponent<LogicCritterCountSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.countThreshold = component.countThreshold;
    this.activateOnGreaterThan = component.activateOnGreaterThan;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void Sim200ms(float dt)
  {
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject);
    if (roomOfGameObject != null)
    {
      this.currentCount = roomOfGameObject.cavity.creatures.Count;
      if (this.countEggs)
        this.currentCount += roomOfGameObject.cavity.eggs.Count;
      this.SetState(!this.activateOnGreaterThan ? this.currentCount < this.countThreshold : this.currentCount > this.countThreshold);
      if (!this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
        return;
      this.selectable.RemoveStatusItem(this.roomStatusGUID, false);
    }
    else
    {
      if (!this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
        this.roomStatusGUID = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom, (object) null);
      this.SetState(false);
    }
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

  private void UpdateVisualState(bool force = false)
  {
    if (this.wasOn == this.switchedOn && !force)
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (!this.switchedOn ? "on_pst" : "on_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
    component.Queue((HashedString) (!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
  }

  protected override void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusInactive : Db.Get().BuildingStatusItems.LogicSensorStatusActive, (object) null);
  }

  public float Threshold
  {
    get
    {
      return (float) this.countThreshold;
    }
    set
    {
      this.countThreshold = (int) value;
    }
  }

  public bool ActivateAboveThreshold
  {
    get
    {
      return this.activateOnGreaterThan;
    }
    set
    {
      this.activateOnGreaterThan = value;
    }
  }

  public float CurrentValue
  {
    get
    {
      return (float) this.currentCount;
    }
  }

  public float RangeMin
  {
    get
    {
      return 0.0f;
    }
  }

  public float RangeMax
  {
    get
    {
      return 64f;
    }
  }

  public float GetRangeMinInputField()
  {
    return this.RangeMin;
  }

  public float GetRangeMaxInputField()
  {
    return this.RangeMax;
  }

  public LocString Title
  {
    get
    {
      return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TITLE;
    }
  }

  public LocString ThresholdValueName
  {
    get
    {
      return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.VALUE_NAME;
    }
  }

  public string AboveToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_ABOVE;
    }
  }

  public string BelowToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_BELOW;
    }
  }

  public string Format(float value, bool units)
  {
    return value.ToString();
  }

  public float ProcessedSliderValue(float input)
  {
    return Mathf.Round(input);
  }

  public float ProcessedInputValue(float input)
  {
    return Mathf.Round(input);
  }

  public LocString ThresholdValueUnits()
  {
    return (LocString) string.Empty;
  }

  public ThresholdScreenLayoutType LayoutType
  {
    get
    {
      return ThresholdScreenLayoutType.SliderBar;
    }
  }

  public int IncrementScale
  {
    get
    {
      return 1;
    }
  }

  public NonLinearSlider.Range[] GetRanges
  {
    get
    {
      return NonLinearSlider.GetDefaultRange(this.RangeMax);
    }
  }
}
