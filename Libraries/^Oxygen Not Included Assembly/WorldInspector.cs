// Decompiled with JetBrains decompiler
// Type: WorldInspector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldInspector : MonoBehaviour
{
  private static readonly string[] massStrings = new string[4];
  private static readonly string[] invalidCellMassStrings = new string[4]
  {
    string.Empty,
    string.Empty,
    string.Empty,
    string.Empty
  };
  private static float cachedMass = -1f;
  private float temperaturePositionWidgetX_Min = 30f;
  private float temperaturePositionWidgetX_Max = 172f;
  public Text PropertyLeftText;
  public Text PropertyRightText;
  public Image PropertyIcon_Left;
  public Image PropertyIcon_Right;
  public WorldInspector.PropertyIcons propertySprites;
  public GameObject TemperatureNotch;
  public Image TemperatureNotchBG;
  public Image TemperatureNotchSymbol;
  public Text TemperatureTextDisplay;
  public Image TemperatureBarImage;
  public Image TransitionStateIcon_Low;
  public Image TransitionStateIcon_High;
  public ToolTip Tooltip_CurrentTemperature;
  public WorldInspector.StateSetting SolidState;
  public WorldInspector.StateSetting LiquidState;
  public WorldInspector.StateSetting GasState;
  private static Element cachedElement;

  private void Update()
  {
    this.Refresh();
  }

  private void Refresh()
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null)
      return;
    CellSelectionObject component1 = SelectTool.Instance.selected.GetComponent<CellSelectionObject>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.UpdateAsSimCell(component1);
    ElementChunk component2 = SelectTool.Instance.selected.GetComponent<ElementChunk>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      this.UpdateAsElementChunk(component2);
    Edible component3 = SelectTool.Instance.selected.GetComponent<Edible>();
    if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null))
      return;
    this.UpdateAsEdible(component3);
  }

  private void UpdateAsSimCell(CellSelectionObject cellObject)
  {
    string[] strArray = WorldInspector.MassStringsReadOnly(cellObject.mouseCell);
    this.PropertyLeftText.text = strArray[0] + strArray[1] + " " + strArray[2];
    this.PropertyIcon_Left.sprite = this.propertySprites.Mass;
    this.PropertyRightText.text = cellObject.tags.ProperName();
    this.PropertyIcon_Right.sprite = this.propertySprites.Resource;
    this.TemperatureTextDisplay.text = GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(cellObject.temperature), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    this.Tooltip_CurrentTemperature.toolTip = "Current Temperature: " + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(cellObject.temperature), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    int state = this.GetState(cellObject);
    this.SetStateColorScheme(state);
    this.Tooltip_CurrentTemperature.toolTip = this.SetCurrentTemperatureTooltip(cellObject.element, state);
    if ((cellObject.state & Element.State.TemperatureInsulated) == Element.State.TemperatureInsulated)
      this.TemperatureNotch.SetActive(false);
    else
      this.TemperatureNotch.SetActive(true);
    this.TemperatureNotch.rectTransform().anchoredPosition = new Vector2(this.GetTemperaturePosition(cellObject), this.TemperatureNotch.rectTransform().anchoredPosition.y);
  }

  private void UpdateAsElementChunk(ElementChunk _chunkObject)
  {
    PrimaryElement component = _chunkObject.GetComponent<PrimaryElement>();
    string empty = string.Empty;
    this.PropertyLeftText.text = string.Format("{0:0.00}", (object) component.Mass) + " kg";
    this.PropertyIcon_Left.sprite = this.propertySprites.Mass;
    this.PropertyRightText.text = ElementLoader.FindElementByHash(component.ElementID).GetMaterialCategoryTag().ProperName();
    this.PropertyIcon_Right.sprite = this.propertySprites.Resource;
    this.TemperatureTextDisplay.text = GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(component.Temperature), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    string str = "Current Temperature: " + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(component.Temperature), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    this.SetStateColorScheme(0);
    this.Tooltip_CurrentTemperature.toolTip = str + this.SetCurrentTemperatureTooltip(ElementLoader.FindElementByHash(component.ElementID), 0) + "\nMelts at: <color=yellow>" + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(ElementLoader.FindElementByHash(component.ElementID).highTemp), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + "</color>";
    this.TemperatureNotch.SetActive(true);
    this.TemperatureNotch.rectTransform().anchoredPosition = new Vector2(this.GetTemperaturePosition(component), this.TemperatureNotch.rectTransform().anchoredPosition.y);
  }

  private void UpdateAsEdible(Edible edibleObject)
  {
    string empty = string.Empty;
    this.PropertyLeftText.text = edibleObject.Units.ToString() + " Rations";
    this.PropertyIcon_Left.sprite = this.propertySprites.Rations;
    this.PropertyRightText.text = edibleObject.GetQuality().ToString();
    this.PropertyIcon_Right.sprite = this.propertySprites.Quality;
    float f = Grid.Temperature[Grid.PosToCell((KMonoBehaviour) edibleObject)];
    this.TemperatureTextDisplay.text = GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(f), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    string str = "Current Temperature: " + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(f), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    this.SetStateColorScheme(0);
    this.Tooltip_CurrentTemperature.toolTip = str + "\nRots at temperatures above: <color=yellow>" + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(edibleObject.FoodInfo.RotTemperature), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + "</color>";
    this.TemperatureNotch.SetActive(true);
    this.TemperatureNotch.rectTransform().anchoredPosition = new Vector2(this.GetTemperaturePosition(edibleObject), this.TemperatureNotch.rectTransform().anchoredPosition.y);
  }

  private int GetState(CellSelectionObject cellObject)
  {
    int num = 0;
    if (cellObject.element.IsGas)
      num = 2;
    else if (cellObject.element.IsLiquid)
      num = 1;
    else if (cellObject.element.IsSolid)
      num = 0;
    return num;
  }

  private void SetStateColorScheme(int state)
  {
    switch (state)
    {
      case 0:
        this.TemperatureBarImage.sprite = this.SolidState.TemperatureBarBG;
        this.TemperatureNotchSymbol.sprite = this.SolidState.StateIcon;
        this.TemperatureNotchBG.color = this.SolidState.StateColor;
        this.TemperatureTextDisplay.color = this.SolidState.StateColor;
        this.TransitionStateIcon_Low.sprite = this.SolidState.StateIcon;
        this.TransitionStateIcon_Low.color = this.SolidState.StateColor;
        this.TransitionStateIcon_High.sprite = this.LiquidState.StateIcon;
        this.TransitionStateIcon_High.color = this.LiquidState.StateColor;
        break;
      case 1:
        this.TemperatureBarImage.sprite = this.LiquidState.TemperatureBarBG;
        this.TemperatureNotchSymbol.sprite = this.LiquidState.StateIcon;
        this.TemperatureNotchBG.color = this.LiquidState.StateColor;
        this.TemperatureTextDisplay.color = this.LiquidState.StateColor;
        this.TransitionStateIcon_Low.sprite = this.SolidState.StateIcon;
        this.TransitionStateIcon_Low.color = this.SolidState.StateColor;
        this.TransitionStateIcon_High.sprite = this.GasState.StateIcon;
        this.TransitionStateIcon_High.color = this.GasState.StateColor;
        break;
      case 2:
        this.TemperatureBarImage.sprite = this.GasState.TemperatureBarBG;
        this.TemperatureNotchSymbol.sprite = this.GasState.StateIcon;
        this.TemperatureNotchBG.color = this.GasState.StateColor;
        this.TemperatureTextDisplay.color = this.GasState.StateColor;
        this.TransitionStateIcon_Low.sprite = this.LiquidState.StateIcon;
        this.TransitionStateIcon_Low.color = this.LiquidState.StateColor;
        this.TransitionStateIcon_High.sprite = this.GasState.StateIcon;
        this.TransitionStateIcon_High.color = this.GasState.StateColor;
        break;
    }
  }

  private string SetCurrentTemperatureTooltip(Element element, int state)
  {
    string str = string.Empty;
    switch (state)
    {
      case 0:
        str = str + "\nMelts at: <color=yellow>" + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(element.highTemp), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + "</color>";
        break;
      case 1:
        str = str + "\nFreezes at: <color=cyan>" + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(element.lowTemp), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + "</color>" + "\nEvaporates at: <color=red>" + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(element.highTemp), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + "</color>";
        break;
      case 2:
        str = str + "\nCondenses at: <color=yellow>" + GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(element.lowTemp), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false) + "</color>";
        break;
    }
    return str;
  }

  private float GetTemperaturePosition(CellSelectionObject cellObject)
  {
    float num1 = 1f;
    float num2 = 2000f;
    switch (this.GetState(cellObject))
    {
      case 0:
        num2 = cellObject.element.highTemp;
        break;
      case 1:
        num1 = cellObject.element.lowTemp;
        num2 = cellObject.element.highTemp;
        break;
      case 2:
        num1 = cellObject.element.lowTemp;
        num2 = num1 + 300f;
        break;
    }
    float num3 = num2 - num1;
    float positionWidgetXMin = this.temperaturePositionWidgetX_Min;
    float positionWidgetXMax = this.temperaturePositionWidgetX_Max;
    float num4 = positionWidgetXMax - positionWidgetXMin;
    return Mathf.Clamp(positionWidgetXMin + (cellObject.temperature - num1) * num4 / num3, positionWidgetXMin, positionWidgetXMax);
  }

  private float GetTemperaturePosition(PrimaryElement chunkObject)
  {
    float num1 = 1f;
    float num2 = ElementLoader.FindElementByHash(chunkObject.ElementID).highTemp - num1;
    float positionWidgetXMin = this.temperaturePositionWidgetX_Min;
    float positionWidgetXMax = this.temperaturePositionWidgetX_Max;
    float num3 = positionWidgetXMax - positionWidgetXMin;
    return Mathf.Clamp(positionWidgetXMin + (chunkObject.Temperature - num1) * num3 / num2, positionWidgetXMin, positionWidgetXMax);
  }

  private float GetTemperaturePosition(Edible edibleObject)
  {
    float num1 = 1f;
    float num2 = edibleObject.FoodInfo.RotTemperature - num1;
    float positionWidgetXMin = this.temperaturePositionWidgetX_Min;
    float positionWidgetXMax = this.temperaturePositionWidgetX_Max;
    float num3 = positionWidgetXMax - positionWidgetXMin;
    float num4 = Grid.Temperature[Grid.PosToCell((KMonoBehaviour) edibleObject)];
    return Mathf.Clamp(positionWidgetXMin + num4 * num3 / num2, positionWidgetXMin, positionWidgetXMax);
  }

  public static void DestroyStatics()
  {
    WorldInspector.cachedElement = (Element) null;
  }

  public static string[] MassStringsReadOnly(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return WorldInspector.invalidCellMassStrings;
    Element element = Grid.Element[cell];
    float num1 = Grid.Mass[cell];
    if (element == WorldInspector.cachedElement && (double) num1 == (double) WorldInspector.cachedMass)
      return WorldInspector.massStrings;
    WorldInspector.cachedElement = element;
    WorldInspector.cachedMass = num1;
    WorldInspector.massStrings[3] = " " + GameUtil.GetBreathableString(element, num1);
    if (element.id == SimHashes.Vacuum)
    {
      WorldInspector.massStrings[0] = "N/A";
      WorldInspector.massStrings[1] = string.Empty;
      WorldInspector.massStrings[2] = string.Empty;
    }
    else if (element.id == SimHashes.Unobtanium)
    {
      WorldInspector.massStrings[0] = (string) STRINGS.UI.NEUTRONIUMMASS;
      WorldInspector.massStrings[1] = string.Empty;
      WorldInspector.massStrings[2] = string.Empty;
    }
    else
    {
      WorldInspector.massStrings[2] = (string) STRINGS.UI.UNITSUFFIXES.MASS.KILOGRAM;
      if ((double) num1 < 5.0)
      {
        num1 *= 1000f;
        WorldInspector.massStrings[2] = (string) STRINGS.UI.UNITSUFFIXES.MASS.GRAM;
      }
      if ((double) num1 < 5.0)
      {
        num1 *= 1000f;
        WorldInspector.massStrings[2] = (string) STRINGS.UI.UNITSUFFIXES.MASS.MILLIGRAM;
      }
      if ((double) num1 < 5.0)
      {
        float f = num1 * 1000f;
        WorldInspector.massStrings[2] = (string) STRINGS.UI.UNITSUFFIXES.MASS.MICROGRAM;
        num1 = Mathf.Floor(f);
      }
      int num2 = Mathf.FloorToInt(num1);
      WorldInspector.massStrings[0] = num2.ToString();
      float num3 = (float) Mathf.FloorToInt((float) (10.0 * ((double) num1 - (double) num2)));
      WorldInspector.massStrings[1] = "." + num3.ToString();
    }
    return WorldInspector.massStrings;
  }

  [Serializable]
  public struct StateSetting
  {
    public Color StateColor;
    public Color StateColor_Dark;
    public Sprite TemperatureBarBG;
    public Sprite StateIcon;
  }

  [Serializable]
  public struct PropertyIcons
  {
    public Sprite Mass;
    public Sprite Rations;
    public Sprite Quality;
    public Sprite Resource;
  }
}
