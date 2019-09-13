// Decompiled with JetBrains decompiler
// Type: DeserializeWarnings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DeserializeWarnings : KMonoBehaviour
{
  public DeserializeWarnings.Warning BuildingTemeperatureIsZeroKelvin;
  public DeserializeWarnings.Warning PipeContentsTemperatureIsNan;
  public DeserializeWarnings.Warning PrimaryElementTemperatureIsNan;
  public DeserializeWarnings.Warning PrimaryElementHasNoElement;
  public static DeserializeWarnings Instance;

  public static void DestroyInstance()
  {
    DeserializeWarnings.Instance = (DeserializeWarnings) null;
  }

  protected override void OnPrefabInit()
  {
    DeserializeWarnings.Instance = this;
  }

  public struct Warning
  {
    private bool isSet;

    public void Warn(string message, GameObject obj = null)
    {
      if (this.isSet)
        return;
      Debug.LogWarning((object) message, (Object) obj);
      this.isSet = true;
    }
  }
}
