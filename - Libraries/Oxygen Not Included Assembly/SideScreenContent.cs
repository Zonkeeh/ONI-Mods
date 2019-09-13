// Decompiled with JetBrains decompiler
// Type: SideScreenContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class SideScreenContent : KScreen
{
  [SerializeField]
  protected string titleKey;
  public GameObject ContentContainer;

  public virtual void SetTarget(GameObject target)
  {
  }

  public virtual void ClearTarget()
  {
  }

  public abstract bool IsValidForTarget(GameObject target);

  public virtual string GetTitle()
  {
    return (string) Strings.Get(this.titleKey);
  }
}
