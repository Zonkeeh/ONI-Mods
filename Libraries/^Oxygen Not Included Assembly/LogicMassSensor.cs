// Decompiled with JetBrains decompiler
// Type: LogicMassSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
  private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>((System.Action<LogicMassSensor, object>) ((component, data) => component.OnCopySettings(data)));
  [SerializeField]
  [Serialize]
  private bool activateAboveThreshold = true;
  public float rangeMax = 1f;
  private float toggleCooldown = 0.15f;
  [SerializeField]
  [Serialize]
  private float threshold;
  [MyCmpGet]
  private LogicPorts logicPorts;
  private bool was_pressed;
  private bool was_on;
  public float rangeMin;
  [Serialize]
  private float massSolid;
  [Serialize]
  private float massPickupables;
  [Serialize]
  private float massActivators;
  private const float MIN_TOGGLE_TIME = 0.15f;
  private HandleVector<int>.Handle solidChangedEntry;
  private HandleVector<int>.Handle pickupablesChangedEntry;
  private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicMassSensor component = ((GameObject) data).GetComponent<LogicMassSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateVisualState(true);
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new System.Action<object>(this.OnPickupablesChanged));
    this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new System.Action<object>(this.OnActivatorsChanged));
    this.OnToggle += new System.Action<bool>(this.SwitchToggled);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
    GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
    GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
    base.OnCleanUp();
  }

  private void Update()
  {
    this.toggleCooldown = Mathf.Max(0.0f, this.toggleCooldown - Time.deltaTime);
    if ((double) this.toggleCooldown != 0.0)
      return;
    float currentValue = this.CurrentValue;
    if ((!this.activateAboveThreshold ? (double) currentValue < (double) this.threshold : (double) currentValue > (double) this.threshold) != this.IsSwitchedOn)
    {
      this.Toggle();
      this.toggleCooldown = 0.15f;
    }
    this.UpdateVisualState(false);
  }

  private void OnSolidChanged(object data)
  {
    int index = Grid.CellAbove(this.NaturalBuildingCell());
    if (Grid.Solid[index])
      this.massSolid = Grid.Mass[index];
    else
      this.massSolid = 0.0f;
  }

  private void OnPickupablesChanged(object data)
  {
    float num = 0.0f;
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) pooledList);
    for (int index = 0; index < pooledList.Count; ++index)
    {
      Pickupable cmp = pooledList[index].obj as Pickupable;
      if (!((UnityEngine.Object) cmp == (UnityEngine.Object) null) && !cmp.wasAbsorbed)
      {
        KPrefabID component = cmp.GetComponent<KPrefabID>();
        if (!component.HasTag(GameTags.Creature) || (component.HasTag(GameTags.Creatures.Walker) || component.HasTag(GameTags.Creatures.Hoverer) || cmp.HasTag(GameTags.Creatures.Flopping)))
          num += cmp.PrimaryElement.Mass;
      }
    }
    pooledList.Recycle();
    this.massPickupables = num;
  }

  private void OnActivatorsChanged(object data)
  {
    float num = 0.0f;
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, (List<ScenePartitionerEntry>) pooledList);
    for (int index = 0; index < pooledList.Count; ++index)
    {
      FloorSwitchActivator floorSwitchActivator = pooledList[index].obj as FloorSwitchActivator;
      if (!((UnityEngine.Object) floorSwitchActivator == (UnityEngine.Object) null))
        num += floorSwitchActivator.PrimaryElement.Mass;
    }
    pooledList.Recycle();
    this.massActivators = num;
  }

  public LocString Title
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
    }
  }

  public float Threshold
  {
    get
    {
      return this.threshold;
    }
    set
    {
      this.threshold = value;
    }
  }

  public bool ActivateAboveThreshold
  {
    get
    {
      return this.activateAboveThreshold;
    }
    set
    {
      this.activateAboveThreshold = value;
    }
  }

  public float CurrentValue
  {
    get
    {
      return this.massSolid + this.massPickupables + this.massActivators;
    }
  }

  public float RangeMin
  {
    get
    {
      return this.rangeMin;
    }
  }

  public float RangeMax
  {
    get
    {
      return this.rangeMax;
    }
  }

  public float GetRangeMinInputField()
  {
    return this.rangeMin;
  }

  public float GetRangeMaxInputField()
  {
    return this.rangeMax;
  }

  public LocString ThresholdValueName
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
    }
  }

  public string AboveToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
    }
  }

  public string BelowToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
    }
  }

  public string Format(float value, bool units)
  {
    GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.Kilogram;
    float mass = value;
    bool includeSuffix = units;
    return GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, massFormat, includeSuffix, "{0:0.#}");
  }

  public float ProcessedSliderValue(float input)
  {
    input = Mathf.Round(input);
    return input;
  }

  public float ProcessedInputValue(float input)
  {
    return input;
  }

  public LocString ThresholdValueUnits()
  {
    return GameUtil.GetCurrentMassUnit(false);
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

  private void SwitchToggled(bool toggled_on)
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !toggled_on ? 0 : 1);
  }

  private void UpdateVisualState(bool force = false)
  {
    bool flag = (double) this.CurrentValue > (double) this.threshold;
    if (flag == this.was_pressed && this.was_on == this.IsSwitchedOn && !force)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (flag)
    {
      if (force)
      {
        component.Play((HashedString) (!this.IsSwitchedOn ? "off_down" : "on_down"), KAnim.PlayMode.Once, 1f, 0.0f);
      }
      else
      {
        component.Play((HashedString) (!this.IsSwitchedOn ? "off_down_pre" : "on_down_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
        component.Queue((HashedString) (!this.IsSwitchedOn ? "off_down" : "on_down"), KAnim.PlayMode.Once, 1f, 0.0f);
      }
    }
    else if (force)
    {
      component.Play((HashedString) (!this.IsSwitchedOn ? "off_up" : "on_up"), KAnim.PlayMode.Once, 1f, 0.0f);
    }
    else
    {
      component.Play((HashedString) (!this.IsSwitchedOn ? "off_up_pre" : "on_up_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
      component.Queue((HashedString) (!this.IsSwitchedOn ? "off_up" : "on_up"), KAnim.PlayMode.Once, 1f, 0.0f);
    }
    this.was_pressed = flag;
    this.was_on = this.IsSwitchedOn;
  }

  protected override void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusInactive : Db.Get().BuildingStatusItems.LogicSensorStatusActive, (object) null);
  }
}
