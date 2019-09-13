// Decompiled with JetBrains decompiler
// Type: ModeSelectScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ModeSelectScreen : NewGameFlowScreen
{
  [SerializeField]
  private MultiToggle nosweatButton;
  private Image nosweatButtonHeader;
  private Image nosweatButtonSelectionFrame;
  [SerializeField]
  private MultiToggle survivalButton;
  private Image survivalButtonHeader;
  private Image survivalButtonSelectionFrame;
  [SerializeField]
  private LocText descriptionArea;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KBatchedAnimController nosweatAnim;
  [SerializeField]
  private KBatchedAnimController survivalAnim;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    HierarchyReferences component1 = this.survivalButton.GetComponent<HierarchyReferences>();
    this.survivalButtonHeader = component1.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
    this.survivalButtonSelectionFrame = component1.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
    this.survivalButton.onEnter += new System.Action(this.OnHoverEnterSurvival);
    this.survivalButton.onExit += new System.Action(this.OnHoverExitSurvival);
    this.survivalButton.onClick += new System.Action(this.OnClickSurvival);
    HierarchyReferences component2 = this.nosweatButton.GetComponent<HierarchyReferences>();
    this.nosweatButtonHeader = component2.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
    this.nosweatButtonSelectionFrame = component2.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
    this.nosweatButton.onEnter += new System.Action(this.OnHoverEnterNosweat);
    this.nosweatButton.onExit += new System.Action(this.OnHoverExitNosweat);
    this.nosweatButton.onClick += new System.Action(this.OnClickNosweat);
    this.closeButton.onClick += new System.Action(((NewGameFlowScreen) this).NavigateBackward);
    this.SetAnimScale();
  }

  private void OnHoverEnterSurvival()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
    this.survivalButtonSelectionFrame.SetAlpha(1f);
    this.survivalButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.5333334f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.SURVIVAL_DESC;
  }

  private void OnHoverExitSurvival()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
    this.survivalButtonSelectionFrame.SetAlpha(0.0f);
    this.survivalButtonHeader.color = new Color(0.3098039f, 0.3411765f, 0.3843137f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
  }

  private void OnClickSurvival()
  {
    this.Deactivate();
    CustomGameSettings.Instance.SetSurvivalDefaults();
    this.NavigateForward();
  }

  private void OnHoverEnterNosweat()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
    this.nosweatButtonSelectionFrame.SetAlpha(1f);
    this.nosweatButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.5333334f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.NOSWEAT_DESC;
  }

  private void OnHoverExitNosweat()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
    this.nosweatButtonSelectionFrame.SetAlpha(0.0f);
    this.nosweatButtonHeader.color = new Color(0.3098039f, 0.3411765f, 0.3843137f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
  }

  private void OnClickNosweat()
  {
    this.Deactivate();
    CustomGameSettings.Instance.SetNosweatDefaults();
    this.NavigateForward();
  }

  private void SetAnimScale()
  {
    float canvasScale = this.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
    if ((UnityEngine.Object) this.nosweatAnim != (UnityEngine.Object) null)
      this.nosweatAnim.animScale *= 1f / canvasScale;
    if (!((UnityEngine.Object) this.survivalAnim != (UnityEngine.Object) null))
      return;
    this.survivalAnim.animScale *= 1f / canvasScale;
  }
}
