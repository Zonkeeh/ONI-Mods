// Decompiled with JetBrains decompiler
// Type: ResearchSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchSideScreen : SideScreenContent
{
  public KButton selectResearchButton;
  public Image researchButtonIcon;
  public GameObject content;
  private GameObject target;
  private System.Action<object> refreshDisplayStateDelegate;
  public LocText DescriptionText;

  public ResearchSideScreen()
  {
    this.refreshDisplayStateDelegate = new System.Action<object>(this.RefreshDisplayState);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.selectResearchButton.onClick += (System.Action) (() => ManagementMenu.Instance.ToggleResearch());
    Research.Instance.Subscribe(-1914338957, this.refreshDisplayStateDelegate);
    Research.Instance.Subscribe(-125623018, this.refreshDisplayStateDelegate);
    this.RefreshDisplayState((object) null);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RefreshDisplayState((object) null);
    this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
    this.target.gameObject.Subscribe(-1852328367, this.refreshDisplayStateDelegate);
    this.target.gameObject.Subscribe(-592767678, this.refreshDisplayStateDelegate);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (!(bool) ((UnityEngine.Object) this.target))
      return;
    this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
    this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
    this.target = (GameObject) null;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Research.Instance.Unsubscribe(-1914338957, this.refreshDisplayStateDelegate);
    Research.Instance.Unsubscribe(-125623018, this.refreshDisplayStateDelegate);
    if (!(bool) ((UnityEngine.Object) this.target))
      return;
    this.target.gameObject.Unsubscribe(-1852328367, this.refreshDisplayStateDelegate);
    this.target.gameObject.Unsubscribe(187661686, this.refreshDisplayStateDelegate);
    this.target = (GameObject) null;
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<ResearchCenter>() != (UnityEngine.Object) null;
  }

  private void RefreshDisplayState(object data = null)
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null)
      return;
    ResearchCenter component = SelectTool.Instance.selected.GetComponent<ResearchCenter>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.researchButtonIcon.sprite = Research.Instance.researchTypes.GetResearchType(component.research_point_type_id).sprite;
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch == null)
    {
      this.DescriptionText.text = "<b>" + (string) STRINGS.UI.UISIDESCREENS.RESEARCHSIDESCREEN.NOSELECTEDRESEARCH + "</b>";
    }
    else
    {
      string empty = string.Empty;
      if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(component.research_point_type_id) || (double) activeResearch.tech.costsByResearchTypeID[component.research_point_type_id] <= 0.0)
        empty += "<color=#7f7f7f>";
      string str = empty + "<b>" + activeResearch.tech.Name + "</b>";
      if (!activeResearch.tech.costsByResearchTypeID.ContainsKey(component.research_point_type_id) || (double) activeResearch.tech.costsByResearchTypeID[component.research_point_type_id] <= 0.0)
        str += "</color>";
      foreach (KeyValuePair<string, float> keyValuePair in activeResearch.tech.costsByResearchTypeID)
      {
        if ((double) keyValuePair.Value != 0.0)
        {
          bool flag = keyValuePair.Key == component.research_point_type_id;
          str += "\n   ";
          str += "<b>";
          if (!flag)
            str += "<color=#7f7f7f>";
          str = str + "- " + Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).name + ": " + (object) activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key] + "/" + (object) activeResearch.tech.costsByResearchTypeID[keyValuePair.Key];
          if (!flag)
            str += "</color>";
          str += "</b>";
        }
      }
      this.DescriptionText.text = str;
    }
  }
}
