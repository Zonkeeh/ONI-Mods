// Decompiled with JetBrains decompiler
// Type: Need
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

public abstract class Need : KMonoBehaviour
{
  protected AttributeInstance expectationAttribute;
  protected Need.ModifierType stressBonus;
  protected Need.ModifierType stressNeutral;
  protected Need.ModifierType stressPenalty;
  protected Need.ModifierType currentStressModifier;

  public string Name { get; protected set; }

  public string ExpectationTooltip { get; protected set; }

  public string Tooltip { get; protected set; }

  public Attribute GetExpectationAttribute()
  {
    return this.expectationAttribute.Attribute;
  }

  protected void SetModifier(Need.ModifierType modifier)
  {
    if (this.currentStressModifier == modifier)
      return;
    if (this.currentStressModifier != null)
      this.UnapplyModifier(this.currentStressModifier);
    if (modifier != null)
      this.ApplyModifier(modifier);
    this.currentStressModifier = modifier;
  }

  private void ApplyModifier(Need.ModifierType modifier)
  {
    if (modifier.modifier != null)
      this.GetAttributes().Add(modifier.modifier);
    if (modifier.statusItem != null)
      this.GetComponent<KSelectable>().AddStatusItem(modifier.statusItem, (object) null);
    if (modifier.thought == null)
      return;
    this.GetSMI<ThoughtGraph.Instance>().AddThought(modifier.thought);
  }

  private void UnapplyModifier(Need.ModifierType modifier)
  {
    if (modifier.modifier != null)
      this.GetAttributes().Remove(modifier.modifier);
    if (modifier.statusItem != null)
      this.GetComponent<KSelectable>().RemoveStatusItem(modifier.statusItem, false);
    if (modifier.thought == null)
      return;
    this.GetSMI<ThoughtGraph.Instance>().RemoveThought(modifier.thought);
  }

  protected class ModifierType
  {
    public AttributeModifier modifier;
    public StatusItem statusItem;
    public Thought thought;
  }
}
