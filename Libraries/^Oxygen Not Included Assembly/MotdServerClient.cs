// Decompiled with JetBrains decompiler
// Type: MotdServerClient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class MotdServerClient
{
  private const string MotdLocalImagePath = "motd_local/image.png";
  private System.Action<MotdServerClient.MotdResponse, string> m_callback;

  private static string MotdServerUrl
  {
    get
    {
      return "https://klei-motd.s3.amazonaws.com/oni/" + MotdServerClient.GetLocalePathSuffix();
    }
  }

  private static string MotdLocalPath
  {
    get
    {
      return "motd_local/" + MotdServerClient.GetLocalePathSuffix();
    }
  }

  private static string GetLocalePathSuffix()
  {
    string str = string.Empty;
    Localization.Locale locale = Localization.GetLocale();
    if (locale != null)
    {
      switch (locale.Lang)
      {
        case Localization.Language.Chinese:
        case Localization.Language.Korean:
        case Localization.Language.Russian:
          str = locale.Code + "/";
          break;
      }
    }
    return str + "motd.json";
  }

  public void GetMotd(System.Action<MotdServerClient.MotdResponse, string> cb)
  {
    this.m_callback = cb;
    MotdServerClient.MotdResponse localResponse = this.GetLocalMotd(MotdServerClient.MotdLocalPath);
    this.GetWebMotd(MotdServerClient.MotdServerUrl, localResponse, (System.Action<MotdServerClient.MotdResponse, string>) ((response, err) =>
    {
      if (err == null)
      {
        this.doCallback(response, err);
      }
      else
      {
        Debug.LogWarning((object) ("Could not retrieve web motd from " + MotdServerClient.MotdServerUrl + ", falling back to local - err: " + err));
        this.doCallback(localResponse, (string) null);
      }
    }));
  }

  private MotdServerClient.MotdResponse GetLocalMotd(string filePath)
  {
    MotdServerClient.MotdResponse motdResponse = JsonConvert.DeserializeObject<MotdServerClient.MotdResponse>(Resources.Load<TextAsset>(filePath.Replace(".json", string.Empty)).ToString());
    motdResponse.image_texture = Resources.Load<Texture2D>("motd_local/image.png");
    return motdResponse;
  }

  private void GetWebMotd(
    string url,
    MotdServerClient.MotdResponse localMotd,
    System.Action<MotdServerClient.MotdResponse, string> cb)
  {
    System.Action<string, string> cb1 = (System.Action<string, string>) ((response, err) =>
    {
      if (err != null)
      {
        cb((MotdServerClient.MotdResponse) null, err);
      }
      else
      {
        MotdServerClient.MotdResponse responseStruct = JsonConvert.DeserializeObject<MotdServerClient.MotdResponse>(response, new JsonSerializerSettings()
        {
          Error = (System.EventHandler<ErrorEventArgs>) ((sender, args) => args.ErrorContext.Handled = true)
        });
        if (responseStruct == null)
          cb((MotdServerClient.MotdResponse) null, "Invalid json from server:" + response);
        else if (responseStruct.version <= localMotd.version)
        {
          Debug.Log((object) ("Using local MOTD at version: " + (object) localMotd.version + ", web version at " + (object) responseStruct.version));
          cb(localMotd, (string) null);
        }
        else
          SimpleNetworkCache.LoadFromCacheOrDownload("motd_image", responseStruct.image_url, responseStruct.image_version, new UnityWebRequest()
          {
            downloadHandler = (DownloadHandler) new DownloadHandlerTexture()
          }, (System.Action<UnityWebRequest>) (wr =>
          {
            string str = (string) null;
            if (string.IsNullOrEmpty(wr.error))
            {
              Debug.Log((object) ("Using web MOTD at version: " + (object) responseStruct.version + ", local version at " + (object) localMotd.version));
              responseStruct.image_texture = DownloadHandlerTexture.GetContent(wr);
            }
            else
              str = "SimpleNetworkCache - " + wr.error;
            cb(responseStruct, str);
            wr.Dispose();
          }));
      }
    });
    this.getAsyncRequest(url, cb1);
  }

  private void getAsyncRequest(string url, System.Action<string, string> cb)
  {
    UnityWebRequest motdRequest = UnityWebRequest.Get(url);
    motdRequest.SetRequestHeader("Content-Type", "application/json");
    motdRequest.SendWebRequest().completed += (System.Action<AsyncOperation>) (operation =>
    {
      cb(motdRequest.downloadHandler.text, motdRequest.error);
      motdRequest.Dispose();
    });
  }

  public void UnregisterCallback()
  {
    this.m_callback = (System.Action<MotdServerClient.MotdResponse, string>) null;
  }

  private void doCallback(MotdServerClient.MotdResponse response, string error)
  {
    if (this.m_callback != null)
      this.m_callback(response, error);
    else
      Debug.Log((object) "Motd Response receieved, but callback was unregistered");
  }

  public class MotdResponse
  {
    public int version { get; set; }

    public string image_header_text { get; set; }

    public int image_version { get; set; }

    public string image_url { get; set; }

    public string image_link_url { get; set; }

    public string news_header_text { get; set; }

    public string news_body_text { get; set; }

    public string patch_notes_summary { get; set; }

    public string patch_notes_link_url { get; set; }

    public string last_update_time { get; set; }

    public string next_update_time { get; set; }

    public string update_text_override { get; set; }

    public Texture2D image_texture { get; set; }
  }
}
