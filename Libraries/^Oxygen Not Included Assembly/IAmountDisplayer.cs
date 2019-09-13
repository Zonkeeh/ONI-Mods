// Decompiled with JetBrains decompiler
// Type: IAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

public interface IAmountDisplayer
{
  string GetValueString(Amount master, AmountInstance instance);

  string GetDescription(Amount master, AmountInstance instance);

  string GetTooltip(Amount master, AmountInstance instance);

  IAttributeFormatter Formatter { get; }
}
