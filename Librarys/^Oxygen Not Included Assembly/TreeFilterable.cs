// Decompiled with JetBrains decompiler
// Type: TreeFilterable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
  private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>((System.Action<TreeFilterable, object>) ((component, data) => component.OnCopySettings(data)));
  public bool showUserMenu = true;
  [SerializeField]
  [Serialize]
  private List<Tag> acceptedTags = new List<Tag>();
  [MyCmpReq]
  private Storage storage;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  public System.Action<Tag[]> OnFilterChanged;

  public List<Tag> AcceptedTags
  {
    get
    {
      return this.acceptedTags;
    }
  }

  private void OnDiscover(Tag category_tag, Tag tag)
  {
    if (!this.storage.storageFilters.Contains(category_tag))
      return;
    bool flag = false;
    if (WorldInventory.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
    {
      foreach (Tag storageFilter in this.storage.storageFilters)
      {
        if (!(storageFilter == category_tag) && WorldInventory.Instance.IsDiscovered(storageFilter))
        {
          flag = true;
          foreach (Tag tag1 in WorldInventory.Instance.GetDiscoveredResourcesFromTag(storageFilter))
          {
            if (!this.acceptedTags.Contains(tag1))
              return;
          }
        }
      }
      if (!flag)
        return;
    }
    foreach (Tag tag1 in WorldInventory.Instance.GetDiscoveredResourcesFromTag(category_tag))
    {
      if (!(tag1 == tag) && !this.acceptedTags.Contains(tag1))
        return;
    }
    this.AddTagToFilter(tag);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<TreeFilterable>(-905833192, TreeFilterable.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    WorldInventory.Instance.OnDiscover += new System.Action<Tag, Tag>(this.OnDiscover);
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
    {
      List<Tag> source = new List<Tag>();
      source.AddRange((IEnumerable<Tag>) this.acceptedTags);
      source.AddRange((IEnumerable<Tag>) this.storage.GetAllTagsInStorage());
      this.UpdateFilters((IList<Tag>) source.Distinct<Tag>().ToList<Tag>());
    }
    if (this.OnFilterChanged != null)
      this.OnFilterChanged(this.acceptedTags.ToArray());
    this.RemoveIncorrectAcceptedTags();
  }

  private void RemoveIncorrectAcceptedTags()
  {
    List<Tag> tagList = new List<Tag>();
    foreach (Tag acceptedTag in this.acceptedTags)
    {
      bool flag = false;
      foreach (Tag storageFilter in this.storage.storageFilters)
      {
        if (WorldInventory.Instance.GetDiscoveredResourcesFromTag(storageFilter).Contains(acceptedTag))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        tagList.Add(acceptedTag);
    }
    foreach (Tag t in tagList)
      this.RemoveTagFromFilter(t);
  }

  protected override void OnCleanUp()
  {
    WorldInventory.Instance.OnDiscover -= new System.Action<Tag, Tag>(this.OnDiscover);
    base.OnCleanUp();
  }

  private void OnCopySettings(object data)
  {
    TreeFilterable component = ((GameObject) data).GetComponent<TreeFilterable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.UpdateFilters((IList<Tag>) component.GetTags());
  }

  public Tag[] GetTags()
  {
    return this.acceptedTags.ToArray();
  }

  public bool ContainsTag(Tag t)
  {
    return this.acceptedTags.Contains(t);
  }

  public void AddTagToFilter(Tag t)
  {
    if (this.ContainsTag(t))
      return;
    this.UpdateFilters((IList<Tag>) new List<Tag>((IEnumerable<Tag>) this.acceptedTags)
    {
      t
    });
  }

  public void RemoveTagFromFilter(Tag t)
  {
    if (!this.ContainsTag(t))
      return;
    List<Tag> tagList = new List<Tag>((IEnumerable<Tag>) this.acceptedTags);
    tagList.Remove(t);
    this.UpdateFilters((IList<Tag>) tagList);
  }

  public void UpdateFilters(IList<Tag> filters)
  {
    this.acceptedTags.Clear();
    this.acceptedTags.AddRange((IEnumerable<Tag>) filters);
    if (this.OnFilterChanged != null)
      this.OnFilterChanged(this.acceptedTags.ToArray());
    if (!((UnityEngine.Object) this.storage != (UnityEngine.Object) null) || this.storage.items == null)
      return;
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (GameObject gameObject in this.storage.items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        KPrefabID component = gameObject.GetComponent<KPrefabID>();
        bool flag = false;
        foreach (Tag acceptedTag in this.acceptedTags)
        {
          if (component.Tags.Contains(acceptedTag))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          gameObjectList.Add(gameObject);
      }
    }
    foreach (GameObject go in gameObjectList)
      this.storage.Drop(go, true);
  }

  public string GetTagsAsStatus(int maxDisplays = 6)
  {
    string str = "Tags:\n";
    List<Tag> first = new List<Tag>((IEnumerable<Tag>) this.acceptedTags);
    first.Intersect<Tag>((IEnumerable<Tag>) this.storage.storageFilters);
    for (int index = 0; index < Mathf.Min(first.Count, maxDisplays); ++index)
    {
      str += first[index].ProperName();
      if (index < Mathf.Min(first.Count, maxDisplays) - 1)
        str += "\n";
      if (index == maxDisplays - 1 && first.Count > maxDisplays)
      {
        str += "\n...";
        break;
      }
    }
    if (this.tag.Length == 0)
      str = "No tags selected";
    return str;
  }
}
