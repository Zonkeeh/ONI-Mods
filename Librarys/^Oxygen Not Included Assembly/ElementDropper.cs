// Decompiled with JetBrains decompiler
// Type: ElementDropper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ElementDropper : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<ElementDropper> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ElementDropper>((System.Action<ElementDropper, object>) ((component, data) => component.OnStorageChanged(data)));
  [SerializeField]
  public Vector3 emitOffset = Vector3.zero;
  [SerializeField]
  public Tag emitTag;
  [SerializeField]
  public float emitMass;
  [MyCmpGet]
  private Storage storage;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<ElementDropper>(-1697596308, ElementDropper.OnStorageChangedDelegate);
  }

  private void OnStorageChanged(object data)
  {
    GameObject first = this.storage.FindFirst(this.emitTag);
    if ((UnityEngine.Object) first == (UnityEngine.Object) null || (double) first.GetComponent<PrimaryElement>().Mass < (double) this.emitMass)
      return;
    Pickupable component = first.GetComponent<Pickupable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component = component.Take(this.emitMass);
      component.transform.SetPosition(component.transform.GetPosition() + this.emitOffset);
    }
    else
    {
      this.storage.Drop(first, true);
      first.transform.SetPosition(first.transform.GetPosition() + this.emitOffset);
    }
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, component.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(component.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), component.transform, 1.5f, false);
  }
}
