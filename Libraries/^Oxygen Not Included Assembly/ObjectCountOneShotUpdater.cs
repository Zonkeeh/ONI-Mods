// Decompiled with JetBrains decompiler
// Type: ObjectCountOneShotUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

internal class ObjectCountOneShotUpdater : OneShotSoundParameterUpdater
{
  private Dictionary<HashedString, int> soundCounts = new Dictionary<HashedString, int>();

  public ObjectCountOneShotUpdater()
    : base((HashedString) "objectCount")
  {
  }

  public override void Update(float dt)
  {
    this.soundCounts.Clear();
  }

  public override void Play(OneShotSoundParameterUpdater.Sound sound)
  {
    UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
    int num = 0;
    this.soundCounts.TryGetValue(sound.path, out num);
    int count;
    this.soundCounts[sound.path] = count = num + 1;
    UpdateObjectCountParameter.ApplySettings(sound.ev, count, settings);
  }
}
