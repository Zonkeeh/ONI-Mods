// Decompiled with JetBrains decompiler
// Type: ModifierModuleNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Modifier;
using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Modify", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class ModifierModuleNodeEditor : BaseNodeEditor
{
  [SerializeField]
  public ProcGen.Noise.Modifier target = new ProcGen.Noise.Modifier();
  private const string Id = "modifierModuleNodeEditor";

  public override string GetID
  {
    get
    {
      return "modifierModuleNodeEditor";
    }
  }

  public override System.Type GetObjectType
  {
    get
    {
      return typeof (ModifierModuleNodeEditor);
    }
  }

  public override NoiseBase GetTarget()
  {
    return (NoiseBase) this.target;
  }

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    ModifierModuleNodeEditor instance = ScriptableObject.CreateInstance<ModifierModuleNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, 100f);
    instance.name = "Modify";
    instance.CreateInput("Source", "IModule3D", NodeSide.Left, 10f);
    instance.CreateInput("Curve Control", "ControlPoints", NodeSide.Left, 50f);
    instance.CreateInput("Terrace Control", "FloatList", NodeSide.Left, 60f);
    instance.CreateOutput("Next Node", "IModule3D", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    IModule3D sourceModule = this.Inputs[0].GetValue<IModule3D>();
    if (sourceModule == null)
      return false;
    ControlPointList controlPointList = this.Inputs[1].GetValue<ControlPointList>();
    if (this.target.modifyType == ProcGen.Noise.Modifier.ModifyType.Curve && (controlPointList == null || controlPointList.points.Count == 0))
      return false;
    FloatList floatList = this.Inputs[2].GetValue<FloatList>();
    if (this.target.modifyType == ProcGen.Noise.Modifier.ModifyType.Terrace && (floatList == null || floatList.points.Count == 0))
      return false;
    IModule3D module = this.target.CreateModule(sourceModule);
    if (module == null)
      return false;
    if (this.target.modifyType == ProcGen.Noise.Modifier.ModifyType.Curve)
    {
      Curve curve = module as Curve;
      curve.ClearControlPoints();
      foreach (ControlPoint control in controlPointList.GetControls())
        curve.AddControlPoint(control);
    }
    else if (this.target.modifyType == ProcGen.Noise.Modifier.ModifyType.Terrace)
    {
      Terrace terrace = module as Terrace;
      terrace.ClearControlPoints();
      foreach (float point in floatList.points)
        terrace.AddControlPoint(point);
    }
    this.Outputs[0].SetValue<IModule3D>(module);
    return true;
  }

  protected override void NodeGUI()
  {
    base.NodeGUI();
  }
}
