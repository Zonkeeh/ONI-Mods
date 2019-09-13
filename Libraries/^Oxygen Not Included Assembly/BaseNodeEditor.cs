// Decompiled with JetBrains decompiler
// Type: BaseNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(true, "Noise/Base Noise Node", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class BaseNodeEditor : NodeEditorFramework.Node
{
  private const string Id = "baseNodeEditor";

  public virtual System.Type GetObjectType
  {
    get
    {
      return typeof (BaseNodeEditor);
    }
  }

  public override string GetID
  {
    get
    {
      return "baseNodeEditor";
    }
  }

  public virtual NoiseBase GetTarget()
  {
    return (NoiseBase) null;
  }

  protected SampleSettings settings
  {
    get
    {
      NoiseNodeCanvas curNodeCanvas = NodeEditor.curNodeCanvas as NoiseNodeCanvas;
      if ((UnityEngine.Object) curNodeCanvas != (UnityEngine.Object) null)
        return curNodeCanvas.settings;
      return (SampleSettings) null;
    }
  }

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    return (NodeEditorFramework.Node) null;
  }

  protected override void NodeGUI()
  {
    GUILayout.BeginHorizontal();
    if (this.Inputs != null)
    {
      GUILayout.BeginVertical();
      foreach (NodeKnob input in this.Inputs)
        input.DisplayLayout();
      GUILayout.EndVertical();
    }
    if (this.Outputs != null)
    {
      GUILayout.BeginVertical();
      foreach (NodeKnob output in this.Outputs)
        output.DisplayLayout();
      GUILayout.EndVertical();
    }
    GUILayout.EndHorizontal();
    if (!GUI.changed)
      return;
    NodeEditor.RecalculateFrom((NodeEditorFramework.Node) this);
  }
}
