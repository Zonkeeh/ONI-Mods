// Decompiled with JetBrains decompiler
// Type: TagFilterScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TagFilterScreen : SideScreenContent
{
  public static TagFilterScreen.TagEntry defaultRootTag = new TagFilterScreen.TagEntry()
  {
    name = "All",
    tag = new Tag(),
    children = new TagFilterScreen.TagEntry[0]
  };
  private TagFilterScreen.TagEntry rootTag = TagFilterScreen.defaultRootTag;
  private List<Tag> acceptedTags = new List<Tag>();
  [SerializeField]
  private KTreeControl treeControl;
  private KTreeControl.UserItem rootItem;
  private TreeFilterable targetFilterable;

  public override bool IsValidForTarget(GameObject target)
  {
    return (Object) target.GetComponent<TreeFilterable>() != (Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    if ((Object) target == (Object) null)
    {
      Debug.LogError((object) "The target object provided was null");
    }
    else
    {
      this.targetFilterable = target.GetComponent<TreeFilterable>();
      if ((Object) this.targetFilterable == (Object) null)
      {
        Debug.LogError((object) "The target provided does not have a Tree Filterable component");
      }
      else
      {
        if (!this.targetFilterable.showUserMenu)
          return;
        this.Filter(this.targetFilterable.AcceptedTags);
        this.Activate();
      }
    }
  }

  protected override void OnActivate()
  {
    this.rootItem = this.BuildDisplay(this.rootTag);
    this.treeControl.SetUserItemRoot(this.rootItem);
    this.treeControl.root.opened = true;
    this.Filter(this.treeControl.root, this.acceptedTags, false);
  }

  public static List<Tag> GetAllTags()
  {
    List<Tag> tagList = new List<Tag>();
    foreach (TagFilterScreen.TagEntry child in TagFilterScreen.defaultRootTag.children)
    {
      if (child.tag.IsValid)
        tagList.Add(child.tag);
    }
    return tagList;
  }

  private KTreeControl.UserItem BuildDisplay(TagFilterScreen.TagEntry root)
  {
    KTreeControl.UserItem userItem = (KTreeControl.UserItem) null;
    if (root.name != null && root.name != string.Empty)
    {
      userItem = new KTreeControl.UserItem()
      {
        text = root.name,
        userData = (object) root.tag
      };
      List<KTreeControl.UserItem> userItemList = new List<KTreeControl.UserItem>();
      if (root.children != null)
      {
        foreach (TagFilterScreen.TagEntry child in root.children)
          userItemList.Add(this.BuildDisplay(child));
      }
      userItem.children = (IList<KTreeControl.UserItem>) userItemList;
    }
    return userItem;
  }

  private static KTreeControl.UserItem CreateTree(
    string tree_name,
    Tag tree_tag,
    IList<Element> items)
  {
    KTreeControl.UserItem userItem1 = new KTreeControl.UserItem()
    {
      text = tree_name,
      userData = (object) tree_tag,
      children = (IList<KTreeControl.UserItem>) new List<KTreeControl.UserItem>()
    };
    foreach (Element element in (IEnumerable<Element>) items)
    {
      KTreeControl.UserItem userItem2 = new KTreeControl.UserItem()
      {
        text = element.name,
        userData = (object) GameTagExtensions.Create(element.id)
      };
      userItem1.children.Add(userItem2);
    }
    return userItem1;
  }

  public void SetRootTag(TagFilterScreen.TagEntry root_tag)
  {
    this.rootTag = root_tag;
  }

  public void Filter(List<Tag> acceptedTags)
  {
    this.acceptedTags = acceptedTags;
  }

  private void Filter(KTreeItem root, List<Tag> acceptedTags, bool parentEnabled)
  {
    root.checkboxChecked = parentEnabled || root.userData != null && acceptedTags.Contains((Tag) root.userData);
    foreach (KTreeItem child in (IEnumerable<KTreeItem>) root.children)
      this.Filter(child, acceptedTags, root.checkboxChecked);
    if (root.checkboxChecked || root.children.Count <= 0)
      return;
    bool flag = true;
    foreach (KTreeItem child in (IEnumerable<KTreeItem>) root.children)
    {
      if (!child.checkboxChecked)
      {
        flag = false;
        break;
      }
    }
    root.checkboxChecked = flag;
  }

  private void AddEnabledTags(KTreeItem root, List<Tag> tags)
  {
    bool flag = false;
    if (root.userData != null)
    {
      Tag userData = (Tag) root.userData;
      if (userData.IsValid && root.checkboxChecked)
      {
        flag = true;
        tags.Add(userData);
      }
    }
    if (flag)
      return;
    foreach (KTreeItem child in (IEnumerable<KTreeItem>) root.children)
      this.AddEnabledTags(child, tags);
  }

  private void UpdateFilters()
  {
    if ((Object) this.targetFilterable == (Object) null)
    {
      Debug.LogError((object) "Cannot update the filters on a null target.");
    }
    else
    {
      List<Tag> tags = new List<Tag>();
      this.AddEnabledTags(this.treeControl.root, tags);
      this.targetFilterable.UpdateFilters((IList<Tag>) tags);
    }
  }

  public class TagEntry
  {
    public string name;
    public Tag tag;
    public TagFilterScreen.TagEntry[] children;
  }
}
