// Decompiled with JetBrains decompiler
// Type: Radiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class Radiator : KMonoBehaviour, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Radiator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Radiator>((System.Action<Radiator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Radiator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<Radiator>((System.Action<Radiator, object>) ((component, data) => component.OnOperationalChanged(data)));
  public Color Color = Color.white;
  public float Range = 5f;
  public int Lux = 1000;
  private int cell = Grid.InvalidCell;
  private List<int> litCells = new List<int>();
  public float Angle;
  public Vector2 Direction;
  public Vector2 Offset;
  public bool drawOverlay;
  public Color overlayColour;
  public LightShape shape;
  public MaterialPropertyBlock materialPropertyBlock;
  private bool isRegistered;
  private HandleVector<int>.Handle solidPartitionerEntry;
  private HandleVector<int>.Handle liquidPartitionerEntry;
  private RadiationGridEmitter emitter;

  public float IntensityAnimation { get; set; }

  protected override void OnPrefabInit()
  {
    this.Subscribe<Radiator>(-592767678, Radiator.OnOperationalChangedDelegate);
    this.Subscribe<Radiator>(824508782, Radiator.OnActiveChangedDelegate);
    this.IntensityAnimation = 1f;
  }

  protected override void OnCmpEnable()
  {
    this.materialPropertyBlock = new MaterialPropertyBlock();
    base.OnCmpEnable();
    Components.Radiators.Add(this);
    if (this.isSpawned)
      this.Refresh();
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged), "Radiator.OnCmpEnable");
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Refresh();
  }

  protected override void OnCmpDisable()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged));
    Components.Radiators.Remove(this);
    base.OnCmpDisable();
    this.Refresh();
  }

  protected override void OnCleanUp()
  {
    this.UnregisterLight();
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
  }

  private void OnCellChanged()
  {
    this.GetComponent<Radiator>().Refresh();
  }

  private void UnregisterLight()
  {
    if (this.isRegistered && Grid.IsValidCell(this.cell))
    {
      GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
      GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
      this.isRegistered = false;
    }
    if (this.emitter == null)
      return;
    this.emitter.Remove();
  }

  [ContextMenu("Refresh")]
  public void Refresh()
  {
    this.UnregisterLight();
    Operational component = this.GetComponent<Operational>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.IsOperational || !this.isActiveAndEnabled)
      return;
    Vector3 pos = this.transform.GetPosition();
    pos = new Vector3(pos.x + this.Offset.x, pos.y + this.Offset.y, pos.z);
    int cell = Grid.PosToCell(pos);
    if (!Grid.IsValidCell(cell))
      return;
    Vector2I xy = Grid.CellToXY(cell);
    int range = (int) this.Range;
    if (this.shape == LightShape.Circle)
    {
      Vector2I vector2I = new Vector2I(xy.x - range, xy.y - range);
      this.solidPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (Radiator), (object) this.gameObject, vector2I.x, vector2I.y, 2 * range, 2 * range, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.TriggerRefresh));
      this.liquidPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (Radiator), (object) this.gameObject, vector2I.x, vector2I.y, 2 * range, 2 * range, GameScenePartitioner.Instance.liquidChangedLayer, new System.Action<object>(this.TriggerRefresh));
    }
    else if (this.shape == LightShape.Cone)
    {
      Vector2I vector2I = new Vector2I(xy.x - range, xy.y - range);
      this.solidPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (Radiator), (object) this.gameObject, vector2I.x, vector2I.y, 2 * range, range, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.TriggerRefresh));
      this.liquidPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (Radiator), (object) this.gameObject, vector2I.x, vector2I.y, 2 * range, range, GameScenePartitioner.Instance.liquidChangedLayer, new System.Action<object>(this.TriggerRefresh));
    }
    this.cell = cell;
    this.litCells.Clear();
    this.emitter = new RadiationGridEmitter(this.cell, this.litCells, this.Lux, this.Range, this.shape, 0.5f);
    this.emitter.Add();
    this.isRegistered = true;
  }

  private void TriggerRefresh(object data)
  {
    this.Refresh();
  }

  private void OnOperationalChanged(object data)
  {
    this.enabled = this.GetComponent<Operational>().IsActive;
    this.Refresh();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, (object) this.Range), (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false)
    };
  }
}
