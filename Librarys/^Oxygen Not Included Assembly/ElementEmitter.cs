// Decompiled with JetBrains decompiler
// Type: ElementEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ElementEmitter : SimComponent
{
  [SerializeField]
  public float emissionFrequency = 1f;
  [SerializeField]
  public byte emitRange = 1;
  [SerializeField]
  public float maxPressure = 1f;
  private Guid statusHandle = Guid.Empty;
  public bool showDescriptor = true;
  private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
  private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
  [SerializeField]
  public ElementConverter.OutputElement outputElement;

  public bool isEmitterBlocked { get; private set; }

  protected override void OnSpawn()
  {
    this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterBlocked), true));
    this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterUnblocked), true));
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
    Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
    base.OnCleanUp();
  }

  public void SetEmitting(bool emitting)
  {
    this.SetSimActive(emitting);
  }

  protected override void OnSimActivate()
  {
    int game_cell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), (int) this.outputElement.outputElementOffset.x, (int) this.outputElement.outputElementOffset.y);
    if (this.outputElement.elementHash != (SimHashes) 0 && (double) this.outputElement.massGenerationRate > 0.0 && (double) this.emissionFrequency > 0.0)
    {
      float emit_temperature = (double) this.outputElement.minOutputTemperature != 0.0 ? this.outputElement.minOutputTemperature : this.GetComponent<PrimaryElement>().Temperature;
      SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int) this.emitRange, this.outputElement.elementHash, this.emissionFrequency, this.outputElement.massGenerationRate, emit_temperature, this.maxPressure, this.outputElement.addedDiseaseIdx, this.outputElement.addedDiseaseCount);
    }
    if (!this.showDescriptor)
      return;
    this.statusHandle = this.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ElementEmitterOutput, (object) this);
  }

  protected override void OnSimDeactivate()
  {
    SimMessages.ModifyElementEmitter(this.simHandle, Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), (int) this.outputElement.outputElementOffset.x, (int) this.outputElement.outputElementOffset.y), (int) this.emitRange, SimHashes.Vacuum, 0.0f, 0.0f, 0.0f, 0.0f, byte.MaxValue, 0);
    if (!this.showDescriptor)
      return;
    this.statusHandle = this.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
  }

  public void ForceEmit(float mass, byte disease_idx, int disease_count, float temperature = -1f)
  {
    if ((double) mass <= 0.0)
      return;
    float temperature1 = (double) temperature <= 0.0 ? this.outputElement.minOutputTemperature : temperature;
    Element elementByHash = ElementLoader.FindElementByHash(this.outputElement.elementHash);
    if (elementByHash.IsGas || elementByHash.IsLiquid)
      SimMessages.AddRemoveSubstance(Grid.PosToCell(this.transform.GetPosition()), this.outputElement.elementHash, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature1, disease_idx, disease_count, true, -1);
    else if (elementByHash.IsSolid)
      elementByHash.substance.SpawnResource(this.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f), mass, temperature1, disease_idx, disease_count, false, true, false);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(this.outputElement.elementHash).name, this.gameObject.transform, 1.5f, false);
  }

  private void OnEmitterBlocked()
  {
    this.isEmitterBlocked = true;
    this.Trigger(1615168894, (object) this);
  }

  private void OnEmitterUnblocked()
  {
    this.isEmitterBlocked = false;
    this.Trigger(-657992955, (object) this);
  }

  protected override void OnSimRegister(
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
  {
    Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
    SimMessages.AddElementEmitter(this.maxPressure, cb_handle.index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
  }

  protected override void OnSimUnregister()
  {
    ElementEmitter.StaticUnregister(this.simHandle);
  }

  private static void StaticUnregister(int sim_handle)
  {
    Debug.Assert(Sim.IsValidHandle(sim_handle));
    SimMessages.RemoveElementEmitter(-1, sim_handle);
  }

  private void OnDrawGizmosSelected()
  {
    int cell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), (int) this.outputElement.outputElementOffset.x, (int) this.outputElement.outputElementOffset.y);
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(Grid.CellToPos(cell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
  }

  protected override System.Action<int> GetStaticUnregister()
  {
    // ISSUE: reference to a compiler-generated field
    if (ElementEmitter.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ElementEmitter.\u003C\u003Ef__mg\u0024cache0 = new System.Action<int>(ElementEmitter.StaticUnregister);
    }
    // ISSUE: reference to a compiler-generated field
    return ElementEmitter.\u003C\u003Ef__mg\u0024cache0;
  }
}
