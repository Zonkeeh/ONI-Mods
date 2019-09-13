// Decompiled with JetBrains decompiler
// Type: AssignableSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AssignableSideScreen : SideScreenContent
{
  private int targetAssignableSubscriptionHandle = -1;
  private Dictionary<IAssignableIdentity, AssignableSideScreenRow> identityRowMap = new Dictionary<IAssignableIdentity, AssignableSideScreenRow>();
  private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();
  [SerializeField]
  private AssignableSideScreenRow rowPrefab;
  [SerializeField]
  private GameObject rowGroup;
  [SerializeField]
  private LocText currentOwnerText;
  [SerializeField]
  private MultiToggle dupeSortingToggle;
  [SerializeField]
  private MultiToggle generalSortingToggle;
  private MultiToggle activeSortToggle;
  private Comparison<IAssignableIdentity> activeSortFunction;
  private bool sortReversed;
  private UIPool<AssignableSideScreenRow> rowPool;

  public Assignable targetAssignable { get; private set; }

  public override string GetTitle()
  {
    if ((UnityEngine.Object) this.targetAssignable != (UnityEngine.Object) null)
      return string.Format(base.GetTitle(), (object) this.targetAssignable.GetProperName());
    return base.GetTitle();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.dupeSortingToggle.onClick += (System.Action) (() => this.SortByName(true));
    this.generalSortingToggle.onClick += (System.Action) (() => this.SortByAssignment(true));
    this.Subscribe(Game.Instance.gameObject, 875045922, new System.Action<object>(this.OnRefreshData));
  }

  private void OnRefreshData(object obj)
  {
    this.SetTarget(this.targetAssignable.gameObject);
  }

  public override void ClearTarget()
  {
    if (this.targetAssignableSubscriptionHandle != -1 && (UnityEngine.Object) this.targetAssignable != (UnityEngine.Object) null)
    {
      this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
      this.targetAssignableSubscriptionHandle = -1;
    }
    this.targetAssignable = (Assignable) null;
    Components.LiveMinionIdentities.OnAdd -= new System.Action<MinionIdentity>(this.OnMinionIdentitiesChanged);
    Components.LiveMinionIdentities.OnRemove -= new System.Action<MinionIdentity>(this.OnMinionIdentitiesChanged);
    base.ClearTarget();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<Assignable>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    Components.LiveMinionIdentities.OnAdd += new System.Action<MinionIdentity>(this.OnMinionIdentitiesChanged);
    Components.LiveMinionIdentities.OnRemove += new System.Action<MinionIdentity>(this.OnMinionIdentitiesChanged);
    if (this.targetAssignableSubscriptionHandle != -1 && (UnityEngine.Object) this.targetAssignable != (UnityEngine.Object) null)
      this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
    this.targetAssignable = target.GetComponent<Assignable>();
    if ((UnityEngine.Object) this.targetAssignable == (UnityEngine.Object) null)
    {
      Debug.LogError((object) string.Format("{0} selected has no Assignable component.", (object) target.GetProperName()));
    }
    else
    {
      if (this.rowPool == null)
        this.rowPool = new UIPool<AssignableSideScreenRow>(this.rowPrefab);
      this.gameObject.SetActive(true);
      this.identityList = new List<MinionAssignablesProxy>((IEnumerable<MinionAssignablesProxy>) Components.MinionAssignablesProxy.Items);
      this.dupeSortingToggle.ChangeState(0);
      this.generalSortingToggle.ChangeState(0);
      this.activeSortToggle = (MultiToggle) null;
      this.activeSortFunction = (Comparison<IAssignableIdentity>) null;
      if (!this.targetAssignable.CanBeAssigned)
        this.HideScreen(true);
      else
        this.HideScreen(false);
      this.targetAssignableSubscriptionHandle = this.targetAssignable.Subscribe(684616645, new System.Action<object>(this.OnAssigneeChanged));
      this.Refresh(this.identityList);
      this.SortByAssignment(false);
    }
  }

  private void OnMinionIdentitiesChanged(MinionIdentity change)
  {
    this.identityList = new List<MinionAssignablesProxy>((IEnumerable<MinionAssignablesProxy>) Components.MinionAssignablesProxy.Items);
    this.Refresh(this.identityList);
  }

  private void OnAssigneeChanged(object data = null)
  {
    foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> identityRow in this.identityRowMap)
      identityRow.Value.Refresh((object) null);
  }

  private void Refresh(List<MinionAssignablesProxy> identities)
  {
    this.ClearContent();
    this.currentOwnerText.text = string.Format((string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED);
    if ((UnityEngine.Object) this.targetAssignable == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.targetAssignable.GetComponent<Equippable>() == (UnityEngine.Object) null && !this.targetAssignable.HasTag(GameTags.NotRoomAssignable))
    {
      Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.targetAssignable.gameObject);
      if (roomOfGameObject != null)
      {
        RoomType roomType = roomOfGameObject.roomType;
        if (roomType.primary_constraint != null && !roomType.primary_constraint.building_criteria(this.targetAssignable.GetComponent<KPrefabID>()))
        {
          AssignableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
          freeElement.sideScreen = this;
          this.identityRowMap.Add((IAssignableIdentity) roomOfGameObject, freeElement);
          freeElement.SetContent((IAssignableIdentity) roomOfGameObject, new System.Action<IAssignableIdentity>(this.OnRowClicked), this);
          return;
        }
      }
    }
    if (this.targetAssignable.canBePublic)
    {
      AssignableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
      freeElement.sideScreen = this;
      freeElement.transform.SetAsFirstSibling();
      this.identityRowMap.Add((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"], freeElement);
      freeElement.SetContent((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"], new System.Action<IAssignableIdentity>(this.OnRowClicked), this);
    }
    foreach (MinionAssignablesProxy identity in identities)
    {
      AssignableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
      freeElement.sideScreen = this;
      this.identityRowMap.Add((IAssignableIdentity) identity, freeElement);
      freeElement.SetContent((IAssignableIdentity) identity, new System.Action<IAssignableIdentity>(this.OnRowClicked), this);
    }
    this.ExecuteSort(this.activeSortFunction);
  }

  private void SortByName(bool reselect)
  {
    this.SelectSortToggle(this.dupeSortingToggle, reselect);
    this.ExecuteSort((Comparison<IAssignableIdentity>) ((i1, i2) => i1.GetProperName().CompareTo(i2.GetProperName()) * (!this.sortReversed ? 1 : -1)));
  }

  private void SortByAssignment(bool reselect)
  {
    this.SelectSortToggle(this.generalSortingToggle, reselect);
    this.ExecuteSort((Comparison<IAssignableIdentity>) ((i1, i2) =>
    {
      int num1 = this.targetAssignable.CanAssignTo(i1).CompareTo(this.targetAssignable.CanAssignTo(i2));
      if (num1 != 0)
        return num1 * -1;
      int num2 = this.identityRowMap[i1].currentState.CompareTo((object) this.identityRowMap[i2].currentState);
      if (num2 != 0)
        return num2 * (!this.sortReversed ? 1 : -1);
      return i1.GetProperName().CompareTo(i2.GetProperName());
    }));
  }

  private void SelectSortToggle(MultiToggle toggle, bool reselect)
  {
    this.dupeSortingToggle.ChangeState(0);
    this.generalSortingToggle.ChangeState(0);
    if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
    {
      if (reselect && (UnityEngine.Object) this.activeSortToggle == (UnityEngine.Object) toggle)
        this.sortReversed = !this.sortReversed;
      this.activeSortToggle = toggle;
    }
    this.activeSortToggle.ChangeState(!this.sortReversed ? 1 : 2);
  }

  private void ExecuteSort(Comparison<IAssignableIdentity> sortFunction)
  {
    if (sortFunction == null)
      return;
    List<IAssignableIdentity> assignableIdentityList = new List<IAssignableIdentity>((IEnumerable<IAssignableIdentity>) this.identityRowMap.Keys);
    assignableIdentityList.Sort(sortFunction);
    for (int index = 0; index < assignableIdentityList.Count; ++index)
      this.identityRowMap[assignableIdentityList[index]].transform.SetSiblingIndex(index);
    this.activeSortFunction = sortFunction;
  }

  private void ClearContent()
  {
    if (this.rowPool != null)
      this.rowPool.DestroyAll();
    foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> identityRow in this.identityRowMap)
      identityRow.Value.targetIdentity = (IAssignableIdentity) null;
    this.identityRowMap.Clear();
  }

  private void HideScreen(bool hide)
  {
    if (hide)
    {
      this.transform.localScale = Vector3.zero;
    }
    else
    {
      if (!(this.transform.localScale != Vector3.one))
        return;
      this.transform.localScale = Vector3.one;
    }
  }

  private void OnRowClicked(IAssignableIdentity identity)
  {
    if (this.targetAssignable.assignee != identity)
    {
      this.ChangeAssignment(identity);
    }
    else
    {
      if (!this.CanDeselect(identity))
        return;
      this.ChangeAssignment((IAssignableIdentity) null);
    }
  }

  private bool CanDeselect(IAssignableIdentity identity)
  {
    return identity is MinionAssignablesProxy;
  }

  private void ChangeAssignment(IAssignableIdentity new_identity)
  {
    this.targetAssignable.Unassign();
    if (new_identity == null)
      return;
    this.targetAssignable.Assign(new_identity);
  }

  private void OnValidStateChanged(bool state)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.Refresh(this.identityList);
  }
}
