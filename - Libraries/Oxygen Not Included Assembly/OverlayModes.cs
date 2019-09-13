// Decompiled with JetBrains decompiler
// Type: OverlayModes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Klei.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class OverlayModes
{
  public class GasConduits : OverlayModes.ConduitMode
  {
    public static readonly HashedString ID = (HashedString) "GasConduit";

    public GasConduits()
      : base((ICollection<Tag>) OverlayScreen.GasVentIDs)
    {
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.GasConduits.ID;
    }

    public override string GetSoundName()
    {
      return "GasVent";
    }
  }

  public class LiquidConduits : OverlayModes.ConduitMode
  {
    public static readonly HashedString ID = (HashedString) "LiquidConduit";

    public LiquidConduits()
      : base((ICollection<Tag>) OverlayScreen.LiquidVentIDs)
    {
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.LiquidConduits.ID;
    }

    public override string GetSoundName()
    {
      return "LiquidVent";
    }
  }

  public abstract class ConduitMode : OverlayModes.Mode
  {
    private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();
    private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();
    private List<int> visited = new List<int>();
    private UniformGrid<SaveLoadRoot> partition;
    private ICollection<Tag> targetIDs;
    private int targetLayer;
    private int cameraLayerMask;
    private int selectionMask;

    public ConduitMode(ICollection<Tag> ids)
    {
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.selectionMask = this.cameraLayerMask;
      this.targetIDs = ids;
    }

    public override void Enable()
    {
      this.RegisterSaveLoadListeners();
      this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
      Camera.main.cullingMask |= this.cameraLayerMask;
      SelectTool.Instance.SetLayerMask(this.selectionMask);
      GridCompositor.Instance.ToggleMinor(false);
      base.Enable();
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      if (!this.targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag()))
        return;
      this.partition.Add(item);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      if (this.layerTargets.Contains(item))
        this.layerTargets.Remove(item);
      this.partition.Remove(item);
    }

    public override void Disable()
    {
      OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      SelectTool.Instance.ClearLayerMask();
      this.UnregisterSaveLoadListeners();
      this.partition.Clear();
      this.layerTargets.Clear();
      GridCompositor.Instance.ToggleMinor(false);
      base.Disable();
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets, min, max, (System.Action<SaveLoadRoot>) null);
      IEnumerator enumerator = this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.AddTargetIfVisible<SaveLoadRoot>((SaveLoadRoot) enumerator.Current, min, max, (ICollection<SaveLoadRoot>) this.layerTargets, this.targetLayer, (System.Action<SaveLoadRoot>) null, (Func<KMonoBehaviour, bool>) null);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      GameObject gameObject = (GameObject) null;
      if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.hover != (UnityEngine.Object) null)
        gameObject = SelectTool.Instance.hover.gameObject;
      this.connectedNetworks.Clear();
      float num = 1f;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
        if (component != null)
        {
          int networkCell = component.GetNetworkCell();
          UtilityNetworkManager<FlowUtilityNetwork, Vent> utilityNetworkManager = !(this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.gasConduitSystem : Game.Instance.liquidConduitSystem;
          this.visited.Clear();
          this.FindConnectedNetworks(networkCell, (IUtilityNetworkMgr) utilityNetworkManager, (ICollection<UtilityNetwork>) this.connectedNetworks, this.visited);
          this.visited.Clear();
          num = OverlayModes.ModeUtil.GetHighlightScale();
        }
      }
      Game.ConduitVisInfo conduitVisInfo = !(this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.gasConduitVisInfo : Game.Instance.liquidConduitVisInfo;
      foreach (SaveLoadRoot layerTarget in this.layerTargets)
      {
        if (!((UnityEngine.Object) layerTarget == (UnityEngine.Object) null))
        {
          BuildingDef def = layerTarget.GetComponent<Building>().Def;
          Color32 color32 = (double) def.ThermalConductivity != 1.0 ? ((double) def.ThermalConductivity >= 1.0 ? conduitVisInfo.overlayRadiantTint : conduitVisInfo.overlayInsulatedTint) : conduitVisInfo.overlayTint;
          if (this.connectedNetworks.Count > 0)
          {
            IBridgedNetworkItem component = layerTarget.GetComponent<IBridgedNetworkItem>();
            if (component != null && component.IsConnectedToNetworks((ICollection<UtilityNetwork>) this.connectedNetworks))
            {
              color32.r = (byte) ((double) color32.r * (double) num);
              color32.g = (byte) ((double) color32.g * (double) num);
              color32.b = (byte) ((double) color32.b * (double) num);
            }
          }
          layerTarget.GetComponent<KBatchedAnimController>().TintColour = color32;
        }
      }
    }

    private void FindConnectedNetworks(
      int cell,
      IUtilityNetworkMgr mgr,
      ICollection<UtilityNetwork> networks,
      List<int> visited)
    {
      if (visited.Contains(cell))
        return;
      visited.Add(cell);
      UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
      if (networkForCell == null)
        return;
      networks.Add(networkForCell);
      UtilityConnections connections = mgr.GetConnections(cell, false);
      if ((connections & UtilityConnections.Right) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Left) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Up) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Down) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
      object endpoint = mgr.GetEndpoint(cell);
      if (endpoint == null)
        return;
      (endpoint as FlowUtilityNetwork.NetworkItem)?.GameObject.GetComponent<IBridgedNetworkItem>()?.AddNetworks(networks);
    }
  }

  public class Crop : OverlayModes.BasePlantMode
  {
    public static readonly HashedString ID = (HashedString) nameof (Crop);
    private List<OverlayModes.Crop.UpdateCropInfo> updateCropInfo = new List<OverlayModes.Crop.UpdateCropInfo>();
    private List<GameObject> harvestableNotificationList = new List<GameObject>();
    private OverlayModes.ColorHighlightCondition[] highlightConditions = new OverlayModes.ColorHighlightCondition[3]
    {
      new OverlayModes.ColorHighlightCondition((Func<KMonoBehaviour, Color>) (h => new Color(0.9568627f, 0.2509804f, 0.2784314f, 0.75f)), (Func<KMonoBehaviour, bool>) (h =>
      {
        WiltCondition component = h.GetComponent<WiltCondition>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          return component.IsWilting();
        return false;
      })),
      new OverlayModes.ColorHighlightCondition((Func<KMonoBehaviour, Color>) (h => new Color(0.9843137f, 0.6901961f, 0.2313726f, 0.75f)), (Func<KMonoBehaviour, bool>) (h => !(h as HarvestDesignatable).CanBeHarvested())),
      new OverlayModes.ColorHighlightCondition((Func<KMonoBehaviour, Color>) (h => new Color(0.4196078f, 0.827451f, 0.5176471f, 0.75f)), (Func<KMonoBehaviour, bool>) (h => (h as HarvestDesignatable).CanBeHarvested()))
    };
    private Canvas uiRoot;
    private int freeHarvestableNotificationIdx;
    private GameObject harvestableNotificationPrefab;

    public Crop(Canvas ui_root, GameObject harvestable_notification_prefab)
      : base((ICollection<Tag>) OverlayScreen.HarvestableIDs)
    {
      this.uiRoot = ui_root;
      this.harvestableNotificationPrefab = harvestable_notification_prefab;
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Crop.ID;
    }

    public override string GetSoundName()
    {
      return "Harvest";
    }

    public override void Update()
    {
      this.updateCropInfo.Clear();
      this.freeHarvestableNotificationIdx = 0;
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>((ICollection<HarvestDesignatable>) this.layerTargets, min, max, (System.Action<HarvestDesignatable>) null);
      IEnumerator enumerator = this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.AddTargetIfVisible<HarvestDesignatable>((HarvestDesignatable) enumerator.Current, min, max, (ICollection<HarvestDesignatable>) this.layerTargets, this.targetLayer, (System.Action<HarvestDesignatable>) null, (Func<KMonoBehaviour, bool>) null);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      foreach (HarvestDesignatable layerTarget in this.layerTargets)
      {
        Vector2I xy = Grid.PosToXY(layerTarget.transform.GetPosition());
        if (min <= xy && xy <= max)
          this.AddCropUI(layerTarget);
      }
      foreach (OverlayModes.Crop.UpdateCropInfo updateCropInfo in this.updateCropInfo)
        updateCropInfo.harvestableUI.GetComponent<HarvestableOverlayWidget>().Refresh(updateCropInfo.harvestable);
      for (int harvestableNotificationIdx = this.freeHarvestableNotificationIdx; harvestableNotificationIdx < this.harvestableNotificationList.Count; ++harvestableNotificationIdx)
      {
        if (this.harvestableNotificationList[harvestableNotificationIdx].activeSelf)
          this.harvestableNotificationList[harvestableNotificationIdx].SetActive(false);
      }
      this.UpdateHighlightTypeOverlay<HarvestDesignatable>(min, max, (ICollection<HarvestDesignatable>) this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
      base.Update();
    }

    public override void Disable()
    {
      this.DisableHarvestableUINotifications();
      base.Disable();
    }

    private void DisableHarvestableUINotifications()
    {
      this.freeHarvestableNotificationIdx = 0;
      foreach (GameObject harvestableNotification in this.harvestableNotificationList)
        harvestableNotification.SetActive(false);
      this.updateCropInfo.Clear();
    }

    public GameObject GetFreeCropUI()
    {
      GameObject gameObject;
      if (this.freeHarvestableNotificationIdx < this.harvestableNotificationList.Count)
      {
        gameObject = this.harvestableNotificationList[this.freeHarvestableNotificationIdx];
        if (!gameObject.gameObject.activeSelf)
          gameObject.gameObject.SetActive(true);
        ++this.freeHarvestableNotificationIdx;
      }
      else
      {
        gameObject = Util.KInstantiateUI(this.harvestableNotificationPrefab.gameObject, this.uiRoot.transform.gameObject, false);
        this.harvestableNotificationList.Add(gameObject);
        ++this.freeHarvestableNotificationIdx;
      }
      return gameObject;
    }

    private void AddCropUI(HarvestDesignatable harvestable)
    {
      GameObject freeCropUi = this.GetFreeCropUI();
      OverlayModes.Crop.UpdateCropInfo updateCropInfo = new OverlayModes.Crop.UpdateCropInfo(harvestable, freeCropUi);
      Vector3 pos = Grid.CellToPos(Grid.PosToCell((KMonoBehaviour) harvestable), 0.5f, -1.25f, 0.0f);
      freeCropUi.GetComponent<RectTransform>().SetPosition(Vector3.up + pos);
      this.updateCropInfo.Add(updateCropInfo);
    }

    private struct UpdateCropInfo
    {
      public HarvestDesignatable harvestable;
      public GameObject harvestableUI;

      public UpdateCropInfo(HarvestDesignatable harvestable, GameObject harvestableUI)
      {
        this.harvestable = harvestable;
        this.harvestableUI = harvestableUI;
      }
    }
  }

  public class Harvest : OverlayModes.BasePlantMode
  {
    public static readonly HashedString ID = (HashedString) "HarvestWhenReady";
    private OverlayModes.ColorHighlightCondition[] highlightConditions = new OverlayModes.ColorHighlightCondition[1]
    {
      new OverlayModes.ColorHighlightCondition((Func<KMonoBehaviour, Color>) (harvestable => new Color(0.65f, 0.65f, 0.65f, 0.65f)), (Func<KMonoBehaviour, bool>) (harvestable => true))
    };

    public Harvest()
      : base((ICollection<Tag>) OverlayScreen.HarvestableIDs)
    {
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Harvest.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Harvest);
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>((ICollection<HarvestDesignatable>) this.layerTargets, min, max, (System.Action<HarvestDesignatable>) null);
      IEnumerator enumerator = this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.AddTargetIfVisible<HarvestDesignatable>((HarvestDesignatable) enumerator.Current, min, max, (ICollection<HarvestDesignatable>) this.layerTargets, this.targetLayer, (System.Action<HarvestDesignatable>) null, (Func<KMonoBehaviour, bool>) null);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      this.UpdateHighlightTypeOverlay<HarvestDesignatable>(min, max, (ICollection<HarvestDesignatable>) this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
      base.Update();
    }
  }

  public abstract class BasePlantMode : OverlayModes.Mode
  {
    protected HashSet<HarvestDesignatable> layerTargets = new HashSet<HarvestDesignatable>();
    protected UniformGrid<HarvestDesignatable> partition;
    protected ICollection<Tag> targetIDs;
    protected int targetLayer;
    private int cameraLayerMask;
    private int selectionMask;

    public BasePlantMode(ICollection<Tag> ids)
    {
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.selectionMask = LayerMask.GetMask("MaskedOverlay");
      this.targetIDs = ids;
    }

    public override void Enable()
    {
      this.RegisterSaveLoadListeners();
      this.partition = OverlayModes.Mode.PopulatePartition<HarvestDesignatable>(this.targetIDs);
      Camera.main.cullingMask |= this.cameraLayerMask;
      SelectTool.Instance.SetLayerMask(this.selectionMask);
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      if (!this.targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag()))
        return;
      HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      this.partition.Add(component);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      if (this.layerTargets.Contains(component))
        this.layerTargets.Remove(component);
      this.partition.Remove(component);
    }

    public override void Disable()
    {
      this.UnregisterSaveLoadListeners();
      this.DisableHighlightTypeOverlay<HarvestDesignatable>((ICollection<HarvestDesignatable>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      this.partition.Clear();
      this.layerTargets.Clear();
      SelectTool.Instance.ClearLayerMask();
    }
  }

  public class Decor : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Decor);
    private HashSet<DecorProvider> layerTargets = new HashSet<DecorProvider>();
    private List<DecorProvider> workingTargets = new List<DecorProvider>();
    private HashSet<Tag> targetIDs = new HashSet<Tag>();
    private OverlayModes.ColorHighlightCondition[] highlightConditions = new OverlayModes.ColorHighlightCondition[1]
    {
      new OverlayModes.ColorHighlightCondition((Func<KMonoBehaviour, Color>) (dp =>
      {
        Color black = Color.black;
        Color b = Color.black;
        if ((UnityEngine.Object) dp != (UnityEngine.Object) null)
        {
          int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
          float decorForCell1 = (dp as DecorProvider).GetDecorForCell(cell);
          if ((double) decorForCell1 > 0.0)
            b = new Color(0.0f, 0.8f, 0.0f, 0.8f);
          else if ((double) decorForCell1 < 0.0)
            b = new Color(1f, 0.0f, 0.0f, 0.4f);
          else if ((UnityEngine.Object) dp.GetComponent<MonumentPart>() != (UnityEngine.Object) null && dp.GetComponent<MonumentPart>().IsMonumentCompleted())
          {
            foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(dp.GetComponent<AttachableBuilding>()))
            {
              float decorForCell2 = gameObject.GetComponent<DecorProvider>().GetDecorForCell(cell);
              if ((double) decorForCell2 > 0.0)
              {
                b = new Color(0.0f, 0.8f, 0.0f, 0.8f);
                break;
              }
              if ((double) decorForCell2 < 0.0)
              {
                b = new Color(1f, 0.0f, 0.0f, 0.4f);
                break;
              }
            }
          }
        }
        return Color.Lerp(black, b, 0.85f);
      }), (Func<KMonoBehaviour, bool>) (dp => SelectToolHoverTextCard.highlightedObjects.Contains(dp.gameObject)))
    };
    private UniformGrid<DecorProvider> partition;
    private int targetLayer;
    private int cameraLayerMask;

    public Decor()
    {
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Decor.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Decor);
    }

    public override void Enable()
    {
      this.RegisterSaveLoadListeners();
      this.targetIDs.UnionWith((IEnumerable<Tag>) Assets.GetPrefabTagsWithComponent<DecorProvider>());
      Tag[] tagArray = new Tag[5]
      {
        new Tag("Tile"),
        new Tag("MeshTile"),
        new Tag("InsulationTile"),
        new Tag("GasPermeableMembrane"),
        new Tag("CarpetTile")
      };
      foreach (Tag tag in tagArray)
        this.targetIDs.Remove(tag);
      foreach (Tag gasVentId in OverlayScreen.GasVentIDs)
        this.targetIDs.Remove(gasVentId);
      foreach (Tag liquidVentId in OverlayScreen.LiquidVentIDs)
        this.targetIDs.Remove(liquidVentId);
      this.partition = OverlayModes.Mode.PopulatePartition<DecorProvider>((ICollection<Tag>) this.targetIDs);
      Camera.main.cullingMask |= this.cameraLayerMask;
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<DecorProvider>((ICollection<DecorProvider>) this.layerTargets, min, max, (System.Action<DecorProvider>) null);
      this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y), (ICollection<DecorProvider>) this.workingTargets);
      for (int index = 0; index < this.workingTargets.Count; ++index)
        this.AddTargetIfVisible<DecorProvider>(this.workingTargets[index], min, max, (ICollection<DecorProvider>) this.layerTargets, this.targetLayer, (System.Action<DecorProvider>) null, (Func<KMonoBehaviour, bool>) null);
      this.UpdateHighlightTypeOverlay<DecorProvider>(min, max, (ICollection<DecorProvider>) this.layerTargets, (ICollection<Tag>) this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
      this.workingTargets.Clear();
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      if (!this.targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag()))
        return;
      DecorProvider component = item.GetComponent<DecorProvider>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      this.partition.Add(component);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      DecorProvider component = item.GetComponent<DecorProvider>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      if (this.layerTargets.Contains(component))
        this.layerTargets.Remove(component);
      this.partition.Remove(component);
    }

    public override void Disable()
    {
      this.DisableHighlightTypeOverlay<DecorProvider>((ICollection<DecorProvider>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      this.UnregisterSaveLoadListeners();
      this.partition.Clear();
      this.layerTargets.Clear();
    }
  }

  public class Disease : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Disease);
    private List<GameObject> diseaseUIList = new List<GameObject>();
    private List<OverlayModes.Disease.UpdateDiseaseInfo> updateDiseaseInfo = new List<OverlayModes.Disease.UpdateDiseaseInfo>();
    private HashSet<KMonoBehaviour> layerTargets = new HashSet<KMonoBehaviour>();
    private HashSet<KMonoBehaviour> privateTargets = new HashSet<KMonoBehaviour>();
    private List<KMonoBehaviour> queuedAdds = new List<KMonoBehaviour>();
    private int cameraLayerMask;
    private int freeDiseaseUI;
    private Canvas diseaseUIParent;
    private GameObject diseaseOverlayPrefab;

    public Disease(Canvas diseaseUIParent, GameObject diseaseOverlayPrefab)
    {
      this.diseaseUIParent = diseaseUIParent;
      this.diseaseOverlayPrefab = diseaseOverlayPrefab;
      this.legendFilters = this.CreateDefaultFilters();
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
    }

    private static float CalculateHUE(Color32 colour)
    {
      byte num1 = Math.Max(colour.r, Math.Max(colour.g, colour.b));
      byte num2 = Math.Min(colour.r, Math.Min(colour.g, colour.b));
      float num3 = 0.0f;
      int num4 = (int) num1 - (int) num2;
      if (num4 == 0)
        num3 = 0.0f;
      else if ((int) num1 == (int) colour.r)
        num3 = (float) ((double) ((int) colour.g - (int) colour.b) / (double) num4 % 6.0);
      else if ((int) num1 == (int) colour.g)
        num3 = (float) ((double) ((int) colour.b - (int) colour.r) / (double) num4 + 2.0);
      else if ((int) num1 == (int) colour.b)
        num3 = (float) ((double) ((int) colour.r - (int) colour.g) / (double) num4 + 4.0);
      return num3;
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Disease.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Disease);
    }

    public override void Enable()
    {
      Infrared.Instance.SetMode(Infrared.Mode.Disease);
      CameraController.Instance.ToggleColouredOverlayView(true);
      Camera.main.cullingMask |= this.cameraLayerMask;
      this.RegisterSaveLoadListeners();
      foreach (DiseaseSourceVisualizer sourceVisualizer in Components.DiseaseSourceVisualizers.Items)
      {
        if (!((UnityEngine.Object) sourceVisualizer == (UnityEngine.Object) null))
          sourceVisualizer.Show(this.ViewMode());
      }
    }

    public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
    {
      return new Dictionary<string, ToolParameterMenu.ToggleState>()
      {
        {
          ToolParameterMenu.FILTERLAYERS.ALL,
          ToolParameterMenu.ToggleState.On
        },
        {
          ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.GASCONDUIT,
          ToolParameterMenu.ToggleState.Off
        }
      };
    }

    public override void OnFiltersChanged()
    {
      Game.Instance.showGasConduitDisease = this.InFilter(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, this.legendFilters);
      Game.Instance.showLiquidConduitDisease = this.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, this.legendFilters);
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null)
        return;
      KBatchedAnimController component = item.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      InfraredVisualizerComponents.ClearOverlayColour(component);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
    }

    public override void Disable()
    {
      foreach (DiseaseSourceVisualizer sourceVisualizer in Components.DiseaseSourceVisualizers.Items)
      {
        if (!((UnityEngine.Object) sourceVisualizer == (UnityEngine.Object) null))
          sourceVisualizer.Show(OverlayModes.None.ID);
      }
      this.UnregisterSaveLoadListeners();
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      foreach (KMonoBehaviour layerTarget in this.layerTargets)
      {
        if (!((UnityEngine.Object) layerTarget == (UnityEngine.Object) null))
        {
          float defaultDepth = OverlayModes.Mode.GetDefaultDepth(layerTarget);
          Vector3 position = layerTarget.transform.GetPosition();
          position.z = defaultDepth;
          layerTarget.transform.SetPosition(position);
          KBatchedAnimController component = layerTarget.GetComponent<KBatchedAnimController>();
          component.enabled = false;
          component.enabled = true;
        }
      }
      CameraController.Instance.ToggleColouredOverlayView(false);
      Infrared.Instance.SetMode(Infrared.Mode.Disabled);
      Game.Instance.showGasConduitDisease = false;
      Game.Instance.showLiquidConduitDisease = false;
      this.freeDiseaseUI = 0;
      foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
        updateDiseaseInfo.ui.gameObject.SetActive(false);
      this.updateDiseaseInfo.Clear();
      this.privateTargets.Clear();
      this.layerTargets.Clear();
    }

    public override List<LegendEntry> GetCustomLegendData()
    {
      List<LegendEntry> legendEntryList = new List<LegendEntry>();
      List<OverlayModes.Disease.DiseaseSortInfo> diseaseSortInfoList = new List<OverlayModes.Disease.DiseaseSortInfo>();
      foreach (Klei.AI.Disease resource in Db.Get().Diseases.resources)
        diseaseSortInfoList.Add(new OverlayModes.Disease.DiseaseSortInfo(resource));
      diseaseSortInfoList.Sort((Comparison<OverlayModes.Disease.DiseaseSortInfo>) ((a, b) => a.sortkey.CompareTo(b.sortkey)));
      foreach (OverlayModes.Disease.DiseaseSortInfo diseaseSortInfo in diseaseSortInfoList)
        legendEntryList.Add(new LegendEntry(diseaseSortInfo.disease.Name, diseaseSortInfo.disease.overlayLegendHovertext.ToString(), (Color) diseaseSortInfo.disease.overlayColour));
      return legendEntryList;
    }

    public GameObject GetFreeDiseaseUI()
    {
      GameObject gameObject;
      if (this.freeDiseaseUI < this.diseaseUIList.Count)
      {
        gameObject = this.diseaseUIList[this.freeDiseaseUI];
        gameObject.gameObject.SetActive(true);
        ++this.freeDiseaseUI;
      }
      else
      {
        gameObject = Util.KInstantiateUI(this.diseaseOverlayPrefab, this.diseaseUIParent.transform.gameObject, false);
        this.diseaseUIList.Add(gameObject);
        ++this.freeDiseaseUI;
      }
      return gameObject;
    }

    private void AddDiseaseUI(MinionIdentity target)
    {
      GameObject freeDiseaseUi = this.GetFreeDiseaseUI();
      DiseaseOverlayWidget component1 = freeDiseaseUi.GetComponent<DiseaseOverlayWidget>();
      OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo = new OverlayModes.Disease.UpdateDiseaseInfo(target.GetComponent<Modifiers>().amounts.Get(Db.Get().Amounts.ImmuneLevel), component1);
      KAnimControllerBase component2 = target.GetComponent<KAnimControllerBase>();
      Vector3 position = !((UnityEngine.Object) component2 != (UnityEngine.Object) null) ? target.transform.GetPosition() + Vector3.down : component2.GetWorldPivot();
      freeDiseaseUi.GetComponent<RectTransform>().SetPosition(position);
      this.updateDiseaseInfo.Add(updateDiseaseInfo);
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      using (new KProfiler.Region("UpdateDiseaseCarriers", (UnityEngine.Object) null))
      {
        this.queuedAdds.Clear();
        foreach (MinionIdentity target in Components.LiveMinionIdentities.Items)
        {
          if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
          {
            Vector2I xy = Grid.PosToXY(target.transform.GetPosition());
            if (min <= xy && xy <= max && !this.privateTargets.Contains((KMonoBehaviour) target))
            {
              this.AddDiseaseUI(target);
              this.queuedAdds.Add((KMonoBehaviour) target);
            }
          }
        }
        foreach (KMonoBehaviour queuedAdd in this.queuedAdds)
          this.privateTargets.Add(queuedAdd);
        this.queuedAdds.Clear();
      }
      foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
        updateDiseaseInfo.ui.Refresh(updateDiseaseInfo.valueSrc);
      bool flag = false;
      if (Game.Instance.showLiquidConduitDisease)
      {
        foreach (Tag liquidVentId in OverlayScreen.LiquidVentIDs)
        {
          if (!OverlayScreen.DiseaseIDs.Contains(liquidVentId))
          {
            OverlayScreen.DiseaseIDs.Add(liquidVentId);
            flag = true;
          }
        }
      }
      else
      {
        foreach (Tag liquidVentId in OverlayScreen.LiquidVentIDs)
        {
          if (OverlayScreen.DiseaseIDs.Contains(liquidVentId))
          {
            OverlayScreen.DiseaseIDs.Remove(liquidVentId);
            flag = true;
          }
        }
      }
      if (Game.Instance.showGasConduitDisease)
      {
        foreach (Tag gasVentId in OverlayScreen.GasVentIDs)
        {
          if (!OverlayScreen.DiseaseIDs.Contains(gasVentId))
          {
            OverlayScreen.DiseaseIDs.Add(gasVentId);
            flag = true;
          }
        }
      }
      else
      {
        foreach (Tag gasVentId in OverlayScreen.GasVentIDs)
        {
          if (OverlayScreen.DiseaseIDs.Contains(gasVentId))
          {
            OverlayScreen.DiseaseIDs.Remove(gasVentId);
            flag = true;
          }
        }
      }
      if (!flag)
        return;
      this.SetLayerZ(-50f);
    }

    private void SetLayerZ(float offset_z)
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.ClearOutsideViewObjects<KMonoBehaviour>((ICollection<KMonoBehaviour>) this.layerTargets, min, max, (ICollection<Tag>) OverlayScreen.DiseaseIDs, (System.Action<KMonoBehaviour>) (go =>
      {
        if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
          return;
        float defaultDepth = OverlayModes.Mode.GetDefaultDepth(go);
        Vector3 position = go.transform.GetPosition();
        position.z = defaultDepth;
        go.transform.SetPosition(position);
        KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
        component.enabled = false;
        component.enabled = true;
      }));
      Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
      foreach (Tag diseaseId in OverlayScreen.DiseaseIDs)
      {
        List<SaveLoadRoot> saveLoadRootList;
        if (lists.TryGetValue(diseaseId, out saveLoadRootList))
        {
          foreach (KMonoBehaviour cmp in saveLoadRootList)
          {
            if (!((UnityEngine.Object) cmp == (UnityEngine.Object) null) && !this.layerTargets.Contains(cmp))
            {
              Vector3 position = cmp.transform.GetPosition();
              if (Grid.IsVisible(Grid.PosToCell(position)) && min <= (Vector2) position && (Vector2) position <= max)
              {
                float defaultDepth = OverlayModes.Mode.GetDefaultDepth(cmp);
                position.z = defaultDepth + offset_z;
                cmp.transform.SetPosition(position);
                KBatchedAnimController component = cmp.GetComponent<KBatchedAnimController>();
                component.enabled = false;
                component.enabled = true;
                this.layerTargets.Add(cmp);
              }
            }
          }
        }
      }
    }

    private struct DiseaseSortInfo
    {
      public float sortkey;
      public Klei.AI.Disease disease;

      public DiseaseSortInfo(Klei.AI.Disease d)
      {
        this.disease = d;
        this.sortkey = OverlayModes.Disease.CalculateHUE(d.overlayColour);
      }
    }

    private struct UpdateDiseaseInfo
    {
      public DiseaseOverlayWidget ui;
      public AmountInstance valueSrc;

      public UpdateDiseaseInfo(AmountInstance amount_inst, DiseaseOverlayWidget ui)
      {
        this.ui = ui;
        this.valueSrc = amount_inst;
      }
    }
  }

  public class Logic : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Logic);
    public static HashSet<Tag> HighlightItemIDs = new HashSet<Tag>();
    private HashSet<ILogicUIElement> ioTargets = new HashSet<ILogicUIElement>();
    private HashSet<ILogicUIElement> workingIOTargets = new HashSet<ILogicUIElement>();
    private HashSet<KBatchedAnimController> wireControllers = new HashSet<KBatchedAnimController>();
    private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();
    private List<int> visited = new List<int>();
    private HashSet<OverlayModes.Logic.BridgeInfo> bridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();
    private HashSet<SaveLoadRoot> gameObjTargets = new HashSet<SaveLoadRoot>();
    private Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo> uiNodes = new Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo>();
    private KCompactedVector<OverlayModes.Logic.UIInfo> uiInfo = new KCompactedVector<OverlayModes.Logic.UIInfo>(0);
    private int conduitTargetLayer;
    private int objectTargetLayer;
    private int cameraLayerMask;
    private int selectionMask;
    private UniformGrid<ILogicUIElement> ioPartition;
    private UniformGrid<SaveLoadRoot> gameObjPartition;
    private LogicModeUI uiAsset;

    public Logic(LogicModeUI ui_asset)
    {
      this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.selectionMask = this.cameraLayerMask;
      this.uiAsset = ui_asset;
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Logic.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Logic);
    }

    public override void Enable()
    {
      Camera.main.cullingMask |= this.cameraLayerMask;
      SelectTool.Instance.SetLayerMask(this.selectionMask);
      this.RegisterSaveLoadListeners();
      this.gameObjPartition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>((ICollection<Tag>) OverlayModes.Logic.HighlightItemIDs);
      this.ioPartition = this.CreateLogicUIPartition();
      GridCompositor.Instance.ToggleMinor(true);
      Game.Instance.logicCircuitManager.onElemAdded += new System.Action<ILogicUIElement>(this.OnUIElemAdded);
      Game.Instance.logicCircuitManager.onElemRemoved += new System.Action<ILogicUIElement>(this.OnUIElemRemoved);
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterLogicOn);
    }

    public override void Disable()
    {
      Game.Instance.logicCircuitManager.onElemAdded -= new System.Action<ILogicUIElement>(this.OnUIElemAdded);
      Game.Instance.logicCircuitManager.onElemRemoved -= new System.Action<ILogicUIElement>(this.OnUIElemRemoved);
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().TechFilterLogicOn, STOP_MODE.ALLOWFADEOUT);
      foreach (SaveLoadRoot gameObjTarget in this.gameObjTargets)
      {
        float defaultDepth = OverlayModes.Mode.GetDefaultDepth((KMonoBehaviour) gameObjTarget);
        Vector3 position = gameObjTarget.transform.GetPosition();
        position.z = defaultDepth;
        gameObjTarget.transform.SetPosition(position);
      }
      OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.gameObjTargets);
      OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>((ICollection<KBatchedAnimController>) this.wireControllers);
      foreach (OverlayModes.Logic.BridgeInfo bridgeController in this.bridgeControllers)
      {
        if ((UnityEngine.Object) bridgeController.controller != (UnityEngine.Object) null)
          OverlayModes.Mode.ResetDisplayValues(bridgeController.controller);
      }
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      SelectTool.Instance.ClearLayerMask();
      this.UnregisterSaveLoadListeners();
      foreach (OverlayModes.Logic.UIInfo data in this.uiInfo.GetDataList())
        data.Release();
      this.uiInfo.Clear();
      this.uiNodes.Clear();
      this.ioPartition.Clear();
      this.ioTargets.Clear();
      this.gameObjPartition.Clear();
      this.gameObjTargets.Clear();
      this.wireControllers.Clear();
      this.bridgeControllers.Clear();
      GridCompositor.Instance.ToggleMinor(false);
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
      if (!OverlayModes.Logic.HighlightItemIDs.Contains(saveLoadTag))
        return;
      this.gameObjPartition.Add(item);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      if (this.gameObjTargets.Contains(item))
        this.gameObjTargets.Remove(item);
      this.gameObjPartition.Remove(item);
    }

    private void OnUIElemAdded(ILogicUIElement elem)
    {
      this.ioPartition.Add(elem);
    }

    private void OnUIElemRemoved(ILogicUIElement elem)
    {
      this.ioPartition.Remove(elem);
      if (!this.ioTargets.Contains(elem))
        return;
      this.ioTargets.Remove(elem);
      this.FreeUI(elem);
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      Tag wire_id = TagManager.Create("LogicWire");
      Tag bridge_id = TagManager.Create("LogicWireBridge");
      OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.gameObjTargets, min, max, (System.Action<SaveLoadRoot>) (root =>
      {
        if ((UnityEngine.Object) root == (UnityEngine.Object) null)
          return;
        KPrefabID component = root.GetComponent<KPrefabID>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        Tag prefabTag = component.PrefabTag;
        if (prefabTag == wire_id)
        {
          this.wireControllers.Remove(root.GetComponent<KBatchedAnimController>());
        }
        else
        {
          if (!(prefabTag == bridge_id))
            return;
          KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
          this.bridgeControllers.RemoveWhere((Predicate<OverlayModes.Logic.BridgeInfo>) (x => (UnityEngine.Object) x.controller == (UnityEngine.Object) controller));
        }
      }));
      OverlayModes.Mode.RemoveOffscreenTargets<ILogicUIElement>((ICollection<ILogicUIElement>) this.ioTargets, (ICollection<ILogicUIElement>) this.workingIOTargets, min, max, new System.Action<ILogicUIElement>(this.FreeUI), (Func<ILogicUIElement, bool>) null);
      using (new KProfiler.Region("UpdateLogicOverlay", (UnityEngine.Object) null))
      {
        IEnumerator enumerator1 = this.gameObjPartition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            SaveLoadRoot current = (SaveLoadRoot) enumerator1.Current;
            if ((UnityEngine.Object) current != (UnityEngine.Object) null)
            {
              KPrefabID component1 = current.GetComponent<KPrefabID>();
              if (component1.PrefabTag == wire_id || component1.PrefabTag == bridge_id)
                this.AddTargetIfVisible<SaveLoadRoot>(current, min, max, (ICollection<SaveLoadRoot>) this.gameObjTargets, this.conduitTargetLayer, (System.Action<SaveLoadRoot>) (root =>
                {
                  if ((UnityEngine.Object) root == (UnityEngine.Object) null)
                    return;
                  KPrefabID component1 = root.GetComponent<KPrefabID>();
                  if (!OverlayModes.Logic.HighlightItemIDs.Contains(component1.PrefabTag))
                    return;
                  if (component1.PrefabTag == wire_id)
                  {
                    this.wireControllers.Add(root.GetComponent<KBatchedAnimController>());
                  }
                  else
                  {
                    if (!(component1.PrefabTag == bridge_id))
                      return;
                    KBatchedAnimController component2 = root.GetComponent<KBatchedAnimController>();
                    int linked_cell1;
                    int linked_cell2;
                    root.GetComponent<LogicUtilityNetworkLink>().GetCells(out linked_cell1, out linked_cell2);
                    this.bridgeControllers.Add(new OverlayModes.Logic.BridgeInfo()
                    {
                      cell = linked_cell1,
                      controller = component2
                    });
                  }
                }), (Func<KMonoBehaviour, bool>) null);
              else
                this.AddTargetIfVisible<SaveLoadRoot>(current, min, max, (ICollection<SaveLoadRoot>) this.gameObjTargets, this.objectTargetLayer, (System.Action<SaveLoadRoot>) (root =>
                {
                  Vector3 position = root.transform.GetPosition();
                  position.z += 2f;
                  root.transform.SetPosition(position);
                  KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
                  component.enabled = false;
                  component.enabled = true;
                }), (Func<KMonoBehaviour, bool>) null);
            }
          }
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
        IEnumerator enumerator2 = this.ioPartition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
          {
            ILogicUIElement current = (ILogicUIElement) enumerator2.Current;
            if (current != null)
              this.AddTargetIfVisible<ILogicUIElement>(current, min, max, (ICollection<ILogicUIElement>) this.ioTargets, this.objectTargetLayer, new System.Action<ILogicUIElement>(this.AddUI), (Func<KMonoBehaviour, bool>) (kcmp =>
              {
                if ((UnityEngine.Object) kcmp != (UnityEngine.Object) null)
                  return OverlayModes.Logic.HighlightItemIDs.Contains(kcmp.GetComponent<KPrefabID>().PrefabTag);
                return false;
              }));
          }
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
        this.connectedNetworks.Clear();
        float num = 1f;
        GameObject gameObject = (GameObject) null;
        if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.hover != (UnityEngine.Object) null)
          gameObject = SelectTool.Instance.hover.gameObject;
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
          if (component != null)
          {
            int networkCell = component.GetNetworkCell();
            this.visited.Clear();
            this.FindConnectedNetworks(networkCell, (IUtilityNetworkMgr) Game.Instance.logicCircuitSystem, (ICollection<UtilityNetwork>) this.connectedNetworks, this.visited);
            this.visited.Clear();
            num = OverlayModes.ModeUtil.GetHighlightScale();
          }
        }
        LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
        Color32 colourOn = this.uiAsset.colourOn;
        Color32 colourOff = this.uiAsset.colourOff;
        colourOff.a = colourOn.a = (byte) 0;
        foreach (KBatchedAnimController wireController in this.wireControllers)
        {
          if (!((UnityEngine.Object) wireController == (UnityEngine.Object) null))
          {
            Color32 color32 = colourOff;
            LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(wireController.transform.GetPosition()));
            if (networkForCell != null)
              color32 = networkForCell.OutputValue <= 0 ? colourOff : colourOn;
            if (this.connectedNetworks.Count > 0)
            {
              IBridgedNetworkItem component = wireController.GetComponent<IBridgedNetworkItem>();
              if (component != null && component.IsConnectedToNetworks((ICollection<UtilityNetwork>) this.connectedNetworks))
              {
                color32.r = (byte) ((double) color32.r * (double) num);
                color32.g = (byte) ((double) color32.g * (double) num);
                color32.b = (byte) ((double) color32.b * (double) num);
              }
            }
            wireController.TintColour = color32;
          }
        }
        foreach (OverlayModes.Logic.BridgeInfo bridgeController in this.bridgeControllers)
        {
          if (!((UnityEngine.Object) bridgeController.controller == (UnityEngine.Object) null))
          {
            Color32 color32 = colourOff;
            LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(bridgeController.cell);
            if (networkForCell != null)
              color32 = networkForCell.OutputValue <= 0 ? colourOff : colourOn;
            if (this.connectedNetworks.Count > 0)
            {
              IBridgedNetworkItem component = bridgeController.controller.GetComponent<IBridgedNetworkItem>();
              if (component != null && component.IsConnectedToNetworks((ICollection<UtilityNetwork>) this.connectedNetworks))
              {
                color32.r = (byte) ((double) color32.r * (double) num);
                color32.g = (byte) ((double) color32.g * (double) num);
                color32.b = (byte) ((double) color32.b * (double) num);
              }
            }
            bridgeController.controller.TintColour = color32;
          }
        }
      }
      this.UpdateUI();
    }

    private void UpdateUI()
    {
      Color32 colourOn = this.uiAsset.colourOn;
      Color32 colourOff = this.uiAsset.colourOff;
      Color32 colourDisconnected = this.uiAsset.colourDisconnected;
      colourOff.a = colourOn.a = byte.MaxValue;
      foreach (OverlayModes.Logic.UIInfo data in this.uiInfo.GetDataList())
      {
        LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(data.cell);
        Color32 color32 = colourDisconnected;
        if (networkForCell != null)
          color32 = networkForCell.OutputValue <= 0 ? colourOff : colourOn;
        if (data.image.color != (Color) color32)
          data.image.color = (Color) color32;
      }
    }

    private void AddUI(ILogicUIElement ui_elem)
    {
      if (this.uiNodes.ContainsKey(ui_elem))
        return;
      HandleVector<int>.Handle handle = this.uiInfo.Allocate(new OverlayModes.Logic.UIInfo(ui_elem, this.uiAsset));
      this.uiNodes.Add(ui_elem, new OverlayModes.Logic.EventInfo()
      {
        uiHandle = handle
      });
    }

    private void FreeUI(ILogicUIElement item)
    {
      OverlayModes.Logic.EventInfo eventInfo;
      if (item == null || !this.uiNodes.TryGetValue(item, out eventInfo))
        return;
      this.uiInfo.GetData(eventInfo.uiHandle).Release();
      this.uiInfo.Free(eventInfo.uiHandle);
      this.uiNodes.Remove(item);
    }

    protected UniformGrid<ILogicUIElement> CreateLogicUIPartition()
    {
      UniformGrid<ILogicUIElement> uniformGrid = new UniformGrid<ILogicUIElement>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
      foreach (ILogicUIElement visElement in Game.Instance.logicCircuitManager.GetVisElements())
      {
        if (visElement != null)
          uniformGrid.Add(visElement);
      }
      return uniformGrid;
    }

    private void FindConnectedNetworks(
      int cell,
      IUtilityNetworkMgr mgr,
      ICollection<UtilityNetwork> networks,
      List<int> visited)
    {
      if (visited.Contains(cell))
        return;
      visited.Add(cell);
      UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
      if (networkForCell == null)
        return;
      networks.Add(networkForCell);
      UtilityConnections connections = mgr.GetConnections(cell, false);
      if ((connections & UtilityConnections.Right) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Left) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Up) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Down) == (UtilityConnections) 0)
        return;
      this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
    }

    private struct BridgeInfo
    {
      public int cell;
      public KBatchedAnimController controller;
    }

    private struct EventInfo
    {
      public HandleVector<int>.Handle uiHandle;
    }

    private struct UIInfo
    {
      public GameObject instance;
      public Image image;
      public int cell;

      public UIInfo(ILogicUIElement ui_elem, LogicModeUI ui_data)
      {
        this.cell = ui_elem.GetLogicUICell();
        this.instance = Util.KInstantiate(ui_data.prefab, Grid.CellToPosCCC(this.cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, (string) null, true, 0);
        this.instance.SetActive(true);
        this.image = this.instance.GetComponent<Image>();
        this.image.raycastTarget = false;
        switch (ui_elem.GetLogicPortSpriteType())
        {
          case LogicPortSpriteType.Input:
            this.image.sprite = ui_data.inputSprite;
            break;
          case LogicPortSpriteType.Output:
            this.image.sprite = ui_data.outputSprite;
            break;
          case LogicPortSpriteType.ResetUpdate:
            this.image.sprite = ui_data.resetSprite;
            break;
        }
      }

      public void Release()
      {
        Util.KDestroyGameObject(this.instance);
      }
    }
  }

  public enum BringToFrontLayerSetting
  {
    None,
    Constant,
    Conditional,
  }

  public class ColorHighlightCondition
  {
    public Func<KMonoBehaviour, Color> highlight_color;
    public Func<KMonoBehaviour, bool> highlight_condition;

    public ColorHighlightCondition(
      Func<KMonoBehaviour, Color> highlight_color,
      Func<KMonoBehaviour, bool> highlight_condition)
    {
      this.highlight_color = highlight_color;
      this.highlight_condition = highlight_condition;
    }
  }

  public class None : OverlayModes.Mode
  {
    public static readonly HashedString ID = HashedString.Invalid;

    public override HashedString ViewMode()
    {
      return OverlayModes.None.ID;
    }

    public override string GetSoundName()
    {
      return "Off";
    }
  }

  public class PathProber : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (PathProber);

    public override HashedString ViewMode()
    {
      return OverlayModes.PathProber.ID;
    }

    public override string GetSoundName()
    {
      return "Off";
    }
  }

  public class Oxygen : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Oxygen);

    public override HashedString ViewMode()
    {
      return OverlayModes.Oxygen.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Oxygen);
    }

    public override void Enable()
    {
      base.Enable();
      int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
      int mask = LayerMask.GetMask("MaskedOverlay");
      SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
    }

    public override void Disable()
    {
      base.Disable();
      SelectTool.Instance.ClearLayerMask();
    }
  }

  public class Light : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Light);

    public override HashedString ViewMode()
    {
      return OverlayModes.Light.ID;
    }

    public override string GetSoundName()
    {
      return "Lights";
    }
  }

  public class Radiation : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Radiation);

    public override HashedString ViewMode()
    {
      return OverlayModes.Radiation.ID;
    }

    public override string GetSoundName()
    {
      return "Lights";
    }
  }

  public class Priorities : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Priorities);

    public override HashedString ViewMode()
    {
      return OverlayModes.Priorities.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Priorities);
    }
  }

  public class ThermalConductivity : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (ThermalConductivity);

    public override HashedString ViewMode()
    {
      return OverlayModes.ThermalConductivity.ID;
    }

    public override string GetSoundName()
    {
      return "HeatFlow";
    }
  }

  public class HeatFlow : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (HeatFlow);

    public override HashedString ViewMode()
    {
      return OverlayModes.HeatFlow.ID;
    }

    public override string GetSoundName()
    {
      return nameof (HeatFlow);
    }
  }

  public class Rooms : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Rooms);

    public override HashedString ViewMode()
    {
      return OverlayModes.Rooms.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Rooms);
    }

    public override List<LegendEntry> GetCustomLegendData()
    {
      List<LegendEntry> legendEntryList = new List<LegendEntry>();
      List<RoomType> roomTypeList = new List<RoomType>((IEnumerable<RoomType>) Db.Get().RoomTypes.resources);
      roomTypeList.Sort((Comparison<RoomType>) ((a, b) => a.sortKey.CompareTo(b.sortKey)));
      foreach (RoomType roomType in roomTypeList)
      {
        string desc = roomType.GetCriteriaString();
        if (roomType.effects != null && roomType.effects.Length > 0)
          desc = desc + "\n\n" + roomType.GetRoomEffectsString();
        legendEntryList.Add(new LegendEntry(roomType.Name + "\n" + roomType.effect, desc, roomType.category.color));
      }
      return legendEntryList;
    }
  }

  public abstract class Mode
  {
    private static List<KMonoBehaviour> workingTargets = new List<KMonoBehaviour>();
    public Dictionary<string, ToolParameterMenu.ToggleState> legendFilters;

    public static void Clear()
    {
      OverlayModes.Mode.workingTargets.Clear();
    }

    public abstract HashedString ViewMode();

    public virtual void Enable()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Disable()
    {
    }

    public virtual List<LegendEntry> GetCustomLegendData()
    {
      return (List<LegendEntry>) null;
    }

    public virtual Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
    {
      return (Dictionary<string, ToolParameterMenu.ToggleState>) null;
    }

    public virtual void OnFiltersChanged()
    {
    }

    public virtual void DisableOverlay()
    {
    }

    public abstract string GetSoundName();

    protected bool InFilter(
      string layer,
      Dictionary<string, ToolParameterMenu.ToggleState> filter)
    {
      return filter.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && filter[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On || filter.ContainsKey(layer) && filter[layer] == ToolParameterMenu.ToggleState.On;
    }

    public void RegisterSaveLoadListeners()
    {
      SaveManager saveManager = SaveLoader.Instance.saveManager;
      saveManager.onRegister += new System.Action<SaveLoadRoot>(this.OnSaveLoadRootRegistered);
      saveManager.onUnregister += new System.Action<SaveLoadRoot>(this.OnSaveLoadRootUnregistered);
    }

    public void UnregisterSaveLoadListeners()
    {
      SaveManager saveManager = SaveLoader.Instance.saveManager;
      saveManager.onRegister -= new System.Action<SaveLoadRoot>(this.OnSaveLoadRootRegistered);
      saveManager.onUnregister -= new System.Action<SaveLoadRoot>(this.OnSaveLoadRootUnregistered);
    }

    protected virtual void OnSaveLoadRootRegistered(SaveLoadRoot root)
    {
    }

    protected virtual void OnSaveLoadRootUnregistered(SaveLoadRoot root)
    {
    }

    protected void ProcessExistingSaveLoadRoots()
    {
      foreach (KeyValuePair<Tag, List<SaveLoadRoot>> list in SaveLoader.Instance.saveManager.GetLists())
      {
        foreach (SaveLoadRoot root in list.Value)
          this.OnSaveLoadRootRegistered(root);
      }
    }

    protected static UniformGrid<T> PopulatePartition<T>(ICollection<Tag> tags) where T : IUniformGridObject
    {
      Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
      UniformGrid<T> uniformGrid = new UniformGrid<T>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
      foreach (Tag tag in (IEnumerable<Tag>) tags)
      {
        List<SaveLoadRoot> saveLoadRootList = (List<SaveLoadRoot>) null;
        if (lists.TryGetValue(tag, out saveLoadRootList))
        {
          foreach (Component component1 in saveLoadRootList)
          {
            T component2 = component1.GetComponent<T>();
            if ((object) component2 != null)
              uniformGrid.Add(component2);
          }
        }
      }
      return uniformGrid;
    }

    protected static void ResetDisplayValues<T>(ICollection<T> targets) where T : MonoBehaviour
    {
      foreach (T target in (IEnumerable<T>) targets)
      {
        if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
        {
          KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            OverlayModes.Mode.ResetDisplayValues(component);
        }
      }
    }

    protected static void ResetDisplayValues(KBatchedAnimController controller)
    {
      controller.SetLayer(0);
      controller.HighlightColour = (Color32) Color.clear;
      controller.TintColour = (Color32) Color.white;
      controller.SetLayer(controller.GetComponent<KPrefabID>().defaultLayer);
    }

    protected static void RemoveOffscreenTargets<T>(
      ICollection<T> targets,
      Vector2I min,
      Vector2I max,
      System.Action<T> on_removed = null)
      where T : KMonoBehaviour
    {
      OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, min, max, (ICollection<Tag>) null, (System.Action<T>) (cmp =>
      {
        if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
          return;
        KBatchedAnimController component = cmp.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          OverlayModes.Mode.ResetDisplayValues(component);
        if (on_removed == null)
          return;
        on_removed(cmp);
      }));
      OverlayModes.Mode.workingTargets.Clear();
    }

    protected static void ClearOutsideViewObjects<T>(
      ICollection<T> targets,
      Vector2I vis_min,
      Vector2I vis_max,
      ICollection<Tag> item_ids,
      System.Action<T> on_remove)
      where T : KMonoBehaviour
    {
      OverlayModes.Mode.workingTargets.Clear();
      foreach (T target in (IEnumerable<T>) targets)
      {
        if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
        {
          Vector2I xy = Grid.PosToXY(target.transform.GetPosition());
          if (!(vis_min <= xy) || !(xy <= vis_max))
          {
            OverlayModes.Mode.workingTargets.Add((KMonoBehaviour) target);
          }
          else
          {
            KPrefabID component = target.GetComponent<KPrefabID>();
            if (item_ids != null && !item_ids.Contains(component.PrefabTag))
              OverlayModes.Mode.workingTargets.Add((KMonoBehaviour) target);
          }
        }
      }
      foreach (T workingTarget in OverlayModes.Mode.workingTargets)
      {
        if (!((UnityEngine.Object) workingTarget == (UnityEngine.Object) null))
        {
          if (on_remove != null)
            on_remove(workingTarget);
          targets.Remove(workingTarget);
        }
      }
      OverlayModes.Mode.workingTargets.Clear();
    }

    protected static void RemoveOffscreenTargets<T>(
      ICollection<T> targets,
      ICollection<T> working_targets,
      Vector2I vis_min,
      Vector2I vis_max,
      System.Action<T> on_removed = null,
      Func<T, bool> special_clear_condition = null)
      where T : IUniformGridObject
    {
      OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, working_targets, vis_min, vis_max, (System.Action<T>) (cmp =>
      {
        if ((object) cmp == null || on_removed == null)
          return;
        on_removed(cmp);
      }));
      if (special_clear_condition == null)
        return;
      working_targets.Clear();
      foreach (T target in (IEnumerable<T>) targets)
      {
        if (special_clear_condition(target))
          working_targets.Add(target);
      }
      foreach (T workingTarget in (IEnumerable<T>) working_targets)
      {
        if ((object) workingTarget != null)
        {
          if (on_removed != null)
            on_removed(workingTarget);
          targets.Remove(workingTarget);
        }
      }
      working_targets.Clear();
    }

    protected static void ClearOutsideViewObjects<T>(
      ICollection<T> targets,
      ICollection<T> working_targets,
      Vector2I vis_min,
      Vector2I vis_max,
      System.Action<T> on_removed = null)
      where T : IUniformGridObject
    {
      working_targets.Clear();
      foreach (T target in (IEnumerable<T>) targets)
      {
        if ((object) target != null)
        {
          Vector2 vector2_1 = target.PosMin();
          Vector2 vector2_2 = target.PosMin();
          if ((double) vector2_2.x < (double) vis_min.x || (double) vector2_2.y < (double) vis_min.y || ((double) vis_max.x < (double) vector2_1.x || (double) vis_max.y < (double) vector2_1.y))
            working_targets.Add(target);
        }
      }
      foreach (T workingTarget in (IEnumerable<T>) working_targets)
      {
        if ((object) workingTarget != null)
        {
          if (on_removed != null)
            on_removed(workingTarget);
          targets.Remove(workingTarget);
        }
      }
      working_targets.Clear();
    }

    protected static float GetDefaultDepth(KMonoBehaviour cmp)
    {
      BuildingComplete component = cmp.GetComponent<BuildingComplete>();
      return !((UnityEngine.Object) component != (UnityEngine.Object) null) ? Grid.GetLayerZ(Grid.SceneLayer.Creatures) : Grid.GetLayerZ(component.Def.SceneLayer);
    }

    protected void UpdateHighlightTypeOverlay<T>(
      Vector2I min,
      Vector2I max,
      ICollection<T> targets,
      ICollection<Tag> item_ids,
      OverlayModes.ColorHighlightCondition[] highlights,
      OverlayModes.BringToFrontLayerSetting bringToFrontSetting,
      int layer)
      where T : KMonoBehaviour
    {
      foreach (T target in (IEnumerable<T>) targets)
      {
        if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
        {
          Vector3 position = target.transform.GetPosition();
          int cell = Grid.PosToCell(position);
          if (Grid.IsValidCell(cell) && Grid.IsVisible(cell) && (min <= (Vector2) position && (Vector2) position <= max))
          {
            KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
            if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
            {
              int layer1 = 0;
              Color32 color32 = (Color32) Color.clear;
              if (highlights != null)
              {
                for (int index = 0; index < highlights.Length; ++index)
                {
                  OverlayModes.ColorHighlightCondition highlight = highlights[index];
                  if (highlight.highlight_condition((KMonoBehaviour) target))
                  {
                    color32 = (Color32) highlight.highlight_color((KMonoBehaviour) target);
                    layer1 = layer;
                    break;
                  }
                }
              }
              switch (bringToFrontSetting)
              {
                case OverlayModes.BringToFrontLayerSetting.Constant:
                  component.SetLayer(layer);
                  break;
                case OverlayModes.BringToFrontLayerSetting.Conditional:
                  component.SetLayer(layer1);
                  break;
              }
              component.HighlightColour = color32;
            }
          }
        }
      }
    }

    protected void DisableHighlightTypeOverlay<T>(ICollection<T> targets) where T : KMonoBehaviour
    {
      Color32 clear = (Color32) Color.clear;
      foreach (T target in (IEnumerable<T>) targets)
      {
        if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
        {
          KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            component.HighlightColour = clear;
            component.SetLayer(0);
          }
        }
      }
      targets.Clear();
    }

    protected void AddTargetIfVisible<T>(
      T instance,
      Vector2I vis_min,
      Vector2I vis_max,
      ICollection<T> targets,
      int layer,
      System.Action<T> on_added = null,
      Func<KMonoBehaviour, bool> should_add = null)
      where T : IUniformGridObject
    {
      if (instance.Equals((object) null))
        return;
      Vector2 vector2_1 = instance.PosMin();
      Vector2 vector2_2 = instance.PosMax();
      if ((double) vector2_2.x < (double) vis_min.x || (double) vector2_2.y < (double) vis_min.y || ((double) vector2_1.x > (double) vis_max.x || (double) vector2_1.y > (double) vis_max.y) || targets.Contains(instance))
        return;
      bool flag1 = false;
      for (int y = (int) vector2_1.y; (double) y <= (double) vector2_2.y; ++y)
      {
        for (int x = (int) vector2_1.x; (double) x <= (double) vector2_2.x; ++x)
        {
          int cell = Grid.XYToCell(x, y);
          if (Grid.Visible[cell] > (byte) 20 || !PropertyTextures.IsFogOfWarEnabled)
          {
            flag1 = true;
            break;
          }
        }
      }
      if (!flag1)
        return;
      bool flag2 = true;
      KMonoBehaviour kmonoBehaviour = (object) instance as KMonoBehaviour;
      if ((UnityEngine.Object) kmonoBehaviour != (UnityEngine.Object) null && should_add != null)
        flag2 = should_add(kmonoBehaviour);
      if (!flag2)
        return;
      if ((UnityEngine.Object) kmonoBehaviour != (UnityEngine.Object) null)
      {
        KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetLayer(layer);
      }
      targets.Add(instance);
      if (on_added == null)
        return;
      on_added(instance);
    }
  }

  public class ModeUtil
  {
    public static float GetHighlightScale()
    {
      return Mathf.SmoothStep(0.5f, 1f, Mathf.Abs(Mathf.Sin(Time.unscaledTime * 4f)));
    }
  }

  public class Power : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Power);
    private List<OverlayModes.Power.UpdatePowerInfo> updatePowerInfo = new List<OverlayModes.Power.UpdatePowerInfo>();
    private List<OverlayModes.Power.UpdateBatteryInfo> updateBatteryInfo = new List<OverlayModes.Power.UpdateBatteryInfo>();
    private List<LocText> powerLabels = new List<LocText>();
    private List<BatteryUI> batteryUIList = new List<BatteryUI>();
    private List<SaveLoadRoot> queuedAdds = new List<SaveLoadRoot>();
    private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();
    private HashSet<SaveLoadRoot> privateTargets = new HashSet<SaveLoadRoot>();
    private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();
    private List<int> visited = new List<int>();
    private int targetLayer;
    private int cameraLayerMask;
    private int selectionMask;
    private Canvas powerLabelParent;
    private LocText powerLabelPrefab;
    private Vector3 powerLabelOffset;
    private BatteryUI batteryUIPrefab;
    private Vector3 batteryUIOffset;
    private Vector3 batteryUITransformerOffset;
    private Vector3 batteryUISmallTransformerOffset;
    private Color32 consumerColour;
    private Color32 generatorColour;
    private Color32 buildingDisabledColour;
    private Color32 circuitUnpoweredColour;
    private Color32 circuitSafeColour;
    private Color32 circuitStrainingColour;
    private Color32 circuitOverloadingColour;
    private int freePowerLabelIdx;
    private int freeBatteryUIIdx;
    private UniformGrid<SaveLoadRoot> partition;

    public Power(
      Canvas powerLabelParent,
      LocText powerLabelPrefab,
      BatteryUI batteryUIPrefab,
      Vector3 powerLabelOffset,
      Vector3 batteryUIOffset,
      Vector3 batteryUITransformerOffset,
      Vector3 batteryUISmallTransformerOffset,
      Color consumerColour,
      Color generatorColour,
      Color buildingDisabledColour,
      Color32 circuitUnpoweredColour,
      Color32 circuitSafeColour,
      Color32 circuitStrainingColour,
      Color32 circuitOverloadingColour)
    {
      this.powerLabelParent = powerLabelParent;
      this.powerLabelPrefab = powerLabelPrefab;
      this.batteryUIPrefab = batteryUIPrefab;
      this.powerLabelOffset = powerLabelOffset;
      this.batteryUIOffset = batteryUIOffset;
      this.batteryUITransformerOffset = batteryUITransformerOffset;
      this.batteryUISmallTransformerOffset = batteryUISmallTransformerOffset;
      this.consumerColour = (Color32) consumerColour;
      this.generatorColour = (Color32) generatorColour;
      this.buildingDisabledColour = (Color32) buildingDisabledColour;
      this.circuitUnpoweredColour = circuitUnpoweredColour;
      this.circuitSafeColour = circuitSafeColour;
      this.circuitStrainingColour = circuitStrainingColour;
      this.circuitOverloadingColour = circuitOverloadingColour;
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.selectionMask = this.cameraLayerMask;
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Power.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Power);
    }

    public override void Enable()
    {
      Camera.main.cullingMask |= this.cameraLayerMask;
      SelectTool.Instance.SetLayerMask(this.selectionMask);
      this.RegisterSaveLoadListeners();
      this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>((ICollection<Tag>) OverlayScreen.WireIDs);
      GridCompositor.Instance.ToggleMinor(true);
    }

    public override void Disable()
    {
      OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      SelectTool.Instance.ClearLayerMask();
      this.UnregisterSaveLoadListeners();
      this.partition.Clear();
      this.layerTargets.Clear();
      this.privateTargets.Clear();
      this.queuedAdds.Clear();
      this.DisablePowerLabels();
      this.DisableBatteryUIs();
      GridCompositor.Instance.ToggleMinor(false);
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
      if (!OverlayScreen.WireIDs.Contains(saveLoadTag))
        return;
      this.partition.Add(item);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      if (this.layerTargets.Contains(item))
        this.layerTargets.Remove(item);
      this.partition.Remove(item);
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets, min, max, (System.Action<SaveLoadRoot>) null);
      using (new KProfiler.Region("UpdatePowerOverlay", (UnityEngine.Object) null))
      {
        IEnumerator enumerator = this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
            this.AddTargetIfVisible<SaveLoadRoot>((SaveLoadRoot) enumerator.Current, min, max, (ICollection<SaveLoadRoot>) this.layerTargets, this.targetLayer, (System.Action<SaveLoadRoot>) null, (Func<KMonoBehaviour, bool>) null);
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
        this.connectedNetworks.Clear();
        float num = 1f;
        GameObject gameObject = (GameObject) null;
        if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.hover != (UnityEngine.Object) null)
          gameObject = SelectTool.Instance.hover.gameObject;
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
          if (component != null)
          {
            int networkCell = component.GetNetworkCell();
            this.visited.Clear();
            this.FindConnectedNetworks(networkCell, (IUtilityNetworkMgr) Game.Instance.electricalConduitSystem, (ICollection<UtilityNetwork>) this.connectedNetworks, this.visited);
            this.visited.Clear();
            num = OverlayModes.ModeUtil.GetHighlightScale();
          }
        }
        CircuitManager circuitManager = Game.Instance.circuitManager;
        foreach (SaveLoadRoot layerTarget in this.layerTargets)
        {
          if (!((UnityEngine.Object) layerTarget == (UnityEngine.Object) null))
          {
            IBridgedNetworkItem component1 = layerTarget.GetComponent<IBridgedNetworkItem>();
            if (component1 != null)
            {
              KBatchedAnimController component2 = (component1 as KMonoBehaviour).GetComponent<KBatchedAnimController>();
              UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(component1.GetNetworkCell());
              ushort circuitID = networkForCell == null ? ushort.MaxValue : (ushort) networkForCell.id;
              float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitID);
              float wattageForCircuit = circuitManager.GetMaxSafeWattageForCircuit(circuitID);
              float neededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
              Color32 color32 = (double) wattsUsedByCircuit > 0.0 ? ((double) wattsUsedByCircuit <= (double) wattageForCircuit ? ((double) neededWhenActive <= (double) wattageForCircuit || (double) wattageForCircuit <= 0.0 || (double) wattsUsedByCircuit / (double) wattageForCircuit < 0.75 ? this.circuitSafeColour : this.circuitStrainingColour) : this.circuitOverloadingColour) : this.circuitUnpoweredColour;
              if (this.connectedNetworks.Count > 0 && component1.IsConnectedToNetworks((ICollection<UtilityNetwork>) this.connectedNetworks))
              {
                color32.r = (byte) ((double) color32.r * (double) num);
                color32.g = (byte) ((double) color32.g * (double) num);
                color32.b = (byte) ((double) color32.b * (double) num);
              }
              component2.TintColour = color32;
            }
          }
        }
      }
      this.queuedAdds.Clear();
      using (new KProfiler.Region("BatteryUI", (UnityEngine.Object) null))
      {
        foreach (Battery bat in Components.Batteries.Items)
        {
          Vector2I xy = Grid.PosToXY(bat.transform.GetPosition());
          if (min <= xy && xy <= max)
          {
            SaveLoadRoot component = bat.GetComponent<SaveLoadRoot>();
            if (!this.privateTargets.Contains(component))
            {
              this.AddBatteryUI(bat);
              this.queuedAdds.Add(component);
            }
          }
        }
        foreach (Generator generator in Components.Generators.Items)
        {
          Vector2I xy = Grid.PosToXY(generator.transform.GetPosition());
          if (min <= xy && xy <= max)
          {
            SaveLoadRoot component = generator.GetComponent<SaveLoadRoot>();
            if (!this.privateTargets.Contains(component))
            {
              this.privateTargets.Add(component);
              if ((UnityEngine.Object) generator.GetComponent<PowerTransformer>() == (UnityEngine.Object) null)
                this.AddPowerLabels((KMonoBehaviour) generator);
            }
          }
        }
        foreach (EnergyConsumer energyConsumer in Components.EnergyConsumers.Items)
        {
          Vector2I xy = Grid.PosToXY(energyConsumer.transform.GetPosition());
          if (min <= xy && xy <= max)
          {
            SaveLoadRoot component = energyConsumer.GetComponent<SaveLoadRoot>();
            if (!this.privateTargets.Contains(component))
            {
              this.privateTargets.Add(component);
              this.AddPowerLabels((KMonoBehaviour) energyConsumer);
            }
          }
        }
      }
      foreach (SaveLoadRoot queuedAdd in this.queuedAdds)
        this.privateTargets.Add(queuedAdd);
      this.queuedAdds.Clear();
      this.UpdatePowerLabels();
    }

    private LocText GetFreePowerLabel()
    {
      LocText locText;
      if (this.freePowerLabelIdx < this.powerLabels.Count)
      {
        locText = this.powerLabels[this.freePowerLabelIdx];
        ++this.freePowerLabelIdx;
      }
      else
      {
        locText = Util.KInstantiateUI<LocText>(this.powerLabelPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
        this.powerLabels.Add(locText);
        ++this.freePowerLabelIdx;
      }
      return locText;
    }

    private void UpdatePowerLabels()
    {
      foreach (OverlayModes.Power.UpdatePowerInfo updatePowerInfo in this.updatePowerInfo)
      {
        KMonoBehaviour kmonoBehaviour = updatePowerInfo.item;
        LocText powerLabel = updatePowerInfo.powerLabel;
        LocText unitLabel = updatePowerInfo.unitLabel;
        Generator generator = updatePowerInfo.generator;
        IEnergyConsumer consumer = updatePowerInfo.consumer;
        if ((UnityEngine.Object) updatePowerInfo.item == (UnityEngine.Object) null)
        {
          powerLabel.gameObject.SetActive(false);
        }
        else
        {
          if ((UnityEngine.Object) generator != (UnityEngine.Object) null && consumer == null)
          {
            int num = int.MaxValue;
            if ((UnityEngine.Object) generator.GetComponent<ManualGenerator>() == (UnityEngine.Object) null)
            {
              generator.GetComponent<Operational>();
              num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
            }
            else
              num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
            powerLabel.text = num == 0 ? num.ToString() : "+" + num.ToString();
            BuildingEnabledButton component = kmonoBehaviour.GetComponent<BuildingEnabledButton>();
            Color color = (Color) (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.IsEnabled ? this.generatorColour : this.buildingDisabledColour);
            powerLabel.color = color;
            unitLabel.color = color;
            Image outputIcon = generator.GetComponent<BuildingCellVisualizer>().GetOutputIcon();
            if ((UnityEngine.Object) outputIcon != (UnityEngine.Object) null)
              outputIcon.color = color;
          }
          if (consumer != null)
          {
            BuildingEnabledButton component = kmonoBehaviour.GetComponent<BuildingEnabledButton>();
            Color color = (Color) (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.IsEnabled ? this.consumerColour : this.buildingDisabledColour);
            int num = Mathf.Max(0, Mathf.RoundToInt(consumer.WattsNeededWhenActive));
            string str = num.ToString();
            powerLabel.text = num == 0 ? str : "-" + str;
            powerLabel.color = color;
            unitLabel.color = color;
            Image inputIcon = kmonoBehaviour.GetComponentInChildren<BuildingCellVisualizer>().GetInputIcon();
            if ((UnityEngine.Object) inputIcon != (UnityEngine.Object) null)
              inputIcon.color = color;
          }
        }
      }
      foreach (OverlayModes.Power.UpdateBatteryInfo updateBatteryInfo in this.updateBatteryInfo)
        updateBatteryInfo.ui.SetContent(updateBatteryInfo.battery);
    }

    private void AddPowerLabels(KMonoBehaviour item)
    {
      IEnergyConsumer componentInChildren1 = item.gameObject.GetComponentInChildren<IEnergyConsumer>();
      Generator componentInChildren2 = item.gameObject.GetComponentInChildren<Generator>();
      if (componentInChildren1 == null && !((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null))
        return;
      float num = -10f;
      if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null)
      {
        LocText freePowerLabel = this.GetFreePowerLabel();
        freePowerLabel.gameObject.SetActive(true);
        freePowerLabel.gameObject.name = item.gameObject.name + "power label";
        LocText component = freePowerLabel.transform.GetChild(0).GetComponent<LocText>();
        component.gameObject.SetActive(true);
        freePowerLabel.enabled = true;
        component.enabled = true;
        Vector3 pos = Grid.CellToPos(componentInChildren2.PowerCell, 0.5f, 0.0f, 0.0f);
        freePowerLabel.rectTransform.SetPosition(pos + this.powerLabelOffset + Vector3.up * (num * 0.02f));
        if (componentInChildren1 != null && componentInChildren1.PowerCell == componentInChildren2.PowerCell)
          num -= 15f;
        this.SetToolTip(freePowerLabel, (string) STRINGS.UI.OVERLAYS.POWER.WATTS_GENERATED);
        this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel, component, componentInChildren2, (IEnergyConsumer) null));
      }
      if (componentInChildren1 == null || componentInChildren1.GetType() == typeof (Battery))
        return;
      LocText freePowerLabel1 = this.GetFreePowerLabel();
      LocText component1 = freePowerLabel1.transform.GetChild(0).GetComponent<LocText>();
      freePowerLabel1.gameObject.SetActive(true);
      component1.gameObject.SetActive(true);
      freePowerLabel1.gameObject.name = item.gameObject.name + "power label";
      freePowerLabel1.enabled = true;
      component1.enabled = true;
      Vector3 pos1 = Grid.CellToPos(componentInChildren1.PowerCell, 0.5f, 0.0f, 0.0f);
      freePowerLabel1.rectTransform.SetPosition(pos1 + this.powerLabelOffset + Vector3.up * (num * 0.02f));
      this.SetToolTip(freePowerLabel1, (string) STRINGS.UI.OVERLAYS.POWER.WATTS_CONSUMED);
      this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel1, component1, (Generator) null, componentInChildren1));
    }

    private void DisablePowerLabels()
    {
      this.freePowerLabelIdx = 0;
      foreach (Component powerLabel in this.powerLabels)
        powerLabel.gameObject.SetActive(false);
      this.updatePowerInfo.Clear();
    }

    private void AddBatteryUI(Battery bat)
    {
      BatteryUI freeBatteryUi = this.GetFreeBatteryUI();
      freeBatteryUi.SetContent(bat);
      Vector3 pos = Grid.CellToPos(bat.PowerCell, 0.5f, 0.0f, 0.0f);
      bool flag = (UnityEngine.Object) bat.powerTransformer != (UnityEngine.Object) null;
      float num = 1f;
      Rotatable component = bat.GetComponent<Rotatable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.GetVisualizerFlipX())
        num = -1f;
      Vector3 vector3 = this.batteryUIOffset;
      if (flag)
        vector3 = bat.GetComponent<Building>().Def.WidthInCells != 2 ? this.batteryUITransformerOffset : this.batteryUISmallTransformerOffset;
      vector3.x *= num;
      freeBatteryUi.GetComponent<RectTransform>().SetPosition(Vector3.up + pos + vector3);
      this.updateBatteryInfo.Add(new OverlayModes.Power.UpdateBatteryInfo(bat, freeBatteryUi));
    }

    private void SetToolTip(LocText label, string text)
    {
      ToolTip component = label.GetComponent<ToolTip>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.toolTip = text;
    }

    private void DisableBatteryUIs()
    {
      this.freeBatteryUIIdx = 0;
      foreach (Component batteryUi in this.batteryUIList)
        batteryUi.gameObject.SetActive(false);
      this.updateBatteryInfo.Clear();
    }

    private BatteryUI GetFreeBatteryUI()
    {
      BatteryUI batteryUi;
      if (this.freeBatteryUIIdx < this.batteryUIList.Count)
      {
        batteryUi = this.batteryUIList[this.freeBatteryUIIdx];
        batteryUi.gameObject.SetActive(true);
        ++this.freeBatteryUIIdx;
      }
      else
      {
        batteryUi = Util.KInstantiateUI<BatteryUI>(this.batteryUIPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
        this.batteryUIList.Add(batteryUi);
        ++this.freeBatteryUIIdx;
      }
      return batteryUi;
    }

    private void FindConnectedNetworks(
      int cell,
      IUtilityNetworkMgr mgr,
      ICollection<UtilityNetwork> networks,
      List<int> visited)
    {
      if (visited.Contains(cell))
        return;
      visited.Add(cell);
      UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
      if (networkForCell == null)
        return;
      networks.Add(networkForCell);
      UtilityConnections connections = mgr.GetConnections(cell, false);
      if ((connections & UtilityConnections.Right) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Left) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Up) != (UtilityConnections) 0)
        this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
      if ((connections & UtilityConnections.Down) == (UtilityConnections) 0)
        return;
      this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
    }

    private struct UpdatePowerInfo
    {
      public KMonoBehaviour item;
      public LocText powerLabel;
      public LocText unitLabel;
      public Generator generator;
      public IEnergyConsumer consumer;

      public UpdatePowerInfo(
        KMonoBehaviour item,
        LocText power_label,
        LocText unit_label,
        Generator g,
        IEnergyConsumer c)
      {
        this.item = item;
        this.powerLabel = power_label;
        this.unitLabel = unit_label;
        this.generator = g;
        this.consumer = c;
      }
    }

    private struct UpdateBatteryInfo
    {
      public Battery battery;
      public BatteryUI ui;

      public UpdateBatteryInfo(Battery battery, BatteryUI ui)
      {
        this.battery = battery;
        this.ui = ui;
      }
    }
  }

  public class SolidConveyor : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (SolidConveyor);
    private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();
    private ICollection<Tag> targetIDs = (ICollection<Tag>) OverlayScreen.SolidConveyorIDs;
    private UniformGrid<SaveLoadRoot> partition;
    private int targetLayer;
    private int cameraLayerMask;
    private int selectionMask;

    public SolidConveyor()
    {
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.selectionMask = this.cameraLayerMask;
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.SolidConveyor.ID;
    }

    public override string GetSoundName()
    {
      return "LiquidVent";
    }

    public override void Enable()
    {
      this.RegisterSaveLoadListeners();
      this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
      Camera.main.cullingMask |= this.cameraLayerMask;
      SelectTool.Instance.SetLayerMask(this.selectionMask);
      GridCompositor.Instance.ToggleMinor(false);
      base.Enable();
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      if (!this.targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag()))
        return;
      this.partition.Add(item);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      if (this.layerTargets.Contains(item))
        this.layerTargets.Remove(item);
      this.partition.Remove(item);
    }

    public override void Disable()
    {
      OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      SelectTool.Instance.ClearLayerMask();
      this.UnregisterSaveLoadListeners();
      this.partition.Clear();
      this.layerTargets.Clear();
      GridCompositor.Instance.ToggleMinor(false);
      base.Disable();
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets, min, max, (System.Action<SaveLoadRoot>) null);
      IEnumerator enumerator = this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.AddTargetIfVisible<SaveLoadRoot>((SaveLoadRoot) enumerator.Current, min, max, (ICollection<SaveLoadRoot>) this.layerTargets, this.targetLayer, (System.Action<SaveLoadRoot>) null, (Func<KMonoBehaviour, bool>) null);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      Color32 white = (Color32) Color.white;
      foreach (SaveLoadRoot layerTarget in this.layerTargets)
      {
        if (!((UnityEngine.Object) layerTarget == (UnityEngine.Object) null))
          layerTarget.GetComponent<KBatchedAnimController>().TintColour = white;
      }
    }
  }

  public class Sound : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Sound);
    private HashSet<NoisePolluter> layerTargets = new HashSet<NoisePolluter>();
    private HashSet<Tag> targetIDs = new HashSet<Tag>();
    private OverlayModes.ColorHighlightCondition[] highlightConditions = new OverlayModes.ColorHighlightCondition[1]
    {
      new OverlayModes.ColorHighlightCondition((Func<KMonoBehaviour, Color>) (np =>
      {
        Color black = Color.black;
        Color b = Color.black;
        float t = 0.8f;
        if ((UnityEngine.Object) np != (UnityEngine.Object) null)
        {
          int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
          if ((double) (np as NoisePolluter).GetNoiseForCell(cell) < 36.0)
          {
            t = 1f;
            b = new Color(0.4f, 0.4f, 0.4f);
          }
        }
        return Color.Lerp(black, b, t);
      }), (Func<KMonoBehaviour, bool>) (np =>
      {
        List<GameObject> highlightedObjects = SelectToolHoverTextCard.highlightedObjects;
        bool flag = false;
        for (int index = 0; index < highlightedObjects.Count; ++index)
        {
          if ((UnityEngine.Object) highlightedObjects[index] != (UnityEngine.Object) null && (UnityEngine.Object) highlightedObjects[index] == (UnityEngine.Object) np.gameObject)
          {
            flag = true;
            break;
          }
        }
        return flag;
      }))
    };
    private UniformGrid<NoisePolluter> partition;
    private int targetLayer;
    private int cameraLayerMask;

    public Sound()
    {
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.targetIDs.UnionWith((IEnumerable<Tag>) Assets.GetPrefabTagsWithComponent<NoisePolluter>());
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Sound.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Sound);
    }

    public override void Enable()
    {
      this.RegisterSaveLoadListeners();
      this.targetIDs.UnionWith((IEnumerable<Tag>) Assets.GetPrefabTagsWithComponent<NoisePolluter>());
      this.partition = OverlayModes.Mode.PopulatePartition<NoisePolluter>((ICollection<Tag>) this.targetIDs);
      Camera.main.cullingMask |= this.cameraLayerMask;
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<NoisePolluter>((ICollection<NoisePolluter>) this.layerTargets, min, max, (System.Action<NoisePolluter>) null);
      IEnumerator enumerator = this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.AddTargetIfVisible<NoisePolluter>((NoisePolluter) enumerator.Current, min, max, (ICollection<NoisePolluter>) this.layerTargets, this.targetLayer, (System.Action<NoisePolluter>) null, (Func<KMonoBehaviour, bool>) null);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      this.UpdateHighlightTypeOverlay<NoisePolluter>(min, max, (ICollection<NoisePolluter>) this.layerTargets, (ICollection<Tag>) this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      if (!this.targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag()))
        return;
      this.partition.Add(item.GetComponent<NoisePolluter>());
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      NoisePolluter component = item.GetComponent<NoisePolluter>();
      if (this.layerTargets.Contains(component))
        this.layerTargets.Remove(component);
      this.partition.Remove(component);
    }

    public override void Disable()
    {
      this.DisableHighlightTypeOverlay<NoisePolluter>((ICollection<NoisePolluter>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      this.UnregisterSaveLoadListeners();
      this.partition.Clear();
      this.layerTargets.Clear();
    }
  }

  public class Suit : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Suit);
    private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();
    private List<GameObject> uiList = new List<GameObject>();
    private UniformGrid<SaveLoadRoot> partition;
    private ICollection<Tag> targetIDs;
    private int freeUiIdx;
    private int targetLayer;
    private int cameraLayerMask;
    private int selectionMask;
    private Canvas uiParent;
    private GameObject overlayPrefab;

    public Suit(Canvas ui_parent, GameObject overlay_prefab)
    {
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.selectionMask = this.cameraLayerMask;
      this.targetIDs = (ICollection<Tag>) OverlayScreen.SuitIDs;
      this.uiParent = ui_parent;
      this.overlayPrefab = overlay_prefab;
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Suit.ID;
    }

    public override string GetSoundName()
    {
      return "SuitRequired";
    }

    public override void Enable()
    {
      this.partition = new UniformGrid<SaveLoadRoot>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
      this.ProcessExistingSaveLoadRoots();
      this.RegisterSaveLoadListeners();
      Camera.main.cullingMask |= this.cameraLayerMask;
      SelectTool.Instance.SetLayerMask(this.selectionMask);
      GridCompositor.Instance.ToggleMinor(false);
      base.Enable();
    }

    public override void Disable()
    {
      this.UnregisterSaveLoadListeners();
      OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      SelectTool.Instance.ClearLayerMask();
      this.partition.Clear();
      this.partition = (UniformGrid<SaveLoadRoot>) null;
      this.layerTargets.Clear();
      for (int index = 0; index < this.uiList.Count; ++index)
        this.uiList[index].SetActive(false);
      GridCompositor.Instance.ToggleMinor(false);
      base.Disable();
    }

    protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
    {
      if (!this.targetIDs.Contains(item.GetComponent<KPrefabID>().GetSaveLoadTag()))
        return;
      this.partition.Add(item);
    }

    protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
    {
      if ((UnityEngine.Object) item == (UnityEngine.Object) null || (UnityEngine.Object) item.gameObject == (UnityEngine.Object) null)
        return;
      if (this.layerTargets.Contains(item))
        this.layerTargets.Remove(item);
      this.partition.Remove(item);
    }

    private GameObject GetFreeUI()
    {
      GameObject gameObject;
      if (this.freeUiIdx >= this.uiList.Count)
      {
        gameObject = Util.KInstantiateUI(this.overlayPrefab, this.uiParent.transform.gameObject, false);
        this.uiList.Add(gameObject);
      }
      else
        gameObject = this.uiList[this.freeUiIdx++];
      if (!gameObject.activeSelf)
        gameObject.SetActive(true);
      return gameObject;
    }

    public override void Update()
    {
      this.freeUiIdx = 0;
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>((ICollection<SaveLoadRoot>) this.layerTargets, min, max, (System.Action<SaveLoadRoot>) null);
      IEnumerator enumerator = this.partition.GetAllIntersecting(new Vector2((float) min.x, (float) min.y), new Vector2((float) max.x, (float) max.y)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.AddTargetIfVisible<SaveLoadRoot>((SaveLoadRoot) enumerator.Current, min, max, (ICollection<SaveLoadRoot>) this.layerTargets, this.targetLayer, (System.Action<SaveLoadRoot>) null, (Func<KMonoBehaviour, bool>) null);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      foreach (SaveLoadRoot layerTarget in this.layerTargets)
      {
        if (!((UnityEngine.Object) layerTarget == (UnityEngine.Object) null))
        {
          layerTarget.GetComponent<KBatchedAnimController>().TintColour = (Color32) Color.white;
          bool flag = false;
          if (layerTarget.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
          {
            flag = true;
          }
          else
          {
            SuitLocker component = layerTarget.GetComponent<SuitLocker>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              flag = (UnityEngine.Object) component.GetStoredOutfit() != (UnityEngine.Object) null;
          }
          if (flag)
            this.GetFreeUI().GetComponent<RectTransform>().SetPosition(layerTarget.transform.GetPosition());
        }
      }
      for (int freeUiIdx = this.freeUiIdx; freeUiIdx < this.uiList.Count; ++freeUiIdx)
      {
        if (this.uiList[freeUiIdx].activeSelf)
          this.uiList[freeUiIdx].SetActive(false);
      }
    }
  }

  public class Temperature : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (Temperature);
    public List<LegendEntry> temperatureLegend = new List<LegendEntry>()
    {
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.MAXHOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.1372549f, 0.1294118f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.VERYHOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.1411765f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.HOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0.0f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.TEMPERATE, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.2313726f, 0.9960784f, 0.2901961f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.COLD, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.1215686f, 0.6313726f, 1f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.VERYCOLD, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.1686275f, 0.7960784f, 1f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.9960784f, 0.9411765f))
    };
    public List<LegendEntry> heatFlowLegend = new List<LegendEntry>()
    {
      new LegendEntry((string) STRINGS.UI.OVERLAYS.HEATFLOW.HEATING, (string) STRINGS.UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0.9098039f, 0.2588235f, 0.1490196f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.HEATFLOW.NEUTRAL, (string) STRINGS.UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0.3098039f, 0.3098039f, 0.3098039f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.HEATFLOW.COOLING, (string) STRINGS.UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0.2509804f, 0.6313726f, 0.9058824f))
    };
    public List<LegendEntry> expandedTemperatureLegend = new List<LegendEntry>()
    {
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.MAXHOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.1372549f, 0.1294118f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.VERYHOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.1411765f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.HOT, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0.0f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.TEMPERATE, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.2313726f, 0.9960784f, 0.2901961f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.COLD, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.1215686f, 0.6313726f, 1f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.VERYCOLD, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.1686275f, 0.7960784f, 1f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, (string) STRINGS.UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.9960784f, 0.9411765f))
    };
    public List<LegendEntry> stateChangeLegend = new List<LegendEntry>()
    {
      new LegendEntry((string) STRINGS.UI.OVERLAYS.STATECHANGE.HIGHPOINT, (string) STRINGS.UI.OVERLAYS.STATECHANGE.TOOLTIPS.HIGHPOINT, new Color(0.8901961f, 0.1372549f, 0.1294118f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.STATECHANGE.STABLE, (string) STRINGS.UI.OVERLAYS.STATECHANGE.TOOLTIPS.STABLE, new Color(0.2313726f, 0.9960784f, 0.2901961f)),
      new LegendEntry((string) STRINGS.UI.OVERLAYS.STATECHANGE.LOWPOINT, (string) STRINGS.UI.OVERLAYS.STATECHANGE.TOOLTIPS.LOWPOINT, new Color(0.5019608f, 0.9960784f, 0.9411765f))
    };

    public Temperature()
    {
      this.legendFilters = this.CreateDefaultFilters();
      int num = SimDebugView.Instance.temperatureThresholds.Length - 1;
      for (int index = 0; index < this.temperatureLegend.Count; ++index)
      {
        this.temperatureLegend[index].colour = SimDebugView.Instance.temperatureThresholds[num - index].color;
        this.temperatureLegend[index].desc = string.Format(this.temperatureLegend[index].desc, (object) GameUtil.GetFormattedTemperature(SimDebugView.Instance.temperatureThresholds[num - index].value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
      }
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.Temperature.ID;
    }

    public override string GetSoundName()
    {
      return nameof (Temperature);
    }

    public override void Enable()
    {
      base.Enable();
    }

    public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
    {
      return new Dictionary<string, ToolParameterMenu.ToggleState>()
      {
        {
          ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE,
          ToolParameterMenu.ToggleState.On
        },
        {
          ToolParameterMenu.FILTERLAYERS.HEATFLOW,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.STATECHANGE,
          ToolParameterMenu.ToggleState.Off
        }
      };
    }

    public override List<LegendEntry> GetCustomLegendData()
    {
      switch (Game.Instance.temperatureOverlayMode)
      {
        case Game.TemperatureOverlayModes.AbsoluteTemperature:
          return this.temperatureLegend;
        case Game.TemperatureOverlayModes.AdaptiveTemperature:
          return this.expandedTemperatureLegend;
        case Game.TemperatureOverlayModes.HeatFlow:
          return this.heatFlowLegend;
        case Game.TemperatureOverlayModes.StateChange:
          return this.stateChangeLegend;
        default:
          return this.temperatureLegend;
      }
    }

    public override void OnFiltersChanged()
    {
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.HEATFLOW, this.legendFilters))
        Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.HeatFlow;
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE, this.legendFilters))
        Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AbsoluteTemperature;
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.ADAPTIVETEMPERATURE, this.legendFilters))
        Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AdaptiveTemperature;
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.STATECHANGE, this.legendFilters))
        Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.StateChange;
      switch (Game.Instance.temperatureOverlayMode)
      {
        case Game.TemperatureOverlayModes.AbsoluteTemperature:
          Infrared.Instance.SetMode(Infrared.Mode.Infrared);
          CameraController.Instance.ToggleColouredOverlayView(true);
          break;
        case Game.TemperatureOverlayModes.AdaptiveTemperature:
          Infrared.Instance.SetMode(Infrared.Mode.Infrared);
          CameraController.Instance.ToggleColouredOverlayView(true);
          break;
        case Game.TemperatureOverlayModes.HeatFlow:
          Infrared.Instance.SetMode(Infrared.Mode.Disabled);
          CameraController.Instance.ToggleColouredOverlayView(false);
          break;
        case Game.TemperatureOverlayModes.StateChange:
          Infrared.Instance.SetMode(Infrared.Mode.Disabled);
          CameraController.Instance.ToggleColouredOverlayView(false);
          break;
      }
    }

    public override void Disable()
    {
      Infrared.Instance.SetMode(Infrared.Mode.Disabled);
      CameraController.Instance.ToggleColouredOverlayView(false);
      base.Disable();
    }
  }

  public class TileMode : OverlayModes.Mode
  {
    public static readonly HashedString ID = (HashedString) nameof (TileMode);
    private HashSet<PrimaryElement> layerTargets = new HashSet<PrimaryElement>();
    private HashSet<Tag> targetIDs = new HashSet<Tag>();
    private OverlayModes.ColorHighlightCondition[] highlightConditions = new OverlayModes.ColorHighlightCondition[1]
    {
      new OverlayModes.ColorHighlightCondition((Func<KMonoBehaviour, Color>) (primary_element =>
      {
        Color color = Color.black;
        if ((UnityEngine.Object) primary_element != (UnityEngine.Object) null)
          color = (Color) (primary_element as PrimaryElement).Element.substance.uiColour;
        return color;
      }), (Func<KMonoBehaviour, bool>) (primary_element => primary_element.gameObject.GetComponent<KBatchedAnimController>().IsVisible()))
    };
    private int targetLayer;
    private int cameraLayerMask;

    public TileMode()
    {
      this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
      this.cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
      this.legendFilters = this.CreateDefaultFilters();
    }

    public override HashedString ViewMode()
    {
      return OverlayModes.TileMode.ID;
    }

    public override string GetSoundName()
    {
      return "SuitRequired";
    }

    public override void Enable()
    {
      base.Enable();
      this.targetIDs.UnionWith((IEnumerable<Tag>) Assets.GetPrefabTagsWithComponent<PrimaryElement>());
      Camera.main.cullingMask |= this.cameraLayerMask;
      int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
      int mask = LayerMask.GetMask("MaskedOverlay");
      SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
    }

    public override void Update()
    {
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      OverlayModes.Mode.RemoveOffscreenTargets<PrimaryElement>((ICollection<PrimaryElement>) this.layerTargets, min, max, (System.Action<PrimaryElement>) null);
      int height = max.y - min.y;
      int width = max.x - min.x;
      Extents extents = new Extents(min.x, min.y, width, height);
      List<ScenePartitionerEntry> gathered_entries = new List<ScenePartitionerEntry>();
      GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, gathered_entries);
      foreach (ScenePartitionerEntry partitionerEntry in gathered_entries)
      {
        PrimaryElement component = ((Component) partitionerEntry.obj).gameObject.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          this.TryAddObject(component, min, max);
      }
      gathered_entries.Clear();
      GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, gathered_entries);
      foreach (ScenePartitionerEntry partitionerEntry in gathered_entries)
      {
        BuildingComplete buildingComplete = (BuildingComplete) partitionerEntry.obj;
        PrimaryElement component = buildingComplete.gameObject.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && buildingComplete.gameObject.layer == 0)
          this.TryAddObject(component, min, max);
      }
      this.UpdateHighlightTypeOverlay<PrimaryElement>(min, max, (ICollection<PrimaryElement>) this.layerTargets, (ICollection<Tag>) this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
    }

    private void TryAddObject(PrimaryElement pe, Vector2I min, Vector2I max)
    {
      Element element = pe.Element;
      foreach (Tag tileOverlayFilter in Game.Instance.tileOverlayFilters)
      {
        if (element.HasTag(tileOverlayFilter))
        {
          this.AddTargetIfVisible<PrimaryElement>(pe, min, max, (ICollection<PrimaryElement>) this.layerTargets, this.targetLayer, (System.Action<PrimaryElement>) null, (Func<KMonoBehaviour, bool>) null);
          break;
        }
      }
    }

    public override void Disable()
    {
      base.Disable();
      this.DisableHighlightTypeOverlay<PrimaryElement>((ICollection<PrimaryElement>) this.layerTargets);
      Camera.main.cullingMask &= ~this.cameraLayerMask;
      this.layerTargets.Clear();
      SelectTool.Instance.ClearLayerMask();
    }

    public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
    {
      return new Dictionary<string, ToolParameterMenu.ToggleState>()
      {
        {
          ToolParameterMenu.FILTERLAYERS.ALL,
          ToolParameterMenu.ToggleState.On
        },
        {
          ToolParameterMenu.FILTERLAYERS.METAL,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.BUILDABLE,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.FILTER,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.ORGANICS,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.FARMABLE,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.GAS,
          ToolParameterMenu.ToggleState.Off
        },
        {
          ToolParameterMenu.FILTERLAYERS.LIQUID,
          ToolParameterMenu.ToggleState.Off
        }
      };
    }

    public override void OnFiltersChanged()
    {
      Game.Instance.tileOverlayFilters.Clear();
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.METAL, this.legendFilters))
      {
        Game.Instance.tileOverlayFilters.Add(GameTags.Metal);
        Game.Instance.tileOverlayFilters.Add(GameTags.RefinedMetal);
      }
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.BUILDABLE, this.legendFilters))
      {
        Game.Instance.tileOverlayFilters.Add(GameTags.BuildableRaw);
        Game.Instance.tileOverlayFilters.Add(GameTags.BuildableProcessed);
      }
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.FILTER, this.legendFilters))
        Game.Instance.tileOverlayFilters.Add(GameTags.Filter);
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE, this.legendFilters))
        Game.Instance.tileOverlayFilters.Add(GameTags.Liquifiable);
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUID, this.legendFilters))
        Game.Instance.tileOverlayFilters.Add(GameTags.Liquid);
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE, this.legendFilters))
        Game.Instance.tileOverlayFilters.Add(GameTags.ConsumableOre);
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.ORGANICS, this.legendFilters))
        Game.Instance.tileOverlayFilters.Add(GameTags.Organics);
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.FARMABLE, this.legendFilters))
      {
        Game.Instance.tileOverlayFilters.Add(GameTags.Farmable);
        Game.Instance.tileOverlayFilters.Add(GameTags.Agriculture);
      }
      if (this.InFilter(ToolParameterMenu.FILTERLAYERS.GAS, this.legendFilters))
      {
        Game.Instance.tileOverlayFilters.Add(GameTags.Breathable);
        Game.Instance.tileOverlayFilters.Add(GameTags.Unbreathable);
      }
      this.DisableHighlightTypeOverlay<PrimaryElement>((ICollection<PrimaryElement>) this.layerTargets);
      this.layerTargets.Clear();
      Game.Instance.ForceOverlayUpdate();
    }
  }
}
