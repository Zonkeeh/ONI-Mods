// Decompiled with JetBrains decompiler
// Type: Database.RoomTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

namespace Database
{
  public class RoomTypes : ResourceSet<RoomType>
  {
    public RoomType Neutral;
    public RoomType Latrine;
    public RoomType PlumbedBathroom;
    public RoomType Barracks;
    public RoomType Bedroom;
    public RoomType MessHall;
    public RoomType GreatHall;
    public RoomType Hospital;
    public RoomType MassageClinic;
    public RoomType PowerPlant;
    public RoomType Farm;
    public RoomType CreaturePen;
    public RoomType MachineShop;
    public RoomType RecRoom;
    public RoomType Park;
    public RoomType NatureReserve;

    public RoomTypes(ResourceSet parent)
      : base(nameof (RoomTypes), parent)
    {
      this.Initialize();
      this.Neutral = this.Add(new RoomType(nameof (Neutral), (string) ROOMS.TYPES.NEUTRAL.NAME, (string) ROOMS.TYPES.NEUTRAL.TOOLTIP, (string) ROOMS.TYPES.NEUTRAL.EFFECT, Db.Get().RoomTypeCategories.None, (RoomConstraints.Constraint) null, (RoomConstraints.Constraint[]) null, new RoomDetails.Detail[4]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT,
        RoomDetails.CREATURE_COUNT,
        RoomDetails.PLANT_COUNT
      }, 0, (RoomType[]) null, false, false, (string[]) null, 0));
      this.PlumbedBathroom = this.Add(new RoomType(nameof (PlumbedBathroom), (string) ROOMS.TYPES.PLUMBEDBATHROOM.NAME, (string) ROOMS.TYPES.PLUMBEDBATHROOM.TOOLTIP, (string) ROOMS.TYPES.PLUMBEDBATHROOM.EFFECT, Db.Get().RoomTypeCategories.Bathroom, RoomConstraints.FLUSH_TOILET, new RoomConstraints.Constraint[5]
      {
        RoomConstraints.ADVANCED_WASH_STATION,
        RoomConstraints.NO_OUTHOUSES,
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 1, (RoomType[]) null, false, false, new string[1]
      {
        "RoomBathroom"
      }, 2));
      this.Latrine = this.Add(new RoomType(nameof (Latrine), (string) ROOMS.TYPES.LATRINE.NAME, (string) ROOMS.TYPES.LATRINE.TOOLTIP, (string) ROOMS.TYPES.LATRINE.EFFECT, Db.Get().RoomTypeCategories.Bathroom, RoomConstraints.TOILET, new RoomConstraints.Constraint[4]
      {
        RoomConstraints.WASH_STATION,
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 1, new RoomType[1]{ this.PlumbedBathroom }, false, false, new string[1]
      {
        "RoomLatrine"
      }, 1));
      this.Bedroom = this.Add(new RoomType(nameof (Bedroom), (string) ROOMS.TYPES.BEDROOM.NAME, (string) ROOMS.TYPES.BEDROOM.TOOLTIP, (string) ROOMS.TYPES.BEDROOM.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.LUXURY_BED_SINGLE, new RoomConstraints.Constraint[6]
      {
        RoomConstraints.NO_COTS,
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64,
        RoomConstraints.DECORATIVE_ITEM,
        RoomConstraints.CEILING_HEIGHT_4
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 1, (RoomType[]) null, false, false, new string[1]
      {
        "RoomBedroom"
      }, 4));
      this.Barracks = this.Add(new RoomType(nameof (Barracks), (string) ROOMS.TYPES.BARRACKS.NAME, (string) ROOMS.TYPES.BARRACKS.TOOLTIP, (string) ROOMS.TYPES.BARRACKS.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.BED_SINGLE, new RoomConstraints.Constraint[3]
      {
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 1, new RoomType[1]{ this.Bedroom }, false, false, new string[1]
      {
        "RoomBarracks"
      }, 3));
      this.GreatHall = this.Add(new RoomType(nameof (GreatHall), (string) ROOMS.TYPES.GREATHALL.NAME, (string) ROOMS.TYPES.GREATHALL.TOOLTIP, (string) ROOMS.TYPES.GREATHALL.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.MESS_STATION_SINGLE, new RoomConstraints.Constraint[5]
      {
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_32,
        RoomConstraints.MAXIMUM_SIZE_120,
        RoomConstraints.DECORATIVE_ITEM_20,
        RoomConstraints.REC_BUILDING
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 1, (RoomType[]) null, false, false, new string[1]
      {
        "RoomGreatHall"
      }, 6));
      this.MessHall = this.Add(new RoomType(nameof (MessHall), (string) ROOMS.TYPES.MESSHALL.NAME, (string) ROOMS.TYPES.MESSHALL.TOOLTIP, (string) ROOMS.TYPES.MESSHALL.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.MESS_STATION_SINGLE, new RoomConstraints.Constraint[3]
      {
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 1, new RoomType[1]{ this.GreatHall }, false, false, new string[1]
      {
        "RoomMessHall"
      }, 5));
      this.MassageClinic = this.Add(new RoomType(nameof (MassageClinic), (string) ROOMS.TYPES.MASSAGE_CLINIC.NAME, (string) ROOMS.TYPES.MASSAGE_CLINIC.TOOLTIP, (string) ROOMS.TYPES.MASSAGE_CLINIC.EFFECT, Db.Get().RoomTypeCategories.Hospital, RoomConstraints.MASSAGE_TABLE, new RoomConstraints.Constraint[4]
      {
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.DECORATIVE_ITEM,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 2, (RoomType[]) null, true, true, (string[]) null, 7));
      this.Hospital = this.Add(new RoomType(nameof (Hospital), (string) ROOMS.TYPES.HOSPITAL.NAME, (string) ROOMS.TYPES.HOSPITAL.TOOLTIP, (string) ROOMS.TYPES.HOSPITAL.EFFECT, Db.Get().RoomTypeCategories.Hospital, RoomConstraints.CLINIC, new RoomConstraints.Constraint[5]
      {
        RoomConstraints.TOILET,
        RoomConstraints.MESS_STATION_SINGLE,
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_96
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 2, (RoomType[]) null, true, true, (string[]) null, 8));
      this.PowerPlant = this.Add(new RoomType(nameof (PowerPlant), (string) ROOMS.TYPES.POWER_PLANT.NAME, (string) ROOMS.TYPES.POWER_PLANT.TOOLTIP, (string) ROOMS.TYPES.POWER_PLANT.EFFECT, Db.Get().RoomTypeCategories.Industrial, RoomConstraints.POWER_STATION, new RoomConstraints.Constraint[2]
      {
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_96
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 2, (RoomType[]) null, true, true, (string[]) null, 9));
      this.Farm = this.Add(new RoomType(nameof (Farm), (string) ROOMS.TYPES.FARM.NAME, (string) ROOMS.TYPES.FARM.TOOLTIP, (string) ROOMS.TYPES.FARM.EFFECT, Db.Get().RoomTypeCategories.Agricultural, RoomConstraints.FARM_STATION, new RoomConstraints.Constraint[2]
      {
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_96
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 2, (RoomType[]) null, true, true, (string[]) null, 10));
      this.CreaturePen = this.Add(new RoomType(nameof (CreaturePen), (string) ROOMS.TYPES.CREATUREPEN.NAME, (string) ROOMS.TYPES.CREATUREPEN.TOOLTIP, (string) ROOMS.TYPES.CREATUREPEN.EFFECT, Db.Get().RoomTypeCategories.Agricultural, RoomConstraints.RANCH_STATION, new RoomConstraints.Constraint[2]
      {
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_96
      }, new RoomDetails.Detail[3]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT,
        RoomDetails.CREATURE_COUNT
      }, 2, (RoomType[]) null, true, true, (string[]) null, 11));
      this.MachineShop = new RoomType(nameof (MachineShop), (string) ROOMS.TYPES.MACHINE_SHOP.NAME, (string) ROOMS.TYPES.MACHINE_SHOP.TOOLTIP, (string) ROOMS.TYPES.MACHINE_SHOP.EFFECT, Db.Get().RoomTypeCategories.Industrial, RoomConstraints.MACHINE_SHOP, new RoomConstraints.Constraint[2]
      {
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_96
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 2, (RoomType[]) null, true, true, (string[]) null, 12);
      this.RecRoom = this.Add(new RoomType(nameof (RecRoom), (string) ROOMS.TYPES.REC_ROOM.NAME, (string) ROOMS.TYPES.REC_ROOM.TOOLTIP, (string) ROOMS.TYPES.REC_ROOM.EFFECT, Db.Get().RoomTypeCategories.Recreation, RoomConstraints.REC_BUILDING, new RoomConstraints.Constraint[4]
      {
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.DECORATIVE_ITEM,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64
      }, new RoomDetails.Detail[2]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT
      }, 0, (RoomType[]) null, true, true, (string[]) null, 13));
      this.NatureReserve = this.Add(new RoomType(nameof (NatureReserve), (string) ROOMS.TYPES.NATURERESERVE.NAME, (string) ROOMS.TYPES.NATURERESERVE.TOOLTIP, (string) ROOMS.TYPES.NATURERESERVE.EFFECT, Db.Get().RoomTypeCategories.Park, RoomConstraints.PARK_BUILDING, new RoomConstraints.Constraint[4]
      {
        RoomConstraints.WILDPLANTS,
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_32,
        RoomConstraints.MAXIMUM_SIZE_120
      }, new RoomDetails.Detail[4]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT,
        RoomDetails.CREATURE_COUNT,
        RoomDetails.PLANT_COUNT
      }, 1, (RoomType[]) null, false, false, new string[1]
      {
        "RoomNatureReserve"
      }, 15));
      this.Park = this.Add(new RoomType(nameof (Park), (string) ROOMS.TYPES.PARK.NAME, (string) ROOMS.TYPES.PARK.TOOLTIP, (string) ROOMS.TYPES.PARK.EFFECT, Db.Get().RoomTypeCategories.Park, RoomConstraints.PARK_BUILDING, new RoomConstraints.Constraint[4]
      {
        RoomConstraints.WILDPLANT,
        RoomConstraints.NO_INDUSTRIAL_MACHINERY,
        RoomConstraints.MINIMUM_SIZE_12,
        RoomConstraints.MAXIMUM_SIZE_64
      }, new RoomDetails.Detail[4]
      {
        RoomDetails.SIZE,
        RoomDetails.BUILDING_COUNT,
        RoomDetails.CREATURE_COUNT,
        RoomDetails.PLANT_COUNT
      }, 1, new RoomType[1]{ this.NatureReserve }, false, false, new string[1]
      {
        "RoomPark"
      }, 14));
    }

    public Assignables[] GetAssignees(Room room)
    {
      if (room == null)
        return new Assignables[0];
      RoomType roomType = room.roomType;
      if (roomType.primary_constraint == null)
        return new Assignables[0];
      List<Assignables> assignablesList = new List<Assignables>();
      foreach (KPrefabID building in room.buildings)
      {
        if (!((UnityEngine.Object) building == (UnityEngine.Object) null) && roomType.primary_constraint.building_criteria(building))
        {
          Assignable component = building.GetComponent<Assignable>();
          if (component.assignee != null)
          {
            foreach (Ownables owner in component.assignee.GetOwners())
            {
              if (!assignablesList.Contains((Assignables) owner))
                assignablesList.Add((Assignables) owner);
            }
          }
        }
      }
      return assignablesList.ToArray();
    }

    public RoomType GetRoomTypeForID(string id)
    {
      foreach (RoomType resource in this.resources)
      {
        if (resource.Id == id)
          return resource;
      }
      return (RoomType) null;
    }

    public RoomType GetRoomType(Room room)
    {
      foreach (RoomType resource1 in this.resources)
      {
        if (resource1 != this.Neutral && resource1.isSatisfactory(room) == RoomType.RoomIdentificationResult.all_satisfied)
        {
          bool flag = false;
          foreach (RoomType resource2 in this.resources)
          {
            if (resource1 != resource2 && resource2 != this.Neutral && this.HasAmbiguousRoomType(room, resource1, resource2))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return resource1;
        }
      }
      return this.Neutral;
    }

    public bool HasAmbiguousRoomType(Room room, RoomType suspected_type, RoomType potential_type)
    {
      RoomType.RoomIdentificationResult identificationResult1 = potential_type.isSatisfactory(room);
      RoomType.RoomIdentificationResult identificationResult2 = suspected_type.isSatisfactory(room);
      if (identificationResult1 == RoomType.RoomIdentificationResult.all_satisfied && identificationResult2 == RoomType.RoomIdentificationResult.all_satisfied)
      {
        if (potential_type.priority > suspected_type.priority || suspected_type.upgrade_paths != null && Array.IndexOf<RoomType>(suspected_type.upgrade_paths, potential_type) != -1)
          return true;
        if (potential_type.upgrade_paths != null && Array.IndexOf<RoomType>(potential_type.upgrade_paths, suspected_type) != -1)
          return false;
      }
      if (identificationResult1 != RoomType.RoomIdentificationResult.primary_unsatisfied && (suspected_type.upgrade_paths == null || Array.IndexOf<RoomType>(suspected_type.upgrade_paths, potential_type) == -1))
      {
        if (suspected_type.primary_constraint == potential_type.primary_constraint)
        {
          suspected_type = this.Neutral;
        }
        else
        {
          bool flag = false;
          if (suspected_type.primary_constraint.stomp_in_conflict != null && suspected_type.primary_constraint.stomp_in_conflict.Contains(potential_type.primary_constraint))
            flag = true;
          else if (suspected_type.additional_constraints != null)
          {
            foreach (RoomConstraints.Constraint additionalConstraint in suspected_type.additional_constraints)
            {
              if (additionalConstraint == potential_type.primary_constraint || additionalConstraint.stomp_in_conflict != null && additionalConstraint.stomp_in_conflict.Contains(potential_type.primary_constraint))
              {
                flag = true;
                break;
              }
            }
          }
          return !flag;
        }
      }
      return false;
    }

    public RoomType[] GetPossibleRoomTypes(Room room)
    {
      RoomType[] array = new RoomType[this.Count];
      int newSize = 0;
      foreach (RoomType resource in this.resources)
      {
        if (resource != this.Neutral && (resource.isSatisfactory(room) == RoomType.RoomIdentificationResult.all_satisfied || resource.isSatisfactory(room) == RoomType.RoomIdentificationResult.primary_satisfied))
        {
          array[newSize] = resource;
          ++newSize;
        }
      }
      if (newSize == 0)
      {
        array[newSize] = this.Neutral;
        ++newSize;
      }
      Array.Resize<RoomType>(ref array, newSize);
      return array;
    }
  }
}
