// Decompiled with JetBrains decompiler
// Type: Sensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sensor
{
  protected Sensors sensors;

  public Sensor(Sensors sensors)
  {
    this.sensors = sensors;
    this.Name = this.GetType().Name;
  }

  public string Name { get; private set; }

  public ComponentType GetComponent<ComponentType>()
  {
    return this.sensors.GetComponent<ComponentType>();
  }

  public GameObject gameObject
  {
    get
    {
      return this.sensors.gameObject;
    }
  }

  public Transform transform
  {
    get
    {
      return this.gameObject.transform;
    }
  }

  public void Trigger(int hash, object data = null)
  {
    this.sensors.Trigger(hash, data);
  }

  public virtual void Update()
  {
  }

  public virtual void ShowEditor()
  {
  }
}
