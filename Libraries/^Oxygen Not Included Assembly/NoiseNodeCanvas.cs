// Decompiled with JetBrains decompiler
// Type: NoiseNodeCanvas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using ProcGen.Noise;
using System.Collections.Generic;
using UnityEngine;

[NodeCanvasType("Noise Canvas")]
public class NoiseNodeCanvas : NodeCanvas
{
  private Dictionary<string, PrimitiveNodeEditor> primitiveLookup = new Dictionary<string, PrimitiveNodeEditor>();
  private Dictionary<string, FilterNodeEditor> filterLookup = new Dictionary<string, FilterNodeEditor>();
  private Dictionary<string, ModifierModuleNodeEditor> modifierLookup = new Dictionary<string, ModifierModuleNodeEditor>();
  private Dictionary<string, SelectorModuleNodeEditor> selectorLookup = new Dictionary<string, SelectorModuleNodeEditor>();
  private Dictionary<string, TransformerNodeEditor> transformerLookup = new Dictionary<string, TransformerNodeEditor>();
  private Dictionary<string, CombinerModuleNodeEditor> combinerLookup = new Dictionary<string, CombinerModuleNodeEditor>();
  private Dictionary<string, FloatPointsNodeEditor> floatlistLookup = new Dictionary<string, FloatPointsNodeEditor>();
  private Dictionary<string, ControlPointsNodeEditor> ctrlpointsLookup = new Dictionary<string, ControlPointsNodeEditor>();
  private NoiseTreeFiles ntf;
  private TerminalNodeEditor terminator;
  private Rect lastRectPos;

  [SerializeField]
  public SampleSettings settings { get; private set; }

  public static NoiseNodeCanvas CreateInstance()
  {
    NoiseNodeCanvas instance = ScriptableObject.CreateInstance<NoiseNodeCanvas>();
    instance.ntf = YamlIO.LoadFile<NoiseTreeFiles>(NoiseTreeFiles.GetPath(), (YamlIO.ErrorHandler) null, (List<Tuple<string, System.Type>>) null);
    return instance;
  }

  public override void UpdateSettings(string sceneCanvasName)
  {
    if (this.settings != null)
      return;
    this.settings = new SampleSettings();
    this.settings.name = sceneCanvasName;
  }

  public override string DrawAdditionalSettings(string sceneCanvasName)
  {
    return sceneCanvasName;
  }

  public override void BeforeSavingCanvas()
  {
    foreach (BaseNodeEditor node in this.nodes)
    {
      NoiseBase target = node.GetTarget();
      if (target != null)
        target.pos = new Vector2f(node.rect.position);
    }
  }

  public override void AdditionalSaveMethods(
    string sceneCanvasName,
    NodeCanvas.CompleteLoadCallback onComplete)
  {
    GUILayout.BeginHorizontal();
    if (GUILayout.Button(new GUIContent("Load Yaml", "Loads the Canvas from a Yaml Save File")))
      this.Load(sceneCanvasName, onComplete);
    if (GUILayout.Button(new GUIContent("Save to Yaml", "Saves the Canvas to a Yaml file"), GUILayout.ExpandWidth(false)))
    {
      this.BeforeSavingCanvas();
      Tree some_object = this.BuildTreeFromCanvas();
      if (some_object != null)
      {
        some_object.ClearEmptyLists();
        string treeFilePath = NoiseTreeFiles.GetTreeFilePath(sceneCanvasName);
        YamlIO.Save<Tree>(some_object, treeFilePath, (List<Tuple<string, System.Type>>) null);
      }
    }
    GUILayout.EndHorizontal();
    if (this.ntf == null)
      this.ntf = YamlIO.LoadFile<NoiseTreeFiles>(NoiseTreeFiles.GetPath(), (YamlIO.ErrorHandler) null, (List<Tuple<string, System.Type>>) null);
    if (this.ntf != null && GUILayout.Button(new GUIContent("Load Tree", "Loads the Canvas from Trees list")))
    {
      GenericMenu genericMenu = new GenericMenu();
      foreach (string treeFile in this.ntf.tree_files)
        genericMenu.AddItem(new GUIContent(treeFile), false, (PopupMenu.MenuFunctionData) (fileName => this.Load((string) fileName, onComplete)), (object) treeFile);
      genericMenu.Show(this.lastRectPos.position, 40f);
    }
    if (UnityEngine.Event.current.type != EventType.Repaint)
      return;
    Rect lastRect = GUILayoutUtility.GetLastRect();
    this.lastRectPos = new Rect(lastRect.x + 2f, lastRect.yMax + 2f, lastRect.width - 4f, 0.0f);
  }

  private void UpdateTerminator()
  {
    if ((UnityEngine.Object) this.terminator == (UnityEngine.Object) null)
    {
      foreach (NodeEditorFramework.Node node in this.nodes)
      {
        if (node.GetType() == typeof (TerminalNodeEditor))
        {
          if ((UnityEngine.Object) this.terminator == (UnityEngine.Object) null)
            this.terminator = node as TerminalNodeEditor;
          else
            node.Delete();
        }
      }
      if ((UnityEngine.Object) this.terminator == (UnityEngine.Object) null)
        this.terminator = (TerminalNodeEditor) NodeEditorFramework.Node.Create("terminalNodeEditor", Vector2.zero);
    }
    NodeEditorFramework.Node.Create("displayNodeEditor", this.terminator.rect.min + new Vector2(0.0f, -290f)).Inputs[0].ApplyConnection(this.terminator.Outputs[0]);
  }

  private ProcGen.Noise.Link GetLink(NodeEditorFramework.Node node)
  {
    ProcGen.Noise.Link link = new ProcGen.Noise.Link();
    System.Type type = node.GetType();
    if (type == typeof (PrimitiveNodeEditor))
    {
      PrimitiveNodeEditor primitiveNodeEditor = node as PrimitiveNodeEditor;
      Debug.Assert(primitiveNodeEditor.target.name != null && primitiveNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = primitiveNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.Primitive;
    }
    else if (type == typeof (FilterNodeEditor))
    {
      FilterNodeEditor filterNodeEditor = node as FilterNodeEditor;
      Debug.Assert(filterNodeEditor.target.name != null && filterNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = filterNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.Filter;
    }
    else if (type == typeof (TransformerNodeEditor))
    {
      TransformerNodeEditor transformerNodeEditor = node as TransformerNodeEditor;
      Debug.Assert(transformerNodeEditor.target.name != null && transformerNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = transformerNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.Transformer;
    }
    else if (type == typeof (SelectorModuleNodeEditor))
    {
      SelectorModuleNodeEditor moduleNodeEditor = node as SelectorModuleNodeEditor;
      Debug.Assert(moduleNodeEditor.target.name != null && moduleNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = moduleNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.Selector;
    }
    else if (type == typeof (ModifierModuleNodeEditor))
    {
      ModifierModuleNodeEditor moduleNodeEditor = node as ModifierModuleNodeEditor;
      Debug.Assert(moduleNodeEditor.target.name != null && moduleNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = moduleNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.Modifier;
    }
    else if (type == typeof (CombinerModuleNodeEditor))
    {
      CombinerModuleNodeEditor moduleNodeEditor = node as CombinerModuleNodeEditor;
      Debug.Assert(moduleNodeEditor.target.name != null && moduleNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = moduleNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.Combiner;
    }
    else if (type == typeof (FloatPointsNodeEditor))
    {
      FloatPointsNodeEditor pointsNodeEditor = node as FloatPointsNodeEditor;
      Debug.Assert(pointsNodeEditor.target.name != null && pointsNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = pointsNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.FloatPoints;
    }
    else if (type == typeof (ControlPointsNodeEditor))
    {
      ControlPointsNodeEditor pointsNodeEditor = node as ControlPointsNodeEditor;
      Debug.Assert(pointsNodeEditor.target.name != null && pointsNodeEditor.target.name != string.Empty, (object) "Invalid target name");
      link.name = pointsNodeEditor.target.name;
      link.type = ProcGen.Noise.Link.Type.ControlPoints;
    }
    else if (type == typeof (TerminalNodeEditor))
    {
      link.name = "TERMINATOR";
      link.type = ProcGen.Noise.Link.Type.Terminator;
    }
    return link;
  }

  public Tree BuildTreeFromCanvas()
  {
    Tree tree = new Tree();
    tree.settings = this.settings;
    foreach (NodeEditorFramework.Node node in this.nodes)
    {
      System.Type type = node.GetType();
      if (type == typeof (PrimitiveNodeEditor))
      {
        PrimitiveNodeEditor primitiveNodeEditor = node as PrimitiveNodeEditor;
        if (primitiveNodeEditor.target.name == null || primitiveNodeEditor.target.name == string.Empty || tree.primitives.ContainsKey(primitiveNodeEditor.target.name))
          primitiveNodeEditor.target.name = "Primitive" + (object) tree.primitives.Count;
        tree.primitives.Add(primitiveNodeEditor.target.name, primitiveNodeEditor.target);
      }
      else if (type == typeof (FilterNodeEditor))
      {
        FilterNodeEditor filterNodeEditor = node as FilterNodeEditor;
        if (filterNodeEditor.target.name == null || filterNodeEditor.target.name == string.Empty || tree.filters.ContainsKey(filterNodeEditor.target.name))
          filterNodeEditor.target.name = "Filter" + (object) tree.filters.Count;
        tree.filters.Add(filterNodeEditor.target.name, filterNodeEditor.target);
      }
      else if (type == typeof (TransformerNodeEditor))
      {
        TransformerNodeEditor transformerNodeEditor = node as TransformerNodeEditor;
        if (transformerNodeEditor.target.name == null || transformerNodeEditor.target.name == string.Empty || tree.transformers.ContainsKey(transformerNodeEditor.target.name))
          transformerNodeEditor.target.name = "Transformer" + (object) tree.transformers.Count;
        tree.transformers.Add(transformerNodeEditor.target.name, transformerNodeEditor.target);
      }
      else if (type == typeof (SelectorModuleNodeEditor))
      {
        SelectorModuleNodeEditor moduleNodeEditor = node as SelectorModuleNodeEditor;
        if (moduleNodeEditor.target.name == null || moduleNodeEditor.target.name == string.Empty || tree.selectors.ContainsKey(moduleNodeEditor.target.name))
          moduleNodeEditor.target.name = "Selector" + (object) tree.selectors.Count;
        tree.selectors.Add(moduleNodeEditor.target.name, moduleNodeEditor.target);
      }
      else if (type == typeof (ModifierModuleNodeEditor))
      {
        ModifierModuleNodeEditor moduleNodeEditor = node as ModifierModuleNodeEditor;
        if (moduleNodeEditor.target.name == null || moduleNodeEditor.target.name == string.Empty || tree.modifiers.ContainsKey(moduleNodeEditor.target.name))
          moduleNodeEditor.target.name = "Modifier" + (object) tree.modifiers.Count;
        tree.modifiers.Add(moduleNodeEditor.target.name, moduleNodeEditor.target);
      }
      else if (type == typeof (CombinerModuleNodeEditor))
      {
        CombinerModuleNodeEditor moduleNodeEditor = node as CombinerModuleNodeEditor;
        if (moduleNodeEditor.target.name == null || moduleNodeEditor.target.name == string.Empty || tree.combiners.ContainsKey(moduleNodeEditor.target.name))
          moduleNodeEditor.target.name = "Combiner" + (object) tree.combiners.Count;
        tree.combiners.Add(moduleNodeEditor.target.name, moduleNodeEditor.target);
      }
      else if (type == typeof (FloatPointsNodeEditor))
      {
        FloatPointsNodeEditor pointsNodeEditor = node as FloatPointsNodeEditor;
        if (pointsNodeEditor.target.name == null || pointsNodeEditor.target.name == string.Empty || tree.floats.ContainsKey(pointsNodeEditor.target.name))
          pointsNodeEditor.target.name = "Terrace Control" + (object) tree.combiners.Count;
        tree.floats.Add(pointsNodeEditor.target.name, pointsNodeEditor.target);
      }
      else if (type == typeof (ControlPointsNodeEditor))
      {
        ControlPointsNodeEditor pointsNodeEditor = node as ControlPointsNodeEditor;
        if (pointsNodeEditor.target.name == null || pointsNodeEditor.target.name == string.Empty || tree.controlpoints.ContainsKey(pointsNodeEditor.target.name))
          pointsNodeEditor.target.name = "Curve Control" + (object) tree.combiners.Count;
        tree.controlpoints.Add(pointsNodeEditor.target.name, pointsNodeEditor.target);
      }
      else if (type == typeof (TerminalNodeEditor) && (UnityEngine.Object) this.terminator == (UnityEngine.Object) null)
        this.terminator = node as TerminalNodeEditor;
    }
    foreach (NodeEditorFramework.Node node in this.nodes)
    {
      System.Type type = node.GetType();
      if (type == typeof (FilterNodeEditor))
      {
        FilterNodeEditor filterNodeEditor = node as FilterNodeEditor;
        NodeLink nodeLink = new NodeLink();
        nodeLink.target = this.GetLink(node);
        if ((UnityEngine.Object) filterNodeEditor.Inputs[0] != (UnityEngine.Object) null && (UnityEngine.Object) filterNodeEditor.Inputs[0].connection != (UnityEngine.Object) null)
          nodeLink.source0 = this.GetLink(filterNodeEditor.Inputs[0].connection.body);
        tree.links.Add(nodeLink);
      }
      else if (type == typeof (TransformerNodeEditor))
      {
        TransformerNodeEditor transformerNodeEditor = node as TransformerNodeEditor;
        NodeLink nodeLink = new NodeLink();
        nodeLink.target = this.GetLink(node);
        if ((UnityEngine.Object) transformerNodeEditor.Inputs[0] != (UnityEngine.Object) null && (UnityEngine.Object) transformerNodeEditor.Inputs[0].connection != (UnityEngine.Object) null)
          nodeLink.source0 = this.GetLink(transformerNodeEditor.Inputs[0].connection.body);
        if ((UnityEngine.Object) transformerNodeEditor.Inputs[1] != (UnityEngine.Object) null && (UnityEngine.Object) transformerNodeEditor.Inputs[1].connection != (UnityEngine.Object) null)
          nodeLink.source1 = this.GetLink(transformerNodeEditor.Inputs[1].connection.body);
        if ((UnityEngine.Object) transformerNodeEditor.Inputs[2] != (UnityEngine.Object) null && (UnityEngine.Object) transformerNodeEditor.Inputs[2].connection != (UnityEngine.Object) null)
          nodeLink.source2 = this.GetLink(transformerNodeEditor.Inputs[2].connection.body);
        if ((UnityEngine.Object) transformerNodeEditor.Inputs[3] != (UnityEngine.Object) null && (UnityEngine.Object) transformerNodeEditor.Inputs[3].connection != (UnityEngine.Object) null)
          nodeLink.source3 = this.GetLink(transformerNodeEditor.Inputs[3].connection.body);
        tree.links.Add(nodeLink);
      }
      else if (type == typeof (SelectorModuleNodeEditor))
      {
        SelectorModuleNodeEditor moduleNodeEditor = node as SelectorModuleNodeEditor;
        NodeLink nodeLink = new NodeLink();
        nodeLink.target = this.GetLink(node);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[0] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[0].connection != (UnityEngine.Object) null)
          nodeLink.source0 = this.GetLink(moduleNodeEditor.Inputs[0].connection.body);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[1] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[1].connection != (UnityEngine.Object) null)
          nodeLink.source1 = this.GetLink(moduleNodeEditor.Inputs[1].connection.body);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[2] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[2].connection != (UnityEngine.Object) null)
          nodeLink.source2 = this.GetLink(moduleNodeEditor.Inputs[2].connection.body);
        tree.links.Add(nodeLink);
      }
      else if (type == typeof (ModifierModuleNodeEditor))
      {
        ModifierModuleNodeEditor moduleNodeEditor = node as ModifierModuleNodeEditor;
        NodeLink nodeLink = new NodeLink();
        nodeLink.target = this.GetLink(node);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[0] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[0].connection != (UnityEngine.Object) null)
          nodeLink.source0 = this.GetLink(moduleNodeEditor.Inputs[0].connection.body);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[1] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[1].connection != (UnityEngine.Object) null)
          nodeLink.source1 = this.GetLink(moduleNodeEditor.Inputs[1].connection.body);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[2] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[2].connection != (UnityEngine.Object) null)
          nodeLink.source2 = this.GetLink(moduleNodeEditor.Inputs[2].connection.body);
        tree.links.Add(nodeLink);
      }
      else if (type == typeof (CombinerModuleNodeEditor))
      {
        CombinerModuleNodeEditor moduleNodeEditor = node as CombinerModuleNodeEditor;
        NodeLink nodeLink = new NodeLink();
        nodeLink.target = this.GetLink(node);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[0] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[0].connection != (UnityEngine.Object) null)
          nodeLink.source0 = this.GetLink(moduleNodeEditor.Inputs[0].connection.body);
        if ((UnityEngine.Object) moduleNodeEditor.Inputs[1] != (UnityEngine.Object) null && (UnityEngine.Object) moduleNodeEditor.Inputs[1].connection != (UnityEngine.Object) null)
          nodeLink.source1 = this.GetLink(moduleNodeEditor.Inputs[1].connection.body);
        tree.links.Add(nodeLink);
      }
      else if (type == typeof (TerminalNodeEditor))
      {
        TerminalNodeEditor terminalNodeEditor = node as TerminalNodeEditor;
        NodeLink nodeLink = new NodeLink();
        nodeLink.target = this.GetLink(node);
        if ((UnityEngine.Object) terminalNodeEditor.Inputs[0] != (UnityEngine.Object) null && (UnityEngine.Object) terminalNodeEditor.Inputs[0].connection != (UnityEngine.Object) null)
          nodeLink.source0 = this.GetLink(terminalNodeEditor.Inputs[0].connection.body);
        tree.links.Add(nodeLink);
      }
    }
    return tree;
  }

  private NodeCanvas Load(string name, NodeCanvas.CompleteLoadCallback onComplete)
  {
    NodeCanvas canvas = (NodeCanvas) null;
    Tree tree = YamlIO.LoadFile<Tree>(NoiseTreeFiles.GetTreeFilePath(name), (YamlIO.ErrorHandler) null, (List<Tuple<string, System.Type>>) null);
    if (tree != null)
    {
      if (tree.settings.name == null || tree.settings.name == string.Empty)
        tree.settings.name = name;
      canvas = (NodeCanvas) NoiseNodeCanvas.PopulateNoiseNodeEditor(tree);
    }
    onComplete(name, canvas);
    return canvas;
  }

  private NodeEditorFramework.Node GetNodeFromLink(ProcGen.Noise.Link link)
  {
    if (link == null)
      return (NodeEditorFramework.Node) null;
    switch (link.type)
    {
      case ProcGen.Noise.Link.Type.Primitive:
        if (this.primitiveLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.primitiveLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in primitives"));
        break;
      case ProcGen.Noise.Link.Type.Filter:
        if (this.filterLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.filterLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in filters"));
        break;
      case ProcGen.Noise.Link.Type.Transformer:
        if (this.transformerLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.transformerLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in transformers"));
        break;
      case ProcGen.Noise.Link.Type.Selector:
        if (this.selectorLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.selectorLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in selectors"));
        break;
      case ProcGen.Noise.Link.Type.Modifier:
        if (this.modifierLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.modifierLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in modifiers"));
        break;
      case ProcGen.Noise.Link.Type.Combiner:
        if (this.combinerLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.combinerLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in combiners"));
        break;
      case ProcGen.Noise.Link.Type.FloatPoints:
        if (this.floatlistLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.floatlistLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in float points"));
        break;
      case ProcGen.Noise.Link.Type.ControlPoints:
        if (this.ctrlpointsLookup.ContainsKey(link.name))
          return (NodeEditorFramework.Node) this.ctrlpointsLookup[link.name];
        Debug.LogError((object) ("Couldnt find [" + link.name + "] in control points"));
        break;
      case ProcGen.Noise.Link.Type.Terminator:
        if ((UnityEngine.Object) this.terminator == (UnityEngine.Object) null)
        {
          this.terminator = (TerminalNodeEditor) NodeEditorFramework.Node.Create("terminalNodeEditor", Vector2.zero);
          this.terminator.name = link.name;
        }
        return (NodeEditorFramework.Node) this.terminator;
    }
    Debug.LogError((object) ("Couldnt find link [" + link.name + "] [" + link.type.ToString() + "]"));
    return (NodeEditorFramework.Node) null;
  }

  private static NoiseNodeCanvas PopulateNoiseNodeEditor(Tree tree)
  {
    NoiseNodeCanvas instance = NoiseNodeCanvas.CreateInstance();
    NodeEditor.curNodeCanvas = (NodeCanvas) instance;
    instance.Populate(tree);
    return instance;
  }

  private void Populate(Tree tree)
  {
    this.settings = tree.settings;
    this.primitiveLookup.Clear();
    foreach (KeyValuePair<string, Primitive> primitive in tree.primitives)
    {
      PrimitiveNodeEditor primitiveNodeEditor = (PrimitiveNodeEditor) NodeEditorFramework.Node.Create("primitiveNodeEditor", (Vector2) primitive.Value.pos);
      primitiveNodeEditor.name = primitive.Key;
      primitiveNodeEditor.target = primitive.Value;
      this.primitiveLookup.Add(primitive.Key, primitiveNodeEditor);
    }
    this.filterLookup.Clear();
    foreach (KeyValuePair<string, ProcGen.Noise.Filter> filter in tree.filters)
    {
      FilterNodeEditor filterNodeEditor = (FilterNodeEditor) NodeEditorFramework.Node.Create("filterNodeEditor", (Vector2) filter.Value.pos);
      filterNodeEditor.name = filter.Key;
      filterNodeEditor.target = filter.Value;
      this.filterLookup.Add(filter.Key, filterNodeEditor);
    }
    this.modifierLookup.Clear();
    foreach (KeyValuePair<string, ProcGen.Noise.Modifier> modifier in tree.modifiers)
    {
      ModifierModuleNodeEditor moduleNodeEditor = (ModifierModuleNodeEditor) NodeEditorFramework.Node.Create("modifierModuleNodeEditor", (Vector2) modifier.Value.pos);
      moduleNodeEditor.name = modifier.Key;
      moduleNodeEditor.target = modifier.Value;
      this.modifierLookup.Add(modifier.Key, moduleNodeEditor);
    }
    this.selectorLookup.Clear();
    foreach (KeyValuePair<string, Selector> selector in tree.selectors)
    {
      SelectorModuleNodeEditor moduleNodeEditor = (SelectorModuleNodeEditor) NodeEditorFramework.Node.Create("selectorModuleNodeEditor", (Vector2) selector.Value.pos);
      moduleNodeEditor.name = selector.Key;
      moduleNodeEditor.target = selector.Value;
      this.selectorLookup.Add(selector.Key, moduleNodeEditor);
    }
    this.transformerLookup.Clear();
    foreach (KeyValuePair<string, Transformer> transformer in tree.transformers)
    {
      TransformerNodeEditor transformerNodeEditor = (TransformerNodeEditor) NodeEditorFramework.Node.Create("transformerNodeEditor", (Vector2) transformer.Value.pos);
      transformerNodeEditor.name = transformer.Key;
      transformerNodeEditor.target = transformer.Value;
      this.transformerLookup.Add(transformer.Key, transformerNodeEditor);
    }
    this.combinerLookup.Clear();
    foreach (KeyValuePair<string, Combiner> combiner in tree.combiners)
    {
      CombinerModuleNodeEditor moduleNodeEditor = (CombinerModuleNodeEditor) NodeEditorFramework.Node.Create("combinerModuleNodeEditor", (Vector2) combiner.Value.pos);
      moduleNodeEditor.name = combiner.Key;
      moduleNodeEditor.target = combiner.Value;
      this.combinerLookup.Add(combiner.Key, moduleNodeEditor);
    }
    this.floatlistLookup.Clear();
    foreach (KeyValuePair<string, FloatList> keyValuePair in tree.floats)
    {
      FloatPointsNodeEditor pointsNodeEditor = (FloatPointsNodeEditor) NodeEditorFramework.Node.Create("floatPointsNodeEditor", (Vector2) keyValuePair.Value.pos);
      pointsNodeEditor.name = keyValuePair.Key;
      pointsNodeEditor.target = keyValuePair.Value;
      this.floatlistLookup.Add(keyValuePair.Key, pointsNodeEditor);
    }
    this.ctrlpointsLookup.Clear();
    foreach (KeyValuePair<string, ControlPointList> controlpoint in tree.controlpoints)
    {
      ControlPointsNodeEditor pointsNodeEditor = (ControlPointsNodeEditor) NodeEditorFramework.Node.Create("controlPointsNodeEditor", (Vector2) controlpoint.Value.pos);
      pointsNodeEditor.name = controlpoint.Key;
      pointsNodeEditor.target = controlpoint.Value;
      this.ctrlpointsLookup.Add(controlpoint.Key, pointsNodeEditor);
    }
    for (int index = 0; index < tree.links.Count; ++index)
    {
      NodeLink link = tree.links[index];
      NodeEditorFramework.Node nodeFromLink = this.GetNodeFromLink(link.target);
      NodeEditorFramework.Node node1 = (NodeEditorFramework.Node) null;
      NodeEditorFramework.Node node2 = (NodeEditorFramework.Node) null;
      NodeEditorFramework.Node node3 = (NodeEditorFramework.Node) null;
      NodeEditorFramework.Node node4 = (NodeEditorFramework.Node) null;
      switch (link.target.type)
      {
        case ProcGen.Noise.Link.Type.Filter:
        case ProcGen.Noise.Link.Type.Terminator:
          node1 = this.GetNodeFromLink(link.source0);
          break;
        case ProcGen.Noise.Link.Type.Transformer:
          node1 = this.GetNodeFromLink(link.source0);
          node2 = this.GetNodeFromLink(link.source1);
          node3 = this.GetNodeFromLink(link.source2);
          node4 = this.GetNodeFromLink(link.source3);
          break;
        case ProcGen.Noise.Link.Type.Selector:
        case ProcGen.Noise.Link.Type.Modifier:
          node1 = this.GetNodeFromLink(link.source0);
          node2 = this.GetNodeFromLink(link.source1);
          node3 = this.GetNodeFromLink(link.source2);
          break;
        case ProcGen.Noise.Link.Type.Combiner:
          node1 = this.GetNodeFromLink(link.source0);
          node2 = this.GetNodeFromLink(link.source1);
          break;
      }
      if ((UnityEngine.Object) node1 != (UnityEngine.Object) null)
      {
        if (nodeFromLink.Inputs.Count == 0)
          Debug.LogError((object) ("Target [" + nodeFromLink.name + "][" + (object) link.target.type + "] doesnt have any inputs"));
        if (node1.Outputs.Count == 0)
          Debug.LogError((object) ("Source [" + node1.name + "][" + (object) link.source0.type + "] doesnt have any outputs"));
        nodeFromLink.Inputs[0].ApplyConnection(node1.Outputs[0]);
      }
      if ((UnityEngine.Object) node2 != (UnityEngine.Object) null)
        nodeFromLink.Inputs[1].ApplyConnection(node2.Outputs[0]);
      if ((UnityEngine.Object) node3 != (UnityEngine.Object) null)
        nodeFromLink.Inputs[2].ApplyConnection(node3.Outputs[0]);
      if ((UnityEngine.Object) node4 != (UnityEngine.Object) null)
        nodeFromLink.Inputs[3].ApplyConnection(node4.Outputs[0]);
    }
    this.UpdateTerminator();
  }
}
