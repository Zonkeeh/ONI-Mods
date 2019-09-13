// Decompiled with JetBrains decompiler
// Type: EntitySplitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using UnityEngine;

[SkipSaveFileSerialization]
public class EntitySplitter : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<EntitySplitter> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<EntitySplitter>((System.Action<EntitySplitter, object>) ((component, data) => component.OnAbsorb(data)));
  public float maxStackSize = float.MaxValue;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Pickupable pickupable = this.GetComponent<Pickupable>();
    if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
      Debug.LogError((object) (this.name + " does not have a pickupable component!"));
    pickupable.OnTake += (Func<float, Pickupable>) (amount => EntitySplitter.Split(pickupable, amount, (GameObject) null));
    Rottable.Instance rottable = this.gameObject.GetSMI<Rottable.Instance>();
    pickupable.absorbable = true;
    pickupable.CanAbsorb = (Func<Pickupable, bool>) (other => EntitySplitter.CanFirstAbsorbSecond(pickupable, rottable, other, this.maxStackSize));
    this.Subscribe<EntitySplitter>(-2064133523, EntitySplitter.OnAbsorbDelegate);
  }

  private static bool CanFirstAbsorbSecond(
    Pickupable pickupable,
    Rottable.Instance rottable,
    Pickupable other,
    float maxStackSize)
  {
    if ((UnityEngine.Object) other == (UnityEngine.Object) null)
      return false;
    KPrefabID component1 = pickupable.GetComponent<KPrefabID>();
    KPrefabID component2 = other.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || (UnityEngine.Object) component2 == (UnityEngine.Object) null || (component1.PrefabTag != component2.PrefabTag || (double) pickupable.TotalAmount + (double) other.TotalAmount > (double) maxStackSize))
      return false;
    if (rottable != null)
    {
      Rottable.Instance smi = other.GetSMI<Rottable.Instance>();
      if (smi == null || !rottable.IsRotLevelStackable(smi))
        return false;
    }
    return true;
  }

  public static Pickupable Split(Pickupable pickupable, float amount, GameObject prefab = null)
  {
    if ((double) amount >= (double) pickupable.TotalAmount && (UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return pickupable;
    Storage storage = pickupable.storage;
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      prefab = Assets.GetPrefab(pickupable.GetComponent<KPrefabID>().PrefabTag);
    GameObject parent = (GameObject) null;
    if ((UnityEngine.Object) pickupable.transform.parent != (UnityEngine.Object) null)
      parent = pickupable.transform.parent.gameObject;
    GameObject gameObject = GameUtil.KInstantiate(prefab, pickupable.transform.GetPosition(), Grid.SceneLayer.Ore, parent, (string) null, 0);
    Debug.Assert((UnityEngine.Object) gameObject != (UnityEngine.Object) null, (object) "WTH, the GO is null, shouldn't happen on instantiate");
    Pickupable component = gameObject.GetComponent<Pickupable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      Debug.LogError((object) ("Edible::OnTake() No Pickupable component for " + gameObject.name), (UnityEngine.Object) gameObject);
    gameObject.SetActive(true);
    component.TotalAmount = Mathf.Min(amount, pickupable.TotalAmount);
    bool keepZeroMassObject = pickupable.PrimaryElement.KeepZeroMassObject;
    pickupable.PrimaryElement.KeepZeroMassObject = true;
    pickupable.TotalAmount -= amount;
    component.Trigger(1335436905, (object) pickupable);
    pickupable.PrimaryElement.KeepZeroMassObject = keepZeroMassObject;
    pickupable.TotalAmount = pickupable.TotalAmount;
    if ((UnityEngine.Object) storage != (UnityEngine.Object) null)
    {
      storage.Trigger(-1697596308, (object) pickupable.gameObject);
      storage.Trigger(-778359855, (object) null);
    }
    return component;
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
      return;
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    PrimaryElement primaryElement = pickupable.PrimaryElement;
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    float temperature = 0.0f;
    float mass1 = component.Mass;
    float mass2 = primaryElement.Mass;
    if ((double) mass1 > 0.0 && (double) mass2 > 0.0)
      temperature = SimUtil.CalculateFinalTemperature(mass1, component.Temperature, mass2, primaryElement.Temperature);
    else if ((double) primaryElement.Mass > 0.0)
      temperature = primaryElement.Temperature;
    component.SetMassTemperature(mass1 + mass2, temperature);
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null))
      return;
    string sound = GlobalAssets.GetSound("Ore_absorb", false);
    if (sound == null || !CameraController.Instance.IsAudibleSound(pickupable.transform.GetPosition(), sound))
      return;
    this.PlaySound3D(sound);
  }
}
