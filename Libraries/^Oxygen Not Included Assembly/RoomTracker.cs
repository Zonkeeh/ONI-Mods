// Decompiled with JetBrains decompiler
// Type: RoomTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

public class RoomTracker : KMonoBehaviour, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<RoomTracker> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<RoomTracker>((System.Action<RoomTracker, object>) ((component, data) => component.OnUpdateRoom(data)));
  public RoomTracker.Requirement requirement;
  public string requiredRoomType;
  public string customStatusItemID;
  private Guid statusItemGuid;

  public Room room { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Debug.Assert(!string.IsNullOrEmpty(this.requiredRoomType) && this.requiredRoomType != Db.Get().RoomTypes.Neutral.Id, (object) "RoomTracker must have a requiredRoomType!");
    this.Subscribe<RoomTracker>(144050788, RoomTracker.OnUpdateRoomDelegate);
    this.FindAndSetRoom();
  }

  public void FindAndSetRoom()
  {
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.gameObject));
    if (cavityForCell != null && cavityForCell.room != null)
      this.OnUpdateRoom((object) cavityForCell.room);
    else
      this.OnUpdateRoom((object) null);
  }

  public bool IsInCorrectRoom()
  {
    if (this.room != null)
      return this.room.roomType.Id == this.requiredRoomType;
    return false;
  }

  public bool SufficientBuildLocation(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    if (this.requirement == RoomTracker.Requirement.Required || this.requirement == RoomTracker.Requirement.CustomRequired)
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
      if ((cavityForCell == null ? (Room) null : cavityForCell.room) == null)
        return false;
    }
    return true;
  }

  private void OnUpdateRoom(object data)
  {
    this.room = (Room) data;
    if (this.room == null || this.room.roomType.Id != this.requiredRoomType)
    {
      switch (this.requirement)
      {
        case RoomTracker.Requirement.TrackingOnly:
          this.statusItemGuid = this.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
          break;
        case RoomTracker.Requirement.Recommended:
          this.statusItemGuid = this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRecommendedRoom, (object) this.requiredRoomType);
          break;
        case RoomTracker.Requirement.Required:
          this.statusItemGuid = this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRequiredRoom, (object) this.requiredRoomType);
          break;
        case RoomTracker.Requirement.CustomRecommended:
        case RoomTracker.Requirement.CustomRequired:
          this.statusItemGuid = this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.Get(this.customStatusItemID), (object) this.requiredRoomType);
          break;
      }
    }
    else
      this.statusItemGuid = this.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (!string.IsNullOrEmpty(this.requiredRoomType))
    {
      string name = Db.Get().RoomTypes.Get(this.requiredRoomType).Name;
      switch (this.requirement)
      {
        case RoomTracker.Requirement.Recommended:
        case RoomTracker.Requirement.CustomRecommended:
          descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.PREFERS_ROOM, (object) name), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.PREFERS_ROOM, (object) name), Descriptor.DescriptorType.Requirement, false));
          break;
        case RoomTracker.Requirement.Required:
        case RoomTracker.Requirement.CustomRequired:
          descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.REQUIRESROOM, (object) name), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESROOM, (object) name), Descriptor.DescriptorType.Requirement, false));
          break;
      }
    }
    return descriptorList;
  }

  public enum Requirement
  {
    TrackingOnly,
    Recommended,
    Required,
    CustomRecommended,
    CustomRequired,
  }
}
