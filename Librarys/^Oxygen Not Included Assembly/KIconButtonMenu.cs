// Decompiled with JetBrains decompiler
// Type: KIconButtonMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KIconButtonMenu : KScreen
{
  private static readonly EventSystem.IntraObjectHandler<KIconButtonMenu> OnSetActivatorDelegate = new EventSystem.IntraObjectHandler<KIconButtonMenu>((System.Action<KIconButtonMenu, object>) ((component, data) => component.OnSetActivator(data)));
  [SerializeField]
  protected bool automaticNavigation = true;
  [SerializeField]
  protected bool followGameObject;
  [SerializeField]
  protected bool keepMenuOpen;
  [SerializeField]
  protected Transform buttonParent;
  [SerializeField]
  private GameObject buttonPrefab;
  [SerializeField]
  protected Sprite[] icons;
  [SerializeField]
  private ToggleGroup externalToggleGroup;
  protected KToggle currentlySelectedToggle;
  [NonSerialized]
  public GameObject[] buttonObjects;
  [SerializeField]
  public TextStyleSetting ToggleToolTipTextStyleSetting;
  protected GameObject go;
  protected IList<KIconButtonMenu.ButtonInfo> buttons;

  protected override void OnActivate()
  {
    base.OnActivate();
    this.RefreshButtons();
  }

  public void SetButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
  {
    this.buttons = buttons;
    if (!this.activateOnSpawn)
      return;
    this.RefreshButtons();
  }

  public virtual void RefreshButtons()
  {
    if (this.buttonObjects != null)
    {
      for (int index = 0; index < this.buttonObjects.Length; ++index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.buttonObjects[index]);
      this.buttonObjects = (GameObject[]) null;
    }
    if (this.buttons == null || this.buttons.Count == 0)
      return;
    this.buttonObjects = new GameObject[this.buttons.Count];
    for (int index = 0; index < this.buttons.Count; ++index)
    {
      KIconButtonMenu.ButtonInfo button = this.buttons[index];
      if (button != null)
      {
        GameObject binstance = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
        button.buttonGo = binstance;
        this.buttonObjects[index] = binstance;
        Transform parent = !((UnityEngine.Object) this.buttonParent != (UnityEngine.Object) null) ? this.transform : this.buttonParent;
        binstance.transform.SetParent(parent, false);
        binstance.SetActive(true);
        binstance.name = button.text + "Button";
        KButton component1 = binstance.GetComponent<KButton>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && button.onClick != null)
          component1.onClick += button.onClick;
        Image image = (Image) null;
        if ((bool) ((UnityEngine.Object) component1))
          image = component1.fgImage;
        if ((UnityEngine.Object) image != (UnityEngine.Object) null)
        {
          image.gameObject.SetActive(false);
          foreach (Sprite icon in this.icons)
          {
            if ((UnityEngine.Object) icon != (UnityEngine.Object) null && icon.name == button.iconName)
            {
              image.sprite = icon;
              image.gameObject.SetActive(true);
              break;
            }
          }
        }
        if ((UnityEngine.Object) button.texture != (UnityEngine.Object) null)
        {
          RawImage componentInChildren = binstance.GetComponentInChildren<RawImage>();
          if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
          {
            componentInChildren.gameObject.SetActive(true);
            componentInChildren.texture = button.texture;
          }
        }
        ToolTip componentInChildren1 = binstance.GetComponentInChildren<ToolTip>();
        if (button.text != null && button.text != string.Empty && (UnityEngine.Object) componentInChildren1 != (UnityEngine.Object) null)
        {
          componentInChildren1.toolTip = button.GetTooltipText();
          LocText componentInChildren2 = binstance.GetComponentInChildren<LocText>();
          if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null)
            componentInChildren2.text = button.text;
        }
        if (button.onToolTip != null)
          componentInChildren1.OnToolTip = button.onToolTip;
        KIconButtonMenu screen = this;
        System.Action onClick = button.onClick;
        System.Action action = (System.Action) (() =>
        {
          onClick.Signal();
          if (!this.keepMenuOpen && (UnityEngine.Object) screen != (UnityEngine.Object) null)
            screen.Deactivate();
          if (!((UnityEngine.Object) binstance != (UnityEngine.Object) null))
            return;
          KToggle component = binstance.GetComponent<KToggle>();
          if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
          this.SelectToggle(component);
        });
        KToggle componentInChildren3 = binstance.GetComponentInChildren<KToggle>();
        if ((UnityEngine.Object) componentInChildren3 != (UnityEngine.Object) null)
        {
          componentInChildren3.onRefresh += button.onRefresh;
          ToggleGroup toggleGroup = this.GetComponent<ToggleGroup>();
          if ((UnityEngine.Object) toggleGroup == (UnityEngine.Object) null)
            toggleGroup = this.externalToggleGroup;
          componentInChildren3.group = toggleGroup;
          componentInChildren3.onClick += action;
          Navigation navigation = componentInChildren3.navigation;
          navigation.mode = !this.automaticNavigation ? Navigation.Mode.None : Navigation.Mode.Automatic;
          componentInChildren3.navigation = navigation;
        }
        else
        {
          KBasicToggle componentInChildren2 = binstance.GetComponentInChildren<KBasicToggle>();
          if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null)
            componentInChildren2.onClick += action;
        }
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          component1.isInteractable = button.isInteractable;
        button.onCreate.Signal<KIconButtonMenu.ButtonInfo>(button);
      }
    }
    this.Update();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.buttons == null || !this.gameObject.activeSelf || !this.enabled)
      return;
    for (int index = 0; index < this.buttons.Count; ++index)
    {
      KIconButtonMenu.ButtonInfo button = this.buttons[index];
      if (e.TryConsume(button.shortcutKey))
      {
        this.buttonObjects[index].GetComponent<KButton>().PlayPointerDownSound();
        this.buttonObjects[index].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
        break;
      }
    }
    base.OnKeyDown(e);
  }

  protected override void OnPrefabInit()
  {
    this.Subscribe<KIconButtonMenu>(315865555, KIconButtonMenu.OnSetActivatorDelegate);
  }

  private void OnSetActivator(object data)
  {
    this.go = (GameObject) data;
    this.Update();
  }

  private void Update()
  {
    if (!this.followGameObject || (UnityEngine.Object) this.go == (UnityEngine.Object) null || (UnityEngine.Object) this.canvas == (UnityEngine.Object) null)
      return;
    Vector3 viewportPoint = Camera.main.WorldToViewportPoint(this.go.transform.GetPosition());
    RectTransform component1 = this.GetComponent<RectTransform>();
    RectTransform component2 = this.canvas.GetComponent<RectTransform>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    component1.anchoredPosition = new Vector2((float) ((double) viewportPoint.x * (double) component2.sizeDelta.x - (double) component2.sizeDelta.x * 0.5), (float) ((double) viewportPoint.y * (double) component2.sizeDelta.y - (double) component2.sizeDelta.y * 0.5));
  }

  protected void SelectToggle(KToggle selectedToggle)
  {
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current == (UnityEngine.Object) null || !UnityEngine.EventSystems.EventSystem.current.enabled)
      return;
    this.currentlySelectedToggle = !((UnityEngine.Object) this.currentlySelectedToggle == (UnityEngine.Object) selectedToggle) ? selectedToggle : (KToggle) null;
    foreach (GameObject buttonObject in this.buttonObjects)
    {
      KToggle component = buttonObject.GetComponent<KToggle>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) component == (UnityEngine.Object) this.currentlySelectedToggle)
        {
          component.Select();
          component.isOn = true;
        }
        else
        {
          component.Deselect();
          component.isOn = false;
        }
      }
    }
  }

  public void ClearSelection()
  {
    foreach (GameObject buttonObject in this.buttonObjects)
    {
      KToggle component1 = buttonObject.GetComponent<KToggle>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        component1.Deselect();
        component1.isOn = false;
      }
      else
      {
        KBasicToggle component2 = buttonObject.GetComponent<KBasicToggle>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.isOn = false;
      }
      ImageToggleState component3 = buttonObject.GetComponent<ImageToggleState>();
      if (component3.GetIsActive())
        component3.SetInactive();
    }
    ToggleGroup component = this.GetComponent<ToggleGroup>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetAllTogglesOff();
    this.SelectToggle((KToggle) null);
  }

  public class ButtonInfo
  {
    public string iconName;
    public string text;
    public string tooltipText;
    public string[] multiText;
    public Action shortcutKey;
    public bool isInteractable;
    public System.Action<KIconButtonMenu.ButtonInfo> onCreate;
    public System.Action onClick;
    public System.Action<GameObject> onRefresh;
    public Func<string> onToolTip;
    public GameObject buttonGo;
    public object userData;
    public Texture texture;

    public ButtonInfo(
      string iconName = "",
      string text = "",
      System.Action on_click = null,
      Action shortcutKey = Action.NumActions,
      System.Action<GameObject> on_refresh = null,
      System.Action<KIconButtonMenu.ButtonInfo> on_create = null,
      Texture texture = null,
      string tooltipText = "",
      bool is_interactable = true)
    {
      this.iconName = iconName;
      this.text = text;
      this.shortcutKey = shortcutKey;
      this.onClick = on_click;
      this.onRefresh = on_refresh;
      this.onCreate = on_create;
      this.texture = texture;
      this.tooltipText = tooltipText;
      this.isInteractable = is_interactable;
    }

    public string GetTooltipText()
    {
      string template = !(this.tooltipText == string.Empty) ? this.tooltipText : this.text;
      if (this.shortcutKey != Action.NumActions)
        template = GameUtil.ReplaceHotkeyString(template, this.shortcutKey);
      return template;
    }

    public delegate void Callback();
  }
}
