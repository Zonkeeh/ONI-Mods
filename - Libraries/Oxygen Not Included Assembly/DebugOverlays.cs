// Decompiled with JetBrains decompiler
// Type: DebugOverlays
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class DebugOverlays : KScreen
{
  public static DebugOverlays instance { get; private set; }

  protected override void OnPrefabInit()
  {
    DebugOverlays.instance = this;
    KPopupMenu componentInChildren = this.GetComponentInChildren<KPopupMenu>();
    componentInChildren.SetOptions((IList<string>) new string[5]
    {
      "None",
      "Rooms",
      "Lighting",
      "Style",
      "Flow"
    });
    componentInChildren.OnSelect += new System.Action<string, int>(this.OnSelect);
    this.gameObject.SetActive(false);
  }

  private void OnSelect(string str, int index)
  {
    if (str != null)
    {
      if (!(str == "None"))
      {
        if (!(str == "Flow"))
        {
          if (!(str == "Lighting"))
          {
            if (str == "Rooms")
            {
              SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
              return;
            }
          }
          else
          {
            SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
            return;
          }
        }
        else
        {
          SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
          return;
        }
      }
      else
      {
        SimDebugView.Instance.SetMode(OverlayModes.None.ID);
        return;
      }
    }
    Debug.LogError((object) ("Unknown debug view: " + str));
  }
}
