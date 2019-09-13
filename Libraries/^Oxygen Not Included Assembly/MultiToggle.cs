// Decompiled with JetBrains decompiler
// Type: MultiToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiToggle : KMonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
  public bool play_sound_on_click = true;
  protected float heldTimeThreshold = 0.4f;
  [Header("Settings")]
  [SerializeField]
  public ToggleState[] states;
  public bool play_sound_on_release;
  public Image toggle_image;
  protected int state;
  public System.Action onClick;
  public System.Action onEnter;
  public System.Action onExit;
  public System.Action onHold;
  public System.Action onStopHold;
  protected bool clickHeldDown;
  protected float totalHeldTime;
  private bool pointerOver;

  public int CurrentState
  {
    get
    {
      return this.state;
    }
  }

  public void NextState()
  {
    this.ChangeState((this.state + 1) % this.states.Length);
  }

  protected virtual void Update()
  {
    if (!this.clickHeldDown)
      return;
    this.totalHeldTime += Time.unscaledDeltaTime;
    if ((double) this.totalHeldTime <= (double) this.heldTimeThreshold || this.onHold == null)
      return;
    this.onHold();
  }

  public void ChangeState(int new_state_index)
  {
    this.state = new_state_index;
    try
    {
      this.toggle_image.sprite = this.states[new_state_index].sprite;
      this.toggle_image.color = this.states[new_state_index].color;
      if (this.states[new_state_index].use_rect_margins)
        this.toggle_image.rectTransform().sizeDelta = this.states[new_state_index].rect_margins;
    }
    catch
    {
      string str = this.gameObject.name;
      for (Transform transform = this.transform; (UnityEngine.Object) transform.parent != (UnityEngine.Object) null; transform = transform.parent)
        str = str.Insert(0, transform.name + ">");
      Debug.LogError((object) ("Multi Toggle state index out of range: " + str + " idx:" + (object) new_state_index), (UnityEngine.Object) this.gameObject);
    }
    foreach (StatePresentationSetting additionalDisplaySetting in this.states[this.state].additional_display_settings)
    {
      if (!((UnityEngine.Object) additionalDisplaySetting.image_target == (UnityEngine.Object) null))
      {
        additionalDisplaySetting.image_target.sprite = additionalDisplaySetting.sprite;
        additionalDisplaySetting.image_target.color = additionalDisplaySetting.color;
      }
    }
    this.RefreshHoverColor();
  }

  public virtual void OnPointerClick(PointerEventData eventData)
  {
    if (this.states.Length - 1 < this.state)
      Debug.LogWarning((object) "Multi toggle has too few / no states");
    if (this.onClick != null)
      this.onClick();
    this.RefreshHoverColor();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.pointerOver = true;
    if (!KInputManager.isFocused)
      return;
    KInputManager.SetUserActive();
    if (this.states.Length == 0)
      return;
    if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
      this.toggle_image.color = this.states[this.state].color_on_hover;
    if (this.states[this.state].use_rect_margins)
      this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
    foreach (StatePresentationSetting additionalDisplaySetting in this.states[this.state].additional_display_settings)
    {
      if (!((UnityEngine.Object) additionalDisplaySetting.image_target == (UnityEngine.Object) null) && additionalDisplaySetting.use_color_on_hover)
        additionalDisplaySetting.image_target.color = additionalDisplaySetting.color_on_hover;
    }
    if (this.onEnter == null)
      return;
    this.onEnter();
  }

  protected void RefreshHoverColor()
  {
    if (!this.pointerOver)
      return;
    if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
      this.toggle_image.color = this.states[this.state].color_on_hover;
    foreach (StatePresentationSetting additionalDisplaySetting in this.states[this.state].additional_display_settings)
    {
      if (!((UnityEngine.Object) additionalDisplaySetting.image_target == (UnityEngine.Object) null) && !((UnityEngine.Object) additionalDisplaySetting.image_target == (UnityEngine.Object) null) && additionalDisplaySetting.use_color_on_hover)
        additionalDisplaySetting.image_target.color = additionalDisplaySetting.color_on_hover;
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.pointerOver = false;
    if (!KInputManager.isFocused)
      return;
    KInputManager.SetUserActive();
    if (this.states.Length == 0)
      return;
    if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
      this.toggle_image.color = this.states[this.state].color;
    if (this.states[this.state].use_rect_margins)
      this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
    foreach (StatePresentationSetting additionalDisplaySetting in this.states[this.state].additional_display_settings)
    {
      if (!((UnityEngine.Object) additionalDisplaySetting.image_target == (UnityEngine.Object) null) && additionalDisplaySetting.use_color_on_hover)
        additionalDisplaySetting.image_target.color = additionalDisplaySetting.color;
    }
    if (this.onExit == null)
      return;
    this.onExit();
  }

  public virtual void OnPointerDown(PointerEventData eventData)
  {
    this.clickHeldDown = true;
    if (!this.play_sound_on_click)
      return;
    if (this.states[this.state].on_click_override_sound_path == string.Empty)
      KFMOD.PlayOneShot(GlobalAssets.GetSound("HUD_Click", false));
    else
      KFMOD.PlayOneShot(GlobalAssets.GetSound(this.states[this.state].on_click_override_sound_path, false));
  }

  public virtual void OnPointerUp(PointerEventData eventData)
  {
    if (this.clickHeldDown)
    {
      if (this.play_sound_on_release && this.states[this.state].on_release_override_sound_path != string.Empty)
        KFMOD.PlayOneShot(GlobalAssets.GetSound(this.states[this.state].on_release_override_sound_path, false));
      this.clickHeldDown = false;
      if (this.onStopHold != null)
        this.onStopHold();
    }
    this.totalHeldTime = 0.0f;
  }
}
