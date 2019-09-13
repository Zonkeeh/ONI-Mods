// Decompiled with JetBrains decompiler
// Type: BudUprootedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

public class BudUprootedMonitor : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<BudUprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BudUprootedMonitor>((System.Action<BudUprootedMonitor, object>) ((component, data) =>
  {
    if (component.uprooted)
      return;
    component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
    component.uprooted = true;
    component.Trigger(-216549700, (object) null);
  }));
  [Serialize]
  public bool canBeUprooted = true;
  public Ref<KPrefabID> parentObject = new Ref<KPrefabID>();
  [Serialize]
  private bool uprooted;
  private HandleVector<int>.Handle partitionerEntry;

  public bool IsUprooted
  {
    get
    {
      if (!this.uprooted)
        return this.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
      return true;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<BudUprootedMonitor>(-216549700, BudUprootedMonitor.OnUprootedDelegate);
  }

  public void SetParentObject(KPrefabID id)
  {
    this.parentObject = new Ref<KPrefabID>(id);
    this.Subscribe(id.gameObject, 1969584890, new System.Action<object>(this.OnLoseParent));
  }

  private void OnLoseParent(object obj)
  {
    if (this.uprooted || this.isNull)
      return;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
    this.uprooted = true;
    this.Trigger(-216549700, (object) null);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  public static bool IsObjectUprooted(GameObject plant)
  {
    BudUprootedMonitor component = plant.GetComponent<BudUprootedMonitor>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return false;
    return component.IsUprooted;
  }
}
