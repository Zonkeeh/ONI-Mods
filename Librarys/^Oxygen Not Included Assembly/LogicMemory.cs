// Decompiled with JetBrains decompiler
// Type: LogicMemory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMemory : KMonoBehaviour
{
  public static readonly HashedString READ_PORT_ID = new HashedString("LogicMemoryRead");
  public static readonly HashedString SET_PORT_ID = new HashedString("LogicMemorySet");
  public static readonly HashedString RESET_PORT_ID = new HashedString("LogicMemoryReset");
  private static readonly EventSystem.IntraObjectHandler<LogicMemory> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicMemory>((System.Action<LogicMemory, object>) ((component, data) => component.OnLogicValueChanged(data)));
  [MyCmpGet]
  private LogicPorts ports;
  [Serialize]
  private int value;
  private static StatusItem infoStatusItem;

  protected override void OnSpawn()
  {
    if (LogicMemory.infoStatusItem == null)
    {
      LogicMemory.infoStatusItem = new StatusItem("StoredValue", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      StatusItem infoStatusItem = LogicMemory.infoStatusItem;
      // ISSUE: reference to a compiler-generated field
      if (LogicMemory.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LogicMemory.\u003C\u003Ef__mg\u0024cache0 = new Func<string, object, string>(LogicMemory.ResolveInfoStatusItemString);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, object, string> fMgCache0 = LogicMemory.\u003C\u003Ef__mg\u0024cache0;
      infoStatusItem.resolveStringCallback = fMgCache0;
    }
    this.Subscribe<LogicMemory>(-801688580, LogicMemory.OnLogicValueChangedDelegate);
  }

  public void OnLogicValueChanged(object data)
  {
    if ((UnityEngine.Object) this.ports == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || (UnityEngine.Object) this == (UnityEngine.Object) null || !(((LogicValueChanged) data).portID != LogicMemory.READ_PORT_ID))
      return;
    int inputValue1 = this.ports.GetInputValue(LogicMemory.SET_PORT_ID);
    int inputValue2 = this.ports.GetInputValue(LogicMemory.RESET_PORT_ID);
    int num = this.value;
    if (inputValue2 == 1)
      num = 0;
    else if (inputValue1 == 1)
      num = 1;
    if (num == this.value)
      return;
    this.value = num;
    this.ports.SendSignal(LogicMemory.READ_PORT_ID, this.value);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Play((HashedString) (num == 0 ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private static string ResolveInfoStatusItemString(string format_str, object data)
  {
    int outputValue = ((LogicMemory) data).ports.GetOutputValue(LogicMemory.READ_PORT_ID);
    return string.Format((string) BUILDINGS.PREFABS.LOGICMEMORY.STATUS_ITEM_VALUE, (object) outputValue);
  }
}
