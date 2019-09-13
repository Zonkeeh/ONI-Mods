// Decompiled with JetBrains decompiler
// Type: RationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class RationBox : KMonoBehaviour, IUserControlledCapacity, IRender1000ms
{
  private static readonly EventSystem.IntraObjectHandler<RationBox> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<RationBox>((System.Action<RationBox, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<RationBox> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<RationBox>((System.Action<RationBox, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private float userMaxCapacity = float.PositiveInfinity;
  [MyCmpReq]
  private Storage storage;
  private FilteredStorage filteredStorage;

  protected override void OnPrefabInit()
  {
    this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, new Tag[1]
    {
      GameTags.Compostable
    }, (IUserControlledCapacity) this, false, Db.Get().ChoreTypes.FoodFetch);
    this.Subscribe<RationBox>(-592767678, RationBox.OnOperationalChangedDelegate);
    this.Subscribe<RationBox>(-905833192, RationBox.OnCopySettingsDelegate);
    WorldInventory.Instance.Discover("FieldRation".ToTag(), GameTags.Edible);
  }

  protected override void OnSpawn()
  {
    Operational component = this.GetComponent<Operational>();
    component.SetActive(component.IsOperational, false);
    this.filteredStorage.FilterChanged();
  }

  protected override void OnCleanUp()
  {
    this.filteredStorage.CleanUp();
  }

  private void OnOperationalChanged(object data)
  {
    Operational component = this.GetComponent<Operational>();
    component.SetActive(component.IsOperational, false);
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    RationBox component = gameObject.GetComponent<RationBox>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }

  public void Render1000ms(float dt)
  {
    Rottable.SetStatusItems(this.GetComponent<KSelectable>(), Rottable.IsRefrigerated(this.gameObject), Rottable.AtmosphereQuality(this.gameObject));
  }

  public float UserMaxCapacity
  {
    get
    {
      return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
    }
    set
    {
      this.userMaxCapacity = value;
      this.filteredStorage.FilterChanged();
    }
  }

  public float AmountStored
  {
    get
    {
      return this.storage.MassStored();
    }
  }

  public float MinCapacity
  {
    get
    {
      return 0.0f;
    }
  }

  public float MaxCapacity
  {
    get
    {
      return this.storage.capacityKg;
    }
  }

  public bool WholeValues
  {
    get
    {
      return false;
    }
  }

  public LocString CapacityUnits
  {
    get
    {
      return GameUtil.GetCurrentMassUnit(false);
    }
  }
}
