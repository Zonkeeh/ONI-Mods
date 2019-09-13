// Decompiled with JetBrains decompiler
// Type: Storage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Storage : Workable, ISaveLoadableDetails, IEffectDescriptor
{
  public bool allowSublimation = true;
  public float capacityKg = 20000f;
  public bool showInUI = true;
  public bool doDiseaseTransfer = true;
  public bool useGunForDelivery = true;
  public int storageNetworkID = -1;
  public List<GameObject> items = new List<GameObject>();
  protected float maxKGPerItem = float.MaxValue;
  public bool allowSettingOnlyFetchMarkedItems = true;
  [SerializeField]
  private List<Storage.StoredItemModifier> defaultStoredItemModifers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide
  };
  public bool ignoreSourcePriority;
  public bool allowItemRemoval;
  public bool onlyTransferFromLowerPriority;
  public bool showDescriptor;
  public bool allowUIItemRemoval;
  public List<Tag> storageFilters;
  public bool sendOnStoreOnSpawn;
  public Storage.FetchCategory fetchCategory;
  public float storageFullMargin;
  public Storage.FXPrefix fxPrefix;
  [MyCmpGet]
  public Prioritizable prioritizable;
  [MyCmpGet]
  public Automatable automatable;
  [MyCmpGet]
  protected PrimaryElement primaryElement;
  public bool dropOnLoad;
  private bool endOfLife;
  [Serialize]
  private bool onlyFetchMarkedItems;
  private static readonly List<Storage.StoredItemModifierInfo> StoredItemModifierHandlers;
  public static readonly List<Storage.StoredItemModifier> StandardSealedStorage;
  public static readonly List<Storage.StoredItemModifier> StandardFabricatorStorage;
  public static readonly List<Storage.StoredItemModifier> StandardInsulatedStorage;
  private static readonly EventSystem.IntraObjectHandler<Storage> OnDeathDelegate;
  private static readonly EventSystem.IntraObjectHandler<Storage> OnQueueDestroyObjectDelegate;
  private static readonly EventSystem.IntraObjectHandler<Storage> OnCopySettingsDelegate;
  private List<GameObject> deleted_objects;

  protected Storage()
  {
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.showProgressBar = false;
    this.faceTargetWhenWorking = true;
  }

  public bool ShouldOnlyTransferFromLowerPriority
  {
    get
    {
      if (!this.onlyTransferFromLowerPriority)
        return this.allowItemRemoval;
      return true;
    }
  }

  public GameObject this[int idx]
  {
    get
    {
      return this.items[idx];
    }
  }

  public int Count
  {
    get
    {
      return this.items.Count;
    }
  }

  public void SetDefaultStoredItemModifiers(List<Storage.StoredItemModifier> modifiers)
  {
    this.defaultStoredItemModifers = modifiers;
  }

  public PrioritySetting masterPriority
  {
    get
    {
      if ((bool) ((UnityEngine.Object) this.prioritizable))
        return this.prioritizable.GetMasterPriority();
      return Chore.DefaultPrioritySetting;
    }
  }

  public override Workable.AnimInfo GetAnim(Worker worker)
  {
    if (!this.useGunForDelivery || !worker.usesMultiTool)
      return base.GetAnim(worker);
    Workable.AnimInfo anim = base.GetAnim(worker);
    anim.smi = (StateMachine.Instance) new MultitoolController.Instance((Workable) this, worker, (HashedString) "store", Assets.GetPrefab((Tag) EffectConfigs.OreAbsorbId));
    return anim;
  }

  public event System.Action OnStorageIncreased;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Storage>(1623392196, Storage.OnDeathDelegate);
    this.Subscribe<Storage>(1502190696, Storage.OnQueueDestroyObjectDelegate);
    this.Subscribe<Storage>(-905833192, Storage.OnCopySettingsDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Storing;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = false;
    this.workingPstComplete = HashedString.Invalid;
    this.workingPstFailed = HashedString.Invalid;
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if (!this.allowSettingOnlyFetchMarkedItems)
      this.onlyFetchMarkedItems = false;
    this.UpdateFetchCategory();
  }

  protected override void OnSpawn()
  {
    this.SetWorkTime(1.5f);
    foreach (GameObject go in this.items)
    {
      this.ApplyStoredItemModifiers(go, true, true);
      if (this.sendOnStoreOnSpawn)
        go.Trigger(856640610, (object) this);
    }
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetSymbolVisiblity((KAnimHashedString) "sweep", this.onlyFetchMarkedItems);
    Prioritizable component2 = this.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.onPriorityChanged += new System.Action<PrioritySetting>(this.OnPriorityChanged);
    this.UpdateFetchCategory();
  }

  public GameObject Store(
    GameObject go,
    bool hide_popups = false,
    bool block_events = false,
    bool do_disease_transfer = true,
    bool is_deserializing = false)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return (GameObject) null;
    GameObject gameObject1 = go;
    Pickupable component = go.GetComponent<Pickupable>();
    if (!hide_popups && (UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
    {
      LocString locString;
      Transform transform;
      if (this.fxPrefix == Storage.FXPrefix.Delivered)
      {
        locString = UI.DELIVERED;
        transform = this.transform;
      }
      else
      {
        locString = UI.PICKEDUP;
        transform = go.transform;
      }
      string text = Assets.IsTagCountable(go.PrefabID()) ? string.Format((string) locString, (object) (int) component.TotalAmount, (object) go.GetProperName()) : string.Format((string) locString, (object) GameUtil.GetFormattedMass(component.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) go.GetProperName());
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, transform, 1.5f, false);
    }
    go.transform.parent = this.transform;
    Vector3 posCcc = Grid.CellToPosCCC(Grid.PosToCell((KMonoBehaviour) this), Grid.SceneLayer.Move);
    posCcc.z = go.transform.GetPosition().z;
    go.transform.SetPosition(posCcc);
    if (!block_events && do_disease_transfer)
      this.TransferDiseaseWithObject(go);
    if (!is_deserializing)
    {
      foreach (GameObject gameObject2 in this.items)
      {
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.prevent_absorb_until_stored)
            component.prevent_absorb_until_stored = false;
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && gameObject2.GetComponent<Pickupable>().TryAbsorb(component, hide_popups, true))
          {
            if (!block_events)
            {
              this.Trigger(-1697596308, (object) go);
              this.Trigger(-778359855, (object) null);
              if (this.OnStorageIncreased != null)
                this.OnStorageIncreased();
            }
            this.ApplyStoredItemModifiers(go, true, false);
            gameObject1 = gameObject2;
            go = (GameObject) null;
            break;
          }
        }
      }
    }
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
    {
      this.items.Add(go);
      if (!is_deserializing)
        this.ApplyStoredItemModifiers(go, true, false);
      if (!block_events)
      {
        go.Trigger(856640610, (object) this);
        this.Trigger(-1697596308, (object) go);
        this.Trigger(-778359855, (object) null);
        if (this.OnStorageIncreased != null)
          this.OnStorageIncreased();
      }
    }
    return gameObject1;
  }

  public PrimaryElement AddOre(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool keep_zero_mass = false,
    bool do_disease_transfer = true)
  {
    if ((double) mass <= 0.0)
      return (PrimaryElement) null;
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      primaryElement.Mass += mass;
      primaryElement.Temperature = finalTemperature;
      primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddOre");
      this.Trigger(-1697596308, (object) primaryElement.gameObject);
    }
    else
    {
      Element elementByHash = ElementLoader.FindElementByHash(element);
      GameObject go = elementByHash.substance.SpawnResource(this.transform.GetPosition(), mass, temperature, disease_idx, disease_count, true, false, true);
      go.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
      elementByHash.substance.ActivateSubstanceGameObject(go, disease_idx, disease_count);
      this.Store(go, true, false, do_disease_transfer, false);
    }
    return primaryElement;
  }

  public PrimaryElement AddLiquid(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool keep_zero_mass = false,
    bool do_disease_transfer = true)
  {
    if ((double) mass <= 0.0)
      return (PrimaryElement) null;
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      primaryElement.Mass += mass;
      primaryElement.Temperature = finalTemperature;
      primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddLiquid");
      this.Trigger(-1697596308, (object) primaryElement.gameObject);
    }
    else
    {
      SubstanceChunk chunk = LiquidSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, this.transform.GetPosition());
      primaryElement = chunk.GetComponent<PrimaryElement>();
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      this.Store(chunk.gameObject, true, false, do_disease_transfer, false);
    }
    return primaryElement;
  }

  public PrimaryElement AddGasChunk(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool keep_zero_mass,
    bool do_disease_transfer = true)
  {
    if ((double) mass <= 0.0)
      return (PrimaryElement) null;
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float mass1 = primaryElement.Mass;
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, mass1, temperature, mass);
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      primaryElement.SetMassTemperature(mass1 + mass, finalTemperature);
      primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddGasChunk");
      this.Trigger(-1697596308, (object) primaryElement.gameObject);
    }
    else
    {
      SubstanceChunk chunk = GasSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, this.transform.GetPosition());
      primaryElement = chunk.GetComponent<PrimaryElement>();
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      this.Store(chunk.gameObject, true, false, do_disease_transfer, false);
    }
    return primaryElement;
  }

  public void Transfer(Storage target, bool block_events = false, bool hide_popups = false)
  {
    while (this.items.Count > 0)
      this.Transfer(this.items[0], target, block_events, hide_popups);
  }

  public float Transfer(
    Storage dest_storage,
    Tag tag,
    float amount,
    bool block_events = false,
    bool hide_popups = false)
  {
    GameObject first = this.FindFirst(tag);
    if (!((UnityEngine.Object) first != (UnityEngine.Object) null))
      return 0.0f;
    PrimaryElement component1 = first.GetComponent<PrimaryElement>();
    if ((double) amount < (double) component1.Units)
    {
      Pickupable component2 = first.GetComponent<Pickupable>();
      Pickupable pickupable = component2.Take(amount);
      dest_storage.Store(pickupable.gameObject, hide_popups, block_events, true, false);
      if (!block_events)
        this.Trigger(-1697596308, (object) component2.gameObject);
    }
    else
    {
      this.Transfer(first, dest_storage, block_events, hide_popups);
      amount = component1.Units;
    }
    return amount;
  }

  public bool Transfer(GameObject go, Storage target, bool block_events = false, bool hide_popups = false)
  {
    this.items.RemoveAll((Predicate<GameObject>) (it => (UnityEngine.Object) it == (UnityEngine.Object) null));
    int count = this.items.Count;
    for (int index = 0; index < count; ++index)
    {
      if ((UnityEngine.Object) this.items[index] == (UnityEngine.Object) go)
      {
        this.items.RemoveAt(index);
        this.ApplyStoredItemModifiers(go, false, false);
        target.Store(go, hide_popups, block_events, true, false);
        if (!block_events)
          this.Trigger(-1697596308, (object) go);
        return true;
      }
    }
    return false;
  }

  public void DropAll(
    Vector3 position,
    bool vent_gas = false,
    bool dump_liquid = false,
    Vector3 offset = default (Vector3),
    bool do_disease_transfer = true)
  {
    while (this.items.Count > 0)
    {
      GameObject go = this.items[0];
      if (do_disease_transfer)
        this.TransferDiseaseWithObject(go);
      this.items.RemoveAt(0);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        bool flag = false;
        if (vent_gas || dump_liquid)
        {
          Dumpable component = go.GetComponent<Dumpable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            if (vent_gas && go.GetComponent<PrimaryElement>().Element.IsGas)
            {
              component.Dump(position + offset);
              flag = true;
            }
            if (dump_liquid && go.GetComponent<PrimaryElement>().Element.IsLiquid)
            {
              component.Dump(position + offset);
              flag = true;
            }
          }
        }
        if (!flag)
        {
          go.transform.SetPosition(position + offset);
          KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
          if ((bool) ((UnityEngine.Object) component))
            component.SetSceneLayer(Grid.SceneLayer.Ore);
          this.MakeWorldActive(go);
        }
      }
    }
  }

  public void DropAll(bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default (Vector3), bool do_disease_transfer = true)
  {
    while (this.items.Count > 0)
    {
      GameObject go = this.items[0];
      if (do_disease_transfer)
        this.TransferDiseaseWithObject(go);
      this.items.RemoveAt(0);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        bool flag = false;
        if (vent_gas || dump_liquid)
        {
          Dumpable component = go.GetComponent<Dumpable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            if (vent_gas && go.GetComponent<PrimaryElement>().Element.IsGas)
            {
              component.Dump();
              flag = true;
            }
            if (dump_liquid && go.GetComponent<PrimaryElement>().Element.IsLiquid)
            {
              component.Dump();
              flag = true;
            }
          }
        }
        if (!flag)
        {
          Vector3 position = Grid.CellToPosCCC(Grid.PosToCell((KMonoBehaviour) this), Grid.SceneLayer.Ore) + offset;
          go.transform.SetPosition(position);
          KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
          if ((bool) ((UnityEngine.Object) component))
            component.SetSceneLayer(Grid.SceneLayer.Ore);
          this.MakeWorldActive(go);
        }
      }
    }
  }

  public List<GameObject> Drop(Tag t)
  {
    ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
    this.Find(t, (List<GameObject>) pooledList);
    foreach (GameObject go in (List<GameObject>) pooledList)
      this.Drop(go, true);
    pooledList.Recycle();
    return (List<GameObject>) pooledList;
  }

  public GameObject Drop(GameObject go, bool do_disease_transfer = true)
  {
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
    {
      int count = this.items.Count;
      for (int index = 0; index < count; ++index)
      {
        if ((UnityEngine.Object) go == (UnityEngine.Object) this.items[index])
        {
          this.items[index] = this.items[count - 1];
          this.items.RemoveAt(count - 1);
          if (do_disease_transfer)
            this.TransferDiseaseWithObject(go);
          this.MakeWorldActive(go);
          break;
        }
      }
    }
    return go;
  }

  public void RenotifyAll()
  {
    this.items.RemoveAll((Predicate<GameObject>) (it => (UnityEngine.Object) it == (UnityEngine.Object) null));
    foreach (GameObject go in this.items)
      go.Trigger(856640610, (object) this);
  }

  private void TransferDiseaseWithObject(GameObject obj)
  {
    if ((UnityEngine.Object) obj == (UnityEngine.Object) null || !this.doDiseaseTransfer || (UnityEngine.Object) this.primaryElement == (UnityEngine.Object) null)
      return;
    PrimaryElement component = obj.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    SimUtil.DiseaseInfo invalid1 = SimUtil.DiseaseInfo.Invalid;
    invalid1.idx = component.DiseaseIdx;
    invalid1.count = (int) ((double) component.DiseaseCount * 0.0500000007450581);
    SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
    invalid2.idx = this.primaryElement.DiseaseIdx;
    invalid2.count = (int) ((double) this.primaryElement.DiseaseCount * 0.0500000007450581);
    component.ModifyDiseaseCount(-invalid1.count, "Storage.TransferDiseaseWithObject");
    this.primaryElement.ModifyDiseaseCount(-invalid2.count, "Storage.TransferDiseaseWithObject");
    if (invalid1.count > 0)
      this.primaryElement.AddDisease(invalid1.idx, invalid1.count, "Storage.TransferDiseaseWithObject");
    if (invalid2.count <= 0)
      return;
    component.AddDisease(invalid2.idx, invalid2.count, "Storage.TransferDiseaseWithObject");
  }

  private void MakeWorldActive(GameObject go)
  {
    go.transform.parent = (Transform) null;
    this.Trigger(-1697596308, (object) go);
    go.Trigger(856640610, (object) null);
    this.ApplyStoredItemModifiers(go, false, false);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.KeepZeroMassObject)
      return;
    component.KeepZeroMassObject = false;
    if ((double) component.Mass > 0.0)
      return;
    Util.KDestroyGameObject(go);
  }

  public List<GameObject> Find(Tag tag, List<GameObject> result)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
        result.Add(go);
    }
    return result;
  }

  public GameObject FindFirst(Tag tag)
  {
    GameObject gameObject = (GameObject) null;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
      {
        gameObject = go;
        break;
      }
    }
    return gameObject;
  }

  public PrimaryElement FindFirstWithMass(Tag tag)
  {
    PrimaryElement primaryElement = (PrimaryElement) null;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
      {
        PrimaryElement component = go.GetComponent<PrimaryElement>();
        if ((double) component.Mass > 0.0)
        {
          primaryElement = component;
          break;
        }
      }
    }
    return primaryElement;
  }

  public List<Tag> GetAllTagsInStorage()
  {
    List<Tag> tagList = new List<Tag>();
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (!tagList.Contains(go.PrefabID()))
        tagList.Add(go.PrefabID());
    }
    return tagList;
  }

  public GameObject Find(int ID)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (ID == go.PrefabID().GetHashCode())
        return go;
    }
    return (GameObject) null;
  }

  public void ConsumeAllIgnoringDisease()
  {
    for (int index = this.items.Count - 1; index >= 0; --index)
      this.ConsumeIgnoringDisease(this.items[index]);
  }

  public void ConsumeAndGetDisease(
    Tag tag,
    float amount,
    out SimUtil.DiseaseInfo disease_info,
    out float aggregate_temperature)
  {
    DebugUtil.Assert(tag.IsValid);
    disease_info = SimUtil.DiseaseInfo.Invalid;
    aggregate_temperature = 0.0f;
    float mass1 = 0.0f;
    bool flag = false;
    for (int index = 0; index < this.items.Count && (double) amount > 0.0; ++index)
    {
      GameObject go = this.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
      {
        PrimaryElement component = go.GetComponent<PrimaryElement>();
        if ((double) component.Units > 0.0)
        {
          flag = true;
          float mass2 = Math.Min(component.Units, amount);
          Debug.Assert((double) mass2 > 0.0, (object) "Delta amount was zero, which should be impossible.");
          aggregate_temperature = SimUtil.CalculateFinalTemperature(mass1, aggregate_temperature, mass2, component.Temperature);
          SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(component, mass2 / component.Units);
          disease_info = SimUtil.CalculateFinalDiseaseInfo(disease_info, percentOfDisease);
          component.Units -= mass2;
          component.ModifyDiseaseCount(-percentOfDisease.count, "Storage.ConsumeAndGetDisease");
          amount -= mass2;
          mass1 += mass2;
        }
        if ((double) component.Units <= 0.0 && !component.KeepZeroMassObject)
        {
          if (this.deleted_objects == null)
            this.deleted_objects = new List<GameObject>();
          this.deleted_objects.Add(go);
        }
        this.Trigger(-1697596308, (object) go);
      }
    }
    if (!flag)
      aggregate_temperature = this.GetComponent<PrimaryElement>().Temperature;
    if (this.deleted_objects == null)
      return;
    for (int index = 0; index < this.deleted_objects.Count; ++index)
    {
      this.items.Remove(this.deleted_objects[index]);
      Util.KDestroyGameObject(this.deleted_objects[index]);
    }
    this.deleted_objects.Clear();
  }

  public void ConsumeAndGetDisease(
    Recipe.Ingredient ingredient,
    out SimUtil.DiseaseInfo disease_info,
    out float temperature)
  {
    this.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out disease_info, out temperature);
  }

  public void ConsumeIgnoringDisease(Tag tag, float amount)
  {
    SimUtil.DiseaseInfo disease_info;
    float aggregate_temperature;
    this.ConsumeAndGetDisease(tag, amount, out disease_info, out aggregate_temperature);
  }

  public void ConsumeIgnoringDisease(GameObject item_go)
  {
    if (!this.items.Contains(item_go))
      return;
    PrimaryElement component = item_go.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.KeepZeroMassObject)
    {
      component.Units = 0.0f;
      component.ModifyDiseaseCount(-component.DiseaseCount, "consume item");
      this.Trigger(-1697596308, (object) item_go);
    }
    else
    {
      this.items.Remove(item_go);
      this.Trigger(-1697596308, (object) item_go);
      item_go.DeleteObject();
    }
  }

  public GameObject Drop(int ID)
  {
    return this.Drop(this.Find(ID), true);
  }

  private void OnDeath(object data)
  {
    this.DropAll(true, true, new Vector3(), true);
  }

  public bool IsFull()
  {
    return (double) this.RemainingCapacity() <= 0.0;
  }

  public bool IsEmpty()
  {
    return this.items.Count == 0;
  }

  public float Capacity()
  {
    return this.capacityKg;
  }

  public bool IsEndOfLife()
  {
    return this.endOfLife;
  }

  public float MassStored()
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (!((UnityEngine.Object) this.items[index] == (UnityEngine.Object) null))
      {
        PrimaryElement component = this.items[index].GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          num += component.Units * component.MassPerUnit;
      }
    }
    return (float) Mathf.RoundToInt(num * 1000f) / 1000f;
  }

  public float UnitsStored()
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (!((UnityEngine.Object) this.items[index] == (UnityEngine.Object) null))
      {
        PrimaryElement component = this.items[index].GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          num += component.Units;
      }
    }
    return (float) Mathf.RoundToInt(num * 1000f) / 1000f;
  }

  public bool Has(Tag tag)
  {
    bool flag = false;
    foreach (GameObject gameObject in this.items)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      if (component.HasTag(tag) && (double) component.Mass > 0.0)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public PrimaryElement AddToPrimaryElement(
    SimHashes element,
    float additional_mass,
    float temperature)
  {
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, additional_mass);
      primaryElement.Mass += additional_mass;
      primaryElement.Temperature = finalTemperature;
    }
    return primaryElement;
  }

  public PrimaryElement FindPrimaryElement(SimHashes element)
  {
    PrimaryElement primaryElement = (PrimaryElement) null;
    foreach (GameObject gameObject in this.items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if (component.ElementID == element)
        {
          primaryElement = component;
          break;
        }
      }
    }
    return primaryElement;
  }

  public float RemainingCapacity()
  {
    return this.capacityKg - this.MassStored();
  }

  public bool GetOnlyFetchMarkedItems()
  {
    return this.onlyFetchMarkedItems;
  }

  public void SetOnlyFetchMarkedItems(bool is_set)
  {
    if (is_set == this.onlyFetchMarkedItems)
      return;
    this.onlyFetchMarkedItems = is_set;
    this.UpdateFetchCategory();
    this.Trigger(644822890, (object) null);
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "sweep", is_set);
  }

  private void UpdateFetchCategory()
  {
    if (this.fetchCategory == Storage.FetchCategory.Building)
      return;
    this.fetchCategory = !this.onlyFetchMarkedItems ? Storage.FetchCategory.GeneralStorage : Storage.FetchCategory.StorageSweepOnly;
  }

  protected override void OnCleanUp()
  {
    if (this.items.Count != 0)
      Debug.LogWarning((object) ("Storage for [" + this.gameObject.name + "] is being destroyed but it still contains items!"), (UnityEngine.Object) this.gameObject);
    base.OnCleanUp();
  }

  private void OnQueueDestroyObject(object data)
  {
    this.endOfLife = true;
    this.DropAll(true, false, new Vector3(), true);
    this.OnCleanUp();
  }

  public void Remove(GameObject go, bool do_disease_transfer = true)
  {
    this.items.Remove(go);
    if (do_disease_transfer)
      this.TransferDiseaseWithObject(go);
    this.Trigger(-1697596308, (object) go);
    this.ApplyStoredItemModifiers(go, false, false);
  }

  public bool ForceStore(Tag tag, float amount)
  {
    Debug.Assert((double) amount < (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT);
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
      {
        go.GetComponent<PrimaryElement>().Mass += amount;
        return true;
      }
    }
    return false;
  }

  public float GetAmountAvailable(Tag tag)
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
        num += go.GetComponent<PrimaryElement>().Units;
    }
    return num;
  }

  public float GetUnitsAvailable(Tag tag)
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
        num += go.GetComponent<PrimaryElement>().Units;
    }
    return num;
  }

  public float GetMassAvailable(Tag tag)
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
        num += go.GetComponent<PrimaryElement>().Mass;
    }
    return num;
  }

  public float GetMassAvailable(SimHashes element)
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject gameObject = this.items[index];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if (component.ElementID == element)
          num += component.Mass;
      }
    }
    return num;
  }

  public bool IsMaterialOnStorage(Tag tag, ref float amount)
  {
    foreach (GameObject gameObject in this.items)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Pickupable component = gameObject.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.GetComponent<KPrefabID>().HasTag(tag))
        {
          amount = component.TotalAmount;
          return true;
        }
      }
    }
    return false;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.showDescriptor)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public static void MakeItemTemperatureInsulated(
    GameObject go,
    bool is_stored,
    bool is_initializing)
  {
    SimTemperatureTransfer component = go.GetComponent<SimTemperatureTransfer>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.enabled = !is_stored;
  }

  public static void MakeItemInvisible(GameObject go, bool is_stored, bool is_initializing)
  {
    if (is_initializing)
      return;
    bool flag = !is_stored;
    KAnimControllerBase component1 = go.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.enabled != flag)
      component1.enabled = flag;
    KSelectable component2 = go.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || component2.enabled == flag)
      return;
    component2.enabled = flag;
  }

  public static void MakeItemSealed(GameObject go, bool is_stored, bool is_initializing)
  {
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    if (is_stored)
      go.GetComponent<KPrefabID>().AddTag(GameTags.Sealed, false);
    else
      go.GetComponent<KPrefabID>().RemoveTag(GameTags.Sealed);
  }

  public static void MakeItemPreserved(GameObject go, bool is_stored, bool is_initializing)
  {
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    if (is_stored)
      go.GetComponent<KPrefabID>().AddTag(GameTags.Preserved, false);
    else
      go.GetComponent<KPrefabID>().RemoveTag(GameTags.Preserved);
  }

  private void ApplyStoredItemModifiers(GameObject go, bool is_stored, bool is_initializing)
  {
    List<Storage.StoredItemModifier> storedItemModifers = this.defaultStoredItemModifers;
    for (int index1 = 0; index1 < storedItemModifers.Count; ++index1)
    {
      Storage.StoredItemModifier storedItemModifier = storedItemModifers[index1];
      for (int index2 = 0; index2 < Storage.StoredItemModifierHandlers.Count; ++index2)
      {
        Storage.StoredItemModifierInfo itemModifierHandler = Storage.StoredItemModifierHandlers[index2];
        if (itemModifierHandler.modifier == storedItemModifier)
        {
          itemModifierHandler.toggleState(go, is_stored, is_initializing);
          break;
        }
      }
    }
  }

  private void OnCopySettings(object data)
  {
    Storage component = ((GameObject) data).GetComponent<Storage>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.SetOnlyFetchMarkedItems(component.onlyFetchMarkedItems);
  }

  private void OnPriorityChanged(PrioritySetting priority)
  {
    foreach (GameObject go in this.items)
      go.Trigger(-1626373771, (object) this);
  }

  private bool ShouldSaveItem(GameObject go)
  {
    bool flag = false;
    if ((UnityEngine.Object) go != (UnityEngine.Object) null && (UnityEngine.Object) go.GetComponent<SaveLoadRoot>() != (UnityEngine.Object) null && (double) go.GetComponent<PrimaryElement>().Mass > 0.0)
      flag = true;
    return flag;
  }

  public void Serialize(BinaryWriter writer)
  {
    int num = 0;
    int count = this.items.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.ShouldSaveItem(this.items[index]))
        ++num;
    }
    writer.Write(num);
    if (num == 0 || this.items == null || this.items.Count <= 0)
      return;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (this.ShouldSaveItem(go))
      {
        SaveLoadRoot component = go.GetComponent<SaveLoadRoot>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          string name = go.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
          writer.WriteKleiString(name);
          component.Save(writer);
        }
        else
          Debug.Log((object) "Tried to save obj in storage but obj has no SaveLoadRoot", (UnityEngine.Object) go);
      }
    }
  }

  public void Deserialize(IReader reader)
  {
    float realtimeSinceStartup1 = Time.realtimeSinceStartup;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    this.ClearItems();
    int capacity = reader.ReadInt32();
    this.items = new List<GameObject>(capacity);
    for (int index = 0; index < capacity; ++index)
    {
      float realtimeSinceStartup2 = Time.realtimeSinceStartup;
      Tag tag = TagManager.Create(reader.ReadKleiString());
      SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
      num1 += Time.realtimeSinceStartup - realtimeSinceStartup2;
      if ((UnityEngine.Object) saveLoadRoot != (UnityEngine.Object) null)
      {
        KBatchedAnimController component = saveLoadRoot.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.enabled = false;
        saveLoadRoot.SetRegistered(false);
        float realtimeSinceStartup3 = Time.realtimeSinceStartup;
        GameObject gameObject = this.Store(saveLoadRoot.gameObject, true, true, false, true);
        num2 += Time.realtimeSinceStartup - realtimeSinceStartup3;
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          float realtimeSinceStartup4 = Time.realtimeSinceStartup;
          gameObject.GetComponent<Pickupable>().OnStore((object) this);
          num3 += Time.realtimeSinceStartup - realtimeSinceStartup4;
          if (this.dropOnLoad)
            this.Drop(saveLoadRoot.gameObject, true);
        }
      }
      else
        Debug.LogWarning((object) ("Tried to deserialize " + tag.ToString() + " into storage but failed"), (UnityEngine.Object) this.gameObject);
    }
  }

  private void ClearItems()
  {
    foreach (GameObject go in this.items)
      go.DeleteObject();
    this.items.Clear();
  }

  static Storage()
  {
    List<Storage.StoredItemModifierInfo> itemModifierInfoList1 = new List<Storage.StoredItemModifierInfo>();
    List<Storage.StoredItemModifierInfo> itemModifierInfoList2 = itemModifierInfoList1;
    // ISSUE: reference to a compiler-generated field
    if (Storage.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Storage.\u003C\u003Ef__mg\u0024cache0 = new System.Action<GameObject, bool, bool>(Storage.MakeItemInvisible);
    }
    // ISSUE: reference to a compiler-generated field
    Storage.StoredItemModifierInfo itemModifierInfo1 = new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Hide, Storage.\u003C\u003Ef__mg\u0024cache0);
    itemModifierInfoList2.Add(itemModifierInfo1);
    List<Storage.StoredItemModifierInfo> itemModifierInfoList3 = itemModifierInfoList1;
    // ISSUE: reference to a compiler-generated field
    if (Storage.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Storage.\u003C\u003Ef__mg\u0024cache1 = new System.Action<GameObject, bool, bool>(Storage.MakeItemTemperatureInsulated);
    }
    // ISSUE: reference to a compiler-generated field
    Storage.StoredItemModifierInfo itemModifierInfo2 = new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Insulate, Storage.\u003C\u003Ef__mg\u0024cache1);
    itemModifierInfoList3.Add(itemModifierInfo2);
    List<Storage.StoredItemModifierInfo> itemModifierInfoList4 = itemModifierInfoList1;
    // ISSUE: reference to a compiler-generated field
    if (Storage.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Storage.\u003C\u003Ef__mg\u0024cache2 = new System.Action<GameObject, bool, bool>(Storage.MakeItemSealed);
    }
    // ISSUE: reference to a compiler-generated field
    Storage.StoredItemModifierInfo itemModifierInfo3 = new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Seal, Storage.\u003C\u003Ef__mg\u0024cache2);
    itemModifierInfoList4.Add(itemModifierInfo3);
    List<Storage.StoredItemModifierInfo> itemModifierInfoList5 = itemModifierInfoList1;
    // ISSUE: reference to a compiler-generated field
    if (Storage.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Storage.\u003C\u003Ef__mg\u0024cache3 = new System.Action<GameObject, bool, bool>(Storage.MakeItemPreserved);
    }
    // ISSUE: reference to a compiler-generated field
    Storage.StoredItemModifierInfo itemModifierInfo4 = new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Preserve, Storage.\u003C\u003Ef__mg\u0024cache3);
    itemModifierInfoList5.Add(itemModifierInfo4);
    Storage.StoredItemModifierHandlers = itemModifierInfoList1;
    Storage.StandardSealedStorage = new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal
    };
    Storage.StandardFabricatorStorage = new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Preserve
    };
    Storage.StandardInsulatedStorage = new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    };
    Storage.OnDeathDelegate = new EventSystem.IntraObjectHandler<Storage>((System.Action<Storage, object>) ((component, data) => component.OnDeath(data)));
    Storage.OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<Storage>((System.Action<Storage, object>) ((component, data) => component.OnQueueDestroyObject(data)));
    Storage.OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Storage>((System.Action<Storage, object>) ((component, data) => component.OnCopySettings(data)));
  }

  public enum StoredItemModifier
  {
    Insulate,
    Hide,
    Seal,
    Preserve,
  }

  public enum FetchCategory
  {
    Building,
    GeneralStorage,
    StorageSweepOnly,
  }

  public enum FXPrefix
  {
    Delivered,
    PickedUp,
  }

  private struct StoredItemModifierInfo
  {
    public Storage.StoredItemModifier modifier;
    public System.Action<GameObject, bool, bool> toggleState;

    public StoredItemModifierInfo(
      Storage.StoredItemModifier modifier,
      System.Action<GameObject, bool, bool> toggle_state)
    {
      this.modifier = modifier;
      this.toggleState = toggle_state;
    }
  }
}
