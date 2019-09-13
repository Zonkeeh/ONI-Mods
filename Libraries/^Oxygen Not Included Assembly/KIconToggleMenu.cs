// Decompiled with JetBrains decompiler
// Type: KIconToggleMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KIconToggleMenu : KScreen
{
  [SerializeField]
  protected bool repeatKeyDownToggles = true;
  protected List<KToggle> toggles = new List<KToggle>();
  private List<KToggle> dontDestroyToggles = new List<KToggle>();
  protected int selected = -1;
  [SerializeField]
  private Transform toggleParent;
  [SerializeField]
  private KToggle prefab;
  [SerializeField]
  private ToggleGroup group;
  [SerializeField]
  private Sprite[] icons;
  [SerializeField]
  public TextStyleSetting ToggleToolTipTextStyleSetting;
  [SerializeField]
  public TextStyleSetting ToggleToolTipHeaderTextStyleSetting;
  protected KToggle currentlySelectedToggle;
  protected IList<KIconToggleMenu.ToggleInfo> toggleInfo;

  public event KIconToggleMenu.OnSelect onSelect;

  public void Setup(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
  {
    this.toggleInfo = toggleInfo;
    this.RefreshButtons();
  }

  protected void Setup()
  {
    this.RefreshButtons();
  }

  protected virtual void RefreshButtons()
  {
    foreach (KToggle toggle in this.toggles)
    {
      if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
      {
        if (!this.dontDestroyToggles.Contains(toggle))
          UnityEngine.Object.Destroy((UnityEngine.Object) toggle.gameObject);
        else
          toggle.ClearOnClick();
      }
    }
    this.toggles.Clear();
    this.dontDestroyToggles.Clear();
    if (this.toggleInfo == null)
      return;
    Transform transform1 = !((UnityEngine.Object) this.toggleParent != (UnityEngine.Object) null) ? this.transform : this.toggleParent;
    for (int index = 0; index < this.toggleInfo.Count; ++index)
    {
      int idx = index;
      KIconToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[index];
      KToggle ktoggle;
      if ((UnityEngine.Object) toggleInfo.instanceOverride != (UnityEngine.Object) null)
      {
        ktoggle = toggleInfo.instanceOverride;
        this.dontDestroyToggles.Add(ktoggle);
      }
      else
        ktoggle = !(bool) ((UnityEngine.Object) toggleInfo.prefabOverride) ? Util.KInstantiateUI<KToggle>(this.prefab.gameObject, transform1.gameObject, true) : Util.KInstantiateUI<KToggle>(toggleInfo.prefabOverride.gameObject, transform1.gameObject, true);
      ktoggle.Deselect();
      ktoggle.gameObject.name = "Toggle:" + toggleInfo.text;
      ktoggle.group = this.group;
      ktoggle.onClick += (System.Action) (() => this.OnClick(idx));
      Transform transform2 = ktoggle.transform.Find("Text");
      if ((UnityEngine.Object) transform2 != (UnityEngine.Object) null)
      {
        LocText component = transform2.GetComponent<LocText>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.text = toggleInfo.text;
      }
      ToolTip component1 = ktoggle.GetComponent<ToolTip>();
      if ((bool) ((UnityEngine.Object) component1))
      {
        if (toggleInfo.tooltipHeader != string.Empty)
        {
          component1.AddMultiStringTooltip(toggleInfo.tooltipHeader, !((UnityEngine.Object) this.ToggleToolTipHeaderTextStyleSetting != (UnityEngine.Object) null) ? (ScriptableObject) this.ToggleToolTipTextStyleSetting : (ScriptableObject) this.ToggleToolTipHeaderTextStyleSetting);
          if ((UnityEngine.Object) this.ToggleToolTipHeaderTextStyleSetting == (UnityEngine.Object) null)
            Debug.Log((object) "!");
        }
        component1.AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(toggleInfo.tooltip, toggleInfo.hotKey), (ScriptableObject) this.ToggleToolTipTextStyleSetting);
      }
      if (toggleInfo.getSpriteCB != null)
        ktoggle.fgImage.sprite = toggleInfo.getSpriteCB();
      else if (toggleInfo.icon != null)
        ktoggle.fgImage.sprite = Assets.GetSprite((HashedString) toggleInfo.icon);
      toggleInfo.toggle = ktoggle;
      this.toggles.Add(ktoggle);
    }
  }

  public Sprite GetIcon(string name)
  {
    foreach (Sprite icon in this.icons)
    {
      if (icon.name == name)
        return icon;
    }
    return (Sprite) null;
  }

  public virtual void ClearSelection()
  {
    if (this.toggles == null)
      return;
    foreach (KToggle toggle in this.toggles)
    {
      toggle.Deselect();
      toggle.ClearAnimState();
    }
    this.selected = -1;
  }

  private void OnClick(int i)
  {
    if (this.onSelect == null)
      return;
    this.selected = i;
    this.onSelect(this.toggleInfo[i]);
    if (!this.toggles[i].isOn)
      this.selected = -1;
    for (int index = 0; index < this.toggles.Count; ++index)
    {
      if (index != this.selected)
        this.toggles[index].isOn = false;
    }
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.toggles == null || this.toggleInfo == null)
      return;
    for (int index = 0; index < this.toggleInfo.Count; ++index)
    {
      if (this.toggles[index].isActiveAndEnabled)
      {
        Action hotKey = this.toggleInfo[index].hotKey;
        if (hotKey != Action.NumActions && e.TryConsume(hotKey))
        {
          if (this.selected == index && !this.repeatKeyDownToggles)
            break;
          this.toggles[index].Click();
          if (this.selected == index)
            this.toggles[index].Deselect();
          this.selected = index;
          break;
        }
      }
    }
  }

  public virtual void Close()
  {
    this.ClearSelection();
    this.Show(false);
  }

  public delegate void OnSelect(KIconToggleMenu.ToggleInfo toggleInfo);

  public class ToggleInfo
  {
    public string text;
    public object userData;
    public string icon;
    public string tooltip;
    public string tooltipHeader;
    public KToggle toggle;
    public Action hotKey;
    public Func<Sprite> getSpriteCB;
    public KToggle prefabOverride;
    public KToggle instanceOverride;

    public ToggleInfo(
      string text,
      string icon,
      object user_data = null,
      Action hotkey = Action.NumActions,
      string tooltip = "",
      string tooltip_header = "")
    {
      this.text = text;
      this.userData = user_data;
      this.icon = icon;
      this.hotKey = hotkey;
      this.tooltip = tooltip;
      this.tooltipHeader = tooltip_header;
    }

    public ToggleInfo(string text, object user_data, Action hotkey, Func<Sprite> get_sprite_cb)
    {
      this.text = text;
      this.userData = user_data;
      this.hotKey = hotkey;
      this.getSpriteCB = get_sprite_cb;
    }
  }
}
