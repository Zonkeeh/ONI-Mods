// Decompiled with JetBrains decompiler
// Type: RequireInputs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[SkipSaveFileSerialization]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
  private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);
  private static readonly Operational.Flag pipesHaveMass = new Operational.Flag(nameof (pipesHaveMass), Operational.Flag.Type.Requirement);
  [SerializeField]
  private bool requirePower = true;
  public bool requireConduitHasMass = true;
  public bool visualizeRequirements = true;
  private bool previouslyConnected = true;
  private bool previouslySatisfied = true;
  [SerializeField]
  private bool requireConduit;
  private bool wasConnected;
  private Guid wireConnectedStatusItem;
  private bool requirementsMet;
  private BuildingEnabledButton button;
  private IEnergyConsumer energy;
  public ConduitConsumer conduitConsumer;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpGet]
  private Operational operational;

  public bool RequiresPower
  {
    get
    {
      return this.requirePower;
    }
  }

  public bool RequiresInputConduit
  {
    get
    {
      return this.requireConduit;
    }
  }

  public void SetRequirements(bool power, bool conduit)
  {
    this.requirePower = power;
    this.requireConduit = conduit;
  }

  public bool RequirementsMet
  {
    get
    {
      return this.requirementsMet;
    }
  }

  protected override void OnPrefabInit()
  {
    this.Bind();
  }

  protected override void OnSpawn()
  {
    this.CheckRequirements(true);
    this.Bind();
  }

  [ContextMenu("Bind")]
  private void Bind()
  {
    if (this.requirePower)
    {
      this.energy = this.GetComponent<IEnergyConsumer>();
      this.button = this.GetComponent<BuildingEnabledButton>();
    }
    if (!this.requireConduit || (bool) ((UnityEngine.Object) this.conduitConsumer))
      return;
    this.conduitConsumer = this.GetComponent<ConduitConsumer>();
  }

  public void Sim200ms(float dt)
  {
    this.CheckRequirements(false);
  }

  private void CheckRequirements(bool forceEvent)
  {
    bool flag1 = true;
    bool flag2 = false;
    if (this.requirePower)
    {
      bool isConnected = this.energy.IsConnected;
      bool isPowered = this.energy.IsPowered;
      if (isConnected)
      {
        if (isConnected != this.wasConnected)
          this.wireConnectedStatusItem = this.selectable.RemoveStatusItem(this.wireConnectedStatusItem, false);
        if (this.visualizeRequirements)
          this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, !isPowered && ((UnityEngine.Object) this.button == (UnityEngine.Object) null || this.button.IsEnabled), (object) this);
        flag1 = flag1 && isPowered;
      }
      else
      {
        if (isConnected != this.wasConnected)
          this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.NeedPower, false);
        if (this.wireConnectedStatusItem == Guid.Empty)
          this.wireConnectedStatusItem = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, (object) null);
        flag1 = flag1 && isConnected;
      }
      this.wasConnected = isConnected;
      flag2 = flag1 != this.RequirementsMet && (UnityEngine.Object) this.GetComponent<Light2D>() != (UnityEngine.Object) null;
    }
    if (this.requireConduit && this.visualizeRequirements)
    {
      bool flag3 = !this.conduitConsumer.enabled || this.conduitConsumer.IsConnected;
      bool flag4 = !this.conduitConsumer.enabled || this.conduitConsumer.IsSatisfied;
      if (this.previouslyConnected != flag3)
      {
        this.previouslyConnected = flag3;
        StatusItem status_item = (StatusItem) null;
        switch (this.conduitConsumer.TypeOfConduit)
        {
          case ConduitType.Gas:
            status_item = Db.Get().BuildingStatusItems.NeedGasIn;
            break;
          case ConduitType.Liquid:
            status_item = Db.Get().BuildingStatusItems.NeedLiquidIn;
            break;
        }
        if (status_item != null)
          this.selectable.ToggleStatusItem(status_item, !flag3, (object) new Tuple<ConduitType, Tag>(this.conduitConsumer.TypeOfConduit, this.conduitConsumer.capacityTag));
        this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag3);
      }
      flag1 = flag1 && flag3;
      if (this.previouslySatisfied != flag4)
      {
        this.previouslySatisfied = flag4;
        StatusItem status_item = (StatusItem) null;
        switch (this.conduitConsumer.TypeOfConduit)
        {
          case ConduitType.Gas:
            status_item = Db.Get().BuildingStatusItems.GasPipeEmpty;
            break;
          case ConduitType.Liquid:
            status_item = Db.Get().BuildingStatusItems.LiquidPipeEmpty;
            break;
        }
        if (this.requireConduitHasMass)
        {
          if (status_item != null)
            this.selectable.ToggleStatusItem(status_item, !flag4, (object) this);
          this.operational.SetFlag(RequireInputs.pipesHaveMass, flag4);
        }
      }
    }
    this.requirementsMet = flag1;
    if (!flag2)
      return;
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject);
    if (roomOfGameObject == null)
      return;
    Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
  }
}
