// Decompiled with JetBrains decompiler
// Type: EdiblesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;

public class EdiblesManager : KMonoBehaviour
{
  public EdiblesManager.FoodInfo GetFoodInfo(string FoodID)
  {
    string str = FoodID.Replace("Compost", string.Empty);
    foreach (EdiblesManager.FoodInfo foodTypes in FOOD.FOOD_TYPES_LIST)
    {
      if (foodTypes.Id == str)
        return foodTypes;
    }
    Debug.LogWarning((object) ("No food with ID: " + FoodID));
    return (EdiblesManager.FoodInfo) null;
  }

  public class FoodInfo : IConsumableUIItem
  {
    public string Id;
    public string Name;
    public string Description;
    public float CaloriesPerUnit;
    public float PreserveTemperature;
    public float RotTemperature;
    public float StaleTime;
    public float SpoilTime;
    public bool CanRot;
    public int Quality;
    public List<string> Effects;

    public FoodInfo(
      string id,
      float caloriesPerUnit,
      int quality,
      float preserveTemperatue,
      float rotTemperature,
      float spoilTime,
      bool can_rot)
    {
      this.Id = id;
      this.CaloriesPerUnit = caloriesPerUnit;
      this.Quality = quality;
      this.PreserveTemperature = preserveTemperatue;
      this.RotTemperature = rotTemperature;
      this.StaleTime = spoilTime / 2f;
      this.SpoilTime = spoilTime;
      this.CanRot = can_rot;
      this.Name = (string) Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".NAME");
      this.Description = (string) Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".DESC");
      this.Effects = new List<string>();
      FOOD.FOOD_TYPES_LIST.Add(this);
    }

    public EdiblesManager.FoodInfo AddEffects(List<string> effects)
    {
      this.Effects.AddRange((IEnumerable<string>) effects);
      return this;
    }

    public string ConsumableId
    {
      get
      {
        return this.Id;
      }
    }

    public string ConsumableName
    {
      get
      {
        return this.Name;
      }
    }

    public int MajorOrder
    {
      get
      {
        return this.Quality;
      }
    }

    public int MinorOrder
    {
      get
      {
        return (int) this.CaloriesPerUnit;
      }
    }

    public bool Display
    {
      get
      {
        return (double) this.CaloriesPerUnit != 0.0;
      }
    }
  }
}
