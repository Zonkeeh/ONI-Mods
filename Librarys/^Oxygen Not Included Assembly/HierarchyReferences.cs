// Decompiled with JetBrains decompiler
// Type: HierarchyReferences
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HierarchyReferences : KMonoBehaviour
{
  public ElementReference[] references;

  public bool HasReference(string name)
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
        return true;
    }
    return false;
  }

  public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
      {
        if (reference.behaviour is SpecifiedType)
          return (SpecifiedType) reference.behaviour;
        Debug.LogError((object) string.Format("Behavior is not specified type"));
      }
    }
    Debug.LogError((object) string.Format("Could not find UI reference '{0}' or convert to specified type)", (object) name));
    return (SpecifiedType) null;
  }

  public Component GetReference(string name)
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
        return reference.behaviour;
    }
    Debug.LogWarning((object) "Couldn't find reference to object named {0} Make sure the name matches the field in the inspector.");
    return (Component) null;
  }
}
