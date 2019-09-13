// Decompiled with JetBrains decompiler
// Type: ElementSplitterComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ElementSplitterComponents : KGameObjectComponentManager<ElementSplitter>
{
  private const float MAX_STACK_SIZE = 25000f;

  public HandleVector<int>.Handle Add(GameObject go)
  {
    return this.Add(go, new ElementSplitter(go));
  }

  protected override void OnPrefabInit(HandleVector<int>.Handle handle)
  {
    ElementSplitter data = this.GetData(handle);
    Pickupable component = data.primaryElement.GetComponent<Pickupable>();
    Func<float, Pickupable> func1 = (Func<float, Pickupable>) (amount => ElementSplitterComponents.OnTake(handle, amount));
    component.OnTake += func1;
    Func<Pickupable, bool> func2 = (Func<Pickupable, bool>) (other => ElementSplitterComponents.CanFirstAbsorbSecond(handle, this.GetHandle(other.gameObject)));
    component.CanAbsorb += func2;
    component.absorbable = true;
    data.onTakeCB = func1;
    data.canAbsorbCB = func2;
    this.SetData(handle, data);
  }

  protected override void OnSpawn(HandleVector<int>.Handle handle)
  {
  }

  protected override void OnCleanUp(HandleVector<int>.Handle handle)
  {
    ElementSplitter data = this.GetData(handle);
    if (!((UnityEngine.Object) data.primaryElement != (UnityEngine.Object) null))
      return;
    Pickupable component = data.primaryElement.GetComponent<Pickupable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.OnTake -= data.onTakeCB;
    component.CanAbsorb -= data.canAbsorbCB;
  }

  private static bool CanFirstAbsorbSecond(
    HandleVector<int>.Handle first,
    HandleVector<int>.Handle second)
  {
    if (first == HandleVector<int>.InvalidHandle || second == HandleVector<int>.InvalidHandle)
      return false;
    ElementSplitter data1 = GameComps.ElementSplitters.GetData(first);
    ElementSplitter data2 = GameComps.ElementSplitters.GetData(second);
    if (data1.primaryElement.ElementID == data2.primaryElement.ElementID)
      return (double) data1.primaryElement.Units + (double) data2.primaryElement.Units < 25000.0;
    return false;
  }

  private static Pickupable OnTake(HandleVector<int>.Handle handle, float amount)
  {
    ElementSplitter data = GameComps.ElementSplitters.GetData(handle);
    Pickupable component1 = data.primaryElement.GetComponent<Pickupable>();
    Storage storage = component1.storage;
    PrimaryElement component2 = component1.GetComponent<PrimaryElement>();
    Pickupable component3 = component2.Element.substance.SpawnResource(component1.transform.GetPosition(), amount, component2.Temperature, byte.MaxValue, 0, true, false, false).GetComponent<Pickupable>();
    component1.TotalAmount -= amount;
    component3.Trigger(1335436905, (object) component1);
    ElementSplitterComponents.CopyRenderSettings(component1.GetComponent<KBatchedAnimController>(), component3.GetComponent<KBatchedAnimController>());
    if ((UnityEngine.Object) storage != (UnityEngine.Object) null)
    {
      storage.Trigger(-1697596308, (object) data.primaryElement.gameObject);
      storage.Trigger(-778359855, (object) null);
    }
    return component3;
  }

  private static void CopyRenderSettings(KBatchedAnimController src, KBatchedAnimController dest)
  {
    if (!((UnityEngine.Object) src != (UnityEngine.Object) null) || !((UnityEngine.Object) dest != (UnityEngine.Object) null))
      return;
    dest.OverlayColour = src.OverlayColour;
  }
}
