// Decompiled with JetBrains decompiler
// Type: LaunchConditionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LaunchConditionManager : KMonoBehaviour, ISim4000ms, ISim1000ms
{
  private Dictionary<RocketFlightCondition, Guid> conditionStatuses = new Dictionary<RocketFlightCondition, Guid>();
  public HashedString triggerPort;
  public HashedString statusPort;
  private LaunchableRocket launchable;
  [Serialize]
  private List<Tuple<string, string, string>> DEBUG_ModuleDestructions;

  public List<RocketModule> rocketModules { get; private set; }

  public void DEBUG_TraceModuleDestruction(string moduleName, string state, string stackTrace)
  {
    if (this.DEBUG_ModuleDestructions == null)
      this.DEBUG_ModuleDestructions = new List<Tuple<string, string, string>>();
    this.DEBUG_ModuleDestructions.Add(new Tuple<string, string, string>(moduleName, state, stackTrace));
  }

  [ContextMenu("Dump Module Destructions")]
  private void DEBUG_DumpModuleDestructions()
  {
    if (this.DEBUG_ModuleDestructions == null || this.DEBUG_ModuleDestructions.Count == 0)
    {
      DebugUtil.LogArgs((object) "Sorry, no logged module destructions. :(");
    }
    else
    {
      foreach (Tuple<string, string, string> moduleDestruction in this.DEBUG_ModuleDestructions)
        DebugUtil.LogArgs((object) moduleDestruction.first, (object) ">", (object) moduleDestruction.second, (object) "\n", (object) moduleDestruction.third, (object) "\nEND MODULE DUMP\n\n");
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.rocketModules = new List<RocketModule>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.launchable = this.GetComponent<LaunchableRocket>();
    this.FindModules();
    this.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged = (System.Action<AttachableBuilding>) (data => this.FindModules());
    this.Subscribe(-1582839653, new System.Action<object>(this.OnTagsChanged));
  }

  private void OnTagsChanged(object data)
  {
    foreach (RocketModule rocketModule in this.rocketModules)
      rocketModule.OnConditionManagerTagsChanged(data);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  public void Sim1000ms(float dt)
  {
    Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
    if (conditionManager == null)
      return;
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(conditionManager.id);
    if (this.gameObject.GetComponent<LogicPorts>().GetInputValue(this.triggerPort) != 1 || spacecraftDestination == null || spacecraftDestination.id == -1)
      return;
    this.Launch(spacecraftDestination);
  }

  public void FindModules()
  {
    List<GameObject> attachedNetwork = AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>());
    foreach (GameObject gameObject in attachedNetwork)
    {
      RocketModule component = gameObject.GetComponent<RocketModule>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.conditionManager == (UnityEngine.Object) null)
      {
        component.conditionManager = this;
        component.RegisterWithConditionManager();
      }
    }
    Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
    if (conditionManager == null)
      return;
    conditionManager.moduleCount = attachedNetwork.Count;
  }

  public void RegisterRocketModule(RocketModule module)
  {
    if (this.rocketModules.Contains(module))
      return;
    this.rocketModules.Add(module);
  }

  public void UnregisterRocketModule(RocketModule module)
  {
    this.rocketModules.Remove(module);
  }

  public List<RocketLaunchCondition> GetLaunchConditionList()
  {
    List<RocketLaunchCondition> rocketLaunchConditionList = new List<RocketLaunchCondition>();
    foreach (RocketModule rocketModule in this.rocketModules)
    {
      foreach (RocketLaunchCondition launchCondition in rocketModule.launchConditions)
        rocketLaunchConditionList.Add(launchCondition);
    }
    return rocketLaunchConditionList;
  }

  public void Launch(SpaceDestination destination)
  {
    if (destination == null)
      Debug.LogError((object) "Null destination passed to launch");
    if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).state != Spacecraft.MissionState.Grounded || !DebugHandler.InstantBuildMode && (!this.CheckReadyToLaunch() || !this.CheckAbleToFly()))
      return;
    this.launchable.Trigger(-1056989049, (object) null);
    SpacecraftManager.instance.SetSpacecraftDestination(this, destination);
    SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).BeginMission(destination);
  }

  public bool CheckReadyToLaunch()
  {
    foreach (RocketModule rocketModule in this.rocketModules)
    {
      foreach (RocketLaunchCondition launchCondition in rocketModule.launchConditions)
      {
        if (launchCondition.EvaluateLaunchCondition() == RocketLaunchCondition.LaunchStatus.Failure)
          return false;
      }
    }
    return true;
  }

  public bool CheckAbleToFly()
  {
    foreach (RocketModule rocketModule in this.rocketModules)
    {
      foreach (RocketFlightCondition flightCondition in rocketModule.flightConditions)
      {
        if (!flightCondition.EvaluateFlightCondition())
          return false;
      }
    }
    return true;
  }

  private void ClearFlightStatuses()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    foreach (KeyValuePair<RocketFlightCondition, Guid> conditionStatuse in this.conditionStatuses)
      component.RemoveStatusItem(conditionStatuse.Value, false);
    this.conditionStatuses.Clear();
  }

  public void Sim4000ms(float dt)
  {
    bool launch = this.CheckReadyToLaunch();
    LogicPorts component1 = this.gameObject.GetComponent<LogicPorts>();
    if (launch)
    {
      Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
      if (conditionManager.state == Spacecraft.MissionState.Grounded || conditionManager.state == Spacecraft.MissionState.Launching)
        component1.SendSignal(this.statusPort, 1);
      else
        component1.SendSignal(this.statusPort, 0);
      KSelectable component2 = this.GetComponent<KSelectable>();
      foreach (RocketModule rocketModule in this.rocketModules)
      {
        foreach (RocketFlightCondition flightCondition in rocketModule.flightConditions)
        {
          if (!flightCondition.EvaluateFlightCondition())
          {
            if (!this.conditionStatuses.ContainsKey(flightCondition))
            {
              StatusItem failureStatusItem = flightCondition.GetFailureStatusItem();
              this.conditionStatuses[flightCondition] = component2.AddStatusItem(failureStatusItem, (object) flightCondition);
            }
          }
          else if (this.conditionStatuses.ContainsKey(flightCondition))
          {
            component2.RemoveStatusItem(this.conditionStatuses[flightCondition], false);
            this.conditionStatuses.Remove(flightCondition);
          }
        }
      }
    }
    else
    {
      this.ClearFlightStatuses();
      component1.SendSignal(this.statusPort, 0);
    }
  }
}
