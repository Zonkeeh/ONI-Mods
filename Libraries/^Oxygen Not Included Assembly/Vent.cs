// Decompiled with JetBrains decompiler
// Type: Vent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Vent : KMonoBehaviour, IEffectDescriptor
{
  private int cell = -1;
  [Serialize]
  public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();
  [SerializeField]
  public ConduitType conduitType = ConduitType.Gas;
  [SerializeField]
  public float overpressureMass = 1f;
  [NonSerialized]
  public bool showConnectivityIcons = true;
  private int sortKey;
  private Vent.StatesInstance smi;
  [SerializeField]
  public Endpoint endpointType;
  [MyCmpGet]
  [NonSerialized]
  public Structure structure;

  public int SortKey
  {
    get
    {
      return this.sortKey;
    }
    set
    {
      this.sortKey = value;
    }
  }

  public void UpdateVentedMass(SimHashes element, float mass)
  {
    if (!this.lifeTimeVentMass.ContainsKey(element))
    {
      this.lifeTimeVentMass.Add(element, mass);
    }
    else
    {
      Dictionary<SimHashes, float> lifeTimeVentMass;
      SimHashes index;
      (lifeTimeVentMass = this.lifeTimeVentMass)[index = element] = lifeTimeVentMass[index] + mass;
    }
  }

  public float GetVentedMass(SimHashes element)
  {
    if (this.lifeTimeVentMass.ContainsKey(element))
      return this.lifeTimeVentMass[element];
    return 0.0f;
  }

  protected override void OnSpawn()
  {
    this.cell = this.GetComponent<Building>().GetUtilityOutputCell();
    this.smi = new Vent.StatesInstance(this);
    this.smi.StartSM();
  }

  public Vent.State GetEndPointState()
  {
    Vent.State state = Vent.State.Invalid;
    switch (this.endpointType)
    {
      case Endpoint.Source:
        state = !this.IsConnected() ? Vent.State.Blocked : Vent.State.Ready;
        break;
      case Endpoint.Sink:
        state = Vent.State.Ready;
        int cell = this.cell;
        if (!this.IsValidOutputCell(cell))
        {
          state = !Grid.Solid[cell] ? Vent.State.OverPressure : Vent.State.Blocked;
          break;
        }
        break;
    }
    return state;
  }

  public bool IsConnected()
  {
    UtilityNetwork networkForCell = Conduit.GetNetworkManager(this.conduitType).GetNetworkForCell(this.cell);
    if (networkForCell != null)
      return (networkForCell as FlowUtilityNetwork).HasSinks;
    return false;
  }

  public bool IsBlocked
  {
    get
    {
      return this.GetEndPointState() != Vent.State.Ready;
    }
  }

  private bool IsValidOutputCell(int output_cell)
  {
    bool flag = false;
    if (((UnityEngine.Object) this.structure == (UnityEngine.Object) null || !this.structure.IsEntombed()) && !Grid.Solid[output_cell])
      flag = (double) Grid.Mass[output_cell] < (double) this.overpressureMass;
    return flag;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    string formattedMass = GameUtil.GetFormattedMass(this.overpressureMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, (object) formattedMass), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, (object) formattedMass), Descriptor.DescriptorType.Effect, false)
    };
  }

  public enum State
  {
    Invalid,
    Ready,
    Blocked,
    OverPressure,
  }

  public class StatesInstance : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.GameInstance
  {
    private Exhaust exhaust;

    public StatesInstance(Vent master)
      : base(master)
    {
      this.exhaust = master.GetComponent<Exhaust>();
    }

    public bool NeedsExhaust()
    {
      if ((UnityEngine.Object) this.exhaust != (UnityEngine.Object) null && this.master.GetEndPointState() != Vent.State.Ready)
        return this.master.endpointType == Endpoint.Source;
      return false;
    }

    public bool Blocked()
    {
      if (this.master.GetEndPointState() == Vent.State.Blocked)
        return this.master.endpointType != Endpoint.Source;
      return false;
    }

    public bool OverPressure()
    {
      if ((UnityEngine.Object) this.exhaust != (UnityEngine.Object) null && this.master.GetEndPointState() == Vent.State.OverPressure)
        return this.master.endpointType != Endpoint.Source;
      return false;
    }

    public void CheckTransitions()
    {
      if (this.NeedsExhaust())
        this.smi.GoTo((StateMachine.BaseState) this.sm.needExhaust);
      else if (this.Blocked())
        this.smi.GoTo((StateMachine.BaseState) this.sm.blocked);
      else if (this.OverPressure())
        this.smi.GoTo((StateMachine.BaseState) this.sm.overPressure);
      else
        this.smi.GoTo((StateMachine.BaseState) this.sm.idle);
    }

    public StatusItem SelectStatusItem(
      StatusItem gas_status_item,
      StatusItem liquid_status_item)
    {
      if (this.master.conduitType == ConduitType.Gas)
        return gas_status_item;
      return liquid_status_item;
    }
  }

  public class States : GameStateMachine<Vent.States, Vent.StatesInstance, Vent>
  {
    public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State idle;
    public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State blocked;
    public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State overPressure;
    public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State needExhaust;
    public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State venting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.Update("CheckTransitions", (System.Action<Vent.StatesInstance, float>) ((smi, dt) => smi.CheckTransitions()), UpdateRate.SIM_200ms, false);
      this.blocked.ToggleStatusItem((Func<Vent.StatesInstance, StatusItem>) (smi => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed, Db.Get().BuildingStatusItems.LiquidVentObstructed)), (Func<Vent.StatesInstance, object>) null);
      this.overPressure.ToggleStatusItem((Func<Vent.StatesInstance, StatusItem>) (smi => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, Db.Get().BuildingStatusItems.LiquidVentOverPressure)), (Func<Vent.StatesInstance, object>) null);
    }
  }
}
