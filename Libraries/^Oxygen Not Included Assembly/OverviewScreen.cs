// Decompiled with JetBrains decompiler
// Type: OverviewScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class OverviewScreen : KTabMenu
{
  private List<KScreen> TabScreens = new List<KScreen>();
  public InstantiateUIPrefabChild ScreenInstantiator;
  public TitleBar titleBar;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ScreenInstantiator.Instantiate();
    foreach (KScreen componentsInChild in this.ScreenInstantiator.GetComponentsInChildren<KScreen>())
      this.TabScreens.Add(componentsInChild);
    foreach (KScreen tabScreen in this.TabScreens)
    {
      this.AddTab(tabScreen.displayName, tabScreen);
      tabScreen.gameObject.SetActive(false);
    }
  }

  public override void ActivateTab(int tabIdx)
  {
    switch (tabIdx)
    {
      case 0:
        this.titleBar.SetTitle((string) UI.JOBS);
        break;
      case 1:
        this.titleBar.SetTitle((string) UI.VITALS);
        break;
    }
    base.ActivateTab(tabIdx);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  protected override void OnActivate()
  {
    base.OnActivate();
  }

  protected override void OnDeactivate()
  {
    foreach (KScreen tabScreen in this.TabScreens)
    {
      tabScreen.Deactivate();
      Object.Destroy((Object) tabScreen);
    }
  }
}
