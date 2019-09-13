// Decompiled with JetBrains decompiler
// Type: SimpleNetworkCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class SimpleNetworkCache
{
  public static void LoadFromCacheOrDownload(
    string cache_id,
    string url,
    int version,
    UnityWebRequest data_wr,
    System.Action<UnityWebRequest> callback)
  {
    string cache_folder = Util.CacheFolder();
    string cache_prefix = System.IO.Path.Combine(cache_folder, cache_id);
    string version_filepath = cache_prefix + "_version";
    string data_filepath = cache_prefix + "_data";
    UnityWebRequest version_wr = new UnityWebRequest(new Uri(version_filepath, UriKind.Absolute), "GET", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) null);
    version_wr.SendWebRequest().completed += (System.Action<AsyncOperation>) (op =>
    {
      if (SimpleNetworkCache.GetVersionFromWebRequest(version_wr) == version)
      {
        data_wr.uri = new Uri(data_filepath, UriKind.Absolute);
        data_wr.SendWebRequest().completed += (System.Action<AsyncOperation>) (fileOp =>
        {
          if (!string.IsNullOrEmpty(data_wr.error))
          {
            Debug.LogWarning((object) ("Failure to read cached file: " + data_filepath));
            try
            {
              File.Delete(version_filepath);
              File.Delete(data_filepath);
            }
            catch
            {
              Debug.LogWarning((object) "Failed to delete cached files");
            }
          }
          callback(data_wr);
        });
      }
      else
      {
        data_wr.url = url;
        data_wr.SendWebRequest().completed += (System.Action<AsyncOperation>) (webOp =>
        {
          if (string.IsNullOrEmpty(data_wr.error))
          {
            try
            {
              Directory.CreateDirectory(cache_folder);
              File.WriteAllBytes(data_filepath, data_wr.downloadHandler.data);
              File.WriteAllText(version_filepath, version.ToString());
            }
            catch
            {
              Debug.LogWarning((object) ("Failed to write cache files to: " + cache_prefix));
            }
          }
          callback(data_wr);
        });
      }
      version_wr.Dispose();
    });
  }

  private static int GetVersionFromWebRequest(UnityWebRequest version_wr)
  {
    int result;
    if (!string.IsNullOrEmpty(version_wr.error) || version_wr.downloadHandler == null || !int.TryParse(version_wr.downloadHandler.text, out result))
      return -1;
    return result;
  }
}
