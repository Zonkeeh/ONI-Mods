// Decompiled with JetBrains decompiler
// Type: TileScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileScreen : KScreen
{
  public Text nameLabel;
  public Text symbolLabel;
  public Text massTitleLabel;
  public Text massAmtLabel;
  public Image massIcon;
  public MinMaxSlider temperatureSlider;
  public Text temperatureSliderText;
  public Image temperatureSliderIcon;
  public Image solidIcon;
  public Image liquidIcon;
  public Image gasIcon;
  public Text solidText;
  public Text gasText;
  [SerializeField]
  private Color temperatureDefaultColour;
  [SerializeField]
  private Color temperatureTransitionColour;

  private bool SetSliderColour(float temperature, float transition_temperature)
  {
    if ((double) Mathf.Abs(temperature - transition_temperature) < 5.0)
    {
      this.temperatureSliderText.color = this.temperatureTransitionColour;
      this.temperatureSliderIcon.color = this.temperatureTransitionColour;
      return true;
    }
    this.temperatureSliderText.color = this.temperatureDefaultColour;
    this.temperatureSliderIcon.color = this.temperatureDefaultColour;
    return false;
  }

  private void DisplayTileInfo()
  {
    Vector3 mousePos = KInputManager.GetMousePos();
    mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
    if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
    {
      Element element1 = Grid.Element[cell];
      this.nameLabel.text = element1.name;
      float num1 = Grid.Mass[cell];
      string str = "kg";
      if ((double) num1 < 5.0)
      {
        num1 *= 1000f;
        str = "g";
      }
      if ((double) num1 < 5.0)
      {
        num1 *= 1000f;
        str = "mg";
      }
      if ((double) num1 < 5.0)
      {
        float f = num1 * 1000f;
        str = "mcg";
        num1 = Mathf.Floor(f);
      }
      this.massAmtLabel.text = string.Format("{0:0.0} {1}", (object) num1, (object) str);
      this.massTitleLabel.text = "mass";
      float num2 = Grid.Temperature[cell];
      if (element1.IsSolid)
      {
        this.solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
        this.gasIcon.gameObject.transform.parent.gameObject.SetActive(false);
        this.massIcon.sprite = this.solidIcon.sprite;
        this.solidText.text = ((int) element1.highTemp).ToString();
        this.gasText.text = string.Empty;
        this.liquidIcon.rectTransform.SetParent(this.solidIcon.transform.parent, true);
        this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(0.0f, 64f));
        this.SetSliderColour(num2, element1.highTemp);
        this.temperatureSlider.SetMinMaxValue(element1.highTemp, Mathf.Min(element1.highTemp + 100f, 4000f), Mathf.Max(element1.highTemp - 100f, 0.0f), Mathf.Min(element1.highTemp + 100f, 4000f));
      }
      else if (element1.IsLiquid)
      {
        this.solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
        this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
        this.massIcon.sprite = this.liquidIcon.sprite;
        this.solidText.text = ((int) element1.lowTemp).ToString();
        this.gasText.text = ((int) element1.highTemp).ToString();
        this.liquidIcon.rectTransform.SetParent(this.temperatureSlider.transform.parent, true);
        this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(-80f, 0.0f));
        if (!this.SetSliderColour(num2, element1.lowTemp))
          this.SetSliderColour(num2, element1.highTemp);
        this.temperatureSlider.SetMinMaxValue(element1.lowTemp, element1.highTemp, Mathf.Max(element1.lowTemp - 100f, 0.0f), Mathf.Min(element1.highTemp + 100f, 5200f));
      }
      else if (element1.IsGas)
      {
        this.solidText.text = string.Empty;
        this.gasText.text = ((int) element1.lowTemp).ToString();
        this.solidIcon.gameObject.transform.parent.gameObject.SetActive(false);
        this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
        this.massIcon.sprite = this.gasIcon.sprite;
        this.SetSliderColour(num2, element1.lowTemp);
        this.liquidIcon.rectTransform.SetParent(this.gasIcon.transform.parent, true);
        this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(0.0f, -64f));
        this.temperatureSlider.SetMinMaxValue(0.0f, Mathf.Max(element1.lowTemp - 100f, 0.0f), 0.0f, element1.lowTemp + 100f);
      }
      this.temperatureSlider.SetExtraValue(num2);
      this.temperatureSliderText.text = GameUtil.GetFormattedTemperature((float) (int) num2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
      Dictionary<int, float> info = FallingWater.instance.GetInfo(cell);
      if (info.Count <= 0)
        return;
      List<Element> elements = ElementLoader.elements;
      foreach (KeyValuePair<int, float> keyValuePair in info)
      {
        Element element2 = elements[keyValuePair.Key];
        Text nameLabel = this.nameLabel;
        nameLabel.text = nameLabel.text + "\n" + element2.name + string.Format(" {0:0.00} kg", (object) keyValuePair.Value);
      }
    }
    else
      this.nameLabel.text = "Unknown";
  }

  private void DisplayConduitFlowInfo()
  {
    HashedString mode = OverlayScreen.Instance.GetMode();
    UtilityNetworkManager<FlowUtilityNetwork, Vent> utilityNetworkManager = !(mode == OverlayModes.GasConduits.ID) ? Game.Instance.liquidConduitSystem : Game.Instance.gasConduitSystem;
    ConduitFlow conduitFlow = !(mode == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
    Vector3 mousePos = KInputManager.GetMousePos();
    mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
    if (Grid.IsValidCell(cell) && utilityNetworkManager.GetConnections(cell, true) != (UtilityConnections) 0)
    {
      ConduitFlow.ConduitContents contents = conduitFlow.GetContents(cell);
      Element elementByHash = ElementLoader.FindElementByHash(contents.element);
      float mass = contents.mass;
      float temperature = contents.temperature;
      this.nameLabel.text = elementByHash.name;
      string str = "kg";
      if ((double) mass < 5.0)
      {
        mass *= 1000f;
        str = "g";
      }
      this.massAmtLabel.text = string.Format("{0:0.0} {1}", (object) mass, (object) str);
      this.massTitleLabel.text = "mass";
      if (elementByHash.IsLiquid)
      {
        this.solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
        this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
        this.massIcon.sprite = this.liquidIcon.sprite;
        this.solidText.text = ((int) elementByHash.lowTemp).ToString();
        this.gasText.text = ((int) elementByHash.highTemp).ToString();
        this.liquidIcon.rectTransform.SetParent(this.temperatureSlider.transform.parent, true);
        this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(-80f, 0.0f));
        if (!this.SetSliderColour(temperature, elementByHash.lowTemp))
          this.SetSliderColour(temperature, elementByHash.highTemp);
        this.temperatureSlider.SetMinMaxValue(elementByHash.lowTemp, elementByHash.highTemp, Mathf.Max(elementByHash.lowTemp - 100f, 0.0f), Mathf.Min(elementByHash.highTemp + 100f, 5200f));
      }
      else if (elementByHash.IsGas)
      {
        this.solidText.text = string.Empty;
        this.gasText.text = ((int) elementByHash.lowTemp).ToString();
        this.solidIcon.gameObject.transform.parent.gameObject.SetActive(false);
        this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
        this.massIcon.sprite = this.gasIcon.sprite;
        this.SetSliderColour(temperature, elementByHash.lowTemp);
        this.liquidIcon.rectTransform.SetParent(this.gasIcon.transform.parent, true);
        this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(0.0f, -64f));
        this.temperatureSlider.SetMinMaxValue(0.0f, Mathf.Max(elementByHash.lowTemp - 100f, 0.0f), 0.0f, elementByHash.lowTemp + 100f);
      }
      this.temperatureSlider.SetExtraValue(temperature);
      this.temperatureSliderText.text = GameUtil.GetFormattedTemperature((float) (int) temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
    }
    else
    {
      this.nameLabel.text = "No Conduit";
      this.symbolLabel.text = string.Empty;
      this.massAmtLabel.text = string.Empty;
      this.massTitleLabel.text = string.Empty;
    }
  }

  private void Update()
  {
    this.transform.SetPosition(KInputManager.GetMousePos());
    HashedString mode = OverlayScreen.Instance.GetMode();
    if (mode == OverlayModes.GasConduits.ID || mode == OverlayModes.LiquidConduits.ID)
      this.DisplayConduitFlowInfo();
    else
      this.DisplayTileInfo();
  }
}
