// Decompiled with JetBrains decompiler
// Type: ToolMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ToolMenu : KScreen
{
  [SerializeField]
  private List<Sprite> icons = new List<Sprite>();
  private List<List<ToolMenu.ToolCollection>> rows = new List<List<ToolMenu.ToolCollection>>();
  public List<ToolMenu.ToolCollection> basicTools = new List<ToolMenu.ToolCollection>();
  public List<ToolMenu.ToolCollection> sandboxTools = new List<ToolMenu.ToolCollection>();
  private HashSet<Action> boundRootActions = new HashSet<Action>();
  private HashSet<Action> boundSubgroupActions = new HashSet<Action>();
  private int smallCollectionMax = 5;
  private HashSet<ToolMenu.CellColorData> colors = new HashSet<ToolMenu.CellColorData>();
  public static ToolMenu Instance;
  public GameObject Prefab_collectionContainer;
  public GameObject Prefab_collectionContainerWindow;
  public PriorityScreen Prefab_priorityScreen;
  public GameObject toolIconPrefab;
  public GameObject toolIconLargePrefab;
  public GameObject sandboxToolIconPrefab;
  public GameObject collectionIconPrefab;
  public GameObject prefabToolRow;
  public GameObject largeToolSet;
  public GameObject smallToolSet;
  public GameObject smallToolBottomRow;
  public GameObject smallToolTopRow;
  public GameObject sandboxToolSet;
  private PriorityScreen priorityScreen;
  public ToolParameterMenu toolParameterMenu;
  public GameObject sandboxToolParameterMenu;
  private GameObject toolEffectDisplayPlane;
  private Texture2D toolEffectDisplayPlaneTexture;
  public Material toolEffectDisplayMaterial;
  private byte[] toolEffectDisplayBytes;
  public ToolMenu.ToolCollection currentlySelectedCollection;
  public ToolMenu.ToolInfo currentlySelectedTool;
  public InterfaceTool activeTool;
  private Coroutine activeOpenAnimationRoutine;
  private Coroutine activeCloseAnimationRoutine;
  [SerializeField]
  public TextStyleSetting ToggleToolTipTextStyleSetting;
  [SerializeField]
  public TextStyleSetting CategoryLabelTextStyle_LeftAlign;

  public static void DestroyInstance()
  {
    ToolMenu.Instance = (ToolMenu) null;
  }

  public PriorityScreen PriorityScreen
  {
    get
    {
      return this.priorityScreen;
    }
  }

  public override float GetSortKey()
  {
    return 5f;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ToolMenu.Instance = this;
    Game.Instance.Subscribe(1798162660, new System.Action<object>(this.OnOverlayChanged));
    this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.Prefab_priorityScreen.gameObject, this.gameObject, false);
    this.priorityScreen.InstantiateButtons(new System.Action<PrioritySetting>(this.OnPriorityClicked), false);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Game.Instance.Unsubscribe(1798162660, new System.Action<object>(this.OnOverlayChanged));
  }

  private void OnOverlayChanged(object overlay_data)
  {
    HashedString hashedString = (HashedString) overlay_data;
    if (!((UnityEngine.Object) PlayerController.Instance.ActiveTool != (UnityEngine.Object) null) || !(PlayerController.Instance.ActiveTool.ViewMode != OverlayModes.None.ID) || !(PlayerController.Instance.ActiveTool.ViewMode != hashedString))
      return;
    this.ChooseCollection((ToolMenu.ToolCollection) null, true);
    this.ChooseTool((ToolMenu.ToolInfo) null);
  }

  protected override void OnSpawn()
  {
    this.activateOnSpawn = true;
    base.OnSpawn();
    this.CreateSandBoxTools();
    this.CreateBasicTools();
    this.rows.Add(this.sandboxTools);
    this.rows.Add(this.basicTools);
    this.rows.ForEach((System.Action<List<ToolMenu.ToolCollection>>) (row => this.InstantiateCollectionsUI((IList<ToolMenu.ToolCollection>) row)));
    this.rows.ForEach((System.Action<List<ToolMenu.ToolCollection>>) (row => this.BuildRowToggles((IList<ToolMenu.ToolCollection>) row)));
    this.rows.ForEach((System.Action<List<ToolMenu.ToolCollection>>) (row => this.BuildToolToggles((IList<ToolMenu.ToolCollection>) row)));
    this.ChooseCollection((ToolMenu.ToolCollection) null, true);
    this.priorityScreen.gameObject.SetActive(false);
    this.ToggleSandboxUI((object) null);
    Game.Instance.Subscribe(-1948169901, new System.Action<object>(this.ToggleSandboxUI));
    this.ResetToolDisplayPlane();
  }

  private void ResetToolDisplayPlane()
  {
    this.toolEffectDisplayPlane = this.CreateToolDisplayPlane("Overlay", World.Instance.transform);
    this.toolEffectDisplayPlaneTexture = this.CreatePlaneTexture(out this.toolEffectDisplayBytes, Grid.WidthInCells, Grid.HeightInCells);
    this.toolEffectDisplayPlane.GetComponent<Renderer>().sharedMaterial = this.toolEffectDisplayMaterial;
    this.toolEffectDisplayPlane.GetComponent<Renderer>().sharedMaterial.mainTexture = (Texture) this.toolEffectDisplayPlaneTexture;
    this.toolEffectDisplayPlane.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -6f));
    this.RefreshToolDisplayPlaneColor();
  }

  private GameObject CreateToolDisplayPlane(string layer, Transform parent)
  {
    GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
    primitive.name = "toolEffectDisplayPlane";
    primitive.SetLayerRecursively(LayerMask.NameToLayer(layer));
    UnityEngine.Object.Destroy((UnityEngine.Object) primitive.GetComponent<Collider>());
    if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
      primitive.transform.SetParent(parent);
    primitive.transform.SetPosition(Vector3.zero);
    primitive.transform.localScale = new Vector3(Grid.WidthInMeters / -10f, 1f, Grid.HeightInMeters / -10f);
    primitive.transform.eulerAngles = new Vector3(270f, 0.0f, 0.0f);
    primitive.GetComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
    return primitive;
  }

  private Texture2D CreatePlaneTexture(out byte[] textureBytes, int width, int height)
  {
    textureBytes = new byte[width * height * 4];
    Texture2D texture2D = new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None);
    texture2D.name = "toolEffectDisplayPlane";
    texture2D.wrapMode = TextureWrapMode.Clamp;
    texture2D.filterMode = FilterMode.Point;
    return texture2D;
  }

  private void Update()
  {
    this.RefreshToolDisplayPlaneColor();
  }

  private void RefreshToolDisplayPlaneColor()
  {
    if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) null || (UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) SelectTool.Instance)
    {
      this.toolEffectDisplayPlane.SetActive(false);
    }
    else
    {
      PlayerController.Instance.ActiveTool.GetOverlayColorData(out this.colors);
      Array.Clear((Array) this.toolEffectDisplayBytes, 0, this.toolEffectDisplayBytes.Length);
      if (this.colors != null)
      {
        foreach (ToolMenu.CellColorData color in this.colors)
        {
          if (Grid.IsValidCell(color.cell))
          {
            int index = color.cell * 4;
            if (index >= 0)
            {
              this.toolEffectDisplayBytes[index] = (byte) ((double) Mathf.Min(color.color.r, 1f) * (double) byte.MaxValue);
              this.toolEffectDisplayBytes[index + 1] = (byte) ((double) Mathf.Min(color.color.g, 1f) * (double) byte.MaxValue);
              this.toolEffectDisplayBytes[index + 2] = (byte) ((double) Mathf.Min(color.color.b, 1f) * (double) byte.MaxValue);
              this.toolEffectDisplayBytes[index + 3] = (byte) ((double) Mathf.Min(color.color.a, 1f) * (double) byte.MaxValue);
            }
          }
        }
      }
      if (!this.toolEffectDisplayPlane.activeSelf)
        this.toolEffectDisplayPlane.SetActive(true);
      this.toolEffectDisplayPlaneTexture.LoadRawTextureData(this.toolEffectDisplayBytes);
      this.toolEffectDisplayPlaneTexture.Apply();
    }
  }

  public void ToggleSandboxUI(object data = null)
  {
    this.ClearSelection();
    PlayerController.Instance.ActivateTool((InterfaceTool) SelectTool.Instance);
    this.sandboxTools[0].toggle.transform.parent.transform.parent.gameObject.SetActive(Game.Instance.SandboxModeActive);
  }

  public static ToolMenu.ToolCollection CreateToolCollection(
    LocString collection_name,
    string icon_name,
    Action hotkey,
    string tool_name,
    LocString tooltip,
    bool largeIcon)
  {
    string text = (string) collection_name;
    string icon_name1 = icon_name;
    bool largeIcon1 = largeIcon;
    ToolMenu.ToolCollection toolCollection = new ToolMenu.ToolCollection(text, icon_name1, string.Empty, false, Action.NumActions, largeIcon1);
    ToolMenu.ToolInfo toolInfo = new ToolMenu.ToolInfo((string) collection_name, icon_name, hotkey, tool_name, toolCollection, (string) tooltip, (System.Action<object>) null, (object) null);
    return toolCollection;
  }

  private void CreateSandBoxTools()
  {
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.BRUSH.NAME, "brush", Action.SandboxBrush, "SandboxBrushTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.SPRINKLE.NAME, "sprinkle", Action.SandboxSprinkle, "SandboxSprinkleTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.SPRINKLE.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.FLOOD.NAME, "flood", Action.SandboxFlood, "SandboxFloodTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.FLOOD.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.SAMPLE.NAME, "sample", Action.SandboxSample, "SandboxSampleTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.SAMPLE.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.HEATGUN.NAME, "brush", Action.SandboxHeatGun, "SandboxHeatTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.HEATGUN.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.SPAWNER.NAME, "spawn", Action.SandboxSpawnEntity, "SandboxSpawnerTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.SPAWNER.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.CLEAR_FLOOR.NAME, "clear_floor", Action.SandboxClearFloor, "SandboxClearFloorTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.CLEAR_FLOOR.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.DESTROY.NAME, "destroy", Action.SandboxDestroy, "SandboxDestroyerTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.DESTROY.TOOLTIP, false));
    this.sandboxTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.SANDBOX.FOW.NAME, "brush", Action.SandboxReveal, "SandboxFOWTool", STRINGS.UI.SANDBOXTOOLS.SETTINGS.FOW.TOOLTIP, false));
  }

  private void CreateBasicTools()
  {
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.DIG.NAME, "icon_action_dig", Action.Dig, "DigTool", STRINGS.UI.TOOLTIPS.DIGBUTTON, true));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.CANCEL.NAME, "icon_action_cancel", Action.BuildingCancel, "CancelTool", STRINGS.UI.TOOLTIPS.CANCELBUTTON, true));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.DECONSTRUCT.NAME, "icon_action_deconstruct", Action.BuildingDeconstruct, "DeconstructTool", STRINGS.UI.TOOLTIPS.DECONSTRUCTBUTTON, true));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.PRIORITIZE.NAME, "icon_action_prioritize", Action.Prioritize, "PrioritizeTool", STRINGS.UI.TOOLTIPS.PRIORITIZEBUTTON, true));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.DISINFECT.NAME, "icon_action_disinfect", Action.Disinfect, "DisinfectTool", STRINGS.UI.TOOLTIPS.DISINFECTBUTTON, false));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.MARKFORSTORAGE.NAME, "icon_action_store", Action.Clear, "ClearTool", STRINGS.UI.TOOLTIPS.CLEARBUTTON, false));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.ATTACK.NAME, "icon_action_attack", Action.Attack, "AttackTool", STRINGS.UI.TOOLTIPS.ATTACKBUTTON, false));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.MOP.NAME, "icon_action_mop", Action.Mop, "MopTool", STRINGS.UI.TOOLTIPS.MOPBUTTON, false));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.CAPTURE.NAME, "icon_action_capture", Action.Capture, "CaptureTool", STRINGS.UI.TOOLTIPS.CAPTUREBUTTON, false));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.HARVEST.NAME, "icon_action_harvest", Action.Harvest, "HarvestTool", STRINGS.UI.TOOLTIPS.HARVESTBUTTON, false));
    this.basicTools.Add(ToolMenu.CreateToolCollection(STRINGS.UI.TOOLS.EMPTY_PIPE.NAME, "icon_action_empty_pipes", Action.EmptyPipe, "EmptyPipeTool", STRINGS.UI.TOOLS.EMPTY_PIPE.TOOLTIP, false));
  }

  private void InstantiateCollectionsUI(IList<ToolMenu.ToolCollection> collections)
  {
    GameObject parent1 = Util.KInstantiateUI(this.prefabToolRow, this.gameObject, true);
    GameObject gameObject1 = Util.KInstantiateUI(this.largeToolSet, parent1, true);
    GameObject parent2 = Util.KInstantiateUI(this.smallToolSet, parent1, true);
    GameObject gameObject2 = Util.KInstantiateUI(this.smallToolBottomRow, parent2, true);
    GameObject gameObject3 = Util.KInstantiateUI(this.smallToolTopRow, parent2, true);
    GameObject gameObject4 = Util.KInstantiateUI(this.sandboxToolSet, parent1, true);
    bool flag = true;
    for (int index1 = 0; index1 < collections.Count; ++index1)
    {
      GameObject parent3;
      if (collections == this.sandboxTools)
        parent3 = gameObject4;
      else if (collections[index1].largeIcon)
      {
        parent3 = gameObject1;
      }
      else
      {
        parent3 = !flag ? gameObject2 : gameObject3;
        flag = !flag;
      }
      ToolMenu.ToolCollection tc = collections[index1];
      tc.toggle = Util.KInstantiateUI(collections[index1].tools.Count <= 1 ? (collections != this.sandboxTools ? (!collections[index1].largeIcon ? this.toolIconPrefab : this.toolIconLargePrefab) : this.sandboxToolIconPrefab) : this.collectionIconPrefab, parent3, true);
      KToggle component = tc.toggle.GetComponent<KToggle>();
      component.soundPlayer.Enabled = false;
      component.onClick += (System.Action) (() =>
      {
        if (this.currentlySelectedCollection == tc && tc.tools.Count >= 1)
          KMonoBehaviour.PlaySound(GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false));
        this.ChooseCollection(tc, true);
      });
      if (tc.tools != null)
      {
        GameObject go;
        if (tc.tools.Count < this.smallCollectionMax)
        {
          go = Util.KInstantiateUI(this.Prefab_collectionContainer, parent3, true);
          go.transform.SetSiblingIndex(go.transform.GetSiblingIndex() - 1);
          go.transform.localScale = Vector3.one;
          go.rectTransform().sizeDelta = new Vector2((float) (tc.tools.Count * 75), 50f);
          tc.MaskContainer = go.GetComponentInChildren<Mask>().gameObject;
          go.SetActive(false);
        }
        else
        {
          go = Util.KInstantiateUI(this.Prefab_collectionContainerWindow, parent3, true);
          go.transform.localScale = Vector3.one;
          go.GetComponentInChildren<LocText>().SetText(tc.text.ToUpper());
          tc.MaskContainer = go.GetComponentInChildren<GridLayoutGroup>().gameObject;
          go.SetActive(false);
        }
        tc.UIMenuDisplay = go;
        for (int index2 = 0; index2 < tc.tools.Count; ++index2)
        {
          ToolMenu.ToolInfo ti = tc.tools[index2];
          GameObject gameObject5 = Util.KInstantiateUI(collections != this.sandboxTools ? (!collections[index1].largeIcon ? this.toolIconPrefab : this.toolIconLargePrefab) : this.sandboxToolIconPrefab, tc.MaskContainer, true);
          gameObject5.name = ti.text;
          ti.toggle = gameObject5.GetComponent<KToggle>();
          if (ti.collection.tools.Count > 1)
          {
            RectTransform rectTransform = ti.toggle.gameObject.GetComponentInChildren<SetTextStyleSetting>().rectTransform();
            if (gameObject5.name.Length > 12)
            {
              rectTransform.GetComponent<SetTextStyleSetting>().SetStyle(this.CategoryLabelTextStyle_LeftAlign);
              rectTransform.anchoredPosition = new Vector2(16f, rectTransform.anchoredPosition.y);
            }
          }
          ti.toggle.onClick += (System.Action) (() => this.ChooseTool(ti));
          tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapse((System.Action<object>) (s =>
          {
            this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
            tc.UIMenuDisplay.SetActive(false);
          }));
        }
      }
    }
    if (gameObject1.transform.childCount == 0)
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject1);
    if (gameObject2.transform.childCount == 0 && gameObject3.transform.childCount == 0)
      UnityEngine.Object.Destroy((UnityEngine.Object) parent2);
    if (gameObject4.transform.childCount != 0)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject4);
  }

  private void ChooseTool(ToolMenu.ToolInfo tool)
  {
    if (this.currentlySelectedTool == tool)
      return;
    if (this.currentlySelectedTool != tool)
    {
      this.currentlySelectedTool = tool;
      if (this.currentlySelectedTool != null && this.currentlySelectedTool.onSelectCallback != null)
        this.currentlySelectedTool.onSelectCallback((object) this.currentlySelectedTool);
    }
    if (this.currentlySelectedTool != null)
    {
      this.currentlySelectedCollection = this.currentlySelectedTool.collection;
      foreach (InterfaceTool tool1 in PlayerController.Instance.tools)
      {
        if (this.currentlySelectedTool.toolName == tool1.name)
        {
          UISounds.PlaySound(UISounds.Sound.ClickObject);
          this.activeTool = tool1;
          PlayerController.Instance.ActivateTool(tool1);
          break;
        }
      }
    }
    else
      PlayerController.Instance.ActivateTool((InterfaceTool) SelectTool.Instance);
    this.rows.ForEach((System.Action<List<ToolMenu.ToolCollection>>) (row => this.RefreshRowDisplay((IList<ToolMenu.ToolCollection>) row)));
  }

  private void RefreshRowDisplay(IList<ToolMenu.ToolCollection> row)
  {
    for (int index1 = 0; index1 < row.Count; ++index1)
    {
      ToolMenu.ToolCollection tc = row[index1];
      if (this.currentlySelectedTool != null && this.currentlySelectedTool.collection == tc)
      {
        if (!tc.UIMenuDisplay.activeSelf || tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing)
        {
          if (tc.tools.Count > 1)
          {
            tc.UIMenuDisplay.SetActive(true);
            if (tc.tools.Count < this.smallCollectionMax)
            {
              float num = Mathf.Clamp((float) (1.0 - (double) tc.tools.Count * 0.150000005960464), 0.5f, 1f);
              tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().speedScale = num;
            }
            tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Expand((System.Action<object>) (s => this.SetToggleState(tc.toggle.GetComponent<KToggle>(), true)));
          }
          else
            this.currentlySelectedTool = tc.tools[0];
        }
      }
      else if (tc.UIMenuDisplay.activeSelf && !tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing && tc.tools.Count > 0)
        tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapse((System.Action<object>) (s =>
        {
          this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
          tc.UIMenuDisplay.SetActive(false);
        }));
      for (int index2 = 0; index2 < tc.tools.Count; ++index2)
      {
        if (tc.tools[index2] == this.currentlySelectedTool)
          this.SetToggleState(tc.tools[index2].toggle, true);
        else
          this.SetToggleState(tc.tools[index2].toggle, false);
      }
    }
  }

  public void TurnLargeCollectionOff()
  {
    if (this.currentlySelectedCollection == null || this.currentlySelectedCollection.tools.Count <= this.smallCollectionMax)
      return;
    this.ChooseCollection((ToolMenu.ToolCollection) null, true);
  }

  private void ChooseCollection(ToolMenu.ToolCollection collection, bool autoSelectTool = true)
  {
    if (collection == this.currentlySelectedCollection)
    {
      if (collection != null && collection.tools.Count > 1)
      {
        this.currentlySelectedCollection = (ToolMenu.ToolCollection) null;
        if (this.currentlySelectedTool != null)
          this.ChooseTool((ToolMenu.ToolInfo) null);
      }
      else if (this.currentlySelectedTool != null && this.currentlySelectedCollection.tools.Contains(this.currentlySelectedTool) && this.currentlySelectedCollection.tools.Count == 1)
      {
        this.currentlySelectedCollection = (ToolMenu.ToolCollection) null;
        this.ChooseTool((ToolMenu.ToolInfo) null);
      }
    }
    else
      this.currentlySelectedCollection = collection;
    this.rows.ForEach((System.Action<List<ToolMenu.ToolCollection>>) (row => this.OpenOrCloseCollectionsInRow((IList<ToolMenu.ToolCollection>) row, true)));
  }

  private void OpenOrCloseCollectionsInRow(IList<ToolMenu.ToolCollection> row, bool autoSelectTool = true)
  {
    for (int index = 0; index < row.Count; ++index)
    {
      ToolMenu.ToolCollection tc = row[index];
      if (this.currentlySelectedCollection == tc)
      {
        if (this.currentlySelectedCollection.tools != null && this.currentlySelectedCollection.tools.Count == 1 || autoSelectTool)
          this.ChooseTool(this.currentlySelectedCollection.tools[0]);
      }
      else if (tc.UIMenuDisplay.activeSelf && !tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing)
        tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapse((System.Action<object>) (s =>
        {
          this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
          tc.UIMenuDisplay.SetActive(false);
        }));
      this.SetToggleState(tc.toggle.GetComponent<KToggle>(), this.currentlySelectedCollection == tc);
    }
  }

  [DebuggerHidden]
  private IEnumerator CloseCollection(ToolMenu.ToolCollection tc)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ToolMenu.\u003CCloseCollection\u003Ec__Iterator0()
    {
      tc = tc,
      \u0024this = this
    };
  }

  private void SetToggleState(KToggle toggle, bool state)
  {
    if (state)
    {
      toggle.Select();
      toggle.isOn = true;
    }
    else
    {
      toggle.Deselect();
      toggle.isOn = false;
    }
  }

  public void ClearSelection()
  {
    if (this.currentlySelectedCollection != null)
      this.ChooseCollection((ToolMenu.ToolCollection) null, true);
    if (this.currentlySelectedTool == null)
      return;
    this.ChooseTool((ToolMenu.ToolInfo) null);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      if (e.IsAction(Action.ToggleSandboxTools))
      {
        if (Application.isEditor)
        {
          DebugUtil.LogArgs((object) "Force-enabling sandbox mode because we're in editor.");
          SaveGame.Instance.sandboxEnabled = true;
        }
        if (SaveGame.Instance.sandboxEnabled)
          Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
      }
      foreach (List<ToolMenu.ToolCollection> row in this.rows)
      {
        if (row != this.sandboxTools || Game.Instance.SandboxModeActive)
        {
          for (int index1 = 0; index1 < row.Count; ++index1)
          {
            Action toolHotkey = row[index1].hotkey;
            if (toolHotkey != Action.NumActions && e.IsAction(toolHotkey) && (this.currentlySelectedCollection == null || this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Find((Predicate<ToolMenu.ToolInfo>) (t => GameInputMapping.CompareActionKeyCodes(t.hotkey, toolHotkey))) == null))
            {
              if (this.currentlySelectedCollection != row[index1])
              {
                this.ChooseCollection(row[index1], false);
                this.ChooseTool(row[index1].tools[0]);
                break;
              }
              if (this.currentlySelectedCollection.tools.Count > 1)
              {
                e.Consumed = true;
                this.ChooseCollection((ToolMenu.ToolCollection) null, true);
                this.ChooseTool((ToolMenu.ToolInfo) null);
                string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
                if (sound != null)
                {
                  KMonoBehaviour.PlaySound(sound);
                  break;
                }
                break;
              }
              break;
            }
            for (int index2 = 0; index2 < row[index1].tools.Count; ++index2)
            {
              if (this.currentlySelectedCollection == null && row[index1].tools.Count == 1 || this.currentlySelectedCollection == row[index1] || this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Count == 1 && row[index1].tools.Count == 1)
              {
                Action hotkey = row[index1].tools[index2].hotkey;
                if (e.IsAction(hotkey) && e.TryConsume(hotkey))
                {
                  if (row[index1].tools.Count == 1 && this.currentlySelectedCollection != row[index1])
                    this.ChooseCollection(row[index1], false);
                  else if (this.currentlySelectedTool != row[index1].tools[index2])
                    this.ChooseTool(row[index1].tools[index2]);
                }
                else if (GameInputMapping.CompareActionKeyCodes(e.GetAction(), hotkey))
                  e.Consumed = true;
              }
            }
          }
        }
      }
      if ((this.currentlySelectedTool != null || this.currentlySelectedCollection != null) && !e.Consumed)
      {
        if (e.TryConsume(Action.Escape))
        {
          string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
          if (sound != null)
            KMonoBehaviour.PlaySound(sound);
          if (this.currentlySelectedCollection != null)
            this.ChooseCollection((ToolMenu.ToolCollection) null, true);
          if (this.currentlySelectedTool != null)
            this.ChooseTool((ToolMenu.ToolInfo) null);
          SelectTool.Instance.Activate();
        }
      }
      else if (!PlayerController.Instance.IsUsingDefaultTool() && !e.Consumed && e.TryConsume(Action.Escape))
        SelectTool.Instance.Activate();
    }
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      if ((this.currentlySelectedTool != null || this.currentlySelectedCollection != null) && !e.Consumed)
      {
        if (PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
        {
          string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
          if (sound != null)
            KMonoBehaviour.PlaySound(sound);
          if (this.currentlySelectedCollection != null)
            this.ChooseCollection((ToolMenu.ToolCollection) null, true);
          if (this.currentlySelectedTool != null)
            this.ChooseTool((ToolMenu.ToolInfo) null);
          SelectTool.Instance.Activate();
        }
      }
      else if (!PlayerController.Instance.IsUsingDefaultTool() && !e.Consumed && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      {
        SelectTool.Instance.Activate();
        string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
        if (sound != null)
          KMonoBehaviour.PlaySound(sound);
      }
    }
    base.OnKeyUp(e);
  }

  protected void BuildRowToggles(IList<ToolMenu.ToolCollection> row)
  {
    for (int index = 0; index < row.Count; ++index)
    {
      ToolMenu.ToolCollection toolCollection = row[index];
      if (!((UnityEngine.Object) toolCollection.toggle == (UnityEngine.Object) null))
      {
        GameObject toggle = toolCollection.toggle;
        foreach (Sprite icon in this.icons)
        {
          if ((UnityEngine.Object) icon != (UnityEngine.Object) null && icon.name == toolCollection.icon)
          {
            toggle.transform.Find("FG").GetComponent<Image>().sprite = icon;
            break;
          }
        }
        Transform transform = toggle.transform.Find("Text");
        if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
        {
          LocText component = transform.GetComponent<LocText>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.text = toolCollection.text;
        }
        ToolTip component1 = toggle.GetComponent<ToolTip>();
        if ((bool) ((UnityEngine.Object) component1))
        {
          if (row[index].tools.Count == 1)
          {
            string newString = GameUtil.ReplaceHotkeyString(row[index].tools[0].tooltip, row[index].tools[0].hotkey);
            component1.AddMultiStringTooltip(newString, (ScriptableObject) this.ToggleToolTipTextStyleSetting);
          }
          else
          {
            string str = row[index].tooltip;
            if (row[index].hotkey != Action.NumActions)
              str = GameUtil.ReplaceHotkeyString(str, row[index].hotkey);
            component1.AddMultiStringTooltip(str, (ScriptableObject) this.ToggleToolTipTextStyleSetting);
          }
        }
      }
    }
  }

  protected void BuildToolToggles(IList<ToolMenu.ToolCollection> row)
  {
    for (int index1 = 0; index1 < row.Count; ++index1)
    {
      ToolMenu.ToolCollection toolCollection = row[index1];
      if (!((UnityEngine.Object) toolCollection.toggle == (UnityEngine.Object) null))
      {
        for (int index2 = 0; index2 < toolCollection.tools.Count; ++index2)
        {
          GameObject gameObject = toolCollection.tools[index2].toggle.gameObject;
          foreach (Sprite icon in this.icons)
          {
            if ((UnityEngine.Object) icon != (UnityEngine.Object) null && icon.name == toolCollection.tools[index2].icon)
            {
              gameObject.transform.Find("FG").GetComponent<Image>().sprite = icon;
              break;
            }
          }
          Transform transform = gameObject.transform.Find("Text");
          if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
          {
            LocText component = transform.GetComponent<LocText>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              component.text = toolCollection.tools[index2].text;
          }
          ToolTip component1 = gameObject.GetComponent<ToolTip>();
          if ((bool) ((UnityEngine.Object) component1))
          {
            string newString = toolCollection.tools.Count <= 1 ? GameUtil.ReplaceHotkeyString(toolCollection.tools[index2].tooltip, toolCollection.tools[index2].hotkey) : GameUtil.ReplaceHotkeyString(toolCollection.tools[index2].tooltip, toolCollection.hotkey, toolCollection.tools[index2].hotkey);
            component1.AddMultiStringTooltip(newString, (ScriptableObject) this.ToggleToolTipTextStyleSetting);
          }
        }
      }
    }
  }

  public bool HasUniqueKeyBindings()
  {
    bool flag = true;
    this.boundRootActions.Clear();
    foreach (List<ToolMenu.ToolCollection> row in this.rows)
    {
      foreach (ToolMenu.ToolCollection toolCollection in row)
      {
        if (this.boundRootActions.Contains(toolCollection.hotkey))
        {
          flag = false;
          break;
        }
        this.boundRootActions.Add(toolCollection.hotkey);
        this.boundSubgroupActions.Clear();
        foreach (ToolMenu.ToolInfo tool in toolCollection.tools)
        {
          if (this.boundSubgroupActions.Contains(tool.hotkey))
          {
            flag = false;
            break;
          }
          this.boundSubgroupActions.Add(tool.hotkey);
        }
      }
    }
    return flag;
  }

  private void OnPriorityClicked(PrioritySetting priority)
  {
    this.priorityScreen.SetScreenPriority(priority, false);
  }

  public class ToolInfo
  {
    public string text;
    public string icon;
    public Action hotkey;
    public string toolName;
    public ToolMenu.ToolCollection collection;
    public string tooltip;
    public KToggle toggle;
    public System.Action<object> onSelectCallback;
    public object toolData;

    public ToolInfo(
      string text,
      string icon_name,
      Action hotkey,
      string ToolName,
      ToolMenu.ToolCollection toolCollection,
      string tooltip = "",
      System.Action<object> onSelectCallback = null,
      object toolData = null)
    {
      this.text = text;
      this.icon = icon_name;
      this.hotkey = hotkey;
      this.toolName = ToolName;
      this.collection = toolCollection;
      toolCollection.tools.Add(this);
      this.tooltip = tooltip;
      this.onSelectCallback = onSelectCallback;
      this.toolData = toolData;
    }
  }

  public class ToolCollection
  {
    public List<ToolMenu.ToolInfo> tools = new List<ToolMenu.ToolInfo>();
    public string text;
    public string icon;
    public string tooltip;
    public bool useInfoMenu;
    public bool largeIcon;
    public GameObject toggle;
    public GameObject UIMenuDisplay;
    public GameObject MaskContainer;
    public Action hotkey;

    public ToolCollection(
      string text,
      string icon_name,
      string tooltip = "",
      bool useInfoMenu = false,
      Action hotkey = Action.NumActions,
      bool largeIcon = false)
    {
      this.text = text;
      this.icon = icon_name;
      this.tooltip = tooltip;
      this.useInfoMenu = useInfoMenu;
      this.hotkey = hotkey;
      this.largeIcon = largeIcon;
    }
  }

  public struct CellColorData
  {
    public int cell;
    public Color color;

    public CellColorData(int cell, Color color)
    {
      this.cell = cell;
      this.color = color;
    }
  }
}
