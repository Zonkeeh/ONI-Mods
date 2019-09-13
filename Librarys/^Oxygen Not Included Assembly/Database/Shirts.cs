// Decompiled with JetBrains decompiler
// Type: Database.Shirts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Database
{
  public class Shirts : ResourceSet<Shirt>
  {
    public Shirt Hot00;
    public Shirt Hot01;
    public Shirt Decor00;
    public Shirt Cold00;
    public Shirt Cold01;

    public Shirts()
    {
      this.Hot00 = this.Add(new Shirt("body_shirt_hot01"));
      this.Hot01 = this.Add(new Shirt("body_shirt_hot02"));
      this.Decor00 = this.Add(new Shirt("body_shirt_decor01"));
      this.Cold00 = this.Add(new Shirt("body_shirt_cold01"));
      this.Cold01 = this.Add(new Shirt("body_shirt_cold02"));
    }
  }
}
