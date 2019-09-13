// Decompiled with JetBrains decompiler
// Type: ParkSign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class ParkSign : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<ParkSign> TriggerRoomEffectsDelegate = new EventSystem.IntraObjectHandler<ParkSign>((System.Action<ParkSign, object>) ((component, data) => component.TriggerRoomEffects(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<ParkSign>(-832141045, ParkSign.TriggerRoomEffectsDelegate);
  }

  private void TriggerRoomEffects(object data)
  {
    Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject)?.roomType.TriggerRoomEffects(this.gameObject.GetComponent<KPrefabID>(), ((GameObject) data).GetComponent<Effects>());
  }
}
