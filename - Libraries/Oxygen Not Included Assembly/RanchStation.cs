// Decompiled with JetBrains decompiler
// Type: RanchStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class RanchStation : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>
{
  public GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State unoperational;
  public RanchStation.OperationalState operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.operational;
    this.unoperational.TagTransition(GameTags.Operational, (GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State) this.operational, false);
    this.operational.TagTransition(GameTags.Operational, this.unoperational, true).ToggleChore((Func<RanchStation.Instance, Chore>) (smi => smi.CreateChore()), this.unoperational, this.unoperational).Update("FindRanachable", (System.Action<RanchStation.Instance, float>) ((smi, dt) => smi.FindRanchable()), UpdateRate.SIM_1000ms, false);
  }

  public class Def : StateMachine.BaseDef
  {
    public HashedString ranchedPreAnim = (HashedString) "idle_loop";
    public HashedString ranchedLoopAnim = (HashedString) "idle_loop";
    public HashedString ranchedPstAnim = (HashedString) "idle_loop";
    public HashedString rancherInteractAnim = (HashedString) "anim_interacts_rancherstation_kanim";
    public int interactLoopCount = 1;
    public Func<RanchStation.Instance, int> getTargetRanchCell = (Func<RanchStation.Instance, int>) (smi => Grid.PosToCell((StateMachine.Instance) smi));
    public Func<GameObject, RanchStation.Instance, bool> isCreatureEligibleToBeRanchedCb;
    public System.Action<GameObject> onRanchCompleteCb;
    public bool synchronizeBuilding;
  }

  public class OperationalState : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State
  {
  }

  public class Instance : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, RanchStation.Def def)
      : base(master, def)
    {
    }

    public RanchableMonitor.Instance targetRanchable { get; private set; }

    public bool shouldCreatureGoGetRanched { get; private set; }

    public Chore CreateChore()
    {
      return (Chore) new RancherChore(this.GetComponent<KPrefabID>());
    }

    public int GetTargetRanchCell()
    {
      return this.def.getTargetRanchCell(this);
    }

    public bool IsCreatureAvailableForRanching()
    {
      if (this.targetRanchable == null)
        return false;
      int targetRanchCell = this.GetTargetRanchCell();
      return RanchStation.Instance.CanRanchableBeRanchedAtRanchStation(this.targetRanchable, this, Game.Instance.roomProber.GetCavityForCell(targetRanchCell), targetRanchCell);
    }

    public void SetRancherIsAvailableForRanching()
    {
      this.shouldCreatureGoGetRanched = true;
    }

    public void ClearRancherIsAvailableForRanching()
    {
      this.shouldCreatureGoGetRanched = false;
    }

    private static bool CanRanchableBeRanchedAtRanchStation(
      RanchableMonitor.Instance ranchable,
      RanchStation.Instance ranch_station,
      CavityInfo ranch_cavity_info,
      int ranch_cell)
    {
      if (!ranchable.IsRunning() || ranchable.targetRanchStation != ranch_station && ranchable.targetRanchStation != null && ranchable.targetRanchStation.IsRunning() || (!ranch_station.def.isCreatureEligibleToBeRanchedCb(ranchable.gameObject, ranch_station) || !ranchable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>()))
        return false;
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(ranchable.transform.GetPosition()));
      return cavityForCell != null && cavityForCell == ranch_cavity_info && ranchable.GetComponent<Navigator>().GetNavigationCost(ranch_cell) != -1;
    }

    public void FindRanchable()
    {
      int targetRanchCell = this.GetTargetRanchCell();
      CavityInfo cavityForCell1 = Game.Instance.roomProber.GetCavityForCell(targetRanchCell);
      if (cavityForCell1 == null || cavityForCell1.room == null || cavityForCell1.room.roomType != Db.Get().RoomTypes.CreaturePen)
      {
        this.TriggerRanchStationNoLongerAvailable();
      }
      else
      {
        if (this.targetRanchable != null && !RanchStation.Instance.CanRanchableBeRanchedAtRanchStation(this.targetRanchable, this, cavityForCell1, targetRanchCell))
          this.TriggerRanchStationNoLongerAvailable();
        if (!this.targetRanchable.IsNullOrStopped())
          return;
        CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(targetRanchCell);
        RanchableMonitor.Instance instance = (RanchableMonitor.Instance) null;
        if (cavityForCell2 != null && cavityForCell2.creatures != null)
        {
          foreach (KPrefabID creature in cavityForCell2.creatures)
          {
            if (!((UnityEngine.Object) creature == (UnityEngine.Object) null))
            {
              RanchableMonitor.Instance smi = creature.GetSMI<RanchableMonitor.Instance>();
              if (!smi.IsNullOrStopped() && RanchStation.Instance.CanRanchableBeRanchedAtRanchStation(smi, this, cavityForCell2, targetRanchCell))
              {
                instance = smi;
                break;
              }
            }
          }
        }
        this.targetRanchable = instance;
        if (this.targetRanchable.IsNullOrStopped())
          return;
        this.targetRanchable.targetRanchStation = this;
      }
    }

    public void TriggerRanchStationNoLongerAvailable()
    {
      if (this.targetRanchable.IsNullOrStopped())
        return;
      this.targetRanchable.targetRanchStation = (RanchStation.Instance) null;
      this.targetRanchable.Trigger(1689625967, (object) null);
      this.targetRanchable = (RanchableMonitor.Instance) null;
    }

    public void RanchCreature()
    {
      if (this.targetRanchable.IsNullOrStopped())
        return;
      Debug.Assert(this.targetRanchable != null, (object) "targetRanchable was null");
      Debug.Assert(this.targetRanchable.GetMaster() != null, (object) "GetMaster was null");
      Debug.Assert(this.def != null, (object) "def was null");
      Debug.Assert(this.def.onRanchCompleteCb != null, (object) "onRanchCompleteCb cb was null");
      this.def.onRanchCompleteCb(this.targetRanchable.gameObject);
      this.targetRanchable.Trigger(1827504087, (object) null);
    }
  }
}
