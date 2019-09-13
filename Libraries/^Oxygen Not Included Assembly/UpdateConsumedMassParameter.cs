// Decompiled with JetBrains decompiler
// Type: UpdateConsumedMassParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;

internal class UpdateConsumedMassParameter : LoopingSoundParameterUpdater
{
  private List<UpdateConsumedMassParameter.Entry> entries = new List<UpdateConsumedMassParameter.Entry>();

  public UpdateConsumedMassParameter()
    : base((HashedString) "consumedMass")
  {
  }

  public override void Add(LoopingSoundParameterUpdater.Sound sound)
  {
    this.entries.Add(new UpdateConsumedMassParameter.Entry()
    {
      creatureCalorieMonitor = sound.transform.GetSMI<CreatureCalorieMonitor.Instance>(),
      ev = sound.ev,
      parameterIdx = sound.description.GetParameterIdx(this.parameter)
    });
  }

  public override void Update(float dt)
  {
    foreach (UpdateConsumedMassParameter.Entry entry in this.entries)
    {
      if (!entry.creatureCalorieMonitor.IsNullOrStopped())
      {
        float fullness = entry.creatureCalorieMonitor.stomach.GetFullness();
        int num = (int) entry.ev.setParameterValueByIndex(entry.parameterIdx, fullness);
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
    public CreatureCalorieMonitor.Instance creatureCalorieMonitor;
    public EventInstance ev;
    public int parameterIdx;
  }
}
