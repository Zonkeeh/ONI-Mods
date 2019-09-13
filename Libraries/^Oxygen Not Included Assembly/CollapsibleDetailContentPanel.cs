// Decompiled with JetBrains decompiler
// Type: CollapsibleDetailContentPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollapsibleDetailContentPanel : KMonoBehaviour
{
  public ImageToggleState ArrowIcon;
  public LocText HeaderLabel;
  public UnityEngine.UI.Button CollapseButton;
  public Transform Content;
  public ScalerMask scalerMask;
  [Space(10f)]
  public DetailLabel labelTemplate;
  public DetailLabelWithButton labelWithActionButtonTemplate;
  private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;
  private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;
  private LoggerFSS log;
  public CollapsibleDetailContentPanel.PanelColors colors;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.CollapseButton.onClick.AddListener(new UnityAction(this.ToggleOpen));
    this.SetColors(this.colors);
    this.ArrowIcon.SetActive();
    this.log = new LoggerFSS("detailpanel", 35);
    this.labels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>>();
    this.buttonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>>();
    this.Commit();
  }

  public void SetTitle(string title)
  {
    this.HeaderLabel.text = title;
  }

  public void Commit()
  {
    int num = 0;
    foreach (CollapsibleDetailContentPanel.Label<DetailLabel> label in this.labels.Values)
    {
      if (label.used)
      {
        ++num;
        if (!label.obj.gameObject.activeSelf)
          label.obj.gameObject.SetActive(true);
      }
      else if (!label.used && label.obj.gameObject.activeSelf)
        label.obj.gameObject.SetActive(false);
      label.used = false;
    }
    foreach (CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label in this.buttonLabels.Values)
    {
      if (label.used)
      {
        ++num;
        if (!label.obj.gameObject.activeSelf)
          label.obj.gameObject.SetActive(true);
      }
      else if (!label.used && label.obj.gameObject.activeSelf)
        label.obj.gameObject.SetActive(false);
      label.used = false;
    }
    if (this.gameObject.activeSelf && num == 0)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      if (this.gameObject.activeSelf || num <= 0)
        return;
      this.gameObject.SetActive(true);
    }
  }

  public void SetLabel(string id, string text, string tooltip)
  {
    CollapsibleDetailContentPanel.Label<DetailLabel> label;
    if (!this.labels.TryGetValue(id, out label))
    {
      label = new CollapsibleDetailContentPanel.Label<DetailLabel>()
      {
        used = true,
        obj = Util.KInstantiateUI(this.labelTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabel>()
      };
      label.obj.gameObject.name = id;
      this.labels[id] = label;
    }
    label.obj.label.AllowLinks = true;
    label.obj.label.text = text;
    label.obj.toolTip.toolTip = tooltip;
    label.used = true;
  }

  public void SetLabelWithButton(
    string id,
    string text,
    string tooltip,
    string buttonText,
    string buttonTooltip,
    System.Action buttonCb)
  {
    CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label;
    if (!this.buttonLabels.TryGetValue(id, out label))
    {
      label = new CollapsibleDetailContentPanel.Label<DetailLabelWithButton>()
      {
        used = true,
        obj = Util.KInstantiateUI(this.labelWithActionButtonTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabelWithButton>()
      };
      label.obj.gameObject.name = id;
      this.buttonLabels[id] = label;
    }
    label.obj.label.AllowLinks = true;
    label.obj.label.text = text;
    label.obj.toolTip.toolTip = tooltip;
    label.obj.buttonLabel.text = buttonText;
    label.obj.buttonToolTip.toolTip = buttonTooltip;
    label.obj.button.ClearOnClick();
    label.obj.button.onClick += buttonCb;
    label.used = true;
  }

  public void SetColors(
    CollapsibleDetailContentPanel.PanelColors newColors)
  {
    this.colors = newColors;
    this.HeaderLabel.color = this.colors.TextColor;
    this.ArrowIcon.ActiveColour = this.colors.ArrowColor;
    this.ArrowIcon.InactiveColour = this.colors.ArrowColor;
    this.CollapseButton.transition = Selectable.Transition.None;
    ColorBlock colorBlock = new ColorBlock()
    {
      normalColor = new Color(this.colors.FrameColor.r, this.colors.FrameColor.g, this.colors.FrameColor.b, this.colors.FrameColor.a),
      highlightedColor = this.colors.FrameColor_Hover,
      pressedColor = this.colors.FrameColor_Press
    };
    colorBlock.disabledColor = colorBlock.normalColor;
    colorBlock.colorMultiplier = 1f;
    this.CollapseButton.colors = colorBlock;
    this.CollapseButton.transition = Selectable.Transition.ColorTint;
  }

  private void ToggleOpen()
  {
    bool flag = !this.scalerMask.gameObject.activeSelf;
    this.scalerMask.gameObject.SetActive(flag);
    if (flag)
    {
      this.ArrowIcon.SetActive();
      this.ForceLocTextsMeshRebuild();
    }
    else
      this.ArrowIcon.SetInactive();
  }

  public void SetCollapsible(bool bCollapsible)
  {
    this.ArrowIcon.gameObject.SetActive(bCollapsible);
    this.CollapseButton.interactable = bCollapsible;
  }

  public void ForceLocTextsMeshRebuild()
  {
    foreach (TMP_Text componentsInChild in this.GetComponentsInChildren<LocText>())
      componentsInChild.ForceMeshUpdate();
  }

  private class Label<T>
  {
    public T obj;
    public bool used;
  }

  [Serializable]
  public struct PanelColors
  {
    public Color FrameColor;
    public Color FrameColor_Hover;
    public Color FrameColor_Press;
    public Color ArrowColor;
    public Color TextColor;
  }
}
