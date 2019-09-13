// Decompiled with JetBrains decompiler
// Type: BaseUtilityBuildTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BaseUtilityBuildTool : DragTool
{
  protected List<BaseUtilityBuildTool.PathNode> path = new List<BaseUtilityBuildTool.PathNode>();
  private int lastCell = -1;
  private IList<Tag> selectedElements;
  private BuildingDef def;
  protected IUtilityNetworkMgr conduitMgr;
  private Coroutine visUpdater;
  private int buildingCount;
  private BuildingCellVisualizer previousCellConnection;
  private int previousCell;

  protected override void OnPrefabInit()
  {
    this.buildingCount = UnityEngine.Random.Range(1, 14);
    this.canChangeDragAxis = false;
  }

  private void Play(GameObject go, string anim)
  {
    go.GetComponent<KBatchedAnimController>().Play((HashedString) anim, KAnim.PlayMode.Once, 1f, 0.0f);
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, PlayerController.GetCursorPos(KInputManager.GetMousePos()), Grid.SceneLayer.Ore, (string) null, LayerMask.NameToLayer("Place"));
    KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.visibilityType = KAnimControllerBase.VisibilityType.Always;
      component.isMovable = true;
      component.SetDirty();
    }
    this.visualizer.SetActive(true);
    this.Play(this.visualizer, "None_Place");
    this.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
    ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
    this.conduitMgr = this.def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>().GetNetworkManager();
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    this.StopVisUpdater();
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.visualizer);
    base.OnDeactivateTool(new_tool);
  }

  public void Activate(BuildingDef def, IList<Tag> selected_elements)
  {
    this.selectedElements = selected_elements;
    this.def = def;
    this.viewMode = def.ViewMode;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (this.path.Count == 0 || this.path[this.path.Count - 1].cell == cell)
      return;
    this.placeSound = GlobalAssets.GetSound("Place_building_" + this.def.AudioSize, false);
    FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, Grid.CellToPos(cell));
    if (this.path.Count > 1 && cell == this.path[this.path.Count - 2].cell)
    {
      if ((UnityEngine.Object) this.previousCellConnection != (UnityEngine.Object) null)
      {
        this.previousCellConnection.ConnectedEvent(this.previousCell);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("OutletDisconnected", false));
        this.previousCellConnection = (BuildingCellVisualizer) null;
      }
      this.previousCell = cell;
      this.CheckForConnection(cell, this.def.PrefabID, string.Empty, ref this.previousCellConnection, false);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.path[this.path.Count - 1].visualizer);
      TileVisualizer.RefreshCell(this.path[this.path.Count - 1].cell, this.def.TileLayer, this.def.ReplacementLayer);
      this.path.RemoveAt(this.path.Count - 1);
      this.buildingCount = this.buildingCount != 1 ? this.buildingCount - 1 : (this.buildingCount = 14);
      int num = (int) instance.setParameterValue("tileCount", (float) this.buildingCount);
      SoundEvent.EndOneShot(instance);
    }
    else if (!this.path.Exists((Predicate<BaseUtilityBuildTool.PathNode>) (n => n.cell == cell)))
    {
      bool flag = this.CheckValidPathPiece(cell);
      this.path.Add(new BaseUtilityBuildTool.PathNode()
      {
        cell = cell,
        visualizer = (GameObject) null,
        valid = flag
      });
      this.CheckForConnection(cell, this.def.PrefabID, "OutletConnected", ref this.previousCellConnection, true);
      this.buildingCount = this.buildingCount % 14 + 1;
      int num = (int) instance.setParameterValue("tileCount", (float) this.buildingCount);
      SoundEvent.EndOneShot(instance);
    }
    this.visualizer.SetActive(this.path.Count < 2);
    ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(this.path.Count);
  }

  protected override int GetDragLength()
  {
    return this.path.Count;
  }

  private bool CheckValidPathPiece(int cell)
  {
    if (this.def.BuildLocationRule == BuildLocationRule.NotInTiles && ((UnityEngine.Object) Grid.Objects[cell, 9] != (UnityEngine.Object) null || Grid.HasDoor[cell]))
      return false;
    GameObject gameObject1 = Grid.Objects[cell, (int) this.def.ObjectLayer];
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1.GetComponent<KAnimGraphTileVisualizer>() == (UnityEngine.Object) null)
      return false;
    GameObject gameObject2 = Grid.Objects[cell, (int) this.def.TileLayer];
    return !((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null) || !((UnityEngine.Object) gameObject2.GetComponent<KAnimGraphTileVisualizer>() == (UnityEngine.Object) null);
  }

  private bool CheckForConnection(
    int cell,
    string defName,
    string soundName,
    ref BuildingCellVisualizer outBcv,
    bool fireEvents = true)
  {
    outBcv = (BuildingCellVisualizer) null;
    DebugUtil.Assert(defName != null, "defName was null");
    Building building = this.GetBuilding(cell);
    if (!(bool) ((UnityEngine.Object) building))
      return false;
    DebugUtil.Assert((bool) ((UnityEngine.Object) building.gameObject), "targetBuilding.gameObject was null");
    int num1 = -1;
    int num2 = -1;
    int num3 = -1;
    if (defName.Contains("LogicWire"))
    {
      LogicPorts component = building.gameObject.GetComponent<LogicPorts>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (component.inputPorts != null)
        {
          foreach (ILogicUIElement inputPort in component.inputPorts)
          {
            DebugUtil.Assert(inputPort != null, "input port was null");
            if (inputPort.GetLogicUICell() == cell)
            {
              num1 = cell;
              break;
            }
          }
        }
        if (num1 == -1 && component.outputPorts != null)
        {
          foreach (ILogicUIElement outputPort in component.outputPorts)
          {
            DebugUtil.Assert(outputPort != null, "output port was null");
            if (outputPort.GetLogicUICell() == cell)
            {
              num2 = cell;
              break;
            }
          }
        }
      }
    }
    else if (defName.Contains("Wire"))
    {
      num1 = building.GetPowerInputCell();
      num2 = building.GetPowerOutputCell();
    }
    else if (defName.Contains("Liquid"))
    {
      if (building.Def.InputConduitType == ConduitType.Liquid)
        num1 = building.GetUtilityInputCell();
      if (building.Def.OutputConduitType == ConduitType.Liquid)
        num2 = building.GetUtilityOutputCell();
      ElementFilter component = building.GetComponent<ElementFilter>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        DebugUtil.Assert(component.portInfo != null, "elementFilter.portInfo was null A");
        if (component.portInfo.conduitType == ConduitType.Liquid)
          num3 = component.GetFilteredCell();
      }
    }
    else if (defName.Contains("Gas"))
    {
      if (building.Def.InputConduitType == ConduitType.Gas)
        num1 = building.GetUtilityInputCell();
      if (building.Def.OutputConduitType == ConduitType.Gas)
        num2 = building.GetUtilityOutputCell();
      ElementFilter component = building.GetComponent<ElementFilter>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        DebugUtil.Assert(component.portInfo != null, "elementFilter.portInfo was null B");
        if (component.portInfo.conduitType == ConduitType.Gas)
          num3 = component.GetFilteredCell();
      }
    }
    if (cell == num1 || cell == num2 || cell == num3)
    {
      BuildingCellVisualizer component = building.gameObject.GetComponent<BuildingCellVisualizer>();
      outBcv = component;
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (fireEvents)
        {
          component.ConnectedEvent(cell);
          string sound = GlobalAssets.GetSound(soundName, false);
          if (sound != null)
            KMonoBehaviour.PlaySound(sound);
        }
        return true;
      }
    }
    outBcv = (BuildingCellVisualizer) null;
    return false;
  }

  private Building GetBuilding(int cell)
  {
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      return gameObject.GetComponent<Building>();
    return (Building) null;
  }

  protected override DragTool.Mode GetMode()
  {
    return DragTool.Mode.Brush;
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null)
      return;
    this.path.Clear();
    int cell = Grid.PosToCell(cursor_pos);
    if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
    {
      bool flag = this.CheckValidPathPiece(cell);
      this.path.Add(new BaseUtilityBuildTool.PathNode()
      {
        cell = cell,
        visualizer = (GameObject) null,
        valid = flag
      });
      this.CheckForConnection(cell, this.def.PrefabID, "OutletConnected", ref this.previousCellConnection, true);
    }
    this.visUpdater = this.StartCoroutine(this.VisUpdater());
    this.visualizer.GetComponent<KBatchedAnimController>().StopAndClear();
    ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(1);
    this.placeSound = GlobalAssets.GetSound("Place_building_" + this.def.AudioSize, false);
    if (this.placeSound != null)
    {
      this.buildingCount = this.buildingCount % 14 + 1;
      FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, Grid.CellToPos(cell));
      if (this.def.AudioSize == "small")
      {
        int num = (int) instance.setParameterValue("tileCount", (float) this.buildingCount);
      }
      SoundEvent.EndOneShot(instance);
    }
    base.OnLeftClickDown(cursor_pos);
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null)
      return;
    this.BuildPath();
    this.StopVisUpdater();
    this.Play(this.visualizer, "None_Place");
    ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(0);
    base.OnLeftClickUp(cursor_pos);
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    int cell = Grid.PosToCell(cursorPos);
    if (this.lastCell != cell)
      this.lastCell = cell;
    if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
      return;
    Color c = Color.white;
    float strength = 0.0f;
    string fail_reason;
    if (!this.def.IsValidPlaceLocation(this.visualizer, cell, Orientation.Neutral, out fail_reason))
    {
      c = Color.red;
      strength = 1f;
    }
    this.SetColor(this.visualizer, c, strength);
  }

  private void SetColor(GameObject root, Color c, float strength)
  {
    KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.TintColour = (Color32) c;
  }

  protected virtual void ApplyPathToConduitSystem()
  {
    DebugUtil.Assert(false, "I don't think this function ever runs");
  }

  [DebuggerHidden]
  private IEnumerator VisUpdater()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BaseUtilityBuildTool.\u003CVisUpdater\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void BuildPath()
  {
    this.ApplyPathToConduitSystem();
    int connectionCount = 0;
    for (int index = 0; index < this.path.Count; ++index)
    {
      BaseUtilityBuildTool.PathNode pathNode = this.path[index];
      Vector3 posCbc = Grid.CellToPosCBC(pathNode.cell, Grid.SceneLayer.Building);
      UtilityConnections utilityConnections1 = (UtilityConnections) 0;
      GameObject go = Grid.Objects[pathNode.cell, (int) this.def.TileLayer];
      UtilityConnections utilityConnections2;
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      {
        utilityConnections2 = this.conduitMgr.GetConnections(pathNode.cell, false);
        string fail_reason;
        if ((DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild) && (this.def.IsValidBuildLocation(this.visualizer, posCbc, Orientation.Neutral) && this.def.IsValidPlaceLocation(this.visualizer, posCbc, Orientation.Neutral, out fail_reason)))
        {
          BuildingDef def = this.def;
          int cell1 = pathNode.cell;
          Orientation orientation = Orientation.Neutral;
          Storage storage = (Storage) null;
          IList<Tag> selectedElements = this.selectedElements;
          float num1 = 293.15f;
          float time = GameClock.Instance.GetTime();
          int cell2 = cell1;
          int num2 = (int) orientation;
          Storage resource_storage = storage;
          IList<Tag> selected_elements = selectedElements;
          double num3 = (double) num1;
          double num4 = (double) time;
          go = def.Build(cell2, (Orientation) num2, resource_storage, selected_elements, (float) num3, true, (float) num4);
        }
        else
        {
          go = this.def.TryPlace((GameObject) null, posCbc, Orientation.Neutral, this.selectedElements, 0);
          if ((UnityEngine.Object) go != (UnityEngine.Object) null)
          {
            if (!this.def.MaterialsAvailable(this.selectedElements) && !DebugHandler.InstantBuildMode)
              PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, (string) UI.TOOLTIPS.NOMATERIAL, (Transform) null, posCbc, 1.5f, false, false);
            Constructable component1 = go.GetComponent<Constructable>();
            if (component1.IconConnectionAnimation(0.1f * (float) connectionCount, connectionCount, "Wire", "OutletConnected_release") || component1.IconConnectionAnimation(0.1f * (float) connectionCount, connectionCount, "Pipe", "OutletConnected_release"))
              ++connectionCount;
            Prioritizable component2 = go.GetComponent<Prioritizable>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            {
              if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
                component2.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
              if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
                component2.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
            }
          }
        }
      }
      else
      {
        IUtilityItem component = (IUtilityItem) go.GetComponent<KAnimGraphTileVisualizer>();
        if (component != null)
          utilityConnections1 = component.Connections;
        utilityConnections2 = utilityConnections1 | this.conduitMgr.GetConnections(pathNode.cell, false);
        if ((UnityEngine.Object) go.GetComponent<BuildingComplete>() != (UnityEngine.Object) null)
          component.UpdateConnections(utilityConnections2);
      }
      if (this.def.ReplacementLayer != ObjectLayer.NumLayers && !DebugHandler.InstantBuildMode && (!Game.Instance.SandboxModeActive || !SandboxToolParameterMenu.instance.settings.InstantBuild) && this.def.IsValidBuildLocation((GameObject) null, posCbc, Orientation.Neutral))
      {
        GameObject gameObject1 = Grid.Objects[pathNode.cell, (int) this.def.TileLayer];
        GameObject gameObject2 = Grid.Objects[pathNode.cell, (int) this.def.ReplacementLayer];
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
        {
          BuildingComplete component1 = gameObject1.GetComponent<BuildingComplete>();
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.Def != (UnityEngine.Object) this.def)
          {
            Constructable component2 = this.def.BuildingUnderConstruction.GetComponent<Constructable>();
            component2.IsReplacementTile = true;
            go = this.def.Instantiate(posCbc, Orientation.Neutral, this.selectedElements, 0);
            component2.IsReplacementTile = false;
            if (!this.def.MaterialsAvailable(this.selectedElements) && !DebugHandler.InstantBuildMode)
              PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, (string) UI.TOOLTIPS.NOMATERIAL, (Transform) null, posCbc, 1.5f, false, false);
            Grid.Objects[pathNode.cell, (int) this.def.ReplacementLayer] = go;
            IUtilityItem component3 = (IUtilityItem) go.GetComponent<KAnimGraphTileVisualizer>();
            if (component3 != null)
              utilityConnections2 = component3.Connections;
            utilityConnections2 |= this.conduitMgr.GetConnections(pathNode.cell, false);
            if ((UnityEngine.Object) go.GetComponent<BuildingComplete>() != (UnityEngine.Object) null)
              component3.UpdateConnections(utilityConnections2);
            string visualizerString = this.conduitMgr.GetVisualizerString(utilityConnections2);
            string anim = visualizerString;
            if (go.GetComponent<KBatchedAnimController>().HasAnimation((HashedString) (visualizerString + "_place")))
              anim += "_place";
            this.Play(go, anim);
          }
        }
      }
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        IUtilityItem component = (IUtilityItem) go.GetComponent<KAnimGraphTileVisualizer>();
        if (component != null)
          component.Connections = utilityConnections2;
      }
      TileVisualizer.RefreshCell(pathNode.cell, this.def.TileLayer, this.def.ReplacementLayer);
    }
    ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(0);
  }

  private BaseUtilityBuildTool.PathNode CreateVisualizer(
    BaseUtilityBuildTool.PathNode node)
  {
    if ((UnityEngine.Object) node.visualizer == (UnityEngine.Object) null)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.def.BuildingPreview, Grid.CellToPosCBC(node.cell, this.def.SceneLayer), Quaternion.identity);
      gameObject.SetActive(true);
      node.visualizer = gameObject;
    }
    return node;
  }

  private void StopVisUpdater()
  {
    for (int index = 0; index < this.path.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.path[index].visualizer);
    this.path.Clear();
    if (this.visUpdater == null)
      return;
    this.StopCoroutine(this.visUpdater);
    this.visUpdater = (Coroutine) null;
  }

  protected struct PathNode
  {
    public int cell;
    public bool valid;
    public GameObject visualizer;

    public void Play(string anim)
    {
      this.visualizer.GetComponent<KBatchedAnimController>().Play((HashedString) anim, KAnim.PlayMode.Once, 1f, 0.0f);
    }
  }
}
