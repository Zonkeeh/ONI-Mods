// Decompiled with JetBrains decompiler
// Type: BuildingGroupScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class BuildingGroupScreen : KScreen
{
  public static BuildingGroupScreen Instance;

  protected override void OnPrefabInit()
  {
    BuildingGroupScreen.Instance = this;
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.ConsumeMouseScroll = true;
  }
}
