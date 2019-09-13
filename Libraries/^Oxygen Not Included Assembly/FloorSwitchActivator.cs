// Decompiled with JetBrains decompiler
// Type: FloorSwitchActivator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class FloorSwitchActivator : KMonoBehaviour
{
  private int last_cell_occupied = -1;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  private bool registered;
  private HandleVector<int>.Handle partitionerEntry;

  public PrimaryElement PrimaryElement
  {
    get
    {
      return this.primaryElement;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Register();
    this.OnCellChange();
  }

  protected override void OnCleanUp()
  {
    this.Unregister();
    base.OnCleanUp();
  }

  private void OnCellChange()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, cell);
    if (Grid.IsValidCell(this.last_cell_occupied) && cell != this.last_cell_occupied)
      this.NotifyChanged(this.last_cell_occupied);
    this.NotifyChanged(cell);
    this.last_cell_occupied = cell;
  }

  private void NotifyChanged(int cell)
  {
    GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, (object) this);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.Register();
  }

  protected override void OnCmpDisable()
  {
    this.Unregister();
    base.OnCmpDisable();
  }

  private void Register()
  {
    if (this.registered)
      return;
    this.partitionerEntry = GameScenePartitioner.Instance.Add("FloorSwitchActivator.Register", (object) this, Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.floorSwitchActivatorLayer, (System.Action<object>) null);
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "FloorSwitchActivator.Register");
    this.registered = true;
  }

  private void Unregister()
  {
    if (!this.registered)
      return;
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    if (this.last_cell_occupied > -1)
      this.NotifyChanged(this.last_cell_occupied);
    this.registered = false;
  }
}
