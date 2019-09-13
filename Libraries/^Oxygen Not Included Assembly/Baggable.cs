// Decompiled with JetBrains decompiler
// Type: Baggable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class Baggable : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Baggable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Baggable>((System.Action<Baggable, object>) ((component, data) => component.OnStore(data)));
  [SerializeField]
  private KAnimFile minionAnimOverride;
  public bool mustStandOntopOfTrapForPickup;
  [Serialize]
  public bool wrangled;
  public bool useGunForPickup;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.minionAnimOverride = Assets.GetAnim((HashedString) "anim_restrain_creature_kanim");
    Pickupable pickupable = this.gameObject.AddOrGet<Pickupable>();
    pickupable.workAnims = new HashedString[2]
    {
      new HashedString("capture"),
      new HashedString("pickup")
    };
    pickupable.workAnimPlayMode = KAnim.PlayMode.Once;
    pickupable.workingPstComplete = HashedString.Invalid;
    pickupable.workingPstFailed = HashedString.Invalid;
    pickupable.overrideAnims = new KAnimFile[1]
    {
      this.minionAnimOverride
    };
    pickupable.trackOnPickup = false;
    pickupable.useGunforPickup = this.useGunForPickup;
    pickupable.synchronizeAnims = false;
    pickupable.SetWorkTime(3f);
    if (this.mustStandOntopOfTrapForPickup)
      pickupable.SetOffsets(new CellOffset[2]
      {
        new CellOffset(),
        new CellOffset(0, -1)
      });
    this.Subscribe<Baggable>(856640610, Baggable.OnStoreDelegate);
    if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.transform.parent.GetComponent<Trap>() != (UnityEngine.Object) null)
        this.GetComponent<KBatchedAnimController>().enabled = true;
      if ((UnityEngine.Object) this.transform.parent.GetComponent<EggIncubator>() != (UnityEngine.Object) null)
        this.wrangled = true;
    }
    if (!this.wrangled)
      return;
    this.SetWrangled();
  }

  private void OnStore(object data)
  {
    Storage cmp = data as Storage;
    if ((UnityEngine.Object) cmp != (UnityEngine.Object) null || data != null && (bool) data)
    {
      this.gameObject.AddTag(GameTags.Creatures.Bagged);
      if (!(bool) ((UnityEngine.Object) cmp) || !cmp.HasTag(GameTags.Minion))
        return;
      this.SetVisible(false);
    }
    else
      this.Free();
  }

  private void SetVisible(bool visible)
  {
    KAnimControllerBase component1 = this.gameObject.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.enabled != visible)
      component1.enabled = visible;
    KSelectable component2 = this.gameObject.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || component2.enabled == visible)
      return;
    component2.enabled = visible;
  }

  public void SetWrangled()
  {
    this.wrangled = true;
    Navigator component = this.GetComponent<Navigator>();
    if ((bool) ((UnityEngine.Object) component) && component.IsValidNavType(NavType.Floor))
      component.SetCurrentNavType(NavType.Floor);
    this.gameObject.AddTag(GameTags.Creatures.Bagged);
  }

  public void Free()
  {
    this.gameObject.RemoveTag(GameTags.Creatures.Bagged);
    this.wrangled = false;
    this.SetVisible(true);
  }
}
