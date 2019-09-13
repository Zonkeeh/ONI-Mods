// Decompiled with JetBrains decompiler
// Type: PhonoboxSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

public class PhonoboxSoundEvent : SoundEvent
{
  private const string SOUND_PARAM_SONG = "jukeboxSong";
  private const string SOUND_PARAM_PITCH = "jukeboxPitch";
  private int song;
  private int pitch;

  public PhonoboxSoundEvent(string file_name, string sound_name, int frame, float min_interval)
    : base(file_name, sound_name, frame, true, true, min_interval, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
    AudioDebug audioDebug = AudioDebug.Get();
    if ((UnityEngine.Object) audioDebug != (UnityEngine.Object) null)
    {
      if (audioDebug.debugSoundEvents)
        Debug.Log((object) (behaviour.name + ", " + this.sound + ", " + (object) this.frame + ", " + (object) position));
    }
    try
    {
      LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      {
        Debug.Log((object) (behaviour.name + " is missing LoopingSounds component. "));
      }
      else
      {
        if (component.IsSoundPlaying(this.sound))
          return;
        if (component.StartSound(this.sound, behaviour, this.noiseValues, this.ignorePause, true))
        {
          EventDescription eventDescription = RuntimeManager.GetEventDescription(this.sound);
          PARAMETER_DESCRIPTION parameter1;
          int parameter2 = (int) eventDescription.getParameter("jukeboxSong", out parameter1);
          int maximum1 = (int) parameter1.maximum;
          PARAMETER_DESCRIPTION parameter3;
          int parameter4 = (int) eventDescription.getParameter("jukeboxPitch", out parameter3);
          int maximum2 = (int) parameter3.maximum;
          this.song = UnityEngine.Random.Range(0, maximum1 + 1);
          this.pitch = UnityEngine.Random.Range(0, maximum2 + 1);
          component.UpdateFirstParameter(this.sound, (HashedString) "jukeboxSong", (float) this.song);
          component.UpdateSecondParameter(this.sound, (HashedString) "jukeboxPitch", (float) this.pitch);
        }
        else
          DebugUtil.LogWarningArgs((object) string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", (object) this.sound, (object) behaviour.name));
      }
    }
    catch (Exception ex)
    {
      string message = string.Format("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + this.sound == null ? "null" : this.sound.ToString(), (object) behaviour.GetType().ToString(), (object) ex.Message, (object) ex.StackTrace);
      Debug.LogError((object) message);
      throw new ArgumentException(message, ex);
    }
  }
}
