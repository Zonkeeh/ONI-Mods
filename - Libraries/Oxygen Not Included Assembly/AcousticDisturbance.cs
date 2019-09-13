// Decompiled with JetBrains decompiler
// Type: AcousticDisturbance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class AcousticDisturbance
{
  private static readonly HashedString[] PreAnims = new HashedString[2]
  {
    (HashedString) "grid_pre",
    (HashedString) "grid_loop"
  };
  private static readonly HashedString PostAnim = (HashedString) "grid_pst";
  private static float distanceDelay = 0.25f;
  private static float duration = 3f;
  private static HashSet<int> cellsInRange = new HashSet<int>();

  public static void Emit(object data, int EmissionRadius)
  {
    GameObject gameObject = (GameObject) data;
    Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
    Vector2 position1 = (Vector2) gameObject.transform.GetPosition();
    int cell1 = Grid.PosToCell(position1);
    int num = EmissionRadius * EmissionRadius;
    int max_depth = Mathf.CeilToInt((float) EmissionRadius);
    AcousticDisturbance.DetermineCellsInRadius(cell1, 0, max_depth, AcousticDisturbance.cellsInRange);
    AcousticDisturbance.DrawVisualEffect(cell1, AcousticDisturbance.cellsInRange);
    for (int index = 0; index < minionIdentities.Count; ++index)
    {
      MinionIdentity cmp = minionIdentities[index];
      if ((UnityEngine.Object) cmp.gameObject != (UnityEngine.Object) gameObject.gameObject)
      {
        Vector2 position2 = (Vector2) cmp.transform.GetPosition();
        if ((double) Vector2.SqrMagnitude(position1 - position2) <= (double) num)
        {
          int cell2 = Grid.PosToCell(position2);
          if (AcousticDisturbance.cellsInRange.Contains(cell2) && cmp.GetSMI<StaminaMonitor.Instance>().IsSleeping())
          {
            cmp.Trigger(-527751701, data);
            cmp.Trigger(1621815900, data);
          }
        }
      }
    }
    AcousticDisturbance.cellsInRange.Clear();
  }

  private static void DrawVisualEffect(int center_cell, HashSet<int> cells)
  {
    SoundEvent.PlayOneShot(GlobalResources.Instance().AcousticDisturbanceSound, Grid.CellToPos(center_cell));
    foreach (int cell in cells)
    {
      int gridDistance = AcousticDisturbance.GetGridDistance(cell, center_cell);
      GameScheduler instance = GameScheduler.Instance;
      double num = (double) AcousticDisturbance.distanceDelay * (double) gridDistance;
      // ISSUE: reference to a compiler-generated field
      if (AcousticDisturbance.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AcousticDisturbance.\u003C\u003Ef__mg\u0024cache0 = new System.Action<object>(AcousticDisturbance.SpawnEffect);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<object> fMgCache0 = AcousticDisturbance.\u003C\u003Ef__mg\u0024cache0;
      // ISSUE: variable of a boxed type
      __Boxed<int> local = (ValueType) cell;
      instance.Schedule("radialgrid_pre", (float) num, fMgCache0, (object) local, (SchedulerGroup) null);
    }
  }

  private static void SpawnEffect(object data)
  {
    Grid.SceneLayer layer = Grid.SceneLayer.InteriorWall;
    KBatchedAnimController effect = FXHelpers.CreateEffect("radialgrid_kanim", Grid.CellToPosCCC((int) data, layer), (Transform) null, false, layer, false);
    effect.destroyOnAnimComplete = false;
    effect.Play(AcousticDisturbance.PreAnims, KAnim.PlayMode.Loop);
    GameScheduler instance = GameScheduler.Instance;
    double duration = (double) AcousticDisturbance.duration;
    // ISSUE: reference to a compiler-generated field
    if (AcousticDisturbance.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AcousticDisturbance.\u003C\u003Ef__mg\u0024cache1 = new System.Action<object>(AcousticDisturbance.DestroyEffect);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<object> fMgCache1 = AcousticDisturbance.\u003C\u003Ef__mg\u0024cache1;
    KBatchedAnimController kbatchedAnimController = effect;
    instance.Schedule("radialgrid_loop", (float) duration, fMgCache1, (object) kbatchedAnimController, (SchedulerGroup) null);
  }

  private static void DestroyEffect(object data)
  {
    KBatchedAnimController kbatchedAnimController = (KBatchedAnimController) data;
    kbatchedAnimController.destroyOnAnimComplete = true;
    kbatchedAnimController.Play(AcousticDisturbance.PostAnim, KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private static int GetGridDistance(int cell, int center_cell)
  {
    Vector2I vector2I = Grid.CellToXY(cell) - Grid.CellToXY(center_cell);
    return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
  }

  private static void DetermineCellsInRadius(
    int cell,
    int depth,
    int max_depth,
    HashSet<int> cells_in_range)
  {
    if (!Grid.IsValidCell(cell) || Grid.Solid[cell])
      return;
    cells_in_range.Add(cell);
    if (depth >= max_depth)
      return;
    int depth1 = depth + 1;
    int cell1 = Grid.CellBelow(cell);
    int cell2 = Grid.CellAbove(cell);
    int cell3 = cell - 1;
    int cell4 = cell + 1;
    bool flag1 = Grid.IsValidCell(cell1) && !Grid.Solid[cell1];
    bool flag2 = Grid.IsValidCell(cell2) && !Grid.Solid[cell2];
    bool flag3 = Grid.IsValidCell(cell3) && !Grid.Solid[cell3];
    bool flag4 = Grid.IsValidCell(cell4) && !Grid.Solid[cell4];
    if (flag1 || flag3)
      AcousticDisturbance.DetermineCellsInRadius(cell1 - 1, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(cell1, depth1, max_depth, cells_in_range);
    if (flag1 || flag4)
      AcousticDisturbance.DetermineCellsInRadius(cell1 + 1, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(cell3, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(cell4, depth1, max_depth, cells_in_range);
    if (flag2 || flag3)
      AcousticDisturbance.DetermineCellsInRadius(cell2 - 1, depth1, max_depth, cells_in_range);
    AcousticDisturbance.DetermineCellsInRadius(cell2, depth1, max_depth, AcousticDisturbance.cellsInRange);
    if (!flag2 && !flag4)
      return;
    AcousticDisturbance.DetermineCellsInRadius(cell2 + 1, depth1, max_depth, cells_in_range);
  }
}
