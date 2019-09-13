// Decompiled with JetBrains decompiler
// Type: MinionPersonalityPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MinionPersonalityPanel : TargetScreen
{
  public GameObject attributesLabelTemplate;
  private GameObject bioPanel;
  private GameObject traitsPanel;
  private DetailsPanelDrawer bioDrawer;
  private DetailsPanelDrawer traitsDrawer;
  public MinionEquipmentPanel panel;
  private SchedulerHandle updateHandle;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<MinionIdentity>() != (UnityEngine.Object) null;
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
  }

  public override void OnSelectTarget(GameObject target)
  {
    this.panel.SetSelectedMinion(target);
    this.panel.Refresh((object) null);
    base.OnSelectTarget(target);
    this.Refresh();
  }

  public override void OnDeselectTarget(GameObject target)
  {
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    if (!((UnityEngine.Object) this.panel == (UnityEngine.Object) null))
      return;
    this.panel = this.GetComponent<MinionEquipmentPanel>();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.bioPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.traitsPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.bioDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.bioPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
    this.traitsDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.traitsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
  }

  protected override void OnCleanUp()
  {
    this.updateHandle.ClearScheduler();
    base.OnCleanUp();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.panel == (UnityEngine.Object) null)
      this.panel = this.GetComponent<MinionEquipmentPanel>();
    this.Refresh();
    this.ScheduleUpdate();
  }

  private void ScheduleUpdate()
  {
    this.updateHandle = UIScheduler.Instance.Schedule("RefreshMinionPersonalityPanel", 1f, (System.Action<object>) (o =>
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
    this.RefreshBio();
    this.RefreshTraits();
  }

  private void RefreshBio()
  {
    MinionIdentity component1 = this.selectedTarget.GetComponent<MinionIdentity>();
    if (!(bool) ((UnityEngine.Object) component1))
    {
      this.bioPanel.SetActive(false);
    }
    else
    {
      this.bioPanel.SetActive(true);
      this.bioPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.PERSONALITY.GROUPNAME_BIO;
      this.bioDrawer.BeginDrawing().NewLabel((string) DUPLICANTS.NAMETITLE + component1.name).NewLabel((string) DUPLICANTS.ARRIVALTIME + (object) (float) ((double) GameClock.Instance.GetCycle() - (double) component1.arrivalTime) + " Cycles").Tooltip(string.Format((string) DUPLICANTS.ARRIVALTIME_TOOLTIP, (object) component1.arrivalTime, (object) component1.name)).NewLabel((string) DUPLICANTS.GENDERTITLE + string.Format((string) Strings.Get(string.Format("STRINGS.DUPLICANTS.GENDER.{0}.NAME", (object) component1.genderStringKey.ToUpper())), (object) component1.gender)).NewLabel(string.Format((string) Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", (object) component1.nameStringKey.ToUpper())), (object) component1.name)).Tooltip(string.Format((string) Strings.Get(string.Format("STRINGS.DUPLICANTS.DESC_TOOLTIP", (object) component1.nameStringKey.ToUpper())), (object) component1.name));
      MinionResume component2 = this.selectedTarget.GetComponent<MinionResume>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AptitudeBySkillGroup.Count > 0)
      {
        this.bioDrawer.NewLabel((string) UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.NAME + "\n").Tooltip(string.Format((string) UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.TOOLTIP, (object) this.selectedTarget.name));
        foreach (KeyValuePair<HashedString, float> keyValuePair in component2.AptitudeBySkillGroup)
        {
          if ((double) keyValuePair.Value != 0.0)
          {
            SkillGroup skillGroup = Db.Get().SkillGroups.Get(keyValuePair.Key);
            this.bioDrawer.NewLabel("  • " + skillGroup.Name).Tooltip(string.Format((string) DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, (object) skillGroup.Name, (object) keyValuePair.Value));
          }
        }
      }
      this.bioDrawer.EndDrawing();
    }
  }

  private void RefreshTraits()
  {
    if (!(bool) ((UnityEngine.Object) this.selectedTarget.GetComponent<MinionIdentity>()))
    {
      this.traitsPanel.SetActive(false);
    }
    else
    {
      this.traitsPanel.SetActive(true);
      this.traitsPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.STATS.GROUPNAME_TRAITS;
      this.traitsDrawer.BeginDrawing();
      foreach (Trait trait in this.selectedTarget.GetComponent<Traits>().TraitList)
        this.traitsDrawer.NewLabel(trait.Name).Tooltip(trait.GetTooltip());
      this.traitsDrawer.EndDrawing();
    }
  }
}
