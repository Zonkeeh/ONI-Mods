// Decompiled with JetBrains decompiler
// Type: FogOfWarMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarMask : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    Grid.OnReveal += new System.Action<int>(this.OnReveal);
  }

  private void OnReveal(int cell)
  {
    if (Grid.PosToCell((KMonoBehaviour) this) != cell)
      return;
    Grid.OnReveal -= new System.Action<int>(this.OnReveal);
    this.gameObject.DeleteObject();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    GameUtil.FloodCollectCells(Grid.PosToCell((KMonoBehaviour) this), (Func<int, bool>) (cell =>
    {
      Grid.Visible[cell] = (byte) 0;
      Grid.PreventFogOfWarReveal[cell] = true;
      return !Grid.Solid[cell];
    }), 300, (HashSet<int>) null, true);
    GameUtil.FloodCollectCells(Grid.PosToCell((KMonoBehaviour) this), (Func<int, bool>) (cell =>
    {
      bool flag = Grid.PreventFogOfWarReveal[cell];
      if (Grid.Solid[cell] && Grid.Foundation[cell])
      {
        Grid.PreventFogOfWarReveal[cell] = true;
        Grid.Visible[cell] = (byte) 0;
        GameObject gameObject = Grid.Objects[cell, 1];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.GetComponent<KPrefabID>().PrefabTag.ToString() == "POIBunkerExteriorDoor")
        {
          Grid.PreventFogOfWarReveal[cell] = false;
          Grid.Visible[cell] = byte.MaxValue;
        }
      }
      if (!flag)
        return Grid.Foundation[cell];
      return true;
    }), 300, (HashSet<int>) null, true);
  }

  public static void ClearMask(int cell)
  {
    if (!Grid.PreventFogOfWarReveal[cell])
      return;
    int start_cell = cell;
    // ISSUE: reference to a compiler-generated field
    if (FogOfWarMask.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FogOfWarMask.\u003C\u003Ef__mg\u0024cache0 = new Func<int, bool>(FogOfWarMask.RevealFogOfWarMask);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, bool> fMgCache0 = FogOfWarMask.\u003C\u003Ef__mg\u0024cache0;
    GameUtil.FloodCollectCells(start_cell, fMgCache0, 300, (HashSet<int>) null, true);
  }

  public static bool RevealFogOfWarMask(int cell)
  {
    bool flag = Grid.PreventFogOfWarReveal[cell];
    if (flag)
    {
      Grid.PreventFogOfWarReveal[cell] = false;
      Grid.Reveal(cell, byte.MaxValue);
    }
    return flag;
  }
}
