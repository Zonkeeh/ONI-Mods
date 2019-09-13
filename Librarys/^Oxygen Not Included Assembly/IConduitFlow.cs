// Decompiled with JetBrains decompiler
// Type: IConduitFlow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IConduitFlow
{
  void AddConduitUpdater(System.Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default);

  void RemoveConduitUpdater(System.Action<float> callback);

  bool IsConduitEmpty(int cell);
}
