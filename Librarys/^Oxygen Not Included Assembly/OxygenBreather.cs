// Decompiled with JetBrains decompiler
// Type: OxygenBreather
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using UnityEngine;

[RequireComponent(typeof (Health))]
public class OxygenBreather : KMonoBehaviour, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnDeathDelegate = new EventSystem.IntraObjectHandler<OxygenBreather>((System.Action<OxygenBreather, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnRevivedDelegate = new EventSystem.IntraObjectHandler<OxygenBreather>((System.Action<OxygenBreather, object>) ((component, data) => component.OnRevived(data)));
  public float O2toCO2conversion = 0.5f;
  [SerializeField]
  public float minCO2ToEmit = 0.3f;
  private bool hasAir = true;
  private Timer hasAirTimer = new Timer();
  private HandleVector<int>.Handle o2Accumulator = HandleVector<int>.InvalidHandle;
  private HandleVector<int>.Handle co2Accumulator = HandleVector<int>.InvalidHandle;
  public float lowOxygenThreshold;
  public float noOxygenThreshold;
  public Vector2 mouthOffset;
  [Serialize]
  public float accumulatedCO2;
  [MyCmpAdd]
  private Notifier notifier;
  [MyCmpGet]
  private Facing facing;
  private AmountInstance temperature;
  private AttributeInstance airConsumptionRate;
  public CellOffset[] breathableCells;
  public System.Action<Sim.MassConsumedCallback> onSimConsume;
  private OxygenBreather.IGasProvider gasProvider;

  public float CO2EmitRate
  {
    get
    {
      return Game.Instance.accumulators.GetAverageRate(this.co2Accumulator);
    }
  }

  public HandleVector<int>.Handle O2Accumulator
  {
    get
    {
      return this.o2Accumulator;
    }
  }

  protected override void OnPrefabInit()
  {
    this.Subscribe<OxygenBreather>(1623392196, OxygenBreather.OnDeathDelegate);
    this.Subscribe<OxygenBreather>(-1117766961, OxygenBreather.OnRevivedDelegate);
  }

  public bool IsLowOxygen()
  {
    return (double) this.GetOxygenPressure(this.mouthCell) < (double) this.lowOxygenThreshold;
  }

  protected override void OnSpawn()
  {
    this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup((Component) this);
    this.o2Accumulator = Game.Instance.accumulators.Add("O2", (KMonoBehaviour) this);
    this.co2Accumulator = Game.Instance.accumulators.Add("CO2", (KMonoBehaviour) this);
    KSelectable component = this.GetComponent<KSelectable>();
    component.AddStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, (object) this);
    component.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, (object) this);
    this.temperature = Db.Get().Amounts.Temperature.Lookup((Component) this);
    NameDisplayScreen.Instance.RegisterComponent(this.gameObject, (object) this, false);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.o2Accumulator);
    Game.Instance.accumulators.Remove(this.co2Accumulator);
    this.SetGasProvider((OxygenBreather.IGasProvider) null);
    base.OnCleanUp();
  }

  public void Consume(Sim.MassConsumedCallback mass_consumed)
  {
    if (this.onSimConsume == null)
      return;
    this.onSimConsume(mass_consumed);
  }

  public void Sim200ms(float dt)
  {
    if (this.gameObject.HasTag(GameTags.Dead))
      return;
    float amount1 = this.airConsumptionRate.GetTotalValue() * dt;
    bool flag = this.gasProvider.ConsumeGas(this, amount1);
    if (flag)
    {
      if (this.gasProvider.ShouldEmitCO2())
      {
        float amount2 = amount1 * this.O2toCO2conversion;
        Game.Instance.accumulators.Accumulate(this.co2Accumulator, amount2);
        this.accumulatedCO2 += amount2;
        if ((double) this.accumulatedCO2 >= (double) this.minCO2ToEmit)
        {
          this.accumulatedCO2 -= this.minCO2ToEmit;
          Vector3 position = this.transform.GetPosition();
          position.x += !this.facing.GetFacing() ? this.mouthOffset.x : -this.mouthOffset.x;
          position.y += this.mouthOffset.y;
          position.z -= 0.5f;
          CO2Manager.instance.SpawnBreath(position, this.minCO2ToEmit, this.temperature.value);
        }
      }
      else if (this.gasProvider.ShouldStoreCO2())
      {
        Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
        if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
        {
          float amount2 = amount1 * this.O2toCO2conversion;
          Game.Instance.accumulators.Accumulate(this.co2Accumulator, amount2);
          this.accumulatedCO2 += amount2;
          if ((double) this.accumulatedCO2 >= (double) this.minCO2ToEmit)
          {
            this.accumulatedCO2 -= this.minCO2ToEmit;
            equippable.GetComponent<Storage>().AddGasChunk(SimHashes.CarbonDioxide, this.minCO2ToEmit, this.temperature.value, byte.MaxValue, 0, false, true);
          }
        }
      }
    }
    if (flag != this.hasAir)
    {
      this.hasAirTimer.Start();
      if (!this.hasAirTimer.TryStop(2f))
        return;
      this.hasAir = flag;
    }
    else
      this.hasAirTimer.Stop();
  }

  private void OnDeath(object data)
  {
    this.enabled = false;
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, false);
    component.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, false);
  }

  private void OnRevived(object data)
  {
    this.enabled = true;
  }

  private int GetMouthCellAtCell(int cell, CellOffset[] offsets)
  {
    float num1 = 0.0f;
    int num2 = cell;
    foreach (CellOffset offset in offsets)
    {
      int cell1 = Grid.OffsetCell(cell, offset);
      float oxygenPressure = this.GetOxygenPressure(cell1);
      if ((double) oxygenPressure > (double) num1 && (double) oxygenPressure > (double) this.noOxygenThreshold)
      {
        num1 = oxygenPressure;
        num2 = cell1;
      }
    }
    return num2;
  }

  public int mouthCell
  {
    get
    {
      return this.GetMouthCellAtCell(Grid.PosToCell((KMonoBehaviour) this), this.breathableCells);
    }
  }

  public bool IsBreathableElementAtCell(int cell, CellOffset[] offsets = null)
  {
    return this.GetBreathableElementAtCell(cell, offsets) != SimHashes.Vacuum;
  }

  public SimHashes GetBreathableElementAtCell(int cell, CellOffset[] offsets = null)
  {
    if (offsets == null)
      offsets = this.breathableCells;
    int mouthCellAtCell = this.GetMouthCellAtCell(cell, offsets);
    if (!Grid.IsValidCell(mouthCellAtCell))
      return SimHashes.Vacuum;
    Element element = Grid.Element[mouthCellAtCell];
    if (element.IsGas && element.HasTag(GameTags.Breathable) && (double) Grid.Mass[mouthCellAtCell] > (double) this.noOxygenThreshold)
      return element.id;
    return SimHashes.Vacuum;
  }

  public bool IsUnderLiquid
  {
    get
    {
      return Grid.Element[this.mouthCell].IsLiquid;
    }
  }

  public bool IsSuffocating
  {
    get
    {
      return !this.hasAir;
    }
  }

  public SimHashes GetBreathableElement
  {
    get
    {
      return this.GetBreathableElementAtCell(Grid.PosToCell((KMonoBehaviour) this), (CellOffset[]) null);
    }
  }

  public bool IsBreathableElement
  {
    get
    {
      return this.IsBreathableElementAtCell(Grid.PosToCell((KMonoBehaviour) this), (CellOffset[]) null);
    }
  }

  private float GetOxygenPressure(int cell)
  {
    if (Grid.IsValidCell(cell) && Grid.Element[cell].HasTag(GameTags.Breathable))
      return Grid.Mass[cell];
    return 0.0f;
  }

  public OxygenBreather.IGasProvider GetGasProvider()
  {
    return this.gasProvider;
  }

  public void SetGasProvider(OxygenBreather.IGasProvider gas_provider)
  {
    if (this.gasProvider != null)
      this.gasProvider.OnClearOxygenBreather(this);
    this.gasProvider = gas_provider;
    if (this.gasProvider == null)
      return;
    this.gasProvider.OnSetOxygenBreather(this);
  }

  public interface IGasProvider
  {
    void OnSetOxygenBreather(OxygenBreather oxygen_breather);

    void OnClearOxygenBreather(OxygenBreather oxygen_breather);

    bool ConsumeGas(OxygenBreather oxygen_breather, float amount);

    bool ShouldEmitCO2();

    bool ShouldStoreCO2();
  }
}
