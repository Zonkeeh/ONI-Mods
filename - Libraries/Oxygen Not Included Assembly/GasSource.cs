// Decompiled with JetBrains decompiler
// Type: GasSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
  protected override CellOffset[] GetOffsetGroup()
  {
    return OffsetGroups.LiquidSource;
  }

  protected override IChunkManager GetChunkManager()
  {
    return (IChunkManager) GasSourceManager.Instance;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }
}
