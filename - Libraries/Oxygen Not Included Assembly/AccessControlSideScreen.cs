// Decompiled with JetBrains decompiler
// Type: AccessControlSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AccessControlSideScreen : SideScreenContent
{
  private AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo = AccessControlSideScreen.MinionIdentitySort.SortInfos[0];
  private Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow> identityRowMap = new Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow>();
  private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();
  [SerializeField]
  private AccessControlSideScreenRow rowPrefab;
  [SerializeField]
  private GameObject rowGroup;
  [SerializeField]
  private AccessControlSideScreenDoor defaultsRow;
  [SerializeField]
  private Toggle sortByNameToggle;
  [SerializeField]
  private Toggle sortByPermissionToggle;
  [SerializeField]
  private Toggle sortByRoleToggle;
  [SerializeField]
  private GameObject disabledOverlay;
  [SerializeField]
  private KImage headerBG;
  private AccessControl target;
  private Door doorTarget;
  private UIPool<AccessControlSideScreenRow> rowPool;

  public override string GetTitle()
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      return string.Format(base.GetTitle(), (object) this.target.GetProperName());
    return base.GetTitle();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.sortByNameToggle.onValueChanged.AddListener((UnityAction<bool>) (reverse_sort =>
    {
      int num = reverse_sort ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      if (AccessControlSideScreen.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AccessControlSideScreen.\u003C\u003Ef__mg\u0024cache0 = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<MinionAssignablesProxy> fMgCache0 = AccessControlSideScreen.\u003C\u003Ef__mg\u0024cache0;
      this.SortEntries(num != 0, fMgCache0);
    }));
    this.sortByRoleToggle.onValueChanged.AddListener((UnityAction<bool>) (reverse_sort =>
    {
      int num = reverse_sort ? 1 : 0;
      // ISSUE: reference to a compiler-generated field
      if (AccessControlSideScreen.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AccessControlSideScreen.\u003C\u003Ef__mg\u0024cache1 = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<MinionAssignablesProxy> fMgCache1 = AccessControlSideScreen.\u003C\u003Ef__mg\u0024cache1;
      this.SortEntries(num != 0, fMgCache1);
    }));
    this.sortByPermissionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SortByPermission));
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<AccessControl>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      this.ClearTarget();
    this.target = target.GetComponent<AccessControl>();
    this.doorTarget = target.GetComponent<Door>();
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    target.Subscribe(1734268753, new System.Action<object>(this.OnDoorStateChanged));
    target.Subscribe(-1525636549, new System.Action<object>(this.OnAccessControlChanged));
    if (this.rowPool == null)
      this.rowPool = new UIPool<AccessControlSideScreenRow>(this.rowPrefab);
    this.gameObject.SetActive(true);
    this.identityList = new List<MinionAssignablesProxy>((IEnumerable<MinionAssignablesProxy>) Components.MinionAssignablesProxy.Items);
    this.Refresh(this.identityList, true);
  }

  public override void ClearTarget()
  {
    base.ClearTarget();
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null))
      return;
    this.target.Unsubscribe(1734268753, new System.Action<object>(this.OnDoorStateChanged));
    this.target.Unsubscribe(-1525636549, new System.Action<object>(this.OnAccessControlChanged));
  }

  private void Refresh(List<MinionAssignablesProxy> identities, bool rebuild)
  {
    Rotatable component = this.target.GetComponent<Rotatable>();
    bool rotated = (UnityEngine.Object) component != (UnityEngine.Object) null && component.IsRotated;
    this.defaultsRow.SetRotated(rotated);
    this.defaultsRow.SetContent(this.target.DefaultPermission, new System.Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnDefaultPermissionChanged));
    if (rebuild)
      this.ClearContent();
    foreach (MinionAssignablesProxy identity in identities)
    {
      AccessControlSideScreenRow controlSideScreenRow;
      if (rebuild)
      {
        controlSideScreenRow = this.rowPool.GetFreeElement(this.rowGroup, true);
        this.identityRowMap.Add(identity, controlSideScreenRow);
      }
      else
        controlSideScreenRow = this.identityRowMap[identity];
      AccessControl.Permission setPermission = this.target.GetSetPermission(identity);
      bool isDefault = this.target.IsDefaultPermission(identity);
      controlSideScreenRow.SetRotated(rotated);
      controlSideScreenRow.SetMinionContent(identity, setPermission, isDefault, new System.Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnPermissionChanged), new System.Action<MinionAssignablesProxy, bool>(this.OnPermissionDefault));
    }
    this.RefreshOnline();
    this.ContentContainer.SetActive(this.target.controlEnabled);
  }

  private void RefreshOnline()
  {
    bool flag = this.target.Online && ((UnityEngine.Object) this.doorTarget == (UnityEngine.Object) null || this.doorTarget.CurrentState == Door.ControlState.Auto);
    this.disabledOverlay.SetActive(!flag);
    this.headerBG.ColorState = !flag ? KImage.ColorSelector.Inactive : KImage.ColorSelector.Active;
  }

  private void SortByPermission(bool state)
  {
    this.ExecuteSort<int>(this.sortByPermissionToggle, state, (Func<MinionAssignablesProxy, int>) (identity =>
    {
      if (this.target.IsDefaultPermission(identity))
        return -1;
      return (int) this.target.GetSetPermission(identity);
    }), false);
  }

  private void ExecuteSort<T>(
    Toggle toggle,
    bool state,
    Func<MinionAssignablesProxy, T> sortFunction,
    bool refresh = false)
  {
    toggle.GetComponent<ImageToggleState>().SetActiveState(state);
    if (!state)
      return;
    this.identityList = !state ? this.identityList.OrderByDescending<MinionAssignablesProxy, T>(sortFunction).ToList<MinionAssignablesProxy>() : this.identityList.OrderBy<MinionAssignablesProxy, T>(sortFunction).ToList<MinionAssignablesProxy>();
    if (refresh)
    {
      this.Refresh(this.identityList, false);
    }
    else
    {
      for (int index = 0; index < this.identityList.Count; ++index)
      {
        if (this.identityRowMap.ContainsKey(this.identityList[index]))
          this.identityRowMap[this.identityList[index]].transform.SetSiblingIndex(index);
      }
    }
  }

  private void SortEntries(bool reverse_sort, Comparison<MinionAssignablesProxy> compare)
  {
    this.identityList.Sort(compare);
    if (reverse_sort)
      this.identityList.Reverse();
    for (int index = 0; index < this.identityList.Count; ++index)
    {
      if (this.identityRowMap.ContainsKey(this.identityList[index]))
        this.identityRowMap[this.identityList[index]].transform.SetSiblingIndex(index);
    }
  }

  private void ClearContent()
  {
    if (this.rowPool != null)
      this.rowPool.ClearAll();
    this.identityRowMap.Clear();
  }

  private void OnDefaultPermissionChanged(
    MinionAssignablesProxy identity,
    AccessControl.Permission permission)
  {
    this.target.DefaultPermission = permission;
    this.Refresh(this.identityList, false);
    foreach (MinionAssignablesProxy identity1 in this.identityList)
    {
      if (this.target.IsDefaultPermission(identity1))
        this.target.ClearPermission(identity1);
    }
  }

  private void OnPermissionChanged(
    MinionAssignablesProxy identity,
    AccessControl.Permission permission)
  {
    this.target.SetPermission(identity, permission);
  }

  private void OnPermissionDefault(MinionAssignablesProxy identity, bool isDefault)
  {
    if (isDefault)
      this.target.ClearPermission(identity);
    else
      this.target.SetPermission(identity, this.target.DefaultPermission);
    this.Refresh(this.identityList, false);
  }

  private void OnAccessControlChanged(object data)
  {
    this.RefreshOnline();
  }

  private void OnDoorStateChanged(object data)
  {
    this.RefreshOnline();
  }

  private void OnSelectSortFunc(IListableOption role, object data)
  {
    if (role == null)
      return;
    foreach (AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo in AccessControlSideScreen.MinionIdentitySort.SortInfos)
    {
      if ((string) sortInfo.name == role.GetProperName())
      {
        this.sortInfo = sortInfo;
        this.identityList.Sort(this.sortInfo.compare);
        for (int index = 0; index < this.identityList.Count; ++index)
        {
          if (this.identityRowMap.ContainsKey(this.identityList[index]))
            this.identityRowMap[this.identityList[index]].transform.SetSiblingIndex(index);
        }
        break;
      }
    }
  }

  private static class MinionIdentitySort
  {
    public static readonly AccessControlSideScreen.MinionIdentitySort.SortInfo[] SortInfos;

    public static int CompareByName(MinionAssignablesProxy a, MinionAssignablesProxy b)
    {
      return a.GetProperName().CompareTo(b.GetProperName());
    }

    public static int CompareByRole(MinionAssignablesProxy a, MinionAssignablesProxy b)
    {
      Debug.Assert((bool) ((UnityEngine.Object) a), (object) "a was null");
      Debug.Assert((bool) ((UnityEngine.Object) b), (object) "b was null");
      GameObject targetGameObject1 = a.GetTargetGameObject();
      GameObject targetGameObject2 = b.GetTargetGameObject();
      MinionResume minionResume1 = !(bool) ((UnityEngine.Object) targetGameObject1) ? (MinionResume) null : targetGameObject1.GetComponent<MinionResume>();
      MinionResume minionResume2 = !(bool) ((UnityEngine.Object) targetGameObject2) ? (MinionResume) null : targetGameObject2.GetComponent<MinionResume>();
      if ((UnityEngine.Object) minionResume2 == (UnityEngine.Object) null)
        return 1;
      if ((UnityEngine.Object) minionResume1 == (UnityEngine.Object) null)
        return -1;
      int num = minionResume1.CurrentRole.CompareTo(minionResume2.CurrentRole);
      if (num == 0)
        return AccessControlSideScreen.MinionIdentitySort.CompareByName(a, b);
      return num;
    }

    static MinionIdentitySort()
    {
      AccessControlSideScreen.MinionIdentitySort.SortInfo[] sortInfoArray = new AccessControlSideScreen.MinionIdentitySort.SortInfo[2];
      AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo1 = new AccessControlSideScreen.MinionIdentitySort.SortInfo();
      sortInfo1.name = STRINGS.UI.MINION_IDENTITY_SORT.NAME;
      AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo2 = sortInfo1;
      // ISSUE: reference to a compiler-generated field
      if (AccessControlSideScreen.MinionIdentitySort.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AccessControlSideScreen.MinionIdentitySort.\u003C\u003Ef__mg\u0024cache0 = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<MinionAssignablesProxy> fMgCache0 = AccessControlSideScreen.MinionIdentitySort.\u003C\u003Ef__mg\u0024cache0;
      sortInfo2.compare = fMgCache0;
      sortInfoArray[0] = sortInfo1;
      AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo3 = new AccessControlSideScreen.MinionIdentitySort.SortInfo();
      sortInfo3.name = STRINGS.UI.MINION_IDENTITY_SORT.ROLE;
      AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo4 = sortInfo3;
      // ISSUE: reference to a compiler-generated field
      if (AccessControlSideScreen.MinionIdentitySort.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AccessControlSideScreen.MinionIdentitySort.\u003C\u003Ef__mg\u0024cache1 = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<MinionAssignablesProxy> fMgCache1 = AccessControlSideScreen.MinionIdentitySort.\u003C\u003Ef__mg\u0024cache1;
      sortInfo4.compare = fMgCache1;
      sortInfoArray[1] = sortInfo3;
      AccessControlSideScreen.MinionIdentitySort.SortInfos = sortInfoArray;
    }

    public class SortInfo : IListableOption
    {
      public LocString name;
      public Comparison<MinionAssignablesProxy> compare;

      public string GetProperName()
      {
        return (string) this.name;
      }
    }
  }
}
