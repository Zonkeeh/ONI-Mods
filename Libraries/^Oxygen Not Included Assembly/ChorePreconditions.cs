// Decompiled with JetBrains decompiler
// Type: ChorePreconditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ChorePreconditions
{
  public Chore.Precondition IsPreemptable = new Chore.Precondition()
  {
    id = nameof (IsPreemptable),
    sortOrder = 1,
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PREEMPTABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (!context.isAttemptingOverride && !context.chore.CanPreempt(context))
        return (Object) context.chore.driver == (Object) null;
      return true;
    })
  };
  public Chore.Precondition HasUrge = new Chore.Precondition()
  {
    id = nameof (HasUrge),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_URGE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.chore.choreType.urge == null)
        return true;
      foreach (Urge urge in context.consumerState.consumer.GetUrges())
      {
        if (context.chore.SatisfiesUrge(urge))
          return true;
      }
      return false;
    })
  };
  public Chore.Precondition IsValid = new Chore.Precondition()
  {
    id = nameof (IsValid),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_VALID,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.chore.IsValid())
  };
  public Chore.Precondition IsPermitted = new Chore.Precondition()
  {
    id = nameof (IsPermitted),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PERMITTED,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.consumer.IsPermittedOrEnabled(context.choreTypeForPermission, context.chore))
  };
  public Chore.Precondition IsAssignedtoMe = new Chore.Precondition()
  {
    id = "IsAssignedToMe",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_ASSIGNED_TO_ME,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((Assignable) data).IsAssignedTo(context.consumerState.gameObject.GetComponent<IAssignableIdentity>()))
  };
  public Chore.Precondition IsInMyRoom = new Chore.Precondition()
  {
    id = nameof (IsInMyRoom),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_IN_MY_ROOM,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      CavityInfo cavityForCell1 = Game.Instance.roomProber.GetCavityForCell((int) data);
      Room room1 = (Room) null;
      if (cavityForCell1 != null)
        room1 = cavityForCell1.room;
      if (room1 != null)
      {
        if ((Object) context.consumerState.ownable != (Object) null)
        {
          foreach (Component owner in room1.GetOwners())
          {
            if ((Object) owner.gameObject == (Object) context.consumerState.gameObject)
              return true;
          }
        }
        else
        {
          Room room2 = (Room) null;
          FetchChore chore = context.chore as FetchChore;
          if (chore != null && (Object) chore.destination != (Object) null)
          {
            CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((KMonoBehaviour) chore.destination));
            if (cavityForCell2 != null)
              room2 = cavityForCell2.room;
            if (room2 != null)
              return room2 == room1;
            return false;
          }
          if (!(context.chore is WorkChore<Tinkerable>))
            return false;
          CavityInfo cavityForCell3 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((context.chore as WorkChore<Tinkerable>).gameObject));
          if (cavityForCell3 != null)
            room2 = cavityForCell3.room;
          if (room2 != null)
            return room2 == room1;
          return false;
        }
      }
      return false;
    })
  };
  public Chore.Precondition IsPreferredAssignable = new Chore.Precondition()
  {
    id = nameof (IsPreferredAssignable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Assignable assignable = (Assignable) data;
      return Game.Instance.assignmentManager.GetPreferredAssignables(context.consumerState.assignables, assignable.slot).Contains(assignable);
    })
  };
  public Chore.Precondition IsPreferredAssignableOrUrgentBladder = new Chore.Precondition()
  {
    id = "IsPreferredAssignableOrUrgent",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Assignable assignable = (Assignable) data;
      if (Game.Instance.assignmentManager.GetPreferredAssignables(context.consumerState.assignables, assignable.slot).Contains(assignable))
        return true;
      PeeChoreMonitor.Instance smi = context.consumerState.gameObject.GetSMI<PeeChoreMonitor.Instance>();
      if (smi == null)
        return false;
      return smi.IsInsideState((StateMachine.BaseState) smi.sm.critical);
    })
  };
  public Chore.Precondition IsNotTransferArm = new Chore.Precondition()
  {
    id = nameof (IsNotTransferArm),
    description = string.Empty,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !context.consumerState.hasSolidTransferArm)
  };
  public Chore.Precondition HasSkillPerk = new Chore.Precondition()
  {
    id = nameof (HasSkillPerk),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SKILL_PERK,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      MinionResume resume = context.consumerState.resume;
      if (!(bool) ((Object) resume))
        return false;
      if (data is SkillPerk)
      {
        SkillPerk perk = data as SkillPerk;
        return resume.HasPerk(perk);
      }
      if (data is HashedString)
      {
        HashedString perkId = (HashedString) data;
        return resume.HasPerk(perkId);
      }
      if (!(data is string))
        return false;
      HashedString perkId1 = (HashedString) ((string) data);
      return resume.HasPerk(perkId1);
    })
  };
  public Chore.Precondition IsMoreSatisfyingEarly = new Chore.Precondition()
  {
    id = nameof (IsMoreSatisfyingEarly),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING,
    sortOrder = -1,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.isAttemptingOverride || context.skipMoreSatisfyingEarlyPrecondition || context.consumerState.selectable.IsSelected)
        return true;
      Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
      if (currentChore == null)
        return true;
      if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
        return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
      if ((Object) context.consumerState.consumer != (Object) null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
        return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
      if (context.masterPriority.priority_value != currentChore.masterPriority.priority_value)
        return context.masterPriority.priority_value > currentChore.masterPriority.priority_value;
      return context.priority > currentChore.choreType.priority;
    })
  };
  public Chore.Precondition IsMoreSatisfyingLate = new Chore.Precondition()
  {
    id = nameof (IsMoreSatisfyingLate),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING,
    sortOrder = 10000,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.isAttemptingOverride || !context.consumerState.selectable.IsSelected)
        return true;
      Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
      if (currentChore == null)
        return true;
      if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
        return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
      if ((Object) context.consumerState.consumer != (Object) null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
        return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
      if (context.masterPriority.priority_value != currentChore.masterPriority.priority_value)
        return context.masterPriority.priority_value > currentChore.masterPriority.priority_value;
      return context.priority > currentChore.choreType.priority;
    })
  };
  public Chore.Precondition IsChattable = new Chore.Precondition()
  {
    id = "CanChat",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_CHAT,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      KMonoBehaviour kmonoBehaviour = (KMonoBehaviour) data;
      if ((Object) context.consumerState.consumer == (Object) null || (Object) context.consumerState.navigator == (Object) null || (Object) kmonoBehaviour == (Object) null)
        return false;
      return context.consumerState.navigator.CanReach((IApproachable) kmonoBehaviour.GetComponent<Chattable>());
    })
  };
  public Chore.Precondition IsNotRedAlert = new Chore.Precondition()
  {
    id = nameof (IsNotRedAlert),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_RED_ALERT,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority)
        return true;
      return !VignetteManager.Instance.Get().IsRedAlert();
    })
  };
  public Chore.Precondition IsScheduledTime = new Chore.Precondition()
  {
    id = nameof (IsScheduledTime),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_SCHEDULED_TIME,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (VignetteManager.Instance.Get().IsRedAlert())
        return true;
      ScheduleBlockType type = (ScheduleBlockType) data;
      ScheduleBlock scheduleBlock = context.consumerState.scheduleBlock;
      if (scheduleBlock != null)
        return scheduleBlock.IsAllowed(type);
      return true;
    })
  };
  public Chore.Precondition CanMoveTo = new Chore.Precondition()
  {
    id = nameof (CanMoveTo),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((Object) context.consumerState.consumer == (Object) null)
        return false;
      KMonoBehaviour kmonoBehaviour = (KMonoBehaviour) data;
      if ((Object) kmonoBehaviour == (Object) null)
        return false;
      IApproachable approachable = (IApproachable) kmonoBehaviour;
      int cost;
      if (!context.consumerState.consumer.GetNavigationCost(approachable, out cost))
        return false;
      context.cost += cost;
      return true;
    })
  };
  public Chore.Precondition CanPickup = new Chore.Precondition()
  {
    id = nameof (CanPickup),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_PICKUP,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Pickupable cmp = (Pickupable) data;
      if ((Object) cmp == (Object) null || (Object) context.consumerState.consumer == (Object) null || (cmp.HasTag(GameTags.StoredPrivate) || !cmp.CouldBePickedUpByMinion(context.consumerState.gameObject)))
        return false;
      return context.consumerState.consumer.CanReach((IApproachable) cmp);
    })
  };
  public Chore.Precondition IsAwake = new Chore.Precondition()
  {
    id = nameof (IsAwake),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_AWAKE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((Object) context.consumerState.consumer == (Object) null)
        return false;
      StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
      return !smi.IsInsideState((StateMachine.BaseState) smi.sm.sleepy.sleeping);
    })
  };
  public Chore.Precondition IsStanding = new Chore.Precondition()
  {
    id = nameof (IsStanding),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_STANDING,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((Object) context.consumerState.consumer == (Object) null || (Object) context.consumerState.navigator == (Object) null)
        return false;
      return context.consumerState.navigator.CurrentNavType == NavType.Floor;
    })
  };
  public Chore.Precondition IsMoving = new Chore.Precondition()
  {
    id = nameof (IsMoving),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MOVING,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((Object) context.consumerState.consumer == (Object) null || (Object) context.consumerState.navigator == (Object) null)
        return false;
      return context.consumerState.navigator.IsMoving();
    })
  };
  public Chore.Precondition IsOffLadder = new Chore.Precondition()
  {
    id = nameof (IsOffLadder),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_OFF_LADDER,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((Object) context.consumerState.consumer == (Object) null || (Object) context.consumerState.navigator == (Object) null || context.consumerState.navigator.CurrentNavType == NavType.Ladder)
        return false;
      return context.consumerState.navigator.CurrentNavType != NavType.Pole;
    })
  };
  public Chore.Precondition NotInTube = new Chore.Precondition()
  {
    id = nameof (NotInTube),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NOT_IN_TUBE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((Object) context.consumerState.consumer == (Object) null || (Object) context.consumerState.navigator == (Object) null)
        return false;
      return context.consumerState.navigator.CurrentNavType != NavType.Tube;
    })
  };
  public Chore.Precondition ConsumerHasTrait = new Chore.Precondition()
  {
    id = nameof (ConsumerHasTrait),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_TRAIT,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      string trait_id = (string) data;
      Traits traits = context.consumerState.traits;
      if ((Object) traits == (Object) null)
        return false;
      return traits.HasTrait(trait_id);
    })
  };
  public Chore.Precondition IsOperational = new Chore.Precondition()
  {
    id = nameof (IsOperational),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_OPERATIONAL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as Operational).IsOperational)
  };
  public Chore.Precondition IsNotMarkedForDeconstruction = new Chore.Precondition()
  {
    id = nameof (IsNotMarkedForDeconstruction),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Deconstructable deconstructable = data as Deconstructable;
      if (!((Object) deconstructable == (Object) null))
        return !deconstructable.IsMarkedForDeconstruction();
      return true;
    })
  };
  public Chore.Precondition IsNotMarkedForDisable = new Chore.Precondition()
  {
    id = nameof (IsNotMarkedForDisable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DISABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      BuildingEnabledButton buildingEnabledButton = data as BuildingEnabledButton;
      if ((Object) buildingEnabledButton == (Object) null)
        return true;
      if (buildingEnabledButton.IsEnabled)
        return !buildingEnabledButton.WaitingForDisable;
      return false;
    })
  };
  public Chore.Precondition IsFunctional = new Chore.Precondition()
  {
    id = nameof (IsFunctional),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_FUNCTIONAL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as Operational).IsFunctional)
  };
  public Chore.Precondition IsOverrideTargetNullOrMe = new Chore.Precondition()
  {
    id = nameof (IsOverrideTargetNullOrMe),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_OVERRIDE_TARGET_NULL_OR_ME,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (!context.isAttemptingOverride && !((Object) context.chore.overrideTarget == (Object) null))
        return (Object) context.chore.overrideTarget == (Object) context.consumerState.consumer;
      return true;
    })
  };
  public Chore.Precondition NotChoreCreator = new Chore.Precondition()
  {
    id = nameof (NotChoreCreator),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NOT_CHORE_CREATOR,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      GameObject gameObject = (GameObject) data;
      return !((Object) context.consumerState.consumer == (Object) null) && !((Object) context.consumerState.gameObject == (Object) gameObject);
    })
  };
  public Chore.Precondition IsGettingMoreStressed = new Chore.Precondition()
  {
    id = nameof (IsGettingMoreStressed),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_GETTING_MORE_STRESSED,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (double) Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).GetDelta() > 0.0)
  };
  public Chore.Precondition IsAllowedByAutomation = new Chore.Precondition()
  {
    id = nameof (IsAllowedByAutomation),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_ALLOWED_BY_AUTOMATION,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((Automatable) data).AllowedByAutomation(context.consumerState.hasSolidTransferArm))
  };
  public Chore.Precondition HasTag = new Chore.Precondition()
  {
    id = nameof (HasTag),
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Tag tag = (Tag) data;
      return context.consumerState.prefabid.HasTag(tag);
    })
  };
  public Chore.Precondition CheckBehaviourPrecondition = new Chore.Precondition()
  {
    id = nameof (CheckBehaviourPrecondition),
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Tag tag = (Tag) data;
      return context.consumerState.consumer.RunBehaviourPrecondition(tag);
    })
  };
  public Chore.Precondition CanDoWorkerPrioritizable = new Chore.Precondition()
  {
    id = nameof (CanDoWorkerPrioritizable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_DO_RECREATION,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((Object) context.consumerState.consumer == (Object) null)
        return false;
      IWorkerPrioritizable workerPrioritizable = data as IWorkerPrioritizable;
      if (workerPrioritizable == null)
        return false;
      int priority = 0;
      if (!workerPrioritizable.GetWorkerPriority(context.consumerState.worker, out priority))
        return false;
      context.consumerPriority += priority;
      return true;
    })
  };
  public Chore.Precondition IsExclusivelyAvailableWithOtherChores = new Chore.Precondition()
  {
    id = nameof (IsExclusivelyAvailableWithOtherChores),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.EXCLUSIVELY_AVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      foreach (Chore chore in (List<Chore>) data)
      {
        if (chore != context.chore && (Object) chore.driver != (Object) null)
          return false;
      }
      return true;
    })
  };
  public Chore.Precondition IsBladderFull = new Chore.Precondition()
  {
    id = nameof (IsBladderFull),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_FULL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      BladderMonitor.Instance smi = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
      return smi != null && smi.NeedsToPee();
    })
  };
  public Chore.Precondition IsBladderNotFull = new Chore.Precondition()
  {
    id = nameof (IsBladderNotFull),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_NOT_FULL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      BladderMonitor.Instance smi = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
      return smi == null || !smi.NeedsToPee();
    })
  };
  public Chore.Precondition NoDeadBodies = new Chore.Precondition()
  {
    id = nameof (NoDeadBodies),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NO_DEAD_BODIES,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => Components.LiveMinionIdentities.Count == Components.MinionIdentities.Count)
  };
  public Chore.Precondition NotCurrentlyPeeing = new Chore.Precondition()
  {
    id = nameof (NotCurrentlyPeeing),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CURRENTLY_PEEING,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      bool flag = true;
      Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
      if (currentChore != null)
      {
        string id = currentChore.choreType.Id;
        flag = id != Db.Get().ChoreTypes.BreakPee.Id && id != Db.Get().ChoreTypes.Pee.Id;
      }
      return flag;
    })
  };
  private static ChorePreconditions _instance;

  public static ChorePreconditions instance
  {
    get
    {
      if (ChorePreconditions._instance == null)
        ChorePreconditions._instance = new ChorePreconditions();
      return ChorePreconditions._instance;
    }
  }

  public static void DestroyInstance()
  {
    ChorePreconditions._instance = (ChorePreconditions) null;
  }
}
