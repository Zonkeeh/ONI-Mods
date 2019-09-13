// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.SettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.CustomSettings
{
  public abstract class SettingConfig
  {
    public SettingConfig(
      string id,
      string label,
      string tooltip,
      string default_level_id,
      string nosweat_default_level_id,
      int coordinate_dimension = -1,
      int coordinate_dimension_width = -1,
      bool debug_only = false,
      bool triggers_custom_game = true)
    {
      this.id = id;
      this.label = label;
      this.tooltip = tooltip;
      this.default_level_id = default_level_id;
      this.nosweat_default_level_id = nosweat_default_level_id;
      this.coordinate_dimension = coordinate_dimension;
      this.coordinate_dimension_width = coordinate_dimension_width;
      this.debug_only = debug_only;
      this.triggers_custom_game = triggers_custom_game;
    }

    public string id { get; private set; }

    public string label { get; private set; }

    public string tooltip { get; private set; }

    public string default_level_id { get; protected set; }

    public string nosweat_default_level_id { get; protected set; }

    public int coordinate_dimension { get; protected set; }

    public int coordinate_dimension_width { get; protected set; }

    public bool triggers_custom_game { get; protected set; }

    public bool debug_only { get; protected set; }

    public abstract SettingLevel GetLevel(string level_id);

    public abstract List<SettingLevel> GetLevels();

    public bool IsDefaultLevel(string level_id)
    {
      return level_id == this.default_level_id;
    }
  }
}
