// Decompiled with JetBrains decompiler
// Type: Database.RoomTypeCategories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Database
{
  public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
  {
    public RoomTypeCategory None;
    public RoomTypeCategory Food;
    public RoomTypeCategory Sleep;
    public RoomTypeCategory Recreation;
    public RoomTypeCategory Bathroom;
    public RoomTypeCategory Hospital;
    public RoomTypeCategory Industrial;
    public RoomTypeCategory Agricultural;
    public RoomTypeCategory Park;

    public RoomTypeCategories(ResourceSet parent)
      : base(nameof (RoomTypeCategories), parent)
    {
      this.Initialize();
      this.None = this.Add(nameof (None), string.Empty, Color.grey);
      this.Food = this.Add(nameof (Food), string.Empty, new Color(1f, 0.8862745f, 0.5176471f));
      this.Sleep = this.Add(nameof (Sleep), string.Empty, new Color(0.6392157f, 1f, 0.5176471f));
      this.Recreation = this.Add(nameof (Recreation), string.Empty, new Color(0.2588235f, 0.6431373f, 0.9568627f));
      this.Bathroom = this.Add(nameof (Bathroom), string.Empty, new Color(0.5176471f, 1f, 0.9568627f));
      this.Hospital = this.Add(nameof (Hospital), string.Empty, new Color(1f, 0.5176471f, 0.5568628f));
      this.Industrial = this.Add(nameof (Industrial), string.Empty, new Color(0.9568627f, 0.772549f, 0.2588235f));
      this.Agricultural = this.Add(nameof (Agricultural), string.Empty, new Color(0.8039216f, 0.9490196f, 0.282353f));
      this.Park = this.Add(nameof (Park), string.Empty, new Color(0.6745098f, 1f, 0.7411765f));
    }

    private RoomTypeCategory Add(string id, string name, Color color)
    {
      RoomTypeCategory resource = new RoomTypeCategory(id, name, color);
      this.Add(resource);
      return resource;
    }
  }
}
