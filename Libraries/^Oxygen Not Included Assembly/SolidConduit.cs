// Decompiled with JetBrains decompiler
// Type: SolidConduit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

[SkipSaveFileSerialization]
public class SolidConduit : KMonoBehaviour, IHaveUtilityNetworkMgr
{
  [MyCmpReq]
  private KAnimGraphTileVisualizer graphTileDependency;
  private System.Action firstFrameCallback;

  public void SetFirstFrameCallback(System.Action ffCb)
  {
    this.firstFrameCallback = ffCb;
    this.StartCoroutine(this.RunCallback());
  }

  [DebuggerHidden]
  private IEnumerator RunCallback()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new SolidConduit.\u003CRunCallback\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public IUtilityNetworkMgr GetNetworkManager()
  {
    return (IUtilityNetworkMgr) Game.Instance.solidConduitSystem;
  }

  public UtilityNetwork GetNetwork()
  {
    return this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell((KMonoBehaviour) this));
  }

  public static SolidConduitFlow GetFlowManager()
  {
    return Game.Instance.solidConduitFlow;
  }

  public Vector3 Position
  {
    get
    {
      return this.transform.GetPosition();
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, (object) this);
  }

  protected override void OnCleanUp()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] == (UnityEngine.Object) null)
    {
      this.GetNetworkManager().RemoveFromNetworks(cell, (object) this, false);
      SolidConduit.GetFlowManager().EmptyConduit(cell);
    }
    base.OnCleanUp();
  }
}
