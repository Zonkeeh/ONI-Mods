// Decompiled with JetBrains decompiler
// Type: KBatchedAnimUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class KBatchedAnimUpdater : Singleton<KBatchedAnimUpdater>
{
  public static readonly Vector2I INVALID_CHUNK_ID = Vector2I.minusone;
  private static readonly Vector2 VISIBLE_RANGE_SCALE = new Vector2(1.5f, 1.5f);
  private List<KBatchedAnimController> updateList = new List<KBatchedAnimController>();
  private List<KBatchedAnimController> alwaysUpdateList = new List<KBatchedAnimController>();
  private List<Vector2I> visibleChunks = new List<Vector2I>();
  private List<Vector2I> previouslyVisibleChunks = new List<Vector2I>();
  private Vector2I vis_chunk_min = Vector2I.zero;
  private Vector2I vis_chunk_max = Vector2I.zero;
  private List<KBatchedAnimUpdater.RegistrationInfo> queuedRegistrations = new List<KBatchedAnimUpdater.RegistrationInfo>();
  private Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo> controllerChunkInfos = new Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo>();
  private List<KBatchedAnimUpdater.MovingControllerInfo> movingControllerInfos = new List<KBatchedAnimUpdater.MovingControllerInfo>();
  private const int VISIBLE_BORDER = 4;
  private List<KBatchedAnimController>[,] controllerGrid;
  private bool[,] visibleChunkGrid;
  private bool[,] previouslyVisibleChunkGrid;
  private const int CHUNKS_TO_CLEAN_PER_TICK = 16;
  private int cleanUpChunkIndex;

  public void InitializeGrid()
  {
    this.Clear();
    Vector2I visibleSize = this.GetVisibleSize();
    int length1 = (visibleSize.x + 32 - 1) / 32;
    int length2 = (visibleSize.y + 32 - 1) / 32;
    this.controllerGrid = new List<KBatchedAnimController>[length1, length2];
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < length1; ++index2)
        this.controllerGrid[index2, index1] = new List<KBatchedAnimController>();
    }
    this.visibleChunks.Clear();
    this.previouslyVisibleChunks.Clear();
    this.previouslyVisibleChunkGrid = new bool[length1, length2];
    this.visibleChunkGrid = new bool[length1, length2];
  }

  public Vector2I GetVisibleSize()
  {
    return new Vector2I((int) ((double) Grid.WidthInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x), (int) ((double) Grid.HeightInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y));
  }

  public void Clear()
  {
    for (int index = 0; index < this.updateList.Count; ++index)
    {
      if ((UnityEngine.Object) this.updateList[index] != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.updateList[index]);
    }
    this.updateList.Clear();
    for (int index = 0; index < this.alwaysUpdateList.Count; ++index)
    {
      if ((UnityEngine.Object) this.alwaysUpdateList[index] != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.alwaysUpdateList[index]);
    }
    this.alwaysUpdateList.Clear();
    this.queuedRegistrations.Clear();
    this.visibleChunks.Clear();
    this.previouslyVisibleChunks.Clear();
    this.controllerGrid = (List<KBatchedAnimController>[,]) null;
    this.previouslyVisibleChunkGrid = (bool[,]) null;
    this.visibleChunkGrid = (bool[,]) null;
  }

  public void UpdateRegister(KBatchedAnimController controller)
  {
    switch (controller.updateRegistrationState)
    {
      case KBatchedAnimUpdater.RegistrationState.PendingRemoval:
        controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
        break;
      case KBatchedAnimUpdater.RegistrationState.Unregistered:
        (controller.visibilityType != KAnimControllerBase.VisibilityType.Always ? this.updateList : this.alwaysUpdateList).Add(controller);
        controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
        break;
    }
  }

  public void UpdateUnregister(KBatchedAnimController controller)
  {
    switch (controller.updateRegistrationState)
    {
      case KBatchedAnimUpdater.RegistrationState.Registered:
        controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.PendingRemoval;
        break;
    }
  }

  public void VisibilityRegister(KBatchedAnimController controller)
  {
    this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo()
    {
      transformId = controller.transform.GetInstanceID(),
      controllerInstanceId = controller.GetInstanceID(),
      controller = controller,
      register = true
    });
  }

  public void VisibilityUnregister(KBatchedAnimController controller)
  {
    if (App.IsExiting)
      return;
    this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo()
    {
      transformId = controller.transform.GetInstanceID(),
      controllerInstanceId = controller.GetInstanceID(),
      controller = controller,
      register = false
    });
  }

  private List<KBatchedAnimController> GetControllerList(Vector2I chunk_xy)
  {
    List<KBatchedAnimController> kbatchedAnimControllerList = (List<KBatchedAnimController>) null;
    if (this.controllerGrid != null && 0 <= chunk_xy.x && (chunk_xy.x < this.controllerGrid.GetLength(0) && 0 <= chunk_xy.y) && chunk_xy.y < this.controllerGrid.GetLength(1))
      kbatchedAnimControllerList = this.controllerGrid[chunk_xy.x, chunk_xy.y];
    return kbatchedAnimControllerList;
  }

  public void LateUpdate()
  {
    this.ProcessMovingAnims();
    this.UpdateVisibility();
    this.ProcessRegistrations();
    this.CleanUp();
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    int count1 = this.alwaysUpdateList.Count;
    for (int index = 0; index < count1; ++index)
    {
      if (this.alwaysUpdateList[index].updateRegistrationState != KBatchedAnimUpdater.RegistrationState.Registered)
      {
        this.alwaysUpdateList[index].updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;
        this.alwaysUpdateList[index] = (KBatchedAnimController) null;
      }
      else
        this.alwaysUpdateList[index].UpdateAnim(unscaledDeltaTime);
    }
    if (!this.DoGridProcessing())
      return;
    float deltaTime = Time.deltaTime;
    int count2 = this.updateList.Count;
    for (int index = 0; index < count2; ++index)
    {
      if (this.updateList[index].updateRegistrationState != KBatchedAnimUpdater.RegistrationState.Registered)
      {
        this.updateList[index].updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;
        this.updateList[index] = (KBatchedAnimController) null;
      }
      else
        this.updateList[index].UpdateAnim(deltaTime);
    }
  }

  public bool IsChunkVisible(Vector2I chunk_xy)
  {
    return this.visibleChunkGrid[chunk_xy.x, chunk_xy.y];
  }

  public void GetVisibleArea(out Vector2I vis_chunk_min, out Vector2I vis_chunk_max)
  {
    vis_chunk_min = this.vis_chunk_min;
    vis_chunk_max = this.vis_chunk_max;
  }

  public static Vector2I PosToChunkXY(Vector3 pos)
  {
    return KAnimBatchManager.CellXYToChunkXY(Grid.PosToXY(pos));
  }

  private void UpdateVisibility()
  {
    if (!this.DoGridProcessing())
      return;
    Vector2I min;
    Vector2I max;
    KBatchedAnimUpdater.GetVisibleCellRange(out min, out max);
    this.vis_chunk_min = new Vector2I(min.x / 32, min.y / 32);
    this.vis_chunk_max = new Vector2I(max.x / 32, max.y / 32);
    this.vis_chunk_max.x = Math.Min(this.vis_chunk_max.x, this.controllerGrid.GetLength(0) - 1);
    this.vis_chunk_max.y = Math.Min(this.vis_chunk_max.y, this.controllerGrid.GetLength(1) - 1);
    bool[,] visibleChunkGrid = this.previouslyVisibleChunkGrid;
    this.previouslyVisibleChunkGrid = this.visibleChunkGrid;
    this.visibleChunkGrid = visibleChunkGrid;
    Array.Clear((Array) this.visibleChunkGrid, 0, this.visibleChunkGrid.Length);
    List<Vector2I> previouslyVisibleChunks = this.previouslyVisibleChunks;
    this.previouslyVisibleChunks = this.visibleChunks;
    this.visibleChunks = previouslyVisibleChunks;
    this.visibleChunks.Clear();
    for (int y = this.vis_chunk_min.y; y <= this.vis_chunk_max.y; ++y)
    {
      for (int x = this.vis_chunk_min.x; x <= this.vis_chunk_max.x; ++x)
      {
        this.visibleChunkGrid[x, y] = true;
        this.visibleChunks.Add(new Vector2I(x, y));
        if (!this.previouslyVisibleChunkGrid[x, y])
        {
          List<KBatchedAnimController> kbatchedAnimControllerList = this.controllerGrid[x, y];
          for (int index = 0; index < kbatchedAnimControllerList.Count; ++index)
          {
            KBatchedAnimController kbatchedAnimController = kbatchedAnimControllerList[index];
            if (!((UnityEngine.Object) kbatchedAnimController == (UnityEngine.Object) null))
              kbatchedAnimController.SetVisiblity(true);
          }
        }
      }
    }
    for (int index1 = 0; index1 < this.previouslyVisibleChunks.Count; ++index1)
    {
      Vector2I previouslyVisibleChunk = this.previouslyVisibleChunks[index1];
      if (!this.visibleChunkGrid[previouslyVisibleChunk.x, previouslyVisibleChunk.y])
      {
        List<KBatchedAnimController> kbatchedAnimControllerList = this.controllerGrid[previouslyVisibleChunk.x, previouslyVisibleChunk.y];
        for (int index2 = 0; index2 < kbatchedAnimControllerList.Count; ++index2)
        {
          KBatchedAnimController kbatchedAnimController = kbatchedAnimControllerList[index2];
          if (!((UnityEngine.Object) kbatchedAnimController == (UnityEngine.Object) null))
            kbatchedAnimController.SetVisiblity(false);
        }
      }
    }
  }

  private void ProcessMovingAnims()
  {
    for (int index = 0; index < this.movingControllerInfos.Count; ++index)
    {
      KBatchedAnimUpdater.MovingControllerInfo movingControllerInfo = this.movingControllerInfos[index];
      if (!((UnityEngine.Object) movingControllerInfo.controller == (UnityEngine.Object) null))
      {
        Vector2I chunkXy = KBatchedAnimUpdater.PosToChunkXY(movingControllerInfo.controller.PositionIncludingOffset);
        if (movingControllerInfo.chunkXY != chunkXy)
        {
          KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo();
          DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(movingControllerInfo.controllerInstanceId, out controllerChunkInfo));
          DebugUtil.Assert((UnityEngine.Object) movingControllerInfo.controller == (UnityEngine.Object) controllerChunkInfo.controller);
          DebugUtil.Assert(controllerChunkInfo.chunkXY == movingControllerInfo.chunkXY);
          List<KBatchedAnimController> controllerList1 = this.GetControllerList(controllerChunkInfo.chunkXY);
          if (controllerList1 != null)
          {
            DebugUtil.Assert(controllerList1.Contains(controllerChunkInfo.controller));
            controllerList1.Remove(controllerChunkInfo.controller);
          }
          List<KBatchedAnimController> controllerList2 = this.GetControllerList(chunkXy);
          if (controllerList2 != null)
          {
            DebugUtil.Assert(!controllerList2.Contains(controllerChunkInfo.controller));
            controllerList2.Add(controllerChunkInfo.controller);
          }
          movingControllerInfo.chunkXY = chunkXy;
          this.movingControllerInfos[index] = movingControllerInfo;
          controllerChunkInfo.chunkXY = chunkXy;
          this.controllerChunkInfos[movingControllerInfo.controllerInstanceId] = controllerChunkInfo;
          if (controllerList2 != null)
            controllerChunkInfo.controller.SetVisiblity(this.visibleChunkGrid[chunkXy.x, chunkXy.y]);
          else
            controllerChunkInfo.controller.SetVisiblity(false);
        }
      }
    }
  }

  private void ProcessRegistrations()
  {
    ListPool<KBatchedAnimController, KBatchedAnimUpdater>.PooledList pooledList = ListPool<KBatchedAnimController, KBatchedAnimUpdater>.Allocate();
    for (int index = 0; index < this.queuedRegistrations.Count; ++index)
    {
      KBatchedAnimUpdater.RegistrationInfo info = this.queuedRegistrations[index];
      if (info.register)
      {
        if (!((UnityEngine.Object) info.controller == (UnityEngine.Object) null))
        {
          int instanceId = info.controller.GetInstanceID();
          DebugUtil.Assert(!this.controllerChunkInfos.ContainsKey(instanceId));
          KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo()
          {
            controller = info.controller,
            chunkXY = KBatchedAnimUpdater.PosToChunkXY(info.controller.PositionIncludingOffset)
          };
          this.controllerChunkInfos[instanceId] = controllerChunkInfo;
          Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(info.controller.transform, new System.Action<Transform, bool>(this.OnMovementStateChanged));
          List<KBatchedAnimController> controllerList = this.GetControllerList(controllerChunkInfo.chunkXY);
          if (controllerList != null)
          {
            DebugUtil.Assert(!controllerList.Contains(info.controller));
            controllerList.Add(info.controller);
          }
          if (Singleton<CellChangeMonitor>.Instance.IsMoving(info.controller.transform))
            this.movingControllerInfos.Add(new KBatchedAnimUpdater.MovingControllerInfo()
            {
              controllerInstanceId = instanceId,
              controller = info.controller,
              chunkXY = controllerChunkInfo.chunkXY
            });
          if (controllerList != null && this.visibleChunkGrid[controllerChunkInfo.chunkXY.x, controllerChunkInfo.chunkXY.y])
            pooledList.Add(info.controller);
        }
      }
      else
      {
        KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo();
        if (this.controllerChunkInfos.TryGetValue(info.controllerInstanceId, out controllerChunkInfo))
        {
          if ((UnityEngine.Object) info.controller != (UnityEngine.Object) null)
          {
            List<KBatchedAnimController> controllerList = this.GetControllerList(controllerChunkInfo.chunkXY);
            if (controllerList != null)
            {
              DebugUtil.Assert(controllerList.Contains(info.controller));
              controllerList.Remove(info.controller);
            }
          }
          this.movingControllerInfos.RemoveAll((Predicate<KBatchedAnimUpdater.MovingControllerInfo>) (x => x.controllerInstanceId == info.controllerInstanceId));
          Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(info.transformId, new System.Action<Transform, bool>(this.OnMovementStateChanged));
          this.controllerChunkInfos.Remove(info.controllerInstanceId);
          pooledList.Remove(info.controller);
        }
      }
    }
    this.queuedRegistrations.Clear();
    foreach (KBatchedAnimController kbatchedAnimController in (List<KBatchedAnimController>) pooledList)
    {
      if ((UnityEngine.Object) kbatchedAnimController != (UnityEngine.Object) null)
        kbatchedAnimController.SetVisiblity(true);
    }
    pooledList.Recycle();
  }

  public void OnMovementStateChanged(Transform transform, bool is_moving)
  {
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      return;
    KBatchedAnimController component = transform.GetComponent<KBatchedAnimController>();
    int controller_instance_id = component.GetInstanceID();
    KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo();
    DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(controller_instance_id, out controllerChunkInfo));
    if (is_moving)
      this.movingControllerInfos.Add(new KBatchedAnimUpdater.MovingControllerInfo()
      {
        controllerInstanceId = controller_instance_id,
        controller = component,
        chunkXY = controllerChunkInfo.chunkXY
      });
    else
      this.movingControllerInfos.RemoveAll((Predicate<KBatchedAnimUpdater.MovingControllerInfo>) (x => x.controllerInstanceId == controller_instance_id));
  }

  private void CleanUp()
  {
    this.updateList.RemoveAll((Predicate<KBatchedAnimController>) (item => (UnityEngine.Object) item == (UnityEngine.Object) null));
    this.alwaysUpdateList.RemoveAll((Predicate<KBatchedAnimController>) (item => (UnityEngine.Object) item == (UnityEngine.Object) null));
    if (!this.DoGridProcessing())
      return;
    int length = this.controllerGrid.GetLength(0);
    for (int index = 0; index < 16; ++index)
    {
      int num = (this.cleanUpChunkIndex + index) % this.controllerGrid.Length;
      this.controllerGrid[num % length, num / length].RemoveAll((Predicate<KBatchedAnimController>) (item => (UnityEngine.Object) item == (UnityEngine.Object) null));
    }
    this.cleanUpChunkIndex = (this.cleanUpChunkIndex + 16) % this.controllerGrid.Length;
  }

  public static void GetVisibleCellRange(out Vector2I min, out Vector2I max)
  {
    Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
    min.x -= 4;
    min.y -= 4;
    min.x = Math.Min((int) ((double) Grid.WidthInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x) - 1, Math.Max(0, min.x));
    min.y = Math.Min((int) ((double) Grid.HeightInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y) - 1, Math.Max(0, min.y));
    max.x += 4;
    max.y += 4;
    max.x = Math.Min((int) ((double) Grid.WidthInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x) - 1, Math.Max(0, max.x));
    max.y = Math.Min((int) ((double) Grid.HeightInCells * (double) KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y) - 1, Math.Max(0, max.y));
  }

  private bool DoGridProcessing()
  {
    if (this.controllerGrid != null)
      return (UnityEngine.Object) Camera.main != (UnityEngine.Object) null;
    return false;
  }

  public enum RegistrationState
  {
    Registered,
    PendingRemoval,
    Unregistered,
  }

  private struct RegistrationInfo
  {
    public bool register;
    public int transformId;
    public int controllerInstanceId;
    public KBatchedAnimController controller;
  }

  private struct ControllerChunkInfo
  {
    public KBatchedAnimController controller;
    public Vector2I chunkXY;
  }

  private struct MovingControllerInfo
  {
    public int controllerInstanceId;
    public KBatchedAnimController controller;
    public Vector2I chunkXY;
  }
}
