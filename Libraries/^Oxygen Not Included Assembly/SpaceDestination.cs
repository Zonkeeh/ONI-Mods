// Decompiled with JetBrains decompiler
// Type: SpaceDestination
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceDestination
{
  private static List<Tuple<float, int>> RARE_ELEMENT_CHANCES = new List<Tuple<float, int>>()
  {
    new Tuple<float, int>(1f, 0),
    new Tuple<float, int>(0.33f, 1),
    new Tuple<float, int>(0.03f, 2)
  };
  private static readonly List<Tuple<SimHashes, MathUtil.MinMax>> RARE_ELEMENTS = new List<Tuple<SimHashes, MathUtil.MinMax>>()
  {
    new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Katairite, new MathUtil.MinMax(1f, 10f)),
    new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Niobium, new MathUtil.MinMax(1f, 10f)),
    new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Fullerene, new MathUtil.MinMax(1f, 10f)),
    new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Isoresin, new MathUtil.MinMax(1f, 10f))
  };
  private static readonly List<Tuple<string, MathUtil.MinMax>> RARE_ITEMS = new List<Tuple<string, MathUtil.MinMax>>()
  {
    new Tuple<string, MathUtil.MinMax>("GeneShufflerRecharge", new MathUtil.MinMax(1f, 2f))
  };
  [Serialize]
  public float activePeriod = 20f;
  [Serialize]
  public float inactivePeriod = 10f;
  [Serialize]
  public Dictionary<SimHashes, float> recoverableElements = new Dictionary<SimHashes, float>();
  [Serialize]
  public List<SpaceDestination.ResearchOpportunity> researchOpportunities = new List<SpaceDestination.ResearchOpportunity>();
  public List<SpaceMission> missions = new List<SpaceMission>();
  private const int MASS_TO_RECOVER_AMOUNT = 1000;
  private const float RARE_ITEM_CHANCE = 0.33f;
  [Serialize]
  public int id;
  [Serialize]
  public string type;
  public bool startAnalyzed;
  [Serialize]
  public int distance;
  [Serialize]
  public float startingOrbitPercentage;
  [Serialize]
  private float availableMass;

  public SpaceDestination(int id, string type, int distance)
  {
    this.id = id;
    this.type = type;
    this.distance = distance;
    SpaceDestinationType destinationType = this.GetDestinationType();
    this.availableMass = (float) (destinationType.maxiumMass - destinationType.minimumMass);
    this.GenerateSurfaceElements();
    this.GenerateMissions();
    this.GenerateResearchOpportunities();
  }

  private static Tuple<SimHashes, MathUtil.MinMax> GetRareElement(SimHashes id)
  {
    foreach (Tuple<SimHashes, MathUtil.MinMax> tuple in SpaceDestination.RARE_ELEMENTS)
    {
      if (tuple.first == id)
        return tuple;
    }
    return (Tuple<SimHashes, MathUtil.MinMax>) null;
  }

  public int OneBasedDistance
  {
    get
    {
      return this.distance + 1;
    }
  }

  public float CurrentMass
  {
    get
    {
      return (float) this.GetDestinationType().minimumMass + this.availableMass;
    }
  }

  public float AvailableMass
  {
    get
    {
      return this.availableMass;
    }
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 9))
      return;
    SpaceDestinationType destinationType = this.GetDestinationType();
    this.availableMass = (float) (destinationType.maxiumMass - destinationType.minimumMass);
  }

  public SpaceDestinationType GetDestinationType()
  {
    return Db.Get().SpaceDestinationTypes.Get(this.type);
  }

  public float GetCurrentOrbitPercentage()
  {
    float num = 0.1f * Mathf.Pow((float) this.OneBasedDistance, 2f);
    return ((float) GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage() + this.startingOrbitPercentage * num) % num / num;
  }

  public SpaceDestination.ResearchOpportunity TryCompleteResearchOpportunity()
  {
    foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.researchOpportunities)
    {
      if (researchOpportunity.TryComplete(this))
        return researchOpportunity;
    }
    return (SpaceDestination.ResearchOpportunity) null;
  }

  public void GenerateSurfaceElements()
  {
    foreach (KeyValuePair<SimHashes, MathUtil.MinMax> keyValuePair in this.GetDestinationType().elementTable)
      this.recoverableElements.Add(keyValuePair.Key, Random.value);
  }

  public SpacecraftManager.DestinationAnalysisState AnalysisState()
  {
    return SpacecraftManager.instance.GetDestinationAnalysisState(this);
  }

  public void GenerateResearchOpportunities()
  {
    this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity((string) UI.STARMAP.DESTINATIONSTUDY.UPPERATMO, TUNING.ROCKETRY.DESTINATION_RESEARCH.BASIC));
    this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity((string) UI.STARMAP.DESTINATIONSTUDY.LOWERATMO, TUNING.ROCKETRY.DESTINATION_RESEARCH.BASIC));
    this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity((string) UI.STARMAP.DESTINATIONSTUDY.MAGNETICFIELD, TUNING.ROCKETRY.DESTINATION_RESEARCH.BASIC));
    this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity((string) UI.STARMAP.DESTINATIONSTUDY.SURFACE, TUNING.ROCKETRY.DESTINATION_RESEARCH.BASIC));
    this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity((string) UI.STARMAP.DESTINATIONSTUDY.SUBSURFACE, TUNING.ROCKETRY.DESTINATION_RESEARCH.BASIC));
    float num1 = 0.0f;
    foreach (Tuple<float, int> tuple in SpaceDestination.RARE_ELEMENT_CHANCES)
      num1 += tuple.first;
    float num2 = Random.value * num1;
    int num3 = 0;
    foreach (Tuple<float, int> tuple in SpaceDestination.RARE_ELEMENT_CHANCES)
    {
      num2 -= tuple.first;
      if ((double) num2 <= 0.0)
        num3 = tuple.second;
    }
    for (int index = 0; index < num3; ++index)
      this.researchOpportunities[Random.Range(0, this.researchOpportunities.Count)].discoveredRareResource = SpaceDestination.RARE_ELEMENTS[Random.Range(0, SpaceDestination.RARE_ELEMENTS.Count)].first;
    if ((double) Random.value >= 0.330000013113022)
      return;
    this.researchOpportunities[Random.Range(0, this.researchOpportunities.Count)].discoveredRareItem = SpaceDestination.RARE_ITEMS[Random.Range(0, SpaceDestination.RARE_ITEMS.Count)].first;
  }

  public void GenerateMissions()
  {
    bool flag = true;
    foreach (SpaceMission mission in this.missions)
    {
      if (mission.craft == null)
        flag = false;
    }
    if (!flag)
      return;
    this.missions.Add(new SpaceMission(this));
  }

  public float GetResourceValue(SimHashes resource, float roll)
  {
    if (this.GetDestinationType().elementTable.ContainsKey(resource))
      return this.GetDestinationType().elementTable[resource].Lerp(roll);
    if (SpaceDestinationTypes.extendedElementTable.ContainsKey(resource))
      return SpaceDestinationTypes.extendedElementTable[resource].Lerp(roll);
    return 0.0f;
  }

  public Dictionary<SimHashes, float> GetMissionResourceResult(
    float totalCargoSpace,
    bool solids = true,
    bool liquids = true,
    bool gasses = true)
  {
    Dictionary<SimHashes, float> dictionary = new Dictionary<SimHashes, float>();
    float num1 = 0.0f;
    foreach (KeyValuePair<SimHashes, float> recoverableElement in this.recoverableElements)
    {
      if (ElementLoader.FindElementByHash(recoverableElement.Key).IsSolid && solids || ElementLoader.FindElementByHash(recoverableElement.Key).IsLiquid && liquids || ElementLoader.FindElementByHash(recoverableElement.Key).IsGas && gasses)
        num1 += this.GetResourceValue(recoverableElement.Key, recoverableElement.Value);
    }
    float num2 = Mathf.Min(this.CurrentMass - (float) this.GetDestinationType().minimumMass, totalCargoSpace);
    foreach (KeyValuePair<SimHashes, float> recoverableElement in this.recoverableElements)
    {
      if (ElementLoader.FindElementByHash(recoverableElement.Key).IsSolid && solids || ElementLoader.FindElementByHash(recoverableElement.Key).IsLiquid && liquids || ElementLoader.FindElementByHash(recoverableElement.Key).IsGas && gasses)
      {
        float num3 = num2 * (this.GetResourceValue(recoverableElement.Key, recoverableElement.Value) / num1);
        dictionary.Add(recoverableElement.Key, num3);
      }
    }
    return dictionary;
  }

  public Dictionary<Tag, int> GetRecoverableEntities()
  {
    Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
    Dictionary<string, int> recoverableEntities = this.GetDestinationType().recoverableEntities;
    if (recoverableEntities != null)
    {
      foreach (KeyValuePair<string, int> keyValuePair in recoverableEntities)
        dictionary.Add((Tag) keyValuePair.Key, keyValuePair.Value);
    }
    return dictionary;
  }

  public Dictionary<Tag, int> GetMissionEntityResult()
  {
    return this.GetRecoverableEntities();
  }

  public void UpdateRemainingResources(CargoBay bay)
  {
    if (!((Object) bay != (Object) null))
      return;
    foreach (KeyValuePair<SimHashes, float> recoverableElement in this.recoverableElements)
    {
      if (this.HasElementType(bay.storageType))
      {
        this.availableMass = Mathf.Max(0.0f, this.availableMass - bay.GetComponent<Storage>().capacityKg);
        break;
      }
    }
  }

  public bool HasElementType(CargoBay.CargoType type)
  {
    foreach (KeyValuePair<SimHashes, float> recoverableElement in this.recoverableElements)
    {
      if (ElementLoader.FindElementByHash(recoverableElement.Key).IsSolid && type == CargoBay.CargoType.solids || ElementLoader.FindElementByHash(recoverableElement.Key).IsLiquid && type == CargoBay.CargoType.liquids || ElementLoader.FindElementByHash(recoverableElement.Key).IsGas && type == CargoBay.CargoType.gasses)
        return true;
    }
    return false;
  }

  public void Replenish(float dt)
  {
    SpaceDestinationType destinationType = this.GetDestinationType();
    if ((double) this.CurrentMass >= (double) destinationType.maxiumMass)
      return;
    this.availableMass += destinationType.replishmentPerSim1000ms;
  }

  public float GetAvailableResourcesPercentage(CargoBay.CargoType cargoType)
  {
    float num = 0.0f;
    float totalMass = this.GetTotalMass();
    foreach (KeyValuePair<SimHashes, float> recoverableElement in this.recoverableElements)
    {
      if (ElementLoader.FindElementByHash(recoverableElement.Key).IsSolid && cargoType == CargoBay.CargoType.solids || ElementLoader.FindElementByHash(recoverableElement.Key).IsLiquid && cargoType == CargoBay.CargoType.liquids || ElementLoader.FindElementByHash(recoverableElement.Key).IsGas && cargoType == CargoBay.CargoType.gasses)
        num += this.GetResourceValue(recoverableElement.Key, recoverableElement.Value) / totalMass;
    }
    return num;
  }

  public float GetTotalMass()
  {
    float num = 0.0f;
    foreach (KeyValuePair<SimHashes, float> recoverableElement in this.recoverableElements)
      num += this.GetResourceValue(recoverableElement.Key, recoverableElement.Value);
    return num;
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class ResearchOpportunity
  {
    [Serialize]
    public SimHashes discoveredRareResource = SimHashes.Void;
    [Serialize]
    public string description;
    [Serialize]
    public int dataValue;
    [Serialize]
    public bool completed;
    [Serialize]
    public string discoveredRareItem;

    public ResearchOpportunity(string description, int pointValue)
    {
      this.description = description;
      this.dataValue = pointValue;
    }

    [OnDeserialized]
    private void OnDeserialized()
    {
      if (this.discoveredRareResource == (SimHashes) 0)
        this.discoveredRareResource = SimHashes.Void;
      if (this.dataValue <= 50)
        return;
      this.dataValue = 50;
    }

    public bool TryComplete(SpaceDestination destination)
    {
      if (this.completed)
        return false;
      this.completed = true;
      if (this.discoveredRareResource != SimHashes.Void && !destination.recoverableElements.ContainsKey(this.discoveredRareResource))
        destination.recoverableElements.Add(this.discoveredRareResource, Random.value);
      return true;
    }
  }
}
