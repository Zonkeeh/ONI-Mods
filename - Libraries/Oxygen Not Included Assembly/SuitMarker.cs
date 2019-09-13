// Decompiled with JetBrains decompiler
// Type: SuitMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SuitMarker : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((System.Action<SuitMarker, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((System.Action<SuitMarker, object>) ((component, data) => component.OnOperationalChanged((bool) data)));
  private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRotatedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>((System.Action<SuitMarker, object>) ((component, data) => component.isRotated = ((Rotatable) data).IsRotated));
  public KAnimFile interactAnim = Assets.GetAnim((HashedString) "anim_equip_clothing_kanim");
  [MyCmpGet]
  private Building building;
  private ScenePartitionerEntry partitionerEntry;
  private SuitMarker.SuitMarkerReactable reactable;
  private bool hasAvailableSuit;
  [Serialize]
  private bool onlyTraverseIfUnequipAvailable;
  private Grid.SuitMarker.Flags gridFlags;
  private int cell;
  public Tag[] LockerTags;
  public PathFinder.PotentialPath.Flags PathFlag;

  private bool OnlyTraverseIfUnequipAvailable
  {
    get
    {
      DebugUtil.Assert(this.onlyTraverseIfUnequipAvailable == ((this.gridFlags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != (Grid.SuitMarker.Flags) 0));
      return this.onlyTraverseIfUnequipAvailable;
    }
    set
    {
      this.onlyTraverseIfUnequipAvailable = value;
      this.UpdateGridFlag(Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable, this.onlyTraverseIfUnequipAvailable);
    }
  }

  private bool isRotated
  {
    get
    {
      return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) != (Grid.SuitMarker.Flags) 0;
    }
    set
    {
      this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
    }
  }

  private bool isOperational
  {
    get
    {
      return (this.gridFlags & Grid.SuitMarker.Flags.Operational) != (Grid.SuitMarker.Flags) 0;
    }
    set
    {
      this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnlyTraverseIfUnequipAvailable = this.onlyTraverseIfUnequipAvailable;
    Debug.Assert((UnityEngine.Object) this.interactAnim != (UnityEngine.Object) null, (object) "interactAnim is null");
    this.Subscribe<SuitMarker>(493375141, SuitMarker.OnRefreshUserMenuDelegate);
    this.isOperational = this.GetComponent<Operational>().IsOperational;
    this.Subscribe<SuitMarker>(-592767678, SuitMarker.OnOperationalChangedDelegate);
    this.isRotated = this.GetComponent<Rotatable>().IsRotated;
    this.Subscribe<SuitMarker>(-1643076535, SuitMarker.OnRotatedDelegate);
    this.CreateNewReactable();
    this.cell = Grid.PosToCell((KMonoBehaviour) this);
    Grid.RegisterSuitMarker(this.cell);
    this.GetComponent<KAnimControllerBase>().Play((HashedString) "no_suit", KAnim.PlayMode.Once, 1f, 0.0f);
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
    this.RefreshTraverseIfUnequipStatusItem();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
  }

  private void CreateNewReactable()
  {
    this.reactable = new SuitMarker.SuitMarkerReactable(this);
  }

  public void GetAttachedLockers(List<SuitLocker> suit_lockers)
  {
    int num1 = !this.isRotated ? -1 : 1;
    int num2 = 1;
    while (true)
    {
      int index = Grid.OffsetCell(this.cell, num2 * num1, 0);
      GameObject gameObject = Grid.Objects[index, 1];
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        KPrefabID component1 = gameObject.GetComponent<KPrefabID>();
        if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
        {
          if (component1.HasAnyTags(this.LockerTags))
          {
            SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
            if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null))
            {
              if (!suit_lockers.Contains(component2))
                suit_lockers.Add(component2);
            }
            else
              goto label_4;
          }
          else
            goto label_6;
        }
        ++num2;
      }
      else
        break;
    }
    return;
label_6:
    return;
label_4:;
  }

  public static bool DoesTraversalDirectionRequireSuit(
    int source_cell,
    int dest_cell,
    Grid.SuitMarker.Flags flags)
  {
    return Grid.CellColumn(dest_cell) > Grid.CellColumn(source_cell) == ((flags & Grid.SuitMarker.Flags.Rotated) == (Grid.SuitMarker.Flags) 0);
  }

  public bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell)
  {
    return SuitMarker.DoesTraversalDirectionRequireSuit(source_cell, dest_cell, this.gridFlags);
  }

  private void Update()
  {
    ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
    this.GetAttachedLockers((List<SuitLocker>) pooledList);
    int emptyLockerCount = 0;
    int fullLockerCount = 0;
    KPrefabID kprefabId = (KPrefabID) null;
    foreach (SuitLocker suitLocker in (List<SuitLocker>) pooledList)
    {
      if (suitLocker.CanDropOffSuit())
        ++emptyLockerCount;
      if ((UnityEngine.Object) suitLocker.GetPartiallyChargedOutfit() != (UnityEngine.Object) null)
        ++fullLockerCount;
      if ((UnityEngine.Object) kprefabId == (UnityEngine.Object) null)
        kprefabId = suitLocker.GetStoredOutfit();
    }
    pooledList.Recycle();
    bool flag = (UnityEngine.Object) kprefabId != (UnityEngine.Object) null;
    if (flag != this.hasAvailableSuit)
    {
      this.GetComponent<KAnimControllerBase>().Play((HashedString) (!flag ? "no_suit" : "off"), KAnim.PlayMode.Once, 1f, 0.0f);
      this.hasAvailableSuit = flag;
    }
    Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
  }

  private void RefreshTraverseIfUnequipStatusItem()
  {
    if (this.OnlyTraverseIfUnequipAvailable)
    {
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, (object) null);
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, false);
    }
    else
    {
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, false);
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, (object) null);
    }
  }

  private void OnEnableTraverseIfUnequipAvailable()
  {
    this.OnlyTraverseIfUnequipAvailable = true;
    this.RefreshTraverseIfUnequipStatusItem();
  }

  private void OnDisableTraverseIfUnequipAvailable()
  {
    this.OnlyTraverseIfUnequipAvailable = false;
    this.RefreshTraverseIfUnequipStatusItem();
  }

  private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
  {
    if (state)
      this.gridFlags |= flag;
    else
      this.gridFlags &= ~flag;
  }

  private void OnOperationalChanged(bool isOperational)
  {
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
    this.isOperational = isOperational;
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, this.OnlyTraverseIfUnequipAvailable ? new KIconButtonMenu.ButtonInfo("action_clearance", (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.NAME, new System.Action(this.OnDisableTraverseIfUnequipAvailable), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_clearance", (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.NAME, new System.Action(this.OnEnableTraverseIfUnequipAvailable), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.TOOLTIP, true), 1f);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.isSpawned)
      Grid.UnregisterSuitMarker(this.cell);
    if (this.partitionerEntry != null)
    {
      this.partitionerEntry.Release();
      this.partitionerEntry = (ScenePartitionerEntry) null;
    }
    if (this.reactable != null)
      this.reactable.Cleanup();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), (GameObject) null);
  }

  private class SuitMarkerReactable : Reactable
  {
    private SuitMarker suitMarker;
    private float startTime;

    public SuitMarkerReactable(SuitMarker suit_marker)
      : base(suit_marker.gameObject, (HashedString) nameof (SuitMarkerReactable), Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0.0f, 0.0f, float.PositiveInfinity)
    {
      this.suitMarker = suit_marker;
    }

    public override bool InternalCanBegin(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null)
        return false;
      if ((UnityEngine.Object) this.suitMarker == (UnityEngine.Object) null)
      {
        this.Cleanup();
        return false;
      }
      if (!this.suitMarker.isOperational)
        return false;
      int x = (int) transition.navGridTransition.x;
      if (x == 0)
        return false;
      if (new_reactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit))
        return (x >= 0 || !this.suitMarker.isRotated) && (x <= 0 || this.suitMarker.isRotated);
      if (x > 0 && this.suitMarker.isRotated || x < 0 && !this.suitMarker.isRotated)
        return false;
      return Grid.HasSuit(Grid.PosToCell((KMonoBehaviour) this.suitMarker), new_reactor.GetComponent<KPrefabID>().InstanceID);
    }

    protected override void InternalBegin()
    {
      this.startTime = Time.time;
      KBatchedAnimController component1 = this.reactor.GetComponent<KBatchedAnimController>();
      component1.AddAnimOverrides(this.suitMarker.interactAnim, 1f);
      component1.Play((HashedString) "working_pre", KAnim.PlayMode.Once, 1f, 0.0f);
      component1.Queue((HashedString) "working_loop", KAnim.PlayMode.Once, 1f, 0.0f);
      component1.Queue((HashedString) "working_pst", KAnim.PlayMode.Once, 1f, 0.0f);
      if (this.suitMarker.HasTag(GameTags.JetSuitBlocker))
      {
        KBatchedAnimController component2 = this.suitMarker.GetComponent<KBatchedAnimController>();
        component2.Play((HashedString) "working_pre", KAnim.PlayMode.Once, 1f, 0.0f);
        component2.Queue((HashedString) "working_loop", KAnim.PlayMode.Once, 1f, 0.0f);
        component2.Queue((HashedString) "working_pst", KAnim.PlayMode.Once, 1f, 0.0f);
      }
      this.suitMarker.CreateNewReactable();
    }

    public override void Update(float dt)
    {
      Facing facing = !(bool) ((UnityEngine.Object) this.reactor) ? (Facing) null : this.reactor.GetComponent<Facing>();
      if ((bool) ((UnityEngine.Object) facing) && (bool) ((UnityEngine.Object) this.suitMarker))
        facing.SetFacing(this.suitMarker.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
      if ((double) Time.time - (double) this.startTime <= 2.79999995231628)
        return;
      this.Run();
      this.Cleanup();
    }

    private void Run()
    {
      if ((UnityEngine.Object) this.reactor == (UnityEngine.Object) null || (UnityEngine.Object) this.suitMarker == (UnityEngine.Object) null)
        return;
      GameObject reactor = this.reactor;
      Equipment equipment = reactor.GetComponent<MinionIdentity>().GetEquipment();
      bool flag1 = !equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit);
      reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
      bool flag2 = false;
      Navigator component = reactor.GetComponent<Navigator>();
      bool flag3 = (UnityEngine.Object) component != (UnityEngine.Object) null && (component.flags & this.suitMarker.PathFlag) != PathFinder.PotentialPath.Flags.None;
      if (flag1 || flag3)
      {
        ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
        this.suitMarker.GetAttachedLockers((List<SuitLocker>) pooledList);
        foreach (SuitLocker suitLocker in (List<SuitLocker>) pooledList)
        {
          if ((UnityEngine.Object) suitLocker.GetFullyChargedOutfit() != (UnityEngine.Object) null && flag1)
          {
            suitLocker.EquipTo(equipment);
            flag2 = true;
            break;
          }
          if (!flag1 && suitLocker.CanDropOffSuit())
          {
            suitLocker.UnequipFrom(equipment);
            flag2 = true;
            break;
          }
        }
        if (flag1 && !flag2)
        {
          SuitLocker suitLocker1 = (SuitLocker) null;
          float num = 0.0f;
          foreach (SuitLocker suitLocker2 in (List<SuitLocker>) pooledList)
          {
            if ((double) suitLocker2.GetSuitScore() > (double) num)
            {
              suitLocker1 = suitLocker2;
              num = suitLocker2.GetSuitScore();
            }
          }
          if ((UnityEngine.Object) suitLocker1 != (UnityEngine.Object) null)
          {
            suitLocker1.EquipTo(equipment);
            flag2 = true;
          }
        }
        pooledList.Recycle();
      }
      if (flag2 || flag1)
        return;
      Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
      assignable.Unassign();
      Notification notification = new Notification((string) MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP), (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
      assignable.GetComponent<Notifier>().Add(notification, string.Empty);
    }

    protected override void InternalEnd()
    {
      if (!((UnityEngine.Object) this.reactor != (UnityEngine.Object) null))
        return;
      this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
    }

    protected override void InternalCleanup()
    {
    }
  }
}
