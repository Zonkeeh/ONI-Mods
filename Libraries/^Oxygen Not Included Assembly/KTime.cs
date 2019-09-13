// Decompiled with JetBrains decompiler
// Type: KTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class KTime : KMonoBehaviour
{
  public float UnscaledGameTime { get; set; }

  public static KTime Instance { get; private set; }

  public static void DestroyInstance()
  {
    KTime.Instance = (KTime) null;
  }

  protected override void OnPrefabInit()
  {
    KTime.Instance = this;
    this.UnscaledGameTime = Time.unscaledTime;
  }

  protected override void OnCleanUp()
  {
    KTime.Instance = (KTime) null;
  }

  public void Update()
  {
    if (SpeedControlScreen.Instance.IsPaused)
      return;
    this.UnscaledGameTime += Time.unscaledDeltaTime;
  }
}
