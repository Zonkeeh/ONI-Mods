// Decompiled with JetBrains decompiler
// Type: KCollider2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick
{
  [SerializeField]
  public Vector2 _offset;
  private Extents cachedExtents;
  private HandleVector<int>.Handle partitionerEntry;

  public Vector2 offset
  {
    get
    {
      return this._offset;
    }
    set
    {
      this._offset = value;
      this.MarkDirty(false);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.autoRegisterSimRender = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
    Transform transform = this.transform;
    // ISSUE: reference to a compiler-generated field
    if (KCollider2D.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      KCollider2D.\u003C\u003Ef__mg\u0024cache0 = new System.Action<Transform, bool>(KCollider2D.OnMovementStateChanged);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<Transform, bool> fMgCache0 = KCollider2D.\u003C\u003Ef__mg\u0024cache0;
    instance.RegisterMovementStateChanged(transform, fMgCache0);
    this.MarkDirty(true);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
    Transform transform = this.transform;
    // ISSUE: reference to a compiler-generated field
    if (KCollider2D.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      KCollider2D.\u003C\u003Ef__mg\u0024cache1 = new System.Action<Transform, bool>(KCollider2D.OnMovementStateChanged);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<Transform, bool> fMgCache1 = KCollider2D.\u003C\u003Ef__mg\u0024cache1;
    instance.UnregisterMovementStateChanged(transform, fMgCache1);
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }

  public void MarkDirty(bool force = false)
  {
    bool flag = force || this.partitionerEntry.IsValid();
    if (!flag)
      return;
    Extents extents = this.GetExtents();
    if (!force && this.cachedExtents.x == extents.x && (this.cachedExtents.y == extents.y && this.cachedExtents.width == extents.width) && this.cachedExtents.height == extents.height)
      return;
    this.cachedExtents = extents;
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    if (!flag)
      return;
    this.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, (object) this, this.cachedExtents, GameScenePartitioner.Instance.collisionLayer, (System.Action<object>) null);
  }

  private void OnMovementStateChanged(bool is_moving)
  {
    if (is_moving)
    {
      this.MarkDirty(false);
      SimAndRenderScheduler.instance.Add((object) this, false);
    }
    else
      SimAndRenderScheduler.instance.Remove((object) this);
  }

  private static void OnMovementStateChanged(Transform transform, bool is_moving)
  {
    transform.GetComponent<KCollider2D>().OnMovementStateChanged(is_moving);
  }

  public void RenderEveryTick(float dt)
  {
    this.MarkDirty(false);
  }

  public abstract bool Intersects(Vector2 pos);

  public abstract Extents GetExtents();

  public abstract Bounds bounds { get; }
}
