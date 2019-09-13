// Decompiled with JetBrains decompiler
// Type: PlanterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class PlanterSideScreen : ReceptacleSideScreen
{
  public DescriptorPanel RequirementsDescriptorPanel;
  public DescriptorPanel HarvestDescriptorPanel;
  public DescriptorPanel EffectsDescriptorPanel;

  public override bool IsValidForTarget(GameObject target)
  {
    return (Object) target.GetComponent<PlantablePlot>() != (Object) null;
  }

  protected override Sprite GetEntityIcon(Tag prefabTag)
  {
    PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
    if ((Object) component != (Object) null)
      return base.GetEntityIcon(new Tag(component.PlantID));
    return base.GetEntityIcon(prefabTag);
  }

  protected override void SetResultDescriptions(GameObject seed_or_plant)
  {
    string empty = string.Empty;
    GameObject go = seed_or_plant;
    PlantableSeed component1 = seed_or_plant.GetComponent<PlantableSeed>();
    List<Descriptor> list = new List<Descriptor>();
    if ((Object) component1 != (Object) null)
    {
      list = component1.GetDescriptors(component1.gameObject);
      if ((Object) this.targetReceptacle.rotatable != (Object) null && this.targetReceptacle.Direction != component1.direction)
      {
        if (component1.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
          empty += (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_FLOOR;
        else if (component1.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
          empty += (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_WALL;
        else if (component1.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
          empty += (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_CEILING;
        empty += "\n\n";
      }
      go = Assets.GetPrefab(component1.PlantID);
      if (!string.IsNullOrEmpty(component1.domesticatedDescription))
        empty += component1.domesticatedDescription;
    }
    else
    {
      InfoDescription component2 = go.GetComponent<InfoDescription>();
      if ((bool) ((Object) component2))
        empty += component2.description;
    }
    this.descriptionLabel.SetText(empty);
    List<Descriptor> cycleDescriptors = GameUtil.GetPlantLifeCycleDescriptors(go);
    if (cycleDescriptors.Count > 0)
    {
      this.HarvestDescriptorPanel.SetDescriptors((IList<Descriptor>) cycleDescriptors);
      this.HarvestDescriptorPanel.gameObject.SetActive(true);
    }
    List<Descriptor> requirementDescriptors = GameUtil.GetPlantRequirementDescriptors(go);
    if (list.Count > 0)
    {
      GameUtil.IndentListOfDescriptors(list, 1);
      requirementDescriptors.InsertRange(requirementDescriptors.Count, (IEnumerable<Descriptor>) list);
    }
    if (requirementDescriptors.Count > 0)
    {
      this.RequirementsDescriptorPanel.SetDescriptors((IList<Descriptor>) requirementDescriptors);
      this.RequirementsDescriptorPanel.gameObject.SetActive(true);
    }
    List<Descriptor> effectDescriptors = GameUtil.GetPlantEffectDescriptors(go);
    if (effectDescriptors.Count > 0)
    {
      this.EffectsDescriptorPanel.SetDescriptors((IList<Descriptor>) effectDescriptors);
      this.EffectsDescriptorPanel.gameObject.SetActive(true);
    }
    else
      this.EffectsDescriptorPanel.gameObject.SetActive(false);
  }

  protected override bool AdditionalCanDepositTest()
  {
    return (this.targetReceptacle as PlantablePlot).ValidPlant;
  }
}
