// Decompiled with JetBrains decompiler
// Type: KSelectableExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class KSelectableExtensions
{
  public static string GetProperName(this Component cmp)
  {
    if ((Object) cmp != (Object) null && (Object) cmp.gameObject != (Object) null)
      return cmp.gameObject.GetProperName();
    return string.Empty;
  }

  public static string GetProperName(this GameObject go)
  {
    if ((Object) go != (Object) null)
    {
      KSelectable component = go.GetComponent<KSelectable>();
      if ((Object) component != (Object) null)
        return component.GetName();
    }
    return string.Empty;
  }

  public static string GetProperName(this KSelectable cmp)
  {
    if ((Object) cmp != (Object) null)
      return cmp.GetName();
    return string.Empty;
  }
}
