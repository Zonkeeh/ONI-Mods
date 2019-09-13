// Decompiled with JetBrains decompiler
// Type: ChoreTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class ChoreTable
{
  private ChoreTable.Entry[] entries;

  public ChoreTable(ChoreTable.Entry[] entries)
  {
    this.entries = entries;
  }

  public int GetChorePriority<StateMachineType>(ChoreConsumer chore_consumer)
  {
    foreach (ChoreTable.Entry entry in this.entries)
    {
      if (entry.stateMachineDef.GetStateMachineType() == typeof (StateMachineType))
        return entry.choreType.priority;
    }
    Debug.LogError((object) (chore_consumer.name + "'s chore table does not have an entry for: " + typeof (StateMachineType).Name));
    return -1;
  }

  public class Builder
  {
    private List<ChoreTable.Builder.Info> infos = new List<ChoreTable.Builder.Info>();
    private int interruptGroupId;

    public ChoreTable.Builder PushInterruptGroup()
    {
      ++this.interruptGroupId;
      return this;
    }

    public ChoreTable.Builder PopInterruptGroup()
    {
      DebugUtil.Assert(this.interruptGroupId > 0);
      --this.interruptGroupId;
      return this;
    }

    public ChoreTable.Builder Add(StateMachine.BaseDef def, bool condition = true)
    {
      if (condition)
        this.infos.Add(new ChoreTable.Builder.Info()
        {
          interruptGroupId = this.interruptGroupId,
          def = def
        });
      return this;
    }

    public ChoreTable CreateTable()
    {
      DebugUtil.Assert(this.interruptGroupId == 0);
      ChoreTable.Entry[] entries = new ChoreTable.Entry[this.infos.Count];
      Stack<int> intStack = new Stack<int>();
      for (int index = 0; index < this.infos.Count; ++index)
      {
        int priority = 10000 - index * 100;
        int interrupt_priority = 10000 - index * 100;
        int interruptGroupId = this.infos[index].interruptGroupId;
        if (interruptGroupId != 0)
        {
          if (intStack.Count != interruptGroupId)
            intStack.Push(interrupt_priority);
          else
            interrupt_priority = intStack.Peek();
        }
        else if (intStack.Count > 0)
          intStack.Pop();
        entries[index] = new ChoreTable.Entry(this.infos[index].def, priority, interrupt_priority);
      }
      return new ChoreTable(entries);
    }

    private struct Info
    {
      public int interruptGroupId;
      public StateMachine.BaseDef def;
    }
  }

  public class ChoreTableChore<StateMachineType, StateMachineInstanceType> : Chore<StateMachineInstanceType>
    where StateMachineInstanceType : StateMachine.Instance
  {
    public ChoreTableChore(
      StateMachine.BaseDef state_machine_def,
      ChoreType chore_type,
      KPrefabID prefab_id)
      : base(chore_type, (IStateMachineTarget) prefab_id, prefab_id.GetComponent<ChoreProvider>(), true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
    {
      this.showAvailabilityInHoverText = false;
      this.smi = state_machine_def.CreateSMI((IStateMachineTarget) this) as StateMachineInstanceType;
    }
  }

  public struct Entry
  {
    public System.Type choreClassType;
    public ChoreType choreType;
    public StateMachine.BaseDef stateMachineDef;

    public Entry(StateMachine.BaseDef state_machine_def, int priority, int interrupt_priority)
    {
      System.Type machineInstanceType = Singleton<StateMachineManager>.Instance.CreateStateMachine(state_machine_def.GetStateMachineType()).GetStateMachineInstanceType();
      this.choreClassType = typeof (ChoreTable.ChoreTableChore<,>).MakeGenericType(state_machine_def.GetStateMachineType(), machineInstanceType);
      this.choreType = new ChoreType(state_machine_def.ToString(), (ResourceSet) null, new string[0], string.Empty, string.Empty, string.Empty, string.Empty, (IEnumerable<Tag>) new Tag[0], priority, priority);
      this.choreType.interruptPriority = interrupt_priority;
      this.stateMachineDef = state_machine_def;
    }
  }

  public class Instance
  {
    private static object[] parameters = new object[3];
    private ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.PooledList entries;

    public Instance(ChoreTable chore_table, KPrefabID prefab_id)
    {
      this.entries = ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.Allocate();
      for (int index = 0; index < chore_table.entries.Length; ++index)
        this.entries.Add(new ChoreTable.Instance.Entry(chore_table.entries[index], prefab_id));
    }

    public static void ResetParameters()
    {
      for (int index = 0; index < ChoreTable.Instance.parameters.Length; ++index)
        ChoreTable.Instance.parameters[index] = (object) null;
    }

    public void OnCleanUp(KPrefabID prefab_id)
    {
      for (int index = 0; index < this.entries.Count; ++index)
        this.entries[index].OnCleanUp(prefab_id);
      this.entries.Recycle();
      this.entries = (ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.PooledList) null;
    }

    private struct Entry
    {
      public Chore chore;

      public Entry(ChoreTable.Entry chore_table_entry, KPrefabID prefab_id)
      {
        ChoreTable.Instance.parameters[0] = (object) chore_table_entry.stateMachineDef;
        ChoreTable.Instance.parameters[1] = (object) chore_table_entry.choreType;
        ChoreTable.Instance.parameters[2] = (object) prefab_id;
        this.chore = (Chore) Activator.CreateInstance(chore_table_entry.choreClassType, ChoreTable.Instance.parameters);
      }

      public void OnCleanUp(KPrefabID prefab_id)
      {
        if (this.chore == null)
          return;
        this.chore.Cancel("ChoreTable.Instance.OnCleanUp");
        this.chore = (Chore) null;
      }
    }
  }
}
