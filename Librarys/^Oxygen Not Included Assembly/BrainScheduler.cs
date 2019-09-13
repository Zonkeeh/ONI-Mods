// Decompiled with JetBrains decompiler
// Type: BrainScheduler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class BrainScheduler : KMonoBehaviour, IRenderEveryTick, ICPULoad
{
  private List<BrainScheduler.BrainGroup> brainGroups = new List<BrainScheduler.BrainGroup>();
  public const float millisecondsPerFrame = 33.33333f;
  public const float secondsPerFrame = 0.03333333f;
  public const float framesPerSecond = 30.00001f;

  private bool isAsyncPathProbeEnabled
  {
    get
    {
      return !TuningData<BrainScheduler.Tuning>.Get().disableAsyncPathProbes;
    }
  }

  protected override void OnPrefabInit()
  {
    this.brainGroups.Add((BrainScheduler.BrainGroup) new BrainScheduler.DupeBrainGroup());
    this.brainGroups.Add((BrainScheduler.BrainGroup) new BrainScheduler.CreatureBrainGroup());
    Components.Brains.Register(new System.Action<Brain>(this.OnAddBrain), new System.Action<Brain>(this.OnRemoveBrain));
    CPUBudget.AddRoot((ICPULoad) this);
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
      CPUBudget.AddChild((ICPULoad) this, (ICPULoad) brainGroup, brainGroup.LoadBalanceThreshold());
    CPUBudget.FinalizeChildren((ICPULoad) this);
  }

  private void OnAddBrain(Brain brain)
  {
    bool test = false;
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
    {
      if (brain.HasTag(brainGroup.tag))
      {
        brainGroup.AddBrain(brain);
        test = true;
      }
      Navigator component = brain.GetComponent<Navigator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.executePathProbeTaskAsync = this.isAsyncPathProbeEnabled;
    }
    DebugUtil.Assert(test);
  }

  private void OnRemoveBrain(Brain brain)
  {
    bool test = false;
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
    {
      if (brain.HasTag(brainGroup.tag))
      {
        test = true;
        brainGroup.RemoveBrain(brain);
      }
      Navigator component = brain.GetComponent<Navigator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.executePathProbeTaskAsync = false;
    }
    DebugUtil.Assert(test);
  }

  public float GetEstimatedFrameTime()
  {
    return TuningData<BrainScheduler.Tuning>.Get().frameTime;
  }

  public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
  {
    return false;
  }

  public void RenderEveryTick(float dt)
  {
    if (Game.IsQuitting() || KMonoBehaviour.isLoadingScene)
      return;
    foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
      brainGroup.RenderEveryTick(dt, this.isAsyncPathProbeEnabled);
  }

  private class Tuning : TuningData<BrainScheduler.Tuning>
  {
    public float frameTime = 5f;
    public bool disableAsyncPathProbes;
  }

  private abstract class BrainGroup : ICPULoad
  {
    private List<Brain> brains = new List<Brain>();
    private WorkItemCollection<Navigator.PathProbeTask, object> pathProbeJob = new WorkItemCollection<Navigator.PathProbeTask, object>();
    private string increaseLoadLabel;
    private string decreaseLoadLabel;
    private int nextUpdateBrain;
    private int nextPathProbeBrain;

    protected BrainGroup(Tag tag)
    {
      this.tag = tag;
      this.probeSize = this.InitialProbeSize();
      this.probeCount = this.InitialProbeCount();
      string str = tag.ToString();
      this.increaseLoadLabel = "IncLoad" + str;
      this.decreaseLoadLabel = "DecLoad" + str;
    }

    public Tag tag { get; private set; }

    public void AddBrain(Brain brain)
    {
      this.brains.Add(brain);
    }

    public void RemoveBrain(Brain brain)
    {
      int num = this.brains.IndexOf(brain);
      if (num == -1)
        return;
      this.brains.RemoveAt(num);
      this.OnRemoveBrain(num, ref this.nextUpdateBrain);
      this.OnRemoveBrain(num, ref this.nextPathProbeBrain);
    }

    public int probeSize { get; private set; }

    public int probeCount { get; private set; }

    public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
    {
      bool flag = (double) frameTimeDelta > 0.0;
      int num1 = 0;
      int num2 = Math.Max(this.probeCount, Math.Min(this.brains.Count, CPUBudget.coreCount));
      int num3 = num1 + (num2 - this.probeCount);
      this.probeCount = num2;
      float num4 = Math.Min(1f, (float) this.probeCount / (float) CPUBudget.coreCount);
      float num5 = num4 * (float) this.probeSize;
      float num6 = num4 * (float) this.probeSize;
      float num7 = currentFrameTime / num6;
      float num8 = frameTimeDelta / num7;
      if (num3 == 0)
      {
        int num9 = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int) ((double) (num5 + num8 / (float) CPUBudget.coreCount) / (double) num4));
        num3 += num9 - this.probeSize;
        this.probeSize = num9;
      }
      if (num3 == 0)
      {
        int num9 = Math.Max(1, (int) num4 + (!flag ? -1 : 1));
        int num10 = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int) (((double) num6 + (double) num8) / (double) num9));
        int num11 = Math.Min(this.brains.Count, num9 * CPUBudget.coreCount);
        num3 += num11 - this.probeCount;
        this.probeCount = num11;
        this.probeSize = num10;
      }
      if (num3 == 0 && flag)
      {
        int num9 = this.probeSize + this.ProbeSizeStep();
        num3 += num9 - this.probeSize;
        this.probeSize = num9;
      }
      if (num3 < 0)
        KProfiler.AddEvent(this.decreaseLoadLabel);
      else if (num3 > 0)
        KProfiler.AddEvent(this.increaseLoadLabel);
      else
        Debug.LogWarning((object) "AdjustLoad() failed");
      return num3 != 0;
    }

    private void IncrementBrainIndex(ref int brainIndex)
    {
      ++brainIndex;
      if (brainIndex != this.brains.Count)
        return;
      brainIndex = 0;
    }

    private void ClampBrainIndex(ref int brainIndex)
    {
      brainIndex = MathUtil.Clamp(0, this.brains.Count - 1, brainIndex);
    }

    private void OnRemoveBrain(int removedIndex, ref int brainIndex)
    {
      if (removedIndex < brainIndex)
      {
        --brainIndex;
      }
      else
      {
        if (brainIndex != this.brains.Count)
          return;
        brainIndex = 0;
      }
    }

    private void AsyncPathProbe()
    {
      int probeSize = this.probeSize;
      this.pathProbeJob.Reset((object) null);
      for (int index = 0; index != this.brains.Count; ++index)
      {
        this.ClampBrainIndex(ref this.nextPathProbeBrain);
        Navigator component = this.brains[this.nextPathProbeBrain].GetComponent<Navigator>();
        this.IncrementBrainIndex(ref this.nextPathProbeBrain);
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.executePathProbeTaskAsync = true;
          component.PathProber.potentialCellsPerUpdate = this.probeSize;
          component.pathProbeTask.Update();
          this.pathProbeJob.Add(component.pathProbeTask);
          if (this.pathProbeJob.Count == this.probeCount)
            break;
        }
      }
      CPUBudget.Start((ICPULoad) this);
      GlobalJobManager.Run((IWorkItemCollection) this.pathProbeJob);
      CPUBudget.End((ICPULoad) this);
    }

    public void RenderEveryTick(float dt, bool isAsyncPathProbeEnabled)
    {
      if (isAsyncPathProbeEnabled)
        this.AsyncPathProbe();
      int num = this.InitialProbeCount();
      for (int index = 0; index != this.brains.Count && num != 0; ++index)
      {
        this.ClampBrainIndex(ref this.nextPathProbeBrain);
        Brain brain = this.brains[this.nextUpdateBrain];
        this.IncrementBrainIndex(ref this.nextUpdateBrain);
        if (brain.IsRunning())
        {
          brain.UpdateBrain();
          --num;
        }
      }
    }

    public void AccumulatePathProbeIterations(Dictionary<string, int> pathProbeIterations)
    {
      foreach (Brain brain in this.brains)
      {
        Navigator component = brain.GetComponent<Navigator>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && !pathProbeIterations.ContainsKey(brain.name))
          pathProbeIterations.Add(brain.name, component.PathProber.updateCount);
      }
    }

    protected abstract int InitialProbeCount();

    protected abstract int InitialProbeSize();

    protected abstract int MinProbeSize();

    protected abstract int IdealProbeSize();

    protected abstract int ProbeSizeStep();

    public abstract float GetEstimatedFrameTime();

    public abstract float LoadBalanceThreshold();
  }

  private class DupeBrainGroup : BrainScheduler.BrainGroup
  {
    public DupeBrainGroup()
      : base(GameTags.DupeBrain)
    {
    }

    protected override int InitialProbeCount()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeCount;
    }

    protected override int InitialProbeSize()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeSize;
    }

    protected override int MinProbeSize()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().minProbeSize;
    }

    protected override int IdealProbeSize()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().idealProbeSize;
    }

    protected override int ProbeSizeStep()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().probeSizeStep;
    }

    public override float GetEstimatedFrameTime()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().estimatedFrameTime;
    }

    public override float LoadBalanceThreshold()
    {
      return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().loadBalanceThreshold;
    }

    public class Tuning : TuningData<BrainScheduler.DupeBrainGroup.Tuning>
    {
      public int initialProbeCount = 1;
      public int initialProbeSize = 1000;
      public int minProbeSize = 100;
      public int idealProbeSize = 1000;
      public int probeSizeStep = 100;
      public float estimatedFrameTime = 2f;
      public float loadBalanceThreshold = 0.1f;
    }
  }

  private class CreatureBrainGroup : BrainScheduler.BrainGroup
  {
    public CreatureBrainGroup()
      : base(GameTags.CreatureBrain)
    {
    }

    protected override int InitialProbeCount()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeCount;
    }

    protected override int InitialProbeSize()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeSize;
    }

    protected override int MinProbeSize()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().minProbeSize;
    }

    protected override int IdealProbeSize()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().idealProbeSize;
    }

    protected override int ProbeSizeStep()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().probeSizeStep;
    }

    public override float GetEstimatedFrameTime()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().estimatedFrameTime;
    }

    public override float LoadBalanceThreshold()
    {
      return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().loadBalanceThreshold;
    }

    public class Tuning : TuningData<BrainScheduler.CreatureBrainGroup.Tuning>
    {
      public int initialProbeCount = 1;
      public int initialProbeSize = 1000;
      public int minProbeSize = 100;
      public int idealProbeSize = 300;
      public int probeSizeStep = 100;
      public float estimatedFrameTime = 1f;
      public float loadBalanceThreshold = 0.1f;
    }
  }
}
