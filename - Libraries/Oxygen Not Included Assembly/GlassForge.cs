// Decompiled with JetBrains decompiler
// Type: GlassForge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class GlassForge : ComplexFabricator
{
  private static readonly EventSystem.IntraObjectHandler<GlassForge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<GlassForge>((System.Action<GlassForge, object>) ((component, data) => component.CheckPipes(data)));
  private Guid statusHandle;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<GlassForge>(-2094018600, GlassForge.CheckPipesDelegate);
  }

  private void CheckPipes(object data)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    int index = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), GlassForgeConfig.outPipeOffset);
    GameObject gameObject = Grid.Objects[index, 16];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      if ((double) gameObject.GetComponent<PrimaryElement>().Element.highTemp > (double) ElementLoader.FindElementByHash(SimHashes.MoltenGlass).lowTemp)
        component.RemoveStatusItem(this.statusHandle, false);
      else
        this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt, (object) null);
    }
    else
      component.RemoveStatusItem(this.statusHandle, false);
  }
}
