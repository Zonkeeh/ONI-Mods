// Decompiled with JetBrains decompiler
// Type: FloatPointsNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Terrace Control", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class FloatPointsNodeEditor : BaseNodeEditor
{
  private static float height = 100f;
  [SerializeField]
  public FloatList target = new FloatList();
  private const string Id = "floatPointsNodeEditor";

  public override string GetID
  {
    get
    {
      return "floatPointsNodeEditor";
    }
  }

  public override System.Type GetObjectType
  {
    get
    {
      return typeof (FloatPointsNodeEditor);
    }
  }

  public override NoiseBase GetTarget()
  {
    return (NoiseBase) this.target;
  }

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    FloatPointsNodeEditor instance = ScriptableObject.CreateInstance<FloatPointsNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, FloatPointsNodeEditor.height);
    instance.name = "Terrace Control";
    instance.CreateOutput("Terrace", "FloatList", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    this.Outputs[0].SetValue<FloatList>(this.target);
    return true;
  }

  protected override void NodeGUI()
  {
    base.NodeGUI();
  }
}
