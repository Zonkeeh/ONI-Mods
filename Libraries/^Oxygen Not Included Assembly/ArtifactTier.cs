// Decompiled with JetBrains decompiler
// Type: ArtifactTier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class ArtifactTier
{
  public EffectorValues decorValues;
  public StringKey name_key;

  public ArtifactTier(StringKey str_key, EffectorValues values)
  {
    this.decorValues = values;
    this.name_key = str_key;
  }
}
