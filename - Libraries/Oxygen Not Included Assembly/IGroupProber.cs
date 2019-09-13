// Decompiled with JetBrains decompiler
// Type: IGroupProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public interface IGroupProber
{
  void Occupy(object prober, int serial_no, IEnumerable<int> cells);

  void SetValidSerialNos(object prober, int previous_serial_no, int serial_no);

  bool ReleaseProber(object prober);
}
