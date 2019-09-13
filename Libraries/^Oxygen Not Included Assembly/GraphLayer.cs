// Decompiled with JetBrains decompiler
// Type: GraphLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (GraphBase))]
public class GraphLayer : KMonoBehaviour
{
  [MyCmpReq]
  protected GraphBase graph_base;

  public GraphBase graph
  {
    get
    {
      if ((Object) this.graph_base == (Object) null)
        this.graph_base = this.GetComponent<GraphBase>();
      return this.graph_base;
    }
  }
}
