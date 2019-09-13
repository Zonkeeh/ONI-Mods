// Decompiled with JetBrains decompiler
// Type: Sounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;

public class Sounds : KMonoBehaviour
{
  public FMODAsset BlowUp_Generic;
  public FMODAsset Build_Generic;
  public FMODAsset InUse_Fabricator;
  public FMODAsset InUse_OxygenGenerator;
  public FMODAsset Place_OreOnSite;
  public FMODAsset Footstep_rock;
  public FMODAsset Ice_crack;
  public FMODAsset BuildingPowerOn;
  public FMODAsset ElectricGridOverload;
  public FMODAsset IngameMusic;
  public FMODAsset[] OreSplashSounds;
  [EventRef]
  public string BlowUp_GenericMigrated;
  [EventRef]
  public string Build_GenericMigrated;
  [EventRef]
  public string InUse_FabricatorMigrated;
  [EventRef]
  public string InUse_OxygenGeneratorMigrated;
  [EventRef]
  public string Place_OreOnSiteMigrated;
  [EventRef]
  public string Footstep_rockMigrated;
  [EventRef]
  public string Ice_crackMigrated;
  [EventRef]
  public string BuildingPowerOnMigrated;
  [EventRef]
  public string ElectricGridOverloadMigrated;
  [EventRef]
  public string IngameMusicMigrated;
  [EventRef]
  public string[] OreSplashSoundsMigrated;

  public static Sounds Instance { get; private set; }

  public static void DestroyInstance()
  {
    Sounds.Instance = (Sounds) null;
  }

  protected override void OnPrefabInit()
  {
    Sounds.Instance = this;
  }
}
