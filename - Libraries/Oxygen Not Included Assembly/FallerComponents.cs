// Decompiled with JetBrains decompiler
// Type: FallerComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FallerComponents : KGameObjectComponentManager<FallerComponent>
{
  public HandleVector<int>.Handle Add(GameObject go, Vector2 initial_velocity)
  {
    return this.Add(go, new FallerComponent(go.transform, initial_velocity));
  }

  public override void Remove(GameObject go)
  {
    HandleVector<int>.Handle handle = this.GetHandle(go);
    this.OnCleanUpImmediate(handle);
    KComponentManager<FallerComponent>.CleanupInfo info = new KComponentManager<FallerComponent>.CleanupInfo((object) go, handle);
    if (!KComponentCleanUp.InCleanUpPhase)
      this.cleanupList.Add(info);
    else
      this.InternalRemoveComponent(info);
  }

  protected override void OnPrefabInit(HandleVector<int>.Handle h)
  {
    FallerComponent data = this.GetData(h);
    Vector3 position = data.transform.GetPosition();
    int cell1 = Grid.PosToCell(position);
    data.cellChangedCB = (System.Action) (() => FallerComponents.OnSolidChanged(h));
    float num = (float) (-(double) GravityComponent.GetRadius(data.transform) - 0.0700000002980232);
    int cell2 = Grid.PosToCell(new Vector3(position.x, position.y + num, position.z));
    bool flag = Grid.IsValidCell(cell2) && Grid.Solid[cell2] && (double) data.initialVelocity.sqrMagnitude == 0.0;
    if (Grid.IsValidCell(cell1) && Grid.Solid[cell1] || flag)
    {
      data.solidChangedCB = (System.Action<object>) (ev_data => FallerComponents.OnSolidChanged(h));
      int height = 2;
      Vector2I xy = Grid.CellToXY(cell1);
      --xy.y;
      if (xy.y < 0)
      {
        xy.y = 0;
        height = 1;
      }
      else if (xy.y == Grid.HeightInCells - 1)
        height = 1;
      data.partitionerEntry = GameScenePartitioner.Instance.Add("Faller", (object) data.transform.gameObject, xy.x, xy.y, 1, height, GameScenePartitioner.Instance.solidChangedLayer, data.solidChangedCB);
      GameComps.Fallers.SetData(h, data);
    }
    else
    {
      GameComps.Fallers.SetData(h, data);
      FallerComponents.AddGravity(data.transform, data.initialVelocity);
    }
  }

  protected override void OnSpawn(HandleVector<int>.Handle h)
  {
    base.OnSpawn(h);
    FallerComponent data = this.GetData(h);
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(data.transform, data.cellChangedCB, "FallerComponent.OnSpawn");
  }

  private void OnCleanUpImmediate(HandleVector<int>.Handle h)
  {
    FallerComponent data = this.GetData(h);
    GameScenePartitioner.Instance.Free(ref data.partitionerEntry);
    if (data.cellChangedCB != null)
    {
      Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(data.transformInstanceId, data.cellChangedCB);
      data.cellChangedCB = (System.Action) null;
    }
    if (GameComps.Gravities.Has((object) data.transform.gameObject))
      GameComps.Gravities.Remove(data.transform.gameObject);
    this.SetData(h, data);
  }

  private static void AddGravity(Transform transform, Vector2 initial_velocity)
  {
    if (GameComps.Gravities.Has((object) transform.gameObject))
      return;
    GameComps.Gravities.Add(transform.gameObject, initial_velocity, (System.Action) (() => FallerComponents.OnLanded(transform)));
    HandleVector<int>.Handle handle = GameComps.Fallers.GetHandle(transform.gameObject);
    FallerComponent data = GameComps.Fallers.GetData(handle);
    if (!data.partitionerEntry.IsValid())
      return;
    GameScenePartitioner.Instance.Free(ref data.partitionerEntry);
    GameComps.Fallers.SetData(handle, data);
  }

  private static void RemoveGravity(Transform transform)
  {
    if (!GameComps.Gravities.Has((object) transform.gameObject))
      return;
    GameComps.Gravities.Remove(transform.gameObject);
    HandleVector<int>.Handle h = GameComps.Fallers.GetHandle(transform.gameObject);
    FallerComponent data = GameComps.Fallers.GetData(h);
    int cell = Grid.CellBelow(Grid.PosToCell(transform.GetPosition()));
    GameScenePartitioner.Instance.Free(ref data.partitionerEntry);
    if (Grid.IsValidCell(cell))
    {
      data.solidChangedCB = (System.Action<object>) (ev_data => FallerComponents.OnSolidChanged(h));
      data.partitionerEntry = GameScenePartitioner.Instance.Add("Faller", (object) transform.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, data.solidChangedCB);
    }
    GameComps.Fallers.SetData(h, data);
  }

  private static void OnLanded(Transform transform)
  {
    FallerComponents.RemoveGravity(transform);
  }

  private static void OnSolidChanged(HandleVector<int>.Handle handle)
  {
    FallerComponent data = GameComps.Fallers.GetData(handle);
    if ((UnityEngine.Object) data.transform == (UnityEngine.Object) null)
      return;
    Vector3 position = data.transform.GetPosition();
    position.y = (float) ((double) position.y - (double) data.offset - 0.100000001490116);
    int cell = Grid.PosToCell(position);
    if (!Grid.IsValidCell(cell))
      return;
    bool flag = !Grid.Solid[cell];
    if (flag == data.isFalling)
      return;
    data.isFalling = flag;
    if (flag)
      FallerComponents.AddGravity(data.transform, Vector2.zero);
    else
      FallerComponents.RemoveGravity(data.transform);
  }
}
