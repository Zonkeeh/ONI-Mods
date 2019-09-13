// Decompiled with JetBrains decompiler
// Type: InfectedFoodDiagram
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfectedFoodDiagram : MonoBehaviour
{
  public LocText minText;
  public LocText maxText;
  public LocText avgText;
  public LocText medianText;

  private void Update()
  {
    List<InfectedFoodDiagram.FoodBit> source = new List<InfectedFoodDiagram.FoodBit>();
    if ((UnityEngine.Object) WorldInventory.Instance != (UnityEngine.Object) null)
    {
      List<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(GameTags.Edible);
      if (pickupables == null)
        return;
      foreach (Component component1 in pickupables)
      {
        Edible component2 = component1.GetComponent<Edible>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          PrimaryElement component3 = component2.GetComponent<PrimaryElement>();
          source.Add(new InfectedFoodDiagram.FoodBit()
          {
            name = component2.name,
            rations = (float) ((double) component2.Calories / 1000.0 / 1000.0),
            disease = component3.DiseaseCount
          });
        }
      }
    }
    if (source.Count == 0)
      return;
    source.Sort((Comparison<InfectedFoodDiagram.FoodBit>) ((a, b) => a.DiseasePerRation.CompareTo(b.DiseasePerRation)));
    this.minText.text = "Min: " + source[0].ToString();
    this.maxText.text = "Max: " + source[source.Count - 1].ToString();
    this.medianText.text = "Median: " + source[source.Count / 2].ToString();
    float num = source.Select<InfectedFoodDiagram.FoodBit, float>((Func<InfectedFoodDiagram.FoodBit, float>) (b => b.rations)).Sum();
    this.avgText.text = "Average: " + ((float) source.Select<InfectedFoodDiagram.FoodBit, int>((Func<InfectedFoodDiagram.FoodBit, int>) (b => b.disease)).Sum() / num).ToString();
  }

  private class FoodBit
  {
    public string name;
    public float rations;
    public int disease;

    public float DiseasePerRation
    {
      get
      {
        return (float) this.disease / this.rations;
      }
    }

    public override string ToString()
    {
      return string.Format("{0}: {1:0.##} (x{2:0.##} = {3})", (object) this.name, (object) this.DiseasePerRation, (object) this.rations, (object) this.disease);
    }
  }
}
