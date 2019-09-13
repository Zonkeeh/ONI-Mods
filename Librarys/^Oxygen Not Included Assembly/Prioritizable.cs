// Decompiled with JetBrains decompiler
// Type: Prioritizable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Prioritizable : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Prioritizable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Prioritizable>((System.Action<Prioritizable, object>) ((component, data) => component.OnCopySettings(data)));
  private static Dictionary<PrioritySetting, PrioritySetting> conversions = new Dictionary<PrioritySetting, PrioritySetting>()
  {
    {
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 1),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 4)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 2),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 5)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 3),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 4),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 5),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.high, 1),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.high, 2),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.high, 3),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.high, 4),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
    },
    {
      new PrioritySetting(PriorityScreen.PriorityClass.high, 5),
      new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
    }
  };
  [SerializeField]
  [Serialize]
  private int masterPriority = int.MinValue;
  [SerializeField]
  [Serialize]
  private PrioritySetting masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
  public bool showIcon = true;
  public float iconScale = 1f;
  public System.Action<PrioritySetting> onPriorityChanged;
  public Vector2 iconOffset;
  [SerializeField]
  private int refCount;
  private HandleVector<int>.Handle scenePartitionerEntry;
  private Guid highPriorityStatusItem;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Prioritizable>(-905833192, Prioritizable.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    Prioritizable component = ((GameObject) data).GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.SetMasterPriority(component.GetMasterPriority());
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if (this.masterPriority != int.MinValue)
    {
      this.masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
      this.masterPriority = int.MinValue;
    }
    PrioritySetting prioritySetting;
    if (!SaveLoader.Instance.GameInfo.IsVersionExactly(7, 2) || !Prioritizable.conversions.TryGetValue(this.masterPrioritySetting, out prioritySetting))
      return;
    this.masterPrioritySetting = prioritySetting;
  }

  protected override void OnSpawn()
  {
    if (this.onPriorityChanged != null)
      this.onPriorityChanged(this.masterPrioritySetting);
    this.RefreshHighPriorityNotification();
    Vector3 position = this.transform.GetPosition();
    this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(this.name, (object) this, new Extents((int) position.x, (int) position.y, 1, 1), GameScenePartitioner.Instance.prioritizableObjects, (System.Action<object>) null);
    Components.Prioritizables.Add(this);
  }

  public PrioritySetting GetMasterPriority()
  {
    return this.masterPrioritySetting;
  }

  public void SetMasterPriority(PrioritySetting priority)
  {
    if (priority.Equals((object) this.masterPrioritySetting))
      return;
    this.masterPrioritySetting = priority;
    if (this.onPriorityChanged != null)
      this.onPriorityChanged(this.masterPrioritySetting);
    this.RefreshHighPriorityNotification();
  }

  public void AddRef()
  {
    ++this.refCount;
    this.RefreshHighPriorityNotification();
  }

  public void RemoveRef()
  {
    --this.refCount;
    this.RefreshHighPriorityNotification();
  }

  public bool IsPrioritizable()
  {
    return this.refCount > 0;
  }

  public bool IsTopPriority()
  {
    if (this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority)
      return this.IsPrioritizable();
    return false;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
    Components.Prioritizables.Remove(this);
  }

  public static void AddRef(GameObject go)
  {
    Prioritizable component = go.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.AddRef();
  }

  public static void RemoveRef(GameObject go)
  {
    Prioritizable component = go.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.RemoveRef();
  }

  private void RefreshHighPriorityNotification()
  {
    bool flag = this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
    if (flag && this.highPriorityStatusItem == Guid.Empty)
    {
      this.highPriorityStatusItem = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmergencyPriority, (object) null);
    }
    else
    {
      if (flag || !(this.highPriorityStatusItem != Guid.Empty))
        return;
      this.highPriorityStatusItem = this.GetComponent<KSelectable>().RemoveStatusItem(this.highPriorityStatusItem, false);
    }
  }
}
