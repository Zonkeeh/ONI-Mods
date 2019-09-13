// Decompiled with JetBrains decompiler
// Type: DebugBaseTemplateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TemplateClasses;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DebugBaseTemplateButton : KScreen
{
  private string SaveName = "enter_template_name";
  public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;
  public List<int> SelectedCells = new List<int>();
  private bool SaveAllBuildings;
  private bool SaveAllPickups;
  public KButton saveBaseButton;
  public KButton clearButton;
  private TemplateContainer pasteAndSelectAsset;
  public KButton AddSelectionButton;
  public KButton RemoveSelectionButton;
  public KButton clearSelectionButton;
  public KButton DestroyButton;
  public KButton DeconstructButton;
  public KButton MoveButton;
  public TemplateContainer moveAsset;
  public TMP_InputField nameField;
  private bool editing;
  public GameObject Placer;

  public static DebugBaseTemplateButton Instance { get; private set; }

  public static void DestroyInstance()
  {
    DebugBaseTemplateButton.Instance = (DebugBaseTemplateButton) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DebugBaseTemplateButton.Instance = this;
    this.gameObject.SetActive(false);
    this.SetupLocText();
    this.ConsumeMouseScroll = true;
    this.nameField.onFocus += (System.Action) (() => this.editing = true);
    this.nameField.onEndEdit.AddListener((UnityAction<string>) (_param1 => this.editing = false));
    this.nameField.onValueChanged.AddListener((UnityAction<string>) (_param1 => Util.ScrubInputField(this.nameField, true)));
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.ConsumeMouseScroll = true;
  }

  public override float GetSortKey()
  {
    return 10f;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.editing)
      e.Consumed = true;
    else
      base.OnKeyDown(e);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.saveBaseButton != (UnityEngine.Object) null)
    {
      this.saveBaseButton.onClick -= new System.Action(this.OnClickSaveBase);
      this.saveBaseButton.onClick += new System.Action(this.OnClickSaveBase);
    }
    if ((UnityEngine.Object) this.clearButton != (UnityEngine.Object) null)
    {
      this.clearButton.onClick -= new System.Action(this.OnClickClear);
      this.clearButton.onClick += new System.Action(this.OnClickClear);
    }
    if ((UnityEngine.Object) this.AddSelectionButton != (UnityEngine.Object) null)
    {
      this.AddSelectionButton.onClick -= new System.Action(this.OnClickAddSelection);
      this.AddSelectionButton.onClick += new System.Action(this.OnClickAddSelection);
    }
    if ((UnityEngine.Object) this.RemoveSelectionButton != (UnityEngine.Object) null)
    {
      this.RemoveSelectionButton.onClick -= new System.Action(this.OnClickRemoveSelection);
      this.RemoveSelectionButton.onClick += new System.Action(this.OnClickRemoveSelection);
    }
    if ((UnityEngine.Object) this.clearSelectionButton != (UnityEngine.Object) null)
    {
      this.clearSelectionButton.onClick -= new System.Action(this.OnClickClearSelection);
      this.clearSelectionButton.onClick += new System.Action(this.OnClickClearSelection);
    }
    if ((UnityEngine.Object) this.MoveButton != (UnityEngine.Object) null)
    {
      this.MoveButton.onClick -= new System.Action(this.OnClickMove);
      this.MoveButton.onClick += new System.Action(this.OnClickMove);
    }
    if ((UnityEngine.Object) this.DestroyButton != (UnityEngine.Object) null)
    {
      this.DestroyButton.onClick -= new System.Action(this.OnClickDestroySelection);
      this.DestroyButton.onClick += new System.Action(this.OnClickDestroySelection);
    }
    if (!((UnityEngine.Object) this.DeconstructButton != (UnityEngine.Object) null))
      return;
    this.DeconstructButton.onClick -= new System.Action(this.OnClickDeconstructSelection);
    this.DeconstructButton.onClick += new System.Action(this.OnClickDeconstructSelection);
  }

  private void SetupLocText()
  {
  }

  private void OnClickDestroySelection()
  {
    DebugTool.Instance.Activate(DebugTool.Type.Destroy);
  }

  private void OnClickDeconstructSelection()
  {
    DebugTool.Instance.Activate(DebugTool.Type.Deconstruct);
  }

  private void OnClickMove()
  {
    DebugTool.Instance.DeactivateTool((InterfaceTool) null);
    this.moveAsset = this.GetSelectionAsAsset();
    StampTool.Instance.Activate(this.moveAsset, false, false);
  }

  private void OnClickAddSelection()
  {
    DebugTool.Instance.Activate(DebugTool.Type.AddSelection);
  }

  private void OnClickRemoveSelection()
  {
    DebugTool.Instance.Activate(DebugTool.Type.RemoveSelection);
  }

  private void OnClickClearSelection()
  {
    this.ClearSelection();
    this.nameField.text = string.Empty;
  }

  private void OnClickClear()
  {
    DebugTool.Instance.Activate(DebugTool.Type.Clear);
  }

  protected override void OnDeactivate()
  {
    if ((UnityEngine.Object) DebugTool.Instance != (UnityEngine.Object) null)
      DebugTool.Instance.DeactivateTool((InterfaceTool) null);
    base.OnDeactivate();
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) DebugTool.Instance != (UnityEngine.Object) null))
      return;
    DebugTool.Instance.DeactivateTool((InterfaceTool) null);
  }

  private TemplateContainer GetSelectionAsAsset()
  {
    List<TemplateClasses.Cell> _cells = new List<TemplateClasses.Cell>();
    List<Prefab> _buildings = new List<Prefab>();
    List<Prefab> _pickupables = new List<Prefab>();
    List<Prefab> _primaryElementOres = new List<Prefab>();
    List<Prefab> _otherEntities = new List<Prefab>();
    HashSet<GameObject> _excludeEntities = new HashSet<GameObject>();
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (int selectedCell in this.SelectedCells)
    {
      num1 += (float) Grid.CellToXY(selectedCell).x;
      num2 += (float) Grid.CellToXY(selectedCell).y;
    }
    float num3;
    int x1;
    int y1;
    Grid.CellToXY(Grid.PosToCell(new Vector3(num1 / (float) this.SelectedCells.Count, num3 = num2 / (float) this.SelectedCells.Count, 0.0f)), out x1, out y1);
    for (int index = 0; index < this.SelectedCells.Count; ++index)
    {
      int selectedCell = this.SelectedCells[index];
      int x2;
      int y2;
      Grid.CellToXY(this.SelectedCells[index], out x2, out y2);
      Element element = ElementLoader.elements[(int) Grid.ElementIdx[selectedCell]];
      string _diseaseName = Grid.DiseaseIdx[selectedCell] == byte.MaxValue ? (string) null : Db.Get().Diseases[(int) Grid.DiseaseIdx[selectedCell]].Id;
      _cells.Add(new TemplateClasses.Cell(x2 - x1, y2 - y1, element.id, Grid.Temperature[selectedCell], Grid.Mass[selectedCell], _diseaseName, Grid.DiseaseCount[selectedCell], Grid.PreventFogOfWarReveal[this.SelectedCells[index]]));
    }
    for (int index = 0; index < Components.BuildingCompletes.Count; ++index)
    {
      BuildingComplete buildingComplete = Components.BuildingCompletes[index];
      if (!_excludeEntities.Contains(buildingComplete.gameObject))
      {
        int x2;
        int y2;
        Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) buildingComplete), out x2, out y2);
        if (this.SaveAllBuildings || this.SelectedCells.Contains(Grid.PosToCell((KMonoBehaviour) buildingComplete)))
        {
          foreach (int placementCell in buildingComplete.PlacementCells)
          {
            int x3;
            int y3;
            Grid.CellToXY(placementCell, out x3, out y3);
            string _diseaseName = Grid.DiseaseIdx[placementCell] == byte.MaxValue ? (string) null : Db.Get().Diseases[(int) Grid.DiseaseIdx[placementCell]].Id;
            _cells.Add(new TemplateClasses.Cell(x3 - x1, y3 - y1, Grid.Element[placementCell].id, Grid.Temperature[placementCell], Grid.Mass[placementCell], _diseaseName, Grid.DiseaseCount[placementCell], false));
          }
          Orientation _rotation = Orientation.Neutral;
          Rotatable component1 = buildingComplete.gameObject.GetComponent<Rotatable>();
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
            _rotation = component1.GetOrientation();
          SimHashes _element1 = SimHashes.Void;
          float num4 = 280f;
          string _disease1 = (string) null;
          int _disease_count1 = 0;
          PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            _element1 = component2.ElementID;
            num4 = component2.Temperature;
            _disease1 = component2.DiseaseIdx == byte.MaxValue ? (string) null : Db.Get().Diseases[(int) component2.DiseaseIdx].Id;
            _disease_count1 = component2.DiseaseCount;
          }
          List<Prefab.template_amount_value> templateAmountValueList1 = new List<Prefab.template_amount_value>();
          List<Prefab.template_amount_value> templateAmountValueList2 = new List<Prefab.template_amount_value>();
          foreach (AmountInstance amount in (Modifications<Amount, AmountInstance>) buildingComplete.gameObject.GetAmounts())
            templateAmountValueList1.Add(new Prefab.template_amount_value(amount.amount.Id, amount.value));
          Battery component3 = buildingComplete.GetComponent<Battery>();
          if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          {
            float joulesAvailable = component3.JoulesAvailable;
            templateAmountValueList2.Add(new Prefab.template_amount_value("joulesAvailable", joulesAvailable));
          }
          Unsealable component4 = buildingComplete.GetComponent<Unsealable>();
          if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
          {
            float num5 = !component4.facingRight ? 0.0f : 1f;
            templateAmountValueList2.Add(new Prefab.template_amount_value("sealedDoorDirection", num5));
          }
          LogicSwitch component5 = buildingComplete.GetComponent<LogicSwitch>();
          if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
          {
            float num5 = !component5.IsSwitchedOn ? 0.0f : 1f;
            templateAmountValueList2.Add(new Prefab.template_amount_value("switchSetting", num5));
          }
          x2 -= x1;
          y2 -= y1;
          float _temperature = Mathf.Clamp(num4, 1f, 99999f);
          Prefab prefab = new Prefab(buildingComplete.PrefabID().Name, Prefab.Type.Building, x2, y2, _element1, _temperature, 0.0f, _disease1, _disease_count1, _rotation, templateAmountValueList1.ToArray(), templateAmountValueList2.ToArray(), 0);
          Storage component6 = buildingComplete.gameObject.GetComponent<Storage>();
          if ((UnityEngine.Object) component6 != (UnityEngine.Object) null)
          {
            foreach (GameObject go in component6.items)
            {
              float _units = 0.0f;
              SimHashes _element2 = SimHashes.Vacuum;
              float _temp = 280f;
              string _disease2 = (string) null;
              int _disease_count2 = 0;
              bool _isOre = false;
              PrimaryElement component7 = go.GetComponent<PrimaryElement>();
              if ((UnityEngine.Object) component7 != (UnityEngine.Object) null)
              {
                _units = component7.Units;
                _element2 = component7.ElementID;
                _temp = component7.Temperature;
                _disease2 = component7.DiseaseIdx == byte.MaxValue ? (string) null : Db.Get().Diseases[(int) component7.DiseaseIdx].Id;
                _disease_count2 = component7.DiseaseCount;
              }
              float num5 = 0.0f;
              Rottable.Instance smi = go.gameObject.GetSMI<Rottable.Instance>();
              if (smi != null)
                num5 = smi.RotValue;
              if ((UnityEngine.Object) go.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
                _isOre = true;
              StorageItem _storage = new StorageItem(go.PrefabID().Name, _units, _temp, _element2, _disease2, _disease_count2, _isOre);
              if (smi != null)
                _storage.rottable.rotAmount = num5;
              prefab.AssignStorage(_storage);
              _excludeEntities.Add(go);
            }
          }
          _buildings.Add(prefab);
          _excludeEntities.Add(buildingComplete.gameObject);
        }
      }
    }
    for (int index = 0; index < _buildings.Count; ++index)
    {
      Prefab prefab = _buildings[index];
      int cell = Grid.XYToCell(prefab.location_x + x1, prefab.location_y + y1);
      string id = prefab.id;
      if (id != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (DebugBaseTemplateButton.\u003C\u003Ef__switch\u0024map2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          DebugBaseTemplateButton.\u003C\u003Ef__switch\u0024map2 = new Dictionary<string, int>(8)
          {
            {
              "Wire",
              1
            },
            {
              "InsulatedWire",
              1
            },
            {
              "HighWattageWire",
              1
            },
            {
              "GasConduit",
              2
            },
            {
              "InsulatedGasConduit",
              2
            },
            {
              "LiquidConduit",
              3
            },
            {
              "InsulatedLiquidConduit",
              3
            },
            {
              "LogicWire",
              4
            }
          };
        }
        int num4;
        // ISSUE: reference to a compiler-generated field
        if (DebugBaseTemplateButton.\u003C\u003Ef__switch\u0024map2.TryGetValue(id, out num4))
        {
          switch (num4)
          {
            case 1:
              prefab.connections = (int) Game.Instance.electricalConduitSystem.GetConnections(cell, true);
              continue;
            case 2:
              prefab.connections = (int) Game.Instance.gasConduitSystem.GetConnections(cell, true);
              continue;
            case 3:
              prefab.connections = (int) Game.Instance.liquidConduitSystem.GetConnections(cell, true);
              continue;
            case 4:
              prefab.connections = (int) Game.Instance.logicCircuitSystem.GetConnections(cell, true);
              continue;
          }
        }
      }
      prefab.connections = 0;
    }
    for (int index = 0; index < Components.Pickupables.Count; ++index)
    {
      if (Components.Pickupables[index].gameObject.activeSelf)
      {
        Pickupable pickupable = Components.Pickupables[index];
        if (!_excludeEntities.Contains(pickupable.gameObject))
        {
          int cell = Grid.PosToCell((KMonoBehaviour) pickupable);
          if ((this.SaveAllPickups || this.SelectedCells.Contains(cell)) && !(bool) ((UnityEngine.Object) Components.Pickupables[index].gameObject.GetComponent<MinionBrain>()))
          {
            int x2;
            int y2;
            Grid.CellToXY(cell, out x2, out y2);
            x2 -= x1;
            y2 -= y1;
            SimHashes _element = SimHashes.Void;
            float _temperature = 280f;
            float _units = 1f;
            string _disease = (string) null;
            int _disease_count = 0;
            float num4 = 0.0f;
            Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
            if (smi != null)
              num4 = smi.RotValue;
            PrimaryElement component = pickupable.gameObject.GetComponent<PrimaryElement>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              _element = component.ElementID;
              _units = component.Units;
              _temperature = component.Temperature;
              _disease = component.DiseaseIdx == byte.MaxValue ? (string) null : Db.Get().Diseases[(int) component.DiseaseIdx].Id;
              _disease_count = component.DiseaseCount;
            }
            if ((UnityEngine.Object) pickupable.gameObject.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
            {
              Prefab prefab = new Prefab(pickupable.PrefabID().Name, Prefab.Type.Ore, x2, y2, _element, _temperature, _units, _disease, _disease_count, Orientation.Neutral, (Prefab.template_amount_value[]) null, (Prefab.template_amount_value[]) null, 0);
              _primaryElementOres.Add(prefab);
            }
            else
            {
              Prefab prefab = new Prefab(pickupable.PrefabID().Name, Prefab.Type.Pickupable, x2, y2, _element, _temperature, _units, _disease, _disease_count, Orientation.Neutral, (Prefab.template_amount_value[]) null, (Prefab.template_amount_value[]) null, 0)
              {
                rottable = new TemplateClasses.Rottable()
              };
              prefab.rottable.rotAmount = num4;
              _pickupables.Add(prefab);
            }
            _excludeEntities.Add(pickupable.gameObject);
          }
        }
      }
    }
    this.GetEntities<Crop>((IEnumerable<Crop>) Components.Crops.Items, x1, y1, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Health>((IEnumerable<Health>) Components.Health.Items, x1, y1, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Harvestable>((IEnumerable<Harvestable>) Components.Harvestables.Items, x1, y1, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Edible>((IEnumerable<Edible>) Components.Edibles.Items, x1, y1, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<Geyser>(x1, y1, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<OccupyArea>(x1, y1, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    this.GetEntities<FogOfWarMask>(x1, y1, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
    TemplateContainer templateContainer = new TemplateContainer();
    templateContainer.Init(_cells, _buildings, _pickupables, _primaryElementOres, _otherEntities);
    return templateContainer;
  }

  private void GetEntities<T>(
    int rootX,
    int rootY,
    ref List<Prefab> _primaryElementOres,
    ref List<Prefab> _otherEntities,
    ref HashSet<GameObject> _excludeEntities)
  {
    this.GetEntities<object>((IEnumerable<object>) UnityEngine.Object.FindObjectsOfType(typeof (T)), rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
  }

  private void GetEntities<T>(
    IEnumerable<T> component_collection,
    int rootX,
    int rootY,
    ref List<Prefab> _primaryElementOres,
    ref List<Prefab> _otherEntities,
    ref HashSet<GameObject> _excludeEntities)
  {
    foreach (T component1 in component_collection)
    {
      if (!_excludeEntities.Contains(((object) component1 as KMonoBehaviour).gameObject) && ((object) component1 as KMonoBehaviour).gameObject.activeSelf)
      {
        int cell = Grid.PosToCell((object) component1 as KMonoBehaviour);
        if (this.SelectedCells.Contains(cell) && !(bool) ((UnityEngine.Object) ((object) component1 as KMonoBehaviour).gameObject.GetComponent<MinionBrain>()))
        {
          int x;
          int y;
          Grid.CellToXY(cell, out x, out y);
          x -= rootX;
          y -= rootY;
          SimHashes _element = SimHashes.Void;
          float _temperature = 280f;
          float _units = 1f;
          string _disease = (string) null;
          int _disease_count = 0;
          PrimaryElement component2 = ((object) component1 as KMonoBehaviour).gameObject.GetComponent<PrimaryElement>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            _element = component2.ElementID;
            _units = component2.Units;
            _temperature = component2.Temperature;
            _disease = component2.DiseaseIdx == byte.MaxValue ? (string) null : Db.Get().Diseases[(int) component2.DiseaseIdx].Id;
            _disease_count = component2.DiseaseCount;
          }
          List<Prefab.template_amount_value> templateAmountValueList = new List<Prefab.template_amount_value>();
          if (((object) component1 as KMonoBehaviour).gameObject.GetAmounts() != null)
          {
            foreach (AmountInstance amount in (Modifications<Amount, AmountInstance>) ((object) component1 as KMonoBehaviour).gameObject.GetAmounts())
              templateAmountValueList.Add(new Prefab.template_amount_value(amount.amount.Id, amount.value));
          }
          if ((UnityEngine.Object) ((object) component1 as KMonoBehaviour).gameObject.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
          {
            Prefab prefab = new Prefab(((object) component1 as KMonoBehaviour).PrefabID().Name, Prefab.Type.Ore, x, y, _element, _temperature, _units, _disease, _disease_count, Orientation.Neutral, templateAmountValueList.ToArray(), (Prefab.template_amount_value[]) null, 0);
            _primaryElementOres.Add(prefab);
            _excludeEntities.Add(((object) component1 as KMonoBehaviour).gameObject);
          }
          else
          {
            Prefab prefab = new Prefab(((object) component1 as KMonoBehaviour).PrefabID().Name, Prefab.Type.Other, x, y, _element, _temperature, _units, _disease, _disease_count, Orientation.Neutral, templateAmountValueList.ToArray(), (Prefab.template_amount_value[]) null, 0);
            _otherEntities.Add(prefab);
            _excludeEntities.Add(((object) component1 as KMonoBehaviour).gameObject);
          }
        }
      }
    }
  }

  private void OnClickSaveBase()
  {
    TemplateContainer selectionAsAsset = this.GetSelectionAsAsset();
    if (this.SelectedCells.Count <= 0)
    {
      Debug.LogWarning((object) "No cells selected. Use buttons above to select the area you want to save.");
    }
    else
    {
      this.SaveName = this.nameField.text;
      if (this.SaveName == null || this.SaveName == string.Empty)
      {
        Debug.LogWarning((object) "Invalid save name. Please enter a name in the input field.");
      }
      else
      {
        selectionAsAsset.SaveToYaml(this.SaveName);
        PasteBaseTemplateScreen.Instance.RefreshStampButtons();
      }
    }
  }

  public void ClearSelection()
  {
    for (int index = this.SelectedCells.Count - 1; index >= 0; --index)
      this.RemoveFromSelection(this.SelectedCells[index]);
  }

  public void DestroySelection()
  {
  }

  public void DeconstructSelection()
  {
  }

  public void AddToSelection(int cell)
  {
    if (this.SelectedCells.Contains(cell))
      return;
    GameObject gameObject = Util.KInstantiate(this.Placer, (GameObject) null, (string) null);
    Grid.Objects[cell, 7] = gameObject;
    Vector3 posCbc = Grid.CellToPosCBC(cell, this.visualizerLayer);
    float num = -0.15f;
    posCbc.z += num;
    gameObject.transform.SetPosition(posCbc);
    this.SelectedCells.Add(cell);
  }

  public void RemoveFromSelection(int cell)
  {
    if (!this.SelectedCells.Contains(cell))
      return;
    GameObject go = Grid.Objects[cell, 7];
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      go.DeleteObject();
    this.SelectedCells.Remove(cell);
  }
}
