// Decompiled with JetBrains decompiler
// Type: DebugElementMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugElementMenu : KButtonMenu
{
  public static DebugElementMenu Instance;
  public GameObject root;

  protected override void OnPrefabInit()
  {
    DebugElementMenu.Instance = this;
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
  }

  public void Turnoff()
  {
    this.root.gameObject.SetActive(false);
  }
}
