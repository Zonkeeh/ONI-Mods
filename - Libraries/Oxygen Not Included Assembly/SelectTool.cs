// Decompiled with JetBrains decompiler
// Type: SelectTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SelectTool : InterfaceTool
{
  public KSelectable selected;
  protected int cell_new;
  private int selectedCell;
  protected int defaultLayerMask;
  public static SelectTool Instance;
  private KSelectable delayedNextSelection;
  private bool delayedSkipSound;
  private KSelectable previousSelection;

  public static void DestroyInstance()
  {
    SelectTool.Instance = (SelectTool) null;
  }

  protected override void OnPrefabInit()
  {
    this.defaultLayerMask = 1 | LayerMask.GetMask("World", "Pickupable", "Place", "PlaceWithDepth", "BlockSelection", "Construction", "Selection");
    this.layerMask = this.defaultLayerMask;
    this.selectMarker = Util.KInstantiateUI<SelectMarker>(EntityPrefabs.Instance.SelectMarker, GameScreenManager.Instance.worldSpaceCanvas, false);
    this.selectMarker.gameObject.SetActive(false);
    this.populateHitsList = true;
    SelectTool.Instance = this;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    ToolMenu.Instance.PriorityScreen.ResetPriority();
    this.Select((KSelectable) null, false);
  }

  public void SetLayerMask(int mask)
  {
    this.layerMask = mask;
    this.ClearHover();
    this.LateUpdate();
  }

  public void ClearLayerMask()
  {
    this.layerMask = this.defaultLayerMask;
  }

  public int GetDefaultLayerMask()
  {
    return this.defaultLayerMask;
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.ClearHover();
    this.Select((KSelectable) null, false);
  }

  public void Focus(Vector3 pos, KSelectable selectable, Vector3 offset)
  {
    if ((UnityEngine.Object) selectable != (UnityEngine.Object) null)
      pos = selectable.transform.GetPosition();
    pos.z = -40f;
    pos += offset;
    CameraController.Instance.SetTargetPos(pos, 8f, true);
  }

  public void SelectAndFocus(Vector3 pos, KSelectable selectable, Vector3 offset)
  {
    this.Focus(pos, selectable, offset);
    this.Select(selectable, false);
  }

  public void SelectAndFocus(Vector3 pos, KSelectable selectable)
  {
    this.SelectAndFocus(pos, selectable, Vector3.zero);
  }

  public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
  {
    this.delayedNextSelection = new_selected;
    this.delayedSkipSound = skipSound;
    this.StartCoroutine(this.DoSelectNextFrame());
  }

  [DebuggerHidden]
  private IEnumerator DoSelectNextFrame()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new SelectTool.\u003CDoSelectNextFrame\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public void Select(KSelectable new_selected, bool skipSound = false)
  {
    if ((UnityEngine.Object) new_selected == (UnityEngine.Object) this.previousSelection)
      return;
    this.previousSelection = new_selected;
    if ((UnityEngine.Object) this.selected != (UnityEngine.Object) null)
      this.selected.Unselect();
    GameObject gameObject = (GameObject) null;
    if ((UnityEngine.Object) new_selected != (UnityEngine.Object) null)
    {
      SelectToolHoverTextCard component = this.GetComponent<SelectToolHoverTextCard>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        int selectedSelectableIndex = component.currentSelectedSelectableIndex;
        int displayedSelectables = component.recentNumberOfDisplayedSelectables;
        if (displayedSelectables != 0)
        {
          int num1 = (selectedSelectableIndex + 1) % displayedSelectables;
          if (!skipSound)
          {
            if (displayedSelectables == 1)
            {
              KFMOD.PlayOneShot(GlobalAssets.GetSound("Select_empty", false));
            }
            else
            {
              FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Select_full", false), Vector3.zero);
              int num2 = (int) instance.setParameterValue("selection", (float) num1);
              SoundEvent.EndOneShot(instance);
            }
            this.playedSoundThisFrame = true;
          }
        }
      }
      if ((UnityEngine.Object) new_selected == (UnityEngine.Object) this.hover)
        this.ClearHover();
      new_selected.Select();
      gameObject = new_selected.gameObject;
      this.selectMarker.SetTargetTransform(gameObject.transform);
      this.selectMarker.gameObject.SetActive(!new_selected.DisableSelectMarker);
    }
    else if ((UnityEngine.Object) this.selectMarker != (UnityEngine.Object) null)
      this.selectMarker.gameObject.SetActive(false);
    this.selected = new_selected;
    Game.Instance.Trigger(-1503271301, (object) gameObject);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    KSelectable objectUnderCursor = this.GetObjectUnderCursor<KSelectable>(true, (Func<KSelectable, bool>) (s => s.GetComponent<KSelectable>().IsSelectable), (Component) this.selected);
    this.selectedCell = Grid.PosToCell(cursor_pos);
    this.Select(objectUnderCursor, false);
  }

  public int GetSelectedCell()
  {
    return this.selectedCell;
  }
}
