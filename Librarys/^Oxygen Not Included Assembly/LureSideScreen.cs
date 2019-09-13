// Decompiled with JetBrains decompiler
// Type: LureSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LureSideScreen : SideScreenContent
{
  public Dictionary<Tag, MultiToggle> toggles_by_tag = new Dictionary<Tag, MultiToggle>();
  private Dictionary<Tag, string> baitAttractionStrings = new Dictionary<Tag, string>()
  {
    {
      GameTags.SlimeMold,
      (string) CREATURES.SPECIES.PUFT.NAME
    },
    {
      GameTags.Phosphorite,
      (string) CREATURES.SPECIES.LIGHTBUG.NAME
    }
  };
  protected CreatureLure target_lure;
  public GameObject prefab_toggle;
  public GameObject toggle_container;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<CreatureLure>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.target_lure = target.GetComponent<CreatureLure>();
    foreach (Tag baitType in this.target_lure.baitTypes)
    {
      Tag bait = baitType;
      LureSideScreen lureSideScreen = this;
      Tag key = bait;
      if (!this.toggles_by_tag.ContainsKey(bait))
      {
        GameObject gameObject = Util.KInstantiateUI(this.prefab_toggle, this.toggle_container, true);
        Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("FGImage");
        gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").text = ElementLoader.GetElement(bait).name;
        reference.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(bait).substance.anim, "ui", false, string.Empty);
        MultiToggle component = gameObject.GetComponent<MultiToggle>();
        this.toggles_by_tag.Add(key, component);
      }
      this.toggles_by_tag[bait].onClick = (System.Action) (() => lureSideScreen.SelectToggle(bait));
    }
    this.RefreshToggles();
  }

  public void SelectToggle(Tag tag)
  {
    if (this.target_lure.activeBaitSetting != tag)
      this.target_lure.ChangeBaitSetting(tag);
    else
      this.target_lure.ChangeBaitSetting(Tag.Invalid);
    this.RefreshToggles();
  }

  private void RefreshToggles()
  {
    foreach (KeyValuePair<Tag, MultiToggle> keyValuePair in this.toggles_by_tag)
    {
      if (this.target_lure.activeBaitSetting == keyValuePair.Key)
        keyValuePair.Value.ChangeState(2);
      else
        keyValuePair.Value.ChangeState(1);
      keyValuePair.Value.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.UISIDESCREENS.LURE.ATTRACTS, (object) ElementLoader.GetElement(keyValuePair.Key).name, (object) this.baitAttractionStrings[keyValuePair.Key]));
    }
  }
}
