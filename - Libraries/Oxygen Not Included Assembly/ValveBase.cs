// Decompiled with JetBrains decompiler
// Type: ValveBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ValveBase : KMonoBehaviour, ISaveLoadable
{
  [SerializeField]
  public float maxFlow = 0.5f;
  protected HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;
  private int curFlowIdx = -1;
  [SerializeField]
  public ConduitType conduitType;
  [Serialize]
  private float currentFlow;
  [MyCmpGet]
  protected KBatchedAnimController controller;
  private int inputCell;
  private int outputCell;
  [SerializeField]
  public ValveBase.AnimRangeInfo[] animFlowRanges;

  public float CurrentFlow
  {
    set
    {
      this.currentFlow = value;
    }
    get
    {
      return this.currentFlow;
    }
  }

  public HandleVector<int>.Handle AccumulatorHandle
  {
    get
    {
      return this.flowAccumulator;
    }
  }

  public float MaxFlow
  {
    get
    {
      return this.maxFlow;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.flowAccumulator = Game.Instance.accumulators.Add("Flow", (KMonoBehaviour) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.inputCell = component.GetUtilityInputCell();
    this.outputCell = component.GetUtilityOutputCell();
    Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    this.UpdateAnim();
    this.OnCmpEnable();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.flowAccumulator);
    Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    base.OnCleanUp();
  }

  private void ConduitUpdate(float dt)
  {
    ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
    ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);
    if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
    {
      this.UpdateAnim();
    }
    else
    {
      ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
      float mass = Mathf.Min(contents.mass, this.currentFlow * dt);
      if ((double) mass > 0.0)
      {
        int disease_count = (int) ((double) (mass / contents.mass) * (double) contents.diseaseCount);
        float num = flowManager.AddElement(this.outputCell, contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count);
        Game.Instance.accumulators.Accumulate(this.flowAccumulator, num);
        if ((double) num > 0.0)
          flowManager.RemoveElement(this.inputCell, num);
      }
      this.UpdateAnim();
    }
  }

  public virtual void UpdateAnim()
  {
    float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
    if ((double) averageRate > 0.0)
    {
      for (int index = 0; index < this.animFlowRanges.Length; ++index)
      {
        if ((double) averageRate <= (double) this.animFlowRanges[index].minFlow)
        {
          if (this.curFlowIdx == index)
            break;
          this.curFlowIdx = index;
          this.controller.Play((HashedString) this.animFlowRanges[index].animName, (double) averageRate > 0.0 ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once, 1f, 0.0f);
          break;
        }
      }
    }
    else
      this.controller.Play((HashedString) "off", KAnim.PlayMode.Once, 1f, 0.0f);
  }

  [Serializable]
  public struct AnimRangeInfo
  {
    public float minFlow;
    public string animName;

    public AnimRangeInfo(float min_flow, string anim_name)
    {
      this.minFlow = min_flow;
      this.animName = anim_name;
    }
  }
}
