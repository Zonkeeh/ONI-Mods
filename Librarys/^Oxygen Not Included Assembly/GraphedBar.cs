// Decompiled with JetBrains decompiler
// Type: GraphedBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GraphedBar : KMonoBehaviour
{
  private List<GameObject> segments = new List<GameObject>();
  public GameObject segments_container;
  public GameObject prefab_segment;
  private GraphedBarFormatting format;

  public void SetFormat(GraphedBarFormatting format)
  {
    this.format = format;
  }

  public void SetValues(int[] values, float x_position)
  {
    this.ClearValues();
    this.gameObject.rectTransform().anchorMin = new Vector2(x_position, 0.0f);
    this.gameObject.rectTransform().anchorMax = new Vector2(x_position, 1f);
    this.gameObject.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float) this.format.width);
    for (int index = 0; index < values.Length; ++index)
    {
      GameObject gameObject = Util.KInstantiateUI(this.prefab_segment, this.segments_container, true);
      LayoutElement component = gameObject.GetComponent<LayoutElement>();
      component.preferredHeight = (float) values[index];
      component.minWidth = (float) this.format.width;
      gameObject.GetComponent<Image>().color = this.format.colors[index % this.format.colors.Length];
      this.segments.Add(gameObject);
    }
  }

  public void ClearValues()
  {
    foreach (UnityEngine.Object segment in this.segments)
      UnityEngine.Object.DestroyImmediate(segment);
    this.segments.Clear();
  }
}
