// Decompiled with JetBrains decompiler
// Type: ComplexFabricator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ComplexFabricator : KMonoBehaviour, ISim200ms, ISim1000ms
{
  public static int MAX_QUEUE_SIZE = 99;
  public static int QUEUE_INFINITE = -1;
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((System.Action<ComplexFabricator, object>) ((component, data) => component.OnStorageChange(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnDroppedAllDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((System.Action<ComplexFabricator, object>) ((component, data) => component.OnDroppedAll(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((System.Action<ComplexFabricator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((System.Action<ComplexFabricator, object>) ((component, data) => component.OnCopySettings(data)));
  public bool duplicantOperated = true;
  [SerializeField]
  public HashedString fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;
  public ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
  public bool labelByResult = true;
  public Vector3 outputOffset = Vector3.zero;
  public TagBits keepAdditionalTags = new TagBits();
  [Serialize]
  private Dictionary<string, int> recipeQueueCounts = new Dictionary<string, int>();
  private int workingOrderIdx = -1;
  private List<int> openOrderCounts = new List<int>();
  private bool queueDirty = true;
  private List<FetchList2> fetchListList = new List<FetchList2>();
  private Dictionary<Tag, float> materialNeedCache = new Dictionary<Tag, float>();
  private const int MaxPrefetchCount = 2;
  protected ComplexFabricatorWorkable workable;
  [SerializeField]
  public ComplexFabricator.ResultState resultState;
  [SerializeField]
  public float heatedTemperature;
  [SerializeField]
  public bool storeProduced;
  public ChoreType choreType;
  public bool keepExcessLiquids;
  private int nextOrderIdx;
  private bool nextOrderIsWorkable;
  [Serialize]
  private string lastWorkingRecipe;
  [Serialize]
  private float orderProgress;
  private bool hasOpenOrders;
  private Chore chore;
  private bool cancelling;
  private ComplexRecipe[] recipe_list;
  [SerializeField]
  public Storage inStorage;
  [SerializeField]
  public Storage buildStorage;
  [SerializeField]
  public Storage outStorage;
  [MyCmpAdd]
  private LoopingSounds loopingSounds;
  [MyCmpReq]
  protected Operational operational;
  [MyCmpAdd]
  private ComplexFabricatorSM fabricatorSM;
  private ProgressBar progressBar;

  public ComplexFabricatorWorkable Workable
  {
    get
    {
      return this.workable;
    }
  }

  public int CurrentOrderIdx
  {
    get
    {
      return this.nextOrderIdx;
    }
  }

  public ComplexRecipe CurrentWorkingOrder
  {
    get
    {
      if (this.HasWorkingOrder)
        return this.recipe_list[this.workingOrderIdx];
      return (ComplexRecipe) null;
    }
  }

  public ComplexRecipe NextOrder
  {
    get
    {
      if (this.nextOrderIsWorkable)
        return this.recipe_list[this.nextOrderIdx];
      return (ComplexRecipe) null;
    }
  }

  public float OrderProgress
  {
    get
    {
      return this.orderProgress;
    }
    set
    {
      this.orderProgress = value;
    }
  }

  public bool HasAnyOrder
  {
    get
    {
      if (!this.HasWorkingOrder)
        return this.hasOpenOrders;
      return true;
    }
  }

  public bool HasWorker
  {
    get
    {
      if (this.duplicantOperated)
        return (UnityEngine.Object) this.workable.worker != (UnityEngine.Object) null;
      return true;
    }
  }

  public bool WaitingForWorker
  {
    get
    {
      if (this.HasWorkingOrder)
        return !this.HasWorker;
      return false;
    }
  }

  private bool HasWorkingOrder
  {
    get
    {
      return this.workingOrderIdx > -1;
    }
  }

  public List<FetchList2> DebugFetchLists
  {
    get
    {
      return this.fetchListList;
    }
  }

  [OnDeserialized]
  protected virtual void OnDeserializedMethod()
  {
    List<string> stringList = new List<string>();
    foreach (string key in this.recipeQueueCounts.Keys)
    {
      if (ComplexRecipeManager.Get().GetRecipe(key) == null)
        stringList.Add(key);
    }
    foreach (string key in stringList)
    {
      Debug.LogWarningFormat("{1} removing missing recipe from queue: {0}", (object) key, (object) this.name);
      this.recipeQueueCounts.Remove(key);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetRecipes();
    this.simRenderLoadBalance = true;
    this.choreType = Db.Get().ChoreTypes.Fabricate;
    this.Subscribe<ComplexFabricator>(-1957399615, ComplexFabricator.OnDroppedAllDelegate);
    this.Subscribe<ComplexFabricator>(-592767678, ComplexFabricator.OnOperationalChangedDelegate);
    this.Subscribe<ComplexFabricator>(-905833192, ComplexFabricator.OnCopySettingsDelegate);
    this.Subscribe<ComplexFabricator>(-1697596308, ComplexFabricator.OnStorageChangeDelegate);
    this.workable = this.GetComponent<ComplexFabricatorWorkable>();
    Components.ComplexFabricators.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.InitRecipeQueueCount();
    foreach (string key in this.recipeQueueCounts.Keys)
    {
      if (this.recipeQueueCounts[key] == 100)
        this.recipeQueueCounts[key] = ComplexFabricator.QUEUE_INFINITE;
    }
    this.buildStorage.Transfer(this.inStorage, true, true);
    this.DropExcessIngredients(this.inStorage);
    int recipeIndex = this.FindRecipeIndex(this.lastWorkingRecipe);
    if (recipeIndex <= -1)
      return;
    this.nextOrderIdx = recipeIndex;
  }

  protected override void OnCleanUp()
  {
    this.CancelAllOpenOrders();
    this.CancelChore();
    Components.ComplexFabricators.Remove(this);
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object data)
  {
    if ((bool) data)
      this.queueDirty = true;
    else
      this.CancelAllOpenOrders();
    this.UpdateChore();
  }

  public void Sim1000ms(float dt)
  {
    this.RefreshAndStartNextOrder();
    if (this.materialNeedCache.Count <= 0 || this.fetchListList.Count != 0)
      return;
    Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} has material needs cached, but no open fetches. materialNeedCache={1}, fetchListList={2}", (object) this.gameObject, (object) this.materialNeedCache.Count, (object) this.fetchListList.Count);
    this.queueDirty = true;
  }

  public void Sim200ms(float dt)
  {
    if (!this.operational.IsOperational)
      return;
    this.operational.SetActive(this.HasWorkingOrder && this.HasWorker, false);
    if (this.duplicantOperated || !this.HasWorkingOrder)
      return;
    ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
    this.orderProgress += dt / recipe.time;
    if ((double) this.orderProgress < 1.0)
      return;
    this.CompleteWorkingOrder();
  }

  private void RefreshAndStartNextOrder()
  {
    if (!this.operational.IsOperational)
      return;
    if (this.queueDirty)
      this.RefreshQueue();
    if (this.HasWorkingOrder || !this.nextOrderIsWorkable)
      return;
    this.StartWorkingOrder(this.nextOrderIdx);
  }

  public void SetQueueDirty()
  {
    this.queueDirty = true;
  }

  private void RefreshQueue()
  {
    this.queueDirty = false;
    this.ValidateWorkingOrder();
    this.ValidateNextOrder();
    this.UpdateOpenOrders();
    this.DropExcessIngredients(this.inStorage);
    this.Trigger(1721324763, (object) this);
  }

  private void StartWorkingOrder(int index)
  {
    Debug.Assert(!this.HasWorkingOrder, (object) "machineOrderIdx already set");
    this.workingOrderIdx = index;
    if (this.recipe_list[this.workingOrderIdx].id != this.lastWorkingRecipe)
    {
      this.orderProgress = 0.0f;
      this.lastWorkingRecipe = this.recipe_list[this.workingOrderIdx].id;
    }
    this.TransferCurrentRecipeIngredientsForBuild();
    Debug.Assert(this.openOrderCounts[this.workingOrderIdx] > 0, (object) "openOrderCount invalid");
    List<int> openOrderCounts;
    int workingOrderIdx;
    (openOrderCounts = this.openOrderCounts)[workingOrderIdx = this.workingOrderIdx] = openOrderCounts[workingOrderIdx] - 1;
    this.UpdateChore();
    this.AdvanceNextOrder();
  }

  private void CancelWorkingOrder()
  {
    Debug.Assert(this.HasWorkingOrder, (object) "machineOrderIdx not set");
    this.buildStorage.Transfer(this.inStorage, true, true);
    this.workingOrderIdx = -1;
    this.orderProgress = 0.0f;
    this.UpdateChore();
  }

  public void CompleteWorkingOrder()
  {
    if (!this.HasWorkingOrder)
    {
      Debug.LogWarning((object) "CompleteWorkingOrder called with no working order.", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
      this.SpawnOrderProduct(recipe);
      float num = this.buildStorage.MassStored();
      if ((double) num != 0.0)
      {
        Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} build storage contains mass {1} after order completion.", (object) this.gameObject, (object) num);
        this.buildStorage.Transfer(this.inStorage, true, true);
      }
      this.DecrementRecipeQueueCountInternal(recipe, true);
      this.workingOrderIdx = -1;
      this.orderProgress = 0.0f;
      this.CancelChore();
      if (this.cancelling)
        return;
      this.RefreshAndStartNextOrder();
    }
  }

  private void ValidateWorkingOrder()
  {
    if (!this.HasWorkingOrder || this.IsRecipeQueued(this.recipe_list[this.workingOrderIdx]))
      return;
    this.CancelWorkingOrder();
  }

  private void UpdateChore()
  {
    if (!this.duplicantOperated)
      return;
    bool flag = this.operational.IsOperational && this.HasWorkingOrder;
    if (flag && this.chore == null)
    {
      this.CreateChore();
    }
    else
    {
      if (flag || this.chore == null)
        return;
      this.CancelChore();
    }
  }

  private void AdvanceNextOrder()
  {
    for (int index = 0; index < this.recipe_list.Length; ++index)
    {
      this.nextOrderIdx = (this.nextOrderIdx + 1) % this.recipe_list.Length;
      ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
      this.nextOrderIsWorkable = this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage);
      if (this.nextOrderIsWorkable)
        break;
    }
  }

  private void ValidateNextOrder()
  {
    ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
    this.nextOrderIsWorkable = this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage);
    if (this.nextOrderIsWorkable)
      return;
    this.AdvanceNextOrder();
  }

  private void CancelAllOpenOrders()
  {
    for (int index = 0; index < this.openOrderCounts.Count; ++index)
      this.openOrderCounts[index] = 0;
    this.ClearMaterialNeeds();
    this.CancelFetches();
  }

  private void UpdateOpenOrders()
  {
    ComplexRecipe[] recipes = this.GetRecipes();
    if (recipes.Length != this.openOrderCounts.Count)
      Debug.LogErrorFormat((UnityEngine.Object) this.gameObject, "Recipe count {0} doesn't match open order count {1}", (object) recipes.Length, (object) this.openOrderCounts.Count);
    bool flag = false;
    this.hasOpenOrders = false;
    for (int index = 0; index < recipes.Length; ++index)
    {
      int recipePrefetchCount = this.GetRecipePrefetchCount(recipes[index]);
      if (recipePrefetchCount > 0)
        this.hasOpenOrders = true;
      int openOrderCount = this.openOrderCounts[index];
      if (openOrderCount != recipePrefetchCount)
      {
        if (recipePrefetchCount < openOrderCount)
          flag = true;
        this.openOrderCounts[index] = recipePrefetchCount;
      }
    }
    DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary1 = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
    DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
    for (int index = 0; index < this.openOrderCounts.Count; ++index)
    {
      if (this.openOrderCounts[index] > 0)
      {
        foreach (ComplexRecipe.RecipeElement ingredient in this.recipe_list[index].ingredients)
          pooledDictionary1[ingredient.material] = this.inStorage.GetAmountAvailable(ingredient.material);
      }
    }
    for (int index = 0; index < this.recipe_list.Length; ++index)
    {
      int openOrderCount = this.openOrderCounts[index];
      if (openOrderCount > 0)
      {
        foreach (ComplexRecipe.RecipeElement ingredient in this.recipe_list[index].ingredients)
        {
          float num1 = ingredient.amount * (float) openOrderCount;
          float num2 = num1 - pooledDictionary1[ingredient.material];
          if ((double) num2 > 0.0)
          {
            float num3;
            missingAmounts.TryGetValue(ingredient.material, out num3);
            missingAmounts[ingredient.material] = num3 + num2;
            pooledDictionary1[ingredient.material] = 0.0f;
          }
          else
          {
            DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary2;
            Tag material;
            (pooledDictionary2 = pooledDictionary1)[material = ingredient.material] = pooledDictionary2[material] - num1;
          }
        }
      }
    }
    if (flag)
      this.CancelFetches();
    if (missingAmounts.Count > 0)
      this.UpdateFetches(missingAmounts);
    this.UpdateMaterialNeeds((Dictionary<Tag, float>) missingAmounts);
    missingAmounts.Recycle();
    pooledDictionary1.Recycle();
  }

  private void UpdateMaterialNeeds(Dictionary<Tag, float> missingAmounts)
  {
    this.ClearMaterialNeeds();
    foreach (KeyValuePair<Tag, float> missingAmount in missingAmounts)
    {
      MaterialNeeds.Instance.UpdateNeed(missingAmount.Key, missingAmount.Value);
      this.materialNeedCache.Add(missingAmount.Key, missingAmount.Value);
    }
  }

  private void ClearMaterialNeeds()
  {
    foreach (KeyValuePair<Tag, float> keyValuePair in this.materialNeedCache)
      MaterialNeeds.Instance.UpdateNeed(keyValuePair.Key, -keyValuePair.Value);
    this.materialNeedCache.Clear();
  }

  private void OnFetchComplete()
  {
    for (int index = this.fetchListList.Count - 1; index >= 0; --index)
    {
      if (this.fetchListList[index].IsComplete)
      {
        this.fetchListList.RemoveAt(index);
        this.queueDirty = true;
      }
    }
  }

  private void OnStorageChange(object data)
  {
    this.queueDirty = true;
  }

  private void OnDroppedAll(object data)
  {
    if (this.HasWorkingOrder)
      this.CancelWorkingOrder();
    this.CancelAllOpenOrders();
    this.RefreshQueue();
  }

  private void DropExcessIngredients(Storage storage)
  {
    TagBits search_tags = new TagBits();
    search_tags.Or(ref this.keepAdditionalTags);
    for (int index = 0; index < this.recipe_list.Length; ++index)
    {
      ComplexRecipe recipe = this.recipe_list[index];
      if (this.IsRecipeQueued(recipe))
      {
        foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
          search_tags.SetTag(ingredient.material);
      }
    }
    for (int index = storage.items.Count - 1; index >= 0; --index)
    {
      GameObject go = storage.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
      {
        PrimaryElement component1 = go.GetComponent<PrimaryElement>();
        if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && (!this.keepExcessLiquids || !component1.Element.IsLiquid))
        {
          KPrefabID component2 = go.GetComponent<KPrefabID>();
          if ((bool) ((UnityEngine.Object) component2) && !component2.HasAnyTags(ref search_tags))
            storage.Drop(go, true);
        }
      }
    }
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    ComplexFabricator component = gameObject.GetComponent<ComplexFabricator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    foreach (ComplexRecipe recipe in this.recipe_list)
    {
      int count;
      if (!component.recipeQueueCounts.TryGetValue(recipe.id, out count))
        count = 0;
      this.SetRecipeQueueCountInternal(recipe, count);
    }
    this.RefreshQueue();
  }

  private int CompareRecipe(ComplexRecipe a, ComplexRecipe b)
  {
    if (a.sortOrder != b.sortOrder)
      return a.sortOrder - b.sortOrder;
    return StringComparer.InvariantCulture.Compare(a.id, b.id);
  }

  public ComplexRecipe[] GetRecipes()
  {
    if (this.recipe_list == null)
    {
      Tag prefabTag = this.GetComponent<KPrefabID>().PrefabTag;
      List<ComplexRecipe> recipes = ComplexRecipeManager.Get().recipes;
      List<ComplexRecipe> complexRecipeList = new List<ComplexRecipe>();
      foreach (ComplexRecipe complexRecipe in recipes)
      {
        foreach (Tag fabricator in complexRecipe.fabricators)
        {
          if (fabricator == prefabTag)
            complexRecipeList.Add(complexRecipe);
        }
      }
      this.recipe_list = complexRecipeList.ToArray();
      Array.Sort<ComplexRecipe>(this.recipe_list, new Comparison<ComplexRecipe>(this.CompareRecipe));
    }
    return this.recipe_list;
  }

  private void InitRecipeQueueCount()
  {
    foreach (ComplexRecipe recipe in this.GetRecipes())
    {
      bool flag = false;
      foreach (string key in this.recipeQueueCounts.Keys)
      {
        if (key == recipe.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.recipeQueueCounts.Add(recipe.id, 0);
      this.openOrderCounts.Add(0);
    }
  }

  private int FindRecipeIndex(string id)
  {
    for (int index = 0; index < this.recipe_list.Length; ++index)
    {
      if (this.recipe_list[index].id == id)
        return index;
    }
    return -1;
  }

  public int GetRecipeQueueCount(ComplexRecipe recipe)
  {
    return this.recipeQueueCounts[recipe.id];
  }

  public bool IsRecipeQueued(ComplexRecipe recipe)
  {
    int recipeQueueCount = this.recipeQueueCounts[recipe.id];
    Debug.Assert(recipeQueueCount >= 0 || recipeQueueCount == ComplexFabricator.QUEUE_INFINITE);
    return recipeQueueCount != 0;
  }

  public int GetRecipePrefetchCount(ComplexRecipe recipe)
  {
    int remainingQueueCount = this.GetRemainingQueueCount(recipe);
    Debug.Assert(remainingQueueCount >= 0);
    return Mathf.Min(2, remainingQueueCount);
  }

  private int GetRemainingQueueCount(ComplexRecipe recipe)
  {
    int recipeQueueCount = this.recipeQueueCounts[recipe.id];
    Debug.Assert(recipeQueueCount >= 0 || recipeQueueCount == ComplexFabricator.QUEUE_INFINITE);
    if (recipeQueueCount == ComplexFabricator.QUEUE_INFINITE)
      return ComplexFabricator.MAX_QUEUE_SIZE;
    if (recipeQueueCount <= 0)
      return 0;
    if (this.IsCurrentRecipe(recipe))
      --recipeQueueCount;
    return recipeQueueCount;
  }

  private bool IsCurrentRecipe(ComplexRecipe recipe)
  {
    if (this.workingOrderIdx < 0)
      return false;
    return this.recipe_list[this.workingOrderIdx].id == recipe.id;
  }

  public void SetRecipeQueueCount(ComplexRecipe recipe, int count)
  {
    this.SetRecipeQueueCountInternal(recipe, count);
    this.RefreshQueue();
  }

  private void SetRecipeQueueCountInternal(ComplexRecipe recipe, int count)
  {
    this.recipeQueueCounts[recipe.id] = count;
  }

  public void IncrementRecipeQueueCount(ComplexRecipe recipe)
  {
    if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
      this.recipeQueueCounts[recipe.id] = 0;
    else if (this.recipeQueueCounts[recipe.id] >= ComplexFabricator.MAX_QUEUE_SIZE)
    {
      this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
    }
    else
    {
      Dictionary<string, int> recipeQueueCounts;
      string id;
      (recipeQueueCounts = this.recipeQueueCounts)[id = recipe.id] = recipeQueueCounts[id] + 1;
    }
    this.RefreshQueue();
  }

  public void DecrementRecipeQueueCount(ComplexRecipe recipe, bool respectInfinite = true)
  {
    this.DecrementRecipeQueueCountInternal(recipe, respectInfinite);
    this.RefreshQueue();
  }

  private void DecrementRecipeQueueCountInternal(ComplexRecipe recipe, bool respectInfinite = true)
  {
    if (respectInfinite && this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
      return;
    if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
      this.recipeQueueCounts[recipe.id] = ComplexFabricator.MAX_QUEUE_SIZE;
    else if (this.recipeQueueCounts[recipe.id] == 0)
    {
      this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
    }
    else
    {
      Dictionary<string, int> recipeQueueCounts;
      string id;
      (recipeQueueCounts = this.recipeQueueCounts)[id = recipe.id] = recipeQueueCounts[id] - 1;
    }
  }

  private void CreateChore()
  {
    Debug.Assert(this.chore == null, (object) "chore should be null");
    this.chore = this.workable.CreateWorkChore(this.choreType, this.orderProgress);
  }

  private void CancelChore()
  {
    if (this.cancelling)
      return;
    this.cancelling = true;
    if (this.chore != null)
    {
      this.chore.Cancel("order cancelled");
      this.chore = (Chore) null;
    }
    this.cancelling = false;
  }

  private void UpdateFetches(
    DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts)
  {
    ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
    foreach (KeyValuePair<Tag, float> missingAmount in (Dictionary<Tag, float>) missingAmounts)
    {
      if ((double) missingAmount.Value >= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.HasPendingFetch(missingAmount.Key))
      {
        FetchList2 fetchList2_1 = new FetchList2(this.inStorage, byHash);
        FetchList2 fetchList2_2 = fetchList2_1;
        Tag key = missingAmount.Key;
        float num1 = missingAmount.Value;
        Tag tag = key;
        double num2 = (double) num1;
        fetchList2_2.Add(tag, (Tag[]) null, (Tag[]) null, (float) num2, FetchOrder2.OperationalRequirement.None);
        fetchList2_1.ShowStatusItem = false;
        fetchList2_1.Submit(new System.Action(this.OnFetchComplete), false);
        this.fetchListList.Add(fetchList2_1);
      }
    }
  }

  private bool HasPendingFetch(Tag tag)
  {
    foreach (FetchList2 fetchList in this.fetchListList)
    {
      float num;
      fetchList.MinimumAmount.TryGetValue(tag, out num);
      if ((double) num > 0.0)
        return true;
    }
    return false;
  }

  private void CancelFetches()
  {
    foreach (FetchList2 fetchList in this.fetchListList)
      fetchList.Cancel("cancel all orders");
    this.fetchListList.Clear();
  }

  protected virtual void TransferCurrentRecipeIngredientsForBuild()
  {
    ComplexRecipe.RecipeElement[] ingredients = this.recipe_list[this.workingOrderIdx].ingredients;
label_8:
    for (int index = 0; index < ingredients.Length; ++index)
    {
      ComplexRecipe.RecipeElement recipeElement = ingredients[index];
      float amount;
      while (true)
      {
        amount = recipeElement.amount - this.buildStorage.GetAmountAvailable(recipeElement.material);
        if ((double) amount > 0.0)
        {
          if ((double) this.inStorage.GetAmountAvailable(recipeElement.material) > 0.0)
          {
            double num = (double) this.inStorage.Transfer(this.buildStorage, recipeElement.material, amount, false, true);
          }
          else
            break;
        }
        else
          goto label_8;
      }
      Debug.LogWarningFormat("TransferCurrentRecipeIngredientsForBuild ran out of {0} but still needed {1} more.", (object) recipeElement.material, (object) amount);
    }
  }

  protected virtual bool HasIngredients(ComplexRecipe recipe, Storage storage)
  {
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      float amountAvailable = storage.GetAmountAvailable(ingredient.material);
      if ((double) (ingredient.amount - amountAvailable) >= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
        return false;
    }
    return true;
  }

  protected virtual List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    SimUtil.DiseaseInfo diseaseInfo;
    diseaseInfo.count = 0;
    diseaseInfo.idx = (byte) 0;
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
      num2 += ingredient.amount;
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      float num3 = ingredient.amount / num2;
      SimUtil.DiseaseInfo disease_info;
      float aggregate_temperature;
      this.buildStorage.ConsumeAndGetDisease(ingredient.material, ingredient.amount, out disease_info, out aggregate_temperature);
      if (disease_info.count > diseaseInfo.count)
        diseaseInfo = disease_info;
      num1 += aggregate_temperature * num3;
    }
    foreach (ComplexRecipe.RecipeElement result in recipe.results)
    {
      GameObject first = this.buildStorage.FindFirst(result.material);
      if ((UnityEngine.Object) first != (UnityEngine.Object) null)
      {
        Edible component = first.GetComponent<Edible>();
        if ((bool) ((UnityEngine.Object) component))
          ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
      }
      switch (this.resultState)
      {
        case ComplexFabricator.ResultState.PassTemperature:
        case ComplexFabricator.ResultState.Heated:
          GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(result.material), Grid.SceneLayer.Ore, (string) null, 0);
          int cell = Grid.PosToCell((KMonoBehaviour) this);
          go.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore) + this.outputOffset);
          PrimaryElement component1 = go.GetComponent<PrimaryElement>();
          component1.Units = result.amount;
          component1.Temperature = this.resultState != ComplexFabricator.ResultState.PassTemperature ? this.heatedTemperature : num1;
          go.SetActive(true);
          float num3 = result.amount / recipe.TotalResultUnits();
          component1.AddDisease(diseaseInfo.idx, Mathf.RoundToInt((float) diseaseInfo.count * num3), "ComplexFabricator.CompleteOrder");
          go.GetComponent<KMonoBehaviour>().Trigger(748399584, (object) null);
          gameObjectList.Add(go);
          if (this.storeProduced)
          {
            this.outStorage.Store(go, false, false, true, false);
            break;
          }
          break;
        case ComplexFabricator.ResultState.Melted:
          if (this.storeProduced)
          {
            float temperature = ElementLoader.GetElement(result.material).lowTemp + (float) (((double) ElementLoader.GetElement(result.material).highTemp - (double) ElementLoader.GetElement(result.material).lowTemp) / 2.0);
            this.outStorage.AddLiquid(ElementLoader.GetElementID(result.material), result.amount, temperature, (byte) 0, 0, false, true);
            break;
          }
          break;
      }
      if (gameObjectList.Count > 0)
      {
        SymbolOverrideController component2 = this.GetComponent<SymbolOverrideController>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          KAnim.Build build = gameObjectList[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
          KAnim.Build.Symbol symbol = build.GetSymbol((KAnimHashedString) build.name);
          if (symbol != null)
          {
            component2.TryRemoveSymbolOverride((HashedString) "output_tracker", 0);
            component2.AddSymbolOverride((HashedString) "output_tracker", symbol, 0);
          }
          else
            Debug.LogWarning((object) (component2.name + " is missing symbol " + build.name));
        }
      }
    }
    return gameObjectList;
  }

  public virtual List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    ComplexRecipe[] recipes = this.GetRecipes();
    if (recipes.Length > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.PROCESSES, (string) UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSES, Descriptor.DescriptorType.Effect);
      descriptorList.Add(descriptor);
    }
    foreach (ComplexRecipe complexRecipe in recipes)
    {
      string txt = string.Empty;
      string empty = string.Empty;
      foreach (ComplexRecipe.RecipeElement ingredient in complexRecipe.ingredients)
      {
        txt = txt + "• " + string.Format((string) UI.BUILDINGEFFECTS.PROCESSEDITEM, (object) string.Empty, (object) ingredient.material.ProperName());
        empty += string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSEDITEM, (object) string.Join(", ", ((IEnumerable<ComplexRecipe.RecipeElement>) complexRecipe.results).Select<ComplexRecipe.RecipeElement, string>((Func<ComplexRecipe.RecipeElement, string>) (r => r.material.ProperName())).ToArray<string>()));
      }
      Descriptor descriptor = new Descriptor(txt, empty, Descriptor.DescriptorType.Effect, false);
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public virtual List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
  {
    return new List<Descriptor>();
  }

  public string GetConversationTopic()
  {
    if (this.HasWorkingOrder)
    {
      ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
      if (recipe != null)
        return recipe.results[0].material.Name;
    }
    return (string) null;
  }

  public enum ResultState
  {
    PassTemperature,
    Heated,
    Melted,
  }
}
