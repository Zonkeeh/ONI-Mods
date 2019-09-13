// Decompiled with JetBrains decompiler
// Type: TreeFilterableSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeFilterableSideScreen : SideScreenContent
{
  private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();
  [SerializeField]
  private MultiToggle allCheckBox;
  [SerializeField]
  private KToggle onlyAllowTransportItemsCheckBox;
  [SerializeField]
  private GameObject onlyallowTransportItemsRow;
  [SerializeField]
  private TreeFilterableSideScreenRow rowPrefab;
  [SerializeField]
  private GameObject rowGroup;
  [SerializeField]
  private TreeFilterableSideScreenElement elementPrefab;
  private GameObject target;
  private bool visualDirty;
  private KImage onlyAllowTransportItemsImg;
  public UIPool<TreeFilterableSideScreenElement> elementPool;
  private UIPool<TreeFilterableSideScreenRow> rowPool;
  private TreeFilterable targetFilterable;
  private Storage storage;

  public bool IsStorage
  {
    get
    {
      return (UnityEngine.Object) this.storage != (UnityEngine.Object) null;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.rowPool = new UIPool<TreeFilterableSideScreenRow>(this.rowPrefab);
    this.elementPool = new UIPool<TreeFilterableSideScreenElement>(this.elementPrefab);
    this.allCheckBox.onClick += (System.Action) (() =>
    {
      switch (this.GetAllCheckboxState())
      {
        case TreeFilterableSideScreenRow.State.Off:
        case TreeFilterableSideScreenRow.State.Mixed:
          this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
          break;
        case TreeFilterableSideScreenRow.State.On:
          this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
          break;
      }
    });
    this.onlyAllowTransportItemsImg = this.onlyAllowTransportItemsCheckBox.gameObject.GetComponentInChildrenOnly<KImage>();
    this.onlyAllowTransportItemsCheckBox.onClick += new System.Action(this.OnlyAllowTransportItemsClicked);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
    this.onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
  }

  private void UpdateAllCheckBoxVisualState()
  {
    switch (this.GetAllCheckboxState())
    {
      case TreeFilterableSideScreenRow.State.Off:
        this.allCheckBox.ChangeState(0);
        break;
      case TreeFilterableSideScreenRow.State.Mixed:
        this.allCheckBox.ChangeState(1);
        break;
      case TreeFilterableSideScreenRow.State.On:
        this.allCheckBox.ChangeState(2);
        break;
    }
    this.visualDirty = false;
  }

  public void Update()
  {
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      if (tagRow.Value.visualDirty)
      {
        tagRow.Value.UpdateCheckBoxVisualState();
        this.visualDirty = true;
      }
    }
    if (!this.visualDirty)
      return;
    this.UpdateAllCheckBoxVisualState();
  }

  private void OnlyAllowTransportItemsClicked()
  {
    this.storage.SetOnlyFetchMarkedItems(!this.storage.GetOnlyFetchMarkedItems());
  }

  private TreeFilterableSideScreenRow.State GetAllCheckboxState()
  {
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      switch (tagRow.Value.GetState())
      {
        case TreeFilterableSideScreenRow.State.Off:
          flag2 = true;
          continue;
        case TreeFilterableSideScreenRow.State.Mixed:
          flag3 = true;
          continue;
        case TreeFilterableSideScreenRow.State.On:
          flag1 = true;
          continue;
        default:
          continue;
      }
    }
    if (flag3)
      return TreeFilterableSideScreenRow.State.Mixed;
    if (flag1 && !flag2)
      return TreeFilterableSideScreenRow.State.On;
    return !flag1 && flag2 || (!flag1 || !flag2) ? TreeFilterableSideScreenRow.State.Off : TreeFilterableSideScreenRow.State.Mixed;
  }

  private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
  {
    switch (newState)
    {
      case TreeFilterableSideScreenRow.State.Off:
        using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
          break;
        }
      case TreeFilterableSideScreenRow.State.On:
        using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
          break;
        }
    }
    this.visualDirty = true;
  }

  public bool GetElementTagAcceptedState(Tag t)
  {
    return this.targetFilterable.ContainsTag(t);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<TreeFilterable>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    this.target = target;
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The target object provided was null");
    }
    else
    {
      this.targetFilterable = target.GetComponent<TreeFilterable>();
      if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
        Debug.LogError((object) "The target provided does not have a Tree Filterable component");
      else if (!this.targetFilterable.showUserMenu)
        DetailsScreen.Instance.DeactivateSideContent();
      else if (this.IsStorage && !this.storage.showInUI)
      {
        DetailsScreen.Instance.DeactivateSideContent();
      }
      else
      {
        this.storage = this.targetFilterable.GetComponent<Storage>();
        this.storage.Subscribe(644822890, new System.Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
        this.OnOnlyFetchMarkedItemsSettingChanged((object) null);
        this.CreateCategories();
      }
    }
  }

  private void OnOnlyFetchMarkedItemsSettingChanged(object data)
  {
    if (this.storage.allowSettingOnlyFetchMarkedItems)
    {
      this.onlyallowTransportItemsRow.SetActive(true);
      this.onlyAllowTransportItemsCheckBox.isOn = this.storage.GetOnlyFetchMarkedItems();
      this.onlyAllowTransportItemsImg.enabled = this.storage.GetOnlyFetchMarkedItems();
    }
    else
      this.onlyallowTransportItemsRow.SetActive(false);
  }

  public bool IsTagAllowed(Tag tag)
  {
    return this.targetFilterable.AcceptedTags.Contains(tag);
  }

  public void AddTag(Tag tag)
  {
    if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
      return;
    this.targetFilterable.AddTagToFilter(tag);
  }

  public void RemoveTag(Tag tag)
  {
    if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
      return;
    this.targetFilterable.RemoveTagFromFilter(tag);
  }

  private List<TreeFilterableSideScreen.TagOrderInfo> GetTagsSortedAlphabetically(
    ICollection<Tag> tags)
  {
    List<TreeFilterableSideScreen.TagOrderInfo> tagOrderInfoList = new List<TreeFilterableSideScreen.TagOrderInfo>();
    foreach (Tag tag in (IEnumerable<Tag>) tags)
      tagOrderInfoList.Add(new TreeFilterableSideScreen.TagOrderInfo()
      {
        tag = tag,
        strippedName = UI.StripLinkFormatting(tag.ProperName())
      });
    tagOrderInfoList.Sort((Comparison<TreeFilterableSideScreen.TagOrderInfo>) ((a, b) => a.strippedName.CompareTo(b.strippedName)));
    return tagOrderInfoList;
  }

  private TreeFilterableSideScreenRow AddRow(Tag rowTag)
  {
    TreeFilterableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
    freeElement.Parent = this;
    this.tagRowMap.Add(rowTag, freeElement);
    Dictionary<Tag, bool> filterMap = new Dictionary<Tag, bool>();
    foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically((ICollection<Tag>) WorldInventory.Instance.GetDiscoveredResourcesFromTag(rowTag)))
      filterMap.Add(tagOrderInfo.tag, this.targetFilterable.ContainsTag(tagOrderInfo.tag) || this.targetFilterable.ContainsTag(rowTag));
    freeElement.SetElement(rowTag, this.targetFilterable.ContainsTag(rowTag), filterMap);
    freeElement.transform.SetAsLastSibling();
    return freeElement;
  }

  public float GetAmountInStorage(Tag tag)
  {
    if (!this.IsStorage)
      return 0.0f;
    return this.storage.GetMassAvailable(tag);
  }

  private void CreateCategories()
  {
    if (this.storage.storageFilters != null && this.storage.storageFilters.Count >= 1)
    {
      bool flag = (UnityEngine.Object) this.target.GetComponent<CreatureDeliveryPoint>() != (UnityEngine.Object) null;
      foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically((ICollection<Tag>) this.storage.storageFilters))
      {
        Tag tag = tagOrderInfo.tag;
        if (flag || WorldInventory.Instance.IsDiscovered(tag))
          this.AddRow(tag);
      }
      this.visualDirty = true;
    }
    else
      Debug.LogError((object) "If you're filtering, your storage filter should have the filters set on it");
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
      this.storage.Unsubscribe(644822890, new System.Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
    this.rowPool.ClearAll();
    this.elementPool.ClearAll();
    this.tagRowMap.Clear();
  }

  private struct TagOrderInfo
  {
    public Tag tag;
    public string strippedName;
  }
}
