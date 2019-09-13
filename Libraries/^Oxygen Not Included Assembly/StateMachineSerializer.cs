// Decompiled with JetBrains decompiler
// Type: StateMachineSerializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;

public class StateMachineSerializer
{
  private static int serializerVersion = 10;
  private List<StateMachineSerializer.Entry> entries = new List<StateMachineSerializer.Entry>();
  private FastReader entryData;

  public void Serialize(List<StateMachine.Instance> state_machines, BinaryWriter writer)
  {
    MemoryStream stream = new MemoryStream();
    BinaryWriter entry_writer = new BinaryWriter((Stream) stream);
    List<StateMachineSerializer.Entry> entries = this.CreateEntries(state_machines, entry_writer);
    long data_size_pos = this.WriteHeader(writer);
    long position = writer.BaseStream.Position;
    this.WriteEntries(entries, writer);
    try
    {
      this.WriteEntryData(stream, writer);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("Stream size: " + (object) stream.Length));
      Debug.Log((object) "StateMachines: ");
      foreach (object stateMachine in state_machines)
        Debug.Log((object) stateMachine.ToString());
      Debug.LogError((object) ex);
    }
    this.WriteDataSize(position, data_size_pos, writer);
  }

  public void Deserialize(IReader reader)
  {
    if (!this.ReadHeader(reader))
      return;
    this.entries = this.ReadEntries(reader);
    this.entryData = this.ReadEntryData(reader);
  }

  private List<StateMachineSerializer.Entry> CreateEntries(
    List<StateMachine.Instance> state_machines,
    BinaryWriter entry_writer)
  {
    List<StateMachineSerializer.Entry> entryList = new List<StateMachineSerializer.Entry>();
    foreach (StateMachine.Instance stateMachine in state_machines)
    {
      if (stateMachine.IsRunning())
      {
        StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry(stateMachine, entry_writer);
        entryList.Add(entry);
      }
    }
    return entryList;
  }

  private void WriteEntryData(MemoryStream stream, BinaryWriter writer)
  {
    writer.Write((int) stream.Length);
    writer.Write(stream.ToArray());
  }

  private FastReader ReadEntryData(IReader reader)
  {
    int length = reader.ReadInt32();
    return new FastReader(reader.ReadBytes(length));
  }

  private void WriteDataSize(long data_start_pos, long data_size_pos, BinaryWriter writer)
  {
    long position = writer.BaseStream.Position;
    long num = position - data_start_pos;
    writer.BaseStream.Position = data_size_pos;
    writer.Write((int) num);
    writer.BaseStream.Position = position;
  }

  private long WriteHeader(BinaryWriter writer)
  {
    int num = 0;
    writer.Write(StateMachineSerializer.serializerVersion);
    long position = writer.BaseStream.Position;
    writer.Write(num);
    return position;
  }

  private bool ReadHeader(IReader reader)
  {
    int num = reader.ReadInt32();
    int length = reader.ReadInt32();
    if (num == StateMachineSerializer.serializerVersion)
      return true;
    Debug.LogWarning((object) ("State machine serializer version mismatch: " + (object) num + "!=" + (object) StateMachineSerializer.serializerVersion + "\nDiscarding data."));
    reader.SkipBytes(length);
    return false;
  }

  private void WriteEntries(
    List<StateMachineSerializer.Entry> serialized_entries,
    BinaryWriter writer)
  {
    writer.Write(serialized_entries.Count);
    for (int index = 0; index < serialized_entries.Count; ++index)
      serialized_entries[index].Serialize(writer);
  }

  private List<StateMachineSerializer.Entry> ReadEntries(IReader reader)
  {
    List<StateMachineSerializer.Entry> entryList = new List<StateMachineSerializer.Entry>();
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      StateMachineSerializer.Entry entry = StateMachineSerializer.Entry.Deserialize(reader);
      if (entry != null)
        entryList.Add(entry);
    }
    return entryList;
  }

  private static string TrimAssemblyInfo(string type_name)
  {
    int length = type_name.IndexOf("[[");
    if (length != -1)
      return type_name.Substring(0, length);
    return type_name;
  }

  private bool Restore(StateMachineSerializer.Entry entry, StateMachine.Instance smi)
  {
    if (entry.version != smi.GetStateMachine().version)
      return false;
    this.entryData.Position = entry.dataPos;
    if (Manager.HasDeserializationMapping(smi.GetType()))
      Deserializer.DeserializeTypeless((object) smi, (IReader) this.entryData);
    if (!smi.GetStateMachine().serializable)
      return false;
    StateMachine.BaseState state = smi.GetStateMachine().GetState(entry.currentState);
    if (state == null)
      return false;
    StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
    int num1 = this.entryData.ReadInt32();
    for (int index = 0; index < num1; ++index)
    {
      int num2 = this.entryData.ReadInt32();
      int position = this.entryData.Position;
      string str1 = this.entryData.ReadKleiString().Replace("Version=4.0.0.0", "Version=2.0.0.0");
      string str2 = this.entryData.ReadKleiString();
      foreach (StateMachine.Parameter.Context context in parameterContexts)
      {
        if (context.parameter.name == str2 && context.GetType().FullName == str1)
        {
          context.Deserialize((IReader) this.entryData);
          break;
        }
      }
      this.entryData.SkipBytes(num2 - (this.entryData.Position - position));
    }
    smi.GoTo(state);
    return true;
  }

  public bool Restore(StateMachine.Instance instance)
  {
    if (this.entryData == null)
      return false;
    System.Type type = instance.GetType();
    for (int index = 0; index < this.entries.Count; ++index)
    {
      StateMachineSerializer.Entry entry = this.entries[index];
      if (entry.type == type)
      {
        this.entries.RemoveAt(index);
        return this.Restore(entry, instance);
      }
    }
    return false;
  }

  private class Entry
  {
    public int version;
    public int dataPos;
    public System.Type type;
    public string currentState;

    public Entry(int version, int data_pos, System.Type type, string current_state)
    {
      this.version = version;
      this.dataPos = data_pos;
      this.type = type;
      this.currentState = current_state;
    }

    public Entry(StateMachine.Instance smi, BinaryWriter entry_writer)
    {
      this.version = smi.GetStateMachine().version;
      this.dataPos = (int) entry_writer.BaseStream.Position;
      this.type = smi.GetType();
      this.currentState = smi.GetCurrentState().name;
      Serializer.SerializeTypeless((object) smi, entry_writer);
      StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
      entry_writer.Write(parameterContexts.Length);
      foreach (StateMachine.Parameter.Context context in parameterContexts)
      {
        long position1 = (long) (int) entry_writer.BaseStream.Position;
        entry_writer.Write(0);
        long position2 = (long) (int) entry_writer.BaseStream.Position;
        entry_writer.WriteKleiString(context.GetType().FullName);
        entry_writer.WriteKleiString(context.parameter.name);
        context.Serialize(entry_writer);
        long position3 = (long) (int) entry_writer.BaseStream.Position;
        entry_writer.BaseStream.Position = position1;
        long num = position3 - position2;
        entry_writer.Write((int) num);
        entry_writer.BaseStream.Position = position3;
      }
    }

    public void Serialize(BinaryWriter writer)
    {
      writer.Write(this.version);
      writer.Write(this.dataPos);
      writer.WriteKleiString(this.type.FullName);
      writer.WriteKleiString(this.currentState);
    }

    public static StateMachineSerializer.Entry Deserialize(IReader reader)
    {
      int version = reader.ReadInt32();
      int data_pos = reader.ReadInt32();
      string typeName = reader.ReadKleiString();
      string current_state = reader.ReadKleiString();
      System.Type type = System.Type.GetType(typeName);
      if (type == null)
        return (StateMachineSerializer.Entry) null;
      return new StateMachineSerializer.Entry(version, data_pos, type, current_state);
    }
  }
}
