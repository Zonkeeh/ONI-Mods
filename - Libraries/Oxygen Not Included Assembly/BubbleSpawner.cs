// Decompiled with JetBrains decompiler
// Type: BubbleSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BubbleSpawner : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<BubbleSpawner> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<BubbleSpawner>((System.Action<BubbleSpawner, object>) ((component, data) => component.OnStorageChanged(data)));
  public Vector3 emitOffset = Vector3.zero;
  public SimHashes element;
  public float emitMass;
  public float emitVariance;
  public Vector2 initialVelocity;
  [MyCmpGet]
  private Storage storage;

  protected override void OnSpawn()
  {
    this.emitMass += (UnityEngine.Random.value - 0.5f) * this.emitVariance * this.emitMass;
    base.OnSpawn();
    this.Subscribe<BubbleSpawner>(-1697596308, BubbleSpawner.OnStorageChangedDelegate);
  }

  private void OnStorageChanged(object data)
  {
    GameObject first = this.storage.FindFirst(ElementLoader.FindElementByHash(this.element).tag);
    if ((UnityEngine.Object) first == (UnityEngine.Object) null)
      return;
    PrimaryElement component = first.GetComponent<PrimaryElement>();
    if ((double) component.Mass < (double) this.emitMass)
      return;
    first.GetComponent<PrimaryElement>().Mass -= this.emitMass;
    BubbleManager.instance.SpawnBubble((Vector2) this.transform.GetPosition(), this.initialVelocity, component.ElementID, this.emitMass, component.Temperature);
  }
}
