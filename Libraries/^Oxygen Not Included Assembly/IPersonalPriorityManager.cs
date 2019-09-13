// Decompiled with JetBrains decompiler
// Type: IPersonalPriorityManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IPersonalPriorityManager
{
  int GetAssociatedSkillLevel(ChoreGroup group);

  int GetPersonalPriority(ChoreGroup group);

  void SetPersonalPriority(ChoreGroup group, int value);

  bool IsChoreGroupDisabled(ChoreGroup group);

  void ResetPersonalPriorities();
}
