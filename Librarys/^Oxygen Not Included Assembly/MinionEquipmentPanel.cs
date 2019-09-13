// Decompiled with JetBrains decompiler
// Type: MinionEquipmentPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MinionEquipmentPanel : KMonoBehaviour
{
  private Dictionary<string, GameObject> labels = new Dictionary<string, GameObject>();
  public GameObject SelectedMinion;
  public GameObject labelTemplate;
  private GameObject roomPanel;
  private GameObject ownablePanel;
  private Storage storage;
  private System.Action<object> refreshDelegate;

  public MinionEquipmentPanel()
  {
    this.refreshDelegate = new System.Action<object>(this.Refresh);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.roomPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.roomPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_ROOMS;
    this.roomPanel.SetActive(true);
    this.ownablePanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject, false);
    this.ownablePanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_OWNABLE;
    this.ownablePanel.SetActive(true);
  }

  public void SetSelectedMinion(GameObject minion)
  {
    if ((UnityEngine.Object) this.SelectedMinion != (UnityEngine.Object) null)
    {
      this.SelectedMinion.Unsubscribe(-448952673, this.refreshDelegate);
      this.SelectedMinion.Unsubscribe(-1285462312, this.refreshDelegate);
      this.SelectedMinion.Unsubscribe(-1585839766, this.refreshDelegate);
    }
    this.SelectedMinion = minion;
    this.SelectedMinion.Subscribe(-448952673, this.refreshDelegate);
    this.SelectedMinion.Subscribe(-1285462312, this.refreshDelegate);
    this.SelectedMinion.Subscribe(-1585839766, this.refreshDelegate);
    this.Refresh((object) null);
  }

  public void Refresh(object data = null)
  {
    if ((UnityEngine.Object) this.SelectedMinion == (UnityEngine.Object) null)
      return;
    this.Build();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!((UnityEngine.Object) this.SelectedMinion != (UnityEngine.Object) null))
      return;
    this.SelectedMinion.Unsubscribe(-448952673, this.refreshDelegate);
    this.SelectedMinion.Unsubscribe(-1285462312, this.refreshDelegate);
    this.SelectedMinion.Unsubscribe(-1585839766, this.refreshDelegate);
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
      gameObject = Util.KInstantiate(this.labelTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject, (string) null);
      gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
      labels[id] = gameObject;
    }
    gameObject.SetActive(true);
    return gameObject;
  }

  private void Build()
  {
    this.ShowAssignables((Assignables) this.SelectedMinion.GetComponent<MinionIdentity>().GetSoleOwner(), this.roomPanel);
    this.ShowAssignables((Assignables) this.SelectedMinion.GetComponent<MinionIdentity>().GetEquipment(), this.ownablePanel);
  }

  private void ShowAssignables(Assignables assignables, GameObject panel)
  {
    bool flag = false;
    foreach (AssignableSlotInstance slot in assignables.Slots)
    {
      if (slot.slot.showInUI)
      {
        GameObject label = this.AddOrGetLabel(this.labels, panel, slot.slot.Name);
        if (slot.IsAssigned())
        {
          label.SetActive(true);
          flag = true;
          string str = !slot.IsAssigned() ? UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES.text : slot.assignable.GetComponent<KSelectable>().GetName();
          label.GetComponent<LocText>().text = string.Format("{0}: {1}", (object) slot.slot.Name, (object) str);
          label.GetComponent<ToolTip>().toolTip = string.Format((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.ASSIGNED_TOOLTIP, (object) str, (object) this.GetAssignedEffectsString(slot), (object) this.SelectedMinion.name);
        }
        else
        {
          label.SetActive(false);
          label.GetComponent<LocText>().text = (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES;
          label.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES_TOOLTIP;
        }
      }
    }
    if (assignables is Ownables)
    {
      if (!flag)
      {
        GameObject label = this.AddOrGetLabel(this.labels, panel, "NothingAssigned");
        this.labels["NothingAssigned"].SetActive(true);
        label.GetComponent<LocText>().text = (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES;
        label.GetComponent<ToolTip>().toolTip = string.Format((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES_TOOLTIP, (object) this.SelectedMinion.name);
      }
      else if (this.labels.ContainsKey("NothingAssigned"))
        this.labels["NothingAssigned"].SetActive(false);
    }
    if (!(assignables is Equipment))
      return;
    if (!flag)
    {
      GameObject label = this.AddOrGetLabel(this.labels, panel, "NoSuitAssigned");
      this.labels["NoSuitAssigned"].SetActive(true);
      label.GetComponent<LocText>().text = (string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT;
      label.GetComponent<ToolTip>().toolTip = string.Format((string) UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT_TOOLTIP, (object) this.SelectedMinion.name);
    }
    else
    {
      if (!this.labels.ContainsKey("NoSuitAssigned"))
        return;
      this.labels["NoSuitAssigned"].SetActive(false);
    }
  }

  private string GetAssignedEffectsString(AssignableSlotInstance slot)
  {
    string str = string.Empty;
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.AddRange((IEnumerable<Descriptor>) GameUtil.GetGameObjectEffects(slot.assignable.gameObject, false));
    if (descriptorList.Count > 0)
    {
      str += "\n";
      foreach (Descriptor descriptor in descriptorList)
        str = str + "  • " + descriptor.IndentedText() + "\n";
    }
    return str;
  }
}
