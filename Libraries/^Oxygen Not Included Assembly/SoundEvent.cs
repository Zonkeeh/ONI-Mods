// Decompiled with JetBrains decompiler
// Type: SoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{Name}")]
public class SoundEvent : AnimEvent
{
  public static int IGNORE_INTERVAL = -1;
  protected bool isDynamic;

  public SoundEvent()
  {
  }

  public SoundEvent(
    string file_name,
    string sound_name,
    int frame,
    bool do_load,
    bool is_looping,
    float min_interval,
    bool is_dynamic)
    : base(file_name, sound_name, frame)
  {
    this.shouldCameraScalePosition = true;
    if (do_load)
    {
      this.sound = GlobalAssets.GetSound(sound_name, false);
      this.soundHash = new HashedString(this.sound);
      if (this.sound == null || !(this.sound == string.Empty))
        ;
    }
    this.minInterval = min_interval;
    this.looping = is_looping;
    this.isDynamic = is_dynamic;
    this.noiseValues = SoundEventVolumeCache.instance.GetVolume(file_name, sound_name);
  }

  public string sound { get; private set; }

  public HashedString soundHash { get; private set; }

  public bool looping { get; private set; }

  public bool ignorePause { get; set; }

  public bool shouldCameraScalePosition { get; set; }

  public float minInterval { get; private set; }

  public EffectorValues noiseValues { get; set; }

  public static bool ShouldPlaySound(
    KBatchedAnimController controller,
    string sound,
    bool is_looping,
    bool is_dynamic)
  {
    CameraController instance1 = CameraController.Instance;
    if ((UnityEngine.Object) instance1 == (UnityEngine.Object) null)
      return true;
    Vector3 position = controller.transform.GetPosition();
    Vector3 offset = controller.Offset;
    position.x += offset.x;
    position.y += offset.y;
    SpeedControlScreen instance2 = SpeedControlScreen.Instance;
    if (is_dynamic)
      return (!((UnityEngine.Object) instance2 != (UnityEngine.Object) null) || !instance2.IsPaused) && instance1.IsAudibleSound((Vector2) position);
    if (sound == null || SoundEvent.IsLowPrioritySound(sound))
      return false;
    if (!instance1.IsAudibleSound(position, sound))
    {
      if (!is_looping && !GlobalAssets.IsHighPriority(sound))
        return false;
    }
    else if ((UnityEngine.Object) instance2 != (UnityEngine.Object) null && instance2.IsPaused)
      return false;
    return true;
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    if (!SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.looping, this.isDynamic))
      return;
    this.PlaySound(behaviour);
  }

  protected void PlaySound(AnimEventManager.EventPlayerData behaviour, string sound)
  {
    Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
    KBatchedAnimController component1 = behaviour.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      Vector3 offset = component1.Offset;
      position.x += offset.x;
      position.y += offset.y;
    }
    AudioDebug audioDebug = AudioDebug.Get();
    if ((UnityEngine.Object) audioDebug != (UnityEngine.Object) null)
    {
      if (audioDebug.debugSoundEvents)
        Debug.Log((object) (behaviour.name + ", " + sound + ", " + (object) this.frame + ", " + (object) position));
    }
    try
    {
      if (this.looping)
      {
        LoopingSounds component2 = behaviour.GetComponent<LoopingSounds>();
        if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
        {
          Debug.Log((object) (behaviour.name + " is missing LoopingSounds component. "));
        }
        else
        {
          if (component2.StartSound(sound, behaviour, this.noiseValues, this.ignorePause, this.shouldCameraScalePosition))
            return;
          DebugUtil.LogWarningArgs((object) string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", (object) sound, (object) behaviour.name));
        }
      }
      else
      {
        if (SoundEvent.PlayOneShot(sound, behaviour, this.noiseValues))
          return;
        DebugUtil.LogWarningArgs((object) string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", (object) sound, (object) behaviour.name));
      }
    }
    catch (Exception ex)
    {
      string message = string.Format("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + sound == null ? "null" : sound.ToString(), (object) behaviour.GetType().ToString(), (object) ex.Message, (object) ex.StackTrace);
      Debug.LogError((object) message);
      throw new ArgumentException(message, ex);
    }
  }

  public virtual void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    this.PlaySound(behaviour, this.sound);
  }

  public static Vector3 GetCameraScaledPosition(Vector3 pos)
  {
    Vector3 vector3 = Vector3.zero;
    if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      vector3 = CameraController.Instance.GetVerticallyScaledPosition((Vector2) pos);
    return vector3;
  }

  public static FMOD.Studio.EventInstance BeginOneShot(string ev, Vector3 pos)
  {
    return KFMOD.BeginOneShot(ev, SoundEvent.GetCameraScaledPosition(pos));
  }

  public static bool EndOneShot(FMOD.Studio.EventInstance instance)
  {
    return KFMOD.EndOneShot(instance);
  }

  public static bool PlayOneShot(string sound, Vector3 pos)
  {
    bool flag = false;
    if (!string.IsNullOrEmpty(sound))
    {
      FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, pos);
      if (instance.isValid())
        flag = SoundEvent.EndOneShot(instance);
    }
    return flag;
  }

  public static bool PlayOneShot(
    string sound,
    AnimEventManager.EventPlayerData behaviour,
    EffectorValues noiseValues)
  {
    bool flag = false;
    if (!string.IsNullOrEmpty(sound))
    {
      Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
      FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, position);
      if (instance.isValid())
        flag = SoundEvent.EndOneShot(instance);
    }
    return flag;
  }

  public override void Stop(AnimEventManager.EventPlayerData behaviour)
  {
    if (!this.looping)
      return;
    LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.StopSound(this.sound);
  }

  protected static bool IsLowPrioritySound(string sound)
  {
    return sound != null && (double) Camera.main.orthographicSize > (double) AudioMixer.LOW_PRIORITY_CUTOFF_DISTANCE && (!AudioMixer.instance.activeNIS && GlobalAssets.IsLowPriority(sound));
  }

  protected void PrintSoundDebug(
    string anim_name,
    string sound,
    string sound_name,
    Vector3 sound_pos)
  {
    if (sound != null)
      Debug.Log((object) (anim_name + ", " + sound_name + ", " + (object) this.frame + ", " + (object) sound_pos));
    else
      Debug.Log((object) ("Missing sound: " + anim_name + ", " + sound_name));
  }
}
