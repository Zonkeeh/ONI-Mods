// Decompiled with JetBrains decompiler
// Type: TreeFilterableSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class TreeFilterableSideScreenRow : KMonoBehaviour
{
  private List<Tag> subTags = new List<Tag>();
  private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();
  public bool visualDirty;
  [SerializeField]
  private LocText elementName;
  [SerializeField]
  private GameObject elementGroup;
  [SerializeField]
  private MultiToggle checkBoxToggle;
  [SerializeField]
  private KToggle arrowToggle;
  [SerializeField]
  private KImage bgImg;
  private TreeFilterableSideScreen parent;

  public TreeFilterableSideScreen Parent
  {
    get
    {
      return this.parent;
    }
    set
    {
      this.parent = value;
    }
  }

  public TreeFilterableSideScreenRow.State GetState()
  {
    bool flag1 = false;
    bool flag2 = false;
    foreach (TreeFilterableSideScreenElement rowElement in this.rowElements)
    {
      if (this.parent.GetElementTagAcceptedState(rowElement.GetElementTag()))
        flag1 = true;
      else
        flag2 = true;
    }
    if (flag1 && !flag2)
      return TreeFilterableSideScreenRow.State.On;
    if (!flag1 && flag2)
      return TreeFilterableSideScreenRow.State.Off;
    if (flag1 && flag2)
      return TreeFilterableSideScreenRow.State.Mixed;
    return this.rowElements.Count > 0 ? TreeFilterableSideScreenRow.State.On : TreeFilterableSideScreenRow.State.Off;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.checkBoxToggle.onClick += (System.Action) (() =>
    {
      switch (this.GetState())
      {
        case TreeFilterableSideScreenRow.State.Off:
        case TreeFilterableSideScreenRow.State.Mixed:
          this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
          break;
        case TreeFilterableSideScreenRow.State.On:
          this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
          break;
      }
    });
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.SetArrowToggleState(false);
  }

  protected override void OnCmpDisable()
  {
    this.SetArrowToggleState(false);
    this.rowElements.ForEach((System.Action<TreeFilterableSideScreenElement>) (row => row.OnSelectionChanged -= new System.Action<Tag, bool>(this.OnElementSelectionChanged)));
    base.OnCmpDisable();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.arrowToggle.onClick -= new System.Action(this.ArrowToggleClicked);
  }

  public void UpdateCheckBoxVisualState()
  {
    this.checkBoxToggle.ChangeState((int) this.GetState());
    this.visualDirty = false;
  }

  public void ChangeCheckBoxState(TreeFilterableSideScreenRow.State newState)
  {
    switch (newState)
    {
      case TreeFilterableSideScreenRow.State.Off:
        this.rowElements.ForEach((System.Action<TreeFilterableSideScreenElement>) (re => re.SetCheckBox(false)));
        break;
      case TreeFilterableSideScreenRow.State.On:
        this.rowElements.ForEach((System.Action<TreeFilterableSideScreenElement>) (re => re.SetCheckBox(true)));
        break;
    }
    this.visualDirty = true;
  }

  private void ArrowToggleClicked()
  {
    this.UpdateArrowToggleState();
  }

  private void SetArrowToggleState(bool state)
  {
    this.arrowToggle.isOn = state;
    this.UpdateArrowToggleState();
  }

  private void UpdateArrowToggleState()
  {
    bool isOn = this.arrowToggle.isOn;
    this.arrowToggle.GetComponent<ImageToggleState>().SetActiveState(isOn);
    this.elementGroup.SetActive(isOn);
    this.bgImg.enabled = isOn;
  }

  private void ArrowToggleDisabledClick()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
  }

  private void OnElementSelectionChanged(Tag t, bool state)
  {
    if (state)
      this.parent.AddTag(t);
    else
      this.parent.RemoveTag(t);
    this.visualDirty = true;
  }

  public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
  {
    this.subTags.Clear();
    this.rowElements.Clear();
    this.elementName.text = mainElementTag.ProperName();
    this.arrowToggle.ClearOnClick();
    this.bgImg.enabled = false;
    this.checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, (object) mainElementTag.ProperName()));
    if (filterMap.Count == 0)
    {
      if (this.elementGroup.activeInHierarchy)
        this.elementGroup.SetActive(false);
      this.arrowToggle.interactable = false;
      this.arrowToggle.onClick += new System.Action(this.ArrowToggleDisabledClick);
      this.arrowToggle.GetComponent<ImageToggleState>().SetDisabled();
    }
    else
    {
      this.arrowToggle.interactable = true;
      this.arrowToggle.onClick += new System.Action(this.ArrowToggleClicked);
      this.arrowToggle.GetComponent<ImageToggleState>().SetActiveState(false);
      foreach (KeyValuePair<Tag, bool> filter in filterMap)
      {
        TreeFilterableSideScreenElement freeElement = this.parent.elementPool.GetFreeElement(this.elementGroup, true);
        freeElement.Parent = this.parent;
        freeElement.SetTag(filter.Key);
        freeElement.SetCheckBox(filter.Value);
        freeElement.OnSelectionChanged += new System.Action<Tag, bool>(this.OnElementSelectionChanged);
        freeElement.SetCheckBox(this.parent.IsTagAllowed(filter.Key));
        this.rowElements.Add(freeElement);
        this.subTags.Add(filter.Key);
      }
    }
    this.UpdateCheckBoxVisualState();
  }

  public enum State
  {
    Off,
    Mixed,
    On,
  }
}
