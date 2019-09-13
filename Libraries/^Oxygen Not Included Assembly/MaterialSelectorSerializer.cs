// Decompiled with JetBrains decompiler
// Type: MaterialSelectorSerializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;

[SerializationConfig(MemberSerialization.OptIn)]
public class MaterialSelectorSerializer : KMonoBehaviour
{
  [Serialize]
  private List<Dictionary<Tag, Tag>> previouslySelectedElements;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.previouslySelectedElements != null)
      return;
    this.previouslySelectedElements = new List<Dictionary<Tag, Tag>>();
  }

  public void SetSelectedElement(int selectorIndex, Tag recipe, Tag element)
  {
    while (this.previouslySelectedElements.Count <= selectorIndex)
      this.previouslySelectedElements.Add(new Dictionary<Tag, Tag>());
    this.previouslySelectedElements[selectorIndex][recipe] = element;
  }

  public Tag GetPreviousElement(int selectorIndex, Tag recipe)
  {
    Tag invalid = Tag.Invalid;
    if (this.previouslySelectedElements.Count <= selectorIndex)
      return invalid;
    this.previouslySelectedElements[selectorIndex].TryGetValue(recipe, out invalid);
    return invalid;
  }
}
