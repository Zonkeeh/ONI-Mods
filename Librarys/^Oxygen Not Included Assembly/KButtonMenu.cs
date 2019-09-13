// Decompiled with JetBrains decompiler
// Type: KButtonMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KButtonMenu : KScreen
{
  private static readonly EventSystem.IntraObjectHandler<KButtonMenu> OnSetActivatorDelegate = new EventSystem.IntraObjectHandler<KButtonMenu>((System.Action<KButtonMenu, object>) ((component, data) => component.OnSetActivator(data)));
  [SerializeField]
  protected bool followGameObject;
  [SerializeField]
  protected bool keepMenuOpen;
  [SerializeField]
  protected Transform buttonParent;
  public GameObject buttonPrefab;
  public bool ShouldConsumeMouseScroll;
  [NonSerialized]
  public GameObject[] buttonObjects;
  protected GameObject go;
  protected IList<KButtonMenu.ButtonInfo> buttons;

  protected override void OnActivate()
  {
    this.ConsumeMouseScroll = this.ShouldConsumeMouseScroll;
    this.RefreshButtons();
  }

  public void SetButtons(IList<KButtonMenu.ButtonInfo> buttons)
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
    if (this.buttons == null)
      return;
    this.buttonObjects = new GameObject[this.buttons.Count];
    for (int index = 0; index < this.buttons.Count; ++index)
    {
      KButtonMenu.ButtonInfo binfo = this.buttons[index];
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
      this.buttonObjects[index] = gameObject;
      Transform parent = !((UnityEngine.Object) this.buttonParent != (UnityEngine.Object) null) ? this.transform : this.buttonParent;
      gameObject.transform.SetParent(parent, false);
      gameObject.SetActive(true);
      gameObject.name = binfo.text + "Button";
      LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>(true);
      if (componentsInChildren != null)
      {
        foreach (LocText locText in componentsInChildren)
        {
          locText.text = !(locText.name == "Hotkey") ? binfo.text : GameUtil.GetActionString(binfo.shortcutKey);
          locText.color = !binfo.isEnabled ? new Color(0.5f, 0.5f, 0.5f) : new Color(1f, 1f, 1f);
        }
      }
      ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
      if (binfo.toolTip != null && binfo.toolTip != string.Empty && (UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        componentInChildren.toolTip = binfo.toolTip;
      KButtonMenu screen = this;
      KButton button = gameObject.GetComponent<KButton>();
      button.isInteractable = binfo.isEnabled;
      if (binfo.popupOptions == null && binfo.onPopulatePopup == null)
      {
        UnityAction onClick = binfo.onClick;
        System.Action action = (System.Action) (() =>
        {
          onClick();
          if (this.keepMenuOpen || !((UnityEngine.Object) screen != (UnityEngine.Object) null))
            return;
          screen.Deactivate();
        });
        button.onClick += action;
      }
      else
        button.onClick += (System.Action) (() => this.SetupPopupMenu(binfo, button));
      binfo.uibutton = button;
      if (binfo.onHover == null)
        ;
    }
    this.Update();
  }

  protected UnityEngine.UI.Button.ButtonClickedEvent SetupPopupMenu(
    KButtonMenu.ButtonInfo binfo,
    KButton button)
  {
    UnityEngine.UI.Button.ButtonClickedEvent buttonClickedEvent = new UnityEngine.UI.Button.ButtonClickedEvent();
    UnityAction call = (UnityAction) (() =>
    {
      List<KButtonMenu.ButtonInfo> buttonInfoList = new List<KButtonMenu.ButtonInfo>();
      if (binfo.onPopulatePopup != null)
        binfo.popupOptions = binfo.onPopulatePopup();
      foreach (string popupOption in binfo.popupOptions)
      {
        string delegate_str = popupOption;
        buttonInfoList.Add(new KButtonMenu.ButtonInfo(delegate_str, (UnityAction) (() =>
        {
          binfo.onPopupClick(delegate_str);
          if (this.keepMenuOpen)
            return;
          this.Deactivate();
        }), Action.NumActions, (KButtonMenu.ButtonInfo.HoverCallback) null, (string) null, (GameObject) null, true, (string[]) null, (System.Action<string>) null, (Func<string[]>) null));
      }
      KButtonMenu component = Util.KInstantiate(ScreenPrefabs.Instance.ButtonGrid.gameObject, (GameObject) null, (string) null).GetComponent<KButtonMenu>();
      component.SetButtons((IList<KButtonMenu.ButtonInfo>) buttonInfoList.ToArray());
      RootMenu.Instance.AddSubMenu((KScreen) component);
      Game.Instance.LocalPlayer.ScreenManager.ActivateScreen(component.gameObject, (GameObject) null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
      component.transform.SetPosition(button.transform.GetPosition() + new Vector3()
      {
        x = (!Util.IsOnLeftSideOfScreen(button.transform.GetPosition()) ? (float) (-(double) button.GetComponent<RectTransform>().rect.width * 0.25) : button.GetComponent<RectTransform>().rect.width * 0.25f)
      });
    });
    binfo.onClick = call;
    buttonClickedEvent.AddListener(call);
    return buttonClickedEvent;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.buttons == null)
      return;
    for (int index = 0; index < this.buttons.Count; ++index)
    {
      KButtonMenu.ButtonInfo button = this.buttons[index];
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
    this.Subscribe<KButtonMenu>(315865555, KButtonMenu.OnSetActivatorDelegate);
  }

  private void OnSetActivator(object data)
  {
    this.go = (GameObject) data;
    this.Update();
  }

  protected override void OnDeactivate()
  {
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

  public class ButtonInfo
  {
    public bool isEnabled = true;
    public string text;
    public Action shortcutKey;
    public GameObject visualizer;
    public UnityAction onClick;
    public KButtonMenu.ButtonInfo.HoverCallback onHover;
    public FMODAsset clickSound;
    public KButton uibutton;
    public string toolTip;
    public string[] popupOptions;
    public System.Action<string> onPopupClick;
    public Func<string[]> onPopulatePopup;
    public object userData;

    public ButtonInfo(
      string text = null,
      UnityAction on_click = null,
      Action shortcut_key = Action.NumActions,
      KButtonMenu.ButtonInfo.HoverCallback on_hover = null,
      string tool_tip = null,
      GameObject visualizer = null,
      bool is_enabled = true,
      string[] popup_options = null,
      System.Action<string> on_popup_click = null,
      Func<string[]> on_populate_popup = null)
    {
      this.text = text;
      this.shortcutKey = shortcut_key;
      this.onClick = on_click;
      this.onHover = on_hover;
      this.visualizer = visualizer;
      this.toolTip = tool_tip;
      this.isEnabled = is_enabled;
      this.uibutton = (KButton) null;
      this.popupOptions = popup_options;
      this.onPopupClick = on_popup_click;
      this.onPopulatePopup = on_populate_popup;
    }

    public ButtonInfo(
      string text,
      Action shortcutKey,
      UnityAction onClick,
      KButtonMenu.ButtonInfo.HoverCallback onHover = null,
      object userData = null)
    {
      this.text = text;
      this.shortcutKey = shortcutKey;
      this.onClick = onClick;
      this.onHover = onHover;
      this.userData = userData;
      this.visualizer = (GameObject) null;
      this.uibutton = (KButton) null;
    }

    public ButtonInfo(
      string text,
      GameObject visualizer,
      Action shortcutKey,
      UnityAction onClick,
      KButtonMenu.ButtonInfo.HoverCallback onHover = null,
      object userData = null)
    {
      this.text = text;
      this.shortcutKey = shortcutKey;
      this.onClick = onClick;
      this.onHover = onHover;
      this.visualizer = visualizer;
      this.userData = userData;
      this.uibutton = (KButton) null;
    }

    public delegate void HoverCallback(GameObject hoverTarget);

    public delegate void Callback();
  }
}
