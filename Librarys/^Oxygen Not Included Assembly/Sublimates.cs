// Decompiled with JetBrains decompiler
// Type: Sublimates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Sublimates : KMonoBehaviour, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<Sublimates> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Sublimates>((System.Action<Sublimates, object>) ((component, data) => component.OnAbsorb(data)));
  private static readonly EventSystem.IntraObjectHandler<Sublimates> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<Sublimates>((System.Action<Sublimates, object>) ((component, data) => component.OnSplitFromChunk(data)));
  private HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private KSelectable selectable;
  [SerializeField]
  public SpawnFXHashes spawnFXHash;
  [SerializeField]
  public Sublimates.Info info;
  [Serialize]
  private float sublimatedMass;

  public float Temperature
  {
    get
    {
      return this.primaryElement.Temperature;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Sublimates>(-2064133523, Sublimates.OnAbsorbDelegate);
    this.Subscribe<Sublimates>(1335436905, Sublimates.OnSplitFromChunkDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.flowAccumulator = Game.Instance.accumulators.Add("EmittedMass", (KMonoBehaviour) this);
    if (this.info.sublimatedElement == SimHashes.Oxygen)
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingOxygenAvg, (object) this);
    else
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingGasAvg, (object) this);
  }

  protected override void OnCleanUp()
  {
    this.flowAccumulator = Game.Instance.accumulators.Remove(this.flowAccumulator);
    base.OnCleanUp();
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
      return;
    Sublimates component = pickupable.GetComponent<Sublimates>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.sublimatedMass += component.sublimatedMass;
  }

  private void OnSplitFromChunk(object data)
  {
    Pickupable pickupable = data as Pickupable;
    PrimaryElement component1 = pickupable.GetComponent<PrimaryElement>();
    Sublimates component2 = pickupable.GetComponent<Sublimates>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    float mass1 = this.primaryElement.Mass;
    float mass2 = component1.Mass;
    float num1 = mass1 / (mass2 + mass1);
    this.sublimatedMass = component2.sublimatedMass * num1;
    float num2 = 1f - num1;
    component2.sublimatedMass *= num2;
  }

  public void Sim200ms(float dt)
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (!Grid.IsValidCell(cell) || this.HasTag(GameTags.Sealed))
      return;
    float num1 = Grid.Mass[cell];
    if ((double) num1 >= (double) this.info.maxDestinationMass)
      return;
    float mass1 = this.primaryElement.Mass;
    if ((double) mass1 > 0.0)
    {
      float num2 = Mathf.Min(Mathf.Max(this.info.sublimationRate, this.info.sublimationRate * Mathf.Pow(mass1, this.info.massPower)) * dt, mass1);
      this.sublimatedMass += num2;
      float num3 = mass1 - num2;
      if ((double) this.sublimatedMass <= (double) this.info.minSublimationAmount)
        return;
      float num4 = this.sublimatedMass / this.primaryElement.Mass;
      byte diseaseIdx;
      int disease_count;
      if (this.info.diseaseIdx == byte.MaxValue)
      {
        diseaseIdx = this.primaryElement.DiseaseIdx;
        disease_count = (int) ((double) this.primaryElement.DiseaseCount * (double) num4);
        this.primaryElement.ModifyDiseaseCount(-disease_count, "Sublimates.SimUpdate");
      }
      else
      {
        float num5 = this.sublimatedMass / this.info.sublimationRate;
        diseaseIdx = this.info.diseaseIdx;
        disease_count = (int) ((double) this.info.diseaseCount * (double) num5);
      }
      float mass2 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num1);
      if ((double) mass2 <= 0.0)
        return;
      this.Emit(cell, mass2, this.primaryElement.Temperature, diseaseIdx, disease_count);
      this.sublimatedMass = Mathf.Max(0.0f, this.sublimatedMass - mass2);
      this.primaryElement.Mass = Mathf.Max(0.0f, this.primaryElement.Mass - mass2);
      this.UpdateStorage();
    }
    else if ((double) this.sublimatedMass > 0.0)
    {
      float mass2 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num1);
      if ((double) mass2 <= 0.0)
        return;
      this.Emit(cell, mass2, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount);
      this.sublimatedMass = Mathf.Max(0.0f, this.sublimatedMass - mass2);
      this.primaryElement.Mass = Mathf.Max(0.0f, this.primaryElement.Mass - mass2);
      this.UpdateStorage();
    }
    else
    {
      if (this.primaryElement.KeepZeroMassObject)
        return;
      Util.KDestroyGameObject(this.gameObject);
    }
  }

  private void UpdateStorage()
  {
    Pickupable component = this.GetComponent<Pickupable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.storage != (UnityEngine.Object) null))
      return;
    component.storage.Trigger(-1697596308, (object) this.gameObject);
  }

  private void Emit(int cell, float mass, float temperature, byte disease_idx, int disease_count)
  {
    SimMessages.AddRemoveSubstance(cell, this.info.sublimatedElement, CellEventLogger.Instance.SublimatesEmit, mass, temperature, disease_idx, disease_count, true, -1);
    Game.Instance.accumulators.Accumulate(this.flowAccumulator, mass);
    if (this.spawnFXHash == SpawnFXHashes.None)
      return;
    this.transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    Game.Instance.SpawnFX(this.spawnFXHash, this.transform.GetPosition(), 0.0f);
  }

  public float AvgFlowRate()
  {
    return Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
  }

  [Serializable]
  public struct Info
  {
    public float sublimationRate;
    public float minSublimationAmount;
    public float maxDestinationMass;
    public float massPower;
    public byte diseaseIdx;
    public int diseaseCount;
    [HashedEnum]
    public SimHashes sublimatedElement;

    public Info(
      float rate,
      float min_amount,
      float max_destination_mass,
      float mass_power,
      SimHashes element,
      byte disease_idx = 255,
      int disease_count = 0)
    {
      this.sublimationRate = rate;
      this.minSublimationAmount = min_amount;
      this.maxDestinationMass = max_destination_mass;
      this.massPower = mass_power;
      this.sublimatedElement = element;
      this.diseaseIdx = disease_idx;
      this.diseaseCount = disease_count;
    }
  }
}
