// Decompiled with JetBrains decompiler
// Type: SafetyChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SafetyChecker
{
  public SafetyChecker(SafetyChecker.Condition[] conditions)
  {
    this.conditions = conditions;
  }

  public SafetyChecker.Condition[] conditions { get; private set; }

  public int GetSafetyConditions(
    int cell,
    int cost,
    SafetyChecker.Context context,
    out bool all_conditions_met)
  {
    int num1 = 0;
    int num2 = 0;
    for (int index = 0; index < this.conditions.Length; ++index)
    {
      SafetyChecker.Condition condition = this.conditions[index];
      if (condition.callback(cell, cost, context))
      {
        num1 |= condition.mask;
        ++num2;
      }
    }
    all_conditions_met = num2 == this.conditions.Length;
    return num1;
  }

  public struct Condition
  {
    public unsafe Condition(
      string id,
      int condition_mask,
      SafetyChecker.Condition.Callback condition_callback)
    {
      *(SafetyChecker.Condition*) ref this = new SafetyChecker.Condition();
      this.callback = condition_callback;
      this.mask = condition_mask;
    }

    public SafetyChecker.Condition.Callback callback { get; private set; }

    public int mask { get; private set; }

    public delegate bool Callback(int cell, int cost, SafetyChecker.Context context);
  }

  public struct Context
  {
    public Navigator navigator;
    public OxygenBreather oxygenBreather;
    public SimTemperatureTransfer temperatureTransferer;
    public PrimaryElement primaryElement;
    public MinionBrain minionBrain;
    public int cell;

    public Context(KMonoBehaviour cmp)
    {
      this.cell = Grid.PosToCell(cmp);
      this.navigator = cmp.GetComponent<Navigator>();
      this.oxygenBreather = cmp.GetComponent<OxygenBreather>();
      this.minionBrain = cmp.GetComponent<MinionBrain>();
      this.temperatureTransferer = cmp.GetComponent<SimTemperatureTransfer>();
      this.primaryElement = cmp.GetComponent<PrimaryElement>();
    }
  }
}
