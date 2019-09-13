// Decompiled with JetBrains decompiler
// Type: SpeedLoopingSoundUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
  private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

  public SpeedLoopingSoundUpdater()
    : base((HashedString) "Speed")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new SpeedLoopingSoundUpdater.Entry()
    {
      ev = sound.ev,
      parameterIdx = sound.description.GetParameterIdx(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
    foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
    {
      int num = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, speedParameterValue);
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

  public static float GetSpeedParameterValue()
  {
    return Time.timeScale * 1f;
  }

  private struct Entry
  {
    public EventInstance ev;
    public int parameterIdx;
  }
}
