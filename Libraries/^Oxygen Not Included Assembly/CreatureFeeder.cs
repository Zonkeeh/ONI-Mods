// Decompiled with JetBrains decompiler
// Type: CreatureFeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class CreatureFeeder : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>((System.Action<CreatureFeeder, object>) ((component, data) => component.OnAteFromStorage(data)));
  public string effectId;

  protected override void OnSpawn()
  {
    Components.CreatureFeeders.Add(this);
    this.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
  }

  protected override void OnCleanUp()
  {
    Components.CreatureFeeders.Remove(this);
  }

  private void OnAteFromStorage(object data)
  {
    if (string.IsNullOrEmpty(this.effectId))
      return;
    (data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
  }
}
