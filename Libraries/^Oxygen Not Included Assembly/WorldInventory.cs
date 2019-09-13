// Decompiled with JetBrains decompiler
// Type: WorldInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
  private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>((System.Action<WorldInventory, object>) ((component, data) => component.GenerateInventoryReport(data)));
  [Serialize]
  private HashSet<Tag> Discovered = new HashSet<Tag>();
  [Serialize]
  private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();
  private Dictionary<Tag, List<Pickupable>> Inventory = new Dictionary<Tag, List<Pickupable>>();
  private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();
  private bool firstUpdate = true;
  private MinionGroupProber Prober;
  private int accessibleUpdateIndex;

  public static WorldInventory Instance { get; private set; }

  public event System.Action<Tag, Tag> OnDiscover;

  protected override void OnPrefabInit()
  {
    WorldInventory.Instance = this;
    this.Subscribe(Game.Instance.gameObject, -1588644844, new System.Action<object>(this.OnAddedFetchable));
    this.Subscribe(Game.Instance.gameObject, -1491270284, new System.Action<object>(this.OnRemovedFetchable));
    this.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
  }

  private void GenerateInventoryReport(object data)
  {
    int num1 = 0;
    int num2 = 0;
    IEnumerator enumerator = Components.Brains.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        CreatureBrain current = enumerator.Current as CreatureBrain;
        if ((UnityEngine.Object) current != (UnityEngine.Object) null)
        {
          if (current.HasTag(GameTags.Creatures.Wild))
          {
            ++num1;
            ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, current.GetProperName(), current.GetProperName());
          }
          else
          {
            ++num2;
            ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, current.GetProperName(), current.GetProperName());
          }
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
    {
      if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
        ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName, (string) null);
    }
  }

  protected override void OnSpawn()
  {
    this.Prober = MinionGroupProber.Get();
    this.StartCoroutine(this.InitialRefresh());
  }

  [DebuggerHidden]
  private IEnumerator InitialRefresh()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    WorldInventory.\u003CInitialRefresh\u003Ec__Iterator0 refreshCIterator0 = new WorldInventory.\u003CInitialRefresh\u003Ec__Iterator0();
    return (IEnumerator) refreshCIterator0;
  }

  public bool IsReachable(Pickupable pickupable)
  {
    return this.Prober.IsReachable((Workable) pickupable);
  }

  public float GetTotalAmount(Tag tag)
  {
    float num = 0.0f;
    this.accessibleAmounts.TryGetValue(tag, out num);
    return num;
  }

  public List<Pickupable> GetPickupables(Tag tag)
  {
    List<Pickupable> pickupableList = (List<Pickupable>) null;
    this.Inventory.TryGetValue(tag, out pickupableList);
    return pickupableList;
  }

  public List<Tag> GetPickupableTagsFromCategoryTag(Tag t)
  {
    List<Tag> tagList = new List<Tag>();
    List<Pickupable> pickupables = this.GetPickupables(t);
    if (pickupables != null && pickupables.Count > 0)
    {
      foreach (Pickupable pickupable in pickupables)
        tagList.AddRange((IEnumerable<Tag>) pickupable.KPrefabID.Tags);
    }
    return tagList;
  }

  public float GetAmount(Tag tag)
  {
    return Mathf.Max(this.GetTotalAmount(tag) - MaterialNeeds.Instance.GetAmount(tag), 0.0f);
  }

  public void Discover(Tag tag, Tag categoryTag)
  {
    bool flag = this.Discovered.Add(tag);
    this.DiscoverCategory(categoryTag, tag);
    if (!flag || this.OnDiscover == null)
      return;
    this.OnDiscover(categoryTag, tag);
  }

  private void DiscoverCategory(Tag category_tag, Tag item_tag)
  {
    HashSet<Tag> tagSet;
    if (!this.DiscoveredCategories.TryGetValue(category_tag, out tagSet))
    {
      tagSet = new HashSet<Tag>();
      this.DiscoveredCategories[category_tag] = tagSet;
    }
    tagSet.Add(item_tag);
  }

  public HashSet<Tag> GetDiscovered()
  {
    return this.Discovered;
  }

  public bool IsDiscovered(Tag tag)
  {
    if (!this.Discovered.Contains(tag))
      return this.DiscoveredCategories.ContainsKey(tag);
    return true;
  }

  public bool AnyDiscovered(ICollection<Tag> tags)
  {
    foreach (Tag tag in (IEnumerable<Tag>) tags)
    {
      if (this.IsDiscovered(tag))
        return true;
    }
    return false;
  }

  public bool Contains(Recipe.Ingredient[] ingredients)
  {
    bool flag = true;
    foreach (Recipe.Ingredient ingredient in ingredients)
    {
      if ((double) this.GetAmount(ingredient.tag) < (double) ingredient.amount)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
  {
    return this.DiscoveredCategories.TryGetValue(tag, out resources);
  }

  public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
  {
    HashSet<Tag> tagSet;
    if (this.DiscoveredCategories.TryGetValue(tag, out tagSet))
      return tagSet;
    return new HashSet<Tag>();
  }

  private void Update()
  {
    int num1 = 0;
    Dictionary<Tag, List<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
    while (enumerator.MoveNext())
    {
      KeyValuePair<Tag, List<Pickupable>> current = enumerator.Current;
      if (num1 == this.accessibleUpdateIndex || this.firstUpdate)
      {
        Tag key = current.Key;
        List<Pickupable> pickupableList = current.Value;
        float num2 = 0.0f;
        for (int index = 0; index < pickupableList.Count; ++index)
        {
          Pickupable cmp = pickupableList[index];
          if ((UnityEngine.Object) cmp != (UnityEngine.Object) null && !cmp.HasTag(GameTags.StoredPrivate))
            num2 += cmp.TotalAmount;
        }
        this.accessibleAmounts[key] = num2;
        this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
        break;
      }
      ++num1;
    }
    this.firstUpdate = false;
  }

  protected override void OnLoadLevel()
  {
    base.OnLoadLevel();
    WorldInventory.Instance = (WorldInventory) null;
  }

  public static Tag GetCategoryForTags(HashSet<Tag> tags)
  {
    Tag tag1 = Tag.Invalid;
    foreach (Tag tag2 in tags)
    {
      if (GameTags.AllCategories.Contains(tag2))
      {
        tag1 = tag2;
        break;
      }
    }
    return tag1;
  }

  public static Tag GetCategoryForEntity(KPrefabID entity)
  {
    ElementChunk component = entity.GetComponent<ElementChunk>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.GetComponent<PrimaryElement>().Element.materialCategory;
    return WorldInventory.GetCategoryForTags(entity.Tags);
  }

  private void OnAddedFetchable(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject.GetComponent<Health>() != (UnityEngine.Object) null)
      return;
    Pickupable component1 = gameObject.GetComponent<Pickupable>();
    KPrefabID component2 = component1.GetComponent<KPrefabID>();
    Tag tag1 = component2.PrefabID();
    if (!this.Inventory.ContainsKey(tag1))
    {
      Tag categoryForEntity = WorldInventory.GetCategoryForEntity(component2);
      DebugUtil.DevAssertArgs((categoryForEntity.IsValid ? 1 : 0) != 0, (object) component1.name, (object) "was found by worldinventory but doesn't have a category! Add it to the element definition.");
      this.Discover(tag1, categoryForEntity);
    }
    foreach (Tag tag2 in component2.Tags)
    {
      List<Pickupable> pickupableList;
      if (!this.Inventory.TryGetValue(tag2, out pickupableList))
      {
        pickupableList = new List<Pickupable>();
        this.Inventory[tag2] = pickupableList;
      }
      pickupableList.Add(component1);
    }
  }

  private void OnRemovedFetchable(object data)
  {
    Pickupable component = ((GameObject) data).GetComponent<Pickupable>();
    foreach (Tag tag in component.GetComponent<KPrefabID>().Tags)
    {
      List<Pickupable> pickupableList;
      if (this.Inventory.TryGetValue(tag, out pickupableList))
        pickupableList.Remove(component);
    }
  }

  public Dictionary<Tag, float> GetAccessibleAmounts()
  {
    return this.accessibleAmounts;
  }
}
