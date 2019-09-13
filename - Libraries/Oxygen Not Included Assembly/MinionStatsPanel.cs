// Decompiled with JetBrains decompiler
// Type: MinionStatsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MinionStatsPanel : TargetScreen
{
  public GameObject attributesLabelTemplate;
  private GameObject resumePanel;
  private GameObject attributesPanel;
  private DetailsPanelDrawer resumeDrawer;
  private DetailsPanelDrawer attributesDrawer;
  private SchedulerHandle updateHandle;

  public override bool IsValidForTarget(GameObject target)
  {
    return (bool) ((UnityEngine.Object) target.GetComponent<MinionIdentity>());
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.resumePanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.attributesPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.resumeDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.resumePanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
    this.attributesDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.attributesPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
  }

  protected override void OnCleanUp()
  {
    this.updateHandle.ClearScheduler();
    base.OnCleanUp();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Refresh();
    this.ScheduleUpdate();
  }

  public override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    this.Refresh();
  }

  private void ScheduleUpdate()
  {
    this.updateHandle = UIScheduler.Instance.Schedule("RefreshMinionStatsPanel", 1f, (System.Action<object>) (o =>
    {
      this.Refresh();
      this.ScheduleUpdate();
    }), (object) null, (SchedulerGroup) null);
  }

  private GameObject AddOrGetLabel(
    Dictionary<string, GameObject> labels,
    GameObject panel,
    string id)
  {
    GameObject gameObject;
    if (labels.ContainsKey(id))
    {
      gameObject = labels[id];
    }
    else
    {
      gameObject = Util.KInstantiate(this.attributesLabelTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject, (string) null);
      gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
      labels[id] = gameObject;
    }
    gameObject.SetActive(true);
    return gameObject;
  }

  private void Refresh()
  {
    if (!this.gameObject.activeSelf || (UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null || (UnityEngine.Object) this.selectedTarget.GetComponent<MinionIdentity>() == (UnityEngine.Object) null)
      return;
    this.RefreshResume();
    this.RefreshAttributes();
  }

  private void RefreshAttributes()
  {
    if (!(bool) ((UnityEngine.Object) this.selectedTarget.GetComponent<MinionIdentity>()))
    {
      this.attributesPanel.SetActive(false);
    }
    else
    {
      this.attributesPanel.SetActive(true);
      this.attributesPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.STATS.GROUPNAME_ATTRIBUTES;
      List<AttributeInstance> all = new List<AttributeInstance>((IEnumerable<AttributeInstance>) this.selectedTarget.GetAttributes().AttributeTable).FindAll((Predicate<AttributeInstance>) (a => a.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill));
      this.attributesDrawer.BeginDrawing();
      if (all.Count > 0)
      {
        foreach (AttributeInstance attributeInstance in all)
          this.attributesDrawer.NewLabel(string.Format("{0}: {1}", (object) attributeInstance.Name, (object) attributeInstance.GetFormattedValue())).Tooltip(attributeInstance.GetAttributeValueTooltip());
      }
      this.attributesDrawer.EndDrawing();
    }
  }

  private void RefreshResume()
  {
    MinionResume component = this.selectedTarget.GetComponent<MinionResume>();
    if (!(bool) ((UnityEngine.Object) component))
    {
      this.resumePanel.SetActive(false);
    }
    else
    {
      this.resumePanel.SetActive(true);
      this.resumePanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = string.Format((string) UI.DETAILTABS.PERSONALITY.GROUPNAME_RESUME, (object) this.selectedTarget.name.ToUpper());
      this.resumeDrawer.BeginDrawing();
      List<Skill> skillList = new List<Skill>();
      foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
      {
        if (keyValuePair.Value)
        {
          Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
          skillList.Add(skill);
        }
      }
      this.resumeDrawer.NewLabel((string) UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS).Tooltip((string) UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS_TOOLTIP);
      if (skillList.Count == 0)
      {
        this.resumeDrawer.NewLabel("  • " + (string) UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.NAME).Tooltip(string.Format((string) UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.TOOLTIP, (object) this.selectedTarget.name));
      }
      else
      {
        foreach (Skill skill in skillList)
        {
          string str = string.Empty;
          foreach (SkillPerk perk in skill.perks)
            str = str + "  • " + perk.Name + "\n";
          this.resumeDrawer.NewLabel("  • " + skill.Name).Tooltip(skill.description + "\n" + str);
        }
      }
      this.resumeDrawer.EndDrawing();
    }
  }
}
