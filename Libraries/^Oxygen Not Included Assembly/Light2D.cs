// Decompiled with JetBrains decompiler
// Type: Light2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class Light2D : KMonoBehaviour, IGameObjectEffectDescriptor, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Light2D> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Light2D>((System.Action<Light2D, object>) ((light, data) => light.enabled = (bool) data));
  [SerializeField]
  private LightGridManager.LightGridEmitter.State pending_emitter_state = LightGridManager.LightGridEmitter.State.DEFAULT;
  private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;
  private HandleVector<int>.Handle liquidPartitionerEntry = HandleVector<int>.InvalidHandle;
  private bool dirty_shape;
  private bool dirty_position;
  public float Angle;
  public Vector2 Direction;
  [SerializeField]
  private Vector2 _offset;
  public bool drawOverlay;
  public Color overlayColour;
  public MaterialPropertyBlock materialPropertyBlock;

  public Light2D()
  {
    this.emitter = new LightGridManager.LightGridEmitter();
    this.Range = 5f;
    this.Lux = 1000;
  }

  private T MaybeDirty<T>(T old_value, T new_value, ref bool dirty)
  {
    if (EqualityComparer<T>.Default.Equals(old_value, new_value))
      return old_value;
    dirty = true;
    return new_value;
  }

  public LightShape shape
  {
    get
    {
      return this.pending_emitter_state.shape;
    }
    set
    {
      this.pending_emitter_state.shape = this.MaybeDirty<LightShape>(this.pending_emitter_state.shape, value, ref this.dirty_shape);
    }
  }

  public LightGridManager.LightGridEmitter emitter { get; private set; }

  public Color Color
  {
    get
    {
      return this.pending_emitter_state.colour;
    }
    set
    {
      this.pending_emitter_state.colour = value;
    }
  }

  public int Lux
  {
    get
    {
      return this.pending_emitter_state.intensity;
    }
    set
    {
      this.pending_emitter_state.intensity = value;
    }
  }

  public float Range
  {
    get
    {
      return this.pending_emitter_state.radius;
    }
    set
    {
      this.pending_emitter_state.radius = this.MaybeDirty<float>(this.pending_emitter_state.radius, value, ref this.dirty_shape);
    }
  }

  private int origin
  {
    get
    {
      return this.pending_emitter_state.origin;
    }
    set
    {
      this.pending_emitter_state.origin = this.MaybeDirty<int>(this.pending_emitter_state.origin, value, ref this.dirty_position);
    }
  }

  public float IntensityAnimation { get; set; }

  public Vector2 Offset
  {
    get
    {
      return this._offset;
    }
    set
    {
      if (!(this._offset != value))
        return;
      this._offset = value;
      this.origin = Grid.PosToCell(this.transform.GetPosition() + (Vector3) this._offset);
    }
  }

  private bool isRegistered
  {
    get
    {
      return this.solidPartitionerEntry != HandleVector<int>.InvalidHandle;
    }
  }

  protected override void OnPrefabInit()
  {
    this.Subscribe<Light2D>(-592767678, Light2D.OnOperationalChangedDelegate);
    this.IntensityAnimation = 1f;
  }

  protected override void OnCmpEnable()
  {
    this.materialPropertyBlock = new MaterialPropertyBlock();
    base.OnCmpEnable();
    Components.Light2Ds.Add(this);
    if (this.isSpawned)
    {
      this.AddToScenePartitioner();
      this.emitter.Refresh(this.pending_emitter_state, true);
    }
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnMoved), "Light2D.OnMoved");
  }

  protected override void OnCmpDisable()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnMoved));
    Components.Light2Ds.Remove(this);
    base.OnCmpDisable();
    this.FullRemove();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.origin = Grid.PosToCell(this.transform.GetPosition() + (Vector3) this.Offset);
    if (!this.isActiveAndEnabled)
      return;
    this.AddToScenePartitioner();
    this.emitter.Refresh(this.pending_emitter_state, true);
  }

  protected override void OnCleanUp()
  {
    this.FullRemove();
  }

  private void OnMoved()
  {
    if (!this.isSpawned)
      return;
    this.FullRefresh();
  }

  private HandleVector<int>.Handle AddToLayer(
    Vector2I xy_min,
    int width,
    int height,
    ScenePartitionerLayer layer)
  {
    return GameScenePartitioner.Instance.Add(nameof (Light2D), (object) this.gameObject, xy_min.x, xy_min.y, width, height, layer, new System.Action<object>(this.OnWorldChanged));
  }

  private void AddToScenePartitioner()
  {
    Vector2I xy = Grid.CellToXY(this.origin);
    int range = (int) this.Range;
    Vector2I xy_min = new Vector2I(xy.x - range, xy.y - range);
    int width = 2 * range;
    int height = this.shape != LightShape.Circle ? range : 2 * range;
    this.solidPartitionerEntry = this.AddToLayer(xy_min, width, height, GameScenePartitioner.Instance.solidChangedLayer);
    this.liquidPartitionerEntry = this.AddToLayer(xy_min, width, height, GameScenePartitioner.Instance.liquidChangedLayer);
  }

  private void RemoveFromScenePartitioner()
  {
    if (!this.isRegistered)
      return;
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
  }

  private void MoveInScenePartitioner()
  {
    GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.origin);
    GameScenePartitioner.Instance.UpdatePosition(this.liquidPartitionerEntry, this.origin);
  }

  [ContextMenu("Refresh")]
  public void FullRefresh()
  {
    if (!this.isSpawned || !this.isActiveAndEnabled)
      return;
    DebugUtil.DevAssert(this.isRegistered, "shouldn't be refreshing if we aren't spawned and enabled");
    int num = (int) this.RefreshShapeAndPosition();
    this.emitter.Refresh(this.pending_emitter_state, true);
  }

  public void FullRemove()
  {
    this.RemoveFromScenePartitioner();
    this.emitter.RemoveFromGrid();
  }

  public Light2D.RefreshResult RefreshShapeAndPosition()
  {
    if (!this.isSpawned)
      return Light2D.RefreshResult.None;
    if (!this.isActiveAndEnabled)
    {
      this.FullRemove();
      return Light2D.RefreshResult.Removed;
    }
    int cell = Grid.PosToCell(this.transform.GetPosition() + (Vector3) this.Offset);
    if (!Grid.IsValidCell(cell))
    {
      this.FullRemove();
      return Light2D.RefreshResult.Removed;
    }
    this.origin = cell;
    if (this.dirty_shape)
    {
      this.RemoveFromScenePartitioner();
      this.AddToScenePartitioner();
    }
    else if (this.dirty_position)
      this.MoveInScenePartitioner();
    this.dirty_shape = false;
    this.dirty_position = false;
    return Light2D.RefreshResult.Updated;
  }

  private void OnWorldChanged(object data)
  {
    this.FullRefresh();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, (object) this.Range), (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false)
    };
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return this.GetDescriptors(def.BuildingComplete);
  }

  public enum RefreshResult
  {
    None,
    Removed,
    Updated,
  }
}
