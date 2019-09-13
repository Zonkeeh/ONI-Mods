// Decompiled with JetBrains decompiler
// Type: UserMenuScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class UserMenuScreen : KIconButtonMenu
{
  private List<MinMaxSlider> sliders = new List<MinMaxSlider>();
  private List<UserMenu.SliderInfo> slidersInfos = new List<UserMenu.SliderInfo>();
  private List<KIconButtonMenu.ButtonInfo> buttonInfos = new List<KIconButtonMenu.ButtonInfo>();
  private GameObject selected;
  public MinMaxSlider sliderPrefab;
  public GameObject sliderParent;
  public PriorityScreen priorityScreenPrefab;
  public GameObject priorityScreenParent;
  private PriorityScreen priorityScreen;

  protected override void OnPrefabInit()
  {
    this.keepMenuOpen = true;
    base.OnPrefabInit();
    this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
    this.priorityScreen.InstantiateButtons(new System.Action<PrioritySetting>(this.OnPriorityClicked), true);
    this.buttonParent.transform.SetAsLastSibling();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.Subscribe(1980521255, new System.Action<object>(this.OnUIRefresh));
  }

  public void SetSelected(GameObject go)
  {
    this.ClearPrioritizable();
    this.selected = go;
    this.RefreshPrioritizable();
  }

  private void ClearPrioritizable()
  {
    if (!((UnityEngine.Object) this.selected != (UnityEngine.Object) null))
      return;
    Prioritizable component = this.selected.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.onPriorityChanged -= new System.Action<PrioritySetting>(this.OnPriorityChanged);
  }

  private void RefreshPrioritizable()
  {
    if (!((UnityEngine.Object) this.selected != (UnityEngine.Object) null))
      return;
    Prioritizable component = this.selected.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.IsPrioritizable())
    {
      component.onPriorityChanged += new System.Action<PrioritySetting>(this.OnPriorityChanged);
      this.priorityScreen.gameObject.SetActive(true);
      this.priorityScreen.SetScreenPriority(component.GetMasterPriority(), false);
    }
    else
      this.priorityScreen.gameObject.SetActive(false);
  }

  public void Refresh(GameObject go)
  {
    if ((UnityEngine.Object) go != (UnityEngine.Object) this.selected)
      return;
    this.buttonInfos.Clear();
    this.slidersInfos.Clear();
    Game.Instance.userMenu.AppendToScreen(go, this);
    this.SetButtons((IList<KIconButtonMenu.ButtonInfo>) this.buttonInfos);
    this.RefreshButtons();
    this.RefreshSliders();
    this.ClearPrioritizable();
    this.RefreshPrioritizable();
    if ((this.sliders == null || this.sliders.Count == 0) && (this.buttonInfos == null || this.buttonInfos.Count == 0) && !this.priorityScreen.gameObject.activeSelf)
      this.transform.parent.gameObject.SetActive(false);
    else
      this.transform.parent.gameObject.SetActive(true);
  }

  public void AddSliders(IList<UserMenu.SliderInfo> sliders)
  {
    this.slidersInfos.AddRange((IEnumerable<UserMenu.SliderInfo>) sliders);
  }

  public void AddButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
  {
    this.buttonInfos.AddRange((IEnumerable<KIconButtonMenu.ButtonInfo>) buttons);
  }

  private void OnUIRefresh(object data)
  {
    this.Refresh(data as GameObject);
  }

  public void RefreshSliders()
  {
    if (this.sliders != null)
    {
      for (int index = 0; index < this.sliders.Count; ++index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.sliders[index].gameObject);
      this.sliders = (List<MinMaxSlider>) null;
    }
    if (this.slidersInfos == null || this.slidersInfos.Count == 0)
      return;
    this.sliders = new List<MinMaxSlider>();
    for (int index = 0; index < this.slidersInfos.Count; ++index)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sliderPrefab.gameObject, Vector3.zero, Quaternion.identity);
      this.slidersInfos[index].sliderGO = gameObject;
      MinMaxSlider component = gameObject.GetComponent<MinMaxSlider>();
      this.sliders.Add(component);
      Transform parent = !((UnityEngine.Object) this.sliderParent != (UnityEngine.Object) null) ? this.transform : this.sliderParent.transform;
      gameObject.transform.SetParent(parent, false);
      gameObject.SetActive(true);
      gameObject.name = "Slider";
      if ((bool) ((UnityEngine.Object) component.toolTip))
        component.toolTip.toolTip = this.slidersInfos[index].toolTip;
      component.lockType = this.slidersInfos[index].lockType;
      component.interactable = this.slidersInfos[index].interactable;
      component.minLimit = this.slidersInfos[index].minLimit;
      component.maxLimit = this.slidersInfos[index].maxLimit;
      component.currentMinValue = this.slidersInfos[index].currentMinValue;
      component.currentMaxValue = this.slidersInfos[index].currentMaxValue;
      component.onMinChange = this.slidersInfos[index].onMinChange;
      component.onMaxChange = this.slidersInfos[index].onMaxChange;
      component.direction = this.slidersInfos[index].direction;
      component.SetMode(this.slidersInfos[index].mode);
      component.SetMinMaxValue(this.slidersInfos[index].currentMinValue, this.slidersInfos[index].currentMaxValue, this.slidersInfos[index].minLimit, this.slidersInfos[index].maxLimit);
    }
  }

  private void OnPriorityClicked(PrioritySetting priority)
  {
    if (!((UnityEngine.Object) this.selected != (UnityEngine.Object) null))
      return;
    Prioritizable component = this.selected.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetMasterPriority(priority);
  }

  private void OnPriorityChanged(PrioritySetting priority)
  {
    this.priorityScreen.SetScreenPriority(priority, false);
  }
}
