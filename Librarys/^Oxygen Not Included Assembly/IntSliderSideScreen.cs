// Decompiled with JetBrains decompiler
// Type: IntSliderSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class IntSliderSideScreen : SideScreenContent
{
  private IIntSliderControl target;
  public List<SliderSet> sliderSets;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.sliderSets.Count; ++index)
    {
      this.sliderSets[index].SetupSlider(index);
      this.sliderSets[index].valueSlider.wholeNumbers = true;
    }
  }

  public override bool IsValidForTarget(GameObject target)
  {
    if (target.GetComponent<IIntSliderControl>() == null)
      return target.GetSMI<IIntSliderControl>() != null;
    return true;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((Object) new_target == (Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<IIntSliderControl>();
      if (this.target == null)
        this.target = new_target.GetSMI<IIntSliderControl>();
      if (this.target == null)
      {
        Debug.LogError((object) "The gameObject received does not contain a Manual Generator component");
      }
      else
      {
        this.titleKey = this.target.SliderTitleKey;
        for (int index = 0; index < this.sliderSets.Count; ++index)
          this.sliderSets[index].SetTarget((ISliderControl) this.target);
      }
    }
  }
}
