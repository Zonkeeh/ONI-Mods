// Decompiled with JetBrains decompiler
// Type: Clearable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Clearable : Workable, ISaveLoadable, IRender200ms
{
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnStore(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnAbsorb(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Clearable> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Clearable>((System.Action<Clearable, object>) ((component, data) => component.OnEquipped(data)));
  public bool isClearable = true;
  [MyCmpReq]
  private Pickupable pickupable;
  [MyCmpReq]
  private KSelectable selectable;
  [Serialize]
  private bool isMarkedForClear;
  private HandleVector<int>.Handle clearHandle;

  protected override void OnPrefabInit()
  {
    this.Subscribe<Clearable>(2127324410, Clearable.OnCancelDelegate);
    this.Subscribe<Clearable>(856640610, Clearable.OnStoreDelegate);
    this.Subscribe<Clearable>(-2064133523, Clearable.OnAbsorbDelegate);
    this.Subscribe<Clearable>(493375141, Clearable.OnRefreshUserMenuDelegate);
    this.Subscribe<Clearable>(-1617557748, Clearable.OnEquippedDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Clearing;
    this.simRenderLoadBalance = true;
    this.autoRegisterSimRender = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.isMarkedForClear)
      return;
    if (this.HasTag(GameTags.Stored))
      this.isMarkedForClear = false;
    else
      this.MarkForClear(true);
  }

  private void OnStore(object data)
  {
    this.CancelClearing();
  }

  private void OnCancel(object data)
  {
    for (ObjectLayerListItem objectLayerListItem = this.pickupable.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
    {
      if ((UnityEngine.Object) objectLayerListItem.gameObject != (UnityEngine.Object) null)
        objectLayerListItem.gameObject.GetComponent<Clearable>().CancelClearing();
    }
  }

  public void CancelClearing()
  {
    if (!this.isMarkedForClear)
      return;
    this.isMarkedForClear = false;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Garbage);
    Prioritizable.RemoveRef(this.gameObject);
    if (this.clearHandle.IsValid())
    {
      GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
      this.clearHandle.Clear();
    }
    this.RefreshClearableStatus();
    SimAndRenderScheduler.instance.Remove((object) this);
  }

  public void MarkForClear(bool force = false)
  {
    if (!this.isClearable || this.isMarkedForClear && !force || (this.pickupable.IsEntombed || this.clearHandle.IsValid() || this.HasTag(GameTags.Stored)))
      return;
    Prioritizable.AddRef(this.gameObject);
    this.GetComponent<KPrefabID>().AddTag(GameTags.Garbage, false);
    this.isMarkedForClear = true;
    this.clearHandle = GlobalChoreProvider.Instance.RegisterClearable(this);
    this.RefreshClearableStatus();
    SimAndRenderScheduler.instance.Add((object) this, this.simRenderLoadBalance);
  }

  private void OnClickClear()
  {
    this.MarkForClear(false);
  }

  private void OnClickCancel()
  {
    this.CancelClearing();
  }

  private void OnEquipped(object data)
  {
    this.CancelClearing();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!this.clearHandle.IsValid())
      return;
    GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
    this.clearHandle.Clear();
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.isClearable || (UnityEngine.Object) this.GetComponent<Health>() != (UnityEngine.Object) null || this.HasTag(GameTags.Stored))
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.isMarkedForClear ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.CLEAR.NAME, new System.Action(this.OnClickClear), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.CLEAR.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.CLEAR.NAME_OFF, new System.Action(this.OnClickCancel), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.CLEAR.TOOLTIP_OFF, true), 1f);
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
      return;
    Clearable component = pickupable.GetComponent<Clearable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.isMarkedForClear)
      return;
    this.MarkForClear(false);
  }

  public void Render200ms(float dt)
  {
    this.RefreshClearableStatus();
  }

  public void RefreshClearableStatus()
  {
    if (this.isMarkedForClear)
    {
      bool on = GlobalChoreProvider.Instance.ClearableHasDestination(this.pickupable);
      this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClear, on, (object) this);
      this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClearNoStorage, !on, (object) this);
    }
    else
    {
      this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClear, false, (object) this);
      this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClearNoStorage, false, (object) this);
    }
  }
}
