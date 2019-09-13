// Decompiled with JetBrains decompiler
// Type: Generator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class Generator : KMonoBehaviour, ISaveLoadable, IEnergyProducer
{
  public static readonly Operational.Flag generatorConnectedFlag = new Operational.Flag("GeneratorConnected", Operational.Flag.Type.Requirement);
  protected static readonly Operational.Flag wireConnectedFlag = new Operational.Flag("generatorWireConnected", Operational.Flag.Type.Requirement);
  private static readonly EventSystem.IntraObjectHandler<Generator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Generator>((System.Action<Generator, object>) ((component, data) => component.OnOperationalChanged(data)));
  protected const int SimUpdateSortKey = 1001;
  [MyCmpReq]
  protected Building building;
  [MyCmpReq]
  protected Operational operational;
  [MyCmpReq]
  protected KSelectable selectable;
  [Serialize]
  private float joulesAvailable;
  [SerializeField]
  public int powerDistributionOrder;
  private float capacity;
  private StatusItem currentStatusItem;
  private Guid statusItemID;
  private AttributeInstance generatorOutputAttribute;

  public int PowerDistributionOrder
  {
    get
    {
      return this.powerDistributionOrder;
    }
  }

  public virtual float Capacity
  {
    get
    {
      return this.capacity;
    }
  }

  public virtual float BaseCapacity
  {
    get
    {
      return this.capacity;
    }
  }

  public virtual bool IsEmpty
  {
    get
    {
      return (double) this.joulesAvailable <= 0.0;
    }
  }

  public virtual float JoulesAvailable
  {
    get
    {
      return this.joulesAvailable;
    }
  }

  public float WattageRating
  {
    get
    {
      return this.building.Def.GeneratorWattageRating * this.Efficiency;
    }
  }

  public float BaseWattageRating
  {
    get
    {
      return this.building.Def.GeneratorWattageRating;
    }
  }

  public float PercentFull
  {
    get
    {
      if ((double) this.Capacity == 0.0)
        return 1f;
      return this.joulesAvailable / this.Capacity;
    }
  }

  public bool HasWire
  {
    get
    {
      bool flag = false;
      GameObject gameObject = Grid.Objects[this.PowerCell, 26];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null)
        flag = true;
      return flag;
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

  private float Efficiency
  {
    get
    {
      return Mathf.Max((float) (1.0 + (double) this.generatorOutputAttribute.GetTotalValue() / 100.0), 0.1f);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.generatorOutputAttribute = this.gameObject.GetAttributes().Add(Db.Get().Attributes.GeneratorOutput);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Generators.Add(this);
    this.Subscribe<Generator>(-592767678, Generator.OnOperationalChangedDelegate);
    this.capacity = Generator.CalculateCapacity(this.building.Def, (Element) null);
    this.PowerCell = this.building.GetPowerOutputCell();
    this.CheckConnectionStatus();
    this.OnOperationalChanged((object) null);
    Game.Instance.energySim.AddGenerator(this);
  }

  public virtual void EnergySim200ms(float dt)
  {
    this.CheckConnectionStatus();
  }

  private void SetStatusItem(StatusItem status_item)
  {
    if (status_item != this.currentStatusItem && this.currentStatusItem != null)
      this.statusItemID = this.selectable.RemoveStatusItem(this.statusItemID, false);
    if (status_item != null && this.statusItemID == Guid.Empty)
      this.statusItemID = this.selectable.AddStatusItem(status_item, (object) this);
    this.currentStatusItem = status_item;
  }

  private void CheckConnectionStatus()
  {
    if (this.CircuitID == ushort.MaxValue)
    {
      if (this.HasWire)
      {
        this.SetStatusItem(Db.Get().BuildingStatusItems.NoPowerConsumers);
        this.operational.SetFlag(Generator.generatorConnectedFlag, true);
      }
      else
      {
        this.SetStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
        this.operational.SetFlag(Generator.generatorConnectedFlag, false);
      }
    }
    else
    {
      this.SetStatusItem((StatusItem) null);
      this.operational.SetFlag(Generator.generatorConnectedFlag, true);
    }
  }

  protected override void OnCleanUp()
  {
    Game.Instance.energySim.RemoveGenerator(this);
    Game.Instance.circuitManager.Disconnect(this);
    Components.Generators.Remove(this);
    base.OnCleanUp();
  }

  public static float CalculateCapacity(BuildingDef def, Element element)
  {
    if (element == null)
      return def.GeneratorBaseCapacity;
    return def.GeneratorBaseCapacity * (float) (1.0 + (!element.HasTag(GameTags.RefinedMetal) ? 0.0 : 1.0));
  }

  public void ResetJoules()
  {
    this.joulesAvailable = 0.0f;
  }

  public virtual void ApplyDeltaJoules(float joulesDelta, bool canOverPower = false)
  {
    this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesDelta, 0.0f, !canOverPower ? this.Capacity : float.MaxValue);
  }

  public void GenerateJoules(float joulesAvailable, bool canOverPower = false)
  {
    Debug.Assert((UnityEngine.Object) this.GetComponent<Battery>() == (UnityEngine.Object) null);
    this.joulesAvailable = Mathf.Clamp(joulesAvailable, 0.0f, !canOverPower ? this.Capacity : float.MaxValue);
    ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, this.joulesAvailable, this.GetProperName(), (string) null);
    if (!Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(this.PrefabID()))
      Game.Instance.savedInfo.powerCreatedbyGeneratorType.Add(this.PrefabID(), 0.0f);
    Dictionary<Tag, float> createdbyGeneratorType;
    Tag index;
    (createdbyGeneratorType = Game.Instance.savedInfo.powerCreatedbyGeneratorType)[index = this.PrefabID()] = createdbyGeneratorType[index] + this.joulesAvailable;
  }

  public void AssignJoulesAvailable(float joulesAvailable)
  {
    Debug.Assert((UnityEngine.Object) this.GetComponent<PowerTransformer>() != (UnityEngine.Object) null);
    this.joulesAvailable = joulesAvailable;
  }

  private void OnOperationalChanged(object data)
  {
    if (this.operational.IsOperational)
      Game.Instance.circuitManager.Connect(this);
    else
      Game.Instance.circuitManager.Disconnect(this);
  }

  public virtual void ConsumeEnergy(float joules)
  {
    this.joulesAvailable = Mathf.Max(0.0f, this.JoulesAvailable - joules);
  }
}
