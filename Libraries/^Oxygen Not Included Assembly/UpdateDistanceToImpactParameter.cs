// Decompiled with JetBrains decompiler
// Type: UpdateDistanceToImpactParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

internal class UpdateDistanceToImpactParameter : LoopingSoundParameterUpdater
{
  private List<UpdateDistanceToImpactParameter.Entry> entries = new List<UpdateDistanceToImpactParameter.Entry>();

  public UpdateDistanceToImpactParameter()
    : base((HashedString) "distanceToImpact")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UpdateDistanceToImpactParameter.Entry()
    {
      comet = sound.transform.GetComponent<Comet>(),
      ev = sound.ev,
      parameterIdx = sound.description.GetParameterIdx(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    foreach (UpdateDistanceToImpactParameter.Entry entry in this.entries)
    {
      if (!((Object) entry.comet == (Object) null))
      {
        float soundDistance = entry.comet.GetSoundDistance();
        int num = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, soundDistance);
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
    public Comet comet;
    public EventInstance ev;
    public int parameterIdx;
  }
}
