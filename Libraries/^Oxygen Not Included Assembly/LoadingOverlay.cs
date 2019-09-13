// Decompiled with JetBrains decompiler
// Type: LoadingOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LoadingOverlay : KModalScreen
{
  private bool loadNextFrame;
  private bool showLoad;
  private System.Action loadCb;
  private static LoadingOverlay instance;

  protected override void OnPrefabInit()
  {
    this.pause = false;
    this.fadeIn = false;
    base.OnPrefabInit();
  }

  private void Update()
  {
    if (!this.loadNextFrame && this.showLoad)
    {
      this.loadNextFrame = true;
      this.showLoad = false;
    }
    else
    {
      if (!this.loadNextFrame)
        return;
      this.loadNextFrame = false;
      this.loadCb();
    }
  }

  public static void DestroyInstance()
  {
    LoadingOverlay.instance = (LoadingOverlay) null;
  }

  public static void Load(System.Action cb)
  {
    GameObject gameObject = GameObject.Find("/SceneInitializerFE/FrontEndManager");
    if ((UnityEngine.Object) LoadingOverlay.instance == (UnityEngine.Object) null)
    {
      LoadingOverlay.instance = Util.KInstantiateUI<LoadingOverlay>(ScreenPrefabs.Instance.loadingOverlay.gameObject, !((UnityEngine.Object) GameScreenManager.Instance == (UnityEngine.Object) null) ? GameScreenManager.Instance.ssOverlayCanvas : gameObject, false);
      LoadingOverlay.instance.GetComponentInChildren<LocText>().SetText((string) UI.FRONTEND.LOADING);
    }
    if ((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null)
    {
      LoadingOverlay.instance.transform.SetParent(GameScreenManager.Instance.ssOverlayCanvas.transform);
      LoadingOverlay.instance.transform.SetSiblingIndex(GameScreenManager.Instance.ssOverlayCanvas.transform.childCount - 1);
    }
    else
    {
      LoadingOverlay.instance.transform.SetParent(gameObject.transform);
      LoadingOverlay.instance.transform.SetSiblingIndex(gameObject.transform.childCount - 1);
    }
    LoadingOverlay.instance.loadCb = cb;
    LoadingOverlay.instance.showLoad = true;
    LoadingOverlay.instance.Activate();
  }

  public static void Clear()
  {
    if (!((UnityEngine.Object) LoadingOverlay.instance != (UnityEngine.Object) null))
      return;
    LoadingOverlay.instance.Deactivate();
  }
}
