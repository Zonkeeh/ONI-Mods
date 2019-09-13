// Decompiled with JetBrains decompiler
// Type: RemoteSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using UnityEngine;

[Serializable]
public class RemoteSoundEvent : SoundEvent
{
  private const string STATE_PARAMETER = "State";

  public RemoteSoundEvent(string file_name, string sound_name, int frame, float min_interval)
    : base(file_name, sound_name, frame, true, false, min_interval, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
    Workable workable = behaviour.GetComponent<Worker>().workable;
    if (!((UnityEngine.Object) workable != (UnityEngine.Object) null))
      return;
    Toggleable component = workable.GetComponent<Toggleable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    IToggleHandler handlerForWorker = component.GetToggleHandlerForWorker(behaviour.GetComponent<Worker>());
    float num1 = 1f;
    if (handlerForWorker != null && handlerForWorker.IsHandlerOn())
      num1 = 0.0f;
    EventInstance instance = SoundEvent.BeginOneShot(this.sound, position);
    int num2 = (int) instance.setParameterValue("State", num1);
    SoundEvent.EndOneShot(instance);
  }
}
