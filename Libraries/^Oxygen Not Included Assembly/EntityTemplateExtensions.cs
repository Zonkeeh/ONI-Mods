// Decompiled with JetBrains decompiler
// Type: EntityTemplateExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class EntityTemplateExtensions
{
  public static DefType AddOrGetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
  {
    StateMachineController machineController = go.AddOrGet<StateMachineController>();
    DefType defType = machineController.GetDef<DefType>();
    if ((object) defType == null)
    {
      defType = Activator.CreateInstance<DefType>();
      machineController.AddDef((StateMachine.BaseDef) defType);
      defType.Configure(machineController.gameObject);
    }
    return defType;
  }

  public static ComponentType AddOrGet<ComponentType>(this GameObject go) where ComponentType : Component
  {
    ComponentType componentType = go.GetComponent<ComponentType>();
    if ((UnityEngine.Object) componentType == (UnityEngine.Object) null)
      componentType = go.AddComponent<ComponentType>();
    KMonoBehaviour kmonoBehaviour = (object) componentType as KMonoBehaviour;
    if ((UnityEngine.Object) kmonoBehaviour != (UnityEngine.Object) null)
      kmonoBehaviour.CreateDef();
    return componentType;
  }
}
