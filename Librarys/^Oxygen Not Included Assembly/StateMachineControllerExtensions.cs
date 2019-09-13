// Decompiled with JetBrains decompiler
// Type: StateMachineControllerExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class StateMachineControllerExtensions
{
  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(
    this StateMachine.Instance smi)
    where StateMachineInstanceType : StateMachine.Instance
  {
    return smi.gameObject.GetSMI<StateMachineInstanceType>();
  }

  public static DefType GetDef<DefType>(this Component cmp) where DefType : StateMachine.BaseDef
  {
    return cmp.gameObject.GetDef<DefType>();
  }

  public static DefType GetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    if ((Object) component == (Object) null)
      return (DefType) null;
    return component.GetDef<DefType>();
  }

  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
  {
    return cmp.gameObject.GetSMI<StateMachineInstanceType>();
  }

  public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    if ((Object) component != (Object) null)
      return component.GetSMI<StateMachineInstanceType>();
    return (StateMachineInstanceType) null;
  }

  public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(
    this Component cmp)
    where StateMachineInstanceType : class
  {
    return cmp.gameObject.GetAllSMI<StateMachineInstanceType>();
  }

  public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(
    this GameObject go)
    where StateMachineInstanceType : class
  {
    StateMachineController component = go.GetComponent<StateMachineController>();
    if ((Object) component != (Object) null)
      return component.GetAllSMI<StateMachineInstanceType>();
    return new List<StateMachineInstanceType>();
  }
}
