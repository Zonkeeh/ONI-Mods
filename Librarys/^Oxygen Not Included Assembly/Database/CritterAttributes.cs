// Decompiled with JetBrains decompiler
// Type: Database.CritterAttributes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class CritterAttributes : ResourceSet<Attribute>
  {
    public Attribute Happiness;
    public Attribute Metabolism;

    public CritterAttributes(ResourceSet parent)
      : base(nameof (CritterAttributes), parent)
    {
      this.Happiness = this.Add(new Attribute(nameof (Happiness), false, Attribute.Display.General, false, 0.0f, (string) null, (string) null));
      this.Metabolism = this.Add(new Attribute(nameof (Metabolism), false, Attribute.Display.Details, false, 0.0f, (string) null, (string) null));
      this.Metabolism.SetFormatter((IAttributeFormatter) new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.None));
    }
  }
}
