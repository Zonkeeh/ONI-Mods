// Decompiled with JetBrains decompiler
// Type: GeneratedEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class GeneratedEquipment
{
  public static void LoadGeneratedEquipment()
  {
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new AtmoSuitConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new JetSuitConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new WarmVestConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new CoolVestConfig());
    EquipmentConfigManager.Instance.RegisterEquipment((IEquipmentConfig) new FunkyVestConfig());
  }
}
