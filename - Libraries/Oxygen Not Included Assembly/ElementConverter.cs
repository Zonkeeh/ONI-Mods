// Decompiled with JetBrains decompiler
// Type: ElementConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverter : StateMachineComponent<ElementConverter.StatesInstance>, IEffectDescriptor
{
  private float totalDiseaseWeight = float.MaxValue;
  private float workSpeedMultiplier = 1f;
  public bool showDescriptors = true;
  private float outputMultiplier = 1f;
  [MyCmpGet]
  private Operational operational;
  [MyCmpReq]
  private Storage storage;
  public System.Action<float> onConvertMass;
  private AttributeInstance machinerySpeedAttribute;
  private const float BASE_INTERVAL = 1f;
  [SerializeField]
  public ElementConverter.ConsumedElement[] consumedElements;
  [SerializeField]
  public ElementConverter.OutputElement[] outputElements;
  private static StatusItem ElementConverterInput;
  private static StatusItem ElementConverterOutput;

  public void SetWorkSpeedMultiplier(float speed)
  {
    this.workSpeedMultiplier = speed;
  }

  public void SetStorage(Storage storage)
  {
    this.storage = storage;
  }

  public float OutputMultiplier
  {
    get
    {
      return this.outputMultiplier;
    }
    set
    {
      this.outputMultiplier = value;
    }
  }

  public float AverageConvertRate
  {
    get
    {
      return Game.Instance.accumulators.GetAverageRate(this.outputElements[0].accumulator);
    }
  }

  public bool HasEnoughMass(Tag tag)
  {
    bool flag = false;
    List<GameObject> items = this.storage.items;
    foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
    {
      if (tag == consumedElement.tag)
      {
        float num = 0.0f;
        for (int index = 0; index < items.Count; ++index)
        {
          GameObject go = items[index];
          if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
            num += go.GetComponent<PrimaryElement>().Mass;
        }
        flag = (double) num >= (double) consumedElement.massConsumptionRate;
        break;
      }
    }
    return flag;
  }

  public bool HasEnoughMassToStartConverting()
  {
    return this.HasEnoughMass();
  }

  public bool CanConvertAtAll()
  {
    bool flag1 = true;
    List<GameObject> items = this.storage.items;
    for (int index1 = 0; index1 < this.consumedElements.Length; ++index1)
    {
      ElementConverter.ConsumedElement consumedElement = this.consumedElements[index1];
      bool flag2 = false;
      for (int index2 = 0; index2 < items.Count; ++index2)
      {
        GameObject go = items[index2];
        if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(consumedElement.tag) && (double) go.GetComponent<PrimaryElement>().Mass > 0.0)
        {
          flag2 = true;
          break;
        }
      }
      if (!flag2)
      {
        flag1 = false;
        break;
      }
    }
    return flag1;
  }

  private float GetSpeedMultiplier()
  {
    return this.machinerySpeedAttribute.GetTotalValue() * this.workSpeedMultiplier;
  }

  private bool HasEnoughMass()
  {
    float num1 = 1f * this.GetSpeedMultiplier();
    bool flag = true;
    List<GameObject> items = this.storage.items;
    for (int index1 = 0; index1 < this.consumedElements.Length; ++index1)
    {
      ElementConverter.ConsumedElement consumedElement = this.consumedElements[index1];
      float num2 = 0.0f;
      for (int index2 = 0; index2 < items.Count; ++index2)
      {
        GameObject go = items[index2];
        if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(consumedElement.tag))
          num2 += go.GetComponent<PrimaryElement>().Mass;
      }
      if ((double) num2 < (double) consumedElement.massConsumptionRate * (double) num1)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void ConvertMass()
  {
    float num1 = 1f * this.GetSpeedMultiplier();
    float a1 = 1f;
    for (int index1 = 0; index1 < this.consumedElements.Length; ++index1)
    {
      ElementConverter.ConsumedElement consumedElement = this.consumedElements[index1];
      float a2 = consumedElement.massConsumptionRate * num1 * a1;
      if ((double) a2 <= 0.0)
      {
        a1 = 0.0f;
        break;
      }
      float b = 0.0f;
      for (int index2 = 0; index2 < this.storage.items.Count; ++index2)
      {
        GameObject go = this.storage.items[index2];
        if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(consumedElement.tag))
        {
          PrimaryElement component = go.GetComponent<PrimaryElement>();
          float num2 = Mathf.Min(a2, component.Mass);
          b += num2 / a2;
        }
      }
      a1 = Mathf.Min(a1, b);
    }
    if ((double) a1 <= 0.0)
      return;
    SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
    diseaseInfo.idx = byte.MaxValue;
    diseaseInfo.count = 0;
    float num3 = 0.0f;
    float num4 = 0.0f;
    float num5 = 0.0f;
    for (int index1 = 0; index1 < this.consumedElements.Length; ++index1)
    {
      ElementConverter.ConsumedElement consumedElement = this.consumedElements[index1];
      float num2 = consumedElement.massConsumptionRate * num1 * a1;
      Game.Instance.accumulators.Accumulate(consumedElement.accumulator, num2);
      for (int index2 = 0; index2 < this.storage.items.Count; ++index2)
      {
        GameObject go = this.storage.items[index2];
        if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
        {
          if (go.HasTag(consumedElement.tag))
          {
            PrimaryElement component = go.GetComponent<PrimaryElement>();
            component.KeepZeroMassObject = true;
            float num6 = Mathf.Min(num2, component.Mass);
            int src2_count = (int) ((double) (num6 / component.Mass) * (double) component.DiseaseCount);
            float num7 = num6 * component.Element.specificHeatCapacity;
            num5 += num7;
            num4 += num7 * component.Temperature;
            component.Mass -= num6;
            component.ModifyDiseaseCount(-src2_count, "ElementConverter.ConvertMass");
            num3 += num6;
            diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo.idx, diseaseInfo.count, component.DiseaseIdx, src2_count);
            num2 -= num6;
            if ((double) num2 <= 0.0)
              break;
          }
          if ((double) num2 <= 0.0)
            Debug.Assert((double) num2 <= 0.0);
        }
      }
    }
    float b1 = (double) num5 <= 0.0 ? 0.0f : num4 / num5;
    if (this.onConvertMass != null && (double) num3 > 0.0)
      this.onConvertMass(num3);
    if (this.outputElements != null && this.outputElements.Length > 0)
    {
      for (int index = 0; index < this.outputElements.Length; ++index)
      {
        ElementConverter.OutputElement outputElement = this.outputElements[index];
        SimUtil.DiseaseInfo a2 = diseaseInfo;
        if ((double) this.totalDiseaseWeight <= 0.0)
        {
          a2.idx = byte.MaxValue;
          a2.count = 0;
        }
        else
        {
          float num2 = outputElement.diseaseWeight / this.totalDiseaseWeight;
          a2.count = (int) ((double) a2.count * (double) num2);
        }
        if (outputElement.addedDiseaseIdx != byte.MaxValue)
          a2 = SimUtil.CalculateFinalDiseaseInfo(a2, new SimUtil.DiseaseInfo()
          {
            idx = outputElement.addedDiseaseIdx,
            count = outputElement.addedDiseaseCount
          });
        float num6 = outputElement.massGenerationRate * this.OutputMultiplier * num1 * a1;
        Game.Instance.accumulators.Accumulate(outputElement.accumulator, num6);
        float temperature = outputElement.useEntityTemperature || (double) b1 == 0.0 && (double) outputElement.minOutputTemperature == 0.0 ? this.GetComponent<PrimaryElement>().Temperature : Mathf.Max(outputElement.minOutputTemperature, b1);
        Element elementByHash = ElementLoader.FindElementByHash(outputElement.elementHash);
        if (outputElement.storeOutput)
        {
          PrimaryElement primaryElement = this.storage.AddToPrimaryElement(outputElement.elementHash, num6, temperature);
          if ((UnityEngine.Object) primaryElement == (UnityEngine.Object) null)
          {
            if (elementByHash.IsGas)
              this.storage.AddGasChunk(outputElement.elementHash, num6, temperature, a2.idx, a2.count, true, true);
            else if (elementByHash.IsLiquid)
              this.storage.AddLiquid(outputElement.elementHash, num6, temperature, a2.idx, a2.count, true, true);
            else
              this.storage.Store(elementByHash.substance.SpawnResource(this.transform.GetPosition(), num6, temperature, a2.idx, a2.count, true, false, false), true, false, true, false);
          }
          else
            primaryElement.AddDisease(a2.idx, a2.count, "ElementConverter.ConvertMass");
        }
        else
        {
          Vector3 vector3 = new Vector3(this.transform.GetPosition().x + outputElement.outputElementOffset.x, this.transform.GetPosition().y + outputElement.outputElementOffset.y, 0.0f);
          int cell = Grid.PosToCell(vector3);
          if (elementByHash.IsLiquid)
          {
            int idx = (int) elementByHash.idx;
            FallingWater.instance.AddParticle(cell, (byte) idx, num6, temperature, a2.idx, a2.count, true, false, false, false);
          }
          else if (elementByHash.IsSolid)
            elementByHash.substance.SpawnResource(vector3, num6, temperature, a2.idx, a2.count, false, false, false);
          else
            SimMessages.AddRemoveSubstance(cell, outputElement.elementHash, CellEventLogger.Instance.OxygenModifierSimUpdate, num6, temperature, a2.idx, a2.count, true, -1);
        }
        if (outputElement.elementHash == SimHashes.Oxygen)
          ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num6, this.gameObject.GetProperName(), (string) null);
      }
    }
    this.storage.Trigger(-1697596308, (object) this.gameObject);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.machinerySpeedAttribute = this.gameObject.GetAttributes().Add(Db.Get().Attributes.MachinerySpeed);
    if (ElementConverter.ElementConverterInput == null)
      ElementConverter.ElementConverterInput = new StatusItem("ElementConverterInput", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022).SetResolveStringCallback((Func<string, object, string>) ((str, data) =>
      {
        ElementConverter.ConsumedElement consumedElement = (ElementConverter.ConsumedElement) data;
        str = str.Replace("{ElementTypes}", consumedElement.Name);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedByTag(consumedElement.tag, consumedElement.Rate, GameUtil.TimeSlice.PerSecond));
        return str;
      }));
    if (ElementConverter.ElementConverterOutput != null)
      return;
    ElementConverter.ElementConverterOutput = new StatusItem("ElementConverterOutput", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022).SetResolveStringCallback((Func<string, object, string>) ((str, data) =>
    {
      ElementConverter.OutputElement outputElement = (ElementConverter.OutputElement) data;
      str = str.Replace("{ElementTypes}", outputElement.Name);
      str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(outputElement.Rate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
      return str;
    }));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.consumedElements.Length; ++index)
      this.consumedElements[index].accumulator = Game.Instance.accumulators.Add("ElementsConsumed", (KMonoBehaviour) this);
    this.totalDiseaseWeight = 0.0f;
    for (int index = 0; index < this.outputElements.Length; ++index)
    {
      this.outputElements[index].accumulator = Game.Instance.accumulators.Add("OutputElements", (KMonoBehaviour) this);
      this.totalDiseaseWeight += this.outputElements[index].diseaseWeight;
    }
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    for (int index = 0; index < this.consumedElements.Length; ++index)
      Game.Instance.accumulators.Remove(this.consumedElements[index].accumulator);
    for (int index = 0; index < this.outputElements.Length; ++index)
      Game.Instance.accumulators.Remove(this.outputElements[index].accumulator);
    base.OnCleanUp();
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (!this.showDescriptors)
      return descriptorList;
    if (this.consumedElements != null)
    {
      foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
      {
        Descriptor descriptor = new Descriptor();
        descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMED, (object) consumedElement.Name, (object) GameUtil.GetFormattedMass(consumedElement.massConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, (object) consumedElement.Name, (object) GameUtil.GetFormattedMass(consumedElement.massConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
        descriptorList.Add(descriptor);
      }
    }
    if (this.outputElements != null)
    {
      foreach (ElementConverter.OutputElement outputElement in this.outputElements)
      {
        Descriptor descriptor = new Descriptor();
        LocString locString1;
        LocString locString2;
        if (outputElement.useEntityTemperature)
        {
          locString1 = UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP;
          locString2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP;
        }
        else if ((double) outputElement.minOutputTemperature > 0.0)
        {
          locString1 = UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINTEMP;
          locString2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINTEMP;
        }
        else
        {
          locString1 = UI.BUILDINGEFFECTS.ELEMENTEMITTED_INPUTTEMP;
          locString2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_INPUTTEMP;
        }
        descriptor.SetupDescriptor(string.Format((string) locString1, (object) outputElement.Name, (object) GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) locString2, (object) outputElement.Name, (object) GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
        descriptorList.Add(descriptor);
      }
    }
    return descriptorList;
  }

  [DebuggerDisplay("{tag} {massConsumptionRate}")]
  [Serializable]
  public struct ConsumedElement
  {
    public Tag tag;
    public float massConsumptionRate;
    public HandleVector<int>.Handle accumulator;

    public ConsumedElement(Tag tag, float kgPerSecond)
    {
      this.tag = tag;
      this.massConsumptionRate = kgPerSecond;
      this.accumulator = HandleVector<int>.InvalidHandle;
    }

    public string Name
    {
      get
      {
        return this.tag.ProperName();
      }
    }

    public float Rate
    {
      get
      {
        return Game.Instance.accumulators.GetAverageRate(this.accumulator);
      }
    }
  }

  [Serializable]
  public struct OutputElement
  {
    public SimHashes elementHash;
    public float minOutputTemperature;
    public bool useEntityTemperature;
    public float massGenerationRate;
    public bool storeOutput;
    public Vector2 outputElementOffset;
    public HandleVector<int>.Handle accumulator;
    public float diseaseWeight;
    public byte addedDiseaseIdx;
    public int addedDiseaseCount;

    public OutputElement(
      float kgPerSecond,
      SimHashes element,
      float minOutputTemperature,
      bool useEntityTemperature = false,
      bool storeOutput = false,
      float outputElementOffsetx = 0.0f,
      float outputElementOffsety = 0.5f,
      float diseaseWeight = 1f,
      byte addedDiseaseIdx = 255,
      int addedDiseaseCount = 0)
    {
      this.elementHash = element;
      this.minOutputTemperature = minOutputTemperature;
      this.useEntityTemperature = useEntityTemperature;
      this.storeOutput = storeOutput;
      this.massGenerationRate = kgPerSecond;
      this.outputElementOffset = new Vector2(outputElementOffsetx, outputElementOffsety);
      this.accumulator = HandleVector<int>.InvalidHandle;
      this.diseaseWeight = diseaseWeight;
      this.addedDiseaseIdx = addedDiseaseIdx;
      this.addedDiseaseCount = addedDiseaseCount;
    }

    public string Name
    {
      get
      {
        return ElementLoader.FindElementByHash(this.elementHash).tag.ProperName();
      }
    }

    public float Rate
    {
      get
      {
        return Game.Instance.accumulators.GetAverageRate(this.accumulator);
      }
    }
  }

  public class StatesInstance : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.GameInstance
  {
    private List<Guid> statusItemEntries = new List<Guid>();

    public StatesInstance(ElementConverter smi)
      : base(smi)
    {
    }

    public void AddStatusItems()
    {
      foreach (ElementConverter.ConsumedElement consumedElement in this.master.consumedElements)
        this.statusItemEntries.Add(this.master.GetComponent<KSelectable>().AddStatusItem(ElementConverter.ElementConverterInput, (object) consumedElement));
      foreach (ElementConverter.OutputElement outputElement in this.master.outputElements)
        this.statusItemEntries.Add(this.master.GetComponent<KSelectable>().AddStatusItem(ElementConverter.ElementConverterOutput, (object) outputElement));
    }

    public void RemoveStatusItems()
    {
      foreach (Guid statusItemEntry in this.statusItemEntries)
        this.master.GetComponent<KSelectable>().RemoveStatusItem(statusItemEntry, false);
      this.statusItemEntries.Clear();
    }
  }

  public class States : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter>
  {
    public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State disabled;
    public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State converting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.disabled.EventTransition(GameHashes.ActiveChanged, this.converting, (StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Transition.ConditionCallback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.operational == (UnityEngine.Object) null))
          return smi.master.operational.IsActive;
        return true;
      }));
      this.converting.Enter("AddStatusItems", (StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback) (smi => smi.AddStatusItems())).Exit("RemoveStatusItems", (StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback) (smi => smi.RemoveStatusItems())).EventTransition(GameHashes.ActiveChanged, this.disabled, (StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Transition.ConditionCallback) (smi =>
      {
        if ((UnityEngine.Object) smi.master.operational != (UnityEngine.Object) null)
          return !smi.master.operational.IsActive;
        return false;
      })).Update("ConvertMass", (System.Action<ElementConverter.StatesInstance, float>) ((smi, dt) => smi.master.ConvertMass()), UpdateRate.SIM_1000ms, true);
    }
  }
}
