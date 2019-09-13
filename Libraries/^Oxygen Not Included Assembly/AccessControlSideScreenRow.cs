// Decompiled with JetBrains decompiler
// Type: AccessControlSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class AccessControlSideScreenRow : AccessControlSideScreenDoor
{
  [SerializeField]
  private CrewPortrait crewPortraitPrefab;
  private CrewPortrait portraitInstance;
  public KToggle defaultButton;
  public GameObject defaultControls;
  public GameObject customControls;
  private System.Action<MinionAssignablesProxy, bool> defaultClickedCallback;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.defaultButton.onValueChanged += new System.Action<bool>(this.OnDefaultButtonChanged);
  }

  private void OnDefaultButtonChanged(bool state)
  {
    this.UpdateButtonStates(!state);
    if (this.defaultClickedCallback == null)
      return;
    this.defaultClickedCallback(this.targetIdentity, !state);
  }

  protected override void UpdateButtonStates(bool isDefault)
  {
    base.UpdateButtonStates(isDefault);
    this.defaultButton.GetComponent<ToolTip>().SetSimpleTooltip((string) (!isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_DEFAULT : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_CUSTOM));
    this.defaultControls.SetActive(isDefault);
    this.customControls.SetActive(!isDefault);
  }

  public void SetMinionContent(
    MinionAssignablesProxy identity,
    AccessControl.Permission permission,
    bool isDefault,
    System.Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange,
    System.Action<MinionAssignablesProxy, bool> onDefaultClick)
  {
    this.SetContent(permission, onPermissionChange);
    if ((UnityEngine.Object) identity == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid data received.");
    }
    else
    {
      if ((UnityEngine.Object) this.portraitInstance == (UnityEngine.Object) null)
      {
        this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, this.defaultButton.gameObject, false);
        this.portraitInstance.SetAlpha(1f);
      }
      this.targetIdentity = identity;
      this.portraitInstance.SetIdentityObject((IAssignableIdentity) identity, false);
      this.portraitInstance.SetSubTitle((string) (!isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_CUSTOM : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_DEFAULT));
      this.defaultClickedCallback = (System.Action<MinionAssignablesProxy, bool>) null;
      this.defaultButton.isOn = !isDefault;
      this.defaultClickedCallback = onDefaultClick;
    }
  }
}
