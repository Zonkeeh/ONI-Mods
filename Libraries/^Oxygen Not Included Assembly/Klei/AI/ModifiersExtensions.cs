// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifiersExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public static class ModifiersExtensions
  {
    public static Attributes GetAttributes(this KMonoBehaviour cmp)
    {
      return cmp.gameObject.GetAttributes();
    }

    public static Attributes GetAttributes(this GameObject go)
    {
      Modifiers component = go.GetComponent<Modifiers>();
      if ((Object) component != (Object) null)
        return component.attributes;
      return (Attributes) null;
    }

    public static Amounts GetAmounts(this KMonoBehaviour cmp)
    {
      return cmp.gameObject.GetAmounts();
    }

    public static Amounts GetAmounts(this GameObject go)
    {
      Modifiers component = go.GetComponent<Modifiers>();
      if ((Object) component != (Object) null)
        return component.amounts;
      return (Amounts) null;
    }

    public static Sicknesses GetSicknesses(this KMonoBehaviour cmp)
    {
      return cmp.gameObject.GetSicknesses();
    }

    public static Sicknesses GetSicknesses(this GameObject go)
    {
      Modifiers component = go.GetComponent<Modifiers>();
      if ((Object) component != (Object) null)
        return component.sicknesses;
      return (Sicknesses) null;
    }
  }
}
