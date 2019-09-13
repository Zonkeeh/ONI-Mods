// Decompiled with JetBrains decompiler
// Type: Battery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class Battery : KMonoBehaviour, IEnergyConsumer, IEffectDescriptor, IEnergyProducer
{
  private static readonly EventSystem.IntraObjectHandler<Battery> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Battery>((System.Action<Battery, object>) ((component, data) => component.OnOperationalChanged(data)));
  [SerializeField]
  public float chargeWattage = float.PositiveInfinity;
  [SerializeField]
  public float capacity;
  [Serialize]
  private float joulesAvailable;
  [MyCmpGet]
  protected Operational operational;
  [MyCmpGet]
  public PowerTransformer powerTransformer;
  private MeterController meter;
  public float joulesLostPerSecond;
  [SerializeField]
  public int powerSortOrder;
  private float PreviousJoulesAvailable;
  private CircuitManager.ConnectionStatus connectionStatus;
  private float dt;
  private float joulesConsumed;

  public float WattsUsed { get; private set; }

  public float WattsNeededWhenActive
  {
    get
    {
      return 0.0f;
    }
  }

  public float PercentFull
  {
    get
    {
      return this.joulesAvailable / this.capacity;
    }
  }

  public float PreviousPercentFull
  {
    get
    {
      return this.PreviousJoulesAvailable / this.capacity;
    }
  }

  public float JoulesAvailable
  {
    get
    {
      return this.joulesAvailable;
    }
  }

  public float Capacity
  {
    get
    {
      return this.capacity;
    }
  }

  public float ChargeCapacity { get; private set; }

  public int PowerSortOrder
  {
    get
    {
      return this.powerSortOrder;
    }
  }

  public string Name
  {
    get
    {
      return this.GetComponent<KSelectable>().GetName();
    }
  }

  public int PowerCell { get; private set; }

  public ushort CircuitID
  {
    get
    {
      return Game.Instance.circuitManager.GetCircuitID(this.PowerCell);
    }
  }

  public bool IsConnected
  {
    get
    {
      return (UnityEngine.Object) Grid.Objects[this.PowerCell, 26] != (UnityEngine.Object) null;
    }
  }

  public bool IsPowered
  {
    get
    {
      return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Batteries.Add(this);
    this.PowerCell = this.GetComponent<Building>().GetPowerInputCell();
    this.Subscribe<Battery>(-592767678, Battery.OnOperationalChangedDelegate);
    this.OnOperationalChanged((object) null);
    MeterController meterController;
    if ((bool) ((UnityEngine.Object) this.GetComponent<PowerTransformer>()))
      meterController = (MeterController) null;
    else
      meterController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
      {
        "meter_target",
        "meter_fill",
        "meter_frame",
        "meter_OL"
      });
    this.meter = meterController;
    Game.Instance.circuitManager.Connect((IEnergyConsumer) this);
    Game.Instance.energySim.AddBattery(this);
  }

  private void OnOperationalChanged(object data)
  {
    if (this.operational.IsOperational)
    {
      Game.Instance.circuitManager.Connect((IEnergyConsumer) this);
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.JoulesAvailable, (object) this);
    }
    else
    {
      Game.Instance.circuitManager.Disconnect((IEnergyConsumer) this);
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.JoulesAvailable, false);
    }
  }

  protected override void OnCleanUp()
  {
    Game.Instance.energySim.RemoveBattery(this);
    Game.Instance.circuitManager.Disconnect((IEnergyConsumer) this);
    Components.Batteries.Remove(this);
    base.OnCleanUp();
  }

  public virtual void EnergySim200ms(float dt)
  {
    this.dt = dt;
    this.joulesConsumed = 0.0f;
    this.WattsUsed = 0.0f;
    this.ChargeCapacity = this.chargeWattage * dt;
    if (this.meter != null)
      this.meter.SetPositionPercent(this.PercentFull);
    this.UpdateSounds();
    this.PreviousJoulesAvailable = this.JoulesAvailable;
    this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);
  }

  private void UpdateSounds()
  {
    float previousPercentFull = this.PreviousPercentFull;
    float percentFull = this.PercentFull;
    if ((double) percentFull == 0.0 && (double) previousPercentFull != 0.0)
      this.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryDischarged);
    if ((double) percentFull > 0.999000012874603 && (double) previousPercentFull <= 0.999000012874603)
      this.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryFull);
    if ((double) percentFull >= 0.25 || (double) previousPercentFull < 0.25)
      return;
    this.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryWarning);
  }

  public void SetConnectionStatus(CircuitManager.ConnectionStatus status)
  {
    this.connectionStatus = status;
    if (status == CircuitManager.ConnectionStatus.NotConnected)
      this.operational.SetActive(false, false);
    else
      this.operational.SetActive(this.operational.IsOperational && (double) this.JoulesAvailable > 0.0, false);
  }

  public void AddEnergy(float joules)
  {
    this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
    this.joulesConsumed += joules;
    this.ChargeCapacity -= joules;
    this.WattsUsed = this.joulesConsumed / this.dt;
  }

  public void ConsumeEnergy(float joules, bool report = false)
  {
    if (report)
      ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -Mathf.Min(this.JoulesAvailable, joules), StringFormatter.Replace((string) BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()), (string) null);
    this.joulesAvailable = Mathf.Max(0.0f, this.JoulesAvailable - joules);
  }

  public void ConsumeEnergy(float joules)
  {
    this.ConsumeEnergy(joules, false);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if ((UnityEngine.Object) this.powerTransformer == (UnityEngine.Object) null)
    {
      descriptorList.Add(new Descriptor((string) UI.BUILDINGEFFECTS.REQUIRESPOWERGENERATOR, (string) UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.BATTERYCAPACITY, (object) GameUtil.GetFormattedJoules(this.capacity, string.Empty, GameUtil.TimeSlice.None)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYCAPACITY, (object) GameUtil.GetFormattedJoules(this.capacity, string.Empty, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.BATTERYLEAK, (object) GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYLEAK, (object) GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Effect, false));
    }
    else
    {
      descriptorList.Add(new Descriptor("Input " + UI.FormatAsLink("Power Wire", "WIRE"), (string) UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
      descriptorList.Add(new Descriptor(string.Format("Output " + UI.FormatAsLink("Power Wire", "WIRE") + " (Limited to {0})", (object) GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic)), (string) UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
    }
    return descriptorList;
  }

  [ContextMenu("Refill Power")]
  public void DEBUG_RefillPower()
  {
    this.joulesAvailable = this.capacity;
  }
}
