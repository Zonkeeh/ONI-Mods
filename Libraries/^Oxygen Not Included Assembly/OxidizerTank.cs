// Decompiled with JetBrains decompiler
// Type: OxidizerTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
  private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnReturnRocketDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>((System.Action<OxidizerTank, object>) ((component, data) => component.OnReturn(data)));
  private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>((System.Action<OxidizerTank, object>) ((component, data) => component.OnStorageChange(data)));
  [Serialize]
  public float targetFillMass = 2700f;
  [SerializeField]
  private Tag[] oxidizerTypes = new Tag[2]
  {
    SimHashes.OxyRock.CreateTag(),
    SimHashes.LiquidOxygen.CreateTag()
  };
  public Storage storage;
  private MeterController meter;
  private bool isSuspended;

  public bool IsSuspended
  {
    get
    {
      return this.isSuspended;
    }
  }

  public float UserMaxCapacity
  {
    get
    {
      return this.targetFillMass;
    }
    set
    {
      this.targetFillMass = value;
      this.storage.capacityKg = this.targetFillMass;
      ConduitConsumer component1 = this.GetComponent<ConduitConsumer>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.capacityKG = this.targetFillMass;
      ManualDeliveryKG component2 = this.GetComponent<ManualDeliveryKG>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.capacity = component2.refillMass = this.targetFillMass;
      this.Trigger(-945020481, (object) this);
    }
  }

  public float MinCapacity
  {
    get
    {
      return 0.0f;
    }
  }

  public float MaxCapacity
  {
    get
    {
      return 2700f;
    }
  }

  public float AmountStored
  {
    get
    {
      return this.storage.MassStored();
    }
  }

  public bool WholeValues
  {
    get
    {
      return false;
    }
  }

  public LocString CapacityUnits
  {
    get
    {
      return GameUtil.GetCurrentMassUnit(false);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "grounded", KAnim.PlayMode.Loop, 1f, 0.0f);
    this.UserMaxCapacity = this.UserMaxCapacity;
    this.Subscribe<OxidizerTank>(1366341636, OxidizerTank.OnReturnRocketDelegate);
    this.Subscribe<OxidizerTank>(-1697596308, OxidizerTank.OnStorageChangeDelegate);
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
  }

  public float MassStored()
  {
    return this.storage.MassStored();
  }

  public float GetTotalOxidizerAvailable()
  {
    float num = 0.0f;
    foreach (Tag oxidizerType in this.oxidizerTypes)
      num += this.storage.GetAmountAvailable(oxidizerType);
    return num;
  }

  public Dictionary<Tag, float> GetOxidizersAvailable()
  {
    Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
    foreach (Tag oxidizerType in this.oxidizerTypes)
      dictionary[oxidizerType] = this.storage.GetAmountAvailable(oxidizerType);
    return dictionary;
  }

  [ContextMenu("Fill Tank")]
  public void FillTank(SimHashes element)
  {
    if (ElementLoader.FindElementByHash(element).IsLiquid)
    {
      this.storage.AddLiquid(element, this.targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, (byte) 0, 0, false, true);
    }
    else
    {
      if (!ElementLoader.FindElementByHash(element).IsSolid)
        return;
      this.storage.Store(ElementLoader.FindElementByHash(element).substance.SpawnResource(this.gameObject.transform.GetPosition(), this.targetFillMass, 300f, byte.MaxValue, 0, false, false, false), false, false, true, false);
    }
  }

  private void OnStorageChange(object data)
  {
    this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
  }

  private void OnReturn(object data)
  {
    this.storage.ConsumeAllIgnoringDisease();
  }
}
