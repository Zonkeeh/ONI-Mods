// Decompiled with JetBrains decompiler
// Type: UserMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserMenu
{
  private List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>> buttons = new List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>>();
  private List<UserMenu.SliderInfo> sliders = new List<UserMenu.SliderInfo>();
  private List<KIconButtonMenu.ButtonInfo> sortedButtons = new List<KIconButtonMenu.ButtonInfo>();

  public void Refresh(GameObject go)
  {
    Game.Instance.Trigger(1980521255, (object) go);
  }

  public void AddButton(GameObject go, KIconButtonMenu.ButtonInfo button, float sort_order = 1f)
  {
    if (button.onClick != null)
    {
      System.Action callback = button.onClick;
      button.onClick = (System.Action) (() =>
      {
        callback();
        Game.Instance.Trigger(1980521255, (object) go);
      });
    }
    this.buttons.Add(new KeyValuePair<KIconButtonMenu.ButtonInfo, float>(button, sort_order));
  }

  public void AddSlider(GameObject go, UserMenu.SliderInfo slider)
  {
    this.sliders.Add(slider);
  }

  public void AppendToScreen(GameObject go, UserMenuScreen screen)
  {
    this.buttons.Clear();
    this.sliders.Clear();
    go.Trigger(493375141, (object) null);
    if (this.buttons.Count > 0)
    {
      this.buttons.Sort((Comparison<KeyValuePair<KIconButtonMenu.ButtonInfo, float>>) ((x, y) =>
      {
        if ((double) x.Value == (double) y.Value)
          return 0;
        return (double) x.Value > (double) y.Value ? 1 : -1;
      }));
      for (int index = 0; index < this.buttons.Count; ++index)
        this.sortedButtons.Add(this.buttons[index].Key);
      screen.AddButtons((IList<KIconButtonMenu.ButtonInfo>) this.sortedButtons);
      this.sortedButtons.Clear();
    }
    if (this.sliders.Count <= 0)
      return;
    screen.AddSliders((IList<UserMenu.SliderInfo>) this.sliders);
  }

  public class SliderInfo
  {
    public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;
    public bool interactable = true;
    public float maxLimit = 100f;
    public float currentMinValue = 10f;
    public float currentMaxValue = 90f;
    public MinMaxSlider.Mode mode;
    public Slider.Direction direction;
    public bool lockRange;
    public string toolTip;
    public string toolTipMin;
    public string toolTipMax;
    public float minLimit;
    public GameObject sliderGO;
    public System.Action<MinMaxSlider> onMinChange;
    public System.Action<MinMaxSlider> onMaxChange;
  }
}
