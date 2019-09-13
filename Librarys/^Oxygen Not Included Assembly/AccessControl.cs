// Decompiled with JetBrains decompiler
// Type: AccessControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class AccessControl : KMonoBehaviour, ISaveLoadable, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate = new EventSystem.IntraObjectHandler<AccessControl>((System.Action<AccessControl, object>) ((component, data) => component.OnControlStateChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AccessControl>((System.Action<AccessControl, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> savedPermissions = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
  [MyCmpGet]
  private Operational operational;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  [Serialize]
  private AccessControl.Permission _defaultPermission;
  [Serialize]
  public bool controlEnabled;
  public Door.ControlState overrideAccess;
  private static StatusItem accessControlActive;

  public AccessControl.Permission DefaultPermission
  {
    get
    {
      return this._defaultPermission;
    }
    set
    {
      this._defaultPermission = value;
      this.SetStatusItem();
      this.SetGridRestrictions((KPrefabID) null, this._defaultPermission);
    }
  }

  public bool Online
  {
    get
    {
      return true;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (AccessControl.accessControlActive == null)
      AccessControl.accessControlActive = new StatusItem("accessControlActive", (string) BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME, (string) BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
    this.Subscribe<AccessControl>(279163026, AccessControl.OnControlStateChangedDelegate);
    this.Subscribe<AccessControl>(-905833192, AccessControl.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RegisterInGrid(true);
    this.SetGridRestrictions((KPrefabID) null, this.DefaultPermission);
    foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> savedPermission in this.savedPermissions)
      this.SetGridRestrictions(savedPermission.Key.Get(), savedPermission.Value);
    ListPool<Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.PooledList pooledList = ListPool<Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.Allocate();
    for (int index = this.savedPermissions.Count - 1; index >= 0; --index)
    {
      KPrefabID kpid = this.savedPermissions[index].Key.Get();
      if ((UnityEngine.Object) kpid != (UnityEngine.Object) null)
      {
        MinionIdentity component = kpid.GetComponent<MinionIdentity>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          pooledList.Add(new Tuple<MinionAssignablesProxy, AccessControl.Permission>(component.assignableProxy.Get(), this.savedPermissions[index].Value));
          this.savedPermissions.RemoveAt(index);
          this.ClearGridRestrictions(kpid);
        }
      }
    }
    foreach (Tuple<MinionAssignablesProxy, AccessControl.Permission> tuple in (List<Tuple<MinionAssignablesProxy, AccessControl.Permission>>) pooledList)
      this.SetPermission(tuple.first, tuple.second);
    pooledList.Recycle();
    this.SetStatusItem();
  }

  protected override void OnCleanUp()
  {
    this.RegisterInGrid(false);
    base.OnCleanUp();
  }

  private void OnControlStateChanged(object data)
  {
    this.overrideAccess = (Door.ControlState) data;
  }

  private void OnCopySettings(object data)
  {
    AccessControl component = ((GameObject) data).GetComponent<AccessControl>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.savedPermissions.Clear();
    foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> savedPermission in component.savedPermissions)
    {
      if ((UnityEngine.Object) savedPermission.Key.Get() != (UnityEngine.Object) null)
        this.SetPermission(savedPermission.Key.Get().GetComponent<MinionAssignablesProxy>(), savedPermission.Value);
    }
    this._defaultPermission = component._defaultPermission;
    this.SetGridRestrictions((KPrefabID) null, this.DefaultPermission);
  }

  public void SetPermission(MinionAssignablesProxy key, AccessControl.Permission permission)
  {
    KPrefabID component = key.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    bool flag = false;
    for (int index = 0; index < this.savedPermissions.Count; ++index)
    {
      if (this.savedPermissions[index].Key.GetId() == component.InstanceID)
      {
        flag = true;
        KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> savedPermission = this.savedPermissions[index];
        this.savedPermissions[index] = new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(savedPermission.Key, permission);
        break;
      }
    }
    if (!flag)
      this.savedPermissions.Add(new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(new Ref<KPrefabID>(component), permission));
    this.SetStatusItem();
    this.SetGridRestrictions(component, permission);
  }

  private void RegisterInGrid(bool register)
  {
    Building component1 = this.GetComponent<Building>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    if (register)
    {
      Rotatable component2 = this.GetComponent<Rotatable>();
      Grid.Restriction.Orientation orientation = (UnityEngine.Object) component2 == (UnityEngine.Object) null || component2.GetOrientation() == Orientation.Neutral ? Grid.Restriction.Orientation.Vertical : Grid.Restriction.Orientation.Horizontal;
      foreach (int placementCell in component1.PlacementCells)
        Grid.RegisterRestriction(placementCell, orientation);
    }
    else
    {
      foreach (int placementCell in component1.PlacementCells)
        Grid.UnregisterRestriction(placementCell);
    }
  }

  private void SetGridRestrictions(KPrefabID kpid, AccessControl.Permission permission)
  {
    Building component = this.GetComponent<Building>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    int minion = !((UnityEngine.Object) kpid != (UnityEngine.Object) null) ? -1 : kpid.InstanceID;
    Grid.Restriction.Directions directions = (Grid.Restriction.Directions) 0;
    switch (permission)
    {
      case AccessControl.Permission.Both:
        directions = (Grid.Restriction.Directions) 0;
        break;
      case AccessControl.Permission.GoLeft:
        directions = Grid.Restriction.Directions.Right;
        break;
      case AccessControl.Permission.GoRight:
        directions = Grid.Restriction.Directions.Left;
        break;
      case AccessControl.Permission.Neither:
        directions = Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right;
        break;
    }
    foreach (int placementCell in component.PlacementCells)
      Grid.SetRestriction(placementCell, minion, directions);
  }

  private void ClearGridRestrictions(KPrefabID kpid)
  {
    Building component = this.GetComponent<Building>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    int minion = !((UnityEngine.Object) kpid != (UnityEngine.Object) null) ? -1 : kpid.InstanceID;
    foreach (int placementCell in component.PlacementCells)
      Grid.ClearRestriction(placementCell, minion);
  }

  public AccessControl.Permission GetPermission(Navigator minion)
  {
    switch (this.overrideAccess)
    {
      case Door.ControlState.Opened:
        return AccessControl.Permission.Both;
      case Door.ControlState.Locked:
        return AccessControl.Permission.Neither;
      default:
        return this.GetSetPermission(this.GetKeyForNavigator(minion));
    }
  }

  private MinionAssignablesProxy GetKeyForNavigator(Navigator minion)
  {
    return minion.GetComponent<MinionIdentity>().assignableProxy.Get();
  }

  public AccessControl.Permission GetSetPermission(MinionAssignablesProxy key)
  {
    return this.GetSetPermission(key.GetComponent<KPrefabID>());
  }

  private AccessControl.Permission GetSetPermission(KPrefabID kpid)
  {
    AccessControl.Permission defaultPermission = this.DefaultPermission;
    if ((UnityEngine.Object) kpid != (UnityEngine.Object) null)
    {
      for (int index = 0; index < this.savedPermissions.Count; ++index)
      {
        if (this.savedPermissions[index].Key.GetId() == kpid.InstanceID)
        {
          defaultPermission = this.savedPermissions[index].Value;
          break;
        }
      }
    }
    return defaultPermission;
  }

  public void ClearPermission(MinionAssignablesProxy key)
  {
    KPrefabID component = key.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      for (int index = 0; index < this.savedPermissions.Count; ++index)
      {
        if (this.savedPermissions[index].Key.GetId() == component.InstanceID)
        {
          this.savedPermissions.RemoveAt(index);
          break;
        }
      }
    }
    this.SetStatusItem();
    this.ClearGridRestrictions(component);
  }

  public bool IsDefaultPermission(MinionAssignablesProxy key)
  {
    bool flag = false;
    KPrefabID component = key.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      for (int index = 0; index < this.savedPermissions.Count; ++index)
      {
        if (this.savedPermissions[index].Key.GetId() == component.InstanceID)
        {
          flag = true;
          break;
        }
      }
    }
    return !flag;
  }

  private void SetStatusItem()
  {
    if (this._defaultPermission != AccessControl.Permission.Both || this.savedPermissions.Count > 0)
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, AccessControl.accessControlActive, (object) null);
    else
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, (StatusItem) null, (object) null);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.ACCESS_CONTROL, (string) UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL, Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor);
    return descriptorList;
  }

  public enum Permission
  {
    Both,
    GoLeft,
    GoRight,
    Neither,
  }
}
