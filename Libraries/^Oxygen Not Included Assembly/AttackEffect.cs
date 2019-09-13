// Decompiled with JetBrains decompiler
// Type: AttackEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class AttackEffect
{
  public string effectID;
  public float effectProbability;

  public AttackEffect(string ID, float probability)
  {
    this.effectID = ID;
    this.effectProbability = probability;
  }
}
