// Decompiled with JetBrains decompiler
// Type: LineLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LineLayer : GraphLayer
{
  private List<GraphedLine> lines = new List<GraphedLine>();
  [Header("Lines")]
  public LineLayer.LineFormat[] line_formatting;
  public GameObject prefab_line;
  public GameObject line_container;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  public GraphedLine NewLine(Tuple<float, float>[] points, string ID = "")
  {
    Vector2[] points1 = new Vector2[points.Length];
    for (int index = 0; index < points.Length; ++index)
      points1[index] = new Vector2(points[index].first, points[index].second);
    return this.NewLine(points1, ID, 128, LineLayer.DataScalingType.DropValues);
  }

  public GraphedLine NewLine(
    Vector2[] points,
    string ID = "",
    int compressDataToPointCount = 128,
    LineLayer.DataScalingType compressType = LineLayer.DataScalingType.DropValues)
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab_line, this.line_container, true);
    if (ID == string.Empty)
      ID = this.lines.Count.ToString();
    gameObject.name = string.Format("line_{0}", (object) ID);
    GraphedLine component = gameObject.GetComponent<GraphedLine>();
    if (points.Length > compressDataToPointCount)
    {
      Vector2[] vector2Array = new Vector2[compressDataToPointCount];
      if (compressType == LineLayer.DataScalingType.DropValues)
      {
        float num1 = (float) (points.Length - compressDataToPointCount + 1);
        float num2 = (float) points.Length / num1;
        int index1 = 0;
        float num3 = 0.0f;
        for (int index2 = 0; index2 < points.Length; ++index2)
        {
          ++num3;
          if ((double) num3 >= (double) num2)
          {
            num3 -= num2;
          }
          else
          {
            vector2Array[index1] = points[index2];
            ++index1;
          }
        }
        if (vector2Array[compressDataToPointCount - 1] == Vector2.zero)
          vector2Array[compressDataToPointCount - 1] = vector2Array[compressDataToPointCount - 2];
      }
      else
      {
        int num1 = points.Length / compressDataToPointCount;
        for (int index1 = 0; index1 < compressDataToPointCount; ++index1)
        {
          if (index1 > 0)
          {
            float num2 = 0.0f;
            switch (compressType)
            {
              case LineLayer.DataScalingType.Average:
                for (int index2 = 0; index2 < num1; ++index2)
                  num2 += points[index1 * num1 - index2].y;
                num2 /= (float) num1;
                break;
              case LineLayer.DataScalingType.Max:
                for (int index2 = 0; index2 < num1; ++index2)
                  num2 = Mathf.Max(num2, points[index1 * num1 - index2].y);
                break;
            }
            vector2Array[index1] = new Vector2(points[index1 * num1].x, num2);
          }
        }
      }
      points = vector2Array;
    }
    component.SetPoints(points);
    component.line_renderer.color = this.line_formatting[this.lines.Count % this.line_formatting.Length].color;
    component.line_renderer.LineThickness = (float) this.line_formatting[this.lines.Count % this.line_formatting.Length].thickness;
    this.lines.Add(component);
    return component;
  }

  public void ClearLines()
  {
    foreach (GraphedLine line in this.lines)
    {
      if ((UnityEngine.Object) line != (UnityEngine.Object) null && (UnityEngine.Object) line.gameObject != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) line.gameObject);
    }
    this.lines.Clear();
  }

  private void Update()
  {
    RectTransform component = this.gameObject.GetComponent<RectTransform>();
    if (!RectTransformUtility.RectangleContainsScreenPoint(component, (Vector2) Input.mousePosition))
    {
      for (int index = 0; index < this.lines.Count; ++index)
        this.lines[index].HidePointHighlight();
    }
    else
    {
      Vector2 localPoint = Vector2.zero;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.gameObject.GetComponent<RectTransform>(), (Vector2) Input.mousePosition, (Camera) null, out localPoint);
      localPoint += component.sizeDelta / 2f;
      for (int index = 0; index < this.lines.Count; ++index)
      {
        if (this.lines[index].PointCount != 0)
        {
          Vector2 dataToPointOnXaxis = this.lines[index].GetClosestDataToPointOnXAxis(localPoint);
          if (!float.IsNaN(dataToPointOnXaxis.x) && !float.IsNaN(dataToPointOnXaxis.y))
            this.lines[index].SetPointHighlight(dataToPointOnXaxis);
          else
            this.lines[index].HidePointHighlight();
        }
      }
    }
  }

  [Serializable]
  public struct LineFormat
  {
    public Color color;
    public int thickness;
  }

  public enum DataScalingType
  {
    Average,
    Max,
    DropValues,
  }
}
