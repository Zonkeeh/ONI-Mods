// Decompiled with JetBrains decompiler
// Type: MeterScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeterScreen : KScreen, IRender1000ms
{
  private MeterScreen.DisplayInfo stressDisplayInfo = new MeterScreen.DisplayInfo()
  {
    selectedIndex = -1
  };
  private MeterScreen.DisplayInfo immunityDisplayInfo = new MeterScreen.DisplayInfo()
  {
    selectedIndex = -1
  };
  private int cachedMinionCount = -1;
  private long cachedCalories = -1;
  private Dictionary<string, float> rationsDict = new Dictionary<string, float>();
  [SerializeField]
  private LocText currentMinions;
  public ToolTip MinionsTooltip;
  public LocText StressText;
  public ToolTip StressTooltip;
  public LocText RationsText;
  public ToolTip RationsTooltip;
  public LocText SickText;
  public ToolTip SickTooltip;
  public TextStyleSetting ToolTipStyle_Header;
  public TextStyleSetting ToolTipStyle_Property;
  private bool startValuesSet;
  [SerializeField]
  private KToggle RedAlertButton;
  public ToolTip RedAlertTooltip;

  public static MeterScreen Instance { get; private set; }

  public static void DestroyInstance()
  {
    MeterScreen.Instance = (MeterScreen) null;
  }

  public bool StartValuesSet
  {
    get
    {
      return this.startValuesSet;
    }
  }

  protected override void OnPrefabInit()
  {
    MeterScreen.Instance = this;
  }

  protected override void OnSpawn()
  {
    this.StressTooltip.OnToolTip = new Func<string>(this.OnStressTooltip);
    this.SickTooltip.OnToolTip = new Func<string>(this.OnSickTooltip);
    this.RationsTooltip.OnToolTip = new Func<string>(this.OnRationsTooltip);
    this.RedAlertTooltip.OnToolTip = new Func<string>(this.OnRedAlertTooltip);
    this.RedAlertButton.onClick += (System.Action) (() => this.OnRedAlertClick());
  }

  private void OnRedAlertClick()
  {
    bool on = !VignetteManager.Instance.Get().IsRedAlertToggledOn();
    VignetteManager.Instance.Get().ToggleRedAlert(on);
    if (on)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
    else
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
  }

  public void Render1000ms(float dt)
  {
    this.Refresh();
  }

  public void InitializeValues()
  {
    if (this.startValuesSet)
      return;
    this.startValuesSet = true;
    this.Refresh();
  }

  private void Refresh()
  {
    this.RefreshMinions();
    this.RefreshRations();
    this.RefreshStress();
    this.RefreshSick();
  }

  private void RefreshMinions()
  {
    int count = Components.LiveMinionIdentities.Count;
    if (count == this.cachedMinionCount)
      return;
    this.cachedMinionCount = count;
    this.currentMinions.text = count.ToString("0");
    this.MinionsTooltip.ClearMultiStringTooltip();
    this.MinionsTooltip.AddMultiStringTooltip(string.Format((string) UI.TOOLTIPS.METERSCREEN_POPULATION, (object) count.ToString("0")), (ScriptableObject) this.ToolTipStyle_Header);
  }

  private void RefreshSick()
  {
    this.SickText.text = MeterScreen.CountSickDupes().ToString();
  }

  private void RefreshRations()
  {
    if (!((UnityEngine.Object) this.RationsText != (UnityEngine.Object) null) || !((UnityEngine.Object) RationTracker.Get() != (UnityEngine.Object) null))
      return;
    long num = (long) RationTracker.Get().CountRations((Dictionary<string, float>) null, true);
    if (this.cachedCalories == num)
      return;
    this.RationsText.text = GameUtil.GetFormattedCalories((float) num, GameUtil.TimeSlice.None, true);
    this.cachedCalories = num;
  }

  private IList<MinionIdentity> GetStressedMinions()
  {
    Amount stress_amount = Db.Get().Amounts.Stress;
    return (IList<MinionIdentity>) new List<MinionIdentity>((IEnumerable<MinionIdentity>) new List<MinionIdentity>((IEnumerable<MinionIdentity>) Components.LiveMinionIdentities.Items).OrderByDescending<MinionIdentity, float>((Func<MinionIdentity, float>) (x => stress_amount.Lookup((Component) x).value)));
  }

  private string OnStressTooltip()
  {
    float maxStress = GameUtil.GetMaxStress();
    this.StressTooltip.ClearMultiStringTooltip();
    this.StressTooltip.AddMultiStringTooltip(string.Format((string) UI.TOOLTIPS.METERSCREEN_AVGSTRESS, (object) (Mathf.Round(maxStress).ToString() + "%")), (ScriptableObject) this.ToolTipStyle_Header);
    Amount stress = Db.Get().Amounts.Stress;
    IList<MinionIdentity> stressedMinions = this.GetStressedMinions();
    for (int index = 0; index < stressedMinions.Count; ++index)
    {
      MinionIdentity id = stressedMinions[index];
      this.AddToolTipAmountPercentLine(this.StressTooltip, stress.Lookup((Component) id), id, index == this.stressDisplayInfo.selectedIndex);
    }
    return string.Empty;
  }

  private string OnSickTooltip()
  {
    int num1 = MeterScreen.CountSickDupes();
    this.SickTooltip.ClearMultiStringTooltip();
    this.SickTooltip.AddMultiStringTooltip(string.Format((string) UI.TOOLTIPS.METERSCREEN_SICK_DUPES, (object) num1.ToString()), (ScriptableObject) this.ToolTipStyle_Header);
    for (int index = 0; index < Components.LiveMinionIdentities.Count; ++index)
    {
      MinionIdentity liveMinionIdentity = Components.LiveMinionIdentities[index];
      string str1 = liveMinionIdentity.GetComponent<KSelectable>().GetName();
      Sicknesses sicknesses = liveMinionIdentity.GetComponent<MinionModifiers>().sicknesses;
      if (sicknesses.IsInfected())
      {
        string str2 = str1 + " (";
        int num2 = 0;
        foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
        {
          str2 = str2 + (num2 <= 0 ? string.Empty : ", ") + sicknessInstance.modifier.Name;
          ++num2;
        }
        str1 = str2 + ")";
      }
      bool selected = index == this.immunityDisplayInfo.selectedIndex;
      this.AddToolTipLine(this.SickTooltip, str1, selected);
    }
    return string.Empty;
  }

  private static int CountSickDupes()
  {
    int num = 0;
    foreach (Component component in Components.LiveMinionIdentities.Items)
    {
      if (component.GetComponent<MinionModifiers>().sicknesses.IsInfected())
        ++num;
    }
    return num;
  }

  private void AddToolTipLine(ToolTip tooltip, string str, bool selected)
  {
    if (selected)
      tooltip.AddMultiStringTooltip("<color=#F0B310FF>" + str + "</color>", (ScriptableObject) this.ToolTipStyle_Property);
    else
      tooltip.AddMultiStringTooltip(str, (ScriptableObject) this.ToolTipStyle_Property);
  }

  private void AddToolTipAmountPercentLine(
    ToolTip tooltip,
    AmountInstance amount,
    MinionIdentity id,
    bool selected)
  {
    string str = id.GetComponent<KSelectable>().GetName() + ":  " + Mathf.Round(amount.value).ToString() + "%";
    this.AddToolTipLine(tooltip, str, selected);
  }

  private string OnRationsTooltip()
  {
    this.rationsDict.Clear();
    float calories = RationTracker.Get().CountRations(this.rationsDict, true);
    this.RationsText.text = GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true);
    this.RationsTooltip.ClearMultiStringTooltip();
    this.RationsTooltip.AddMultiStringTooltip(string.Format((string) UI.TOOLTIPS.METERSCREEN_MEALHISTORY, (object) GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), (ScriptableObject) this.ToolTipStyle_Header);
    this.RationsTooltip.AddMultiStringTooltip(string.Empty, (ScriptableObject) this.ToolTipStyle_Property);
    foreach (KeyValuePair<string, float> keyValuePair in this.rationsDict.OrderByDescending<KeyValuePair<string, float>, float>((Func<KeyValuePair<string, float>, float>) (x =>
    {
      EdiblesManager.FoodInfo foodInfo = Game.Instance.ediblesManager.GetFoodInfo(x.Key);
      return x.Value * (foodInfo == null ? -1f : foodInfo.CaloriesPerUnit);
    })).ToDictionary<KeyValuePair<string, float>, string, float>((Func<KeyValuePair<string, float>, string>) (t => t.Key), (Func<KeyValuePair<string, float>, float>) (t => t.Value)))
    {
      EdiblesManager.FoodInfo foodInfo = Game.Instance.ediblesManager.GetFoodInfo(keyValuePair.Key);
      this.RationsTooltip.AddMultiStringTooltip(foodInfo == null ? string.Format((string) UI.TOOLTIPS.METERSCREEN_INVALID_FOOD_TYPE, (object) keyValuePair.Key) : string.Format("{0}: {1}", (object) foodInfo.Name, (object) GameUtil.GetFormattedCalories(keyValuePair.Value * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), (ScriptableObject) this.ToolTipStyle_Property);
    }
    return string.Empty;
  }

  private string OnRedAlertTooltip()
  {
    this.RedAlertTooltip.ClearMultiStringTooltip();
    this.RedAlertTooltip.AddMultiStringTooltip((string) UI.TOOLTIPS.RED_ALERT_TITLE, (ScriptableObject) this.ToolTipStyle_Header);
    this.RedAlertTooltip.AddMultiStringTooltip((string) UI.TOOLTIPS.RED_ALERT_CONTENT, (ScriptableObject) this.ToolTipStyle_Property);
    return string.Empty;
  }

  private void RefreshStress()
  {
    this.StressText.text = Mathf.Round(GameUtil.GetMaxStress()).ToString();
  }

  public void OnClickStress(BaseEventData base_ev_data)
  {
    IList<MinionIdentity> stressedMinions = this.GetStressedMinions();
    this.UpdateDisplayInfo(base_ev_data, ref this.stressDisplayInfo, stressedMinions);
    this.OnStressTooltip();
    this.StressTooltip.forceRefresh = true;
  }

  private IList<MinionIdentity> GetSickMinions()
  {
    return (IList<MinionIdentity>) Components.LiveMinionIdentities.Items;
  }

  public void OnClickImmunity(BaseEventData base_ev_data)
  {
    IList<MinionIdentity> sickMinions = this.GetSickMinions();
    this.UpdateDisplayInfo(base_ev_data, ref this.immunityDisplayInfo, sickMinions);
    this.OnSickTooltip();
    this.SickTooltip.forceRefresh = true;
  }

  private void UpdateDisplayInfo(
    BaseEventData base_ev_data,
    ref MeterScreen.DisplayInfo display_info,
    IList<MinionIdentity> minions)
  {
    PointerEventData pointerEventData = base_ev_data as PointerEventData;
    if (pointerEventData == null)
      return;
    switch (pointerEventData.button)
    {
      case PointerEventData.InputButton.Left:
        if (Components.LiveMinionIdentities.Count < display_info.selectedIndex)
          display_info.selectedIndex = -1;
        if (Components.LiveMinionIdentities.Count <= 0)
          break;
        display_info.selectedIndex = (display_info.selectedIndex + 1) % Components.LiveMinionIdentities.Count;
        MinionIdentity minion = minions[display_info.selectedIndex];
        SelectTool.Instance.SelectAndFocus(minion.transform.GetPosition(), minion.GetComponent<KSelectable>(), new Vector3(5f, 0.0f, 0.0f));
        break;
      case PointerEventData.InputButton.Right:
        display_info.selectedIndex = -1;
        break;
    }
  }

  private struct DisplayInfo
  {
    public int selectedIndex;
  }
}
