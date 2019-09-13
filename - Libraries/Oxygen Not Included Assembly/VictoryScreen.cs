// Decompiled with JetBrains decompiler
// Type: VictoryScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class VictoryScreen : KModalScreen
{
  [SerializeField]
  private KButton DismissButton;
  [SerializeField]
  private LocText descriptionText;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Init();
  }

  private void Init()
  {
    if (!(bool) ((UnityEngine.Object) this.DismissButton))
      return;
    this.DismissButton.onClick += (System.Action) (() => this.Dismiss());
  }

  private void Retire()
  {
    if (!RetireColonyUtility.SaveColonySummaryData())
      return;
    this.Show(false);
  }

  private void Dismiss()
  {
    this.Show(false);
  }

  public void SetAchievements(string[] achievementIDs)
  {
    string str = string.Empty;
    for (int index = 0; index < achievementIDs.Length; ++index)
    {
      if (index > 0)
        str += "\n";
      str = str + GameUtil.ApplyBoldString(Db.Get().ColonyAchievements.Get(achievementIDs[index]).Name) + "\n" + Db.Get().ColonyAchievements.Get(achievementIDs[index]).description;
    }
    this.descriptionText.text = str;
  }
}
