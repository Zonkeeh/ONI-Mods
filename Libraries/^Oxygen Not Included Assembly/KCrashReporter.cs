// Decompiled with JetBrains decompiler
// Type: KCrashReporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class KCrashReporter : MonoBehaviour
{
  public static string MOST_RECENT_SAVEFILE = (string) null;
  public static bool ignoreAll = false;
  public static bool debugWasUsed = false;
  public static bool haveActiveMods = false;
  public static string error_canvas_name = "ErrorCanvas";
  private static bool disableDeduping = false;
  private static readonly Regex failedToLoadModuleRegEx = new Regex("^Failed to load '(.*?)' with error (.*)", RegexOptions.Multiline);
  public static bool terminateOnError = true;
  private static readonly string[] IgnoreStrings = new string[3]
  {
    "Releasing render texture whose render buffer is set as Camera's target buffer with Camera.SetTargetBuffers!",
    "The profiler has run out of samples for this frame. This frame will be skipped. Increase the sample limit using Profiler.maxNumberOfSamplesPerFrame",
    "Trying to add Text (LocText) for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported."
  };
  public const string CRASH_REPORTER_SERVER = "http://crashes.klei.ca";
  private static bool hasReportedError;
  [SerializeField]
  private LoadScreen loadScreenPrefab;
  [SerializeField]
  private GameObject reportErrorPrefab;
  [SerializeField]
  private ConfirmDialogScreen confirmDialogPrefab;
  private ReportErrorDialog errorDialog;
  private static string dataRoot;

  public static event System.Action<string> onCrashReported;

  private void OnEnable()
  {
    KCrashReporter.dataRoot = Application.dataPath;
    Application.logMessageReceived += new Application.LogCallback(this.HandleLog);
    KCrashReporter.ignoreAll = true;
    string path = System.IO.Path.Combine(KCrashReporter.dataRoot, "hashes.json");
    if (System.IO.File.Exists(path))
    {
      StringBuilder stringBuilder = new StringBuilder();
      MD5 md5 = MD5.Create();
      Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(path));
      if (dictionary.Count > 0)
      {
        bool flag = true;
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
          string key = keyValuePair.Key;
          string str = keyValuePair.Value;
          stringBuilder.Length = 0;
          using (FileStream fileStream = new FileStream(System.IO.Path.Combine(KCrashReporter.dataRoot, key), FileMode.Open, FileAccess.Read))
          {
            foreach (byte num in md5.ComputeHash((Stream) fileStream))
              stringBuilder.AppendFormat("{0:x2}", (object) num);
            if (stringBuilder.ToString() != str)
            {
              flag = false;
              break;
            }
          }
        }
        if (flag)
          KCrashReporter.ignoreAll = false;
      }
      else
        KCrashReporter.ignoreAll = false;
    }
    else
      KCrashReporter.ignoreAll = false;
    if (KCrashReporter.ignoreAll)
      Debug.Log((object) "Ignoring crash due to mismatched hashes.json entries.");
    if (System.IO.File.Exists("ignorekcrashreporter.txt"))
    {
      KCrashReporter.ignoreAll = true;
      Debug.Log((object) "Ignoring crash due to ignorekcrashreporter.txt");
    }
    if (!Application.isEditor || GenericGameSettings.instance.enableEditorCrashReporting)
      return;
    KCrashReporter.terminateOnError = false;
  }

  private void OnDisable()
  {
  }

  private void HandleLog(string msg, string stack_trace, LogType type)
  {
    if (KCrashReporter.ignoreAll || Array.IndexOf<string>(KCrashReporter.IgnoreStrings, msg) != -1 || msg != null && msg.StartsWith("<RI.Hid>"))
      return;
    if (type == LogType.Exception)
      RestartWarning.ShouldWarn = true;
    if (!((UnityEngine.Object) this.errorDialog == (UnityEngine.Object) null) || type != LogType.Exception && type != LogType.Error || KCrashReporter.terminateOnError && ReportErrorDialog.hasCrash)
      return;
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      SpeedControlScreen.Instance.Pause(true);
    string error = msg;
    string stack_trace1 = stack_trace;
    if (string.IsNullOrEmpty(stack_trace1))
      stack_trace1 = new StackTrace(5, true).ToString();
    if (App.isLoading)
    {
      if (SceneInitializerLoader.deferred_error.IsValid)
        return;
      SceneInitializerLoader.deferred_error = new SceneInitializerLoader.DeferredError()
      {
        msg = error,
        stack_trace = stack_trace1
      };
    }
    else
      this.ShowDialog(error, stack_trace1);
  }

  public bool ShowDialog(string error, string stack_trace)
  {
    if ((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null && Global.Instance.modManager != null && Global.Instance.modManager.HaveLoadedMods())
    {
      Global.Instance.modManager.HandleCrash();
      return true;
    }
    if ((UnityEngine.Object) this.errorDialog != (UnityEngine.Object) null)
      return false;
    GameObject gameObject1 = GameObject.Find(KCrashReporter.error_canvas_name);
    if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
    {
      gameObject1 = new GameObject();
      gameObject1.name = KCrashReporter.error_canvas_name;
      Canvas canvas = gameObject1.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
      gameObject1.AddComponent<GraphicRaycaster>();
    }
    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.reportErrorPrefab, Vector3.zero, Quaternion.identity);
    gameObject2.transform.SetParent(gameObject1.transform, false);
    this.errorDialog = gameObject2.GetComponentInChildren<ReportErrorDialog>();
    this.errorDialog.PopupConfirmDialog((System.Action) (() =>
    {
      string save_file_hash = (string) null;
      if (KCrashReporter.MOST_RECENT_SAVEFILE != null)
        save_file_hash = KCrashReporter.UploadSaveFile(KCrashReporter.MOST_RECENT_SAVEFILE, stack_trace, (Dictionary<string, string>) null);
      KCrashReporter.ReportError(error, stack_trace, save_file_hash, this.confirmDialogPrefab, this.errorDialog.UserMessage());
    }), new System.Action(this.OnQuitToDesktop), new System.Action(this.OnCloseErrorDialog));
    return true;
  }

  private void OnCloseErrorDialog()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.errorDialog.gameObject);
    this.errorDialog = (ReportErrorDialog) null;
    if (!((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null))
      return;
    SpeedControlScreen.Instance.Unpause(true);
  }

  private void OnQuitToDesktop()
  {
    App.Quit();
  }

  private static string UploadSaveFile(
    string save_file,
    string stack_trace,
    Dictionary<string, string> metadata = null)
  {
    Debug.Log((object) string.Format("Save_file: {0}", (object) save_file));
    if (KPrivacyPrefs.instance.disableDataCollection || save_file == null || !System.IO.File.Exists(save_file))
      return string.Empty;
    using (WebClient webClient = new WebClient())
    {
      Encoding utF8 = Encoding.UTF8;
      webClient.Encoding = utF8;
      byte[] buffer = System.IO.File.ReadAllBytes(save_file);
      string str1 = "----" + System.DateTime.Now.Ticks.ToString("x");
      webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + str1);
      string empty = string.Empty;
      string str2;
      using (SHA1CryptoServiceProvider cryptoServiceProvider = new SHA1CryptoServiceProvider())
        str2 = BitConverter.ToString(cryptoServiceProvider.ComputeHash(buffer)).Replace("-", string.Empty);
      string str3 = empty + string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", (object) str1, (object) "hash", (object) str2);
      if (metadata != null)
      {
        string str4 = JsonConvert.SerializeObject((object) metadata);
        str3 += string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", (object) str1, (object) nameof (metadata), (object) str4);
      }
      string s1 = str3 + string.Format("--{0}\r\nContent-Disposition: form-data; name=\"save\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", new object[3]
      {
        (object) str1,
        (object) save_file,
        (object) "application/x-spss-sav"
      });
      byte[] bytes1 = utF8.GetBytes(s1);
      string s2 = string.Format("\r\n--{0}--\r\n", (object) str1);
      byte[] bytes2 = utF8.GetBytes(s2);
      byte[] data = new byte[bytes1.Length + buffer.Length + bytes2.Length];
      Buffer.BlockCopy((Array) bytes1, 0, (Array) data, 0, bytes1.Length);
      Buffer.BlockCopy((Array) buffer, 0, (Array) data, bytes1.Length, buffer.Length);
      Buffer.BlockCopy((Array) bytes2, 0, (Array) data, bytes1.Length + buffer.Length, bytes2.Length);
      Uri address = new Uri("http://crashes.klei.ca/submitSave");
      try
      {
        webClient.UploadData(address, "POST", data);
        return str2;
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex);
        return string.Empty;
      }
    }
  }

  private static string GetUserID()
  {
    if (!DistributionPlatform.Initialized)
      return Environment.UserName;
    return DistributionPlatform.Inst.Name + "ID_" + DistributionPlatform.Inst.LocalUser.Name + "_" + (object) DistributionPlatform.Inst.LocalUser.Id;
  }

  private static string GetLogContents()
  {
    string empty = string.Empty;
    string path;
    switch (Application.platform)
    {
      case RuntimePlatform.OSXEditor:
        path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Logs/Unity/Editor.log");
        break;
      case RuntimePlatform.OSXPlayer:
        path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Logs/Unity/Player.log");
        break;
      case RuntimePlatform.WindowsPlayer:
        path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../LocalLow/Klei/Oxygen Not Included/output_log.txt");
        break;
      case RuntimePlatform.WindowsEditor:
        path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity/Editor/Editor.log");
        break;
      case RuntimePlatform.LinuxPlayer:
        path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "unity3d/Klei/Oxygen Not Included/Player.log");
        break;
      default:
        return string.Empty;
    }
    if (!System.IO.File.Exists(path))
      return string.Empty;
    using (FileStream fileStream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        return streamReader.ReadToEnd();
    }
  }

  public static void ReportError(
    string msg,
    string stack_trace,
    string save_file_hash,
    ConfirmDialogScreen confirm_prefab,
    string userMessage = "")
  {
    if (KCrashReporter.ignoreAll)
      return;
    Debug.Log((object) "Reporting error.\n");
    if (msg != null)
      Debug.Log((object) msg);
    if (stack_trace != null)
      Debug.Log((object) stack_trace);
    KCrashReporter.hasReportedError = true;
    if (KPrivacyPrefs.instance.disableDataCollection)
      return;
    string str1;
    using (WebClient webClient = new WebClient())
    {
      webClient.Encoding = Encoding.UTF8;
      if (string.IsNullOrEmpty(msg))
        msg = "No message";
      Match match = KCrashReporter.failedToLoadModuleRegEx.Match(msg);
      if (match.Success)
        msg = "Failed to load '" + System.IO.Path.GetFileName(match.Groups[1].ToString()) + "' with error '" + match.Groups[2].ToString() + "'.";
      if (string.IsNullOrEmpty(stack_trace))
        stack_trace = string.Format("No stack trace {0}\n\n{1}", (object) ("LU-" + 366134U.ToString()), (object) msg);
      List<string> stringList = new List<string>();
      if (KCrashReporter.debugWasUsed)
        stringList.Add("(Debug Used)");
      if (KCrashReporter.haveActiveMods)
        stringList.Add("(Mods Active)");
      stringList.Add(msg);
      string[] strArray = new string[8]
      {
        "Debug:LogError",
        "UnityEngine.Debug",
        "Output:LogError",
        "DebugUtil:Assert",
        "System.Array",
        "System.Collections",
        "KCrashReporter.Assert",
        "No stack trace."
      };
      string str2 = stack_trace;
      char[] chArray = new char[1]{ '\n' };
      foreach (string str3 in str2.Split(chArray))
      {
        if (stringList.Count < 5)
        {
          if (!string.IsNullOrEmpty(str3))
          {
            bool flag = false;
            foreach (string str4 in strArray)
            {
              if (str3.StartsWith(str4))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              stringList.Add(str3);
          }
        }
        else
          break;
      }
      if (userMessage == STRINGS.UI.CRASHSCREEN.BODY.text)
        userMessage = string.Empty;
      KCrashReporter.Error error = new KCrashReporter.Error();
      error.user = KCrashReporter.GetUserID();
      error.callstack = stack_trace;
      if (KCrashReporter.disableDeduping)
        error.callstack = error.callstack + "\n" + Guid.NewGuid().ToString();
      error.fullstack = string.Format("{0}\n\n{1}", (object) msg, (object) stack_trace);
      error.build = 366134;
      error.log = KCrashReporter.GetLogContents();
      error.summaryline = string.Join("\n", stringList.ToArray());
      error.user_message = userMessage;
      if (!string.IsNullOrEmpty(save_file_hash))
        error.save_hash = save_file_hash;
      if (DistributionPlatform.Initialized)
        error.steam64_verified = DistributionPlatform.Inst.LocalUser.Id.ToInt64();
      string data = JsonConvert.SerializeObject((object) error);
      string empty = string.Empty;
      Uri address = new Uri("http://crashes.klei.ca/submitCrash");
      Debug.Log((object) "Submitting crash:");
      try
      {
        webClient.UploadStringAsync(address, data);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex);
      }
      if ((UnityEngine.Object) confirm_prefab != (UnityEngine.Object) null)
        ((ConfirmDialogScreen) KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, (GameObject) null)).PopupConfirmDialog("Reported Error", (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
      str1 = empty;
    }
    if (KCrashReporter.onCrashReported == null)
      return;
    KCrashReporter.onCrashReported(str1);
  }

  public static void ReportBug(string msg, string save_file)
  {
    string stack_trace = "Bug Report From: " + KCrashReporter.GetUserID() + " at " + System.DateTime.Now.ToString();
    string save_file_hash = KCrashReporter.UploadSaveFile(save_file, stack_trace, new Dictionary<string, string>()
    {
      {
        "user",
        KCrashReporter.GetUserID()
      }
    });
    KCrashReporter.ReportError(msg, stack_trace, save_file_hash, ScreenPrefabs.Instance.ConfirmDialogScreen, string.Empty);
  }

  public static void Assert(bool condition, string message)
  {
    if (condition || KCrashReporter.hasReportedError)
      return;
    StackTrace stackTrace = new StackTrace(1, true);
    KCrashReporter.ReportError("ASSERT: " + message, stackTrace.ToString(), (string) null, (ConfirmDialogScreen) null, string.Empty);
  }

  public static void ReportSimDLLCrash(string msg, string stack_trace, string dmp_filename)
  {
    if (KCrashReporter.hasReportedError)
      return;
    string save_file_hash = (string) null;
    string str1 = (string) null;
    string str2 = (string) null;
    if (dmp_filename != null)
    {
      string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(dmp_filename);
      str1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(KCrashReporter.dataRoot), dmp_filename);
      str2 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(KCrashReporter.dataRoot), withoutExtension + ".sav");
      System.IO.File.Move(str1, str2);
      save_file_hash = KCrashReporter.UploadSaveFile(str2, stack_trace, new Dictionary<string, string>()
      {
        {
          "user",
          KCrashReporter.GetUserID()
        }
      });
    }
    KCrashReporter.ReportError(msg, stack_trace, save_file_hash, (ConfirmDialogScreen) null, string.Empty);
    if (dmp_filename == null)
      return;
    System.IO.File.Move(str2, str1);
  }

  private class Error
  {
    public string game = "simgame";
    public int build = -1;
    public string platform = Environment.OSVersion.ToString();
    public string user = "unknown";
    public string callstack = string.Empty;
    public string fullstack = string.Empty;
    public string log = string.Empty;
    public string summaryline = string.Empty;
    public string user_message = string.Empty;
    public string save_hash = string.Empty;
    public ulong steam64_verified;
    public bool is_server;
    public bool is_dedicated;
  }
}
