// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.SeedSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.CustomSettings
{
  public class SeedSettingConfig : SettingConfig
  {
    public SeedSettingConfig(
      string id,
      string label,
      string tooltip,
      bool debug_only = false,
      bool triggers_custom_game = true)
      : base(id, label, tooltip, string.Empty, string.Empty, -1, -1, debug_only, triggers_custom_game)
    {
    }

    public override SettingLevel GetLevel(string level_id)
    {
      return new SettingLevel(level_id, level_id, level_id, 0, (object) null);
    }

    public override List<SettingLevel> GetLevels()
    {
      return new List<SettingLevel>();
    }
  }
}
