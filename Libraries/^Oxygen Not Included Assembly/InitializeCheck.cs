// Decompiled with JetBrains decompiler
// Type: InitializeCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGenGame;
using STRINGS;
using System.IO;
using UnityEngine;

public class InitializeCheck : MonoBehaviour
{
  private static readonly string testFile = "testfile";
  private static readonly string testSave = "testsavefile";
  public Canvas rootCanvasPrefab;
  public ConfirmDialogScreen confirmDialogScreen;
  public Sprite sadDupe;
  private InitializeCheck.SavePathIssue test_issue;

  public static InitializeCheck.SavePathIssue savePathState { get; private set; }

  private void Awake()
  {
    this.CheckForSavePathIssue();
    if (InitializeCheck.savePathState == InitializeCheck.SavePathIssue.Ok && !ReportErrorDialog.hasCrash)
    {
      AudioMixer.Create();
      App.LoadScene("frontend");
    }
    else
    {
      Canvas cmp = this.gameObject.AddComponent<Canvas>();
      cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
      cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);
      Camera camera = this.gameObject.AddComponent<Camera>();
      camera.orthographic = true;
      camera.orthographicSize = 200f;
      camera.backgroundColor = Color.black;
      camera.clearFlags = CameraClearFlags.Color;
      camera.nearClipPlane = 0.0f;
      Debug.Log((object) ("Cannot initialize filesystem. [" + InitializeCheck.savePathState.ToString() + "]"));
      Localization.Initialize(true);
      this.ShowFileErrorDialogs();
    }
  }

  private GameObject CreateUIRoot()
  {
    return Util.KInstantiate((Component) this.rootCanvasPrefab, (GameObject) null, "CanvasRoot");
  }

  private void ShowErrorDialog(string msg)
  {
    ConfirmDialogScreen confirmDialogScreen = Util.KInstantiateUI<ConfirmDialogScreen>(this.confirmDialogScreen.gameObject, this.CreateUIRoot(), true);
    string str = msg;
    System.Action action1 = new System.Action(this.Quit);
    System.Action action2 = (System.Action) null;
    Sprite sadDupe = this.sadDupe;
    string text = str;
    System.Action on_confirm = action1;
    System.Action on_cancel = action2;
    Sprite image_sprite = sadDupe;
    confirmDialogScreen.PopupConfirmDialog(text, on_confirm, on_cancel, (string) null, (System.Action) null, (string) null, (string) null, (string) null, image_sprite, true);
  }

  private void ShowFileErrorDialogs()
  {
    string msg = (string) null;
    switch (InitializeCheck.savePathState)
    {
      case InitializeCheck.SavePathIssue.WriteTestFail:
        msg = string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, (object) SaveLoader.GetSavePrefix());
        break;
      case InitializeCheck.SavePathIssue.SpaceTestFail:
        msg = string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, (object) SaveLoader.GetSavePrefix());
        break;
      case InitializeCheck.SavePathIssue.WorldGenFilesFail:
        msg = string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FILES, (object) (WorldGen.WORLDGEN_SAVE_FILENAME + "\n" + WorldGen.SIM_SAVE_FILENAME));
        break;
    }
    if (msg == null)
      return;
    this.ShowErrorDialog(msg);
  }

  private void CheckForSavePathIssue()
  {
    if (this.test_issue != InitializeCheck.SavePathIssue.Ok)
    {
      InitializeCheck.savePathState = this.test_issue;
    }
    else
    {
      string savePrefix = SaveLoader.GetSavePrefix();
      InitializeCheck.savePathState = InitializeCheck.SavePathIssue.Ok;
      try
      {
        SaveLoader.GetSavePrefixAndCreateFolder();
        using (FileStream fileStream = File.Open(savePrefix + InitializeCheck.testFile, FileMode.Create, FileAccess.Write))
        {
          BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream);
          fileStream.Close();
        }
      }
      catch
      {
        InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WriteTestFail;
        goto label_27;
      }
      using (FileStream fileStream = File.Open(savePrefix + InitializeCheck.testSave, FileMode.Create, FileAccess.Write))
      {
        try
        {
          fileStream.SetLength(15000000L);
          BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream);
          fileStream.Close();
        }
        catch
        {
          fileStream.Close();
          InitializeCheck.savePathState = InitializeCheck.SavePathIssue.SpaceTestFail;
          goto label_27;
        }
      }
      try
      {
        using (File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Append))
          ;
        using (File.Open(WorldGen.SIM_SAVE_FILENAME, FileMode.Append))
          ;
      }
      catch
      {
        InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WorldGenFilesFail;
      }
label_27:
      try
      {
        if (File.Exists(savePrefix + InitializeCheck.testFile))
          File.Delete(savePrefix + InitializeCheck.testFile);
        if (!File.Exists(savePrefix + InitializeCheck.testSave))
          return;
        File.Delete(savePrefix + InitializeCheck.testSave);
      }
      catch
      {
      }
    }
  }

  private void Quit()
  {
    Debug.Log((object) "Quitting...");
    App.Quit();
  }

  public enum SavePathIssue
  {
    Ok,
    WriteTestFail,
    SpaceTestFail,
    WorldGenFilesFail,
  }
}
