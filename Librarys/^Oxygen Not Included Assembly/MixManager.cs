// Decompiled with JetBrains decompiler
// Type: MixManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public class MixManager : MonoBehaviour
{
  private void Update()
  {
    if (AudioMixer.instance == null || !AudioMixer.instance.persistentSnapshotsActive)
      return;
    AudioMixer.instance.UpdatePersistentSnapshotParameters();
  }

  private void OnApplicationFocus(bool hasFocus)
  {
    if (AudioMixer.instance == null || (Object) AudioMixerSnapshots.Get() == (Object) null)
      return;
    if (!hasFocus && KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1)
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
    else
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().GameNotFocusedSnapshot, STOP_MODE.ALLOWFADEOUT);
  }
}
