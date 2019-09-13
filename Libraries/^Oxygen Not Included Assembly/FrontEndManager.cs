// Decompiled with JetBrains decompiler
// Type: FrontEndManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FrontEndManager : KMonoBehaviour
{
  public static bool firstInit = true;
  public static FrontEndManager Instance;
  public GameObject[] SpawnOnLoadScreens;
  public GameObject[] SpawnOnLaunchScreens;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    FrontEndManager.Instance = this;
    if (this.SpawnOnLoadScreens != null && this.SpawnOnLoadScreens.Length != 0)
    {
      foreach (GameObject spawnOnLoadScreen in this.SpawnOnLoadScreens)
      {
        if ((Object) spawnOnLoadScreen != (Object) null)
          Util.KInstantiateUI(spawnOnLoadScreen, this.gameObject, true);
      }
    }
    if (!FrontEndManager.firstInit)
      return;
    FrontEndManager.firstInit = false;
    if (this.SpawnOnLaunchScreens == null || this.SpawnOnLoadScreens.Length == 0)
      return;
    foreach (GameObject spawnOnLaunchScreen in this.SpawnOnLaunchScreens)
    {
      if ((Object) spawnOnLaunchScreen != (Object) null)
        Util.KInstantiateUI(spawnOnLaunchScreen, this.gameObject, true);
    }
  }

  private void LateUpdate()
  {
    if (Debug.developerConsoleVisible)
      Debug.developerConsoleVisible = false;
    KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
    KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
    KAnimBatchManager.Instance().Render();
  }
}
