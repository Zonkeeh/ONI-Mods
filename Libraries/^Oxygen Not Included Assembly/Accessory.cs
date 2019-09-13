// Decompiled with JetBrains decompiler
// Type: Accessory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Accessory : Resource
{
  public Accessory(
    string id,
    ResourceSet parent,
    AccessorySlot slot,
    HashedString batchSource,
    KAnim.Build.Symbol symbol)
    : base(id, parent, (string) null)
  {
    this.slot = slot;
    this.symbol = symbol;
    this.batchSource = batchSource;
  }

  public KAnim.Build.Symbol symbol { get; private set; }

  public HashedString batchSource { get; private set; }

  public AccessorySlot slot { get; private set; }
}
