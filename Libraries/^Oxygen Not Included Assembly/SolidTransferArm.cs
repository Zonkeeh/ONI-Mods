// Decompiled with JetBrains decompiler
// Type: SolidTransferArm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using FMODUnity;
using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : StateMachineComponent<SolidTransferArm.SMInstance>, ISim1000ms, IRenderEveryTick
{
  public static TagBits tagBits = new TagBits(STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Concat<Tag>((IEnumerable<Tag>) STORAGEFILTERS.FOOD).ToArray<Tag>());
  private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>((System.Action<SolidTransferArm, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>((System.Action<SolidTransferArm, object>) ((component, data) => component.OnEndChore(data)));
  private static List<SolidTransferArm.CachedPickupable> cached_pickupables = new List<SolidTransferArm.CachedPickupable>();
  private static WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext> batch_update_job = new WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext>();
  private static HashedString HASH_ROTATION = (HashedString) "rotation";
  public int pickupRange = 4;
  private float max_carry_weight = 1000f;
  private List<Pickupable> pickupables = new List<Pickupable>();
  [EventRef]
  private string rotateSound = "TransferArm_rotate";
  private float arm_rot = 45f;
  private float turn_rate = 360f;
  private HashSet<int> reachableCells = new HashSet<int>();
  [MyCmpReq]
  private Operational operational;
  [MyCmpGet]
  private KSelectable selectable;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private Rotatable rotatable;
  [MyCmpAdd]
  private Worker worker;
  [MyCmpAdd]
  private ChoreConsumer choreConsumer;
  [MyCmpAdd]
  private ChoreDriver choreDriver;
  private Extents pickupableExtents;
  private KBatchedAnimController arm_anim_ctrl;
  private GameObject arm_go;
  private LoopingSounds looping_sounds;
  private bool rotateSoundPlaying;
  private KAnimLink link;
  private bool rotation_complete;
  private SolidTransferArm.ArmAnim arm_anim;
  private int serial_no;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreConsumer.AddProvider((ChoreProvider) GlobalChoreProvider.Instance);
    this.choreConsumer.SetReach(this.pickupRange);
    Klei.AI.Attributes attributes = this.GetAttributes();
    if (attributes.Get(Db.Get().Attributes.CarryAmount) == null)
      attributes.Add(Db.Get().Attributes.CarryAmount);
    this.GetAttributes().Add(new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, this.max_carry_weight, this.gameObject.GetProperName(), false, false, true));
    this.worker.usesMultiTool = false;
    this.storage.fxPrefix = Storage.FXPrefix.PickedUp;
    this.simRenderLoadBalance = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    string name = component.name + ".arm";
    this.arm_go = new GameObject(name);
    this.arm_go.SetActive(false);
    this.arm_go.transform.parent = component.transform;
    this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
    this.rotateSound = GlobalAssets.GetSound(this.rotateSound, false);
    this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
    this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
    this.arm_anim_ctrl.AnimFiles = new KAnimFile[1]
    {
      component.AnimFiles[0]
    };
    this.arm_anim_ctrl.initialAnim = "arm";
    this.arm_anim_ctrl.isMovable = true;
    this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
    component.SetSymbolVisiblity((KAnimHashedString) "arm_target", false);
    bool symbolVisible;
    Vector3 column = (Vector3) component.GetSymbolTransform(new HashedString("arm_target"), out symbolVisible).GetColumn(3);
    column.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
    this.arm_go.transform.SetPosition(column);
    this.arm_go.SetActive(true);
    this.link = new KAnimLink((KAnimControllerBase) component, (KAnimControllerBase) this.arm_anim_ctrl);
    ChoreGroups choreGroups = Db.Get().ChoreGroups;
    for (int index = 0; index < choreGroups.Count; ++index)
      this.choreConsumer.SetPermittedByUser(choreGroups[index], true);
    this.Subscribe<SolidTransferArm>(-592767678, SolidTransferArm.OnOperationalChangedDelegate);
    this.Subscribe<SolidTransferArm>(1745615042, SolidTransferArm.OnEndChoreDelegate);
    this.RotateArm(this.rotatable.GetRotatedOffset(Vector3.up), true, 0.0f);
    this.DropLeftovers();
    component.enabled = false;
    component.enabled = true;
    MinionGroupProber.Get().SetValidSerialNos((object) this, this.serial_no, this.serial_no);
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    MinionGroupProber.Get().ReleaseProber((object) this);
    base.OnCleanUp();
  }

  private static void CachePickupables()
  {
    SolidTransferArm.cached_pickupables.Clear();
    foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchable in Game.Instance.fetchManager.prefabIdToFetchables)
    {
      List<FetchManager.Fetchable> dataList = prefabIdToFetchable.Value.fetchables.GetDataList();
      SolidTransferArm.cached_pickupables.Capacity = Math.Max(SolidTransferArm.cached_pickupables.Capacity, SolidTransferArm.cached_pickupables.Count + dataList.Count);
      foreach (FetchManager.Fetchable fetchable in dataList)
        SolidTransferArm.cached_pickupables.Add(new SolidTransferArm.CachedPickupable()
        {
          pickupable = fetchable.pickupable,
          storage_cell = fetchable.pickupable.storageCell
        });
    }
  }

  public static void BatchUpdate(
    List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms,
    float time_delta)
  {
    SolidTransferArm.BatchUpdateContext shared_data = new SolidTransferArm.BatchUpdateContext(solid_transfer_arms);
    if (shared_data.solid_transfer_arms.Count == 0)
    {
      shared_data.Finish();
    }
    else
    {
      SolidTransferArm.CachePickupables();
      SolidTransferArm.batch_update_job.Reset(shared_data);
      int num1 = Math.Max(1, shared_data.solid_transfer_arms.Count / CPUBudget.coreCount);
      int num2 = Math.Min(shared_data.solid_transfer_arms.Count, CPUBudget.coreCount);
      for (int index = 0; index != num2; ++index)
      {
        int start = index * num1;
        int end = index != num2 - 1 ? start + num1 : shared_data.solid_transfer_arms.Count;
        SolidTransferArm.batch_update_job.Add(new SolidTransferArm.BatchUpdateTask(start, end));
      }
      GlobalJobManager.Run((IWorkItemCollection) SolidTransferArm.batch_update_job);
      for (int idx = 0; idx != SolidTransferArm.batch_update_job.Count; ++idx)
        SolidTransferArm.batch_update_job.GetWorkItem(idx).Finish();
      shared_data.Finish();
    }
  }

  private void Sim()
  {
    Chore.Precondition.Context out_context = new Chore.Precondition.Context();
    if (this.choreConsumer.FindNextChore(ref out_context))
    {
      if (out_context.chore is FetchChore)
      {
        this.choreDriver.SetChore(out_context);
        this.arm_anim_ctrl.enabled = false;
        this.arm_anim_ctrl.enabled = true;
      }
      else
        Debug.Assert(false, (object) ("I am but a lowly transfer arm. I should only acquire FetchChores: " + (object) out_context.chore));
    }
    this.operational.SetActive(this.choreDriver.HasChore(), false);
  }

  public void Sim1000ms(float dt)
  {
  }

  private void UpdateArmAnim()
  {
    FetchAreaChore currentChore = this.choreDriver.GetCurrentChore() as FetchAreaChore;
    if ((bool) ((UnityEngine.Object) this.worker.workable) && currentChore != null && this.rotation_complete)
    {
      this.StopRotateSound();
      this.SetArmAnim(!currentChore.IsDelivering ? SolidTransferArm.ArmAnim.Pickup : SolidTransferArm.ArmAnim.Drop);
    }
    else
      this.SetArmAnim(SolidTransferArm.ArmAnim.Idle);
  }

  private bool AsyncUpdate(int cell, HashSet<int> workspace, GameObject game_object)
  {
    workspace.Clear();
    int x;
    int y;
    Grid.CellToXY(cell, out x, out y);
    for (int index1 = y - this.pickupRange; index1 < y + this.pickupRange + 1; ++index1)
    {
      for (int index2 = x - this.pickupRange; index2 < x + this.pickupRange + 1; ++index2)
      {
        int cell1 = Grid.XYToCell(index2, index1);
        if (Grid.IsValidCell(cell1) && Grid.IsPhysicallyAccessible(x, y, index2, index1, true))
          workspace.Add(cell1);
      }
    }
    bool flag = !this.reachableCells.SetEquals((IEnumerable<int>) workspace);
    if (flag)
    {
      this.reachableCells.Clear();
      this.reachableCells.UnionWith((IEnumerable<int>) workspace);
    }
    this.pickupables.Clear();
    foreach (SolidTransferArm.CachedPickupable cachedPickupable in SolidTransferArm.cached_pickupables)
    {
      if (Grid.GetCellRange(cell, cachedPickupable.storage_cell) <= this.pickupRange && this.IsPickupableRelevantToMyInterests(cachedPickupable.pickupable.KPrefabID, cachedPickupable.storage_cell) && cachedPickupable.pickupable.CouldBePickedUpByTransferArm(game_object))
        this.pickupables.Add(cachedPickupable.pickupable);
    }
    return flag;
  }

  private void IncrementSerialNo()
  {
    ++this.serial_no;
    MinionGroupProber.Get().SetValidSerialNos((object) this, this.serial_no, this.serial_no);
    MinionGroupProber.Get().Occupy((object) this, this.serial_no, (IEnumerable<int>) this.reachableCells);
  }

  public bool IsCellReachable(int cell)
  {
    return this.reachableCells.Contains(cell);
  }

  private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell)
  {
    if (prefabID.HasAnyTags(ref SolidTransferArm.tagBits))
      return this.IsCellReachable(storage_cell);
    return false;
  }

  public void FindFetchTarget(
    Storage destination,
    TagBits tag_bits,
    TagBits required_tags,
    TagBits forbid_tags,
    float required_amount,
    ref Pickupable target)
  {
    target = FetchManager.FindFetchTarget(this.pickupables, destination, ref tag_bits, ref required_tags, ref forbid_tags, required_amount);
  }

  public void RenderEveryTick(float dt)
  {
    if ((bool) ((UnityEngine.Object) this.worker.workable))
    {
      Vector3 targetPoint = this.worker.workable.GetTargetPoint();
      targetPoint.z = 0.0f;
      Vector3 position = this.transform.GetPosition();
      position.z = 0.0f;
      this.RotateArm(Vector3.Normalize(targetPoint - position), false, dt);
    }
    this.UpdateArmAnim();
  }

  private void SetArmAnim(SolidTransferArm.ArmAnim new_anim)
  {
    if (new_anim == this.arm_anim)
      return;
    this.arm_anim = new_anim;
    switch (this.arm_anim)
    {
      case SolidTransferArm.ArmAnim.Idle:
        this.arm_anim_ctrl.Play((HashedString) "arm", KAnim.PlayMode.Loop, 1f, 0.0f);
        break;
      case SolidTransferArm.ArmAnim.Pickup:
        this.arm_anim_ctrl.Play((HashedString) "arm_pickup", KAnim.PlayMode.Loop, 1f, 0.0f);
        break;
      case SolidTransferArm.ArmAnim.Drop:
        this.arm_anim_ctrl.Play((HashedString) "arm_drop", KAnim.PlayMode.Loop, 1f, 0.0f);
        break;
    }
  }

  private void OnOperationalChanged(object data)
  {
    if ((bool) data)
      return;
    if (this.choreDriver.HasChore())
      this.choreDriver.StopChore();
    this.UpdateArmAnim();
  }

  private void OnEndChore(object data)
  {
    this.DropLeftovers();
  }

  private void DropLeftovers()
  {
    if (this.storage.IsEmpty() || this.choreDriver.HasChore())
      return;
    this.storage.DropAll(false, false, new Vector3(), true);
  }

  private void SetArmRotation(float rot)
  {
    this.arm_rot = rot;
    this.arm_go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.arm_rot);
  }

  private void RotateArm(Vector3 target_dir, bool warp, float dt)
  {
    float a = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
    if ((double) a < -180.0)
      a += 360f;
    if ((double) a > 180.0)
      a -= 360f;
    if (!warp)
      a = Mathf.Clamp(a, -this.turn_rate * dt, this.turn_rate * dt);
    this.arm_rot += a;
    this.SetArmRotation(this.arm_rot);
    this.rotation_complete = Mathf.Approximately(a, 0.0f);
    if (!warp && !this.rotation_complete)
    {
      if (!this.rotateSoundPlaying)
        this.StartRotateSound();
      this.SetRotateSoundParameter(this.arm_rot);
    }
    else
      this.StopRotateSound();
  }

  private void StartRotateSound()
  {
    if (this.rotateSoundPlaying)
      return;
    this.looping_sounds.StartSound(this.rotateSound);
    this.rotateSoundPlaying = true;
  }

  private void SetRotateSoundParameter(float arm_rot)
  {
    if (!this.rotateSoundPlaying)
      return;
    this.looping_sounds.SetParameter(this.rotateSound, SolidTransferArm.HASH_ROTATION, arm_rot);
  }

  private void StopRotateSound()
  {
    if (!this.rotateSoundPlaying)
      return;
    this.looping_sounds.StopSound(this.rotateSound);
    this.rotateSoundPlaying = false;
  }

  [Conditional("ENABLE_FETCH_PROFILING")]
  private static void BeginDetailedSample(string region_name)
  {
  }

  [Conditional("ENABLE_FETCH_PROFILING")]
  private static void BeginDetailedSample(string region_name, int count)
  {
  }

  [Conditional("ENABLE_FETCH_PROFILING")]
  private static void EndDetailedSample()
  {
  }

  private enum ArmAnim
  {
    Idle,
    Pickup,
    Drop,
  }

  public class SMInstance : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.GameInstance
  {
    public SMInstance(SolidTransferArm master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm>
  {
    public StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.BoolParameter transferring;
    public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State off;
    public SolidTransferArm.States.ReadyStates on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.root.DoNothing();
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State) this.on, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational)).Enter((StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State.Callback) (smi => smi.master.StopRotateSound()));
      this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
      this.on.working.PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
    }

    public class ReadyStates : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State
    {
      public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State idle;
      public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State working;
    }
  }

  private struct BatchUpdateContext
  {
    public ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.PooledList solid_transfer_arms;
    public ListPool<bool, SolidTransferArm.BatchUpdateContext>.PooledList refreshed_reachable_cells;
    public ListPool<int, SolidTransferArm.BatchUpdateContext>.PooledList cells;
    public ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.PooledList game_objects;

    public BatchUpdateContext(
      List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms)
    {
      this.solid_transfer_arms = ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.Allocate();
      this.solid_transfer_arms.Capacity = solid_transfer_arms.Count;
      this.refreshed_reachable_cells = ListPool<bool, SolidTransferArm.BatchUpdateContext>.Allocate();
      this.refreshed_reachable_cells.Capacity = solid_transfer_arms.Count;
      this.cells = ListPool<int, SolidTransferArm.BatchUpdateContext>.Allocate();
      this.cells.Capacity = solid_transfer_arms.Count;
      this.game_objects = ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.Allocate();
      this.game_objects.Capacity = solid_transfer_arms.Count;
      for (int index = 0; index != solid_transfer_arms.Count; ++index)
      {
        UpdateBucketWithUpdater<ISim1000ms>.Entry solidTransferArm = solid_transfer_arms[index];
        solidTransferArm.lastUpdateTime = 0.0f;
        solid_transfer_arms[index] = solidTransferArm;
        SolidTransferArm data = (SolidTransferArm) solidTransferArm.data;
        if (data.operational.IsOperational)
        {
          this.solid_transfer_arms.Add(data);
          this.refreshed_reachable_cells.Add(false);
          this.cells.Add(Grid.PosToCell((KMonoBehaviour) data));
          this.game_objects.Add(data.gameObject);
        }
      }
    }

    public void Finish()
    {
      for (int index = 0; index != this.solid_transfer_arms.Count; ++index)
      {
        if (this.refreshed_reachable_cells[index])
          this.solid_transfer_arms[index].IncrementSerialNo();
        this.solid_transfer_arms[index].Sim();
      }
      this.refreshed_reachable_cells.Recycle();
      this.cells.Recycle();
      this.game_objects.Recycle();
      this.solid_transfer_arms.Recycle();
    }
  }

  private struct BatchUpdateTask : IWorkItem<SolidTransferArm.BatchUpdateContext>
  {
    private int start;
    private int end;
    private HashSetPool<int, SolidTransferArm>.PooledHashSet reachable_cells_workspace;

    public BatchUpdateTask(int start, int end)
    {
      this.start = start;
      this.end = end;
      this.reachable_cells_workspace = HashSetPool<int, SolidTransferArm>.Allocate();
    }

    public void Run(SolidTransferArm.BatchUpdateContext context)
    {
      for (int start = this.start; start != this.end; ++start)
        context.refreshed_reachable_cells[start] = context.solid_transfer_arms[start].AsyncUpdate(context.cells[start], (HashSet<int>) this.reachable_cells_workspace, context.game_objects[start]);
    }

    public void Finish()
    {
      this.reachable_cells_workspace.Recycle();
    }
  }

  public struct CachedPickupable
  {
    public Pickupable pickupable;
    public int storage_cell;
  }
}
