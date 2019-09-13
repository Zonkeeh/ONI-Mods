// Decompiled with JetBrains decompiler
// Type: AttackProperties
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class AttackProperties
{
  public float aoe_radius = 2f;
  public Weapon attacker;
  public AttackProperties.DamageType damageType;
  public AttackProperties.TargetType targetType;
  public float base_damage_min;
  public float base_damage_max;
  public int maxHits;
  public List<AttackEffect> effects;

  public enum DamageType
  {
    Standard,
  }

  public enum TargetType
  {
    Single,
    AreaOfEffect,
  }
}
