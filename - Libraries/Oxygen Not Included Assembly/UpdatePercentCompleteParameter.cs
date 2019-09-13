// Decompiled with JetBrains decompiler
// Type: UpdatePercentCompleteParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater
{
  private List<UpdatePercentCompleteParameter.Entry> entries = new List<UpdatePercentCompleteParameter.Entry>();

  public UpdatePercentCompleteParameter()
    : base((HashedString) "percentComplete")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UpdatePercentCompleteParameter.Entry()
    {
      worker = sound.transform.GetComponent<Worker>(),
      ev = sound.ev,
      parameterIdx = sound.description.GetParameterIdx(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    foreach (UpdatePercentCompleteParameter.Entry entry in this.entries)
    {
      if (!((Object) entry.worker == (Object) null))
      {
        Workable workable = entry.worker.workable;
        if (!((Object) workable == (Object) null))
        {
          float percentComplete = workable.GetPercentComplete();
          int num = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, percentComplete);
        }
      }
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
    public Worker worker;
    public EventInstance ev;
    public int parameterIdx;
  }
}
