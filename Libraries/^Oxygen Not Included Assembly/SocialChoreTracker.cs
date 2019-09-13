// Decompiled with JetBrains decompiler
// Type: SocialChoreTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class SocialChoreTracker
{
  public Func<int, Chore> CreateChoreCB;
  public int choreCount;
  private GameObject owner;
  private CellOffset[] choreOffsets;
  private Chore[] chores;
  private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;
  private bool updating;

  public SocialChoreTracker(GameObject owner, CellOffset[] chore_offsets)
  {
    this.owner = owner;
    this.choreOffsets = chore_offsets;
    this.chores = new Chore[this.choreOffsets.Length];
    Extents extents = new Extents(Grid.PosToCell(owner), this.choreOffsets);
    this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("PrintingPodSocialize", (object) owner, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new System.Action<object>(this.OnCellChanged));
  }

  public void Update(bool update = true)
  {
    if (this.updating)
      return;
    this.updating = true;
    int num = 0;
    for (int index = 0; index < this.choreOffsets.Length; ++index)
    {
      CellOffset choreOffset = this.choreOffsets[index];
      Chore chore = this.chores[index];
      if (update && num < this.choreCount && this.IsOffsetValid(choreOffset))
      {
        ++num;
        if (chore == null || chore.isComplete)
          this.chores[index] = this.CreateChoreCB == null ? (Chore) null : this.CreateChoreCB(index);
      }
      else if (chore != null)
      {
        chore.Cancel("locator invalidated");
        this.chores[index] = (Chore) null;
      }
    }
    this.updating = false;
  }

  private void OnCellChanged(object data)
  {
    if (!this.owner.HasTag(GameTags.Operational))
      return;
    this.Update(true);
  }

  public void Clear()
  {
    GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
    this.Update(false);
  }

  private bool IsOffsetValid(CellOffset offset)
  {
    int cell = Grid.OffsetCell(Grid.PosToCell(this.owner), offset);
    int anchor_cell = Grid.CellBelow(cell);
    return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, true);
  }
}
