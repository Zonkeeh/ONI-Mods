// Decompiled with JetBrains decompiler
// Type: UprootedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class UprootedMonitor : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<UprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<UprootedMonitor>((System.Action<UprootedMonitor, object>) ((component, data) =>
  {
    if (component.uprooted)
      return;
    component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
    component.uprooted = true;
    component.Trigger(-216549700, (object) null);
  }));
  [Serialize]
  public bool canBeUprooted = true;
  public CellOffset monitorCell = new CellOffset(0, -1);
  private int position;
  private int ground;
  [Serialize]
  private bool uprooted;
  private HandleVector<int>.Handle partitionerEntry;

  public bool IsUprooted
  {
    get
    {
      if (!this.uprooted)
        return this.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
      return true;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<UprootedMonitor>(-216549700, UprootedMonitor.OnUprootedDelegate);
    this.position = Grid.PosToCell(this.gameObject);
    this.ground = Grid.OffsetCell(this.position, this.monitorCell);
    if (Grid.IsValidCell(this.position) && Grid.IsValidCell(this.ground))
      this.partitionerEntry = GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", (object) this.gameObject, this.ground, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnGroundChanged));
    this.OnGroundChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  public bool CheckTileGrowable()
  {
    return !this.canBeUprooted || !this.uprooted && this.IsCellSafe(this.position);
  }

  public bool IsCellSafe(int cell)
  {
    if (!Grid.IsCellOffsetValid(cell, this.monitorCell))
      return false;
    int index = Grid.OffsetCell(cell, this.monitorCell);
    return Grid.Solid[index];
  }

  public void OnGroundChanged(object callbackData)
  {
    if (this.CheckTileGrowable())
      return;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
    this.uprooted = true;
    this.Trigger(-216549700, (object) null);
  }

  public static bool IsObjectUprooted(GameObject plant)
  {
    UprootedMonitor component = plant.GetComponent<UprootedMonitor>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return false;
    return component.IsUprooted;
  }
}
