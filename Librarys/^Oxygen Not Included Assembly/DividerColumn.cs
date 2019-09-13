// Decompiled with JetBrains decompiler
// Type: DividerColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class DividerColumn : TableColumn
{
  public DividerColumn(Func<bool> revealed = null, string scrollerID = "")
    : base((System.Action<IAssignableIdentity, GameObject>) ((minion, widget_go) =>
    {
      if (revealed != null)
      {
        if (revealed())
        {
          if (widget_go.activeSelf)
            return;
          widget_go.SetActive(true);
        }
        else
        {
          if (!widget_go.activeSelf)
            return;
          widget_go.SetActive(false);
        }
      }
      else
        widget_go.SetActive(true);
    }), (Comparison<IAssignableIdentity>) null, (System.Action<IAssignableIdentity, GameObject, ToolTip>) null, (System.Action<IAssignableIdentity, GameObject, ToolTip>) null, revealed, false, scrollerID)
  {
  }

  public override GameObject GetDefaultWidget(GameObject parent)
  {
    return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
  }

  public override GameObject GetMinionWidget(GameObject parent)
  {
    return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
  }

  public override GameObject GetHeaderWidget(GameObject parent)
  {
    return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
  }
}
