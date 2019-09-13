// Decompiled with JetBrains decompiler
// Type: GeneratedBuildings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedBuildings
{
  public static void LoadGeneratedBuildings(List<System.Type> types)
  {
    System.Type type1 = typeof (IBuildingConfig);
    List<System.Type> typeList = new List<System.Type>();
    foreach (System.Type type2 in types)
    {
      if (type1.IsAssignableFrom(type2) && !type2.IsAbstract && !type2.IsInterface)
        typeList.Add(type2);
    }
    foreach (System.Type type2 in typeList)
    {
      object instance = Activator.CreateInstance(type2);
      BuildingConfigManager.Instance.RegisterBuilding(instance as IBuildingConfig);
    }
  }

  public static void MakeBuildingAlwaysOperational(GameObject go)
  {
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<BuildingEnabledButton>());
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<Operational>());
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<LogicPorts>());
  }

  public static void RemoveLoopingSounds(GameObject go)
  {
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<LoopingSounds>());
  }

  public static void RemoveDefaultLogicPorts(GameObject go)
  {
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go.GetComponent<LogicPorts>());
  }

  public static void RegisterWithOverlay(HashSet<Tag> overlay_tags, string id)
  {
    overlay_tags.Add(new Tag(id));
    overlay_tags.Add(new Tag(id + "UnderConstruction"));
  }

  public static void RegisterLogicPorts(
    GameObject go,
    LogicPorts.Port[] inputs,
    LogicPorts.Port[] outputs)
  {
    LogicPorts logicPorts = go.AddOrGet<LogicPorts>();
    logicPorts.inputPortInfo = inputs;
    logicPorts.outputPortInfo = outputs;
  }

  public static void RegisterLogicPorts(GameObject go, LogicPorts.Port[] inputs)
  {
    LogicPorts logicPorts = go.AddOrGet<LogicPorts>();
    logicPorts.inputPortInfo = inputs;
    logicPorts.outputPortInfo = (LogicPorts.Port[]) null;
  }

  public static void RegisterLogicPorts(GameObject go, LogicPorts.Port output)
  {
    LogicPorts logicPorts = go.AddOrGet<LogicPorts>();
    logicPorts.inputPortInfo = (LogicPorts.Port[]) null;
    logicPorts.outputPortInfo = new LogicPorts.Port[1]
    {
      output
    };
  }
}
