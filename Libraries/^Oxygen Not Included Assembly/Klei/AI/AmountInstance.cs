// Decompiled with JetBrains decompiler
// Type: Klei.AI.AmountInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
  [SerializationConfig(MemberSerialization.OptIn)]
  [DebuggerDisplay("{amount.Name} {value} ({deltaAttribute.value}/{minAttribute.value}/{maxAttribute.value})")]
  public class AmountInstance : ModifierInstance<Amount>, ISaveLoadable, ISim200ms
  {
    private static WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext> batch_update_job = new WorkItemCollection<AmountInstance.BatchUpdateTask, AmountInstance.BatchUpdateContext>();
    [Serialize]
    public float value;
    public AttributeInstance minAttribute;
    public AttributeInstance maxAttribute;
    public AttributeInstance deltaAttribute;
    public System.Action<float> OnDelta;
    public System.Action OnMaxValueReached;
    public bool hide;
    private bool _paused;

    public AmountInstance(Amount amount, GameObject game_object)
      : base(game_object, amount)
    {
      Attributes attributes = game_object.GetAttributes();
      this.minAttribute = attributes.Add(amount.minAttribute);
      this.maxAttribute = attributes.Add(amount.maxAttribute);
      this.deltaAttribute = attributes.Add(amount.deltaAttribute);
    }

    public Amount amount
    {
      get
      {
        return this.modifier;
      }
    }

    public bool paused
    {
      get
      {
        return this._paused;
      }
      set
      {
        this._paused = this.paused;
        if (this._paused)
          this.Deactivate();
        else
          this.Activate();
      }
    }

    public float GetMin()
    {
      return this.minAttribute.GetTotalValue();
    }

    public float GetMax()
    {
      return this.maxAttribute.GetTotalValue();
    }

    public float GetDelta()
    {
      return this.deltaAttribute.GetTotalValue();
    }

    public float SetValue(float value)
    {
      this.value = Mathf.Min(Mathf.Max(value, this.GetMin()), this.GetMax());
      return this.value;
    }

    public void Publish(float delta, float previous_value)
    {
      if (this.OnDelta != null)
        this.OnDelta(delta);
      if (this.OnMaxValueReached == null || (double) previous_value >= (double) this.GetMax() || (double) this.value < (double) this.GetMax())
        return;
      this.OnMaxValueReached();
    }

    public float ApplyDelta(float delta)
    {
      float previous_value = this.value;
      double num = (double) this.SetValue(this.value + delta);
      this.Publish(delta, previous_value);
      return this.value;
    }

    public string GetValueString()
    {
      return this.amount.GetValueString(this);
    }

    public string GetDescription()
    {
      return this.amount.GetDescription(this);
    }

    public string GetTooltip()
    {
      return this.amount.GetTooltip(this);
    }

    public void Activate()
    {
      SimAndRenderScheduler.instance.Add((object) this, false);
    }

    public void Sim200ms(float dt)
    {
    }

    public static void BatchUpdate(
      List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances,
      float time_delta)
    {
      if ((double) time_delta == 0.0)
        return;
      AmountInstance.BatchUpdateContext shared_data = new AmountInstance.BatchUpdateContext(amount_instances, time_delta);
      AmountInstance.batch_update_job.Reset(shared_data);
      int num = 512;
      for (int start = 0; start < amount_instances.Count; start += num)
      {
        int end = start + num;
        if (amount_instances.Count < end)
          end = amount_instances.Count;
        AmountInstance.batch_update_job.Add(new AmountInstance.BatchUpdateTask(start, end));
      }
      GlobalJobManager.Run((IWorkItemCollection) AmountInstance.batch_update_job);
      shared_data.Finish();
    }

    public void Deactivate()
    {
      SimAndRenderScheduler.instance.Remove((object) this);
    }

    private struct BatchUpdateContext
    {
      public List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances;
      public float time_delta;
      public ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.PooledList results;

      public BatchUpdateContext(
        List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances,
        float time_delta)
      {
        for (int index = 0; index != amount_instances.Count; ++index)
        {
          UpdateBucketWithUpdater<ISim200ms>.Entry amountInstance = amount_instances[index];
          amountInstance.lastUpdateTime = 0.0f;
          amount_instances[index] = amountInstance;
        }
        this.amount_instances = amount_instances;
        this.time_delta = time_delta;
        this.results = ListPool<AmountInstance.BatchUpdateContext.Result, AmountInstance>.Allocate();
        this.results.Capacity = this.amount_instances.Count;
      }

      public void Finish()
      {
        foreach (AmountInstance.BatchUpdateContext.Result result in (List<AmountInstance.BatchUpdateContext.Result>) this.results)
          result.amount_instance.Publish(result.delta, result.previous);
        this.results.Recycle();
      }

      public struct Result
      {
        public AmountInstance amount_instance;
        public float previous;
        public float delta;
      }
    }

    private struct BatchUpdateTask : IWorkItem<AmountInstance.BatchUpdateContext>
    {
      private int start;
      private int end;

      public BatchUpdateTask(int start, int end)
      {
        this.start = start;
        this.end = end;
      }

      public void Run(AmountInstance.BatchUpdateContext context)
      {
        for (int start = this.start; start != this.end; ++start)
        {
          AmountInstance data = (AmountInstance) context.amount_instances[start].data;
          float num1 = data.GetDelta() * context.time_delta;
          if ((double) num1 != 0.0)
          {
            context.results.Add(new AmountInstance.BatchUpdateContext.Result()
            {
              amount_instance = data,
              previous = data.value,
              delta = num1
            });
            double num2 = (double) data.SetValue(data.value + num1);
          }
        }
      }
    }
  }
}
