// Decompiled with JetBrains decompiler
// Type: VoiceSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Klei.AI;
using UnityEngine;

public class VoiceSoundEvent : SoundEvent
{
  public static float locomotionSoundProb = 50f;
  public float intervalBetweenSpeaking = 10f;
  public float timeLastSpoke;

  public VoiceSoundEvent(string file_name, string sound_name, int frame, bool is_looping)
    : base(file_name, sound_name, frame, false, is_looping, (float) SoundEvent.IGNORE_INTERVAL, true)
  {
    this.noiseValues = SoundEventVolumeCache.instance.GetVolume(nameof (VoiceSoundEvent), sound_name);
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    VoiceSoundEvent.PlayVoice(this.name, behaviour.controller, this.intervalBetweenSpeaking, this.looping);
  }

  public static EventInstance PlayVoice(
    string name,
    KBatchedAnimController controller,
    float interval_between_speaking,
    bool looping)
  {
    EventInstance instance = new EventInstance();
    MinionIdentity component1 = controller.GetComponent<MinionIdentity>();
    if ((Object) component1 == (Object) null || name.Contains("state") && (double) Time.time - (double) component1.timeLastSpoke < (double) interval_between_speaking)
      return instance;
    if (name.Contains(":"))
    {
      if ((double) Random.Range(0, 100) > (double) float.Parse(name.Split(':')[1]))
        return instance;
    }
    Worker component2 = controller.GetComponent<Worker>();
    string assetName = VoiceSoundEvent.GetAssetName(name, (Component) component2);
    StaminaMonitor.Instance smi = component2.GetSMI<StaminaMonitor.Instance>();
    if (!name.Contains("sleep_") && smi != null && smi.IsSleeping())
      return instance;
    Vector3 position = component2.transform.GetPosition();
    string sound = GlobalAssets.GetSound(assetName, true);
    if (!SoundEvent.ShouldPlaySound(controller, sound, looping, false))
      return instance;
    if (sound != null)
    {
      if (looping)
      {
        LoopingSounds component3 = controller.GetComponent<LoopingSounds>();
        if ((Object) component3 == (Object) null)
          Debug.Log((object) (controller.name + " is missing LoopingSounds component. "));
        else if (!component3.StartSound(sound))
          DebugUtil.LogWarningArgs((object) string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", (object) sound, (object) controller.name));
      }
      else
      {
        instance = SoundEvent.BeginOneShot(sound, position);
        if (sound.Contains("sleep_") && controller.GetComponent<Traits>().HasTrait("Snorer"))
        {
          int num = (int) instance.setParameterValue("snoring", 1f);
        }
        SoundEvent.EndOneShot(instance);
        component1.timeLastSpoke = Time.time;
      }
    }
    else if (AudioDebug.Get().debugVoiceSounds)
      Debug.LogWarning((object) ("Missing voice sound: " + assetName));
    return instance;
  }

  private static string GetAssetName(string name, Component cmp)
  {
    string b = "F01";
    if ((Object) cmp != (Object) null)
    {
      MinionIdentity component = cmp.GetComponent<MinionIdentity>();
      if ((Object) component != (Object) null)
        b = component.GetVoiceId();
    }
    string d = name;
    if (name.Contains(":"))
      d = name.Split(':')[0];
    return StringFormatter.Combine("DupVoc_", b, "_", d);
  }

  public override void Stop(AnimEventManager.EventPlayerData behaviour)
  {
    if (!this.looping)
      return;
    LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
    if (!((Object) component != (Object) null))
      return;
    string sound = GlobalAssets.GetSound(VoiceSoundEvent.GetAssetName(this.name, (Component) component), true);
    component.StopSound(sound);
  }
}
