// Decompiled with JetBrains decompiler
// Type: MetricsOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

public class MetricsOptionsScreen : KModalScreen
{
  public LocText title;
  public KButton dismissButton;
  public KButton closeButton;
  public GameObject enableButton;
  public UnityEngine.UI.Button descriptionButton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.title.SetText((string) STRINGS.UI.FRONTEND.METRICS_OPTIONS_SCREEN.TITLE);
    GameObject gameObject = this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").gameObject;
    gameObject.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.METRICS_OPTIONS_SCREEN.TOOLTIP);
    gameObject.transform.GetChild(0).gameObject.SetActive(!KPrivacyPrefs.instance.disableDataCollection);
    gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OnClickToggle());
    this.enableButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").SetText((string) STRINGS.UI.FRONTEND.METRICS_OPTIONS_SCREEN.ENABLE_BUTTON);
    this.dismissButton.onClick += (System.Action) (() => this.Deactivate());
    this.closeButton.onClick += (System.Action) (() => this.Deactivate());
    this.descriptionButton.onClick.AddListener((UnityAction) (() => Application.OpenURL("https://www.kleientertainment.com/privacy-policy")));
  }

  private void OnClickToggle()
  {
    KPrivacyPrefs.instance.disableDataCollection = !KPrivacyPrefs.instance.disableDataCollection;
    KPrivacyPrefs.Save();
    ThreadedHttps<KleiMetrics>.Instance.SetEnabled(!KPrivacyPrefs.instance.disableDataCollection);
    this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(ThreadedHttps<KleiMetrics>.Instance.enabled);
  }
}
