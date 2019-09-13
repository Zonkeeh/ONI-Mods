// Decompiled with JetBrains decompiler
// Type: UserVolumeLoopingUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;

internal abstract class UserVolumeLoopingUpdater : LoopingSoundParameterUpdater
{
  private List<UserVolumeLoopingUpdater.Entry> entries = new List<UserVolumeLoopingUpdater.Entry>();
  private string playerPref;

  public UserVolumeLoopingUpdater(string parameter, string player_pref)
    : base((HashedString) parameter)
  {
    this.playerPref = player_pref;
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UserVolumeLoopingUpdater.Entry()
    {
      ev = sound.ev,
      parameterIdx = sound.description.GetParameterIdx(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    if (string.IsNullOrEmpty(this.playerPref))
      return;
    float num1 = KPlayerPrefs.GetFloat(this.playerPref);
    foreach (UserVolumeLoopingUpdater.Entry entry in this.entries)
    {
      int num2 = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, num1);
    }
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

  private struct Entry
  {
    public EventInstance ev;
    public int parameterIdx;
  }
}
