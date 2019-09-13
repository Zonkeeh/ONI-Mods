// Decompiled with JetBrains decompiler
// Type: TimeOfDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  private float scale;
  private TimeOfDay.TimeRegion timeRegion;
  private EventInstance nightLPEvent;
  public static TimeOfDay Instance;

  public static void DestroyInstance()
  {
    TimeOfDay.Instance = (TimeOfDay) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    TimeOfDay.Instance = this;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    TimeOfDay.Instance = (TimeOfDay) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.timeRegion = this.GetCurrentTimeRegion();
    double num = (double) this.UpdateSunlightIntensity();
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    this.UpdateVisuals();
  }

  public TimeOfDay.TimeRegion GetCurrentTimeRegion()
  {
    return (double) GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875 ? TimeOfDay.TimeRegion.Night : TimeOfDay.TimeRegion.Day;
  }

  private void Update()
  {
    this.UpdateVisuals();
    this.UpdateAudio();
  }

  private void UpdateVisuals()
  {
    float num1 = 0.875f;
    float num2 = 0.2f;
    float num3 = 1f;
    float b = 0.0f;
    if ((double) GameClock.Instance.GetCurrentCycleAsPercentage() >= (double) num1)
      b = num3;
    this.scale = Mathf.Lerp(this.scale, b, Time.deltaTime * num2);
    Shader.SetGlobalVector("_TimeOfDay", new Vector4(this.scale, this.UpdateSunlightIntensity(), 0.0f, 0.0f));
  }

  private void UpdateAudio()
  {
    TimeOfDay.TimeRegion currentTimeRegion = this.GetCurrentTimeRegion();
    if (currentTimeRegion == this.timeRegion)
      return;
    this.TriggerSoundChange(currentTimeRegion);
    this.timeRegion = currentTimeRegion;
    this.Trigger(1791086652, (object) null);
  }

  public void Sim4000ms(float dt)
  {
    double num = (double) this.UpdateSunlightIntensity();
  }

  private float UpdateSunlightIntensity()
  {
    float num1 = 0.875f;
    float num2 = GameClock.Instance.GetCurrentCycleAsPercentage() / num1;
    if ((double) num2 >= 1.0)
      num2 = 0.0f;
    float num3 = Mathf.Sin(num2 * 3.141593f);
    Game.Instance.currentSunlightIntensity = num3 * 80000f;
    return num3;
  }

  private void TriggerSoundChange(TimeOfDay.TimeRegion new_region)
  {
    switch (new_region)
    {
      case TimeOfDay.TimeRegion.Day:
        AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().NightStartedMigrated, STOP_MODE.ALLOWFADEOUT);
        if (MusicManager.instance.SongIsPlaying("Stinger_Loop_Night"))
          MusicManager.instance.StopSong("Stinger_Loop_Night", true, STOP_MODE.ALLOWFADEOUT);
        MusicManager.instance.PlaySong("Stinger_Day", false);
        MusicManager.instance.PlayDynamicMusic();
        break;
      case TimeOfDay.TimeRegion.Night:
        AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
        MusicManager.instance.PlaySong("Stinger_Loop_Night", false);
        break;
    }
  }

  public void SetScale(float new_scale)
  {
    this.scale = new_scale;
  }

  public enum TimeRegion
  {
    Invalid,
    Day,
    Night,
  }
}
