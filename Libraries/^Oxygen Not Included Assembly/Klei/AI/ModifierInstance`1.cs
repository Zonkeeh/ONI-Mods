// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifierInstance`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class ModifierInstance<ModifierType> : IStateMachineTarget
  {
    public ModifierType modifier;

    public ModifierInstance(GameObject game_object, ModifierType modifier)
    {
      this.gameObject = game_object;
      this.modifier = modifier;
    }

    public GameObject gameObject { get; private set; }

    public ComponentType GetComponent<ComponentType>()
    {
      return this.gameObject.GetComponent<ComponentType>();
    }

    public int Subscribe(int hash, System.Action<object> handler)
    {
      return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);
    }

    public void Unsubscribe(int hash, System.Action<object> handler)
    {
      this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);
    }

    public void Unsubscribe(int id)
    {
      this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);
    }

    public void Trigger(int hash, object data = null)
    {
      this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);
    }

    public Transform transform
    {
      get
      {
        return this.gameObject.transform;
      }
    }

    public bool isNull
    {
      get
      {
        return (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null;
      }
    }

    public string name
    {
      get
      {
        return this.gameObject.name;
      }
    }

    public virtual void OnCleanUp()
    {
    }
  }
}
