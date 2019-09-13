// Decompiled with JetBrains decompiler
// Type: RationTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;

[SerializationConfig(MemberSerialization.OptIn)]
public class RationTracker : KMonoBehaviour, ISaveLoadable
{
  private static readonly EventSystem.IntraObjectHandler<RationTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<RationTracker>((System.Action<RationTracker, object>) ((component, data) => component.OnNewDay(data)));
  [Serialize]
  public RationTracker.Frame currentFrame = new RationTracker.Frame();
  [Serialize]
  public RationTracker.Frame previousFrame = new RationTracker.Frame();
  [Serialize]
  public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();
  private static RationTracker instance;

  public static void DestroyInstance()
  {
    RationTracker.instance = (RationTracker) null;
  }

  public static RationTracker Get()
  {
    return RationTracker.instance;
  }

  protected override void OnPrefabInit()
  {
    RationTracker.instance = this;
  }

  protected override void OnSpawn()
  {
    this.Subscribe<RationTracker>(631075836, RationTracker.OnNewDayDelegate);
  }

  private void OnNewDay(object data)
  {
    this.previousFrame = this.currentFrame;
    this.currentFrame = new RationTracker.Frame();
  }

  public float CountRations(Dictionary<string, float> unitCountByFoodType, bool excludeUnreachable = true)
  {
    float num = 0.0f;
    List<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(GameTags.Edible);
    if (pickupables != null)
    {
      foreach (Pickupable pickupable in pickupables)
      {
        if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
        {
          Edible component = pickupable.GetComponent<Edible>();
          num += component.Calories;
          if (unitCountByFoodType != null)
          {
            if (!unitCountByFoodType.ContainsKey(component.FoodID))
              unitCountByFoodType[component.FoodID] = 0.0f;
            Dictionary<string, float> dictionary;
            string foodId;
            (dictionary = unitCountByFoodType)[foodId = component.FoodID] = dictionary[foodId] + component.Units;
          }
        }
      }
    }
    return num;
  }

  public float CountRationsByFoodType(string foodID, bool excludeUnreachable = true)
  {
    float num = 0.0f;
    List<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(GameTags.Edible);
    if (pickupables != null)
    {
      foreach (Pickupable pickupable in pickupables)
      {
        if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
        {
          Edible component = pickupable.GetComponent<Edible>();
          if (component.FoodID == foodID)
            num += component.Calories;
        }
      }
    }
    return num;
  }

  public void RegisterCaloriesProduced(float calories)
  {
    this.currentFrame.caloriesProduced += calories;
  }

  public void RegisterRationsConsumed(Edible edible)
  {
    this.currentFrame.caloriesConsumed += edible.caloriesConsumed;
    if (!this.caloriesConsumedByFood.ContainsKey(edible.FoodInfo.Id))
    {
      this.caloriesConsumedByFood.Add(edible.FoodInfo.Id, edible.caloriesConsumed);
    }
    else
    {
      Dictionary<string, float> caloriesConsumedByFood;
      string id;
      (caloriesConsumedByFood = this.caloriesConsumedByFood)[id = edible.FoodInfo.Id] = caloriesConsumedByFood[id] + edible.caloriesConsumed;
    }
  }

  public float GetCaloiresConsumedByFood(List<string> foodTypes)
  {
    float num = 0.0f;
    foreach (string foodType in foodTypes)
    {
      if (this.caloriesConsumedByFood.ContainsKey(foodType))
        num += this.caloriesConsumedByFood[foodType];
    }
    return num;
  }

  public float GetCaloriesConsumed()
  {
    float num = 0.0f;
    foreach (KeyValuePair<string, float> keyValuePair in this.caloriesConsumedByFood)
      num += keyValuePair.Value;
    return num;
  }

  public struct Frame
  {
    public float caloriesProduced;
    public float caloriesConsumed;
  }
}
