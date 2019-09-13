// Decompiled with JetBrains decompiler
// Type: GraphedLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

[Serializable]
public class GraphedLine : KMonoBehaviour
{
  private Vector2[] points = new Vector2[0];
  public UILineRenderer line_renderer;
  public LineLayer layer;
  [SerializeField]
  private GameObject highlightPoint;

  public int PointCount
  {
    get
    {
      return this.points.Length;
    }
  }

  public void SetPoints(Vector2[] points)
  {
    this.points = points;
    this.UpdatePoints();
  }

  private void UpdatePoints()
  {
    Vector2[] vector2Array = new Vector2[this.points.Length];
    for (int index = 0; index < vector2Array.Length; ++index)
      vector2Array[index] = this.layer.graph.GetRelativePosition(this.points[index]);
    this.line_renderer.Points = vector2Array;
  }

  public Vector2 GetClosestDataToPointOnXAxis(Vector2 toPoint)
  {
    float num = this.layer.graph.axis_x.min_value + this.layer.graph.axis_x.range * (toPoint.x / this.layer.graph.rectTransform().sizeDelta.x);
    Vector2 vector2 = Vector2.zero;
    foreach (Vector2 point in this.points)
    {
      if ((double) Mathf.Abs(point.x - num) < (double) Mathf.Abs(vector2.x - num))
        vector2 = point;
    }
    return vector2;
  }

  public void HidePointHighlight()
  {
    this.highlightPoint.SetActive(false);
  }

  public void SetPointHighlight(Vector2 point)
  {
    this.highlightPoint.SetActive(true);
    Vector2 relativePosition = this.layer.graph.GetRelativePosition(point);
    this.highlightPoint.rectTransform().SetLocalPosition((Vector3) new Vector2((float) ((double) relativePosition.x * (double) this.layer.graph.rectTransform().sizeDelta.x - (double) this.layer.graph.rectTransform().sizeDelta.x / 2.0), (float) ((double) relativePosition.y * (double) this.layer.graph.rectTransform().sizeDelta.y - (double) this.layer.graph.rectTransform().sizeDelta.y / 2.0)));
    ToolTip component = this.layer.graph.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    component.tooltipPositionOffset = new Vector2(this.highlightPoint.rectTransform().localPosition.x, (float) ((double) this.layer.graph.rectTransform().rect.height / 2.0 - 12.0));
    component.SetSimpleTooltip(this.layer.graph.axis_x.name + " " + (object) point.x + ", " + (object) Mathf.RoundToInt(point.y) + " " + this.layer.graph.axis_y.name);
    ToolTipScreen.Instance.SetToolTip(component);
  }
}
