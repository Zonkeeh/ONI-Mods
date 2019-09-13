// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.SettingLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.CustomSettings
{
  public class SettingLevel
  {
    public SettingLevel(
      string id,
      string label,
      string tooltip,
      int coordinate_offset = 0,
      object userdata = null)
    {
      this.id = id;
      this.label = label;
      this.tooltip = tooltip;
      this.userdata = userdata;
      this.coordinate_offset = coordinate_offset;
    }

    public string id { get; private set; }

    public string tooltip { get; private set; }

    public string label { get; private set; }

    public object userdata { get; private set; }

    public int coordinate_offset { get; private set; }
  }
}
