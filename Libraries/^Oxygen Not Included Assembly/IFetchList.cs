// Decompiled with JetBrains decompiler
// Type: IFetchList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public interface IFetchList
{
  Storage Destination { get; }

  float GetMinimumAmount(Tag tag);

  Dictionary<Tag, float> GetRemaining();

  Dictionary<Tag, float> GetRemainingMinimum();
}
