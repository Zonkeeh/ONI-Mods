// Decompiled with JetBrains decompiler
// Type: KMod.Testing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace KMod
{
  public static class Testing
  {
    public static Testing.DLLLoading dll_loading;
    public const Testing.SaveLoad SAVE_LOAD = Testing.SaveLoad.NoTesting;
    public const Testing.Install INSTALL = Testing.Install.NoTesting;
    public const Testing.Boot BOOT = Testing.Boot.NoTesting;

    public enum DLLLoading
    {
      NoTesting,
      Fail,
      UseModLoaderDLLExclusively,
    }

    public enum SaveLoad
    {
      NoTesting,
      FailSave,
      FailLoad,
    }

    public enum Install
    {
      NoTesting,
      ForceUninstall,
      ForceReinstall,
      ForceUpdate,
    }

    public enum Boot
    {
      NoTesting,
      Crash,
    }
  }
}
