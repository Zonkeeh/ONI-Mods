// Decompiled with JetBrains decompiler
// Type: Uncoverable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Uncoverable : KMonoBehaviour
{
  [MyCmpReq]
  private OccupyArea occupyArea;
  [Serialize]
  private bool hasBeenUncovered;
  private HandleVector<int>.Handle partitionerEntry;

  private bool IsAnyCellShowing()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    OccupyArea occupyArea = this.occupyArea;
    int rootCell = cell;
    // ISSUE: reference to a compiler-generated field
    if (Uncoverable.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Uncoverable.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(Uncoverable.IsCellBlocked);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, object, bool> fMgCache0 = Uncoverable.\u003C\u003Ef__mg\u0024cache0;
    return !occupyArea.TestArea(rootCell, (object) null, fMgCache0);
  }

  private static bool IsCellBlocked(int cell, object data)
  {
    if (Grid.Element[cell].IsSolid)
      return !Grid.Foundation[cell];
    return false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.IsAnyCellShowing())
      this.hasBeenUncovered = true;
    if (this.hasBeenUncovered)
      return;
    this.GetComponent<KSelectable>().IsSelectable = false;
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Uncoverable.OnSpawn", (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
  }

  private void OnSolidChanged(object data)
  {
    if (!this.IsAnyCellShowing() || this.hasBeenUncovered || !this.partitionerEntry.IsValid())
      return;
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.hasBeenUncovered = true;
    this.GetComponent<KSelectable>().IsSelectable = true;
    string notification = (string) MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION;
    HashedString invalid = HashedString.Invalid;
    // ISSUE: reference to a compiler-generated field
    if (Uncoverable.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Uncoverable.\u003C\u003Ef__mg\u0024cache1 = new Func<List<Notification>, object, string>(Uncoverable.OnNotificationToolTip);
    }
    // ISSUE: reference to a compiler-generated field
    Func<List<Notification>, object, string> fMgCache1 = Uncoverable.\u003C\u003Ef__mg\u0024cache1;
    this.gameObject.AddOrGet<Notifier>().Add(new Notification(notification, NotificationType.Good, invalid, fMgCache1, (object) this, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null), string.Empty);
  }

  private static string OnNotificationToolTip(List<Notification> notifications, object data)
  {
    Uncoverable cmp = (Uncoverable) data;
    return MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION_TOOLTIP.Replace("{Uncoverable}", cmp.GetProperName());
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }
}
