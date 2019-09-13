// Decompiled with JetBrains decompiler
// Type: StateMachineController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.IO;

[SerializationConfig(MemberSerialization.OptIn)]
public class StateMachineController : KMonoBehaviour, ISaveLoadableDetails, IStateMachineControllerHack
{
  private static readonly EventSystem.IntraObjectHandler<StateMachineController> OnTargetDestroyedDelegate = new EventSystem.IntraObjectHandler<StateMachineController>((System.Action<StateMachineController, object>) ((component, data) => component.OnTargetDestroyed(data)));
  private List<StateMachine.Instance> stateMachines = new List<StateMachine.Instance>();
  private LoggerFSSSS log = new LoggerFSSSS(nameof (StateMachineController), 35);
  private StateMachineSerializer serializer = new StateMachineSerializer();
  public DefHandle defHandle;

  public StateMachineController.CmpDef cmpdef
  {
    get
    {
      return this.defHandle.Get<StateMachineController.CmpDef>();
    }
  }

  public IEnumerator<StateMachine.Instance> GetEnumerator()
  {
    return (IEnumerator<StateMachine.Instance>) this.stateMachines.GetEnumerator();
  }

  public void AddStateMachineInstance(StateMachine.Instance state_machine)
  {
    if (this.stateMachines.Contains(state_machine))
      return;
    this.stateMachines.Add(state_machine);
  }

  public void RemoveStateMachineInstance(StateMachine.Instance state_machine)
  {
    if (state_machine.GetStateMachine().saveHistory || state_machine.GetStateMachine().debugSettings.saveHistory)
      return;
    this.stateMachines.Remove(state_machine);
  }

  public bool HasStateMachineInstance(StateMachine.Instance state_machine)
  {
    return this.stateMachines.Contains(state_machine);
  }

  public void AddDef(StateMachine.BaseDef def)
  {
    this.cmpdef.defs.Add(def);
  }

  public LoggerFSSSS GetLog()
  {
    return this.log;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.log.SetName(this.name);
    this.Subscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
    this.Subscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
  }

  private void OnTargetDestroyed(object data)
  {
    while (this.stateMachines.Count > 0)
    {
      StateMachine.Instance stateMachine = this.stateMachines[0];
      stateMachine.StopSM("StateMachineController.OnCleanUp");
      this.stateMachines.Remove(stateMachine);
    }
  }

  protected override void OnLoadLevel()
  {
    while (this.stateMachines.Count > 0)
    {
      StateMachine.Instance stateMachine = this.stateMachines[0];
      stateMachine.FreeResources();
      this.stateMachines.Remove(stateMachine);
    }
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    while (this.stateMachines.Count > 0)
    {
      StateMachine.Instance stateMachine = this.stateMachines[0];
      stateMachine.StopSM("StateMachineController.OnCleanUp");
      this.stateMachines.Remove(stateMachine);
    }
  }

  public void CreateSMIS()
  {
    if (!this.defHandle.IsValid())
      return;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
      def.CreateSMI((IStateMachineTarget) this);
  }

  public void StartSMIS()
  {
    if (!this.defHandle.IsValid())
      return;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      StateMachine.Instance smi = this.GetSMI(Singleton<StateMachineManager>.Instance.CreateStateMachine(def.GetStateMachineType()).GetStateMachineInstanceType());
      if (smi != null && !smi.IsRunning())
        smi.StartSM();
    }
  }

  public void Serialize(BinaryWriter writer)
  {
    this.serializer.Serialize(this.stateMachines, writer);
  }

  public void Deserialize(IReader reader)
  {
    this.serializer.Deserialize(reader);
  }

  public bool Restore(StateMachine.Instance smi)
  {
    return this.serializer.Restore(smi);
  }

  public DefType GetDef<DefType>() where DefType : StateMachine.BaseDef
  {
    if (!this.defHandle.IsValid())
      return (DefType) null;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      DefType defType = def as DefType;
      if ((object) defType != null)
        return defType;
    }
    return (DefType) null;
  }

  public List<DefType> GetDefs<DefType>() where DefType : StateMachine.BaseDef
  {
    List<DefType> defTypeList = new List<DefType>();
    if (!this.defHandle.IsValid())
      return defTypeList;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      DefType defType = def as DefType;
      if ((object) defType != null)
        defTypeList.Add(defType);
    }
    return defTypeList;
  }

  public StateMachine.Instance GetSMI(System.Type type)
  {
    for (int index = 0; index < this.stateMachines.Count; ++index)
    {
      StateMachine.Instance stateMachine = this.stateMachines[index];
      if (type.IsAssignableFrom(stateMachine.GetType()))
        return stateMachine;
    }
    return (StateMachine.Instance) null;
  }

  public StateMachineInstanceType GetSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
  {
    return (object) this.GetSMI(typeof (StateMachineInstanceType)) as StateMachineInstanceType;
  }

  public List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
  {
    List<StateMachineInstanceType> machineInstanceTypeList = new List<StateMachineInstanceType>();
    foreach (StateMachine.Instance stateMachine in this.stateMachines)
    {
      StateMachineInstanceType machineInstanceType = (object) stateMachine as StateMachineInstanceType;
      if ((object) machineInstanceType != null)
        machineInstanceTypeList.Add(machineInstanceType);
    }
    return machineInstanceTypeList;
  }

  public List<IGameObjectEffectDescriptor> GetDescriptors()
  {
    List<IGameObjectEffectDescriptor> effectDescriptorList = new List<IGameObjectEffectDescriptor>();
    if (!this.defHandle.IsValid())
      return effectDescriptorList;
    foreach (StateMachine.BaseDef def in this.cmpdef.defs)
    {
      if (def is IGameObjectEffectDescriptor)
        effectDescriptorList.Add(def as IGameObjectEffectDescriptor);
    }
    return effectDescriptorList;
  }

  public class CmpDef
  {
    public List<StateMachine.BaseDef> defs = new List<StateMachine.BaseDef>();
  }
}
