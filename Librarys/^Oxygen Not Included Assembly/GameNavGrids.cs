// Decompiled with JetBrains decompiler
// Type: GameNavGrids
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameNavGrids
{
  public NavGrid DuplicantGrid;
  public NavGrid WalkerGrid1x1;
  public NavGrid WalkerBabyGrid1x1;
  public NavGrid WalkerGrid1x2;
  public NavGrid DreckoGrid;
  public NavGrid DreckoBabyGrid;
  public NavGrid FloaterGrid;
  public NavGrid FlyerGrid1x2;
  public NavGrid FlyerGrid1x1;
  public NavGrid FlyerGrid2x2;
  public NavGrid SwimmerGrid;
  public NavGrid DiggerGrid;
  public NavGrid SquirrelGrid;

  public GameNavGrids(Pathfinding pathfinding)
  {
    this.CreateDuplicantNavigation(pathfinding);
    this.WalkerGrid1x1 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x1", new CellOffset[1]
    {
      new CellOffset(0, 0)
    });
    this.WalkerBabyGrid1x1 = this.CreateWalkerBabyNavigation(pathfinding, "WalkerBabyNavGrid", new CellOffset[1]
    {
      new CellOffset(0, 0)
    });
    this.WalkerGrid1x2 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x2", new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    });
    this.CreateDreckoNavigation(pathfinding);
    this.CreateDreckoBabyNavigation(pathfinding);
    this.CreateFloaterNavigation(pathfinding);
    this.FlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x1", new CellOffset[1]
    {
      new CellOffset(0, 0)
    });
    this.FlyerGrid1x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x2", new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    });
    this.FlyerGrid2x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid2x2", new CellOffset[4]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1),
      new CellOffset(1, 0),
      new CellOffset(1, 1)
    });
    this.CreateSwimmerNavigation(pathfinding);
    this.CreateDiggerNavigation(pathfinding);
    this.CreateSquirrelNavigation(pathfinding);
  }

  private void CreateDuplicantNavigation(Pathfinding pathfinding)
  {
    NavOffset[] invalid_nav_offsets = new NavOffset[3]
    {
      new NavOffset(NavType.Floor, 1, 0),
      new NavOffset(NavType.Ladder, 1, 0),
      new NavOffset(NavType.Pole, 1, 0)
    };
    CellOffset[] bounding_offsets = new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    };
    this.DuplicantGrid = new NavGrid("MinionNavGrid", this.MirrorTransitions(new NavGrid.Transition[108]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 1),
        new NavOffset(NavType.Ladder, 1, 1),
        new NavOffset(NavType.Pole, 1, 1)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[6]
      {
        new NavOffset(NavType.Floor, 1, 0),
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, -1),
        new NavOffset(NavType.Ladder, 1, -1),
        new NavOffset(NavType.Pole, 1, -1)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 1, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 1, NavAxis.NA, false, false, true, 14, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 14, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Ladder, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 0, 0)
      }, false),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Ladder, 0, 1),
        new NavOffset(NavType.Floor, 0, 1)
      }, false),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Floor, 0, -1),
        new NavOffset(NavType.Ladder, 0, -1)
      }, false),
      new NavGrid.Transition(NavType.Ladder, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 15, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, 1, NavAxis.NA, true, true, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, -1, NavAxis.NA, true, true, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 25, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 1, NavAxis.NA, false, false, true, 50, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 1, NavAxis.NA, false, false, true, 50, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Pole, 1, 0),
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 0, 0)
      }, false),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Pole, 0, 1),
        new NavOffset(NavType.Floor, 0, 1)
      }, false),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Floor, 0, -1),
        new NavOffset(NavType.Pole, 0, -1)
      }, false),
      new NavGrid.Transition(NavType.Pole, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 50, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ladder, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 20, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, 1, NavAxis.NA, true, true, true, 50, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, -1, NavAxis.NA, true, true, false, 6, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Pole, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false),
      new NavGrid.Transition(NavType.Floor, NavType.Tube, 0, 2, NavAxis.NA, false, false, false, 40, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 1, NavAxis.NA, false, false, false, 7, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 2, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 0, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 7, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -2, NavAxis.NA, false, false, false, 17, string.Empty, new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1),
        new CellOffset(2, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -2)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -2, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 2, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ladder, 0, 1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 1, NavAxis.NA, false, false, false, 7, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 1, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 2, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 7, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -2, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -1, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -2, NavAxis.NA, false, false, false, 17, string.Empty, new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1),
        new CellOffset(2, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -2)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -2, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 2, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Pole, 0, 1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 1, NavAxis.NA, false, false, false, 7, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 1, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 2, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 7, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -2, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -1, NavAxis.NA, false, false, false, 13, string.Empty, new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -1)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -2, NavAxis.NA, false, false, false, 17, string.Empty, new CellOffset[4]
      {
        new CellOffset(1, 0),
        new CellOffset(2, 0),
        new CellOffset(1, -1),
        new CellOffset(2, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, -2)
      }, false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -2, NavAxis.NA, false, false, false, 10, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 0, NavAxis.NA, true, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, 1, NavAxis.NA, true, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, -1, NavAxis.NA, true, false, false, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.Y, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 0, 1)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.X, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 1, 0)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.Y, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 0, -1)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.X, false, false, false, 10, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Tube, 1, 0)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, false, 15, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, false, 25, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Hover, 1, 0),
        new NavOffset(NavType.Hover, 0, 1)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, false, 25, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.Hover, 1, 0),
        new NavOffset(NavType.Hover, 0, -1)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Hover, 1, 0, NavAxis.NA, false, false, false, 15, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Hover, 0, 1, NavAxis.NA, false, false, false, 20, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 15, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 15, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false)
    }), new NavGrid.NavTypeData[5]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_default"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ladder,
        idleAnim = (HashedString) "ladder_idle"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Pole,
        idleAnim = (HashedString) "pole_idle"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Tube,
        idleAnim = (HashedString) "tube_idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Hover,
        idleAnim = (HashedString) "hover_hover_1_0_loop"
      }
    }, bounding_offsets, new NavTableValidator[5]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(true),
      (NavTableValidator) new GameNavGrids.LadderValidator(),
      (NavTableValidator) new GameNavGrids.PoleValidator(),
      (NavTableValidator) new GameNavGrids.TubeValidator(),
      (NavTableValidator) new GameNavGrids.FlyingValidator(true, true, true)
    }, 2, 3, 32);
    this.DuplicantGrid.updateEveryFrame = true;
    pathfinding.AddNavGrid(this.DuplicantGrid);
  }

  private NavGrid CreateWalkerNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[6]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false)
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private NavGrid CreateWalkerBabyNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[1]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false)
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private void CreateDreckoNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[24]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 3, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 4, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 1, -2)
      }, true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 2, NavAxis.NA, false, false, true, 5, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 2)
      }, new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, 0, 0)
      }, true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 4, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[2]
      {
        new NavOffset(NavType.RightWall, 0, 0),
        new NavOffset(NavType.Floor, 2, 2)
      }, true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, 0, 0)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, 0, 1)
      }, false),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, -1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, 0, 0)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, -1, 0)
      }, false),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, -1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 0, 0)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 0, -1)
      }, false),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 0, 0)
      }, new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 0)
      }, false),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 1, -1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.LeftWall, 1, -2)
      }, true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -2, -1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(0, -1),
        new CellOffset(-1, -1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, -1, -1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Ceiling, -2, -1)
      }, true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(-1, 0),
        new CellOffset(-1, 1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, -1, 1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.RightWall, -1, 2)
      }, true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 1, 1)
      }, new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Floor, 2, 1)
      }, true)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[4]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.570796f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.141593f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      }
    };
    this.DreckoGrid = new NavGrid("DreckoNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[3]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(this.DreckoGrid);
  }

  private void CreateDreckoBabyNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[12]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[4]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.570796f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.141593f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      }
    };
    this.DreckoBabyGrid = new NavGrid("DreckoBabyNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[3]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(this.DreckoBabyGrid);
  }

  private void CreateFloaterNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    this.FloaterGrid = new NavGrid("FloaterNavGrid", this.MirrorTransitions(new NavGrid.Transition[17]
    {
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, -1)
      }, false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, 0)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, -2)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 0, 0)
      }, false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 0, -2)
      }, false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 1, NavAxis.NA, false, false, true, 3, string.Empty, new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, 1),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 2, 0)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 0, NavAxis.NA, false, false, true, 3, string.Empty, new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1),
        new CellOffset(1, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 2, -1)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, -1, NavAxis.NA, false, false, true, 3, string.Empty, new CellOffset[3]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1),
        new CellOffset(1, -2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 2, -2)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 2, NavAxis.NA, false, false, true, 3, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, 1)
      }, true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -2, NavAxis.NA, false, false, true, 3, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[1]
      {
        new NavOffset(NavType.Hover, 1, -3)
      }, true),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false)
    }), new NavGrid.NavTypeData[2]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Hover,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "swim_idle_loop"
      }
    }, bounding_offsets, new NavTableValidator[2]
    {
      (NavTableValidator) new GameNavGrids.HoverValidator(),
      (NavTableValidator) new GameNavGrids.SwimValidator()
    }, 2, 2, 22);
    pathfinding.AddNavGrid(this.FloaterGrid);
  }

  private NavGrid CreateFlyerNavigation(
    Pathfinding pathfinding,
    string id,
    CellOffset[] bounding_offsets)
  {
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[12]
    {
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 2, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[2]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Hover,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    NavGrid nav_grid = new NavGrid(id, transitions, nav_type_data, bounding_offsets, new NavTableValidator[2]
    {
      (NavTableValidator) new GameNavGrids.FlyingValidator(false, false, false),
      (NavTableValidator) new GameNavGrids.SwimValidator()
    }, 2, 2, transitions.Length);
    pathfinding.AddNavGrid(nav_grid);
    return nav_grid;
  }

  private void CreateSwimmerNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[5]
    {
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[1]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Swim,
        idleAnim = (HashedString) "idle_loop"
      }
    };
    this.SwimmerGrid = new NavGrid("SwimmerNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[1]
    {
      (NavTableValidator) new GameNavGrids.SwimValidator()
    }, 1, 1, transitions.Length);
    pathfinding.AddNavGrid(this.SwimmerGrid);
  }

  private void CreateDiggerNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[24]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 0, NavAxis.NA, false, false, true, 1, "idle1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 1, NavAxis.NA, false, false, true, 1, "idle2", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 0, 1, NavAxis.NA, false, false, true, 1, "idle3", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, -1, NavAxis.NA, false, false, true, 1, "idle4", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.Solid, 0, -1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Solid, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 1, "drill_out", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.Solid, 0, 1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Solid, NavType.Ceiling, 0, -1, NavAxis.NA, false, false, true, 1, "drill_out_ceiling", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Solid, NavType.LeftWall, 1, 0, NavAxis.NA, false, false, true, 1, "drill_out_left_wall", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.Solid, -1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Solid, NavType.RightWall, -1, 0, NavAxis.NA, false, false, true, 1, "drill_out_right_wall", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.RightWall, NavType.Solid, 1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[5]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.141593f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.570796f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Solid,
        idleAnim = (HashedString) "idle1"
      }
    };
    this.DiggerGrid = new NavGrid("DiggerNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[4]
    {
      (NavTableValidator) new GameNavGrids.SolidValidator(),
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(this.DiggerGrid);
  }

  private void CreateSquirrelNavigation(Pathfinding pathfinding)
  {
    CellOffset[] bounding_offsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    NavGrid.Transition[] transitions = this.MirrorTransitions(new NavGrid.Transition[17]
    {
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, string.Empty, new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[2]
      {
        new CellOffset(0, 1),
        new CellOffset(0, 2)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, string.Empty, new CellOffset[2]
      {
        new CellOffset(1, 0),
        new CellOffset(1, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false),
      new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, -1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(-1, 0)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true),
      new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[1]
      {
        new CellOffset(0, 1)
      }, new CellOffset[0], new NavOffset[0], new NavOffset[0], true)
    });
    NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[4]
    {
      new NavGrid.NavTypeData()
      {
        navType = NavType.Floor,
        idleAnim = (HashedString) "idle_loop"
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.Ceiling,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.0f, -1f, 0.0f),
        rotation = -3.141593f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.RightWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(0.5f, -0.5f, 0.0f),
        rotation = -1.570796f
      },
      new NavGrid.NavTypeData()
      {
        navType = NavType.LeftWall,
        idleAnim = (HashedString) "idle_loop",
        animControllerOffset = (Vector2) new Vector3(-0.5f, -0.5f, 0.0f),
        rotation = -4.712389f
      }
    };
    this.SquirrelGrid = new NavGrid("SquirrelNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[3]
    {
      (NavTableValidator) new GameNavGrids.FloorValidator(false),
      (NavTableValidator) new GameNavGrids.WallValidator(),
      (NavTableValidator) new GameNavGrids.CeilingValidator()
    }, 2, 3, transitions.Length);
    pathfinding.AddNavGrid(this.SquirrelGrid);
  }

  private CellOffset[] MirrorOffsets(CellOffset[] offsets)
  {
    List<CellOffset> cellOffsetList = new List<CellOffset>();
    foreach (CellOffset offset in offsets)
    {
      offset.x = -offset.x;
      cellOffsetList.Add(offset);
    }
    return cellOffsetList.ToArray();
  }

  private NavOffset[] MirrorNavOffsets(NavOffset[] offsets)
  {
    List<NavOffset> navOffsetList = new List<NavOffset>();
    foreach (NavOffset offset in offsets)
    {
      offset.navType = NavGrid.MirrorNavType(offset.navType);
      offset.offset.x = -offset.offset.x;
      navOffsetList.Add(offset);
    }
    return navOffsetList.ToArray();
  }

  private NavGrid.Transition[] MirrorTransitions(NavGrid.Transition[] transitions)
  {
    List<NavGrid.Transition> transitionList = new List<NavGrid.Transition>();
    foreach (NavGrid.Transition transition1 in transitions)
    {
      transitionList.Add(transition1);
      if (transition1.x != (sbyte) 0 || transition1.start == NavType.RightWall || (transition1.end == NavType.RightWall || transition1.start == NavType.LeftWall) || transition1.end == NavType.LeftWall)
      {
        NavGrid.Transition transition2 = transition1;
        transition2.x = -transition2.x;
        transition2.voidOffsets = this.MirrorOffsets(transition1.voidOffsets);
        transition2.solidOffsets = this.MirrorOffsets(transition1.solidOffsets);
        transition2.validNavOffsets = this.MirrorNavOffsets(transition1.validNavOffsets);
        transition2.invalidNavOffsets = this.MirrorNavOffsets(transition1.invalidNavOffsets);
        transition2.start = NavGrid.MirrorNavType(transition2.start);
        transition2.end = NavGrid.MirrorNavType(transition2.end);
        transitionList.Add(transition2);
      }
    }
    transitionList.Sort((Comparison<NavGrid.Transition>) ((x, y) => x.cost.CompareTo(y.cost)));
    return transitionList.ToArray();
  }

  public class SwimValidator : NavTableValidator
  {
    public SwimValidator()
    {
      World.Instance.OnLiquidChanged += new System.Action<int>(this.OnLiquidChanged);
      GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[9], new System.Action<int, object>(this.OnFoundationTileChanged));
    }

    private void OnFoundationTileChanged(int cell, object unused)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool is_valid = Grid.IsSubstantialLiquid(cell, 0.35f) && this.IsClear(cell, bounding_offsets, false);
      nav_table.SetValid(cell, NavType.Swim, is_valid);
    }

    private void OnLiquidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }
  }

  public class FloorValidator : NavTableValidator
  {
    private bool isDupe;

    public FloorValidator(bool is_dupe)
    {
      World.Instance.OnSolidChanged += new System.Action<int>(this.OnSolidChanged);
      Components.Ladders.Register(new System.Action<Ladder>(this.OnAddLadder), new System.Action<Ladder>(this.OnRemoveLadder));
      this.isDupe = is_dupe;
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag = GameNavGrids.FloorValidator.IsWalkableCell(cell, Grid.CellBelow(cell), this.isDupe);
      nav_table.SetValid(cell, NavType.Floor, flag && this.IsClear(cell, bounding_offsets, this.isDupe));
    }

    public static bool IsWalkableCell(int cell, int anchor_cell, bool is_dupe)
    {
      if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(anchor_cell) || !NavTableValidator.IsCellPassable(cell, is_dupe))
        return false;
      if (Grid.FakeFloor[anchor_cell])
        return true;
      if (Grid.Solid[anchor_cell])
        return !Grid.DupePassable[anchor_cell];
      if (is_dupe)
        return (Grid.NavValidatorMasks[cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) == (Grid.NavValidatorFlags) 0 && (Grid.NavValidatorMasks[anchor_cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) != (Grid.NavValidatorFlags) 0;
      return false;
    }

    private void OnAddLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnRemoveLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new System.Action<int>(this.OnSolidChanged);
      Components.Ladders.Unregister(new System.Action<Ladder>(this.OnAddLadder), new System.Action<Ladder>(this.OnRemoveLadder));
    }
  }

  public class WallValidator : NavTableValidator
  {
    public WallValidator()
    {
      World.Instance.OnSolidChanged += new System.Action<int>(this.OnSolidChanged);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag1 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellRight(cell));
      bool flag2 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellLeft(cell));
      nav_table.SetValid(cell, NavType.RightWall, flag1 && this.IsClear(cell, bounding_offsets, false));
      nav_table.SetValid(cell, NavType.LeftWall, flag2 && this.IsClear(cell, bounding_offsets, false));
    }

    private static bool IsWalkableCell(int cell, int anchor_cell)
    {
      return Grid.IsValidCell(cell) && Grid.IsValidCell(anchor_cell) && NavTableValidator.IsCellPassable(cell, false) && (Grid.Solid[anchor_cell] || Grid.CritterImpassable[anchor_cell]);
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new System.Action<int>(this.OnSolidChanged);
    }
  }

  public class CeilingValidator : NavTableValidator
  {
    public CeilingValidator()
    {
      World.Instance.OnSolidChanged += new System.Action<int>(this.OnSolidChanged);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool flag = GameNavGrids.CeilingValidator.IsWalkableCell(cell, Grid.CellAbove(cell));
      nav_table.SetValid(cell, NavType.Ceiling, flag && this.IsClear(cell, bounding_offsets, false));
    }

    private static bool IsWalkableCell(int cell, int anchor_cell)
    {
      return Grid.IsValidCell(cell) && Grid.IsValidCell(anchor_cell) && NavTableValidator.IsCellPassable(cell, false) && (Grid.Solid[anchor_cell] || (!Grid.HasDoor[cell] || Grid.FakeFloor[cell]) && (Grid.FakeFloor[anchor_cell] || Grid.HasDoor[anchor_cell]));
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new System.Action<int>(this.OnSolidChanged);
    }
  }

  public class LadderValidator : NavTableValidator
  {
    public LadderValidator()
    {
      Components.Ladders.Register(new System.Action<Ladder>(this.OnAddLadder), new System.Action<Ladder>(this.OnRemoveLadder));
    }

    private void OnAddLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnRemoveLadder(Ladder ladder)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) ladder);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      nav_table.SetValid(cell, NavType.Ladder, this.IsClear(cell, bounding_offsets, true) && Grid.HasLadder[cell]);
    }

    public override void Clear()
    {
      Components.Ladders.Unregister(new System.Action<Ladder>(this.OnAddLadder), new System.Action<Ladder>(this.OnRemoveLadder));
    }
  }

  public class PoleValidator : GameNavGrids.LadderValidator
  {
    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      nav_table.SetValid(cell, NavType.Pole, this.IsClear(cell, bounding_offsets, true) && Grid.HasPole[cell]);
    }
  }

  public class TubeValidator : NavTableValidator
  {
    public TubeValidator()
    {
      Components.ITravelTubePieces.Register(new System.Action<ITravelTubePiece>(this.OnAddLadder), new System.Action<ITravelTubePiece>(this.OnRemoveLadder));
    }

    private void OnAddLadder(ITravelTubePiece tube)
    {
      int cell = Grid.PosToCell(tube.Position);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    private void OnRemoveLadder(ITravelTubePiece tube)
    {
      int cell = Grid.PosToCell(tube.Position);
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      nav_table.SetValid(cell, NavType.Tube, Grid.HasTube[cell]);
    }

    public override void Clear()
    {
      Components.ITravelTubePieces.Unregister(new System.Action<ITravelTubePiece>(this.OnAddLadder), new System.Action<ITravelTubePiece>(this.OnRemoveLadder));
    }
  }

  public class FlyingValidator : NavTableValidator
  {
    private bool exclude_floor;
    private bool exclude_jet_suit_blockers;
    private bool allow_door_traversal;
    private HandleVector<int>.Handle buildingParititonerEntry;

    public FlyingValidator(
      bool exclude_floor = false,
      bool exclude_jet_suit_blockers = false,
      bool allow_door_traversal = false)
    {
      this.exclude_floor = exclude_floor;
      this.exclude_jet_suit_blockers = exclude_jet_suit_blockers;
      this.allow_door_traversal = allow_door_traversal;
      World.Instance.OnSolidChanged += new System.Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged += new System.Action<int>(this.MarkCellDirty);
      GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new System.Action<int, object>(this.OnBuildingChange));
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool is_valid = false;
      if (Grid.IsValidCell(Grid.CellAbove(cell)))
      {
        is_valid = !Grid.IsSubstantialLiquid(cell, 0.35f) && this.IsClear(cell, bounding_offsets, this.allow_door_traversal);
        if (is_valid && this.exclude_floor)
        {
          int cell1 = Grid.CellBelow(cell);
          if (Grid.IsValidCell(cell1))
            is_valid = this.IsClear(cell1, bounding_offsets, this.allow_door_traversal);
        }
        if (is_valid && this.exclude_jet_suit_blockers)
        {
          GameObject go = Grid.Objects[cell, 1];
          is_valid = (UnityEngine.Object) go == (UnityEngine.Object) null || !go.HasTag(GameTags.JetSuitBlocker);
        }
      }
      nav_table.SetValid(cell, NavType.Hover, is_valid);
    }

    private void OnBuildingChange(int cell, object data)
    {
      this.MarkCellDirty(cell);
    }

    private void MarkCellDirty(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new System.Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged -= new System.Action<int>(this.MarkCellDirty);
      GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new System.Action<int, object>(this.OnBuildingChange));
    }
  }

  public class HoverValidator : NavTableValidator
  {
    public HoverValidator()
    {
      World.Instance.OnSolidChanged += new System.Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged += new System.Action<int>(this.MarkCellDirty);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      int cell1 = Grid.CellBelow(cell);
      if (!Grid.IsValidCell(cell1))
        return;
      bool flag = Grid.Solid[cell1] || Grid.FakeFloor[cell1] || Grid.IsSubstantialLiquid(cell1, 0.35f);
      nav_table.SetValid(cell, NavType.Hover, !Grid.IsSubstantialLiquid(cell, 0.35f) && flag && this.IsClear(cell, bounding_offsets, false));
    }

    private void MarkCellDirty(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new System.Action<int>(this.MarkCellDirty);
      World.Instance.OnLiquidChanged -= new System.Action<int>(this.MarkCellDirty);
    }
  }

  public class SolidValidator : NavTableValidator
  {
    public SolidValidator()
    {
      World.Instance.OnSolidChanged += new System.Action<int>(this.OnSolidChanged);
    }

    public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
    {
      bool is_valid = GameNavGrids.SolidValidator.IsDiggable(cell, Grid.CellBelow(cell));
      nav_table.SetValid(cell, NavType.Solid, is_valid);
    }

    public static bool IsDiggable(int cell, int anchor_cell)
    {
      if (Grid.IsValidCell(cell) && Grid.Solid[cell])
      {
        if (Grid.HasDoor[cell] || Grid.Foundation[cell])
        {
          GameObject gameObject = Grid.Objects[cell, 1];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
            if (Grid.Element[cell].hardness < (byte) 150)
              return !component.Element.HasTag(GameTags.RefinedMetal);
            return false;
          }
        }
        else
        {
          byte num = Grid.ElementIdx[cell];
          Element element = ElementLoader.elements[(int) num];
          if (Grid.Element[cell].hardness < (byte) 150)
            return !element.HasTag(GameTags.RefinedMetal);
          return false;
        }
      }
      return false;
    }

    private void OnSolidChanged(int cell)
    {
      if (this.onDirty == null)
        return;
      this.onDirty(cell);
    }

    public override void Clear()
    {
      World.Instance.OnSolidChanged -= new System.Action<int>(this.OnSolidChanged);
    }
  }
}
