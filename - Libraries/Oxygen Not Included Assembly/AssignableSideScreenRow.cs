// Decompiled with JetBrains decompiler
// Type: AssignableSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class AssignableSideScreenRow : KMonoBehaviour
{
  private int refreshHandle = -1;
  [SerializeField]
  private CrewPortrait crewPortraitPrefab;
  [SerializeField]
  private LocText assignmentText;
  public AssignableSideScreen sideScreen;
  private CrewPortrait portraitInstance;
  [MyCmpReq]
  private MultiToggle toggle;
  public IAssignableIdentity targetIdentity;
  public AssignableSideScreenRow.AssignableState currentState;

  public void Refresh(object data = null)
  {
    if (!this.sideScreen.targetAssignable.CanAssignTo(this.targetIdentity))
    {
      this.currentState = AssignableSideScreenRow.AssignableState.Disabled;
      this.assignmentText.text = (string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED;
    }
    else if (this.sideScreen.targetAssignable.assignee == this.targetIdentity)
    {
      this.currentState = AssignableSideScreenRow.AssignableState.Selected;
      this.assignmentText.text = (string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGNED;
    }
    else
    {
      bool flag = false;
      KMonoBehaviour targetIdentity = this.targetIdentity as KMonoBehaviour;
      if ((UnityEngine.Object) targetIdentity != (UnityEngine.Object) null)
      {
        Ownables component1 = targetIdentity.GetComponent<Ownables>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          AssignableSlotInstance slot = component1.GetSlot(this.sideScreen.targetAssignable.slot);
          if (slot != null && slot.IsAssigned())
          {
            this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
            this.assignmentText.text = slot.assignable.GetProperName();
            flag = true;
          }
        }
        Equipment component2 = targetIdentity.GetComponent<Equipment>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          AssignableSlotInstance slot = component2.GetSlot(this.sideScreen.targetAssignable.slot);
          if (slot != null && slot.IsAssigned())
          {
            this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
            this.assignmentText.text = slot.assignable.GetProperName();
            flag = true;
          }
        }
      }
      if (!flag)
      {
        this.currentState = AssignableSideScreenRow.AssignableState.Unassigned;
        this.assignmentText.text = (string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED;
      }
    }
    this.toggle.ChangeState((int) this.currentState);
  }

  protected override void OnCleanUp()
  {
    if (this.refreshHandle == -1)
      Game.Instance.Unsubscribe(this.refreshHandle);
    base.OnCleanUp();
  }

  public void SetContent(
    IAssignableIdentity identity_object,
    System.Action<IAssignableIdentity> selectionCallback,
    AssignableSideScreen assignableSideScreen)
  {
    if (this.refreshHandle == -1)
      Game.Instance.Unsubscribe(this.refreshHandle);
    this.refreshHandle = Game.Instance.Subscribe(-2146166042, (System.Action<object>) (o =>
    {
      if (!((UnityEngine.Object) this != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || !this.gameObject.activeInHierarchy)
        return;
      this.Refresh((object) null);
    }));
    this.toggle = this.GetComponent<MultiToggle>();
    this.sideScreen = assignableSideScreen;
    this.targetIdentity = identity_object;
    if ((UnityEngine.Object) this.portraitInstance == (UnityEngine.Object) null)
    {
      this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, this.gameObject, false);
      this.portraitInstance.transform.SetSiblingIndex(1);
      this.portraitInstance.SetAlpha(1f);
    }
    this.toggle.onClick = (System.Action) (() => selectionCallback(this.targetIdentity));
    this.portraitInstance.SetIdentityObject(identity_object, false);
    this.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.GetTooltip);
    this.Refresh((object) null);
  }

  private string GetTooltip()
  {
    ToolTip component = this.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    if (this.targetIdentity != null && !this.targetIdentity.IsNull())
    {
      switch (this.currentState)
      {
        case AssignableSideScreenRow.AssignableState.Selected:
          component.AddMultiStringTooltip(string.Format((string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGN_TOOLTIP, (object) this.targetIdentity.GetProperName()), (ScriptableObject) null);
          break;
        case AssignableSideScreenRow.AssignableState.Disabled:
          component.AddMultiStringTooltip(string.Format((string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED_TOOLTIP, (object) this.targetIdentity.GetProperName()), (ScriptableObject) null);
          break;
        default:
          component.AddMultiStringTooltip(string.Format((string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGN_TO_TOOLTIP, (object) this.targetIdentity.GetProperName()), (ScriptableObject) null);
          break;
      }
    }
    return string.Empty;
  }

  public enum AssignableState
  {
    Selected,
    AssignedToOther,
    Unassigned,
    Disabled,
  }
}
