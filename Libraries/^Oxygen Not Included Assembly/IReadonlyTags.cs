// Decompiled with JetBrains decompiler
// Type: IReadonlyTags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IReadonlyTags
{
  bool HasTag(string tag);

  bool HasTag(int hashtag);

  bool HasTags(int[] tags);
}
