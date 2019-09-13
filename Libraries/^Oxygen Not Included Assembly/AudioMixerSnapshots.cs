// Decompiled with JetBrains decompiler
// Type: AudioMixerSnapshots
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixerSnapshots : ScriptableObject
{
  [NonSerialized]
  public List<string> snapshotMap = new List<string>();
  [EventRef]
  public string TechFilterOnMigrated;
  [EventRef]
  public string TechFilterLogicOn;
  [EventRef]
  public string NightStartedMigrated;
  [EventRef]
  public string MenuOpenMigrated;
  [EventRef]
  public string MenuOpenHalfEffect;
  [EventRef]
  public string SpeedPausedMigrated;
  [EventRef]
  public string DuplicantCountAttenuatorMigrated;
  [EventRef]
  public string NewBaseSetupSnapshot;
  [EventRef]
  public string FrontEndSnapshot;
  [EventRef]
  public string FrontEndWelcomeScreenSnapshot;
  [EventRef]
  public string FrontEndWorldGenerationSnapshot;
  [EventRef]
  public string IntroNIS;
  [EventRef]
  public string PulseSnapshot;
  [EventRef]
  public string ESCPauseSnapshot;
  [EventRef]
  public string MENUNewDuplicantSnapshot;
  [EventRef]
  public string UserVolumeSettingsSnapshot;
  [EventRef]
  public string DuplicantCountMovingSnapshot;
  [EventRef]
  public string DuplicantCountSleepingSnapshot;
  [EventRef]
  public string PortalLPDimmedSnapshot;
  [EventRef]
  public string DynamicMusicPlayingSnapshot;
  [EventRef]
  public string FabricatorSideScreenOpenSnapshot;
  [EventRef]
  public string SpaceVisibleSnapshot;
  [EventRef]
  public string MENUStarmapSnapshot;
  [EventRef]
  public string GameNotFocusedSnapshot;
  [EventRef]
  public string FacilityVisibleSnapshot;
  [EventRef]
  public string TutorialVideoPlayingSnapshot;
  [EventRef]
  public string VictoryMessageSnapshot;
  [EventRef]
  public string VictoryNISGenericSnapshot;
  [EventRef]
  public string VictoryNISRocketSnapshot;
  [EventRef]
  public string VictoryCinematicSnapshot;
  [EventRef]
  public string VictoryFadeToBlackSnapshot;
  [EventRef]
  public string MuteDynamicMusicSnapshot;
  [SerializeField]
  [EventRef]
  private string[] snapshots;
  private static AudioMixerSnapshots instance;

  public static AudioMixerSnapshots Get()
  {
    if ((UnityEngine.Object) AudioMixerSnapshots.instance == (UnityEngine.Object) null)
      AudioMixerSnapshots.instance = Resources.Load<AudioMixerSnapshots>(nameof (AudioMixerSnapshots));
    return AudioMixerSnapshots.instance;
  }

  [ContextMenu("Reload")]
  public void ReloadSnapshots()
  {
    this.snapshotMap.Clear();
    foreach (string snapshot in this.snapshots)
      this.snapshotMap.Add(snapshot);
  }
}
