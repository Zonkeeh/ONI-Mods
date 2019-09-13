// Decompiled with JetBrains decompiler
// Type: CellSelectionObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CellSelectionObject : KMonoBehaviour
{
  private float zDepth = -0.5f;
  private Vector3 offset = new Vector3(0.5f, 0.5f, 0.0f);
  private Dictionary<HashedString, Func<bool>> overlayFilterMap = new Dictionary<HashedString, Func<bool>>();
  private bool isAppFocused = true;
  [HideInInspector]
  public CellSelectionObject alternateSelectionObject;
  private float zDepthSelected;
  private KBoxCollider2D mCollider;
  private KSelectable mSelectable;
  public GameObject SelectedDisplaySprite;
  public Sprite Sprite_Selected;
  public Sprite Sprite_Hover;
  public int mouseCell;
  private int selectedCell;
  public string ElementName;
  public Element element;
  public Element.State state;
  public float Mass;
  public float temperature;
  public Tag tags;
  public byte diseaseIdx;
  public int diseaseCount;
  private float updateTimer;

  public int SelectedCell
  {
    get
    {
      return this.selectedCell;
    }
  }

  public float FlowRate
  {
    get
    {
      return Grid.AccumulatedFlow[this.selectedCell] / 3f;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.mCollider = this.GetComponent<KBoxCollider2D>();
    this.mCollider.size = new Vector2(1.1f, 1.1f);
    this.mSelectable = this.GetComponent<KSelectable>();
    this.SelectedDisplaySprite.transform.localScale = Vector3.one * (25f / 64f);
    this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
    this.Subscribe(Game.Instance.gameObject, 493375141, new System.Action<object>(this.ForceRefreshUserMenu));
    this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, (Func<bool>) (() => Grid.Element[this.mouseCell].IsGas));
    this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, (Func<bool>) (() => Grid.Element[this.mouseCell].IsGas));
    this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, (Func<bool>) (() => Grid.Element[this.mouseCell].IsLiquid));
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  private void OnApplicationFocus(bool focusStatus)
  {
    this.isAppFocused = focusStatus;
  }

  private void Update()
  {
    if (!this.isAppFocused || (UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null || ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null || !Game.Instance.GameStarted()))
      return;
    this.SelectedDisplaySprite.SetActive(PlayerController.Instance.IsUsingDefaultTool() && !DebugHandler.HideUI);
    if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) this.mSelectable)
    {
      this.mouseCell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
      if (Grid.IsValidCell(this.mouseCell) && Grid.IsVisible(this.mouseCell))
      {
        bool flag = true;
        foreach (KeyValuePair<HashedString, Func<bool>> overlayFilter in this.overlayFilterMap)
        {
          if (overlayFilter.Value == null)
            Debug.LogWarning((object) "Filter value is null");
          else if ((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null)
            Debug.LogWarning((object) "Overlay screen Instance is null");
          else if (OverlayScreen.Instance.GetMode() == overlayFilter.Key)
          {
            flag = false;
            if (this.gameObject.layer != LayerMask.NameToLayer("MaskedOverlay"))
              this.gameObject.layer = LayerMask.NameToLayer("MaskedOverlay");
            if (!overlayFilter.Value())
            {
              this.SelectedDisplaySprite.SetActive(false);
              return;
            }
            break;
          }
        }
        if (flag && this.gameObject.layer != LayerMask.NameToLayer("Default"))
          this.gameObject.layer = LayerMask.NameToLayer("Default");
        Vector3 position = Grid.CellToPos(this.mouseCell, 0.0f, 0.0f, 0.0f) + this.offset;
        position.z = this.zDepth;
        this.transform.SetPosition(position);
        this.mSelectable.SetName(Grid.Element[this.mouseCell].name);
      }
      if ((UnityEngine.Object) SelectTool.Instance.hover != (UnityEngine.Object) this.mSelectable)
        this.SelectedDisplaySprite.SetActive(false);
    }
    this.updateTimer += Time.deltaTime;
    if ((double) this.updateTimer < 0.5)
      return;
    this.updateTimer = 0.0f;
    if (!((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.mSelectable))
      return;
    this.UpdateValues();
  }

  public void UpdateValues()
  {
    if (!Grid.IsValidCell(this.selectedCell))
      return;
    this.Mass = Grid.Mass[this.selectedCell];
    this.element = Grid.Element[this.selectedCell];
    this.ElementName = this.element.name;
    this.state = this.element.state;
    this.tags = this.element.GetMaterialCategoryTag();
    this.temperature = Grid.Temperature[this.selectedCell];
    this.diseaseIdx = Grid.DiseaseIdx[this.selectedCell];
    this.diseaseCount = Grid.DiseaseCount[this.selectedCell];
    this.mSelectable.SetName(Grid.Element[this.selectedCell].name);
    DetailsScreen.Instance.Trigger(-1514841199, (object) null);
    this.UpdateStatusItem();
    if (this.element.id == SimHashes.OxyRock)
    {
      this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.OxyRockEmitting, (object) this);
      if ((double) this.FlowRate <= 0.0)
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.OxyRockBlocked, (object) this);
      else
        this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.OxyRockBlocked, false);
    }
    else
    {
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.OxyRockEmitting, false);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.OxyRockBlocked, false);
    }
    if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(this.selectedCell))
      this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BuriedItem, (object) this);
    else
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.BuriedItem, true);
    this.mSelectable.ToggleStatusItem(Db.Get().MiscStatusItems.Space, CellSelectionObject.IsExposedToSpace(this.selectedCell), (object) null);
  }

  public static bool IsExposedToSpace(int cell)
  {
    return Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space && (UnityEngine.Object) Grid.Objects[cell, 2] == (UnityEngine.Object) null;
  }

  private void UpdateStatusItem()
  {
    if (this.element.id == SimHashes.Vacuum || this.element.id == SimHashes.Void)
    {
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalCategory, true);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, true);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalMass, true);
      this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalDisease, true);
    }
    else
    {
      if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalCategory))
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, (object) (Func<Element>) (() => this.element));
      if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalTemperature))
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, (object) this);
      if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalMass))
        this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalMass, (object) this);
      if (this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalDisease))
        return;
      this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalDisease, (object) this);
    }
  }

  public void OnObjectSelected(object o)
  {
    this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
    this.UpdateStatusItem();
    if (!((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.mSelectable))
      return;
    this.selectedCell = Grid.PosToCell(this.gameObject);
    this.UpdateValues();
    Vector3 position = Grid.CellToPos(this.selectedCell, 0.0f, 0.0f, 0.0f) + this.offset;
    position.z = this.zDepthSelected;
    this.transform.SetPosition(position);
    this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Selected;
  }

  public string MassString()
  {
    return string.Format("{0:0.00}", (object) this.Mass);
  }

  private void ForceRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.Refresh(this.gameObject);
  }
}
