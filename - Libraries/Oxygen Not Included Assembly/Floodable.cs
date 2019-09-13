// Decompiled with JetBrains decompiler
// Type: Floodable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class Floodable : KMonoBehaviour
{
  public static Operational.Flag notFloodedFlag = new Operational.Flag("not_flooded", Operational.Flag.Type.Functional);
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpGet]
  private SimCellOccupier simCellOccupier;
  [MyCmpReq]
  private Operational operational;
  private bool isFlooded;
  private HandleVector<int>.Handle partitionerEntry;

  public bool IsFlooded
  {
    get
    {
      return this.isFlooded;
    }
  }

  public BuildingDef Def
  {
    get
    {
      return this.building.Def;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Floodable.OnSpawn", (object) this.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new System.Action<object>(this.OnElementChanged));
    this.OnElementChanged((object) null);
  }

  private void OnElementChanged(object data)
  {
    bool flag = false;
    for (int index = 0; index < this.building.PlacementCells.Length; ++index)
    {
      if (Grid.IsSubstantialLiquid(this.building.PlacementCells[index], 0.35f))
      {
        flag = true;
        break;
      }
    }
    if (flag == this.isFlooded)
      return;
    this.isFlooded = flag;
    this.operational.SetFlag(Floodable.notFloodedFlag, !this.isFlooded);
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Flooded, this.isFlooded, (object) this);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }
}
