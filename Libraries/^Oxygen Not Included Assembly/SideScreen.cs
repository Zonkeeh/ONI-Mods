// Decompiled with JetBrains decompiler
// Type: SideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SideScreen : KScreen
{
  [SerializeField]
  private GameObject contentBody;

  public void SetContent(SideScreenContent sideScreenContent, GameObject target)
  {
    if ((Object) sideScreenContent.transform.parent != (Object) this.contentBody.transform)
      sideScreenContent.transform.SetParent(this.contentBody.transform);
    sideScreenContent.SetTarget(target);
  }
}
