// Decompiled with JetBrains decompiler
// Type: Sensors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class Sensors : KMonoBehaviour
{
  public List<Sensor> sensors = new List<Sensor>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<Brain>().onPreUpdate += new System.Action(this.OnBrainPreUpdate);
  }

  public SensorType GetSensor<SensorType>() where SensorType : Sensor
  {
    foreach (Sensor sensor in this.sensors)
    {
      if (typeof (SensorType).IsAssignableFrom(sensor.GetType()))
        return (SensorType) sensor;
    }
    Debug.LogError((object) ("Missing sensor of type: " + typeof (SensorType).Name));
    return (SensorType) null;
  }

  public void Add(Sensor sensor)
  {
    this.sensors.Add(sensor);
    sensor.Update();
  }

  public void UpdateSensors()
  {
    foreach (Sensor sensor in this.sensors)
      sensor.Update();
  }

  private void OnBrainPreUpdate()
  {
    this.UpdateSensors();
  }

  public void ShowEditor()
  {
    foreach (Sensor sensor in this.sensors)
      sensor.ShowEditor();
  }
}
