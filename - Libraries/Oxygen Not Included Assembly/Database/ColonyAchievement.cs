// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Database
{
  public class ColonyAchievement : Resource
  {
    public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();
    public string description;
    public bool isVictoryCondition;
    public string messageTitle;
    public string messageBody;
    public string shortVideoName;
    public string loopVideoName;
    public string steamAchievementId;
    public string icon;
    public System.Action<KMonoBehaviour> victorySequence;

    public ColonyAchievement(
      string Id,
      string steamAchievementId,
      string Name,
      string description,
      bool isVictoryCondition,
      List<ColonyAchievementRequirement> requirementChecklist,
      string messageTitle = "",
      string messageBody = "",
      string videoDataName = "",
      string victoryLoopVideo = "",
      System.Action<KMonoBehaviour> VictorySequence = null,
      string victorySnapshot = "",
      string icon = "")
      : base(Id, Name)
    {
      this.Id = Id;
      this.steamAchievementId = steamAchievementId;
      this.Name = Name;
      this.description = description;
      this.isVictoryCondition = isVictoryCondition;
      this.requirementChecklist = requirementChecklist;
      this.messageTitle = messageTitle;
      this.messageBody = messageBody;
      this.shortVideoName = videoDataName;
      this.loopVideoName = victoryLoopVideo;
      this.victorySequence = VictorySequence;
      this.victoryNISSnapshot = !string.IsNullOrEmpty(victorySnapshot) ? victorySnapshot : AudioMixerSnapshots.Get().VictoryNISGenericSnapshot;
      this.icon = icon;
    }

    public string victoryNISSnapshot { get; private set; }
  }
}
