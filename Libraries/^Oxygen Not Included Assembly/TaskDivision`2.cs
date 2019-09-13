// Decompiled with JetBrains decompiler
// Type: TaskDivision`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal class TaskDivision<Task, SharedData> where Task : DivisibleTask<SharedData>, new()
{
  public Task[] tasks;

  public TaskDivision(int taskCount)
  {
    this.tasks = new Task[taskCount];
    for (int index = 0; index != this.tasks.Length; ++index)
      this.tasks[index] = new Task();
  }

  public TaskDivision()
    : this(CPUBudget.coreCount)
  {
  }

  public void Initialize(int count)
  {
    int num = count / this.tasks.Length;
    for (int index = 0; index != this.tasks.Length; ++index)
    {
      this.tasks[index].start = index * num;
      this.tasks[index].end = this.tasks[index].start + num;
    }
    DebugUtil.Assert(this.tasks[this.tasks.Length - 1].end + count % this.tasks.Length == count);
    this.tasks[this.tasks.Length - 1].end = count;
  }

  public void Run(SharedData sharedData)
  {
    foreach (Task task in this.tasks)
      task.Run(sharedData);
  }
}
