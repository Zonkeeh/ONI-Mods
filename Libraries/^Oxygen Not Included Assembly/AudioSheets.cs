// Decompiled with JetBrains decompiler
// Type: AudioSheets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class AudioSheets : ScriptableObject
{
  public List<AudioSheet> sheets = new List<AudioSheet>();
  public Dictionary<HashedString, List<AnimEvent>> events = new Dictionary<HashedString, List<AnimEvent>>();

  public virtual void Initialize()
  {
    foreach (AudioSheet sheet in this.sheets)
    {
      foreach (AudioSheet.SoundInfo soundInfo in sheet.soundInfos)
      {
        string type = soundInfo.Type;
        if (type == null || type == string.Empty)
          type = sheet.defaultType;
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name0, soundInfo.Frame0);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name1, soundInfo.Frame1);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name2, soundInfo.Frame2);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name3, soundInfo.Frame3);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name4, soundInfo.Frame4);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name5, soundInfo.Frame5);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name6, soundInfo.Frame6);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name7, soundInfo.Frame7);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name8, soundInfo.Frame8);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name9, soundInfo.Frame9);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name10, soundInfo.Frame10);
        this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name11, soundInfo.Frame11);
      }
    }
  }

  private void CreateSound(
    string file_name,
    string anim_name,
    string type,
    float min_interval,
    string sound_name,
    int frame)
  {
    if (string.IsNullOrEmpty(sound_name))
      return;
    HashedString key = (HashedString) (file_name + "." + anim_name);
    AnimEvent soundOfType = this.CreateSoundOfType(type, file_name, sound_name, frame, min_interval);
    if (soundOfType == null)
    {
      Debug.LogError((object) ("Unknown sound type: " + type));
    }
    else
    {
      List<AnimEvent> animEventList = (List<AnimEvent>) null;
      if (!this.events.TryGetValue(key, out animEventList))
      {
        animEventList = new List<AnimEvent>();
        this.events[key] = animEventList;
      }
      animEventList.Add(soundOfType);
    }
  }

  protected abstract AnimEvent CreateSoundOfType(
    string type,
    string file_name,
    string sound_name,
    int frame,
    float min_interval);

  public List<AnimEvent> GetEvents(HashedString anim_id)
  {
    List<AnimEvent> animEventList = (List<AnimEvent>) null;
    this.events.TryGetValue(anim_id, out animEventList);
    return animEventList;
  }
}
