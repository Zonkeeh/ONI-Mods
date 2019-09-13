// Decompiled with JetBrains decompiler
// Type: RoomConstraints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

public static class RoomConstraints
{
  public static RoomConstraints.Constraint CEILING_HEIGHT_4 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 4), 1, string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.NAME, (object) "4"), string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, (object) "4"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint CEILING_HEIGHT_6 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 6), 1, string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.NAME, (object) "6"), string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, (object) "6"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MINIMUM_SIZE_12 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells >= 12), 1, string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.NAME, (object) "12"), string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, (object) "12"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MINIMUM_SIZE_32 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells >= 32), 1, string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.NAME, (object) "32"), string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, (object) "32"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MAXIMUM_SIZE_64 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells <= 64), 1, string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "64"), string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "64"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MAXIMUM_SIZE_96 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells <= 96), 1, string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "96"), string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "96"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MAXIMUM_SIZE_120 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells <= 120), 1, string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "120"), string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "120"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint NO_INDUSTRIAL_MACHINERY = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
        return false;
    }
    return true;
  }), 1, (string) ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME, (string) ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint NO_COTS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.Bed) && !building.HasTag(RoomConstraints.ConstraintTags.LuxuryBed))
        return false;
    }
    return true;
  }), 1, (string) ROOMS.CRITERIA.NO_COTS.NAME, (string) ROOMS.CRITERIA.NO_COTS.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint NO_OUTHOUSES = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.Toilet) && !building.HasTag(RoomConstraints.ConstraintTags.FlushToilet))
        return false;
    }
    return true;
  }), 1, (string) ROOMS.CRITERIA.NO_OUTHOUSES.NAME, (string) ROOMS.CRITERIA.NO_OUTHOUSES.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint LUXURY_BED_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBed)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.LUXURY_BED_SINGLE.NAME, (string) ROOMS.CRITERIA.LUXURY_BED_SINGLE.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint BED_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc =>
  {
    if (bc.HasTag(RoomConstraints.ConstraintTags.Bed))
      return !bc.HasTag(RoomConstraints.ConstraintTags.Clinic);
    return false;
  }), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.BED_SINGLE.NAME, (string) ROOMS.CRITERIA.BED_SINGLE.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint BUILDING_DECOR_POSITIVE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc =>
  {
    DecorProvider component = bc.GetComponent<DecorProvider>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.baseDecor > 0.0;
  }), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME, (string) ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint DECORATIVE_ITEM = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(GameTags.Decoration)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, (string) ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint DECORATIVE_ITEM_20 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc =>
  {
    if (bc.HasTag(GameTags.Decoration))
      return bc.HasTag(RoomConstraints.ConstraintTags.Decor20);
    return false;
  }), (Func<Room, bool>) null, 1, string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM_N.NAME, (object) "20"), string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM_N.DESCRIPTION, (object) "20"), (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint POWER_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.PowerStation)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.POWER_STATION.NAME, (string) ROOMS.CRITERIA.POWER_STATION.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint FARM_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FarmStation)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.FARM_STATION.NAME, (string) ROOMS.CRITERIA.FARM_STATION.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint RANCH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.RanchStation)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.RANCH_STATION.NAME, (string) ROOMS.CRITERIA.RANCH_STATION.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint REC_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.RecBuilding)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.REC_BUILDING.NAME, (string) ROOMS.CRITERIA.REC_BUILDING.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MACHINE_SHOP = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.MachineShop)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.MACHINE_SHOP.NAME, (string) ROOMS.CRITERIA.MACHINE_SHOP.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint FOOD_BOX = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FoodStorage)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.FOOD_BOX.NAME, (string) ROOMS.CRITERIA.FOOD_BOX.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint LIGHT = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID creature in room.cavity.creatures)
    {
      if ((UnityEngine.Object) creature != (UnityEngine.Object) null && (UnityEngine.Object) creature.GetComponent<Light2D>() != (UnityEngine.Object) null)
        return true;
    }
    foreach (KPrefabID building in room.buildings)
    {
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null))
      {
        Light2D component1 = building.GetComponent<Light2D>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          RequireInputs component2 = building.GetComponent<RequireInputs>();
          return component1.enabled || (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.RequirementsMet;
        }
      }
    }
    return false;
  }), 1, (string) ROOMS.CRITERIA.LIGHT.NAME, (string) ROOMS.CRITERIA.LIGHT.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MASSAGE_TABLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.MassageTable)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.MASSAGE_TABLE.NAME, (string) ROOMS.CRITERIA.MASSAGE_TABLE.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint MESS_STATION_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.MessTable)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.MESS_STATION_SINGLE.NAME, (string) ROOMS.CRITERIA.MESS_STATION_SINGLE.DESCRIPTION, new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.REC_BUILDING
  });
  public static RoomConstraints.Constraint RESEARCH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.ResearchStation)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.RESEARCH_STATION.NAME, (string) ROOMS.CRITERIA.RESEARCH_STATION.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint TOILET = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Toilet)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.TOILET.NAME, (string) ROOMS.CRITERIA.TOILET.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint FLUSH_TOILET = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FlushToilet)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.FLUSH_TOILET.NAME, (string) ROOMS.CRITERIA.FLUSH_TOILET.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint WASH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.WashStation)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.WASH_STATION.NAME, (string) ROOMS.CRITERIA.WASH_STATION.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint ADVANCED_WASH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.AdvancedWashStation)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.ADVANCED_WASH_STATION.NAME, (string) ROOMS.CRITERIA.ADVANCED_WASH_STATION.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint CLINIC = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Clinic)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.CLINIC.NAME, (string) ROOMS.CRITERIA.CLINIC.DESCRIPTION, new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.TOILET,
    RoomConstraints.FLUSH_TOILET,
    RoomConstraints.MESS_STATION_SINGLE
  });
  public static RoomConstraints.Constraint PARK_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Park)), (Func<Room, bool>) null, 1, (string) ROOMS.CRITERIA.PARK_BUILDING.NAME, (string) ROOMS.CRITERIA.PARK_BUILDING.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint ORIGINALTILES = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 4), 1, string.Empty, string.Empty, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint WILDANIMAL = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.creatures.Count + room.cavity.eggs.Count > 0), 1, (string) ROOMS.CRITERIA.WILDANIMAL.NAME, (string) ROOMS.CRITERIA.WILDANIMAL.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint WILDANIMALS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID creature in room.cavity.creatures)
    {
      if (creature.HasTag(GameTags.Creatures.Wild))
        ++num;
    }
    return num >= 2;
  }), 1, (string) ROOMS.CRITERIA.WILDANIMALS.NAME, (string) ROOMS.CRITERIA.WILDANIMALS.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint WILDPLANT = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID plant in room.cavity.plants)
    {
      if ((UnityEngine.Object) plant != (UnityEngine.Object) null)
      {
        BasicForagePlantPlanted component1 = plant.GetComponent<BasicForagePlantPlanted>();
        ReceptacleMonitor component2 = plant.GetComponent<ReceptacleMonitor>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.Replanted)
          ++num;
        else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          ++num;
      }
    }
    return num >= 2;
  }), 1, (string) ROOMS.CRITERIA.WILDPLANT.NAME, (string) ROOMS.CRITERIA.WILDPLANT.DESCRIPTION, (List<RoomConstraints.Constraint>) null);
  public static RoomConstraints.Constraint WILDPLANTS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID plant in room.cavity.plants)
    {
      if ((UnityEngine.Object) plant != (UnityEngine.Object) null)
      {
        BasicForagePlantPlanted component1 = plant.GetComponent<BasicForagePlantPlanted>();
        ReceptacleMonitor component2 = plant.GetComponent<ReceptacleMonitor>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.Replanted)
          ++num;
        else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          ++num;
      }
    }
    return num >= 4;
  }), 1, (string) ROOMS.CRITERIA.WILDPLANTS.NAME, (string) ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION, (List<RoomConstraints.Constraint>) null);

  public static string RoomCriteriaString(Room room)
  {
    string empty = string.Empty;
    RoomType roomType = room.roomType;
    string str;
    if (roomType != Db.Get().RoomTypes.Neutral)
    {
      str = empty + "<b>" + (string) ROOMS.CRITERIA.HEADER + "</b>" + "\n    • " + roomType.primary_constraint.name;
      if (roomType.additional_constraints != null)
      {
        foreach (RoomConstraints.Constraint additionalConstraint in roomType.additional_constraints)
          str = !additionalConstraint.isSatisfied(room) ? str + "\n<color=#F44A47FF>    • " + additionalConstraint.name + "</color>" : str + "\n    • " + additionalConstraint.name;
      }
    }
    else
    {
      RoomType[] possibleRoomTypes = Db.Get().RoomTypes.GetPossibleRoomTypes(room);
      str = empty + (possibleRoomTypes.Length <= 1 ? string.Empty : "<b>" + (string) ROOMS.CRITERIA.POSSIBLE_TYPES_HEADER + "</b>");
      foreach (RoomType suspected_type in possibleRoomTypes)
      {
        if (suspected_type != Db.Get().RoomTypes.Neutral)
        {
          if (str != string.Empty)
            str += "\n";
          str = str + "<b><color=#BCBCBC>    • " + suspected_type.Name + "</b> (" + suspected_type.primary_constraint.name + ")</color>";
          bool flag1 = false;
          if (suspected_type.additional_constraints != null)
          {
            foreach (RoomConstraints.Constraint additionalConstraint in suspected_type.additional_constraints)
            {
              if (!additionalConstraint.isSatisfied(room))
              {
                flag1 = true;
                str = additionalConstraint.building_criteria == null ? str + "\n<color=#F44A47FF>        • " + string.Format((string) ROOMS.CRITERIA.CRITERIA_FAILED.FAILED, (object) additionalConstraint.name) + "</color>" : str + "\n<color=#F44A47FF>        • " + string.Format((string) ROOMS.CRITERIA.CRITERIA_FAILED.MISSING_BUILDING, (object) additionalConstraint.name) + "</color>";
              }
            }
          }
          if (!flag1)
          {
            bool flag2 = false;
            foreach (RoomType resource in Db.Get().RoomTypes.resources)
            {
              if (resource != suspected_type && resource != Db.Get().RoomTypes.Neutral && Db.Get().RoomTypes.HasAmbiguousRoomType(room, suspected_type, resource))
              {
                flag2 = true;
                break;
              }
            }
            if (flag2)
              str = str + "\n<color=#F44A47FF>        • " + (string) ROOMS.CRITERIA.NO_TYPE_CONFLICTS + "</color>";
          }
        }
      }
    }
    return str;
  }

  public static class ConstraintTags
  {
    public static Tag Bed = nameof (Bed).ToTag();
    public static Tag LuxuryBed = nameof (LuxuryBed).ToTag();
    public static Tag Toilet = nameof (Toilet).ToTag();
    public static Tag FlushToilet = nameof (FlushToilet).ToTag();
    public static Tag MessTable = nameof (MessTable).ToTag();
    public static Tag Clinic = nameof (Clinic).ToTag();
    public static Tag FoodStorage = nameof (FoodStorage).ToTag();
    public static Tag WashStation = nameof (WashStation).ToTag();
    public static Tag AdvancedWashStation = nameof (AdvancedWashStation).ToTag();
    public static Tag ResearchStation = nameof (ResearchStation).ToTag();
    public static Tag LightSource = nameof (LightSource).ToTag();
    public static Tag MassageTable = nameof (MassageTable).ToTag();
    public static Tag IndustrialMachinery = nameof (IndustrialMachinery).ToTag();
    public static Tag PowerStation = nameof (PowerStation).ToTag();
    public static Tag FarmStation = nameof (FarmStation).ToTag();
    public static Tag CreatureRelocator = nameof (CreatureRelocator).ToTag();
    public static Tag CreatureFeeder = nameof (CreatureFeeder).ToTag();
    public static Tag RanchStation = nameof (RanchStation).ToTag();
    public static Tag RecBuilding = nameof (RecBuilding).ToTag();
    public static Tag MachineShop = nameof (MachineShop).ToTag();
    public static Tag Park = nameof (Park).ToTag();
    public static Tag NatureReserve = nameof (NatureReserve).ToTag();
    public static Tag Decor20 = nameof (Decor20).ToTag();
  }

  public class Constraint
  {
    public int times_required = 1;
    public string name;
    public string description;
    public Func<Room, bool> room_criteria;
    public Func<KPrefabID, bool> building_criteria;
    public List<RoomConstraints.Constraint> stomp_in_conflict;

    public Constraint(
      Func<KPrefabID, bool> building_criteria,
      Func<Room, bool> room_criteria,
      int times_required = 1,
      string name = "",
      string description = "",
      List<RoomConstraints.Constraint> stomp_in_conflict = null)
    {
      this.room_criteria = room_criteria;
      this.building_criteria = building_criteria;
      this.times_required = times_required;
      this.name = name;
      this.description = description;
      this.stomp_in_conflict = stomp_in_conflict;
    }

    public bool isSatisfied(Room room)
    {
      int num = 0;
      if (this.room_criteria != null && this.room_criteria(room))
        ++num;
      if (this.building_criteria != null)
      {
        foreach (KPrefabID building in room.buildings)
        {
          if (!((UnityEngine.Object) building == (UnityEngine.Object) null) && this.building_criteria(building))
            ++num;
        }
        foreach (KPrefabID plant in room.plants)
        {
          if (!((UnityEngine.Object) plant == (UnityEngine.Object) null) && this.building_criteria(plant))
            ++num;
        }
      }
      return num >= this.times_required;
    }
  }
}
