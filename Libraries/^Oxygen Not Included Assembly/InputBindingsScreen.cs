// Decompiled with JetBrains decompiler
// Type: InputBindingsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBindingsScreen : KModalScreen
{
  private static readonly KeyCode[] validKeys = new KeyCode[111]
  {
    KeyCode.Backspace,
    KeyCode.Tab,
    KeyCode.Clear,
    KeyCode.Return,
    KeyCode.Pause,
    KeyCode.Space,
    KeyCode.Exclaim,
    KeyCode.DoubleQuote,
    KeyCode.Hash,
    KeyCode.Dollar,
    KeyCode.Ampersand,
    KeyCode.Quote,
    KeyCode.LeftParen,
    KeyCode.RightParen,
    KeyCode.Asterisk,
    KeyCode.Plus,
    KeyCode.Comma,
    KeyCode.Minus,
    KeyCode.Period,
    KeyCode.Slash,
    KeyCode.Alpha0,
    KeyCode.Alpha1,
    KeyCode.Alpha2,
    KeyCode.Alpha3,
    KeyCode.Alpha4,
    KeyCode.Alpha5,
    KeyCode.Alpha6,
    KeyCode.Alpha7,
    KeyCode.Alpha8,
    KeyCode.Alpha9,
    KeyCode.Colon,
    KeyCode.Semicolon,
    KeyCode.Less,
    KeyCode.Equals,
    KeyCode.Greater,
    KeyCode.Question,
    KeyCode.At,
    KeyCode.LeftBracket,
    KeyCode.Backslash,
    KeyCode.RightBracket,
    KeyCode.Caret,
    KeyCode.Underscore,
    KeyCode.BackQuote,
    KeyCode.A,
    KeyCode.B,
    KeyCode.C,
    KeyCode.D,
    KeyCode.E,
    KeyCode.F,
    KeyCode.G,
    KeyCode.H,
    KeyCode.I,
    KeyCode.J,
    KeyCode.K,
    KeyCode.L,
    KeyCode.M,
    KeyCode.N,
    KeyCode.O,
    KeyCode.P,
    KeyCode.Q,
    KeyCode.R,
    KeyCode.S,
    KeyCode.T,
    KeyCode.U,
    KeyCode.V,
    KeyCode.W,
    KeyCode.X,
    KeyCode.Y,
    KeyCode.Z,
    KeyCode.Delete,
    KeyCode.Keypad0,
    KeyCode.Keypad1,
    KeyCode.Keypad2,
    KeyCode.Keypad3,
    KeyCode.Keypad4,
    KeyCode.Keypad5,
    KeyCode.Keypad6,
    KeyCode.Keypad7,
    KeyCode.Keypad8,
    KeyCode.Keypad9,
    KeyCode.KeypadPeriod,
    KeyCode.KeypadDivide,
    KeyCode.KeypadMultiply,
    KeyCode.KeypadMinus,
    KeyCode.KeypadPlus,
    KeyCode.KeypadEnter,
    KeyCode.KeypadEquals,
    KeyCode.UpArrow,
    KeyCode.DownArrow,
    KeyCode.RightArrow,
    KeyCode.LeftArrow,
    KeyCode.Insert,
    KeyCode.Home,
    KeyCode.End,
    KeyCode.PageUp,
    KeyCode.PageDown,
    KeyCode.F1,
    KeyCode.F2,
    KeyCode.F3,
    KeyCode.F4,
    KeyCode.F5,
    KeyCode.F6,
    KeyCode.F7,
    KeyCode.F8,
    KeyCode.F9,
    KeyCode.F10,
    KeyCode.F11,
    KeyCode.F12,
    KeyCode.F13,
    KeyCode.F14,
    KeyCode.F15
  };
  private Action actionToRebind = Action.NumActions;
  private int activeScreen = -1;
  private List<string> screens = new List<string>();
  private const string ROOT_KEY = "STRINGS.INPUT_BINDINGS.";
  [SerializeField]
  private OptionsMenuScreen optionsScreen;
  [SerializeField]
  private ConfirmDialogScreen confirmPrefab;
  public KButton backButton;
  public KButton resetButton;
  public KButton closeButton;
  public KButton prevScreenButton;
  public KButton nextScreenButton;
  private bool waitingForKeyPress;
  private bool ignoreRootConflicts;
  private KButton activeButton;
  [SerializeField]
  private LocText screenTitle;
  [SerializeField]
  private GameObject parent;
  [SerializeField]
  private GameObject entryPrefab;
  private ConfirmDialogScreen confirmDialog;
  private UIPool<HorizontalLayoutGroup> entryPool;

  public override bool IsModal()
  {
    return true;
  }

  private bool IsKeyDown(KeyCode key_code)
  {
    if (!Input.GetKey(key_code))
      return Input.GetKeyDown(key_code);
    return true;
  }

  private string GetModifierString(Modifier modifiers)
  {
    string str = string.Empty;
    IEnumerator enumerator = Enum.GetValues(typeof (Modifier)).GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Modifier current = (Modifier) enumerator.Current;
        if ((modifiers & current) != Modifier.None)
          str = str + " + " + current.ToString();
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return str;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.entryPrefab.SetActive(false);
    this.prevScreenButton.onClick += new System.Action(this.OnPrevScreen);
    this.nextScreenButton.onClick += new System.Action(this.OnNextScreen);
  }

  protected override void OnActivate()
  {
    this.CollectScreens();
    this.screenTitle.text = (string) Strings.Get("STRINGS.INPUT_BINDINGS." + this.screens[this.activeScreen].ToUpper() + ".NAME");
    this.closeButton.onClick += new System.Action(this.OnBack);
    this.backButton.onClick += new System.Action(this.OnBack);
    this.resetButton.onClick += new System.Action(this.OnReset);
    this.BuildDisplay();
  }

  private void CollectScreens()
  {
    this.screens.Clear();
    for (int index = 0; index < GameInputMapping.KeyBindings.Length; ++index)
    {
      BindingEntry keyBinding = GameInputMapping.KeyBindings[index];
      if (keyBinding.mGroup != null && keyBinding.mRebindable && !this.screens.Contains(keyBinding.mGroup))
      {
        if (keyBinding.mGroup == "Root")
          this.activeScreen = this.screens.Count;
        this.screens.Add(keyBinding.mGroup);
      }
    }
  }

  protected override void OnDeactivate()
  {
    GameInputMapping.SaveBindings();
    this.DestroyDisplay();
  }

  private LocString GetActionString(Action action)
  {
    return (LocString) null;
  }

  private string GetBindingText(BindingEntry binding)
  {
    string keycodeLocalized = GameUtil.GetKeycodeLocalized(binding.mKeyCode);
    if (binding.mKeyCode != KKeyCode.LeftAlt && binding.mKeyCode != KKeyCode.RightAlt && (binding.mKeyCode != KKeyCode.LeftControl && binding.mKeyCode != KKeyCode.RightControl) && (binding.mKeyCode != KKeyCode.LeftShift && binding.mKeyCode != KKeyCode.RightShift))
      keycodeLocalized += this.GetModifierString(binding.mModifier);
    return keycodeLocalized;
  }

  private void BuildDisplay()
  {
    this.screenTitle.text = (string) Strings.Get("STRINGS.INPUT_BINDINGS." + this.screens[this.activeScreen].ToUpper() + ".NAME");
    if (this.entryPool == null)
      this.entryPool = new UIPool<HorizontalLayoutGroup>(this.entryPrefab.GetComponent<HorizontalLayoutGroup>());
    this.DestroyDisplay();
    int index1 = 0;
    for (int index2 = 0; index2 < GameInputMapping.KeyBindings.Length; ++index2)
    {
      BindingEntry binding = GameInputMapping.KeyBindings[index2];
      if (binding.mGroup == this.screens[this.activeScreen] && binding.mRebindable)
      {
        GameObject gameObject = this.entryPool.GetFreeElement(this.parent, true).gameObject;
        gameObject.transform.GetChild(0).GetComponentInChildren<LocText>().text = (string) Strings.Get("STRINGS.INPUT_BINDINGS." + binding.mGroup.ToUpper() + "." + binding.mAction.ToString().ToUpper());
        LocText key_label = gameObject.transform.GetChild(1).GetComponentInChildren<LocText>();
        key_label.text = this.GetBindingText(binding);
        KButton button = gameObject.GetComponentInChildren<KButton>();
        button.onClick += (System.Action) (() =>
        {
          this.waitingForKeyPress = true;
          this.actionToRebind = binding.mAction;
          this.ignoreRootConflicts = binding.mIgnoreRootConflics;
          this.activeButton = button;
          key_label.text = (string) STRINGS.UI.FRONTEND.INPUT_BINDINGS_SCREEN.WAITING_FOR_INPUT;
        });
        gameObject.transform.SetSiblingIndex(index1);
        ++index1;
      }
    }
  }

  private void DestroyDisplay()
  {
    this.entryPool.ClearAll();
  }

  private void Update()
  {
    if (!this.waitingForKeyPress)
      return;
    Modifier modifier = (Modifier) (0 | (this.IsKeyDown(KeyCode.LeftAlt) || this.IsKeyDown(KeyCode.RightAlt) ? 1 : 0) | (this.IsKeyDown(KeyCode.LeftControl) || this.IsKeyDown(KeyCode.RightControl) ? 2 : 0) | (this.IsKeyDown(KeyCode.LeftShift) || this.IsKeyDown(KeyCode.RightShift) ? 4 : 0) | (!this.IsKeyDown(KeyCode.CapsLock) ? 0 : 8));
    bool flag = false;
    for (int index = 0; index < InputBindingsScreen.validKeys.Length; ++index)
    {
      KeyCode validKey = InputBindingsScreen.validKeys[index];
      if (Input.GetKeyDown(validKey))
      {
        this.Bind((KKeyCode) validKey, modifier);
        flag = true;
      }
    }
    if (flag)
      return;
    float axis = Input.GetAxis("Mouse ScrollWheel");
    KKeyCode kkey_code = KKeyCode.None;
    if ((double) axis < 0.0)
      kkey_code = KKeyCode.MouseScrollDown;
    else if ((double) axis > 0.0)
      kkey_code = KKeyCode.MouseScrollUp;
    if (kkey_code == KKeyCode.None)
      return;
    this.Bind(kkey_code, modifier);
  }

  private BindingEntry GetDuplicatedBinding(
    string activeScreen,
    BindingEntry new_binding)
  {
    BindingEntry bindingEntry = new BindingEntry();
    for (int index = 0; index < GameInputMapping.KeyBindings.Length; ++index)
    {
      BindingEntry keyBinding = GameInputMapping.KeyBindings[index];
      if (new_binding.IsBindingEqual(keyBinding) && (keyBinding.mGroup == null || keyBinding.mGroup == activeScreen || (keyBinding.mGroup == "Root" || activeScreen == "Root")) && ((!(activeScreen == "Root") || !keyBinding.mIgnoreRootConflics) && (!(keyBinding.mGroup == "Root") || !new_binding.mIgnoreRootConflics)))
      {
        bindingEntry = keyBinding;
        break;
      }
    }
    return bindingEntry;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.waitingForKeyPress)
      e.Consumed = true;
    else if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    e.Consumed = true;
  }

  private void OnBack()
  {
    string text;
    switch (this.NumUnboundActions())
    {
      case 0:
        this.Deactivate();
        return;
      case 1:
        BindingEntry firstUnbound = this.GetFirstUnbound();
        text = string.Format((string) STRINGS.UI.FRONTEND.INPUT_BINDINGS_SCREEN.UNBOUND_ACTION, (object) firstUnbound.mAction.ToString());
        break;
      default:
        text = (string) STRINGS.UI.FRONTEND.INPUT_BINDINGS_SCREEN.MULTIPLE_UNBOUND_ACTIONS;
        break;
    }
    this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, this.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
    this.confirmDialog.PopupConfirmDialog(text, (System.Action) (() => this.Deactivate()), (System.Action) (() => this.confirmDialog.Deactivate()), (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    this.confirmDialog.gameObject.SetActive(true);
  }

  private int NumUnboundActions()
  {
    int num = 0;
    for (int index = 0; index < GameInputMapping.KeyBindings.Length; ++index)
    {
      BindingEntry keyBinding = GameInputMapping.KeyBindings[index];
      if (keyBinding.mKeyCode == KKeyCode.None && (BuildMenu.UseHotkeyBuildMenu() || !keyBinding.mIgnoreRootConflics))
        ++num;
    }
    return num;
  }

  private BindingEntry GetFirstUnbound()
  {
    BindingEntry bindingEntry = new BindingEntry();
    for (int index = 0; index < GameInputMapping.KeyBindings.Length; ++index)
    {
      BindingEntry keyBinding = GameInputMapping.KeyBindings[index];
      if (keyBinding.mKeyCode == KKeyCode.None)
      {
        bindingEntry = keyBinding;
        break;
      }
    }
    return bindingEntry;
  }

  private void OnReset()
  {
    GameInputMapping.KeyBindings = (BindingEntry[]) GameInputMapping.DefaultBindings.Clone();
    Global.Instance.GetInputManager().RebindControls();
    this.BuildDisplay();
  }

  public void OnPrevScreen()
  {
    if (this.activeScreen > 0)
      --this.activeScreen;
    else
      this.activeScreen = this.screens.Count - 1;
    this.BuildDisplay();
  }

  public void OnNextScreen()
  {
    if (this.activeScreen < this.screens.Count - 1)
      ++this.activeScreen;
    else
      this.activeScreen = 0;
    this.BuildDisplay();
  }

  private void Bind(KKeyCode kkey_code, Modifier modifier)
  {
    BindingEntry bindingEntry = new BindingEntry(this.screens[this.activeScreen], GamepadButton.NumButtons, kkey_code, modifier, this.actionToRebind, true, this.ignoreRootConflicts);
    for (int index = 0; index < GameInputMapping.KeyBindings.Length; ++index)
    {
      BindingEntry keyBinding = GameInputMapping.KeyBindings[index];
      if (keyBinding.mRebindable && keyBinding.mAction == this.actionToRebind)
      {
        BindingEntry duplicatedBinding = this.GetDuplicatedBinding(this.screens[this.activeScreen], bindingEntry);
        GameInputMapping.KeyBindings[index] = bindingEntry;
        this.activeButton.GetComponentInChildren<LocText>().text = this.GetBindingText(bindingEntry);
        if (duplicatedBinding.mAction != Action.Invalid && duplicatedBinding.mAction != this.actionToRebind)
        {
          this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, this.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
          string str = (string) Strings.Get("STRINGS.INPUT_BINDINGS." + duplicatedBinding.mGroup.ToUpper() + "." + duplicatedBinding.mAction.ToString().ToUpper());
          string bindingText = this.GetBindingText(duplicatedBinding);
          string text = string.Format((string) STRINGS.UI.FRONTEND.INPUT_BINDINGS_SCREEN.DUPLICATE, (object) str, (object) bindingText);
          this.Unbind(duplicatedBinding.mAction);
          this.confirmDialog.PopupConfirmDialog(text, (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
          this.confirmDialog.gameObject.SetActive(true);
        }
        Global.Instance.GetInputManager().RebindControls();
        this.waitingForKeyPress = false;
        this.actionToRebind = Action.NumActions;
        this.activeButton = (KButton) null;
        this.BuildDisplay();
        break;
      }
    }
  }

  private void Unbind(Action action)
  {
    for (int index = 0; index < GameInputMapping.KeyBindings.Length; ++index)
    {
      BindingEntry keyBinding = GameInputMapping.KeyBindings[index];
      if (keyBinding.mAction == action)
      {
        keyBinding.mKeyCode = KKeyCode.None;
        keyBinding.mModifier = Modifier.None;
        GameInputMapping.KeyBindings[index] = keyBinding;
      }
    }
  }
}
