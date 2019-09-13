// Decompiled with JetBrains decompiler
// Type: SpeedOneShotUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
  public SpeedOneShotUpdater()
    : base((HashedString) "Speed")
  {
  }

  public override void Play(OneShotSoundParameterUpdater.Sound sound)
  {
    int num = (int) sound.ev.setParameterValueByIndex(sound.description.GetParameterIdx(this.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue());
  }
}
