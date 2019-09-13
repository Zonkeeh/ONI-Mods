// Decompiled with JetBrains decompiler
// Type: ControlPointsNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Curve Control", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class ControlPointsNodeEditor : BaseNodeEditor
{
  private static float height = 100f;
  [SerializeField]
  public ControlPointList target = new ControlPointList();
  private const string Id = "controlPointsNodeEditor";

  public override string GetID
  {
    get
    {
      return "controlPointsNodeEditor";
    }
  }

  public override System.Type GetObjectType
  {
    get
    {
      return typeof (ControlPointsNodeEditor);
    }
  }

  public override NoiseBase GetTarget()
  {
    return (NoiseBase) this.target;
  }

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    ControlPointsNodeEditor instance = ScriptableObject.CreateInstance<ControlPointsNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, ControlPointsNodeEditor.height);
    instance.name = "Curve Control";
    instance.CreateOutput("Curve", "ControlPoints", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    this.Outputs[0].SetValue<ControlPointList>(this.target);
    return true;
  }

  protected override void NodeGUI()
  {
    base.NodeGUI();
  }
}
