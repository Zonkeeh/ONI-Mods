// Decompiled with JetBrains decompiler
// Type: WireBuildTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class WireBuildTool : BaseUtilityBuildTool
{
  public static WireBuildTool Instance;

  public static void DestroyInstance()
  {
    WireBuildTool.Instance = (WireBuildTool) null;
  }

  protected override void OnPrefabInit()
  {
    WireBuildTool.Instance = this;
    base.OnPrefabInit();
    this.viewMode = OverlayModes.Power.ID;
  }

  protected override void ApplyPathToConduitSystem()
  {
    if (this.path.Count < 2)
      return;
    for (int index = 1; index < this.path.Count; ++index)
    {
      if (this.path[index - 1].valid && this.path[index].valid)
      {
        int cell1 = this.path[index - 1].cell;
        int cell2 = this.path[index].cell;
        UtilityConnections cell3 = UtilityConnectionsExtensions.DirectionFromToCell(cell1, this.path[index].cell);
        if (cell3 != (UtilityConnections) 0)
        {
          UtilityConnections new_connection = cell3.InverseDirection();
          this.conduitMgr.AddConnection(cell3, cell1, false);
          this.conduitMgr.AddConnection(new_connection, cell2, false);
        }
      }
    }
  }
}
