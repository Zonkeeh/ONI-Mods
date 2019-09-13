// Decompiled with JetBrains decompiler
// Type: LoopingSoundParameterUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

public abstract class LoopingSoundParameterUpdater
{
  public LoopingSoundParameterUpdater(HashedString parameter)
  {
    this.parameter = parameter;
  }

  public HashedString parameter { get; private set; }

  public abstract void Add(LoopingSoundParameterUpdater.Sound sound);

  public abstract void Update(float dt);

  public abstract void Remove(LoopingSoundParameterUpdater.Sound sound);

  public struct Sound
  {
    public EventInstance ev;
    public HashedString path;
    public Transform transform;
    public SoundDescription description;
  }
}
