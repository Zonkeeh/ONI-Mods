// Decompiled with JetBrains decompiler
// Type: LaunchInitializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using System.Threading;
using UnityEngine;

public class LaunchInitializer : MonoBehaviour
{
  [SerializeField]
  private int numWaitFrames = 1;
  public const string BUILD_PREFIX = "LU";
  public GameObject[] SpawnPrefabs;

  private void Update()
  {
    if (this.numWaitFrames > Time.renderedFrameCount)
      return;
    if (!SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
      Debug.LogError((object) "Machine does not support RGBAFloat32");
    GraphicsOptionsScreen.SetResolutionFromPrefs();
    Util.ApplyInvariantCultureToThread(Thread.CurrentThread);
    Debug.Log((object) ("release Build: LU-" + 366134U.ToString()));
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    KPlayerPrefs.instance.Load();
    KFMOD.Initialize();
    for (int index = 0; index < this.SpawnPrefabs.Length; ++index)
    {
      GameObject spawnPrefab = this.SpawnPrefabs[index];
      if ((UnityEngine.Object) spawnPrefab != (UnityEngine.Object) null)
        Util.KInstantiate(spawnPrefab, this.gameObject, (string) null);
    }
    LaunchInitializer.DeleteLingeringFiles();
    this.enabled = false;
  }

  private static void DeleteLingeringFiles()
  {
    string[] strArray = new string[3]
    {
      "fmod.log",
      "load_stats_0.json",
      "OxygenNotIncluded_Data/output_log.txt"
    };
    string directoryName = System.IO.Path.GetDirectoryName(Application.dataPath);
    foreach (string path2 in strArray)
    {
      string path = System.IO.Path.Combine(directoryName, path2);
      try
      {
        if (File.Exists(path))
          File.Delete(path);
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ex);
      }
    }
  }
}
