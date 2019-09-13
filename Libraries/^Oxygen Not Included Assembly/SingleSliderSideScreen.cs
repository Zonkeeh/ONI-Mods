// Decompiled with JetBrains decompiler
// Type: SingleSliderSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SingleSliderSideScreen : SideScreenContent
{
  private ISingleSliderControl target;
  public List<SliderSet> sliderSets;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.sliderSets.Count; ++index)
      this.sliderSets[index].SetupSlider(index);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    KPrefabID component = target.GetComponent<KPrefabID>();
    if (target.GetComponent<ISingleSliderControl>() != null && !component.HasTag("HydrogenGenerator".ToTag()) && !component.HasTag("MethaneGenerator".ToTag()))
      return !component.HasTag("PetroleumGenerator".ToTag());
    return false;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((Object) new_target == (Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<ISingleSliderControl>();
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
