// Decompiled with JetBrains decompiler
// Type: BuildingElementEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingElementEmitter : KMonoBehaviour, IEffectDescriptor, IElementEmitter, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<BuildingElementEmitter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<BuildingElementEmitter>((System.Action<BuildingElementEmitter, object>) ((component, data) => component.OnActiveChanged(data)));
  [SerializeField]
  public float emitRate = 0.3f;
  [SerializeField]
  [Serialize]
  public float temperature = 293f;
  [SerializeField]
  [HashedEnum]
  public SimHashes element = SimHashes.Oxygen;
  [SerializeField]
  public byte emitRange = 1;
  [SerializeField]
  public byte emitDiseaseIdx = byte.MaxValue;
  private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
  private int simHandle = -1;
  private bool dirty = true;
  [SerializeField]
  public Vector2 modifierOffset;
  [SerializeField]
  public int emitDiseaseCount;
  private bool simActive;
  private Guid statusHandle;

  public float AverageEmitRate
  {
    get
    {
      return Game.Instance.accumulators.GetAverageRate(this.accumulator);
    }
  }

  public float EmitRate
  {
    get
    {
      return this.emitRate;
    }
  }

  public SimHashes Element
  {
    get
    {
      return this.element;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.accumulator = Game.Instance.accumulators.Add("Element", (KMonoBehaviour) this);
    this.Subscribe<BuildingElementEmitter>(824508782, BuildingElementEmitter.OnActiveChangedDelegate);
    this.SimRegister();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.accumulator);
    this.SimUnregister();
    base.OnCleanUp();
  }

  private void OnActiveChanged(object data)
  {
    this.simActive = ((Operational) data).IsActive;
    this.dirty = true;
  }

  public void Sim200ms(float dt)
  {
    this.UnsafeUpdate(dt);
  }

  private unsafe void UnsafeUpdate(float dt)
  {
    if (!Sim.IsValidHandle(this.simHandle))
      return;
    this.UpdateSimState();
    Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[Sim.GetHandleIndex(this.simHandle)];
    if ((double) emittedMassInfo.mass <= 0.0)
      return;
    Game.Instance.accumulators.Accumulate(this.accumulator, emittedMassInfo.mass);
    if (this.element != SimHashes.Oxygen)
      return;
    ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, emittedMassInfo.mass, this.gameObject.GetProperName(), (string) null);
  }

  private void UpdateSimState()
  {
    if (!this.dirty)
      return;
    this.dirty = false;
    if (this.simActive)
    {
      if (this.element != (SimHashes) 0 && (double) this.emitRate > 0.0)
        SimMessages.ModifyElementEmitter(this.simHandle, Grid.PosToCell(new Vector3(this.transform.GetPosition().x + this.modifierOffset.x, this.transform.GetPosition().y + this.modifierOffset.y, 0.0f)), (int) this.emitRange, this.element, 0.2f, this.emitRate * 0.2f, this.temperature, float.MaxValue, this.emitDiseaseIdx, this.emitDiseaseCount);
      this.statusHandle = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (object) this);
    }
    else
    {
      SimMessages.ModifyElementEmitter(this.simHandle, 0, 0, SimHashes.Vacuum, 0.0f, 0.0f, 0.0f, 0.0f, byte.MaxValue, 0);
      this.statusHandle = this.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, (bool) ((UnityEngine.Object) this));
    }
  }

  private void SimRegister()
  {
    if (!this.isSpawned || this.simHandle != -1)
      return;
    this.simHandle = -2;
    Game.ComplexCallbackHandleVector<int> componentCallbackManager = Game.Instance.simComponentCallbackManager;
    // ISSUE: reference to a compiler-generated field
    if (BuildingElementEmitter.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BuildingElementEmitter.\u003C\u003Ef__mg\u0024cache0 = new System.Action<int, object>(BuildingElementEmitter.OnSimRegisteredCallback);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<int, object> fMgCache0 = BuildingElementEmitter.\u003C\u003Ef__mg\u0024cache0;
    SimMessages.AddElementEmitter(float.MaxValue, componentCallbackManager.Add(fMgCache0, (object) this, nameof (BuildingElementEmitter)).index, -1, -1);
  }

  private void SimUnregister()
  {
    if (this.simHandle == -1)
      return;
    if (Sim.IsValidHandle(this.simHandle))
      SimMessages.RemoveElementEmitter(-1, this.simHandle);
    this.simHandle = -1;
  }

  private static void OnSimRegisteredCallback(int handle, object data)
  {
    ((BuildingElementEmitter) data).OnSimRegistered(handle);
  }

  private void OnSimRegistered(int handle)
  {
    if ((UnityEngine.Object) this != (UnityEngine.Object) null)
      this.simHandle = handle;
    else
      SimMessages.RemoveElementEmitter(-1, handle);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string str = ElementLoader.FindElementByHash(this.element).tag.ProperName();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_FIXEDTEMP, (object) str, (object) GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_FIXEDTEMP, (object) str, (object) GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor);
    return descriptorList;
  }
}
