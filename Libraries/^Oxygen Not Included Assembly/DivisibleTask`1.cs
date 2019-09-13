// Decompiled with JetBrains decompiler
// Type: DivisibleTask`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
  public string name;
  public int start;
  public int end;

  protected DivisibleTask(string name)
  {
    this.name = name;
  }

  public void Run(SharedData sharedData)
  {
    this.RunDivision(sharedData);
  }

  protected abstract void RunDivision(SharedData sharedData);
}
