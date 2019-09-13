// Decompiled with JetBrains decompiler
// Type: SteamUGCService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Ionic.Zip;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SteamUGCService : MonoBehaviour
{
  private static readonly string[] previewFileNames = new string[4]
  {
    "preview.png",
    "preview.png",
    ".png",
    ".jpg"
  };
  private UGCQueryHandle_t details_query = UGCQueryHandle_t.Invalid;
  private HashSet<PublishedFileId_t> downloads = new HashSet<PublishedFileId_t>();
  private HashSet<PublishedFileId_t> queries = new HashSet<PublishedFileId_t>();
  private HashSet<PublishedFileId_t> proxies = new HashSet<PublishedFileId_t>();
  private HashSet<SteamUGCDetails_t> publishes = new HashSet<SteamUGCDetails_t>();
  private HashSet<PublishedFileId_t> removals = new HashSet<PublishedFileId_t>();
  private HashSet<SteamUGCDetails_t> previews = new HashSet<SteamUGCDetails_t>();
  private List<SteamUGCService.Mod> mods = new List<SteamUGCService.Mod>();
  private Dictionary<PublishedFileId_t, int> retry_counts = new Dictionary<PublishedFileId_t, int>();
  private List<SteamUGCService.IClient> clients = new List<SteamUGCService.IClient>();
  private Callback<RemoteStoragePublishedFileSubscribed_t> on_subscribed;
  private Callback<RemoteStoragePublishedFileUpdated_t> on_updated;
  private Callback<RemoteStoragePublishedFileUnsubscribed_t> on_unsubscribed;
  private CallResult<SteamUGCQueryCompleted_t> on_query_completed;
  private static SteamUGCService instance;
  private const EItemState DOWNLOADING_MASK = EItemState.k_EItemStateDownloading | EItemState.k_EItemStateDownloadPending;
  private const int RETRY_THRESHOLD = 1000;

  private SteamUGCService()
  {
    this.on_subscribed = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(this.OnItemSubscribed));
    this.on_unsubscribed = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(this.OnItemUnsubscribed));
    this.on_updated = Callback<RemoteStoragePublishedFileUpdated_t>.Create(new Callback<RemoteStoragePublishedFileUpdated_t>.DispatchDelegate(this.OnItemUpdated));
    this.on_query_completed = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryDetailsCompleted));
    this.mods = new List<SteamUGCService.Mod>();
  }

  public static SteamUGCService Instance
  {
    get
    {
      return SteamUGCService.instance;
    }
  }

  public static void Initialize()
  {
    if ((UnityEngine.Object) SteamUGCService.instance != (UnityEngine.Object) null)
      return;
    GameObject gameObject = GameObject.Find("/SteamManager");
    SteamUGCService.instance = gameObject.GetComponent<SteamUGCService>();
    if (!((UnityEngine.Object) SteamUGCService.instance == (UnityEngine.Object) null))
      return;
    SteamUGCService.instance = gameObject.AddComponent<SteamUGCService>();
  }

  public void AddClient(SteamUGCService.IClient client)
  {
    this.clients.Add(client);
    ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
    foreach (SteamUGCService.Mod mod in this.mods)
      pooledList.Add(mod.fileId);
    client.UpdateMods((IEnumerable<PublishedFileId_t>) pooledList, Enumerable.Empty<PublishedFileId_t>(), Enumerable.Empty<PublishedFileId_t>(), Enumerable.Empty<SteamUGCService.Mod>());
    pooledList.Recycle();
  }

  public void RemoveClient(SteamUGCService.IClient client)
  {
    this.clients.Remove(client);
  }

  public void Awake()
  {
    Debug.Assert((UnityEngine.Object) SteamUGCService.instance == (UnityEngine.Object) null);
    SteamUGCService.instance = this;
    uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
    if (numSubscribedItems == 0U)
      return;
    PublishedFileId_t[] pvecPublishedFileID = new PublishedFileId_t[(IntPtr) numSubscribedItems];
    int subscribedItems = (int) SteamUGC.GetSubscribedItems(pvecPublishedFileID, numSubscribedItems);
    this.downloads.UnionWith((IEnumerable<PublishedFileId_t>) pvecPublishedFileID);
  }

  public bool IsSubscribed(PublishedFileId_t item)
  {
    if (!this.downloads.Contains(item) && !this.proxies.Contains(item) && (!this.queries.Contains(item) && !this.publishes.Any<SteamUGCDetails_t>((Func<SteamUGCDetails_t, bool>) (candidate => candidate.m_nPublishedFileId == item))))
      return this.mods.Exists((Predicate<SteamUGCService.Mod>) (candidate => candidate.fileId == item));
    return true;
  }

  public SteamUGCService.Mod FindMod(PublishedFileId_t item)
  {
    return this.mods.Find((Predicate<SteamUGCService.Mod>) (candidate => candidate.fileId == item));
  }

  private void OnDestroy()
  {
    Debug.Assert((UnityEngine.Object) SteamUGCService.instance == (UnityEngine.Object) this);
    SteamUGCService.instance = (SteamUGCService) null;
  }

  private Texture2D LoadPreviewImage(SteamUGCDetails_t details)
  {
    byte[] numArray;
    if (details.m_hPreviewFile != UGCHandle_t.Invalid)
    {
      SteamRemoteStorage.UGCDownload(details.m_hPreviewFile, 0U);
      numArray = new byte[details.m_nPreviewFileSize];
      if (SteamRemoteStorage.UGCRead(details.m_hPreviewFile, numArray, details.m_nPreviewFileSize, 0U, EUGCReadAction.k_EUGCRead_ContinueReadingUntilFinished) != details.m_nPreviewFileSize)
      {
        Debug.LogFormat("Preview image load failed");
        numArray = (byte[]) null;
      }
    }
    else
    {
      System.DateTime lastModified;
      numArray = SteamUGCService.GetBytesFromZip(details.m_nPublishedFileId, SteamUGCService.previewFileNames, out lastModified, false);
    }
    Texture2D tex = (Texture2D) null;
    if (numArray != null)
    {
      tex = new Texture2D(2, 2);
      tex.LoadImage(numArray);
    }
    else
    {
      Dictionary<PublishedFileId_t, int> retryCounts;
      PublishedFileId_t nPublishedFileId;
      (retryCounts = this.retry_counts)[nPublishedFileId = details.m_nPublishedFileId] = retryCounts[nPublishedFileId] + 1;
    }
    return tex;
  }

  private void Update()
  {
    if (!SteamManager.Initialized || (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      return;
    this.downloads.ExceptWith((IEnumerable<PublishedFileId_t>) this.removals);
    this.publishes.RemoveWhere((Predicate<SteamUGCDetails_t>) (publish => this.removals.Contains(publish.m_nPublishedFileId)));
    this.previews.RemoveWhere((Predicate<SteamUGCDetails_t>) (publish => this.removals.Contains(publish.m_nPublishedFileId)));
    this.proxies.ExceptWith((IEnumerable<PublishedFileId_t>) this.removals);
    HashSetPool<SteamUGCService.Mod, SteamUGCService>.PooledHashSet loaded_previews = HashSetPool<SteamUGCService.Mod, SteamUGCService>.Allocate();
    HashSetPool<PublishedFileId_t, SteamUGCService>.PooledHashSet cancelled_previews = HashSetPool<PublishedFileId_t, SteamUGCService>.Allocate();
    foreach (SteamUGCDetails_t preview in this.previews)
    {
      SteamUGCService.Mod mod = this.FindMod(preview.m_nPublishedFileId);
      DebugUtil.DevAssert(mod != null, "expect mod with pending preview to be published");
      mod.previewImage = this.LoadPreviewImage(preview);
      if ((UnityEngine.Object) mod.previewImage != (UnityEngine.Object) null)
        loaded_previews.Add(mod);
      else if (1000 < this.retry_counts[preview.m_nPublishedFileId])
        cancelled_previews.Add(mod.fileId);
    }
    this.previews.RemoveWhere((Predicate<SteamUGCDetails_t>) (publish =>
    {
      if (!loaded_previews.Any<SteamUGCService.Mod>((Func<SteamUGCService.Mod, bool>) (mod => mod.fileId == publish.m_nPublishedFileId)))
        return cancelled_previews.Contains(publish.m_nPublishedFileId);
      return true;
    }));
    cancelled_previews.Recycle();
    ListPool<SteamUGCService.Mod, SteamUGCService>.PooledList pooledList1 = ListPool<SteamUGCService.Mod, SteamUGCService>.Allocate();
    HashSetPool<PublishedFileId_t, SteamUGCService>.PooledHashSet published = HashSetPool<PublishedFileId_t, SteamUGCService>.Allocate();
    foreach (SteamUGCDetails_t publish in this.publishes)
    {
      if (((int) SteamUGC.GetItemState(publish.m_nPublishedFileId) & 48) == 0)
      {
        Debug.LogFormat("publishing mod {0}", (object) publish.m_rgchTitle);
        SteamUGCService.Mod mod = new SteamUGCService.Mod(publish, this.LoadPreviewImage(publish));
        pooledList1.Add(mod);
        if (publish.m_hPreviewFile != UGCHandle_t.Invalid && (UnityEngine.Object) mod.previewImage == (UnityEngine.Object) null)
          this.previews.Add(publish);
        published.Add(publish.m_nPublishedFileId);
      }
    }
    this.publishes.RemoveWhere((Predicate<SteamUGCDetails_t>) (publish => published.Contains(publish.m_nPublishedFileId)));
    published.Recycle();
    foreach (PublishedFileId_t proxy in this.proxies)
    {
      Debug.LogFormat("proxy mod {0}", (object) proxy);
      pooledList1.Add(new SteamUGCService.Mod(proxy));
    }
    this.proxies.Clear();
    ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList2 = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
    ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList3 = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
    foreach (SteamUGCService.Mod mod1 in (List<SteamUGCService.Mod>) pooledList1)
    {
      SteamUGCService.Mod mod = mod1;
      int index = this.mods.FindIndex((Predicate<SteamUGCService.Mod>) (candidate => candidate.fileId == mod.fileId));
      if (index == -1)
      {
        this.mods.Add(mod);
        pooledList2.Add(mod.fileId);
      }
      else
      {
        this.mods[index] = mod;
        pooledList3.Add(mod.fileId);
      }
    }
    pooledList1.Recycle();
    bool flag = this.details_query == UGCQueryHandle_t.Invalid;
    if (pooledList2.Count != 0 || pooledList3.Count != 0 || flag && this.removals.Count != 0 || loaded_previews.Count != 0)
    {
      foreach (SteamUGCService.IClient client in this.clients)
        client.UpdateMods((IEnumerable<PublishedFileId_t>) pooledList2, (IEnumerable<PublishedFileId_t>) pooledList3, !flag ? Enumerable.Empty<PublishedFileId_t>() : (IEnumerable<PublishedFileId_t>) this.removals, (IEnumerable<SteamUGCService.Mod>) loaded_previews);
    }
    pooledList2.Recycle();
    pooledList3.Recycle();
    loaded_previews.Recycle();
    if (flag)
    {
      foreach (PublishedFileId_t removal1 in this.removals)
      {
        PublishedFileId_t removal = removal1;
        this.mods.RemoveAll((Predicate<SteamUGCService.Mod>) (candidate => candidate.fileId == removal));
      }
      this.removals.Clear();
    }
    foreach (PublishedFileId_t download in this.downloads)
    {
      EItemState itemState = (EItemState) SteamUGC.GetItemState(download);
      if (((itemState & EItemState.k_EItemStateInstalled) == EItemState.k_EItemStateNone || (itemState & EItemState.k_EItemStateNeedsUpdate) != EItemState.k_EItemStateNone) && (itemState & (EItemState.k_EItemStateDownloading | EItemState.k_EItemStateDownloadPending)) == EItemState.k_EItemStateNone)
        SteamUGC.DownloadItem(download, false);
    }
    if (!(this.details_query == UGCQueryHandle_t.Invalid))
      return;
    this.queries.UnionWith((IEnumerable<PublishedFileId_t>) this.downloads);
    this.downloads.Clear();
    if (this.queries.Count == 0)
      return;
    this.details_query = SteamUGC.CreateQueryUGCDetailsRequest(this.queries.ToArray<PublishedFileId_t>(), (uint) this.queries.Count);
    this.on_query_completed.Set(SteamUGC.SendQueryUGCRequest(this.details_query), (CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate) null);
  }

  private void OnSteamUGCQueryDetailsCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
  {
    switch (pCallback.m_eResult)
    {
      case EResult.k_EResultOK:
        for (uint index = 0; index < pCallback.m_unNumResultsReturned; ++index)
        {
          SteamUGCDetails_t pDetails = new SteamUGCDetails_t();
          SteamUGC.GetQueryUGCResult(this.details_query, index, out pDetails);
          if (!this.removals.Contains(pDetails.m_nPublishedFileId))
          {
            this.publishes.Add(pDetails);
            this.retry_counts[pDetails.m_nPublishedFileId] = 0;
          }
          this.queries.Remove(pDetails.m_nPublishedFileId);
        }
        break;
      case EResult.k_EResultBusy:
        Debug.Log((object) ("[OnSteamUGCQueryDetailsCompleted] - handle: " + (object) pCallback.m_handle + " -- Result: " + (object) pCallback.m_eResult + " Resending"));
        break;
      default:
        Debug.Log((object) ("[OnSteamUGCQueryDetailsCompleted] - handle: " + (object) pCallback.m_handle + " -- Result: " + (object) pCallback.m_eResult + " -- NUm results: " + (object) pCallback.m_unNumResultsReturned + " --Total Matching: " + (object) pCallback.m_unTotalMatchingResults + " -- cached: " + (object) pCallback.m_bCachedData));
        HashSet<PublishedFileId_t> proxies = this.proxies;
        this.proxies = this.queries;
        this.queries = proxies;
        break;
    }
    SteamUGC.ReleaseQueryUGCRequest(this.details_query);
    this.details_query = UGCQueryHandle_t.Invalid;
  }

  private void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t pCallback)
  {
    this.downloads.Add(pCallback.m_nPublishedFileId);
  }

  private void OnItemUpdated(RemoteStoragePublishedFileUpdated_t pCallback)
  {
    this.downloads.Add(pCallback.m_nPublishedFileId);
  }

  private void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t pCallback)
  {
    this.removals.Add(pCallback.m_nPublishedFileId);
  }

  public static byte[] GetBytesFromZip(
    PublishedFileId_t item,
    string[] filesToExtract,
    out System.DateTime lastModified,
    bool getFirstMatch = false)
  {
    byte[] numArray = (byte[]) null;
    lastModified = System.DateTime.MinValue;
    ulong punSizeOnDisk;
    string pchFolder;
    uint punTimeStamp;
    SteamUGC.GetItemInstallInfo(item, out punSizeOnDisk, out pchFolder, 1024U, out punTimeStamp);
    try
    {
      lastModified = File.GetLastWriteTimeUtc(pchFolder);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (ZipFile zipFile = ZipFile.Read(pchFolder))
        {
          ZipEntry zipEntry = (ZipEntry) null;
          foreach (string name in filesToExtract)
          {
            if (name.Length > 4)
            {
              if (zipFile.ContainsEntry(name))
                zipEntry = zipFile[name];
            }
            else
            {
              foreach (ZipEntry entry in (IEnumerable<ZipEntry>) zipFile.Entries)
              {
                if (entry.FileName.EndsWith(name))
                {
                  zipEntry = entry;
                  break;
                }
              }
            }
            if (zipEntry != null)
              break;
          }
          if (zipEntry != null)
          {
            zipEntry.Extract((Stream) memoryStream);
            memoryStream.Flush();
            numArray = memoryStream.ToArray();
          }
        }
      }
    }
    catch (Exception ex)
    {
    }
    return numArray;
  }

  public class Mod
  {
    public Texture2D previewImage;

    public Mod(SteamUGCDetails_t item, Texture2D previewImage)
    {
      this.title = item.m_rgchTitle;
      this.description = item.m_rgchDescription;
      this.fileId = item.m_nPublishedFileId;
      this.lastUpdateTime = (ulong) item.m_rtimeUpdated;
      this.tags = new List<string>((IEnumerable<string>) item.m_rgchTags.Split(','));
      this.previewImage = previewImage;
    }

    public Mod(PublishedFileId_t id)
    {
      this.title = string.Empty;
      this.description = string.Empty;
      this.fileId = id;
      this.lastUpdateTime = 0UL;
      this.tags = new List<string>();
      this.previewImage = (Texture2D) null;
    }

    public string title { get; private set; }

    public string description { get; private set; }

    public PublishedFileId_t fileId { get; private set; }

    public ulong lastUpdateTime { get; private set; }

    public List<string> tags { get; private set; }
  }

  public interface IClient
  {
    void UpdateMods(
      IEnumerable<PublishedFileId_t> added,
      IEnumerable<PublishedFileId_t> updated,
      IEnumerable<PublishedFileId_t> removed,
      IEnumerable<SteamUGCService.Mod> loaded_previews);
  }
}
