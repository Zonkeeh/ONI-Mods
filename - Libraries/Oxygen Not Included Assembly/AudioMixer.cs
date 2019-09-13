// Decompiled with JetBrains decompiler
// Type: AudioMixer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer
{
  private static AudioMixer _instance = (AudioMixer) null;
  public static float LOW_PRIORITY_CUTOFF_DISTANCE = 10f;
  public static float PULSE_SNAPSHOT_BPM = 120f;
  public static int VISIBLE_DUPLICANTS_BEFORE_ATTENUATION = 2;
  private static readonly HashedString UserVolumeSettingsHash = new HashedString("event:/Snapshots/Mixing/Snapshot_UserVolumeSettings");
  public Dictionary<HashedString, EventInstance> activeSnapshots = new Dictionary<HashedString, EventInstance>();
  public List<HashedString> SnapshotDebugLog = new List<HashedString>();
  private Dictionary<string, int> visibleDupes = new Dictionary<string, int>();
  public Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = new Dictionary<string, AudioMixer.UserVolumeBus>();
  private const string DUPLICANT_COUNT_ID = "duplicantCount";
  private const string PULSE_ID = "Pulse";
  private const string SNAPSHOT_ACTIVE_ID = "snapshotActive";
  private const string SPACE_VISIBLE_ID = "spaceVisible";
  private const string FACILITY_VISIBLE_ID = "facilityVisible";
  public bool activeNIS;
  private EventInstance duplicantCountInst;
  private EventInstance pulseInst;
  private EventInstance duplicantCountMovingInst;
  private EventInstance duplicantCountSleepingInst;
  private EventInstance spaceVisibleInst;
  private EventInstance facilityVisibleInst;
  public bool persistentSnapshotsActive;

  public static AudioMixer instance
  {
    get
    {
      return AudioMixer._instance;
    }
  }

  public static AudioMixer Create()
  {
    AudioMixer._instance = new AudioMixer();
    AudioMixerSnapshots audioMixerSnapshots = AudioMixerSnapshots.Get();
    if ((Object) audioMixerSnapshots != (Object) null)
      audioMixerSnapshots.ReloadSnapshots();
    return AudioMixer._instance;
  }

  public static void Destroy()
  {
    AudioMixer._instance.StopAll(STOP_MODE.IMMEDIATE);
    AudioMixer._instance = (AudioMixer) null;
  }

  public EventInstance Start(string snapshot)
  {
    EventInstance eventInstance;
    if (!this.activeSnapshots.TryGetValue((HashedString) snapshot, out eventInstance))
    {
      if (RuntimeManager.IsInitialized)
      {
        eventInstance = KFMOD.CreateInstance(snapshot);
        this.activeSnapshots[(HashedString) snapshot] = eventInstance;
        int num1 = (int) eventInstance.start();
        int num2 = (int) eventInstance.setParameterValue("snapshotActive", 1f);
      }
      else
        eventInstance = new EventInstance();
    }
    AudioMixer.instance.Log("Start Snapshot: " + snapshot);
    return eventInstance;
  }

  public bool Stop(HashedString snapshot, STOP_MODE stop_mode = STOP_MODE.ALLOWFADEOUT)
  {
    bool flag = false;
    EventInstance eventInstance;
    if (this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
    {
      int num1 = (int) eventInstance.setParameterValue("snapshotActive", 0.0f);
      int num2 = (int) eventInstance.stop(stop_mode);
      int num3 = (int) eventInstance.release();
      this.activeSnapshots.Remove(snapshot);
      flag = true;
      AudioMixer.instance.Log("Stop Snapshot: [" + (object) snapshot + "] with fadeout mode: [" + (object) stop_mode + "]");
    }
    else
      AudioMixer.instance.Log("Tried to stop snapshot: [" + (object) snapshot + "] but it wasn't active.");
    return flag;
  }

  public void Reset()
  {
    this.StopAll(STOP_MODE.IMMEDIATE);
  }

  public void StopAll(STOP_MODE stop_mode = STOP_MODE.IMMEDIATE)
  {
    List<HashedString> hashedStringList = new List<HashedString>();
    foreach (KeyValuePair<HashedString, EventInstance> activeSnapshot in this.activeSnapshots)
    {
      if (activeSnapshot.Key != AudioMixer.UserVolumeSettingsHash)
        hashedStringList.Add(activeSnapshot.Key);
    }
    for (int index = 0; index < hashedStringList.Count; ++index)
      this.Stop(hashedStringList[index], stop_mode);
  }

  public bool SnapshotIsActive(HashedString snapshot_name)
  {
    return this.activeSnapshots.ContainsKey(snapshot_name);
  }

  public void SetSnapshotParameter(
    string snapshot_name,
    string parameter_name,
    float parameter_value,
    bool shouldLog = true)
  {
    if (shouldLog)
      this.Log(string.Format("Set Param {0}: {1}, {2}", (object) snapshot_name, (object) parameter_name, (object) parameter_value));
    EventInstance eventInstance;
    if (this.activeSnapshots.TryGetValue((HashedString) snapshot_name, out eventInstance))
    {
      int num = (int) eventInstance.setParameterValue(parameter_name, parameter_value);
    }
    else
      this.Log("Tried to set [" + parameter_name + "] to [" + (object) parameter_value + "] but [" + snapshot_name + "] is not active.");
  }

  public void StartPersistentSnapshots()
  {
    this.persistentSnapshotsActive = true;
    this.Start(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
    this.Start(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
    this.Start(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
    this.spaceVisibleInst = this.Start(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
    this.facilityVisibleInst = this.Start(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
    this.Start(AudioMixerSnapshots.Get().PulseSnapshot);
  }

  public void StopPersistentSnapshots()
  {
    this.persistentSnapshotsActive = false;
    this.Stop((HashedString) AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated, STOP_MODE.ALLOWFADEOUT);
    this.Stop((HashedString) AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot, STOP_MODE.ALLOWFADEOUT);
    this.Stop((HashedString) AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot, STOP_MODE.ALLOWFADEOUT);
    this.Stop((HashedString) AudioMixerSnapshots.Get().SpaceVisibleSnapshot, STOP_MODE.ALLOWFADEOUT);
    this.Stop((HashedString) AudioMixerSnapshots.Get().FacilityVisibleSnapshot, STOP_MODE.ALLOWFADEOUT);
    this.Stop((HashedString) AudioMixerSnapshots.Get().PulseSnapshot, STOP_MODE.ALLOWFADEOUT);
  }

  public void UpdatePersistentSnapshotParameters()
  {
    this.SetVisibleDuplicants();
    if (this.activeSnapshots.TryGetValue((HashedString) AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot, out this.duplicantCountMovingInst))
    {
      int num1 = (int) this.duplicantCountMovingInst.setParameterValue("duplicantCount", (float) Mathf.Max(0, this.visibleDupes["moving"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION));
    }
    if (this.activeSnapshots.TryGetValue((HashedString) AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot, out this.duplicantCountSleepingInst))
    {
      int num2 = (int) this.duplicantCountSleepingInst.setParameterValue("duplicantCount", (float) Mathf.Max(0, this.visibleDupes["sleeping"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION));
    }
    if (this.activeSnapshots.TryGetValue((HashedString) AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated, out this.duplicantCountInst))
    {
      int num3 = (int) this.duplicantCountInst.setParameterValue("duplicantCount", (float) Mathf.Max(0, this.visibleDupes["visible"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION));
    }
    if (!this.activeSnapshots.TryGetValue((HashedString) AudioMixerSnapshots.Get().PulseSnapshot, out this.pulseInst))
      return;
    float num4 = AudioMixer.PULSE_SNAPSHOT_BPM / 60f;
    switch (SpeedControlScreen.Instance.GetSpeed())
    {
      case 1:
        num4 /= 2f;
        break;
      case 2:
        num4 /= 3f;
        break;
    }
    int num5 = (int) this.pulseInst.setParameterValue("Pulse", Mathf.Abs(Mathf.Sin(Time.time * 3.141593f * num4)));
  }

  public void UpdateSpaceVisibleSnapshot(float percent)
  {
    int num = (int) this.spaceVisibleInst.setParameterValue("spaceVisible", percent);
  }

  public void UpdateFacilityVisibleSnapshot(float percent)
  {
    int num = (int) this.facilityVisibleInst.setParameterValue("facilityVisible", percent);
  }

  private void SetVisibleDuplicants()
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    for (int index = 0; index < Components.LiveMinionIdentities.Count; ++index)
    {
      if (CameraController.Instance.IsVisiblePos(Components.LiveMinionIdentities[index].transform.GetPosition()))
      {
        ++num1;
        Navigator component = Components.LiveMinionIdentities[index].GetComponent<Navigator>();
        if ((Object) component != (Object) null && component.IsMoving())
        {
          ++num2;
        }
        else
        {
          StaminaMonitor.Instance smi = Components.LiveMinionIdentities[index].GetComponent<Worker>().GetSMI<StaminaMonitor.Instance>();
          if (smi != null && smi.IsSleeping())
            ++num3;
        }
      }
    }
    this.visibleDupes["visible"] = num1;
    this.visibleDupes["moving"] = num2;
    this.visibleDupes["sleeping"] = num3;
  }

  public void StartUserVolumesSnapshot()
  {
    this.Start(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
    EventInstance eventInstance;
    if (!this.activeSnapshots.TryGetValue((HashedString) AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot, out eventInstance))
      return;
    EventDescription description1;
    int description2 = (int) eventInstance.getDescription(out description1);
    USER_PROPERTY property;
    int userProperty = (int) description1.getUserProperty("buses", out property);
    string[] strArray = property.stringValue().Split('-');
    for (int index = 0; index < strArray.Length; ++index)
    {
      float num = 1f;
      string key = "Volume_" + strArray[index];
      if (KPlayerPrefs.HasKey(key))
        num = KPlayerPrefs.GetFloat(key);
      AudioMixer.UserVolumeBus userVolumeBus = new AudioMixer.UserVolumeBus();
      userVolumeBus.busLevel = num;
      userVolumeBus.labelString = (string) Strings.Get("STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUDIO_BUS_" + strArray[index].ToUpper());
      this.userVolumeSettings.Add(strArray[index], userVolumeBus);
      this.SetUserVolume(strArray[index], userVolumeBus.busLevel);
    }
  }

  public void SetUserVolume(string bus, float value)
  {
    if (!this.userVolumeSettings.ContainsKey(bus))
    {
      Debug.LogError((object) "The provided bus doesn't exist. Check yo'self fool!");
    }
    else
    {
      if ((double) value > 1.0)
        value = 1f;
      else if ((double) value < 0.0)
        value = 0.0f;
      this.userVolumeSettings[bus].busLevel = value;
      KPlayerPrefs.SetFloat("Volume_" + bus, value);
      EventInstance eventInstance;
      if (this.activeSnapshots.TryGetValue((HashedString) AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot, out eventInstance))
      {
        int num = (int) eventInstance.setParameterValue("userVolume_" + bus, this.userVolumeSettings[bus].busLevel);
      }
      else
        this.Log("Tried to set [" + bus + "] to [" + (object) value + "] but UserVolumeSettingsSnapshot is not active.");
      if (!(bus == "Music"))
        return;
      this.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", value, true);
    }
  }

  private void Log(string s)
  {
  }

  public class UserVolumeBus
  {
    public string labelString;
    public float busLevel;
  }
}
