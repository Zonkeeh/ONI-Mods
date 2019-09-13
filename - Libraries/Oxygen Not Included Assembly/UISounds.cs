// Decompiled with JetBrains decompiler
// Type: UISounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class UISounds : KMonoBehaviour
{
  [SerializeField]
  private bool logSounds;
  [SerializeField]
  private UISounds.SoundData[] soundData;

  public static UISounds Instance { get; private set; }

  public static void DestroyInstance()
  {
    UISounds.Instance = (UISounds) null;
  }

  protected override void OnPrefabInit()
  {
    UISounds.Instance = this;
  }

  public static void PlaySound(UISounds.Sound sound)
  {
    UISounds.Instance.PlaySoundInternal(sound);
  }

  private void PlaySoundInternal(UISounds.Sound sound)
  {
    for (int index = 0; index < this.soundData.Length; ++index)
    {
      if (this.soundData[index].sound == sound)
      {
        if (this.logSounds)
          DebugUtil.LogArgs((object) "Play sound", (object) this.soundData[index].name);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.soundData[index].name, false));
      }
    }
  }

  public enum Sound
  {
    NegativeNotification,
    PositiveNotification,
    Select,
    Negative,
    Back,
    ClickObject,
    HUD_Mouseover,
    Object_Mouseover,
    ClickHUD,
  }

  [Serializable]
  private struct SoundData
  {
    public string name;
    public UISounds.Sound sound;
  }
}
