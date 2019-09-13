// Decompiled with JetBrains decompiler
// Type: PatchNotesScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PatchNotesScreen : KModalScreen
{
  private static int PatchNotesVersion = 9;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton okButton;
  [SerializeField]
  private KButton fullPatchNotes;
  [SerializeField]
  private KButton previousVersion;
  [SerializeField]
  private LocText changesLabel;
  private string m_patchNotesUrl;
  private string m_patchNotesText;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.changesLabel.text = this.m_patchNotesText;
    this.closeButton.onClick += new System.Action(this.MarkAsReadAndClose);
    this.closeButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = "HUD_Click_Close";
    this.okButton.onClick += new System.Action(this.MarkAsReadAndClose);
    this.previousVersion.onClick += (System.Action) (() => Application.OpenURL("http://support.kleientertainment.com/customer/portal/articles/2776550"));
    this.fullPatchNotes.onClick += new System.Action(this.OnPatchNotesClick);
  }

  public static bool ShouldShowScreen()
  {
    return KPlayerPrefs.GetInt("PatchNotesVersion") < PatchNotesScreen.PatchNotesVersion;
  }

  private void MarkAsReadAndClose()
  {
    KPlayerPrefs.SetInt("PatchNotesVersion", PatchNotesScreen.PatchNotesVersion);
    this.gameObject.SetActive(false);
  }

  public void UpdatePatchNotes(string patchNotesSummary, string url)
  {
    this.m_patchNotesUrl = url;
    this.m_patchNotesText = patchNotesSummary;
    this.changesLabel.text = this.m_patchNotesText;
  }

  private void OnPatchNotesClick()
  {
    Application.OpenURL(this.m_patchNotesUrl);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.MarkAsReadAndClose();
    else
      base.OnKeyDown(e);
  }
}
