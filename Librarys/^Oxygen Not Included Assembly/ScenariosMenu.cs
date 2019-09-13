// Decompiled with JetBrains decompiler
// Type: ScenariosMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenariosMenu : KModalScreen, SteamUGCService.IClient
{
  private List<GameObject> buttons = new List<GameObject>();
  public const string TAG_SCENARIO = "scenario";
  public KButton textButton;
  public KButton dismissButton;
  public KButton closeButton;
  public KButton workshopButton;
  public KButton loadScenarioButton;
  [Space]
  public GameObject ugcContainer;
  public GameObject ugcButtonPrefab;
  public LocText noScenariosText;
  public RectTransform contentRoot;
  public RectTransform detailsRoot;
  public LocText scenarioTitle;
  public LocText scenarioDetails;
  private PublishedFileId_t activeItem;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.dismissButton.onClick += (System.Action) (() => this.Deactivate());
    this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText((string) STRINGS.UI.FRONTEND.OPTIONS_SCREEN.BACK);
    this.closeButton.onClick += (System.Action) (() => this.Deactivate());
    this.workshopButton.onClick += (System.Action) (() => this.OnClickOpenWorkshop());
    this.RebuildScreen();
  }

  private void RebuildScreen()
  {
    foreach (UnityEngine.Object button in this.buttons)
      UnityEngine.Object.Destroy(button);
    this.buttons.Clear();
    this.RebuildUGCButtons();
  }

  private void RebuildUGCButtons()
  {
    ListPool<SteamUGCService.Mod, ScenariosMenu>.PooledList pooledList = ListPool<SteamUGCService.Mod, ScenariosMenu>.Allocate();
    bool flag1 = pooledList.Count > 0;
    this.noScenariosText.gameObject.SetActive(!flag1);
    this.contentRoot.gameObject.SetActive(flag1);
    bool flag2 = true;
    if (pooledList.Count != 0)
    {
      for (int index1 = 0; index1 < pooledList.Count; ++index1)
      {
        GameObject gameObject = Util.KInstantiateUI(this.ugcButtonPrefab, this.ugcContainer, false);
        gameObject.name = pooledList[index1].title + "_button";
        gameObject.gameObject.SetActive(true);
        HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
        TMP_FontAsset fontForLangage = LanguageOptionsScreen.GetFontForLangage(pooledList[index1].fileId);
        LocText reference = component1.GetReference<LocText>("Title");
        reference.SetText(pooledList[index1].title);
        reference.font = fontForLangage;
        Texture2D previewImage = pooledList[index1].previewImage;
        if ((UnityEngine.Object) previewImage != (UnityEngine.Object) null)
          component1.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float) previewImage.width, (float) previewImage.height)), Vector2.one * 0.5f);
        KButton component2 = gameObject.GetComponent<KButton>();
        int index2 = index1;
        PublishedFileId_t item = pooledList[index2].fileId;
        component2.onClick += (System.Action) (() => this.ShowDetails(item));
        component2.onDoubleClick += (System.Action) (() => this.LoadScenario(item));
        this.buttons.Add(gameObject);
        if (item == this.activeItem)
          flag2 = false;
      }
    }
    if (flag2)
      this.HideDetails();
    pooledList.Recycle();
  }

  private void LoadScenario(PublishedFileId_t item)
  {
    ulong punSizeOnDisk;
    string pchFolder;
    uint punTimeStamp;
    SteamUGC.GetItemInstallInfo(item, out punSizeOnDisk, out pchFolder, 1024U, out punTimeStamp);
    DebugUtil.LogArgs((object) nameof (LoadScenario), (object) pchFolder, (object) punSizeOnDisk, (object) punTimeStamp);
    System.DateTime lastModified;
    byte[] bytesFromZip = SteamUGCService.GetBytesFromZip(item, new string[1]
    {
      ".sav"
    }, out lastModified, false);
    string path = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), "scenario.sav");
    File.WriteAllBytes(path, bytesFromZip);
    SaveLoader.SetActiveSaveFilePath(path);
    Time.timeScale = 0.0f;
    App.LoadScene("backend");
  }

  private ConfirmDialogScreen GetConfirmDialog()
  {
    KScreen component = KScreenManager.AddChild(this.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
    component.Activate();
    return component.GetComponent<ConfirmDialogScreen>();
  }

  private void ShowDetails(PublishedFileId_t item)
  {
    this.activeItem = item;
    SteamUGCService.Mod mod = SteamUGCService.Instance.FindMod(item);
    if (mod != null)
    {
      this.scenarioTitle.text = mod.title;
      this.scenarioDetails.text = mod.description;
    }
    this.loadScenarioButton.onClick += (System.Action) (() => this.LoadScenario(item));
    this.detailsRoot.gameObject.SetActive(true);
  }

  private void HideDetails()
  {
    this.detailsRoot.gameObject.SetActive(false);
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    SteamUGCService.Instance.AddClient((SteamUGCService.IClient) this);
    this.HideDetails();
  }

  protected override void OnDeactivate()
  {
    base.OnDeactivate();
    SteamUGCService.Instance.RemoveClient((SteamUGCService.IClient) this);
  }

  private void OnClickOpenWorkshop()
  {
    Application.OpenURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=scenario");
  }

  public void UpdateMods(
    IEnumerable<PublishedFileId_t> added,
    IEnumerable<PublishedFileId_t> updated,
    IEnumerable<PublishedFileId_t> removed,
    IEnumerable<SteamUGCService.Mod> loaded_previews)
  {
    this.RebuildScreen();
  }
}
