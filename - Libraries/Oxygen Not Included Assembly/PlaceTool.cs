// Decompiled with JetBrains decompiler
// Type: PlaceTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PlaceTool : DragTool
{
  [SerializeField]
  private TextStyleSetting tooltipStyle;
  private Tag previewTag;
  private Placeable source;
  private ToolTip tooltip;
  public static PlaceTool Instance;
  private bool active;

  public static void DestroyInstance()
  {
    PlaceTool.Instance = (PlaceTool) null;
  }

  protected override void OnPrefabInit()
  {
    PlaceTool.Instance = this;
    this.tooltip = this.GetComponent<ToolTip>();
  }

  protected override void OnActivateTool()
  {
    this.active = true;
    base.OnActivateTool();
    this.visualizer = GameUtil.KInstantiate(Assets.GetPrefab(this.previewTag), Grid.SceneLayer.Front, (string) null, LayerMask.NameToLayer("Place"));
    KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
    if ((Object) component != (Object) null)
    {
      component.visibilityType = KAnimControllerBase.VisibilityType.Always;
      component.isMovable = true;
    }
    this.visualizer.SetActive(true);
    this.ShowToolTip();
    this.GetComponent<BuildToolHoverTextCard>().currentDef = (BuildingDef) null;
    ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
    if ((Object) component == (Object) null)
      this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
    else
      component.SetLayer(LayerMask.NameToLayer("Place"));
    GridCompositor.Instance.ToggleMajor(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    this.active = false;
    GridCompositor.Instance.ToggleMajor(false);
    this.HideToolTip();
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
    Object.Destroy((Object) this.visualizer);
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetDeactivateSound(), false));
    base.OnDeactivateTool(new_tool);
  }

  public void Activate(Placeable source, Tag previewTag)
  {
    this.source = source;
    this.previewTag = previewTag;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  public void Deactivate()
  {
    SelectTool.Instance.Activate();
    this.source = (Placeable) null;
    this.previewTag = Tag.Invalid;
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if ((Object) this.visualizer == (Object) null)
      return;
    bool flag = false;
    if (this.visualizer.GetComponent<EntityPreview>().Valid)
    {
      if (DebugHandler.InstantBuildMode)
        this.source.Place(cell);
      else
        this.source.QueuePlacement(cell);
      flag = true;
    }
    if (!flag)
      return;
    this.Deactivate();
  }

  protected override DragTool.Mode GetMode()
  {
    return DragTool.Mode.Brush;
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
}
