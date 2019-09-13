// Decompiled with JetBrains decompiler
// Type: DoorTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransitionLayer : TransitionDriver.OverrideLayer
{
  private List<Door> doors = new List<Door>();
  private Door targetDoor;

  public DoorTransitionLayer(Navigator navigator)
    : base(navigator)
  {
  }

  public override void Destroy()
  {
    base.Destroy();
  }

  private bool AreAllDoorsOpen()
  {
    foreach (Door door in this.doors)
    {
      if ((UnityEngine.Object) door != (UnityEngine.Object) null && !door.IsOpen())
        return false;
    }
    return true;
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    int cell1 = Grid.PosToCell((KMonoBehaviour) navigator);
    int cell2 = Grid.OffsetCell(cell1, transition.x, transition.y);
    this.AddDoor(cell2);
    if (navigator.CurrentNavType != NavType.Tube)
      this.AddDoor(Grid.CellAbove(cell2));
    for (int index = 0; index < transition.navGridTransition.voidOffsets.Length; ++index)
      this.AddDoor(Grid.OffsetCell(cell1, transition.navGridTransition.voidOffsets[index]));
    if (this.doors.Count > 0 && !this.AreAllDoorsOpen())
    {
      transition.anim = navigator.NavGrid.GetIdleAnim(navigator.CurrentNavType);
      transition.isLooping = false;
      transition.end = transition.start;
      transition.speed = 1f;
      transition.animSpeed = 1f;
      transition.x = 0;
      transition.y = 0;
      transition.isCompleteCB = (Func<bool>) (() => this.AreAllDoorsOpen());
    }
    foreach (Door door in this.doors)
    {
      double num = (double) door.Open();
    }
  }

  public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.UpdateTransition(navigator, transition);
  }

  public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.EndTransition(navigator, transition);
    foreach (Door door in this.doors)
    {
      if ((UnityEngine.Object) door != (UnityEngine.Object) null)
        door.Close();
    }
    this.doors.Clear();
  }

  private void AddDoor(int cell)
  {
    Door door = this.GetDoor(cell);
    if (!((UnityEngine.Object) door != (UnityEngine.Object) null) || this.doors.Contains(door))
      return;
    this.doors.Add(door);
  }

  private Door GetDoor(int cell)
  {
    if (!Grid.HasDoor[cell])
      return (Door) null;
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      Door component = gameObject.GetComponent<Door>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.isSpawned)
        return component;
    }
    return (Door) null;
  }
}
