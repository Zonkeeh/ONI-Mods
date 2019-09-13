// Decompiled with JetBrains decompiler
// Type: PasteBaseTemplateScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PasteBaseTemplateScreen : KScreen
{
  private List<GameObject> template_buttons = new List<GameObject>();
  public static PasteBaseTemplateScreen Instance;
  public GameObject button_list_container;
  public GameObject prefab_paste_button;
  private List<string> base_template_assets;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    PasteBaseTemplateScreen.Instance = this;
    TemplateCache.Init();
    this.ConsumeMouseScroll = true;
    this.RefreshStampButtons();
  }

  public void RefreshStampButtons()
  {
    foreach (UnityEngine.Object templateButton in this.template_buttons)
      UnityEngine.Object.Destroy(templateButton);
    this.template_buttons.Clear();
    this.base_template_assets = TemplateCache.CollectBaseTemplateNames("bases");
    this.base_template_assets.AddRange((IEnumerable<string>) TemplateCache.CollectBaseTemplateNames("poi"));
    this.base_template_assets.AddRange((IEnumerable<string>) TemplateCache.CollectBaseTemplateNames(string.Empty));
    foreach (string baseTemplateAsset in this.base_template_assets)
    {
      GameObject gameObject = Util.KInstantiateUI(this.prefab_paste_button, this.button_list_container, true);
      KButton component = gameObject.GetComponent<KButton>();
      string template_name = baseTemplateAsset;
      component.onClick += (System.Action) (() => this.OnClickPasteButton(template_name));
      gameObject.GetComponentInChildren<LocText>().text = template_name;
      this.template_buttons.Add(gameObject);
    }
  }

  private void OnClickPasteButton(string template_name)
  {
    if (template_name == null)
      return;
    DebugTool.Instance.DeactivateTool((InterfaceTool) null);
    DebugBaseTemplateButton.Instance.ClearSelection();
    DebugBaseTemplateButton.Instance.nameField.text = template_name;
    TemplateContainer template = TemplateCache.GetTemplate(template_name);
    StampTool.Instance.Activate(template, true, false);
  }
}
