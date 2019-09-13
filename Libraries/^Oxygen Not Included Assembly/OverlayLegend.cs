// Decompiled with JetBrains decompiler
// Type: OverlayLegend
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayLegend : KScreen
{
  private List<GameObject> activeDiagrams = new List<GameObject>();
  public static OverlayLegend Instance;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private Sprite emptySprite;
  [SerializeField]
  private List<OverlayLegend.OverlayInfo> overlayInfoList;
  [SerializeField]
  private GameObject unitPrefab;
  [SerializeField]
  private GameObject activeUnitsParent;
  [SerializeField]
  private GameObject diagramsParent;
  [SerializeField]
  private GameObject inactiveUnitsParent;
  [SerializeField]
  private GameObject toolParameterMenuPrefab;
  private ToolParameterMenu filterMenu;
  private OverlayModes.Mode currentMode;
  private List<GameObject> inactiveUnitObjs;
  private List<GameObject> activeUnitObjs;

  [ContextMenu("Set all fonts color")]
  public void SetAllFontsColor()
  {
    foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
    {
      for (int index = 0; index < overlayInfo.infoUnits.Count; ++index)
      {
        if (overlayInfo.infoUnits[index].fontColor == Color.clear)
          overlayInfo.infoUnits[index].fontColor = Color.white;
      }
    }
  }

  [ContextMenu("Set all tooltips")]
  public void SetAllTooltips()
  {
    foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
    {
      string oldValue = overlayInfo.name.Replace("NAME", string.Empty);
      for (int index = 0; index < overlayInfo.infoUnits.Count; ++index)
      {
        string str1 = overlayInfo.infoUnits[index].description.Replace(oldValue, string.Empty);
        string str2 = oldValue + "TOOLTIPS." + str1;
        overlayInfo.infoUnits[index].tooltip = str2;
      }
    }
  }

  [ContextMenu("Set Sliced for empty icons")]
  public void SetSlicedForEmptyIcons()
  {
    foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
    {
      for (int index = 0; index < overlayInfo.infoUnits.Count; ++index)
      {
        if ((UnityEngine.Object) overlayInfo.infoUnits[index].icon == (UnityEngine.Object) this.emptySprite)
          overlayInfo.infoUnits[index].sliceIcon = true;
      }
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) OverlayLegend.Instance == (UnityEngine.Object) null)
    {
      OverlayLegend.Instance = this;
      this.activeUnitObjs = new List<GameObject>();
      this.inactiveUnitObjs = new List<GameObject>();
      foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
      {
        overlayInfo.name = (string) Strings.Get(overlayInfo.name);
        for (int index = 0; index < overlayInfo.infoUnits.Count; ++index)
        {
          overlayInfo.infoUnits[index].description = (string) Strings.Get(overlayInfo.infoUnits[index].description);
          if (!string.IsNullOrEmpty(overlayInfo.infoUnits[index].tooltip))
            overlayInfo.infoUnits[index].tooltip = (string) Strings.Get(overlayInfo.infoUnits[index].tooltip);
        }
      }
      this.ClearLegend();
    }
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  protected override void OnLoadLevel()
  {
    OverlayLegend.Instance = (OverlayLegend) null;
    this.activeDiagrams.Clear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    base.OnLoadLevel();
  }

  private void SetLegend(OverlayLegend.OverlayInfo overlayInfo)
  {
    if (overlayInfo == null)
      this.ClearLegend();
    else if (!overlayInfo.isProgrammaticallyPopulated && (overlayInfo.infoUnits == null || overlayInfo.infoUnits.Count == 0))
    {
      this.ClearLegend();
    }
    else
    {
      this.Show(true);
      this.title.text = overlayInfo.name;
      if (overlayInfo.isProgrammaticallyPopulated)
        this.PopulateGeneratedLegend(overlayInfo, false);
      else
        this.PopulateOverlayInfoUnits(overlayInfo, false);
    }
  }

  public void SetLegend(OverlayModes.Mode mode, bool refreshing = false)
  {
    if (this.currentMode != null && this.currentMode.ViewMode() == mode.ViewMode() && !refreshing)
      return;
    this.ClearLegend();
    OverlayLegend.OverlayInfo overlayInfo = this.overlayInfoList.Find((Predicate<OverlayLegend.OverlayInfo>) (ol => ol.mode == mode.ViewMode()));
    this.currentMode = mode;
    this.SetLegend(overlayInfo);
  }

  public GameObject GetFreeUnitObject()
  {
    if (this.inactiveUnitObjs.Count == 0)
      this.inactiveUnitObjs.Add(Util.KInstantiateUI(this.unitPrefab, this.inactiveUnitsParent, false));
    GameObject inactiveUnitObj = this.inactiveUnitObjs[0];
    this.inactiveUnitObjs.RemoveAt(0);
    this.activeUnitObjs.Add(inactiveUnitObj);
    return inactiveUnitObj;
  }

  private void RemoveActiveObjects()
  {
    while (this.activeUnitObjs.Count > 0)
    {
      this.activeUnitObjs[0].transform.Find("Icon").GetComponent<Image>().enabled = false;
      this.activeUnitObjs[0].GetComponentInChildren<LocText>().enabled = false;
      this.activeUnitObjs[0].transform.SetParent(this.inactiveUnitsParent.transform);
      this.activeUnitObjs[0].SetActive(false);
      this.inactiveUnitObjs.Add(this.activeUnitObjs[0]);
      this.activeUnitObjs.RemoveAt(0);
    }
  }

  public void ClearLegend()
  {
    this.RemoveActiveObjects();
    for (int index = 0; index < this.activeDiagrams.Count; ++index)
    {
      if ((UnityEngine.Object) this.activeDiagrams[index] != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.activeDiagrams[index]);
    }
    this.activeDiagrams.Clear();
    Vector2 sizeDelta = this.diagramsParent.GetComponent<RectTransform>().sizeDelta;
    sizeDelta.y = 0.0f;
    this.diagramsParent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    this.Show(false);
  }

  public OverlayLegend.OverlayInfo GetOverlayInfo(OverlayModes.Mode mode)
  {
    for (int index = 0; index < this.overlayInfoList.Count; ++index)
    {
      if (this.overlayInfoList[index].mode == mode.ViewMode())
        return this.overlayInfoList[index];
    }
    return (OverlayLegend.OverlayInfo) null;
  }

  private void PopulateOverlayInfoUnits(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
  {
    if (overlayInfo.infoUnits != null && overlayInfo.infoUnits.Count > 0)
    {
      this.activeUnitsParent.SetActive(true);
      foreach (OverlayLegend.OverlayInfoUnit infoUnit in overlayInfo.infoUnits)
      {
        GameObject freeUnitObject = this.GetFreeUnitObject();
        if ((UnityEngine.Object) infoUnit.icon != (UnityEngine.Object) null)
        {
          Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
          component.gameObject.SetActive(true);
          component.sprite = infoUnit.icon;
          component.color = infoUnit.color;
          component.enabled = true;
          component.type = !infoUnit.sliceIcon ? Image.Type.Simple : Image.Type.Sliced;
        }
        else
          freeUnitObject.transform.Find("Icon").gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(infoUnit.description))
        {
          LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
          componentInChildren.text = string.Format(infoUnit.description, infoUnit.formatData);
          componentInChildren.color = infoUnit.fontColor;
          componentInChildren.enabled = true;
        }
        ToolTip component1 = freeUnitObject.GetComponent<ToolTip>();
        if (!string.IsNullOrEmpty(infoUnit.tooltip))
        {
          component1.toolTip = string.Format(infoUnit.tooltip, infoUnit.tooltipFormatData);
          component1.enabled = true;
        }
        else
          component1.enabled = false;
        freeUnitObject.SetActive(true);
        freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
      }
    }
    else
      this.activeUnitsParent.SetActive(false);
    if (isRefresh)
      return;
    if (overlayInfo.diagrams != null && overlayInfo.diagrams.Count > 0)
    {
      this.diagramsParent.SetActive(true);
      foreach (GameObject diagram in overlayInfo.diagrams)
        this.activeDiagrams.Add(Util.KInstantiateUI(diagram, this.diagramsParent, false));
    }
    else
      this.diagramsParent.SetActive(false);
  }

  private void PopulateGeneratedLegend(OverlayLegend.OverlayInfo info, bool isRefresh = false)
  {
    if (isRefresh)
      this.RemoveActiveObjects();
    if (info.infoUnits != null && info.infoUnits.Count > 0)
      this.PopulateOverlayInfoUnits(info, isRefresh);
    List<LegendEntry> customLegendData = this.currentMode.GetCustomLegendData();
    if (customLegendData != null)
    {
      this.activeUnitsParent.SetActive(true);
      foreach (LegendEntry legendEntry in customLegendData)
      {
        GameObject freeUnitObject = this.GetFreeUnitObject();
        Image component1 = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
        component1.gameObject.SetActive(true);
        component1.sprite = Assets.instance.LegendColourBox;
        component1.color = legendEntry.colour;
        component1.enabled = true;
        component1.type = Image.Type.Simple;
        LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
        componentInChildren.text = legendEntry.name;
        componentInChildren.color = Color.white;
        componentInChildren.enabled = true;
        ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
        component2.enabled = true;
        component2.toolTip = legendEntry.desc;
        freeUnitObject.SetActive(true);
        freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
      }
    }
    else
      this.activeUnitsParent.SetActive(false);
    if (isRefresh || this.currentMode.legendFilters == null)
      return;
    GameObject gameObject = Util.KInstantiateUI(this.toolParameterMenuPrefab, this.diagramsParent, false);
    this.activeDiagrams.Add(gameObject);
    this.diagramsParent.SetActive(true);
    this.filterMenu = gameObject.GetComponent<ToolParameterMenu>();
    this.filterMenu.PopulateMenu(this.currentMode.legendFilters);
    this.filterMenu.onParametersChanged += new System.Action(this.OnFiltersChanged);
    this.OnFiltersChanged();
  }

  private void OnFiltersChanged()
  {
    this.currentMode.OnFiltersChanged();
    this.PopulateGeneratedLegend(this.GetOverlayInfo(this.currentMode), true);
    Game.Instance.ForceOverlayUpdate();
  }

  private void DisableOverlay()
  {
    this.filterMenu.onParametersChanged -= new System.Action(this.OnFiltersChanged);
    this.filterMenu.ClearMenu();
    this.filterMenu.gameObject.SetActive(false);
    this.filterMenu = (ToolParameterMenu) null;
  }

  private void PopulateNoiseLegend(OverlayLegend.OverlayInfo info)
  {
    if (info.infoUnits != null && info.infoUnits.Count > 0)
      this.PopulateOverlayInfoUnits(info, false);
    string[] names = Enum.GetNames(typeof (AudioEventManager.NoiseEffect));
    Array values = Enum.GetValues(typeof (AudioEventManager.NoiseEffect));
    Color[] dbColours = SimDebugView.dbColours;
    for (int index = 0; index < names.Length; ++index)
    {
      GameObject freeUnitObject = this.GetFreeUnitObject();
      Image component1 = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
      component1.gameObject.SetActive(true);
      component1.sprite = Assets.instance.LegendColourBox;
      component1.color = index != 0 ? Color.Lerp(dbColours[index * 2], dbColours[Mathf.Min(dbColours.Length - 1, index * 2 + 1)], 0.5f) : new Color(1f, 1f, 1f, 0.7f);
      component1.enabled = true;
      component1.type = Image.Type.Simple;
      string upper = names[index].ToUpper();
      int num1 = (int) values.GetValue(index);
      int num2 = (int) values.GetValue(index);
      LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
      componentInChildren.text = (string) Strings.Get("STRINGS.UI.OVERLAYS.NOISE_POLLUTION.NAMES." + upper) + " " + string.Format((string) STRINGS.UI.OVERLAYS.NOISE_POLLUTION.RANGE, (object) num1);
      componentInChildren.color = Color.white;
      componentInChildren.enabled = true;
      ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
      component2.enabled = true;
      component2.toolTip = string.Format((string) Strings.Get("STRINGS.UI.OVERLAYS.NOISE_POLLUTION.TOOLTIPS." + upper), (object) num1, (object) num2);
      freeUnitObject.SetActive(true);
      freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
    }
  }

  [Serializable]
  public class OverlayInfoUnit
  {
    public Sprite icon;
    public string description;
    public string tooltip;
    public Color color;
    public Color fontColor;
    public object formatData;
    public object tooltipFormatData;
    public bool sliceIcon;

    public OverlayInfoUnit(
      Sprite icon,
      string description,
      Color color,
      Color fontColor,
      object formatData = null,
      bool sliceIcon = false)
    {
      this.icon = icon;
      this.description = description;
      this.color = color;
      this.fontColor = fontColor;
      this.formatData = formatData;
      this.sliceIcon = sliceIcon;
    }
  }

  [Serializable]
  public class OverlayInfo
  {
    public string name;
    public HashedString mode;
    public List<OverlayLegend.OverlayInfoUnit> infoUnits;
    public List<GameObject> diagrams;
    public bool isProgrammaticallyPopulated;
  }
}
