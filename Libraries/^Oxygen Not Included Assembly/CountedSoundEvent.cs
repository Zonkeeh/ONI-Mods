// Decompiled with JetBrains decompiler
// Type: CountedSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using UnityEngine;

public class CountedSoundEvent : SoundEvent
{
  private int counterModulus = int.MinValue;
  private const int COUNTER_MODULUS_INVALID = -2147483648;
  private const int COUNTER_MODULUS_CLEAR = -1;

  public CountedSoundEvent(
    string file_name,
    string sound_name,
    int frame,
    bool do_load,
    bool is_looping,
    float min_interval,
    bool is_dynamic)
    : base(file_name, CountedSoundEvent.BaseSoundName(sound_name), frame, do_load, is_looping, min_interval, is_dynamic)
  {
    if (sound_name.Contains(":"))
    {
      string[] strArray = sound_name.Split(':');
      if (strArray.Length != 2)
        DebugUtil.LogErrorArgs((object) "Invalid CountedSoundEvent parameter for", (object) (file_name + "." + sound_name + "." + frame.ToString() + ":"), (object) ("'" + sound_name + "'"));
      for (int index = 1; index < strArray.Length; ++index)
        this.ParseParameter(strArray[index]);
    }
    else
      DebugUtil.LogErrorArgs((object) "CountedSoundEvent for", (object) (file_name + "." + sound_name + "." + frame.ToString()), (object) (" - Must specify max number of steps on event: '" + sound_name + "'"));
  }

  private static string BaseSoundName(string sound_name)
  {
    int length = sound_name.IndexOf(":");
    if (length > 0)
      return sound_name.Substring(0, length);
    return sound_name;
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    if (string.IsNullOrEmpty(this.sound) || !SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.looping, this.isDynamic))
      return;
    int num1 = -1;
    GameObject gameObject = behaviour.controller.gameObject;
    if (this.counterModulus >= -1)
    {
      HandleVector<int>.Handle h = GameComps.WhiteBoards.GetHandle(gameObject);
      if (!h.IsValid())
        h = GameComps.WhiteBoards.Add(gameObject);
      num1 = !GameComps.WhiteBoards.HasValue(h, this.soundHash) ? 0 : (int) GameComps.WhiteBoards.GetValue(h, this.soundHash);
      int num2 = this.counterModulus != -1 ? (num1 + 1) % this.counterModulus : 0;
      GameComps.WhiteBoards.SetValue(h, this.soundHash, (object) num2);
    }
    EventInstance instance = SoundEvent.BeginOneShot(this.sound, behaviour.GetComponent<Transform>().GetPosition());
    if (!instance.isValid())
      return;
    if (num1 >= 0)
    {
      int num3 = (int) instance.setParameterValue("eventCount", (float) num1);
    }
    SoundEvent.EndOneShot(instance);
  }

  private void ParseParameter(string param)
  {
    this.counterModulus = int.Parse(param);
    if (this.counterModulus != -1 && this.counterModulus < 2)
      throw new ArgumentException("CountedSoundEvent modulus must be 2 or larger");
  }
}
