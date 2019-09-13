// Decompiled with JetBrains decompiler
// Type: SolidBooster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SolidBooster : RocketEngine
{
  public Storage fuelStorage;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.gameObject.Subscribe(1366341636, new System.Action<object>(this.OnReturn));
  }

  [ContextMenu("Fill Tank")]
  public void FillTank()
  {
    Element element1 = ElementLoader.GetElement(this.fuelTag);
    this.fuelStorage.Store(element1.substance.SpawnResource(this.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element1.defaultValues.temperature, byte.MaxValue, 0, false, false, false), false, false, true, false);
    Element element2 = ElementLoader.GetElement(GameTags.OxyRock);
    this.fuelStorage.Store(element2.substance.SpawnResource(this.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element2.defaultValues.temperature, byte.MaxValue, 0, false, false, false), false, false, true, false);
  }

  private void OnReturn(object data)
  {
    if (!((UnityEngine.Object) this.fuelStorage != (UnityEngine.Object) null) || this.fuelStorage.items == null)
      return;
    for (int index = this.fuelStorage.items.Count - 1; index >= 0; --index)
      Util.KDestroyGameObject(this.fuelStorage.items[index]);
    this.fuelStorage.items.Clear();
  }
}
