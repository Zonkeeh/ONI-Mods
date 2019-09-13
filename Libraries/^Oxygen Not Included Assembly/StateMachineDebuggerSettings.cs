// Decompiled with JetBrains decompiler
// Type: StateMachineDebuggerSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineDebuggerSettings : ScriptableObject
{
  public List<StateMachineDebuggerSettings.Entry> entries = new List<StateMachineDebuggerSettings.Entry>();
  private static StateMachineDebuggerSettings _Instance;

  public IEnumerator<StateMachineDebuggerSettings.Entry> GetEnumerator()
  {
    return (IEnumerator<StateMachineDebuggerSettings.Entry>) this.entries.GetEnumerator();
  }

  public static StateMachineDebuggerSettings Get()
  {
    if ((UnityEngine.Object) StateMachineDebuggerSettings._Instance == (UnityEngine.Object) null)
    {
      StateMachineDebuggerSettings._Instance = Resources.Load<StateMachineDebuggerSettings>(nameof (StateMachineDebuggerSettings));
      StateMachineDebuggerSettings._Instance.Initialize();
    }
    return StateMachineDebuggerSettings._Instance;
  }

  private void Initialize()
  {
    foreach (System.Type currentDomainType in App.GetCurrentDomainTypes())
    {
      if (typeof (StateMachine).IsAssignableFrom(currentDomainType))
        this.CreateEntry(currentDomainType);
    }
    this.entries.RemoveAll((Predicate<StateMachineDebuggerSettings.Entry>) (x => x.type == null));
  }

  public StateMachineDebuggerSettings.Entry CreateEntry(System.Type type)
  {
    foreach (StateMachineDebuggerSettings.Entry entry in this.entries)
    {
      if (type.FullName == entry.typeName)
      {
        entry.type = type;
        return entry;
      }
    }
    StateMachineDebuggerSettings.Entry entry1 = new StateMachineDebuggerSettings.Entry(type);
    this.entries.Add(entry1);
    return entry1;
  }

  public void Clear()
  {
    this.entries.Clear();
    this.Initialize();
  }

  [Serializable]
  public class Entry
  {
    public System.Type type;
    public string typeName;
    public bool breakOnGoTo;
    public bool enableConsoleLogging;
    public bool saveHistory;

    public Entry(System.Type type)
    {
      this.typeName = type.FullName;
      this.type = type;
    }

    public static void ShowHeader()
    {
    }

    public void ShowEditor()
    {
    }
  }
}
