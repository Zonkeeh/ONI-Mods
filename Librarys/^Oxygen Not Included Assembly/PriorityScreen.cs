// Decompiled with JetBrains decompiler
// Type: PriorityScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class PriorityScreen : KScreen
{
  protected List<PriorityButton> buttons_basic = new List<PriorityButton>();
  protected List<PriorityButton> buttons_emergency = new List<PriorityButton>();
  private PrioritySetting lastSelectedPriority = new PrioritySetting(PriorityScreen.PriorityClass.basic, -1);
  [SerializeField]
  protected PriorityButton buttonPrefab_basic;
  [SerializeField]
  protected GameObject EmergencyContainer;
  [SerializeField]
  protected PriorityButton button_emergency;
  [SerializeField]
  protected GameObject PriorityMenuContainer;
  [SerializeField]
  protected KButton button_priorityMenu;
  [SerializeField]
  protected KToggle button_toggleHigh;
  [SerializeField]
  protected GameObject diagram;
  private PrioritySetting priority;
  private System.Action<PrioritySetting> onClick;

  public void InstantiateButtons(System.Action<PrioritySetting> on_click, bool playSelectionSound = true)
  {
    this.onClick = on_click;
    for (int index = 1; index <= 9; ++index)
    {
      int priority_value = index;
      PriorityButton priorityButton = Util.KInstantiateUI<PriorityButton>(this.buttonPrefab_basic.gameObject, this.buttonPrefab_basic.transform.parent.gameObject, false);
      this.buttons_basic.Add(priorityButton);
      priorityButton.playSelectionSound = playSelectionSound;
      priorityButton.onClick = this.onClick;
      priorityButton.text.text = priority_value.ToString();
      priorityButton.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, priority_value);
      priorityButton.tooltip.SetSimpleTooltip(string.Format((string) UI.PRIORITYSCREEN.BASIC, (object) priority_value));
    }
    this.buttonPrefab_basic.gameObject.SetActive(false);
    this.button_emergency.playSelectionSound = playSelectionSound;
    this.button_emergency.onClick = this.onClick;
    this.button_emergency.priority = new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1);
    this.button_emergency.tooltip.SetSimpleTooltip((string) UI.PRIORITYSCREEN.TOP_PRIORITY);
    this.button_toggleHigh.gameObject.SetActive(false);
    this.PriorityMenuContainer.SetActive(true);
    this.button_priorityMenu.gameObject.SetActive(true);
    this.button_priorityMenu.onClick += new System.Action(this.PriorityButtonClicked);
    this.button_priorityMenu.GetComponent<ToolTip>().SetSimpleTooltip((string) UI.PRIORITYSCREEN.OPEN_JOBS_SCREEN);
    this.diagram.SetActive(false);
    this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
  }

  private void OnClick(PrioritySetting priority)
  {
    if (this.onClick == null)
      return;
    this.onClick(priority);
  }

  public void ShowDiagram(bool show)
  {
    this.diagram.SetActive(show);
  }

  public void ResetPriority()
  {
    this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
  }

  public void PriorityButtonClicked()
  {
    ManagementMenu.Instance.TogglePriorities();
  }

  private void RefreshButton(PriorityButton b, PrioritySetting priority, bool play_sound)
  {
    if (b.priority == priority)
    {
      b.toggle.Select();
      b.toggle.isOn = true;
      if (!play_sound)
        return;
      b.toggle.soundPlayer.Play(0);
    }
    else
      b.toggle.isOn = false;
  }

  public void SetScreenPriority(PrioritySetting priority, bool play_sound = false)
  {
    if (this.lastSelectedPriority == priority)
      return;
    this.lastSelectedPriority = priority;
    if (priority.priority_class == PriorityScreen.PriorityClass.high)
      this.button_toggleHigh.isOn = true;
    else if (priority.priority_class == PriorityScreen.PriorityClass.basic)
      this.button_toggleHigh.isOn = false;
    for (int index = 0; index < this.buttons_basic.Count; ++index)
    {
      this.buttons_basic[index].priority = new PrioritySetting(!this.button_toggleHigh.isOn ? PriorityScreen.PriorityClass.basic : PriorityScreen.PriorityClass.high, index + 1);
      this.buttons_basic[index].tooltip.SetSimpleTooltip(string.Format((string) (!this.button_toggleHigh.isOn ? UI.PRIORITYSCREEN.BASIC : UI.PRIORITYSCREEN.HIGH), (object) (index + 1)));
      this.RefreshButton(this.buttons_basic[index], this.lastSelectedPriority, play_sound);
    }
    this.RefreshButton(this.button_emergency, this.lastSelectedPriority, play_sound);
  }

  public PrioritySetting GetLastSelectedPriority()
  {
    return this.lastSelectedPriority;
  }

  public static void PlayPriorityConfirmSound(PrioritySetting priority)
  {
    EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Priority_Tool_Confirm", false), Vector3.zero);
    if (!instance.isValid())
      return;
    float num1 = 0.0f;
    if (priority.priority_class >= PriorityScreen.PriorityClass.high)
      num1 += 10f;
    if (priority.priority_class >= PriorityScreen.PriorityClass.topPriority)
      num1 = num1;
    float num2 = num1 + (float) priority.priority_value;
    int num3 = (int) instance.setParameterValue(nameof (priority), num2);
    KFMOD.EndOneShot(instance);
  }

  public enum PriorityClass
  {
    idle = -1, // 0xFFFFFFFF
    basic = 0,
    high = 1,
    personalNeeds = 2,
    topPriority = 3,
    compulsory = 4,
  }
}
