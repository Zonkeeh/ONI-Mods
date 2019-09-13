// Decompiled with JetBrains decompiler
// Type: OreSizeVisualizerComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OreSizeVisualizerComponents : KGameObjectComponentManager<OreSizeVisualizerData>
{
  private static readonly OreSizeVisualizerComponents.MassTier[] MassTiers = new OreSizeVisualizerComponents.MassTier[3]
  {
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle1",
      massRequired = 50f,
      colliderRadius = 0.15f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle2",
      massRequired = 600f,
      colliderRadius = 0.2f
    },
    new OreSizeVisualizerComponents.MassTier()
    {
      animName = (HashedString) "idle3",
      massRequired = float.MaxValue,
      colliderRadius = 0.25f
    }
  };

  public HandleVector<int>.Handle Add(GameObject go)
  {
    HandleVector<int>.Handle h = this.Add(go, new OreSizeVisualizerData(go));
    this.OnPrefabInit(h);
    return h;
  }

  protected override void OnPrefabInit(HandleVector<int>.Handle handle)
  {
    System.Action<object> handler = (System.Action<object>) (ev_data => OreSizeVisualizerComponents.OnMassChanged(handle, ev_data));
    OreSizeVisualizerData data = this.GetData(handle);
    data.onMassChangedCB = handler;
    data.primaryElement.Subscribe(-2064133523, handler);
    data.primaryElement.Subscribe(1335436905, handler);
    this.SetData(handle, data);
  }

  protected override void OnSpawn(HandleVector<int>.Handle handle)
  {
    OreSizeVisualizerData data = this.GetData(handle);
    OreSizeVisualizerComponents.OnMassChanged(handle, (object) data.primaryElement.GetComponent<Pickupable>());
  }

  protected override void OnCleanUp(HandleVector<int>.Handle handle)
  {
    OreSizeVisualizerData data = this.GetData(handle);
    if (!((UnityEngine.Object) data.primaryElement != (UnityEngine.Object) null))
      return;
    System.Action<object> onMassChangedCb = data.onMassChangedCB;
    data.primaryElement.Unsubscribe(-2064133523, onMassChangedCb);
    data.primaryElement.Unsubscribe(1335436905, onMassChangedCb);
  }

  private static void OnMassChanged(HandleVector<int>.Handle handle, object other_data)
  {
    PrimaryElement primaryElement = GameComps.OreSizeVisualizers.GetData(handle).primaryElement;
    float mass = primaryElement.Mass;
    if (other_data != null)
    {
      PrimaryElement component = ((Component) other_data).GetComponent<PrimaryElement>();
      mass += component.Mass;
    }
    OreSizeVisualizerComponents.MassTier massTier = new OreSizeVisualizerComponents.MassTier();
    for (int index = 0; index < OreSizeVisualizerComponents.MassTiers.Length; ++index)
    {
      if ((double) mass <= (double) OreSizeVisualizerComponents.MassTiers[index].massRequired)
      {
        massTier = OreSizeVisualizerComponents.MassTiers[index];
        break;
      }
    }
    primaryElement.GetComponent<KBatchedAnimController>().Play(massTier.animName, KAnim.PlayMode.Once, 1f, 0.0f);
    KCircleCollider2D component1 = primaryElement.GetComponent<KCircleCollider2D>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.radius = massTier.colliderRadius;
    primaryElement.Trigger(1807976145, (object) null);
  }

  private struct MassTier
  {
    public HashedString animName;
    public float massRequired;
    public float colliderRadius;
  }
}
