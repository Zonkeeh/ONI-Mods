// Decompiled with JetBrains decompiler
// Type: GridVisibleArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GridVisibleArea
{
  private GridArea[] Areas = new GridArea[3];
  private List<GridVisibleArea.Callback> Callbacks = new List<GridVisibleArea.Callback>();

  public GridArea CurrentArea
  {
    get
    {
      return this.Areas[0];
    }
  }

  public GridArea PreviousArea
  {
    get
    {
      return this.Areas[1];
    }
  }

  public GridArea PreviousPreviousArea
  {
    get
    {
      return this.Areas[2];
    }
  }

  public void Update()
  {
    this.Areas[2] = this.Areas[1];
    this.Areas[1] = this.Areas[0];
    this.Areas[0] = GridVisibleArea.GetVisibleArea();
    foreach (GridVisibleArea.Callback callback in this.Callbacks)
      callback.OnUpdate();
  }

  public void AddCallback(string name, System.Action on_update)
  {
    this.Callbacks.Add(new GridVisibleArea.Callback()
    {
      Name = name,
      OnUpdate = on_update
    });
  }

  public void Run(System.Action<int> in_view)
  {
    if (in_view == null)
      return;
    this.CurrentArea.Run(in_view);
  }

  public void Run(
    System.Action<int> outside_view,
    System.Action<int> inside_view,
    System.Action<int> inside_view_second_time)
  {
    if (outside_view != null)
      this.PreviousArea.RunOnDifference(this.CurrentArea, outside_view);
    if (inside_view != null)
      this.CurrentArea.RunOnDifference(this.PreviousArea, inside_view);
    if (inside_view_second_time == null)
      return;
    this.PreviousArea.RunOnDifference(this.PreviousPreviousArea, inside_view_second_time);
  }

  public void RunIfVisible(int cell, System.Action<int> action)
  {
    this.CurrentArea.RunIfInside(cell, action);
  }

  public static GridArea GetVisibleArea()
  {
    GridArea gridArea = new GridArea();
    if ((UnityEngine.Object) Camera.main != (UnityEngine.Object) null)
    {
      Vector3 worldPoint1 = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
      Vector3 worldPoint2 = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.GetPosition().z));
      gridArea.SetExtents(Math.Max((int) ((double) worldPoint2.x - 0.5), 0), Math.Max((int) ((double) worldPoint2.y - 0.5), 0), Math.Min((int) ((double) worldPoint1.x + 1.5), Grid.WidthInCells), Math.Min((int) ((double) worldPoint1.y + 1.5), Grid.HeightInCells));
    }
    return gridArea;
  }

  public struct Callback
  {
    public System.Action OnUpdate;
    public string Name;
  }
}
