// Decompiled with JetBrains decompiler
// Type: Klei.AI.PrefabAttributeModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.AI
{
  public class PrefabAttributeModifiers : KMonoBehaviour
  {
    public List<AttributeModifier> descriptors = new List<AttributeModifier>();

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
    }

    public void AddAttributeDescriptor(AttributeModifier modifier)
    {
      this.descriptors.Add(modifier);
    }

    public void RemovePrefabAttribute(AttributeModifier modifier)
    {
      this.descriptors.Remove(modifier);
    }
  }
}
