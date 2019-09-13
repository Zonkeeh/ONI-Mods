// Decompiled with JetBrains decompiler
// Type: MainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : KScreen
{
  private static int LANGUAGE_CONFIRMATION_VERSION = 2;
  private bool refreshResumeButton = true;
  private Dictionary<string, MainMenu.SaveFileEntry> saveFileEntries = new Dictionary<string, MainMenu.SaveFileEntry>();
  public RectTransform LogoAndMenu;
  public KButton Button_ResumeGame;
  public GameObject topLeftAlphaMessage;
  private float lastUpdateTime;
  private MotdServerClient m_motdServerClient;
  private GameObject GameSettingsScreen;
  [SerializeField]
  private KButton buttonPrefab;
  [SerializeField]
  private GameObject buttonParent;
  [SerializeField]
  private LocText motdImageHeader;
  [SerializeField]
  private UnityEngine.UI.Button motdImageButton;
  [SerializeField]
  private Image motdImage;
  [SerializeField]
  private LocText motdNewsHeader;
  [SerializeField]
  private LocText motdNewsBody;
  [SerializeField]
  private PatchNotesScreen patchNotesScreen;
  [SerializeField]
  private NextUpdateTimer nextUpdateTimer;
  private static bool HasAutoresumedOnce;

  private KButton MakeButton(MainMenu.ButtonInfo info)
  {
    KButton kbutton = Util.KInstantiateUI<KButton>(this.buttonPrefab.gameObject, this.buttonParent, true);
    kbutton.onClick += info.action;
    LocText componentInChildren = kbutton.GetComponentInChildren<LocText>();
    componentInChildren.text = (string) info.text;
    componentInChildren.fontSize = (float) info.fontSize;
    return kbutton;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.MakeButton(new MainMenu.ButtonInfo(STRINGS.UI.FRONTEND.MAINMENU.NEWGAME, new System.Action(this.NewGame), 22));
    this.MakeButton(new MainMenu.ButtonInfo(STRINGS.UI.FRONTEND.MAINMENU.LOADGAME, new System.Action(this.LoadGame), 14));
    this.MakeButton(new MainMenu.ButtonInfo(STRINGS.UI.FRONTEND.MAINMENU.RETIREDCOLONIES, (System.Action) (() => MainMenu.ActivateRetiredColoniesScreen(this.transform.gameObject, string.Empty, (string[]) null)), 14));
    if (DistributionPlatform.Initialized)
    {
      this.MakeButton(new MainMenu.ButtonInfo(STRINGS.UI.FRONTEND.MAINMENU.TRANSLATIONS, new System.Action(this.Translations), 14));
      this.MakeButton(new MainMenu.ButtonInfo(STRINGS.UI.FRONTEND.MODS.TITLE, new System.Action(this.Mods), 14));
    }
    this.MakeButton(new MainMenu.ButtonInfo(STRINGS.UI.FRONTEND.MAINMENU.OPTIONS, new System.Action(this.Options), 14));
    this.MakeButton(new MainMenu.ButtonInfo(STRINGS.UI.FRONTEND.MAINMENU.QUITTODESKTOP, new System.Action(this.QuitGame), 14));
    KCrashReporter.MOST_RECENT_SAVEFILE = (string) null;
    this.RefreshResumeButton();
    this.Button_ResumeGame.onClick += new System.Action(this.ResumeGame);
    this.StartFEAudio();
    this.SpawnVideoScreen();
    this.CheckPlayerPrefsCorruption();
    if (PatchNotesScreen.ShouldShowScreen())
      this.patchNotesScreen.gameObject.SetActive(true);
    this.CheckDoubleBoundKeys();
    this.topLeftAlphaMessage.gameObject.SetActive(false);
    this.nextUpdateTimer.gameObject.SetActive(false);
    this.m_motdServerClient = new MotdServerClient();
    this.m_motdServerClient.GetMotd((System.Action<MotdServerClient.MotdResponse, string>) ((response, error) =>
    {
      if (error == null)
      {
        this.topLeftAlphaMessage.gameObject.SetActive(true);
        this.nextUpdateTimer.gameObject.SetActive(true);
        this.motdImageHeader.text = response.image_header_text;
        this.motdNewsHeader.text = response.news_header_text;
        this.motdNewsBody.text = response.news_body_text;
        this.patchNotesScreen.UpdatePatchNotes(response.patch_notes_summary, response.patch_notes_link_url);
        this.nextUpdateTimer.UpdateReleaseTimes(response.last_update_time, response.next_update_time, response.update_text_override);
        if (!((UnityEngine.Object) this.motdImage != (UnityEngine.Object) null) || !((UnityEngine.Object) response.image_texture != (UnityEngine.Object) null))
          return;
        this.motdImage.sprite = Sprite.Create(response.image_texture, new Rect(0.0f, 0.0f, (float) response.image_texture.width, (float) response.image_texture.height), Vector2.zero);
        if ((double) this.motdImage.sprite.rect.height != 0.0)
        {
          AspectRatioFitter component = this.motdImage.gameObject.GetComponent<AspectRatioFitter>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            float num = this.motdImage.sprite.rect.width / this.motdImage.sprite.rect.height;
            component.aspectRatio = num;
          }
          else
            Debug.LogWarning((object) "Missing AspectRatioFitter on MainMenu motd image.");
        }
        this.motdImageButton.onClick.AddListener((UnityAction) (() => Application.OpenURL(response.image_link_url)));
      }
      else
        Debug.LogWarning((object) ("Motd Request error: " + error));
    }));
    this.lastUpdateTime = Time.unscaledTime;
    this.activateOnSpawn = true;
  }

  public void RefreshMainMenu()
  {
    if (!this.refreshResumeButton)
      return;
    this.RefreshResumeButton();
  }

  private void PlayMouseOverSound()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
  }

  private void PlayMouseClickSound()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
  }

  protected override void OnSpawn()
  {
    Debug.Log((object) "-- MAIN MENU -- ");
    base.OnSpawn();
    Canvas.ForceUpdateCanvases();
    this.ShowLanguageConfirmation();
    string savePrefix = SaveLoader.GetSavePrefix();
    try
    {
      string path = System.IO.Path.Combine(savePrefix, "__SPCCHK");
      using (FileStream fileStream = File.OpenWrite(path))
      {
        byte[] buffer = new byte[1024];
        for (int index = 0; index < 15360; ++index)
          fileStream.Write(buffer, 0, buffer.Length);
      }
      File.Delete(path);
    }
    catch (Exception ex)
    {
      Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject, true).PopupConfirmDialog(string.Format(!(ex is IOException) ? string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, (object) savePrefix) : string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, (object) savePrefix), (object) savePrefix), (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    }
    Global.Instance.modManager.Report(this.gameObject);
    if ((!GenericGameSettings.instance.autoResumeGame || MainMenu.HasAutoresumedOnce) && string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame))
      return;
    MainMenu.HasAutoresumedOnce = true;
    this.ResumeGame();
  }

  private void UnregisterMotdRequest()
  {
    if (this.m_motdServerClient == null)
      return;
    this.m_motdServerClient.UnregisterCallback();
    this.m_motdServerClient = (MotdServerClient) null;
  }

  protected override void OnDeactivate()
  {
    base.OnDeactivate();
    this.UnregisterMotdRequest();
  }

  public override void ScreenUpdate(bool topLevel)
  {
    this.refreshResumeButton = topLevel;
  }

  protected override void OnLoadLevel()
  {
    base.OnLoadLevel();
    this.UnregisterMotdRequest();
  }

  private void ShowLanguageConfirmation()
  {
    if (!SteamManager.Initialized || SteamUtils.GetSteamUILanguage() != "schinese" || KPlayerPrefs.GetInt("LanguageConfirmationVersion") >= MainMenu.LANGUAGE_CONFIRMATION_VERSION)
      return;
    KPlayerPrefs.SetInt("LanguageConfirmationVersion", MainMenu.LANGUAGE_CONFIRMATION_VERSION);
    this.Translations();
  }

  private void ResumeGame()
  {
    string path = !string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame) ? GenericGameSettings.instance.performanceCapture.saveGame : SaveLoader.GetLatestSaveFile();
    if (string.IsNullOrEmpty(path))
      return;
    KCrashReporter.MOST_RECENT_SAVEFILE = path;
    SaveLoader.SetActiveSaveFilePath(path);
    LoadingOverlay.Load((System.Action) (() => App.LoadScene("backend")));
  }

  private void NewGame()
  {
    this.GetComponent<NewGameFlow>().BeginFlow();
  }

  private void LoadGame()
  {
    if ((UnityEngine.Object) LoadScreen.Instance == (UnityEngine.Object) null)
    {
      LoadScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.LoadScreen.gameObject, this.gameObject, true).GetComponent<LoadScreen>();
      component.requireConfirmation = false;
      component.SetBackgroundActive(true);
    }
    LoadScreen.Instance.gameObject.SetActive(true);
  }

  public static void ActivateRetiredColoniesScreen(
    GameObject parent,
    string colonyID = "",
    string[] newlyAchieved = null)
  {
    if ((UnityEngine.Object) RetiredColonyInfoScreen.Instance == (UnityEngine.Object) null)
      Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
    RetiredColonyInfoScreen.Instance.Show(true);
    if (string.IsNullOrEmpty(colonyID))
      return;
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
      RetireColonyUtility.SaveColonySummaryData();
    RetiredColonyInfoScreen.Instance.LoadColony(RetiredColonyInfoScreen.Instance.GetColonyDataByBaseName(colonyID));
  }

  public static void ActivateRetiredColoniesScreenFromData(
    GameObject parent,
    RetiredColonyData data)
  {
    if ((UnityEngine.Object) RetiredColonyInfoScreen.Instance == (UnityEngine.Object) null)
      Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
    RetiredColonyInfoScreen.Instance.Show(true);
    RetiredColonyInfoScreen.Instance.LoadColony(data);
  }

  private void SpawnVideoScreen()
  {
    VideoScreen.Instance = Util.KInstantiateUI(ScreenPrefabs.Instance.VideoScreen.gameObject, this.gameObject, false).GetComponent<VideoScreen>();
  }

  private void Update()
  {
    if ((double) Time.unscaledTime - (double) this.lastUpdateTime <= 1.0)
      return;
    this.RefreshMainMenu();
    this.lastUpdateTime = Time.unscaledTime;
  }

  private void RefreshResumeButton()
  {
    string latestSaveFile = SaveLoader.GetLatestSaveFile();
    bool flag = !string.IsNullOrEmpty(latestSaveFile) && File.Exists(latestSaveFile);
    if (flag)
    {
      try
      {
        if (GenericGameSettings.instance.demoMode)
          flag = false;
        System.DateTime lastWriteTime = File.GetLastWriteTime(latestSaveFile);
        MainMenu.SaveFileEntry saveFileEntry1 = new MainMenu.SaveFileEntry();
        SaveGame.Header header = new SaveGame.Header();
        SaveGame.GameInfo gameInfo1 = new SaveGame.GameInfo();
        SaveGame.GameInfo gameInfo2;
        if (!this.saveFileEntries.TryGetValue(latestSaveFile, out saveFileEntry1) || saveFileEntry1.timeStamp != lastWriteTime)
        {
          gameInfo2 = SaveLoader.LoadHeader(latestSaveFile, out header);
          MainMenu.SaveFileEntry saveFileEntry2 = new MainMenu.SaveFileEntry()
          {
            timeStamp = lastWriteTime,
            header = header,
            headerData = gameInfo2
          };
          this.saveFileEntries[latestSaveFile] = saveFileEntry2;
        }
        else
        {
          header = saveFileEntry1.header;
          gameInfo2 = saveFileEntry1.headerData;
        }
        if (header.buildVersion > 366134U || gameInfo2.saveMajorVersion < 7)
          flag = false;
        string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(latestSaveFile);
        if (!string.IsNullOrEmpty(gameInfo2.baseName))
          this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = string.Format((string) STRINGS.UI.FRONTEND.MAINMENU.RESUMEBUTTON_BASENAME, (object) gameInfo2.baseName, (object) (gameInfo2.numberOfCycles + 1));
        else
          this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = withoutExtension;
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ex);
        flag = false;
      }
    }
    if ((UnityEngine.Object) this.Button_ResumeGame != (UnityEngine.Object) null && (UnityEngine.Object) this.Button_ResumeGame.gameObject != (UnityEngine.Object) null)
      this.Button_ResumeGame.gameObject.SetActive(flag);
    else
      Debug.LogWarning((object) "Why is the resume game button null?");
  }

  private void Translations()
  {
    Util.KInstantiateUI<LanguageOptionsScreen>(ScreenPrefabs.Instance.languageOptionsScreen.gameObject, this.transform.parent.gameObject, false).SetBackgroundActive(true);
  }

  private void Mods()
  {
    Util.KInstantiateUI<ModsScreen>(ScreenPrefabs.Instance.modsMenu.gameObject, this.transform.parent.gameObject, false).SetBackgroundActive(true);
  }

  private void Options()
  {
    Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, this.gameObject, true);
  }

  private void QuitGame()
  {
    App.Quit();
  }

  public void StartFEAudio()
  {
    AudioMixer.instance.Reset();
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSnapshot);
    if (!AudioMixer.instance.SnapshotIsActive((HashedString) AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot))
      AudioMixer.instance.StartUserVolumesSnapshot();
    if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying("Music_TitleTheme"))
      MusicManager.instance.PlaySong("Music_TitleTheme", false);
    this.CheckForAudioDriverIssue();
  }

  private void CheckForAudioDriverIssue()
  {
    if (KFMOD.didFmodInitializeSuccessfully)
      return;
    ConfirmDialogScreen confirmDialogScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject, true);
    string audioDrivers = (string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS;
    System.Action action1 = (System.Action) null;
    System.Action action2 = (System.Action) null;
    string audioDriversMoreInfo = (string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS_MORE_INFO;
    System.Action action3 = (System.Action) (() => Application.OpenURL("http://support.kleientertainment.com/customer/en/portal/articles/2947881-no-audio-when-playing-oxygen-not-included"));
    Sprite sadDupeAudio = GlobalResources.Instance().sadDupeAudio;
    string text = audioDrivers;
    System.Action on_confirm = action1;
    System.Action on_cancel = action2;
    string configurable_text = audioDriversMoreInfo;
    System.Action on_configurable_clicked = action3;
    Sprite image_sprite = sadDupeAudio;
    confirmDialogScreen.PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, (string) null, (string) null, (string) null, image_sprite, true);
  }

  private void CheckPlayerPrefsCorruption()
  {
    if (!KPlayerPrefs.HasCorruptedFlag())
      return;
    KPlayerPrefs.ResetCorruptedFlag();
    ConfirmDialogScreen confirmDialogScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject, true);
    string playerPrefsCorrupted = (string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.PLAYER_PREFS_CORRUPTED;
    System.Action action1 = (System.Action) null;
    System.Action action2 = (System.Action) null;
    Sprite sadDupe = GlobalResources.Instance().sadDupe;
    string text = playerPrefsCorrupted;
    System.Action on_confirm = action1;
    System.Action on_cancel = action2;
    Sprite image_sprite = sadDupe;
    confirmDialogScreen.PopupConfirmDialog(text, on_confirm, on_cancel, (string) null, (System.Action) null, (string) null, (string) null, (string) null, image_sprite, true);
  }

  private void CheckDoubleBoundKeys()
  {
    string str1 = string.Empty;
    HashSet<BindingEntry> bindingEntrySet = new HashSet<BindingEntry>();
    for (int index1 = 0; index1 < GameInputMapping.KeyBindings.Length; ++index1)
    {
      if (GameInputMapping.KeyBindings[index1].mKeyCode != KKeyCode.Mouse1)
      {
        for (int index2 = 0; index2 < GameInputMapping.KeyBindings.Length; ++index2)
        {
          if (index1 != index2)
          {
            BindingEntry keyBinding1 = GameInputMapping.KeyBindings[index2];
            if (!bindingEntrySet.Contains(keyBinding1))
            {
              BindingEntry keyBinding2 = GameInputMapping.KeyBindings[index1];
              if (keyBinding2.mKeyCode != KKeyCode.None && keyBinding2.mKeyCode == keyBinding1.mKeyCode && (keyBinding2.mModifier == keyBinding1.mModifier && keyBinding2.mRebindable) && keyBinding1.mRebindable)
              {
                string mGroup1 = GameInputMapping.KeyBindings[index1].mGroup;
                string mGroup2 = GameInputMapping.KeyBindings[index2].mGroup;
                if ((mGroup1 == "Root" || mGroup2 == "Root" || mGroup1 == mGroup2) && ((!(mGroup1 == "Root") || !keyBinding1.mIgnoreRootConflics) && (!(mGroup2 == "Root") || !keyBinding2.mIgnoreRootConflics)))
                {
                  str1 = str1 + "\n\n" + (object) keyBinding2.mAction + ": <b>" + (object) keyBinding2.mKeyCode + "</b>\n" + (object) keyBinding1.mAction + ": <b>" + (object) keyBinding1.mKeyCode + "</b>";
                  BindingEntry bindingEntry = keyBinding2;
                  bindingEntry.mKeyCode = KKeyCode.None;
                  bindingEntry.mModifier = Modifier.None;
                  GameInputMapping.KeyBindings[index1] = bindingEntry;
                  bindingEntry = keyBinding1;
                  bindingEntry.mKeyCode = KKeyCode.None;
                  bindingEntry.mModifier = Modifier.None;
                  GameInputMapping.KeyBindings[index2] = bindingEntry;
                }
              }
            }
          }
        }
        bindingEntrySet.Add(GameInputMapping.KeyBindings[index1]);
      }
    }
    if (!(str1 != string.Empty))
      return;
    ConfirmDialogScreen confirmDialogScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject, true);
    string str2 = string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.DUPLICATE_KEY_BINDINGS, (object) str1);
    System.Action action1 = (System.Action) null;
    System.Action action2 = (System.Action) null;
    Sprite sadDupe = GlobalResources.Instance().sadDupe;
    string text = str2;
    System.Action on_confirm = action1;
    System.Action on_cancel = action2;
    Sprite image_sprite = sadDupe;
    confirmDialogScreen.PopupConfirmDialog(text, on_confirm, on_cancel, (string) null, (System.Action) null, (string) null, (string) null, (string) null, image_sprite, true);
  }

  private void RestartGame()
  {
    App.instance.Restart();
  }

  private struct ButtonInfo
  {
    public LocString text;
    public System.Action action;
    public int fontSize;

    public ButtonInfo(LocString text, System.Action action, int font_size)
    {
      this.text = text;
      this.action = action;
      this.fontSize = font_size;
    }
  }

  private struct SaveFileEntry
  {
    public System.DateTime timeStamp;
    public SaveGame.Header header;
    public SaveGame.GameInfo headerData;
  }
}
