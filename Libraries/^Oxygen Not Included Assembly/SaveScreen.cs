// Decompiled with JetBrains decompiler
// Type: SaveScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;
using UnityEngine;

public class SaveScreen : KModalScreen
{
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton newSaveButton;
  [SerializeField]
  private KButton oldSaveButtonPrefab;
  [SerializeField]
  private Transform oldSavesRoot;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.oldSaveButtonPrefab.gameObject.SetActive(false);
    this.newSaveButton.onClick += new System.Action(this.OnClickNewSave);
    this.closeButton.onClick += new System.Action(((KScreen) this).Deactivate);
  }

  protected override void OnCmpEnable()
  {
    foreach (string allFile in SaveLoader.GetAllFiles())
      this.AddExistingSaveFile(allFile);
    SpeedControlScreen.Instance.Pause(true);
  }

  protected override void OnDeactivate()
  {
    SpeedControlScreen.Instance.Unpause(true);
    base.OnDeactivate();
  }

  private void AddExistingSaveFile(string filename)
  {
    KButton kbutton = Util.KInstantiateUI<KButton>(this.oldSaveButtonPrefab.gameObject, this.oldSavesRoot.gameObject, true);
    HierarchyReferences component1 = kbutton.GetComponent<HierarchyReferences>();
    LocText component2 = component1.GetReference<RectTransform>("Title").GetComponent<LocText>();
    LocText component3 = component1.GetReference<RectTransform>("Date").GetComponent<LocText>();
    System.DateTime lastWriteTime = File.GetLastWriteTime(filename);
    component2.text = string.Format("{0}", (object) System.IO.Path.GetFileNameWithoutExtension(filename));
    component3.text = string.Format("{0:H:mm:ss}" + Localization.GetFileDateFormat(0), (object) lastWriteTime);
    kbutton.onClick += (System.Action) (() => this.Save(filename));
  }

  public static string GetValidSaveFilename(string filename)
  {
    string str = ".sav";
    if (System.IO.Path.GetExtension(filename).ToLower() != str)
      filename += str;
    return filename;
  }

  public void Save(string filename)
  {
    filename = SaveScreen.GetValidSaveFilename(filename);
    if (File.Exists(filename))
      ScreenPrefabs.Instance.ConfirmDoAction(string.Format((string) UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, (object) System.IO.Path.GetFileNameWithoutExtension(filename)), (System.Action) (() => this.DoSave(filename)), this.transform.parent);
    else
      this.DoSave(filename);
  }

  private void DoSave(string filename)
  {
    ReportErrorDialog.MOST_RECENT_SAVEFILE = filename;
    try
    {
      SaveLoader.Instance.Save(filename, false, true);
      this.Deactivate();
    }
    catch (IOException ex)
    {
      SaveScreen saveScreen = this;
      Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format((string) UI.FRONTEND.SAVESCREEN.IO_ERROR, (object) ex.ToString()), (System.Action) (() => saveScreen.Deactivate()), (System.Action) null, (string) UI.FRONTEND.SAVESCREEN.REPORT_BUG, (System.Action) (() => KCrashReporter.ReportError(ex.Message, ex.StackTrace.ToString(), (string) null, (ConfirmDialogScreen) null, string.Empty)), (string) null, (string) null, (string) null, (Sprite) null, true);
    }
  }

  public void OnClickNewSave()
  {
    ((FileNameDialog) KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.FileNameDialog.gameObject, this.transform.parent.gameObject)).onConfirm = (System.Action<string>) (filename =>
    {
      filename = System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), filename);
      this.Save(filename);
    });
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.Deactivate();
    e.Consumed = true;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    e.Consumed = true;
  }
}
