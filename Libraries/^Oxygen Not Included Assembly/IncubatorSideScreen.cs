// Decompiled with JetBrains decompiler
// Type: IncubatorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class IncubatorSideScreen : ReceptacleSideScreen
{
  public DescriptorPanel RequirementsDescriptorPanel;
  public DescriptorPanel HarvestDescriptorPanel;
  public DescriptorPanel EffectsDescriptorPanel;
  public MultiToggle continuousToggle;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<EggIncubator>() != (UnityEngine.Object) null;
  }

  protected override void SetResultDescriptions(GameObject go)
  {
    string empty = string.Empty;
    InfoDescription component = go.GetComponent<InfoDescription>();
    if ((bool) ((UnityEngine.Object) component))
      empty += component.description;
    this.descriptionLabel.SetText(empty);
  }

  protected override bool RequiresAvailableAmountToDeposit()
  {
    return false;
  }

  protected override Sprite GetEntityIcon(Tag prefabTag)
  {
    return Def.GetUISprite((object) Assets.GetPrefab(prefabTag), "ui", false).first;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    EggIncubator incubator = target.GetComponent<EggIncubator>();
    this.continuousToggle.ChangeState(!incubator.autoReplaceEntity ? 1 : 0);
    this.continuousToggle.onClick = (System.Action) (() =>
    {
      incubator.autoReplaceEntity = !incubator.autoReplaceEntity;
      this.continuousToggle.ChangeState(!incubator.autoReplaceEntity ? 1 : 0);
    });
  }
}
