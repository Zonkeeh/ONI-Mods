// Decompiled with JetBrains decompiler
// Type: BuildTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Rendering;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class BuildTool : DragTool
{
  private int lastCell = -1;
  private int lastDragCell = -1;
  [SerializeField]
  private TextStyleSetting tooltipStyle;
  private Orientation lastDragOrientation;
  private IList<Tag> selectedElements;
  private BuildingDef def;
  private Orientation buildingOrientation;
  private GameObject source;
  private ToolTip tooltip;
  public static BuildTool Instance;
  private bool active;
  private int buildingCount;

  public static void DestroyInstance()
  {
    BuildTool.Instance = (BuildTool) null;
  }

  protected override void OnPrefabInit()
  {
    BuildTool.Instance = this;
    this.tooltip = this.GetComponent<ToolTip>();
    this.buildingCount = Random.Range(1, 14);
    this.canChangeDragAxis = false;
  }

  protected override void OnActivateTool()
  {
    this.lastDragCell = -1;
    if ((Object) this.visualizer != (Object) null)
    {
      this.ClearTilePreview();
      Object.Destroy((Object) this.visualizer);
    }
    this.active = true;
    base.OnActivateTool();
    this.buildingOrientation = Orientation.Neutral;
    this.placementPivot = this.def.placementPivot;
    Vector3 cursorPos = PlayerController.GetCursorPos(KInputManager.GetMousePos());
    this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, cursorPos, Grid.SceneLayer.Ore, (string) null, LayerMask.NameToLayer("Place"));
    KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
    if ((Object) component != (Object) null)
    {
      component.visibilityType = KAnimControllerBase.VisibilityType.Always;
      component.isMovable = true;
      component.Offset = this.def.GetVisualizerOffset();
      component.Offset = component.Offset + this.def.placementPivot;
      component.name = component.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
    }
    this.visualizer.SetActive(true);
    this.UpdateVis(cursorPos);
    this.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
    ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
    if ((Object) component == (Object) null)
      this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
    else
      component.SetLayer(LayerMask.NameToLayer("Place"));
    GridCompositor.Instance.ToggleMajor(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    this.lastDragCell = -1;
    if (!this.active)
      return;
    this.active = false;
    GridCompositor.Instance.ToggleMajor(false);
    this.buildingOrientation = Orientation.Neutral;
    this.HideToolTip();
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
    this.ClearTilePreview();
    Object.Destroy((Object) this.visualizer);
    if ((Object) new_tool == (Object) SelectTool.Instance)
      Game.Instance.Trigger(-1190690038, (object) null);
    base.OnDeactivateTool(new_tool);
  }

  public void Activate(BuildingDef def, IList<Tag> selected_elements, GameObject source = null)
  {
    this.selectedElements = selected_elements;
    this.def = def;
    this.source = source;
    this.viewMode = def.ViewMode;
    ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    this.OnActivateTool();
  }

  public void Deactivate()
  {
    this.selectedElements = (IList<Tag>) null;
    SelectTool.Instance.Activate();
    this.def = (BuildingDef) null;
    this.source = (GameObject) null;
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
  }

  public int GetLastCell
  {
    get
    {
      return this.lastCell;
    }
  }

  public Orientation GetBuildingOrientation
  {
    get
    {
      return this.buildingOrientation;
    }
  }

  private void ClearTilePreview()
  {
    if (!Grid.IsValidBuildingCell(this.lastCell) || !this.def.IsTilePiece)
      return;
    GameObject gameObject1 = Grid.Objects[this.lastCell, (int) this.def.TileLayer];
    if ((Object) this.visualizer == (Object) gameObject1)
      Grid.Objects[this.lastCell, (int) this.def.TileLayer] = (GameObject) null;
    if (!this.def.isKAnimTile)
      return;
    GameObject gameObject2 = (GameObject) null;
    if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
      gameObject2 = Grid.Objects[this.lastCell, (int) this.def.ReplacementLayer];
    if (!((Object) gameObject1 == (Object) null) && !((Object) gameObject1.GetComponent<Constructable>() == (Object) null) || !((Object) gameObject2 == (Object) null) && !((Object) gameObject2 == (Object) this.visualizer))
      return;
    World.Instance.blockTileRenderer.RemoveBlock(this.def, false, SimHashes.Void, this.lastCell);
    World.Instance.blockTileRenderer.RemoveBlock(this.def, true, SimHashes.Void, this.lastCell);
    TileVisualizer.RefreshCell(this.lastCell, this.def.TileLayer, this.def.ReplacementLayer);
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    cursorPos -= this.placementPivot;
    base.OnMouseMove(cursorPos);
    this.UpdateVis(cursorPos);
  }

  private void UpdateVis(Vector3 pos)
  {
    string fail_reason;
    bool flag1 = this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out fail_reason);
    bool isReplacement = this.def.IsValidReplaceLocation(pos, this.buildingOrientation, this.def.ReplacementLayer, this.def.ObjectLayer);
    bool flag2 = flag1 || isReplacement;
    if ((Object) this.visualizer != (Object) null)
    {
      Color c = Color.white;
      float strength = 0.0f;
      if (!flag2)
      {
        c = Color.red;
        strength = 1f;
      }
      this.SetColor(this.visualizer, c, strength);
    }
    int cell = Grid.PosToCell(pos);
    if (!((Object) this.def != (Object) null))
      return;
    Vector3 posCbc = Grid.CellToPosCBC(cell, this.def.SceneLayer);
    this.visualizer.transform.SetPosition(posCbc);
    this.transform.SetPosition(posCbc - Vector3.up * 0.5f);
    if (this.def.IsTilePiece)
    {
      this.ClearTilePreview();
      if (Grid.IsValidBuildingCell(cell))
      {
        GameObject gameObject1 = Grid.Objects[cell, (int) this.def.TileLayer];
        if ((Object) gameObject1 == (Object) null)
          Grid.Objects[cell, (int) this.def.TileLayer] = this.visualizer;
        if (this.def.isKAnimTile)
        {
          GameObject gameObject2 = (GameObject) null;
          if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
            gameObject2 = Grid.Objects[cell, (int) this.def.ReplacementLayer];
          if ((Object) gameObject1 == (Object) null || (Object) gameObject1.GetComponent<Constructable>() == (Object) null && (Object) gameObject2 == (Object) null)
          {
            TileVisualizer.RefreshCell(cell, this.def.TileLayer, this.def.ReplacementLayer);
            if ((Object) this.def.BlockTileAtlas != (Object) null)
            {
              int layer = LayerMask.NameToLayer("Overlay");
              BlockTileRenderer blockTileRenderer = World.Instance.blockTileRenderer;
              blockTileRenderer.SetInvalidPlaceCell(cell, !flag2);
              if (this.lastCell != cell)
                blockTileRenderer.SetInvalidPlaceCell(this.lastCell, false);
              blockTileRenderer.AddBlock(layer, this.def, isReplacement, SimHashes.Void, cell);
            }
          }
        }
      }
    }
    if (this.lastCell == cell)
      return;
    this.lastCell = cell;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.RotateBuilding))
    {
      if (!((Object) this.visualizer != (Object) null))
        return;
      Rotatable component = this.visualizer.GetComponent<Rotatable>();
      if (!((Object) component != (Object) null))
        return;
      KFMOD.PlayOneShot(GlobalAssets.GetSound("HUD_Rotate", false));
      this.buildingOrientation = component.Rotate();
      if (Grid.IsValidBuildingCell(this.lastCell))
        this.UpdateVis(Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building));
      if (!this.Dragging || this.lastDragCell == -1)
        return;
      this.TryBuild(this.lastDragCell);
    }
    else
      base.OnKeyDown(e);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    this.TryBuild(cell);
  }

  private void TryBuild(int cell)
  {
    if ((Object) this.visualizer == (Object) null || cell == this.lastDragCell && this.buildingOrientation == this.lastDragOrientation || Grid.PosToCell(this.visualizer) != cell && ((bool) ((Object) this.def.BuildingComplete.GetComponent<LogicPorts>()) || (bool) ((Object) this.def.BuildingComplete.GetComponent<LogicGateBase>())))
      return;
    this.lastDragCell = cell;
    this.lastDragOrientation = this.buildingOrientation;
    this.ClearTilePreview();
    Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Building);
    GameObject gameObject = (GameObject) null;
    if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild)
    {
      string fail_reason;
      if (this.def.IsValidBuildLocation(this.visualizer, posCbc, this.buildingOrientation) && this.def.IsValidPlaceLocation(this.visualizer, posCbc, this.buildingOrientation, out fail_reason))
      {
        gameObject = this.def.Build(cell, this.buildingOrientation, (Storage) null, this.selectedElements, 293.15f, false, GameClock.Instance.GetTime());
        if ((Object) this.source != (Object) null)
          this.source.DeleteObject();
      }
    }
    else
    {
      gameObject = this.def.TryPlace(this.visualizer, posCbc, this.buildingOrientation, this.selectedElements, 0);
      if ((Object) gameObject == (Object) null && this.def.ReplacementLayer != ObjectLayer.NumLayers)
      {
        GameObject replacementCandidate = this.def.GetReplacementCandidate(cell);
        if ((Object) replacementCandidate != (Object) null && !this.def.IsReplacementLayerOccupied(cell))
        {
          BuildingComplete component = replacementCandidate.GetComponent<BuildingComplete>();
          if ((Object) component != (Object) null && component.Def.Replaceable && this.def.CanReplace(replacementCandidate) && ((Object) component.Def != (Object) this.def || this.selectedElements[0] != replacementCandidate.GetComponent<PrimaryElement>().Element.tag))
          {
            gameObject = this.def.TryReplaceTile(this.visualizer, posCbc, this.buildingOrientation, this.selectedElements, 0);
            Grid.Objects[cell, (int) this.def.ReplacementLayer] = gameObject;
          }
        }
      }
      if ((Object) gameObject != (Object) null)
      {
        Prioritizable component = gameObject.GetComponent<Prioritizable>();
        if ((Object) component != (Object) null)
        {
          if ((Object) BuildMenu.Instance != (Object) null)
            component.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
          if ((Object) PlanScreen.Instance != (Object) null)
            component.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
        }
        if ((Object) this.source != (Object) null)
          this.source.Trigger(2121280625, (object) gameObject);
      }
    }
    if (!((Object) gameObject != (Object) null))
      return;
    if (this.def.MaterialsAvailable(this.selectedElements) || DebugHandler.InstantBuildMode)
    {
      this.placeSound = GlobalAssets.GetSound("Place_Building_" + this.def.AudioSize, false);
      if (this.placeSound != null)
      {
        this.buildingCount = this.buildingCount % 14 + 1;
        EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, posCbc);
        if (this.def.AudioSize == "small")
        {
          int num = (int) instance.setParameterValue("tileCount", (float) this.buildingCount);
        }
        SoundEvent.EndOneShot(instance);
      }
    }
    else
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, (string) UI.TOOLTIPS.NOMATERIAL, (Transform) null, posCbc, 1.5f, false, false);
    Rotatable component1 = gameObject.GetComponent<Rotatable>();
    if (!((Object) component1 != (Object) null))
      return;
    component1.SetOrientation(this.buildingOrientation);
  }

  protected override DragTool.Mode GetMode()
  {
    return DragTool.Mode.Brush;
  }

  private void SetColor(GameObject root, Color c, float strength)
  {
    KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
    if (!((Object) component != (Object) null))
      return;
    component.TintColour = (Color32) c;
  }

  private void ShowToolTip()
  {
    ToolTipScreen.Instance.SetToolTip(this.tooltip);
  }

  private void HideToolTip()
  {
    ToolTipScreen.Instance.ClearToolTip(this.tooltip);
  }

  public void Update()
  {
    if (!this.active)
      return;
    KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
    if (!((Object) component != (Object) null))
      return;
    component.SetLayer(LayerMask.NameToLayer("Place"));
  }

  public override string GetDeactivateSound()
  {
    return "HUD_Click_Deselect";
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    base.OnLeftClickUp(cursor_pos);
  }
}
