// Decompiled with JetBrains decompiler
// Type: TravelTubeBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
public class TravelTubeBridge : KMonoBehaviour, ITravelTubePiece
{
  private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>((System.Action<TravelTubeBridge, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>((System.Action<TravelTubeBridge, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));

  public Vector3 Position
  {
    get
    {
      return this.transform.GetPosition();
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Grid.HasTube[Grid.PosToCell((KMonoBehaviour) this)] = true;
    Components.ITravelTubePieces.Add((ITravelTubePiece) this);
    this.Subscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate);
    this.Subscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate);
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate, false);
    this.Unsubscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate, false);
    Grid.HasTube[Grid.PosToCell((KMonoBehaviour) this)] = false;
    Components.ITravelTubePieces.Remove((ITravelTubePiece) this);
    base.OnCleanUp();
  }

  private void OnBuildingBroken(object data)
  {
  }

  private void OnBuildingFullyRepaired(object data)
  {
  }
}
