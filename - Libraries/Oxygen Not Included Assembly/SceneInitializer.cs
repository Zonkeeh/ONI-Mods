// Decompiled with JetBrains decompiler
// Type: SceneInitializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
  public List<GameObject> preloadPrefabs = new List<GameObject>();
  public List<GameObject> prefabs = new List<GameObject>();
  public const int MAXDEPTH = -30000;
  public const int SCREENDEPTH = -1000;
  public GameObject prefab_NewSaveGame;

  public static SceneInitializer Instance { get; private set; }

  private void Awake()
  {
    Localization.SwapToLocalizedFont();
    string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
    string str = Application.dataPath + (object) System.IO.Path.DirectorySeparatorChar + "Plugins";
    if (!environmentVariable.Contains(str))
      Environment.SetEnvironmentVariable("PATH", environmentVariable + (object) System.IO.Path.PathSeparator + str, EnvironmentVariableTarget.Process);
    SceneInitializer.Instance = this;
    this.PreLoadPrefabs();
  }

  private void OnDestroy()
  {
    SceneInitializer.Instance = (SceneInitializer) null;
  }

  private void PreLoadPrefabs()
  {
    foreach (GameObject preloadPrefab in this.preloadPrefabs)
    {
      if ((UnityEngine.Object) preloadPrefab != (UnityEngine.Object) null)
        Util.KInstantiate(preloadPrefab, preloadPrefab.transform.GetPosition(), Quaternion.identity, this.gameObject, (string) null, true, 0);
    }
  }

  public void NewSaveGamePrefab()
  {
    if (!((UnityEngine.Object) this.prefab_NewSaveGame != (UnityEngine.Object) null) || !((UnityEngine.Object) SaveGame.Instance == (UnityEngine.Object) null))
      return;
    Util.KInstantiate(this.prefab_NewSaveGame, this.gameObject, (string) null);
  }

  public void PostLoadPrefabs()
  {
    foreach (GameObject prefab in this.prefabs)
    {
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        Util.KInstantiate(prefab, this.gameObject, (string) null);
    }
  }
}
