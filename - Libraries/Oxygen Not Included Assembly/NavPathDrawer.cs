// Decompiled with JetBrains decompiler
// Type: NavPathDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class NavPathDrawer : KMonoBehaviour
{
  private PathFinder.Path path;
  public Material material;
  private Vector3 navigatorPos;
  private Navigator navigator;

  public static NavPathDrawer Instance { get; private set; }

  public static void DestroyInstance()
  {
    NavPathDrawer.Instance = (NavPathDrawer) null;
  }

  protected override void OnPrefabInit()
  {
    this.material = new Material(Shader.Find("Lines/Colored Blended"));
    NavPathDrawer.Instance = this;
  }

  protected override void OnCleanUp()
  {
    NavPathDrawer.Instance = (NavPathDrawer) null;
  }

  public void DrawPath(Vector3 navigator_pos, PathFinder.Path path)
  {
    this.navigatorPos = navigator_pos;
    this.navigatorPos.y += 0.5f;
    this.path = path;
  }

  public Navigator GetNavigator()
  {
    return this.navigator;
  }

  public void SetNavigator(Navigator navigator)
  {
    this.navigator = navigator;
  }

  public void ClearNavigator()
  {
    this.navigator = (Navigator) null;
  }

  private void DrawPath(PathFinder.Path path, Vector3 navigator_pos, Color color)
  {
    if (path.nodes == null || path.nodes.Count <= 1)
      return;
    GL.PushMatrix();
    this.material.SetPass(0);
    GL.Begin(1);
    GL.Color(color);
    GL.Vertex(navigator_pos);
    GL.Vertex(NavTypeHelper.GetNavPos(path.nodes[1].cell, path.nodes[1].navType));
    for (int index = 1; index < path.nodes.Count - 1; ++index)
    {
      Vector3 navPos1 = NavTypeHelper.GetNavPos(path.nodes[index].cell, path.nodes[index].navType);
      Vector3 navPos2 = NavTypeHelper.GetNavPos(path.nodes[index + 1].cell, path.nodes[index + 1].navType);
      GL.Vertex(navPos1);
      GL.Vertex(navPos2);
    }
    GL.End();
    GL.PopMatrix();
  }

  private void OnPostRender()
  {
    this.DrawPath(this.path, this.navigatorPos, Color.white);
    this.path = new PathFinder.Path();
    this.DebugDrawSelectedNavigator();
    if (!((Object) this.navigator != (Object) null))
      return;
    GL.PushMatrix();
    this.material.SetPass(0);
    GL.Begin(1);
    this.navigator.RunQuery((PathFinderQuery) PathFinderQueries.drawNavGridQuery.Reset((MinionBrain) null));
    GL.End();
    GL.PopMatrix();
  }

  private void DebugDrawSelectedNavigator()
  {
    if (!DebugHandler.DebugPathFinding || (Object) SelectTool.Instance == (Object) null || (Object) SelectTool.Instance.selected == (Object) null)
      return;
    Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
    if ((Object) component == (Object) null)
      return;
    int mouseCell = DebugHandler.GetMouseCell();
    if (!Grid.IsValidCell(mouseCell))
      return;
    PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell((KMonoBehaviour) component), component.CurrentNavType, component.flags);
    PathFinder.Path path = new PathFinder.Path();
    PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, (PathFinderQuery) PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
    string text = string.Empty + "Source: " + (object) Grid.PosToCell((KMonoBehaviour) component) + "\n" + "Dest: " + (object) mouseCell + "\n" + "Cost: " + (object) path.cost;
    this.DrawPath(path, component.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), Color.green);
    DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
  }
}
