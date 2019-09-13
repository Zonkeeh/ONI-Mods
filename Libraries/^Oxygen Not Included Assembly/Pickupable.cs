// Decompiled with JetBrains decompiler
// Type: Pickupable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pickupable : Workable, IHasSortOrder
{
  private static CellOffset[] displacementOffsets = new CellOffset[8]
  {
    new CellOffset(0, 1),
    new CellOffset(0, -1),
    new CellOffset(1, 0),
    new CellOffset(-1, 0),
    new CellOffset(1, 1),
    new CellOffset(1, -1),
    new CellOffset(-1, 1),
    new CellOffset(-1, -1)
  };
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Pickupable>((System.Action<Pickupable, object>) ((component, data) => component.OnStore(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnLandedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((System.Action<Pickupable, object>) ((component, data) => component.OnLanded(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnOreSizeChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((System.Action<Pickupable, object>) ((component, data) => component.OnOreSizeChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((System.Action<Pickupable, object>) ((component, data) => component.OnReachableChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Pickupable>((System.Action<Pickupable, object>) ((component, data) => component.RefreshStorageTags(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((System.Action<Pickupable, object>) ((component, data) => component.OnTagsChanged(data)));
  public Func<Pickupable, bool> CanAbsorb = (Func<Pickupable, bool>) (other => false);
  public bool useGunforPickup = true;
  public bool trackOnPickup = true;
  private List<Pickupable.Reservation> reservations = new List<Pickupable.Reservation>();
  private int entombedCell = -1;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  public const float WorkTime = 1.5f;
  [MyCmpReq]
  [NonSerialized]
  public KPrefabID KPrefabID;
  [MyCmpAdd]
  [NonSerialized]
  public Clearable Clearable;
  [MyCmpAdd]
  [NonSerialized]
  public Prioritizable prioritizable;
  public bool absorbable;
  public Func<float, Pickupable> OnTake;
  public System.Action OnReservationsChanged;
  public ObjectLayerListItem objectLayerListItem;
  public Workable targetWorkable;
  public KAnimFile carryAnimOverride;
  private KBatchedAnimController lastCarrier;
  private bool isReachable;
  private bool isEntombed;
  private bool cleaningUp;
  private int nextTicketNumber;
  private HandleVector<int>.Handle solidPartitionerEntry;
  private HandleVector<int>.Handle partitionerEntry;
  private LoggerFSSF log;

  private Pickupable()
  {
    this.showProgressBar = false;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.shouldTransferDiseaseWithWorker = false;
  }

  public PrimaryElement PrimaryElement
  {
    get
    {
      return this.primaryElement;
    }
  }

  public int sortOrder { get; set; }

  public Storage storage { get; set; }

  public float MinTakeAmount
  {
    get
    {
      return 0.0f;
    }
  }

  public bool prevent_absorb_until_stored { get; set; }

  public bool isKinematic { get; set; }

  public bool wasAbsorbed { get; private set; }

  public int cachedCell { get; private set; }

  public int storageCell
  {
    get
    {
      if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
        return Grid.PosToCell((KMonoBehaviour) this.storage);
      return this.cachedCell;
    }
  }

  public bool IsEntombed
  {
    get
    {
      return this.isEntombed;
    }
    set
    {
      if (value == this.isEntombed)
        return;
      this.isEntombed = value;
      if (this.isEntombed)
        this.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
      else
        this.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
      this.Trigger(-1089732772, (object) null);
      this.UpdateEntombedVisualizer();
    }
  }

  private bool CouldBePickedUpCommon(GameObject carrier)
  {
    if ((double) this.UnreservedAmount < (double) this.MinTakeAmount)
      return false;
    if ((double) this.UnreservedAmount <= 0.0)
      return (double) this.FindReservedAmount(carrier) > 0.0;
    return true;
  }

  public bool CouldBePickedUpByMinion(GameObject carrier)
  {
    if (!this.CouldBePickedUpCommon(carrier))
      return false;
    if (!((UnityEngine.Object) this.storage == (UnityEngine.Object) null) && (bool) ((UnityEngine.Object) this.storage.automatable))
      return !this.storage.automatable.GetAutomationOnly();
    return true;
  }

  public bool CouldBePickedUpByTransferArm(GameObject carrier)
  {
    return this.CouldBePickedUpCommon(carrier);
  }

  public float FindReservedAmount(GameObject reserver)
  {
    for (int index = 0; index < this.reservations.Count; ++index)
    {
      if ((UnityEngine.Object) this.reservations[index].reserver == (UnityEngine.Object) reserver)
        return this.reservations[index].amount;
    }
    return 0.0f;
  }

  public float UnreservedAmount
  {
    get
    {
      return this.TotalAmount - this.ReservedAmount;
    }
  }

  public float ReservedAmount { get; private set; }

  public float TotalAmount
  {
    get
    {
      return this.primaryElement.Units;
    }
    set
    {
      DebugUtil.Assert((UnityEngine.Object) this.primaryElement != (UnityEngine.Object) null);
      this.primaryElement.Units = value;
      if ((double) value < (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.GetComponent<PrimaryElement>().KeepZeroMassObject)
        this.gameObject.DeleteObject();
      this.NotifyChanged(Grid.PosToCell((KMonoBehaviour) this));
    }
  }

  private void RefreshReservedAmount()
  {
    this.ReservedAmount = 0.0f;
    for (int index = 0; index < this.reservations.Count; ++index)
      this.ReservedAmount += this.reservations[index].amount;
  }

  [Conditional("UNITY_EDITOR")]
  private void Log(string evt, string param, float value)
  {
  }

  public void ClearReservations()
  {
    this.reservations.Clear();
    this.RefreshReservedAmount();
  }

  [ContextMenu("Print Reservations")]
  public void PrintReservations()
  {
    foreach (Pickupable.Reservation reservation in this.reservations)
      Debug.Log((object) reservation.ToString());
  }

  public int Reserve(string context, GameObject reserver, float amount)
  {
    int ticket = this.nextTicketNumber++;
    this.reservations.Add(new Pickupable.Reservation(reserver, amount, ticket));
    this.RefreshReservedAmount();
    if (this.OnReservationsChanged != null)
      this.OnReservationsChanged();
    return ticket;
  }

  public void Unreserve(string context, int ticket)
  {
    for (int index = 0; index < this.reservations.Count; ++index)
    {
      if (this.reservations[index].ticket == ticket)
      {
        this.reservations.RemoveAt(index);
        this.RefreshReservedAmount();
        if (this.OnReservationsChanged == null)
          break;
        this.OnReservationsChanged();
        break;
      }
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workingPstComplete = HashedString.Invalid;
    this.workingPstFailed = HashedString.Invalid;
    this.log = new LoggerFSSF(nameof (Pickupable));
    this.workerStatusItem = Db.Get().DuplicantStatusItems.PickingUp;
    this.SetWorkTime(1.5f);
    this.targetWorkable = (Workable) this;
    this.resetProgressOnStop = true;
    this.gameObject.layer = Game.PickupableLayer;
    this.UpdateCachedCell(Grid.PosToCell(this.transform.GetPosition()));
    this.Subscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
    this.Subscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
    this.Subscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
    this.Subscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
    this.Subscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
    this.KPrefabID.AddTag(GameTags.Pickupable, false);
    Components.Pickupables.Add(this);
  }

  protected override void OnLoadLevel()
  {
    base.OnLoadLevel();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(cell))
    {
      this.gameObject.DeleteObject();
    }
    else
    {
      this.UpdateCachedCell(cell);
      new ReachabilityMonitor.Instance((Workable) this).StartSM();
      new FetchableMonitor.Instance(this).StartSM();
      this.SetWorkTime(1.5f);
      this.faceTargetWhenWorking = true;
      KSelectable component1 = this.GetComponent<KSelectable>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetStatusIndicatorOffset(new Vector3(0.0f, -0.65f, 0.0f));
      this.OnTagsChanged((object) null);
      this.TryToOffsetIfBuried();
      DecorProvider component2 = this.GetComponent<DecorProvider>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && string.IsNullOrEmpty(component2.overrideName))
        component2.overrideName = (string) UI.OVERLAYS.DECOR.CLUTTER;
      this.UpdateEntombedVisualizer();
      this.Subscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
    }
  }

  public void RegisterListeners()
  {
    if (this.cleaningUp || this.solidPartitionerEntry.IsValid())
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    this.objectLayerListItem = new ObjectLayerListItem(this.gameObject, ObjectLayer.Pickupables, cell);
    this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterSolidListener", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterPickupable", (object) this, cell, GameScenePartitioner.Instance.pickupablesLayer, (System.Action<object>) null);
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "Pickupable.OnCellChange");
    Singleton<CellChangeMonitor>.Instance.MarkDirty(this.transform);
  }

  public void UnregisterListeners()
  {
    if (this.objectLayerListItem != null)
    {
      this.objectLayerListItem.Clear();
      this.objectLayerListItem = (ObjectLayerListItem) null;
    }
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
  }

  private void OnSolidChanged(object data)
  {
    this.TryToOffsetIfBuried();
  }

  public void TryToOffsetIfBuried()
  {
    if (this.KPrefabID.HasTag(GameTags.Stored) || this.KPrefabID.HasTag(GameTags.Equipped))
      return;
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(cell1))
      return;
    DeathMonitor.Instance smi = this.gameObject.GetSMI<DeathMonitor.Instance>();
    if ((smi == null || smi.IsDead()) && (Grid.Solid[cell1] && Grid.Foundation[cell1] || Grid.Properties[cell1] != (byte) 0))
    {
      for (int index = 0; index < Pickupable.displacementOffsets.Length; ++index)
      {
        int cell2 = Grid.OffsetCell(cell1, Pickupable.displacementOffsets[index]);
        if (Grid.IsValidCell(cell2) && !Grid.Solid[cell2])
        {
          Vector3 posCbc = Grid.CellToPosCBC(cell2, Grid.SceneLayer.Move);
          KCollider2D component = this.GetComponent<KCollider2D>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            posCbc.y += this.transform.GetPosition().y - component.bounds.min.y;
          this.transform.SetPosition(posCbc);
          cell1 = cell2;
          this.RemoveFaller();
          this.AddFaller(Vector2.zero);
          break;
        }
      }
    }
    this.HandleSolidCell(cell1);
  }

  private bool HandleSolidCell(int cell)
  {
    bool isEntombed = this.IsEntombed;
    bool flag = false;
    if (Grid.IsValidCell(cell) && Grid.Solid[cell])
    {
      DeathMonitor.Instance smi = this.gameObject.GetSMI<DeathMonitor.Instance>();
      if (smi == null || smi.IsDead())
      {
        this.Clearable.CancelClearing();
        flag = true;
      }
    }
    if (flag != isEntombed && !this.KPrefabID.HasTag(GameTags.Stored))
    {
      this.IsEntombed = flag;
      this.GetComponent<KSelectable>().IsSelectable = !this.IsEntombed;
    }
    this.UpdateEntombedVisualizer();
    return this.IsEntombed;
  }

  private void OnCellChange()
  {
    Vector3 position = this.transform.GetPosition();
    int cell1 = Grid.PosToCell(position);
    if (!Grid.IsValidCell(cell1))
    {
      Vector2 vector2_1 = new Vector2(-0.1f * (float) Grid.WidthInCells, 1.1f * (float) Grid.WidthInCells);
      Vector2 vector2_2 = new Vector2(-0.1f * (float) Grid.HeightInCells, 1.1f * (float) Grid.HeightInCells);
      if ((double) position.x >= (double) vector2_1.x && (double) vector2_1.y >= (double) position.x && ((double) position.y >= (double) vector2_2.x && (double) vector2_2.y >= (double) position.y))
        return;
      this.DeleteObject();
    }
    else
    {
      this.ReleaseEntombedVisualizerAndAddFaller(true);
      if (this.HandleSolidCell(cell1))
        return;
      this.objectLayerListItem.Update(cell1);
      bool flag = false;
      if (this.absorbable && !this.KPrefabID.HasTag(GameTags.Stored))
      {
        int cell2 = Grid.CellBelow(cell1);
        if (Grid.IsValidCell(cell2) && Grid.Solid[cell2])
        {
          ObjectLayerListItem nextItem = this.objectLayerListItem.nextItem;
          while (nextItem != null)
          {
            GameObject gameObject = nextItem.gameObject;
            nextItem = nextItem.nextItem;
            Pickupable component = gameObject.GetComponent<Pickupable>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              flag = component.TryAbsorb(this, false, false);
              if (flag)
                break;
            }
          }
        }
      }
      GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, cell1);
      GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, cell1);
      int cachedCell = this.cachedCell;
      this.UpdateCachedCell(cell1);
      if (!flag)
        this.NotifyChanged(cell1);
      if (!Grid.IsValidCell(cachedCell) || cell1 == cachedCell)
        return;
      this.NotifyChanged(cachedCell);
    }
  }

  private void OnTagsChanged(object data)
  {
    if (!this.KPrefabID.HasTag(GameTags.Stored) && !this.KPrefabID.HasTag(GameTags.Equipped))
    {
      this.RegisterListeners();
      this.AddFaller(Vector2.zero);
    }
    else
    {
      this.UnregisterListeners();
      this.RemoveFaller();
    }
  }

  private void NotifyChanged(int new_cell)
  {
    GameScenePartitioner.Instance.TriggerEvent(new_cell, GameScenePartitioner.Instance.pickupablesChangedLayer, (object) this);
  }

  public bool TryAbsorb(Pickupable other, bool hide_effects, bool allow_cross_storage = false)
  {
    if ((UnityEngine.Object) other == (UnityEngine.Object) null || other.wasAbsorbed || (this.wasAbsorbed || !other.CanAbsorb(this)) || (this.prevent_absorb_until_stored || !allow_cross_storage && (UnityEngine.Object) this.storage == (UnityEngine.Object) null != ((UnityEngine.Object) other.storage == (UnityEngine.Object) null)))
      return false;
    this.Absorb(other);
    if (!hide_effects && (UnityEngine.Object) EffectPrefabs.Instance != (UnityEngine.Object) null)
    {
      Vector3 position = this.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
      Util.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.OreAbsorbId), position, Quaternion.identity, (GameObject) null, (string) null, true, 0).SetActive(true);
    }
    return true;
  }

  protected override void OnCleanUp()
  {
    this.cleaningUp = true;
    this.ReleaseEntombedVisualizerAndAddFaller(false);
    this.RemoveFaller();
    if ((bool) ((UnityEngine.Object) this.storage))
      this.storage.Remove(this.gameObject, true);
    this.UnregisterListeners();
    Components.Pickupables.Remove(this);
    if (this.reservations.Count > 0)
    {
      this.reservations.Clear();
      if (this.OnReservationsChanged != null)
        this.OnReservationsChanged();
    }
    if (Grid.IsValidCell(this.cachedCell))
      this.NotifyChanged(this.cachedCell);
    base.OnCleanUp();
  }

  public Pickupable Take(float amount)
  {
    if ((double) amount <= 0.0)
      return (Pickupable) null;
    if (this.OnTake != null)
    {
      if ((double) amount >= (double) this.TotalAmount && (UnityEngine.Object) this.storage != (UnityEngine.Object) null)
        this.storage.Remove(this.gameObject, true);
      float num = Math.Min(this.TotalAmount, amount);
      if ((double) num <= 0.0)
        return (Pickupable) null;
      return this.OnTake(num);
    }
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
      this.storage.Remove(this.gameObject, true);
    return this;
  }

  private void Absorb(Pickupable pickupable)
  {
    Debug.Assert(!this.wasAbsorbed);
    Debug.Assert(!pickupable.wasAbsorbed);
    this.Trigger(-2064133523, (object) pickupable);
    pickupable.Trigger(-1940207677, (object) this.gameObject);
    pickupable.wasAbsorbed = true;
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) pickupable.GetComponent<KSelectable>())
      SelectTool.Instance.Select(component, false);
    pickupable.gameObject.DeleteObject();
    this.NotifyChanged(Grid.PosToCell((KMonoBehaviour) this));
  }

  private void RefreshStorageTags(object data = null)
  {
    if (data is Storage || data != null && (bool) data)
    {
      this.KPrefabID.AddTag(GameTags.Stored, false);
      if (this.storage == null || !this.storage.allowItemRemoval)
        this.KPrefabID.AddTag(GameTags.StoredPrivate, false);
      else
        this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
    }
    else
    {
      this.KPrefabID.RemoveTag(GameTags.Stored);
      this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
    }
  }

  public void OnStore(object data)
  {
    this.storage = data as Storage;
    bool flag = data is Storage || data != null && (bool) data;
    SaveLoadRoot component1 = this.GetComponent<SaveLoadRoot>();
    if ((UnityEngine.Object) this.carryAnimOverride != (UnityEngine.Object) null && (UnityEngine.Object) this.lastCarrier != (UnityEngine.Object) null)
    {
      this.lastCarrier.RemoveAnimOverrides(this.carryAnimOverride);
      this.lastCarrier = (KBatchedAnimController) null;
    }
    KSelectable component2 = this.GetComponent<KSelectable>();
    if ((bool) ((UnityEngine.Object) component2))
      component2.IsSelectable = !flag;
    if (flag)
    {
      int cachedCell = this.cachedCell;
      this.RefreshStorageTags(data);
      if (this.storage != null)
      {
        if ((UnityEngine.Object) this.carryAnimOverride != (UnityEngine.Object) null && (UnityEngine.Object) this.storage.GetComponent<Navigator>() != (UnityEngine.Object) null)
        {
          this.lastCarrier = this.storage.GetComponent<KBatchedAnimController>();
          if ((UnityEngine.Object) this.lastCarrier != (UnityEngine.Object) null)
            this.lastCarrier.AddAnimOverrides(this.carryAnimOverride, 0.0f);
        }
        this.UpdateCachedCell(Grid.PosToCell((KMonoBehaviour) this.storage));
      }
      this.NotifyChanged(cachedCell);
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
        return;
      component1.SetRegistered(false);
    }
    else
    {
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetRegistered(true);
      this.RemovedFromStorage();
    }
  }

  private void RemovedFromStorage()
  {
    this.storage = (Storage) null;
    this.UpdateCachedCell(Grid.PosToCell((KMonoBehaviour) this));
    this.RefreshStorageTags((object) null);
    this.AddFaller(Vector2.zero);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.enabled = true;
    this.gameObject.transform.rotation = Quaternion.identity;
    this.RegisterListeners();
    component.GetBatchInstanceData().ClearOverrideTransformMatrix();
  }

  private void UpdateCachedCell(int cell)
  {
    this.cachedCell = cell;
    this.GetOffsets(this.cachedCell);
  }

  public override Workable.AnimInfo GetAnim(Worker worker)
  {
    if (!this.useGunforPickup || !worker.usesMultiTool)
      return base.GetAnim(worker);
    Workable.AnimInfo anim = base.GetAnim(worker);
    anim.smi = (StateMachine.Instance) new MultitoolController.Instance((Workable) this, worker, (HashedString) "pickup", Assets.GetPrefab((Tag) EffectConfigs.OreAbsorbId));
    return anim;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Storage component = worker.GetComponent<Storage>();
    Pickupable.PickupableStartWorkInfo startWorkInfo = (Pickupable.PickupableStartWorkInfo) worker.startWorkInfo;
    float amount = startWorkInfo.amount;
    Pickupable pickupable1 = this;
    if ((UnityEngine.Object) pickupable1 != (UnityEngine.Object) null)
    {
      Pickupable pickupable2 = pickupable1.Take(amount);
      if ((UnityEngine.Object) pickupable2 != (UnityEngine.Object) null)
      {
        component.Store(pickupable2.gameObject, false, false, true, false);
        worker.workCompleteData = (object) pickupable2;
        startWorkInfo.setResultCb(pickupable2.gameObject);
      }
      else
        startWorkInfo.setResultCb((GameObject) null);
    }
    else
      startWorkInfo.setResultCb((GameObject) null);
  }

  public override Vector3 GetTargetPoint()
  {
    return this.transform.GetPosition();
  }

  public bool IsReachable()
  {
    return this.isReachable;
  }

  private void OnReachableChanged(object data)
  {
    this.isReachable = (bool) data;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.isReachable)
      component.RemoveStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, false);
    else
      component.AddStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, (object) this);
  }

  private void AddFaller(Vector2 initial_velocity)
  {
    if ((UnityEngine.Object) this.GetComponent<Health>() != (UnityEngine.Object) null || GameComps.Fallers.Has((object) this.gameObject))
      return;
    GameComps.Fallers.Add(this.gameObject, initial_velocity);
  }

  private void RemoveFaller()
  {
    if ((UnityEngine.Object) this.GetComponent<Health>() != (UnityEngine.Object) null || !GameComps.Fallers.Has((object) this.gameObject))
      return;
    GameComps.Fallers.Remove(this.gameObject);
  }

  private void OnOreSizeChanged(object data)
  {
    Vector3 vector3 = Vector3.zero;
    HandleVector<int>.Handle handle = GameComps.Gravities.GetHandle(this.gameObject);
    if (handle.IsValid())
      vector3 = (Vector3) GameComps.Gravities.GetData(handle).velocity;
    this.RemoveFaller();
    if (this.KPrefabID.HasTag(GameTags.Stored))
      return;
    this.AddFaller((Vector2) vector3);
  }

  private void OnLanded(object data)
  {
    if ((UnityEngine.Object) CameraController.Instance == (UnityEngine.Object) null)
      return;
    Vector3 position = this.transform.GetPosition();
    Vector2I xy = Grid.PosToXY(position);
    if (xy.x < 0 || Grid.WidthInCells <= xy.x || (xy.y < 0 || Grid.HeightInCells <= xy.y))
    {
      this.DeleteObject();
    }
    else
    {
      Vector2 vector2 = (Vector2) data;
      if ((double) vector2.sqrMagnitude <= 0.200000002980232 || SpeedControlScreen.Instance.IsPaused)
        return;
      Element element = this.primaryElement.Element;
      if (element.substance == null)
        return;
      string str1 = element.substance.GetOreBumpSound() ?? (!element.HasTag(GameTags.RefinedMetal) ? (!element.HasTag(GameTags.Metal) ? "Rock" : "RawMetal") : "RefinedMetal");
      string sound = GlobalAssets.GetSound(!(element.tag.ToString() == "Creature") || this.gameObject.HasTag(GameTags.Seed) ? "Ore_bump_" + str1 : "Bodyfall_rock", true);
      string str2 = sound == null ? GlobalAssets.GetSound("Ore_bump_rock", false) : sound;
      if (!CameraController.Instance.IsAudibleSound(this.transform.GetPosition(), str2))
        return;
      int cell = Grid.PosToCell(position);
      bool isLiquid = Grid.Element[cell].IsLiquid;
      float num1 = 0.0f;
      if (isLiquid)
        num1 = SoundUtil.GetLiquidDepth(cell);
      FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(str2, CameraController.Instance.GetVerticallyScaledPosition((Vector2) this.transform.GetPosition()));
      int num2 = (int) instance.setParameterValue("velocity", vector2.magnitude);
      int num3 = (int) instance.setParameterValue("liquidDepth", num1);
      KFMOD.EndOneShot(instance);
    }
  }

  private void UpdateEntombedVisualizer()
  {
    if (this.IsEntombed)
    {
      if (this.entombedCell != -1)
        return;
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      if (EntombedItemManager.CanEntomb(this))
        SaveGame.Instance.entombedItemManager.Add(this);
      if (!((UnityEngine.Object) Grid.Objects[cell, 1] == (UnityEngine.Object) null))
        return;
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(cell))
        return;
      this.entombedCell = cell;
      component.enabled = false;
      this.RemoveFaller();
    }
    else
      this.ReleaseEntombedVisualizerAndAddFaller(true);
  }

  private void ReleaseEntombedVisualizerAndAddFaller(bool add_faller_if_necessary)
  {
    if (this.entombedCell == -1)
      return;
    Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.entombedCell);
    this.entombedCell = -1;
    this.GetComponent<KBatchedAnimController>().enabled = true;
    if (!add_faller_if_necessary)
      return;
    this.AddFaller(Vector2.zero);
  }

  private struct Reservation
  {
    public GameObject reserver;
    public float amount;
    public int ticket;

    public Reservation(GameObject reserver, float amount, int ticket)
    {
      this.reserver = reserver;
      this.amount = amount;
      this.ticket = ticket;
    }

    public override string ToString()
    {
      return this.reserver.name + ", " + (object) this.amount + ", " + (object) this.ticket;
    }
  }

  public class PickupableStartWorkInfo : Worker.StartWorkInfo
  {
    public PickupableStartWorkInfo(
      Pickupable pickupable,
      float amount,
      System.Action<GameObject> set_result_cb)
      : base(pickupable.targetWorkable)
    {
      this.originalPickupable = pickupable;
      this.amount = amount;
      this.setResultCb = set_result_cb;
    }

    public float amount { get; private set; }

    public Pickupable originalPickupable { get; private set; }

    public System.Action<GameObject> setResultCb { get; private set; }
  }
}
