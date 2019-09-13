// Decompiled with JetBrains decompiler
// Type: InspectSaveScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InspectSaveScreen : KModalScreen
{
  private Dictionary<KButton, string> buttonFileMap = new Dictionary<KButton, string>();
  private string currentPath = string.Empty;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton mainSaveBtn;
  [SerializeField]
  private KButton backupBtnPrefab;
  [SerializeField]
  private KButton deleteSaveBtn;
  [SerializeField]
  private GameObject buttonGroup;
  private UIPool<KButton> buttonPool;
  private ConfirmDialogScreen confirmScreen;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.closeButton.onClick += new System.Action(this.CloseScreen);
    this.deleteSaveBtn.onClick += new System.Action(this.DeleteSave);
  }

  private void CloseScreen()
  {
    LoadScreen.Instance.Show(true);
    this.Show(false);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      return;
    this.buttonPool.ClearAll();
    this.buttonFileMap.Clear();
  }

  public void SetTarget(string path)
  {
    if (string.IsNullOrEmpty(path))
    {
      Debug.LogError((object) "The directory path provided is empty.");
      this.Show(false);
    }
    else if (!Directory.Exists(path))
    {
      Debug.LogError((object) "The directory provided does not exist.");
      this.Show(false);
    }
    else
    {
      if (this.buttonPool == null)
        this.buttonPool = new UIPool<KButton>(this.backupBtnPrefab);
      this.currentPath = path;
      IEnumerable<string> source = ((IEnumerable<string>) Directory.GetFiles(path)).Where<string>((Func<string, bool>) (filename => System.IO.Path.GetExtension(filename).ToLower() == ".sav"));
      // ISSUE: reference to a compiler-generated field
      if (InspectSaveScreen.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InspectSaveScreen.\u003C\u003Ef__mg\u0024cache0 = new Func<string, System.DateTime>(File.GetLastWriteTime);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, System.DateTime> fMgCache0 = InspectSaveScreen.\u003C\u003Ef__mg\u0024cache0;
      List<string> list = source.OrderByDescending<string, System.DateTime>(fMgCache0).ToList<string>();
      string str = list[0];
      if (File.Exists(str))
      {
        this.mainSaveBtn.gameObject.SetActive(true);
        this.AddNewSave(this.mainSaveBtn, str);
      }
      else
        this.mainSaveBtn.gameObject.SetActive(false);
      if (list.Count > 1)
      {
        for (int index = 1; index < list.Count; ++index)
          this.AddNewSave(this.buttonPool.GetFreeElement(this.buttonGroup, true), list[index]);
      }
      this.Show(true);
    }
  }

  private void ConfirmDoAction(string message, System.Action action)
  {
    if (!((UnityEngine.Object) this.confirmScreen == (UnityEngine.Object) null))
      return;
    this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject, false);
    this.confirmScreen.PopupConfirmDialog(message, action, (System.Action) (() => {}), (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    this.confirmScreen.GetComponent<LayoutElement>().ignoreLayout = true;
    this.confirmScreen.gameObject.SetActive(true);
  }

  private void DeleteSave()
  {
    if (string.IsNullOrEmpty(this.currentPath))
      Debug.LogError((object) "The path provided is not valid and cannot be deleted.");
    else
      this.ConfirmDoAction((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, (System.Action) (() =>
      {
        foreach (string file in Directory.GetFiles(this.currentPath))
          File.Delete(file);
        Directory.Delete(this.currentPath);
        this.CloseScreen();
      }));
  }

  private void AddNewSave(KButton btn, string file)
  {
  }

  private void ButtonClicked(KButton btn)
  {
    LoadingOverlay.Load((System.Action) (() => this.Load(this.buttonFileMap[btn])));
  }

  private void Load(string filename)
  {
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      LoadScreen.ForceStopGame();
    SaveLoader.SetActiveSaveFilePath(filename);
    App.LoadScene("backend");
    this.Deactivate();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.CloseScreen();
    else
      base.OnKeyDown(e);
  }
}
