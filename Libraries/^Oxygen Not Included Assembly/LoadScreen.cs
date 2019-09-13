// Decompiled with JetBrains decompiler
// Type: LoadScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : KModalScreen
{
  public bool requireConfirmation = true;
  private Dictionary<string, KButton> fileButtonMap = new Dictionary<string, KButton>();
  private InspectSaveScreen inspectScreenInstance;
  [SerializeField]
  private HierarchyReferences saveButtonPrefab;
  [SerializeField]
  private GameObject saveButtonRoot;
  [SerializeField]
  private LocText saveDetails;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton loadButton;
  [SerializeField]
  private KButton deleteButton;
  [SerializeField]
  private GameObject previewImageRoot;
  [SerializeField]
  private ColorStyleSetting validSaveFileStyle;
  [SerializeField]
  private ColorStyleSetting invalidSaveFileStyle;
  public LocText FileName;
  public LocText CyclesSurvivedValue;
  public LocText DuplicantsAliveValue;
  public LocText InfoText;
  public System.Action<string, string> onClick;
  private UIPool<HierarchyReferences> savenameRowPool;
  private ConfirmDialogScreen confirmScreen;
  private string selectedFileName;
  private KButton currentExpanded;
  private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> saveFiles;

  public static LoadScreen Instance { get; private set; }

  public static void DestroyInstance()
  {
    LoadScreen.Instance = (LoadScreen) null;
  }

  protected override void OnPrefabInit()
  {
    Debug.Assert((UnityEngine.Object) LoadScreen.Instance == (UnityEngine.Object) null);
    LoadScreen.Instance = this;
    base.OnPrefabInit();
    this.savenameRowPool = new UIPool<HierarchyReferences>(this.saveButtonPrefab);
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      SpeedControlScreen.Instance.Pause(false);
    if (this.onClick == null)
      this.onClick = new System.Action<string, string>(this.SetSelectedGame);
    if ((UnityEngine.Object) this.closeButton != (UnityEngine.Object) null)
      this.closeButton.onClick += (System.Action) (() => this.Deactivate());
    if ((UnityEngine.Object) this.loadButton != (UnityEngine.Object) null)
      this.loadButton.onClick += new System.Action(this.Load);
    if (!((UnityEngine.Object) this.deleteButton != (UnityEngine.Object) null))
      return;
    this.deleteButton.onClick += new System.Action(this.Delete);
    this.deleteButton.isInteractable = false;
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.RefreshFiles();
  }

  private void GetFilesList()
  {
    this.saveFiles = new Dictionary<string, List<LoadScreen.SaveGameFileDetails>>();
    List<string> allFiles = SaveLoader.GetAllFiles();
    if (allFiles.Count <= 0)
      return;
    for (int index = 0; index < allFiles.Count; ++index)
    {
      if (this.IsFileValid(allFiles[index]))
      {
        Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = this.GetFileInfo(allFiles[index]);
        SaveGame.Header first = fileInfo.first;
        SaveGame.GameInfo second = fileInfo.second;
        System.DateTime lastWriteTime = File.GetLastWriteTime(allFiles[index]);
        string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(!(second.originalSaveName != string.Empty) ? allFiles[index] : second.originalSaveName);
        LoadScreen.SaveGameFileDetails saveGameFileDetails = new LoadScreen.SaveGameFileDetails();
        saveGameFileDetails.BaseName = second.baseName;
        saveGameFileDetails.FileName = allFiles[index];
        saveGameFileDetails.FileDate = lastWriteTime;
        saveGameFileDetails.FileHeader = first;
        saveGameFileDetails.FileInfo = second;
        if (!this.saveFiles.ContainsKey(withoutExtension))
          this.saveFiles.Add(withoutExtension, new List<LoadScreen.SaveGameFileDetails>());
        this.saveFiles[withoutExtension].Add(saveGameFileDetails);
      }
    }
  }

  private bool IsFileValid(string filename)
  {
    bool flag = false;
    try
    {
      SaveGame.Header header;
      flag = SaveLoader.LoadHeader(filename, out header).saveMajorVersion >= 7;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("Corrupted save file: " + filename + "\n" + ex.ToString()));
    }
    return flag;
  }

  private Tuple<SaveGame.Header, SaveGame.GameInfo> GetFileInfo(string filename)
  {
    try
    {
      SaveGame.Header header;
      SaveGame.GameInfo b = SaveLoader.LoadHeader(filename, out header);
      if (b.saveMajorVersion >= 7)
        return new Tuple<SaveGame.Header, SaveGame.GameInfo>(header, b);
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ex);
      this.InfoText.text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.CORRUPTEDSAVE, (object) filename);
    }
    return (Tuple<SaveGame.Header, SaveGame.GameInfo>) null;
  }

  private void RefreshFiles()
  {
    if (this.savenameRowPool != null)
      this.savenameRowPool.ClearAll();
    if (this.fileButtonMap != null)
      this.fileButtonMap.Clear();
    this.GetFilesList();
    if (this.saveFiles.Count > 0)
    {
      foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> saveFile in this.saveFiles)
        this.AddExistingSaveFile(saveFile.Key, saveFile.Value);
    }
    this.InfoText.text = string.Empty;
    this.CyclesSurvivedValue.text = "-";
    this.DuplicantsAliveValue.text = "-";
    this.deleteButton.isInteractable = false;
    this.loadButton.isInteractable = false;
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.RefreshFiles();
  }

  protected override void OnDeactivate()
  {
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      SpeedControlScreen.Instance.Unpause(false);
    this.selectedFileName = (string) null;
    base.OnDeactivate();
  }

  private void SetHeaderButtonActive(KButton headerButton, bool activeState)
  {
    ImageToggleState component = headerButton.GetComponent<ImageToggleState>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetActiveState(activeState);
  }

  private void SetChildrenActive(HierarchyReferences hierarchy, bool state)
  {
    for (int index = 0; index < hierarchy.transform.childCount; ++index)
    {
      GameObject gameObject = hierarchy.transform.GetChild(index).gameObject;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        gameObject.SetActive(state);
    }
  }

  private void AddExistingSaveFile(
    string savename,
    List<LoadScreen.SaveGameFileDetails> fileDetailsList)
  {
    HierarchyReferences savenameRow = this.savenameRowPool.GetFreeElement(this.saveButtonRoot, true);
    KButton headerButton = savenameRow.GetReference<RectTransform>("Button").GetComponent<KButton>();
    headerButton.ClearOnClick();
    LocText headerTitle = savenameRow.GetReference<RectTransform>("HeaderTitle").GetComponent<LocText>();
    LocText component1 = savenameRow.GetReference<RectTransform>("SaveTitle").GetComponent<LocText>();
    LocText component2 = savenameRow.GetReference<RectTransform>("HeaderDate").GetComponent<LocText>();
    RectTransform saveDetailsRow = savenameRow.GetReference<RectTransform>("SaveDetailsRow");
    LocText component3 = savenameRow.GetReference<RectTransform>("SaveDetailsBaseName").GetComponent<LocText>();
    RectTransform savefileRowTemplate = savenameRow.GetReference<RectTransform>("SavefileRowTemplate");
    fileDetailsList.Sort((Comparison<LoadScreen.SaveGameFileDetails>) ((x, y) => y.FileDate.CompareTo(x.FileDate)));
    headerTitle.text = fileDetailsList[0].BaseName;
    component1.text = savename;
    component2.text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), (object) fileDetailsList[0].FileDate);
    component3.text = string.Format("{0}: {1}", (object) STRINGS.UI.FRONTEND.LOADSCREEN.BASE_NAME, (object) fileDetailsList[0].BaseName);
    for (int index = 0; index < savenameRow.transform.childCount; ++index)
    {
      GameObject gameObject = savenameRow.transform.GetChild(index).gameObject;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.name.Contains("Clone"))
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
    }
    bool flag1 = true;
    foreach (LoadScreen.SaveGameFileDetails fileDetails1 in fileDetailsList)
    {
      LoadScreen.SaveGameFileDetails fileDetails = fileDetails1;
      RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(savefileRowTemplate, savenameRow.transform);
      HierarchyReferences component4 = rectTransform.GetComponent<HierarchyReferences>();
      KButton component5 = rectTransform.GetComponent<KButton>();
      RectTransform reference1 = component4.GetReference<RectTransform>("NewestLabel");
      RectTransform reference2 = component4.GetReference<RectTransform>("AutoLabel");
      LocText component6 = component4.GetReference<RectTransform>("SaveText").GetComponent<LocText>();
      LocText component7 = component4.GetReference<RectTransform>("DateText").GetComponent<LocText>();
      reference1.gameObject.SetActive(flag1);
      flag1 = false;
      reference2.gameObject.SetActive(fileDetails.FileInfo.isAutoSave);
      component6.text = System.IO.Path.GetFileNameWithoutExtension(fileDetails.FileName);
      component7.text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), (object) fileDetails.FileDate);
      component5.onClick += (System.Action) (() => this.onClick(fileDetails.FileName, savename));
      component5.onDoubleClick += (System.Action) (() =>
      {
        this.onClick(fileDetails.FileName, savename);
        this.Load();
      });
      this.fileButtonMap.Add(fileDetails.FileName, component5);
    }
    headerButton.onClick += (System.Action) (() =>
    {
      bool activeSelf = saveDetailsRow.gameObject.activeSelf;
      bool flag2 = (UnityEngine.Object) headerButton == (UnityEngine.Object) this.currentExpanded;
      if (flag2)
        this.SetChildrenActive(savenameRow, !activeSelf);
      else if (!activeSelf && !flag2)
        this.SetChildrenActive(savenameRow, true);
      if ((UnityEngine.Object) this.currentExpanded != (UnityEngine.Object) null && !flag2)
        this.SetHeaderButtonActive(this.currentExpanded, false);
      this.currentExpanded = headerButton;
      this.SetHeaderButtonActive(this.currentExpanded, true);
      headerTitle.transform.parent.gameObject.SetActive(true);
      savefileRowTemplate.gameObject.SetActive(false);
      this.onClick(fileDetailsList[0].FileName, savename);
    });
    headerButton.onDoubleClick += (System.Action) (() =>
    {
      this.onClick(fileDetailsList[0].FileName, savename);
      LoadingOverlay.Load(new System.Action(this.DoLoad));
    });
    savenameRow.transform.SetAsLastSibling();
  }

  public static void ForceStopGame()
  {
    ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
    Game.Instance.SetIsLoading();
    Grid.CellCount = 0;
    Sim.Shutdown();
  }

  private static bool IsSaveFileFromUnsupportedFutureBuild(SaveGame.Header header)
  {
    return header.buildVersion > 366134U;
  }

  private void SetSelectedGame(string filename, string savename)
  {
    if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
    {
      Debug.LogError((object) "The filename provided is not valid.");
      this.deleteButton.isInteractable = false;
    }
    else
    {
      this.deleteButton.isInteractable = true;
      KButton kbutton = this.selectedFileName == null ? (KButton) null : this.fileButtonMap[this.selectedFileName];
      if ((UnityEngine.Object) kbutton != (UnityEngine.Object) null)
        kbutton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
      this.selectedFileName = filename;
      this.FileName.text = System.IO.Path.GetFileName(this.selectedFileName);
      this.fileButtonMap[this.selectedFileName].GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
      try
      {
        SaveGame.Header header;
        SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out header);
        string fileName = System.IO.Path.GetFileName(filename);
        if (gameInfo.isAutoSave)
        {
          string str = fileName + "\n" + (string) STRINGS.UI.FRONTEND.LOADSCREEN.AUTOSAVEWARNING;
        }
        this.CyclesSurvivedValue.text = gameInfo.numberOfCycles.ToString();
        this.DuplicantsAliveValue.text = gameInfo.numberOfDuplicants.ToString();
        this.InfoText.text = string.Empty;
        if (LoadScreen.IsSaveFileFromUnsupportedFutureBuild(header))
        {
          this.InfoText.text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.SAVE_TOO_NEW, (object) filename, (object) header.buildVersion, (object) 366134U);
          this.loadButton.isInteractable = false;
          this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
        }
        else if (gameInfo.saveMajorVersion < 7)
        {
          this.InfoText.text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.UNSUPPORTED_SAVE_VERSION, (object) filename, (object) gameInfo.saveMajorVersion, (object) gameInfo.saveMinorVersion, (object) 7, (object) 11);
          this.loadButton.isInteractable = false;
          this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
        }
        else if (!this.loadButton.isInteractable)
        {
          this.loadButton.isInteractable = true;
          this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
        }
        if (this.InfoText.text == string.Empty)
        {
          if (gameInfo.isAutoSave)
            this.InfoText.text = (string) STRINGS.UI.FRONTEND.LOADSCREEN.AUTOSAVEWARNING;
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ex);
        this.InfoText.text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.CORRUPTEDSAVE, (object) filename);
        if (this.loadButton.isInteractable)
        {
          this.loadButton.isInteractable = false;
          this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
        }
        this.deleteButton.isInteractable = false;
      }
      try
      {
        Sprite sprite = RetireColonyUtility.LoadColonyPreview(this.selectedFileName, savename);
        Image component = this.previewImageRoot.GetComponent<Image>();
        component.sprite = sprite;
        component.color = !(bool) ((UnityEngine.Object) sprite) ? Color.black : Color.white;
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex);
      }
    }
  }

  private void Load()
  {
    LoadingOverlay.Load(new System.Action(this.DoLoad));
  }

  private void DoLoad()
  {
    LoadScreen.DoLoad(this.selectedFileName);
    this.Deactivate();
  }

  private static void DoLoad(string filename)
  {
    ReportErrorDialog.MOST_RECENT_SAVEFILE = filename;
    bool flag = true;
    SaveGame.Header header;
    SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out header);
    string str1 = (string) null;
    string str2 = (string) null;
    if (header.buildVersion > 366134U)
    {
      str1 = header.buildVersion.ToString();
      str2 = 366134U.ToString();
    }
    else if (gameInfo.saveMajorVersion < 7)
    {
      str1 = string.Format("v{0}.{1}", (object) gameInfo.saveMajorVersion, (object) gameInfo.saveMinorVersion);
      str2 = string.Format("v{0}.{1}", (object) 7, (object) 11);
    }
    if (!flag)
    {
      Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, !((UnityEngine.Object) FrontEndManager.Instance == (UnityEngine.Object) null) ? FrontEndManager.Instance.gameObject : GameScreenManager.Instance.ssOverlayCanvas, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format((string) STRINGS.UI.CRASHSCREEN.LOADFAILED, (object) "Version Mismatch", (object) str1, (object) str2), (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    }
    else
    {
      if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
        LoadScreen.ForceStopGame();
      SaveLoader.SetActiveSaveFilePath(filename);
      Time.timeScale = 0.0f;
      App.LoadScene("backend");
    }
  }

  private void MoreInfo()
  {
    Application.OpenURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
  }

  private void Delete()
  {
    if (string.IsNullOrEmpty(this.selectedFileName))
      Debug.LogError((object) "The path provided is not valid and cannot be deleted.");
    else
      this.ConfirmDoAction(string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, (object) System.IO.Path.GetFileName(this.selectedFileName)), (System.Action) (() =>
      {
        this.fileButtonMap[this.selectedFileName].GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
        this.fileButtonMap[this.selectedFileName].isInteractable = true;
        File.Delete(this.selectedFileName);
        this.selectedFileName = (string) null;
        this.RefreshFiles();
      }));
  }

  private void ConfirmDoAction(string message, System.Action action)
  {
    if (!((UnityEngine.Object) this.confirmScreen == (UnityEngine.Object) null))
      return;
    this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject, false);
    this.confirmScreen.PopupConfirmDialog(message, action, (System.Action) (() => {}), (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    this.confirmScreen.gameObject.SetActive(true);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.Deactivate();
    base.OnKeyUp(e);
  }

  private struct SaveGameFileDetails
  {
    public string BaseName;
    public string FileName;
    public System.DateTime FileDate;
    public SaveGame.Header FileHeader;
    public SaveGame.GameInfo FileInfo;
  }
}
