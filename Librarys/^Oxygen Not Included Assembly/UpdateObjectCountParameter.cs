// Decompiled with JetBrains decompiler
// Type: UpdateObjectCountParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

internal class UpdateObjectCountParameter : LoopingSoundParameterUpdater
{
  private static Dictionary<HashedString, UpdateObjectCountParameter.Settings> settings = new Dictionary<HashedString, UpdateObjectCountParameter.Settings>();
  private static readonly HashedString parameterHash = (HashedString) "objectCount";
  private List<UpdateObjectCountParameter.Entry> entries = new List<UpdateObjectCountParameter.Entry>();

  public UpdateObjectCountParameter()
    : base((HashedString) "objectCount")
  {
  }

  public static UpdateObjectCountParameter.Settings GetSettings(
    HashedString path_hash,
    SoundDescription description)
  {
    UpdateObjectCountParameter.Settings settings = new UpdateObjectCountParameter.Settings();
    if (!UpdateObjectCountParameter.settings.TryGetValue(path_hash, out settings))
    {
      settings = new UpdateObjectCountParameter.Settings();
      EventDescription eventDescription = RuntimeManager.GetEventDescription(description.path);
      USER_PROPERTY property1;
      settings.minObjects = eventDescription.getUserProperty("minObj", out property1) != RESULT.OK ? 1f : (float) (short) property1.floatValue();
      USER_PROPERTY property2;
      settings.maxObjects = eventDescription.getUserProperty("maxObj", out property2) != RESULT.OK ? 0.0f : property2.floatValue();
      USER_PROPERTY property3;
      if (eventDescription.getUserProperty("curveType", out property3) == RESULT.OK && property3.stringValue() == "exp")
        settings.useExponentialCurve = true;
      settings.parameterIdx = description.GetParameterIdx(UpdateObjectCountParameter.parameterHash);
      settings.path = path_hash;
      UpdateObjectCountParameter.settings[path_hash] = settings;
    }
    return settings;
  }

  public static void ApplySettings(
    EventInstance ev,
    int count,
    UpdateObjectCountParameter.Settings settings)
  {
    float num1 = 0.0f;
    if ((double) settings.maxObjects != (double) settings.minObjects)
      num1 = Mathf.Clamp01((float) (((double) count - (double) settings.minObjects) / ((double) settings.maxObjects - (double) settings.minObjects)));
    if (settings.useExponentialCurve)
      num1 *= num1;
    int num2 = (int) ev.setParameterValueByIndex(settings.parameterIdx, num1);
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
    this.entries.Add(new UpdateObjectCountParameter.Entry()
    {
      ev = sound.ev,
      settings = settings
    });
  }

  public override void Update(float dt)
  {
    DictionaryPool<HashedString, int, LoopingSoundManager>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, int, LoopingSoundManager>.Allocate();
    foreach (UpdateObjectCountParameter.Entry entry in this.entries)
    {
      int num = 0;
      pooledDictionary.TryGetValue(entry.settings.path, out num);
      pooledDictionary[entry.settings.path] = ++num;
    }
    foreach (UpdateObjectCountParameter.Entry entry in this.entries)
    {
      int count = pooledDictionary[entry.settings.path];
      UpdateObjectCountParameter.ApplySettings(entry.ev, count, entry.settings);
    }
    pooledDictionary.Recycle();
  }

  public override void Remove(LoopingSoundParameterUpdater.Sound sound)
  {
    for (int index = 0; index < this.entries.Count; ++index)
    {
      if (this.entries[index].ev.handle == sound.ev.handle)
      {
        this.entries.RemoveAt(index);
        break;
      }
    }
  }

  public static void Clear()
  {
    UpdateObjectCountParameter.settings.Clear();
  }

  private struct Entry
  {
    public EventInstance ev;
    public UpdateObjectCountParameter.Settings settings;
  }

  public struct Settings
  {
    public HashedString path;
    public int parameterIdx;
    public float minObjects;
    public float maxObjects;
    public bool useExponentialCurve;
  }
}
