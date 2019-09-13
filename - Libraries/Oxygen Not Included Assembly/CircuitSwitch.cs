// Decompiled with JetBrains decompiler
// Type: CircuitSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch
{
  [SerializeField]
  public ObjectLayer objectLayer;
  private Wire attachedWire;
  private Guid wireConnectedGUID;
  private bool wasOn;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.CircuitOnToggle);
    int cell = Grid.PosToCell(this.transform.GetPosition());
    GameObject gameObject = Grid.Objects[cell, (int) this.objectLayer];
    Wire wire = !((UnityEngine.Object) gameObject != (UnityEngine.Object) null) ? (Wire) null : gameObject.GetComponent<Wire>();
    if ((UnityEngine.Object) wire == (UnityEngine.Object) null)
      this.wireConnectedGUID = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, (object) null);
    this.AttachWire(wire);
    this.wasOn = this.switchedOn;
    this.UpdateCircuit(true);
    this.GetComponent<KBatchedAnimController>().Play((HashedString) (!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
      this.UnsubscribeFromWire(this.attachedWire);
    bool switchedOn = this.switchedOn;
    this.switchedOn = true;
    this.UpdateCircuit(false);
    this.switchedOn = switchedOn;
  }

  public bool IsConnected()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    GameObject gameObject = Grid.Objects[cell, (int) this.objectLayer];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      return gameObject.GetComponent<IDisconnectable>() != null;
    return false;
  }

  private void CircuitOnToggle(bool on)
  {
    this.UpdateCircuit(true);
  }

  public void AttachWire(Wire wire)
  {
    if ((UnityEngine.Object) wire == (UnityEngine.Object) this.attachedWire)
      return;
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
      this.UnsubscribeFromWire(this.attachedWire);
    this.attachedWire = wire;
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
    {
      this.SubscribeToWire(this.attachedWire);
      this.UpdateCircuit(true);
      this.wireConnectedGUID = this.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID, false);
    }
    else
    {
      if (!(this.wireConnectedGUID == Guid.Empty))
        return;
      this.wireConnectedGUID = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, (object) null);
    }
  }

  private void OnWireDestroyed(object data)
  {
    if (!((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null))
      return;
    this.attachedWire.Unsubscribe(1969584890, new System.Action<object>(this.OnWireDestroyed));
  }

  private void OnWireStateChanged(object data)
  {
    this.UpdateCircuit(true);
  }

  private void SubscribeToWire(Wire wire)
  {
    wire.Subscribe(1969584890, new System.Action<object>(this.OnWireDestroyed));
    wire.Subscribe(-1735440190, new System.Action<object>(this.OnWireStateChanged));
    wire.Subscribe(774203113, new System.Action<object>(this.OnWireStateChanged));
  }

  private void UnsubscribeFromWire(Wire wire)
  {
    wire.Unsubscribe(1969584890, new System.Action<object>(this.OnWireDestroyed));
    wire.Unsubscribe(-1735440190, new System.Action<object>(this.OnWireStateChanged));
    wire.Unsubscribe(774203113, new System.Action<object>(this.OnWireStateChanged));
  }

  private void UpdateCircuit(bool should_update_anim = true)
  {
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
    {
      if (this.switchedOn)
        this.attachedWire.Connect();
      else
        this.attachedWire.Disconnect();
    }
    if (should_update_anim && this.wasOn != this.switchedOn)
    {
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      component.Play((HashedString) (!this.switchedOn ? "on_pst" : "on_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
      component.Queue((HashedString) (!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
      Game.Instance.userMenu.Refresh(this.gameObject);
    }
    this.wasOn = this.switchedOn;
  }
}
