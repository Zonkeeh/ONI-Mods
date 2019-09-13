// Decompiled with JetBrains decompiler
// Type: ElectricalUtilityNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalUtilityNetwork : UtilityNetwork
{
  private List<Wire>[] wireGroups = new List<Wire>[5];
  private Notification overloadedNotification;
  private const float MIN_OVERLOAD_TIME_FOR_DAMAGE = 6f;
  private const float MIN_OVERLOAD_NOTIFICATION_DISPLAY_TIME = 5f;
  private GameObject targetOverloadedWire;
  private float timeOverloaded;
  private float timeOverloadNotificationDisplayed;

  public override void AddItem(int cell, object item)
  {
    Wire wire = (Wire) item;
    Wire.WattageRating maxWattageRating = wire.MaxWattageRating;
    List<Wire> wireList = this.wireGroups[(int) maxWattageRating];
    if (wireList == null)
    {
      wireList = new List<Wire>();
      this.wireGroups[(int) maxWattageRating] = wireList;
    }
    wireList.Add(wire);
    this.timeOverloaded = Mathf.Max(this.timeOverloaded, wire.circuitOverloadTime);
  }

  public override void Reset(UtilityNetworkGridNode[] grid)
  {
    for (int index1 = 0; index1 < 5; ++index1)
    {
      List<Wire> wireGroup = this.wireGroups[index1];
      if (wireGroup != null)
      {
        for (int index2 = 0; index2 < wireGroup.Count; ++index2)
        {
          Wire wire = wireGroup[index2];
          if ((UnityEngine.Object) wire != (UnityEngine.Object) null)
          {
            wire.circuitOverloadTime = this.timeOverloaded;
            int cell = Grid.PosToCell(wire.transform.GetPosition());
            UtilityNetworkGridNode utilityNetworkGridNode = grid[cell];
            utilityNetworkGridNode.networkIdx = -1;
            grid[cell] = utilityNetworkGridNode;
          }
        }
        wireGroup.Clear();
      }
    }
    this.RemoveOverloadedNotification();
  }

  public void UpdateOverloadTime(
    float dt,
    float watts_used,
    List<WireUtilityNetworkLink>[] bridgeGroups)
  {
    bool flag = false;
    List<Wire> wireList = (List<Wire>) null;
    List<WireUtilityNetworkLink> utilityNetworkLinkList = (List<WireUtilityNetworkLink>) null;
    for (int index = 0; index < 5; ++index)
    {
      List<Wire> wireGroup = this.wireGroups[index];
      List<WireUtilityNetworkLink> bridgeGroup = bridgeGroups[index];
      float maxWattageAsFloat = Wire.GetMaxWattageAsFloat((Wire.WattageRating) index);
      if ((double) watts_used > (double) maxWattageAsFloat && (bridgeGroup != null && bridgeGroup.Count > 0 || wireGroup != null && wireGroup.Count > 0))
      {
        flag = true;
        wireList = wireGroup;
        utilityNetworkLinkList = bridgeGroup;
        break;
      }
    }
    wireList?.RemoveAll((Predicate<Wire>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null));
    utilityNetworkLinkList?.RemoveAll((Predicate<WireUtilityNetworkLink>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null));
    if (flag)
    {
      this.timeOverloaded += dt;
      if ((double) this.timeOverloaded <= 6.0)
        return;
      this.timeOverloaded = 0.0f;
      if ((UnityEngine.Object) this.targetOverloadedWire == (UnityEngine.Object) null)
      {
        if (utilityNetworkLinkList != null && utilityNetworkLinkList.Count > 0)
        {
          int index = UnityEngine.Random.Range(0, utilityNetworkLinkList.Count);
          this.targetOverloadedWire = utilityNetworkLinkList[index].gameObject;
        }
        else if (wireList != null && wireList.Count > 0)
        {
          int index = UnityEngine.Random.Range(0, wireList.Count);
          this.targetOverloadedWire = wireList[index].gameObject;
        }
      }
      if ((UnityEngine.Object) this.targetOverloadedWire != (UnityEngine.Object) null)
        this.targetOverloadedWire.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
        {
          damage = 1,
          source = (string) BUILDINGS.DAMAGESOURCES.CIRCUIT_OVERLOADED,
          popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CIRCUIT_OVERLOADED,
          takeDamageEffect = SpawnFXHashes.BuildingSpark,
          fullDamageEffectName = "spark_damage_kanim",
          statusItemID = Db.Get().BuildingStatusItems.Overloaded.Id
        });
      if (this.overloadedNotification != null)
        return;
      this.timeOverloadNotificationDisplayed = 0.0f;
      this.overloadedNotification = new Notification((string) MISC.NOTIFICATIONS.CIRCUIT_OVERLOADED.NAME, NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) null, (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, this.targetOverloadedWire.transform);
      Game.Instance.FindOrAdd<Notifier>().Add(this.overloadedNotification, string.Empty);
    }
    else
    {
      this.timeOverloaded = Mathf.Max(0.0f, this.timeOverloaded - dt * 0.95f);
      this.timeOverloadNotificationDisplayed += dt;
      if ((double) this.timeOverloadNotificationDisplayed <= 5.0)
        return;
      this.RemoveOverloadedNotification();
    }
  }

  private void RemoveOverloadedNotification()
  {
    if (this.overloadedNotification == null)
      return;
    Game.Instance.FindOrAdd<Notifier>().Remove(this.overloadedNotification);
    this.overloadedNotification = (Notification) null;
  }

  public float GetMaxSafeWattage()
  {
    for (int index = 0; index < this.wireGroups.Length; ++index)
    {
      List<Wire> wireGroup = this.wireGroups[index];
      if (wireGroup != null && wireGroup.Count > 0)
        return Wire.GetMaxWattageAsFloat((Wire.WattageRating) index);
    }
    return 0.0f;
  }

  public override void RemoveItem(int cell, object item)
  {
    if (item.GetType() != typeof (Wire))
      return;
    ((Wire) item).circuitOverloadTime = 0.0f;
  }
}
