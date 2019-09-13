// Decompiled with JetBrains decompiler
// Type: SoundEventVolumeCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
  public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();

  public static SoundEventVolumeCache instance
  {
    get
    {
      return Singleton<SoundEventVolumeCache>.Instance;
    }
  }

  public void AddVolume(string animFile, string eventName, EffectorValues vals)
  {
    HashedString key = new HashedString(animFile + ":" + eventName);
    if (!this.volumeCache.ContainsKey(key))
      this.volumeCache.Add(key, vals);
    else
      this.volumeCache[key] = vals;
  }

  public EffectorValues GetVolume(string animFile, string eventName)
  {
    HashedString key = new HashedString(animFile + ":" + eventName);
    if (!this.volumeCache.ContainsKey(key))
      return new EffectorValues();
    return this.volumeCache[key];
  }
}
