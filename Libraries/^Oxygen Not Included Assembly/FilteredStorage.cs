// Decompiled with JetBrains decompiler
// Type: FilteredStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class FilteredStorage
{
  public static readonly HashedString FULL_PORT_ID = (HashedString) "FULL";
  public static readonly Color32 FILTER_TINT = (Color32) Color.white;
  public static readonly Color32 NO_FILTER_TINT = (Color32) new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);
  public Color32 filterTint = FilteredStorage.FILTER_TINT;
  public Color32 noFilterTint = FilteredStorage.NO_FILTER_TINT;
  private bool hasMeter = true;
  private KMonoBehaviour root;
  private FetchList2 fetchList;
  private IUserControlledCapacity capacityControl;
  private TreeFilterable filterable;
  private Storage storage;
  private MeterController meter;
  private MeterController logicMeter;
  private Tag[] requiredTags;
  private Tag[] forbiddenTags;
  private bool useLogicMeter;
  private static StatusItem capacityStatusItem;
  private static StatusItem noFilterStatusItem;
  private ChoreType choreType;

  public FilteredStorage(
    KMonoBehaviour root,
    Tag[] required_tags,
    Tag[] forbidden_tags,
    IUserControlledCapacity capacity_control,
    bool use_logic_meter,
    ChoreType fetch_chore_type)
  {
    this.root = root;
    this.requiredTags = required_tags;
    this.forbiddenTags = forbidden_tags;
    this.capacityControl = capacity_control;
    this.useLogicMeter = use_logic_meter;
    this.choreType = fetch_chore_type;
    root.Subscribe(-1697596308, new System.Action<object>(this.OnStorageChanged));
    root.Subscribe(-543130682, new System.Action<object>(this.OnUserSettingsChanged));
    this.filterable = root.FindOrAdd<TreeFilterable>();
    this.filterable.OnFilterChanged += new System.Action<Tag[]>(this.OnFilterChanged);
    this.storage = root.GetComponent<Storage>();
    this.storage.Subscribe(644822890, new System.Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
    if (FilteredStorage.capacityStatusItem == null)
    {
      FilteredStorage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      FilteredStorage.capacityStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        FilteredStorage filteredStorage = (FilteredStorage) data;
        float amountStored = filteredStorage.GetAmountStored();
        float b = filteredStorage.storage.capacityKg;
        string newValue1 = Util.FormatWholeNumber((double) amountStored <= (double) b - (double) filteredStorage.storage.storageFullMargin || (double) amountStored >= (double) b ? Mathf.Floor(amountStored) : b);
        IUserControlledCapacity component = filteredStorage.root.GetComponent<IUserControlledCapacity>();
        if (component != null)
          b = Mathf.Min(component.UserMaxCapacity, b);
        string newValue2 = Util.FormatWholeNumber(b);
        str = str.Replace("{Stored}", newValue1);
        str = str.Replace("{Capacity}", newValue2);
        str = component == null ? str.Replace("{Units}", (string) GameUtil.GetCurrentMassUnit(false)) : str.Replace("{Units}", (string) component.CapacityUnits);
        return str;
      });
      FilteredStorage.noFilterStatusItem = new StatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
    }
    root.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, FilteredStorage.capacityStatusItem, (object) this);
  }

  public void SetHasMeter(bool has_meter)
  {
    this.hasMeter = has_meter;
  }

  private void OnOnlyFetchMarkedItemsSettingChanged(object data)
  {
    this.OnFilterChanged(this.filterable.GetTags());
  }

  private void CreateMeter()
  {
    if (!this.hasMeter)
      return;
    this.meter = new MeterController((KAnimControllerBase) this.root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[2]
    {
      "meter_frame",
      "meter_level"
    });
  }

  private void CreateLogicMeter()
  {
    if (!this.hasMeter)
      return;
    this.logicMeter = new MeterController((KAnimControllerBase) this.root.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[0]);
  }

  public void CleanUp()
  {
    if ((UnityEngine.Object) this.filterable != (UnityEngine.Object) null)
      this.filterable.OnFilterChanged -= new System.Action<Tag[]>(this.OnFilterChanged);
    if (this.fetchList == null)
      return;
    this.fetchList.Cancel("Parent destroyed");
  }

  public void FilterChanged()
  {
    if (this.hasMeter)
    {
      if (this.meter == null)
        this.CreateMeter();
      if (this.logicMeter == null && this.useLogicMeter)
        this.CreateLogicMeter();
    }
    this.OnFilterChanged(this.filterable.GetTags());
    this.UpdateMeter();
  }

  private void OnUserSettingsChanged(object data)
  {
    this.OnFilterChanged(this.filterable.GetTags());
    this.UpdateMeter();
  }

  private void OnStorageChanged(object data)
  {
    if (this.fetchList == null)
      this.OnFilterChanged(this.filterable.GetTags());
    this.UpdateMeter();
  }

  private void UpdateMeter()
  {
    float percent_full = Mathf.Clamp01(this.GetAmountStored() / this.GetMaxCapacityMinusStorageMargin());
    if (this.meter == null)
      return;
    this.meter.SetPositionPercent(percent_full);
  }

  public bool IsFull()
  {
    float percent_full = Mathf.Clamp01(this.GetAmountStored() / this.GetMaxCapacityMinusStorageMargin());
    if (this.meter != null)
      this.meter.SetPositionPercent(percent_full);
    return (double) percent_full >= 1.0;
  }

  private void OnFetchComplete()
  {
    this.OnFilterChanged(this.filterable.GetTags());
  }

  private float GetMaxCapacity()
  {
    float a = this.storage.capacityKg;
    if (this.capacityControl != null)
      a = Mathf.Min(a, this.capacityControl.UserMaxCapacity);
    return a;
  }

  private float GetMaxCapacityMinusStorageMargin()
  {
    return this.GetMaxCapacity() - this.storage.storageFullMargin;
  }

  private float GetAmountStored()
  {
    float num = this.storage.MassStored();
    if (this.capacityControl != null)
      num = this.capacityControl.AmountStored;
    return num;
  }

  private void OnFilterChanged(Tag[] tags)
  {
    KBatchedAnimController component = this.root.GetComponent<KBatchedAnimController>();
    bool flag = tags != null && tags.Length != 0;
    component.TintColour = !flag ? this.noFilterTint : this.filterTint;
    if (this.fetchList != null)
    {
      this.fetchList.Cancel(string.Empty);
      this.fetchList = (FetchList2) null;
    }
    float minusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
    float amountStored = this.GetAmountStored();
    if ((double) Mathf.Max(0.0f, minusStorageMargin - amountStored) > 0.0 && flag)
    {
      float amount = Mathf.Max(0.0f, this.GetMaxCapacity() - amountStored);
      this.fetchList = new FetchList2(this.storage, this.choreType);
      this.fetchList.ShowStatusItem = false;
      this.fetchList.Add(tags, this.requiredTags, this.forbiddenTags, amount, FetchOrder2.OperationalRequirement.Functional);
      this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
    }
    this.root.GetComponent<KSelectable>().ToggleStatusItem(FilteredStorage.noFilterStatusItem, !flag, (object) this);
  }

  public void SetLogicMeter(bool on)
  {
    if (this.logicMeter == null)
      return;
    this.logicMeter.SetPositionPercent(!on ? 0.0f : 1f);
  }
}
