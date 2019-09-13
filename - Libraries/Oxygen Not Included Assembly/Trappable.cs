// Decompiled with JetBrains decompiler
// Type: Trappable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class Trappable : KMonoBehaviour, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Trappable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Trappable>((System.Action<Trappable, object>) ((component, data) => component.OnStore(data)));
  private bool registered;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Register();
    this.OnCellChange();
  }

  protected override void OnCleanUp()
  {
    this.Unregister();
    base.OnCleanUp();
  }

  private void OnCellChange()
  {
    GameScenePartitioner.Instance.TriggerEvent(Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.trapsLayer, (object) this);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.Register();
  }

  protected override void OnCmpDisable()
  {
    this.Unregister();
    base.OnCmpDisable();
  }

  private void Register()
  {
    if (this.registered)
      return;
    this.Subscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "Trappable.Register");
    this.registered = true;
  }

  private void Unregister()
  {
    if (!this.registered)
      return;
    this.Unsubscribe<Trappable>(856640610, Trappable.OnStoreDelegate, false);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    this.registered = false;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_TRAP, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_TRAP, Descriptor.DescriptorType.Effect, false)
    };
  }

  public void OnStore(object data)
  {
    Storage storage = data as Storage;
    if ((bool) (!(bool) ((UnityEngine.Object) storage) ? (UnityEngine.Object) null : (UnityEngine.Object) storage.GetComponent<Trap>()))
      this.gameObject.AddTag(GameTags.Trapped);
    else
      this.gameObject.RemoveTag(GameTags.Trapped);
  }
}
