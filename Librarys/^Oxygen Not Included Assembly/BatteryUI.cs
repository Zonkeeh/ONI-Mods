// Decompiled with JetBrains decompiler
// Type: BatteryUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : KMonoBehaviour
{
  [SerializeField]
  private Color energyIncreaseColor = Color.green;
  [SerializeField]
  private Color energyDecreaseColor = Color.red;
  [SerializeField]
  private LocText currentKJLabel;
  [SerializeField]
  private Image batteryBG;
  [SerializeField]
  private Image batteryMeter;
  [SerializeField]
  private Sprite regularBatteryBG;
  [SerializeField]
  private Sprite bigBatteryBG;
  private LocText unitLabel;
  private const float UIUnit = 10f;
  private Dictionary<float, float> sizeMap;

  private void Initialize()
  {
    if ((Object) this.unitLabel == (Object) null)
      this.unitLabel = this.currentKJLabel.gameObject.GetComponentInChildrenOnly<LocText>();
    if (this.sizeMap != null && this.sizeMap.Count != 0)
      return;
    this.sizeMap = new Dictionary<float, float>();
    this.sizeMap.Add(20000f, 10f);
    this.sizeMap.Add(40000f, 25f);
    this.sizeMap.Add(60000f, 40f);
  }

  public void SetContent(Battery bat)
  {
    if ((Object) bat == (Object) null)
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(false);
    }
    else
    {
      this.Initialize();
      RectTransform component1 = this.batteryBG.GetComponent<RectTransform>();
      float x = 0.0f;
      foreach (KeyValuePair<float, float> size in this.sizeMap)
      {
        if ((double) bat.Capacity <= (double) size.Key)
        {
          x = size.Value;
          break;
        }
      }
      this.batteryBG.sprite = (double) bat.Capacity < 40000.0 ? this.regularBatteryBG : this.bigBatteryBG;
      float y = 25f;
      component1.sizeDelta = new Vector2(x, y);
      BuildingEnabledButton component2 = bat.GetComponent<BuildingEnabledButton>();
      Color color = !((Object) component2 != (Object) null) || component2.IsEnabled ? ((double) bat.PercentFull < (double) bat.PreviousPercentFull ? this.energyDecreaseColor : this.energyIncreaseColor) : Color.gray;
      this.batteryMeter.color = color;
      this.batteryBG.color = color;
      float num = this.batteryBG.GetComponent<RectTransform>().rect.height * bat.PercentFull;
      this.batteryMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(x - 5.5f, num - 5.5f);
      color.a = 1f;
      if (this.currentKJLabel.color != color)
      {
        this.currentKJLabel.color = color;
        this.unitLabel.color = color;
      }
      this.currentKJLabel.text = bat.JoulesAvailable.ToString("F0");
    }
  }
}
