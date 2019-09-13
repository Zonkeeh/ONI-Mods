// Decompiled with JetBrains decompiler
// Type: EditableTitleBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EditableTitleBar : TitleBar
{
  public KButton editNameButton;
  public KButton randomNameButton;
  public TMP_InputField inputField;
  private Coroutine postEndEdit;
  private Coroutine preToggleNameEditing;

  public event System.Action<string> OnNameChanged;

  public event System.Action OnStartedEditing;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.randomNameButton != (UnityEngine.Object) null)
      this.randomNameButton.onClick += new System.Action(this.GenerateRandomName);
    if ((UnityEngine.Object) this.editNameButton != (UnityEngine.Object) null)
      this.EnableEditButtonClick();
    if (!((UnityEngine.Object) this.inputField != (UnityEngine.Object) null))
      return;
    this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
  }

  private void OnEndEdit(string finalStr)
  {
    finalStr = Localization.FilterDirtyWords(finalStr);
    this.SetEditingState(false);
    if (string.IsNullOrEmpty(finalStr))
      return;
    if (this.OnNameChanged != null)
      this.OnNameChanged(finalStr);
    this.titleText.text = finalStr;
    if (this.postEndEdit != null)
      this.StopCoroutine(this.postEndEdit);
    if (!this.gameObject.activeInHierarchy || !this.enabled)
      return;
    this.postEndEdit = this.StartCoroutine(this.PostOnEndEditRoutine());
  }

  [DebuggerHidden]
  private IEnumerator PostOnEndEditRoutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EditableTitleBar.\u003CPostOnEndEditRoutine\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator PreToggleNameEditingRoutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EditableTitleBar.\u003CPreToggleNameEditingRoutine\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  private void EnableEditButtonClick()
  {
    this.editNameButton.onClick += (System.Action) (() =>
    {
      if (this.preToggleNameEditing != null)
        return;
      this.preToggleNameEditing = this.StartCoroutine(this.PreToggleNameEditingRoutine());
    });
  }

  private void GenerateRandomName()
  {
    if (this.postEndEdit != null)
      this.StopCoroutine(this.postEndEdit);
    string randomDuplicantName = GameUtil.GenerateRandomDuplicantName();
    if (this.OnNameChanged != null)
      this.OnNameChanged(randomDuplicantName);
    this.titleText.text = randomDuplicantName;
    this.SetEditingState(true);
  }

  private void ToggleNameEditing()
  {
    this.editNameButton.ClearOnClick();
    bool state = !this.inputField.gameObject.activeInHierarchy;
    if ((UnityEngine.Object) this.randomNameButton != (UnityEngine.Object) null)
      this.randomNameButton.gameObject.SetActive(state);
    this.SetEditingState(state);
  }

  private void SetEditingState(bool state)
  {
    this.titleText.gameObject.SetActive(!state);
    if (this.setCameraControllerState)
      CameraController.Instance.DisableUserCameraControl = state;
    if ((UnityEngine.Object) this.inputField == (UnityEngine.Object) null)
      return;
    this.inputField.gameObject.SetActive(state);
    if (state)
    {
      this.inputField.text = this.titleText.text;
      this.inputField.Select();
      this.inputField.ActivateInputField();
      if (this.OnStartedEditing == null)
        return;
      this.OnStartedEditing();
    }
    else
      this.inputField.DeactivateInputField();
  }

  public void ForceStopEditing()
  {
    if (this.postEndEdit != null)
      this.StopCoroutine(this.postEndEdit);
    this.editNameButton.ClearOnClick();
    this.SetEditingState(false);
    this.EnableEditButtonClick();
  }

  public void SetUserEditable(bool editable)
  {
    this.userEditable = editable;
    this.editNameButton.gameObject.SetActive(editable);
    this.editNameButton.ClearOnClick();
    this.EnableEditButtonClick();
  }
}
