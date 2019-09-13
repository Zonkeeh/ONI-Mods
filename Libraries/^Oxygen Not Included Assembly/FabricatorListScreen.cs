// Decompiled with JetBrains decompiler
// Type: FabricatorListScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class FabricatorListScreen : KToggleMenu
{
  private void Refresh()
  {
    List<KToggleMenu.ToggleInfo> toggleInfoList = new List<KToggleMenu.ToggleInfo>();
    foreach (Fabricator fabricator in Components.Fabricators.Items)
    {
      KSelectable component = fabricator.GetComponent<KSelectable>();
      toggleInfoList.Add(new KToggleMenu.ToggleInfo(component.GetName(), (object) fabricator, Action.NumActions));
    }
    this.Setup((IList<KToggleMenu.ToggleInfo>) toggleInfoList);
  }

  protected override void OnSpawn()
  {
    this.onSelect += new KToggleMenu.OnSelect(this.OnClickFabricator);
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.Refresh();
  }

  private void OnClickFabricator(KToggleMenu.ToggleInfo toggle_info)
  {
    Fabricator userData = (Fabricator) toggle_info.userData;
    SelectTool.Instance.Select(userData.GetComponent<KSelectable>(), false);
  }
}
