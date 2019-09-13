// Decompiled with JetBrains decompiler
// Type: AttributeModifierExpectation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class AttributeModifierExpectation : Expectation
{
  public AttributeModifier modifier;
  public Sprite icon;

  public AttributeModifierExpectation(
    string id,
    string name,
    string description,
    AttributeModifier modifier,
    Sprite icon)
    : base(id, name, description, (System.Action<MinionResume>) (resume => resume.GetAttributes().Get(modifier.AttributeId).Add(modifier)), (System.Action<MinionResume>) (resume => resume.GetAttributes().Get(modifier.AttributeId).Remove(modifier)))
  {
    this.modifier = modifier;
    this.icon = icon;
  }
}
