// Decompiled with JetBrains decompiler
// Type: ResearchEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ResearchEntry : KMonoBehaviour
{
  public static readonly string UnlockedTechKey = "UnlockedTech";
  [Header("Colors")]
  [SerializeField]
  private Color defaultColor = Color.blue;
  [SerializeField]
  private Color completedColor = Color.yellow;
  [SerializeField]
  private Color pendingColor = Color.magenta;
  [SerializeField]
  private Color completedHeaderColor = Color.grey;
  [SerializeField]
  private Color incompleteHeaderColor = Color.grey;
  [SerializeField]
  private Color pendingHeaderColor = Color.grey;
  private bool isOn = true;
  public int lineThickness_active = 6;
  public int lineThickness_inactive = 2;
  private Dictionary<string, GameObject> progressBarsByResearchTypeID = new Dictionary<string, GameObject>();
  private Dictionary<string, object> unlockedTechMetric = new Dictionary<string, object>()
  {
    {
      ResearchEntry.UnlockedTechKey,
      (object) null
    }
  };
  [Header("Labels")]
  [SerializeField]
  private LocText researchName;
  [Header("Transforms")]
  [SerializeField]
  private Transform progressBarContainer;
  [SerializeField]
  private Transform lineContainer;
  [Header("Prefabs")]
  [SerializeField]
  private GameObject iconPanel;
  [SerializeField]
  private GameObject iconPrefab;
  [SerializeField]
  private GameObject linePrefab;
  [SerializeField]
  private GameObject progressBarPrefab;
  [Header("Graphics")]
  [SerializeField]
  private Image BG;
  [SerializeField]
  private Image titleBG;
  [SerializeField]
  private Image borderHighlight;
  [SerializeField]
  private Image filterHighlight;
  [SerializeField]
  private Image filterLowlight;
  [SerializeField]
  private Sprite hoverBG;
  [SerializeField]
  private Sprite completedBG;
  private Sprite defaultBG;
  [MyCmpGet]
  private KToggle toggle;
  private ResearchScreen researchScreen;
  private Dictionary<Tech, UILineRenderer> techLineMap;
  private Tech targetTech;
  private Coroutine fadeRoutine;
  public Color activeLineColor;
  public Color inactiveLineColor;
  public Material StandardUIMaterial;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.techLineMap = new Dictionary<Tech, UILineRenderer>();
    this.BG.color = this.defaultColor;
    foreach (Tech key in this.targetTech.requiredTech)
    {
      float num = (float) ((double) this.targetTech.width / 2.0 + 18.0);
      Vector2 vector2_1 = Vector2.zero;
      Vector2 vector2_2 = Vector2.zero;
      if ((double) key.center.y > (double) this.targetTech.center.y + 2.0)
      {
        vector2_1 = new Vector2(0.0f, 20f);
        vector2_2 = new Vector2(0.0f, -20f);
      }
      else if ((double) key.center.y < (double) this.targetTech.center.y - 2.0)
      {
        vector2_1 = new Vector2(0.0f, -20f);
        vector2_2 = new Vector2(0.0f, 20f);
      }
      UILineRenderer component = Util.KInstantiateUI(this.linePrefab, this.lineContainer.gameObject, true).GetComponent<UILineRenderer>();
      component.Points = new Vector2[4]
      {
        new Vector2(0.0f, 0.0f) + vector2_1,
        new Vector2((float) -(((double) this.targetTech.center.x - (double) num - ((double) key.center.x + (double) num)) / 2.0), 0.0f) + vector2_1,
        new Vector2((float) -(((double) this.targetTech.center.x - (double) num - ((double) key.center.x + (double) num)) / 2.0), key.center.y - this.targetTech.center.y) + vector2_2,
        new Vector2((float) (-((double) this.targetTech.center.x - (double) num - ((double) key.center.x + (double) num)) + 2.0), key.center.y - this.targetTech.center.y) + vector2_2
      };
      component.LineThickness = (float) this.lineThickness_inactive;
      component.color = this.inactiveLineColor;
      this.techLineMap.Add(key, component);
    }
    this.QueueStateChanged(false);
    if (this.targetTech == null)
      return;
    foreach (TechInstance research in Research.Instance.GetResearchQueue())
    {
      if (research.tech == this.targetTech)
        this.QueueStateChanged(true);
    }
  }

  public void SetTech(Tech newTech)
  {
    if (newTech == null)
    {
      Debug.LogError((object) "The research provided is null!");
    }
    else
    {
      if (this.targetTech == newTech)
        return;
      foreach (ResearchType type in Research.Instance.researchTypes.Types)
      {
        if (newTech.costsByResearchTypeID.ContainsKey(type.id) && (double) newTech.costsByResearchTypeID[type.id] > 0.0)
        {
          GameObject gameObject = Util.KInstantiateUI(this.progressBarPrefab, this.progressBarContainer.gameObject, true);
          Image componentsInChild = gameObject.GetComponentsInChildren<Image>()[2];
          Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
          componentsInChild.color = type.color;
          component.sprite = type.sprite;
          this.progressBarsByResearchTypeID[type.id] = gameObject;
        }
      }
      if ((UnityEngine.Object) this.researchScreen == (UnityEngine.Object) null)
        this.researchScreen = this.transform.parent.GetComponentInParent<ResearchScreen>();
      if (newTech.IsComplete())
        this.ResearchCompleted(false);
      this.targetTech = newTech;
      this.researchName.text = this.targetTech.Name;
      string empty = string.Empty;
      foreach (TechItem unlockedItem in this.targetTech.unlockedItems)
      {
        KImage componentInChildrenOnly = this.GetFreeIcon().GetComponentInChildrenOnly<KImage>();
        componentInChildrenOnly.transform.parent.gameObject.SetActive(true);
        if (empty != string.Empty)
          empty += ", ";
        empty += unlockedItem.Name;
        string str = string.Format("{0}\n{1}", (object) unlockedItem.Name, (object) unlockedItem.description);
        componentInChildrenOnly.GetComponent<ToolTip>().toolTip = str;
        componentInChildrenOnly.sprite = unlockedItem.UISprite();
      }
      this.researchName.GetComponent<ToolTip>().toolTip = string.Format("{0}\n{1}\n\n{2}", (object) this.targetTech.Name, (object) this.targetTech.desc, (object) string.Format((string) STRINGS.UI.RESEARCHSCREEN_UNLOCKSTOOLTIP, (object) empty));
      this.toggle.ClearOnClick();
      this.toggle.onClick += new System.Action(this.OnResearchClicked);
      this.toggle.onPointerEnter += (KToggle.PointerEvent) (() =>
      {
        this.researchScreen.TurnEverythingOff();
        this.OnHover(true, this.targetTech);
      });
      this.toggle.soundPlayer.AcceptClickCondition = (Func<bool>) (() => !this.targetTech.IsComplete());
      this.toggle.onPointerExit += (KToggle.PointerEvent) (() => this.researchScreen.TurnEverythingOff());
    }
  }

  public void SetEverythingOff()
  {
    if (!this.isOn)
      return;
    this.borderHighlight.gameObject.SetActive(false);
    foreach (KeyValuePair<Tech, UILineRenderer> techLine in this.techLineMap)
    {
      techLine.Value.LineThickness = (float) this.lineThickness_inactive;
      techLine.Value.color = this.inactiveLineColor;
    }
    this.isOn = false;
  }

  public void SetEverythingOn()
  {
    if (this.isOn)
      return;
    this.UpdateProgressBars();
    this.borderHighlight.gameObject.SetActive(true);
    foreach (KeyValuePair<Tech, UILineRenderer> techLine in this.techLineMap)
    {
      techLine.Value.LineThickness = (float) this.lineThickness_active;
      techLine.Value.color = this.activeLineColor;
    }
    this.transform.SetAsLastSibling();
    this.isOn = true;
  }

  private void OnHover(bool entered, Tech hoverSource)
  {
    this.SetEverythingOn();
    foreach (Tech tech in this.targetTech.requiredTech)
    {
      ResearchEntry entry = this.researchScreen.GetEntry(tech);
      if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
        entry.OnHover(entered, this.targetTech);
    }
  }

  private void OnResearchClicked()
  {
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch != null && activeResearch.tech != this.targetTech)
      this.researchScreen.CancelResearch();
    Research.Instance.SetActiveResearch(this.targetTech, true);
    if (DebugHandler.InstantBuildMode)
      Research.Instance.CompleteQueue();
    this.UpdateProgressBars();
  }

  private void OnResearchCanceled()
  {
    if (this.targetTech.IsComplete())
      return;
    this.toggle.ClearOnClick();
    this.toggle.onClick += new System.Action(this.OnResearchClicked);
    this.researchScreen.CancelResearch();
    Research.Instance.CancelResearch(this.targetTech, true);
  }

  public void QueueStateChanged(bool isSelected)
  {
    if (isSelected)
    {
      if (!this.targetTech.IsComplete())
      {
        this.toggle.isOn = true;
        this.BG.color = this.pendingColor;
        this.titleBG.color = this.pendingHeaderColor;
        this.toggle.ClearOnClick();
        this.toggle.onClick += new System.Action(this.OnResearchCanceled);
      }
      else
        this.toggle.isOn = false;
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
        keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
      foreach (Graphic componentsInChild in this.iconPanel.GetComponentsInChildren<Image>())
        componentsInChild.material = this.StandardUIMaterial;
    }
    else if (this.targetTech.IsComplete())
    {
      this.toggle.isOn = false;
      this.BG.color = this.completedColor;
      this.titleBG.color = this.completedHeaderColor;
      this.defaultColor = this.completedColor;
      this.toggle.ClearOnClick();
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
        keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
      foreach (Graphic componentsInChild in this.iconPanel.GetComponentsInChildren<Image>())
        componentsInChild.material = this.StandardUIMaterial;
    }
    else
    {
      this.toggle.isOn = false;
      this.BG.color = this.defaultColor;
      this.titleBG.color = this.incompleteHeaderColor;
      this.toggle.ClearOnClick();
      this.toggle.onClick += new System.Action(this.OnResearchClicked);
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
        keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = new Color(0.5215687f, 0.5215687f, 0.5215687f);
    }
  }

  public void UpdateFilterState(string filter_string)
  {
    bool flag = false;
    if (!string.IsNullOrEmpty(filter_string))
    {
      flag = STRINGS.UI.StripLinkFormatting(this.researchName.text).ToLower().Contains(filter_string);
      if (!flag)
      {
        foreach (TechItem unlockedItem in this.targetTech.unlockedItems)
        {
          if (STRINGS.UI.StripLinkFormatting(unlockedItem.Name).ToLower().Contains(filter_string))
          {
            flag = true;
            break;
          }
          if (STRINGS.UI.StripLinkFormatting(unlockedItem.description).ToLower().Contains(filter_string))
          {
            flag = true;
            break;
          }
        }
      }
    }
    this.filterHighlight.gameObject.SetActive(flag);
    this.filterLowlight.gameObject.SetActive(!flag && !string.IsNullOrEmpty(filter_string));
  }

  public void SetPercentage(float percent)
  {
  }

  public void UpdateProgressBars()
  {
    foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
    {
      Transform child = keyValuePair.Value.transform.GetChild(0);
      float num;
      if (this.targetTech.IsComplete())
      {
        num = 1f;
        child.GetComponentInChildren<LocText>().text = ((double) this.targetTech.costsByResearchTypeID[keyValuePair.Key]).ToString() + "/" + (object) this.targetTech.costsByResearchTypeID[keyValuePair.Key];
      }
      else
      {
        TechInstance orAdd = Research.Instance.GetOrAdd(this.targetTech);
        if (orAdd != null)
        {
          child.GetComponentInChildren<LocText>().text = ((double) orAdd.progressInventory.PointsByTypeID[keyValuePair.Key]).ToString() + "/" + (object) this.targetTech.costsByResearchTypeID[keyValuePair.Key];
          num = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key] / this.targetTech.costsByResearchTypeID[keyValuePair.Key];
        }
        else
          continue;
      }
      child.GetComponentsInChildren<Image>()[2].fillAmount = num;
      child.GetComponent<ToolTip>().SetSimpleTooltip(Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).description);
    }
  }

  private GameObject GetFreeIcon()
  {
    return Util.KInstantiateUI(this.iconPrefab, this.iconPanel, false);
  }

  private Image GetFreeLine()
  {
    return Util.KInstantiateUI<Image>(this.linePrefab.gameObject, this.gameObject, false);
  }

  public void ResearchCompleted(bool notify = true)
  {
    this.BG.color = this.completedColor;
    this.titleBG.color = this.completedHeaderColor;
    this.defaultColor = this.completedColor;
    if (notify)
    {
      this.unlockedTechMetric[ResearchEntry.UnlockedTechKey] = (object) this.targetTech.Id;
      ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedTechMetric);
    }
    this.toggle.ClearOnClick();
    if (!notify)
      return;
    ResearchCompleteMessage researchCompleteMessage = new ResearchCompleteMessage(this.targetTech);
    MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
    Messenger.Instance.QueueMessage((Message) researchCompleteMessage);
  }
}
