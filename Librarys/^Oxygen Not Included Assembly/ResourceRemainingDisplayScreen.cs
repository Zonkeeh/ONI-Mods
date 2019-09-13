// Decompiled with JetBrains decompiler
// Type: ResourceRemainingDisplayScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ResourceRemainingDisplayScreen : KScreen
{
  private List<Tag> selected_elements = new List<Tag>();
  public static ResourceRemainingDisplayScreen instance;
  public GameObject dispayPrefab;
  public LocText label;
  private Recipe currentRecipe;
  private int numberOfPendingConstructions;
  private int displayedConstructionCostMultiplier;
  private RectTransform rect;

  public static void DestroyInstance()
  {
    ResourceRemainingDisplayScreen.instance = (ResourceRemainingDisplayScreen) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Activate();
    ResourceRemainingDisplayScreen.instance = this;
    this.dispayPrefab.SetActive(false);
  }

  public void ActivateDisplay(GameObject target)
  {
    this.numberOfPendingConstructions = 0;
    this.dispayPrefab.SetActive(true);
  }

  public void DeactivateDisplay()
  {
    this.dispayPrefab.SetActive(false);
  }

  public void SetResources(IList<Tag> _selected_elements, Recipe recipe)
  {
    this.selected_elements.Clear();
    foreach (Tag selectedElement in (IEnumerable<Tag>) _selected_elements)
      this.selected_elements.Add(selectedElement);
    this.currentRecipe = recipe;
    Debug.Assert(this.selected_elements.Count == recipe.Ingredients.Count);
  }

  public void SetNumberOfPendingConstructions(int number)
  {
    this.numberOfPendingConstructions = number;
  }

  public void Update()
  {
    if (!this.dispayPrefab.activeSelf)
      return;
    if ((Object) this.canvas != (Object) null)
    {
      if ((Object) this.rect == (Object) null)
        this.rect = this.GetComponent<RectTransform>();
      this.rect.anchoredPosition = (Vector2) this.WorldToScreen(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
    }
    if (this.displayedConstructionCostMultiplier == this.numberOfPendingConstructions)
      this.label.text = string.Empty;
    else
      this.displayedConstructionCostMultiplier = this.numberOfPendingConstructions;
  }

  public string GetString()
  {
    string str = string.Empty;
    if (this.selected_elements != null && this.currentRecipe != null)
    {
      for (int index = 0; index < this.currentRecipe.Ingredients.Count; ++index)
      {
        Tag selectedElement = this.selected_elements[index];
        float num1 = this.currentRecipe.Ingredients[index].amount * (float) this.numberOfPendingConstructions;
        float num2 = WorldInventory.Instance.GetTotalAmount(selectedElement) - WorldInventory.Instance.GetAmount(selectedElement);
        float mass = WorldInventory.Instance.GetTotalAmount(selectedElement) - (num2 + num1);
        if ((double) mass < 0.0)
          mass = 0.0f;
        str = str + selectedElement.ProperName() + ": " + GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}") + " / " + GameUtil.GetFormattedMass(this.currentRecipe.Ingredients[index].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
        if (index < this.selected_elements.Count - 1)
          str += "\n";
      }
    }
    return str;
  }
}
