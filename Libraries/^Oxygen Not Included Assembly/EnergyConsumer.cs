// Decompiled with JetBrains decompiler
// Type: EnergyConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumer : KMonoBehaviour, ISaveLoadable, IEnergyConsumer, IEffectDescriptor
{
  public static readonly Operational.Flag PoweredFlag = new Operational.Flag("powered", Operational.Flag.Type.Requirement);
  private Dictionary<string, float> lastTimeSoundPlayed = new Dictionary<string, float>();
  private float soundDecayTime = 10f;
  [MyCmpReq]
  private Building building;
  [MyCmpGet]
  protected Operational operational;
  [MyCmpGet]
  private KSelectable selectable;
  [SerializeField]
  public int powerSortOrder;
  [Serialize]
  protected float circuitOverloadTime;
  private float _BaseWattageRating;

  public int PowerSortOrder
  {
    get
    {
      return this.powerSortOrder;
    }
  }

  public int PowerCell { get; private set; }

  public bool HasWire
  {
    get
    {
      return (Object) Grid.Objects[this.PowerCell, 26] != (Object) null;
    }
  }

  public virtual bool IsPowered
  {
    get
    {
      return this.operational.GetFlag(EnergyConsumer.PoweredFlag);
    }
    private set
    {
      this.operational.SetFlag(EnergyConsumer.PoweredFlag, value);
    }
  }

  public bool IsConnected
  {
    get
    {
      return this.CircuitID != ushort.MaxValue;
    }
  }

  public string Name
  {
    get
    {
      return this.selectable.GetName();
    }
  }

  public ushort CircuitID { get; private set; }

  public float BaseWattageRating
  {
    get
    {
      return this._BaseWattageRating;
    }
    set
    {
      this._BaseWattageRating = value;
    }
  }

  public float WattsUsed
  {
    get
    {
      if (this.operational.IsActive)
        return this.BaseWattageRating;
      return 0.0f;
    }
  }

  public float WattsNeededWhenActive
  {
    get
    {
      return this.building.Def.EnergyConsumptionWhenActive;
    }
  }

  public float BaseWattsNeededWhenActive
  {
    get
    {
      return this.building.Def.EnergyConsumptionWhenActive;
    }
  }

  protected override void OnPrefabInit()
  {
    this.CircuitID = ushort.MaxValue;
    this.IsPowered = false;
    this.BaseWattageRating = this.building.Def.EnergyConsumptionWhenActive;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.EnergyConsumers.Add(this);
    this.PowerCell = this.GetComponent<Building>().GetPowerInputCell();
    Game.Instance.circuitManager.Connect((IEnergyConsumer) this);
    Game.Instance.energySim.AddEnergyConsumer(this);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.energySim.RemoveEnergyConsumer(this);
    Game.Instance.circuitManager.Disconnect((IEnergyConsumer) this);
    Components.EnergyConsumers.Remove(this);
    base.OnCleanUp();
  }

  public virtual void EnergySim200ms(float dt)
  {
    this.CircuitID = Game.Instance.circuitManager.GetCircuitID(this.PowerCell);
    if (!this.IsConnected)
      this.IsPowered = false;
    this.circuitOverloadTime = Mathf.Max(0.0f, this.circuitOverloadTime - dt);
  }

  public virtual void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
  {
    switch (connection_status)
    {
      case CircuitManager.ConnectionStatus.NotConnected:
        this.IsPowered = false;
        break;
      case CircuitManager.ConnectionStatus.Unpowered:
        if (!this.IsPowered || !((Object) this.GetComponent<Battery>() == (Object) null))
          break;
        this.IsPowered = false;
        this.circuitOverloadTime = 6f;
        this.PlayCircuitSound("overdraw");
        break;
      case CircuitManager.ConnectionStatus.Powered:
        if (this.IsPowered || (double) this.circuitOverloadTime > 0.0)
          break;
        this.IsPowered = true;
        this.PlayCircuitSound("powered");
        break;
    }
  }

  protected void PlayCircuitSound(string state)
  {
    string sound = (string) null;
    if (state == "powered")
      sound = Sounds.Instance.BuildingPowerOnMigrated;
    else if (state == "overdraw")
      sound = Sounds.Instance.ElectricGridOverloadMigrated;
    else
      Debug.Log((object) "Invalid state for sound in EnergyConsumer.");
    if (!CameraController.Instance.IsAudibleSound((Vector2) this.transform.GetPosition()))
      return;
    float num1;
    if (!this.lastTimeSoundPlayed.TryGetValue(state, out num1))
      num1 = 0.0f;
    float num2 = (Time.time - num1) / this.soundDecayTime;
    FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition((Vector2) this.transform.GetPosition()));
    int num3 = (int) instance.setParameterValue("timeSinceLast", num2);
    KFMOD.EndOneShot(instance);
    this.lastTimeSoundPlayed[state] = Time.time;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return (List<Descriptor>) null;
  }
}
