// Decompiled with JetBrains decompiler
// Type: GraphBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class GraphBase : KMonoBehaviour
{
  protected List<GameObject> guides = new List<GameObject>();
  [Header("Axis")]
  public GraphAxis axis_x;
  public GraphAxis axis_y;
  [Header("References")]
  public GameObject prefab_guide_x;
  public GameObject prefab_guide_y;
  public GameObject prefab_guide_horizontal_label;
  public GameObject prefab_guide_vertical_label;
  public GameObject guides_x;
  public GameObject guides_y;
  public LocText label_title;
  public LocText label_x;
  public LocText label_y;
  public string graphName;

  public Vector2 GetRelativePosition(Vector2 absolute_point)
  {
    Vector2 zero = Vector2.zero;
    float num1 = Mathf.Max(1f, this.axis_x.max_value - this.axis_x.min_value);
    float num2 = absolute_point.x - this.axis_x.min_value;
    zero.x = num2 / num1;
    float num3 = Mathf.Max(1f, this.axis_y.max_value - this.axis_y.min_value);
    float num4 = absolute_point.y - this.axis_y.min_value;
    zero.y = num4 / num3;
    return zero;
  }

  public Vector2 GetRelativeSize(Vector2 absolute_size)
  {
    return this.GetRelativePosition(absolute_size);
  }

  public void ClearGuides()
  {
    foreach (GameObject guide in this.guides)
    {
      if ((Object) guide != (Object) null)
        Object.DestroyImmediate((Object) guide);
    }
    this.guides.Clear();
  }

  public void RefreshGuides()
  {
    this.ClearGuides();
    int num = 2;
    GameObject parent1 = Util.KInstantiateUI(this.prefab_guide_y, this.guides_y, true);
    parent1.name = "guides_vertical";
    Vector2[] vector2Array1 = new Vector2[num * (int) ((double) this.axis_x.range / (double) this.axis_x.guide_frequency)];
    for (int index = 0; index < vector2Array1.Length; index += num)
    {
      Vector2 absolute_point1 = new Vector2((float) index * (this.axis_x.guide_frequency / (float) num), this.axis_y.min_value);
      vector2Array1[index] = this.GetRelativePosition(absolute_point1);
      Vector2 absolute_point2 = new Vector2((float) index * (this.axis_x.guide_frequency / (float) num), this.axis_y.max_value);
      vector2Array1[index + 1] = this.GetRelativePosition(absolute_point2);
      GameObject go = Util.KInstantiateUI(this.prefab_guide_vertical_label, parent1, true);
      go.GetComponent<LocText>().alignment = TextAlignmentOptions.Bottom;
      go.GetComponent<LocText>().text = ((int) this.axis_x.guide_frequency * (index / num)).ToString();
      go.rectTransform().SetLocalPosition((Vector3) (new Vector2((float) index * (this.gameObject.rectTransform().rect.width / (float) vector2Array1.Length), 4f) - this.gameObject.rectTransform().rect.size / 2f));
    }
    parent1.GetComponent<UILineRenderer>().Points = vector2Array1;
    this.guides.Add(parent1);
    GameObject parent2 = Util.KInstantiateUI(this.prefab_guide_x, this.guides_x, true);
    parent2.name = "guides_horizontal";
    Vector2[] vector2Array2 = new Vector2[num * (int) ((double) this.axis_y.range / (double) this.axis_y.guide_frequency)];
    for (int index = 0; index < vector2Array2.Length; index += num)
    {
      Vector2 absolute_point1 = new Vector2(this.axis_x.min_value, (float) index * (this.axis_y.guide_frequency / (float) num));
      vector2Array2[index] = this.GetRelativePosition(absolute_point1);
      Vector2 absolute_point2 = new Vector2(this.axis_x.max_value, (float) index * (this.axis_y.guide_frequency / (float) num));
      vector2Array2[index + 1] = this.GetRelativePosition(absolute_point2);
      GameObject go = Util.KInstantiateUI(this.prefab_guide_horizontal_label, parent2, true);
      go.GetComponent<LocText>().alignment = TextAlignmentOptions.MidlineLeft;
      go.GetComponent<LocText>().text = ((int) this.axis_y.guide_frequency * (index / num)).ToString();
      go.rectTransform().SetLocalPosition((Vector3) (new Vector2(8f, (float) index * (this.gameObject.rectTransform().rect.height / (float) vector2Array2.Length)) - this.gameObject.rectTransform().rect.size / 2f));
    }
    parent2.GetComponent<UILineRenderer>().Points = vector2Array2;
    this.guides.Add(parent2);
  }
}
