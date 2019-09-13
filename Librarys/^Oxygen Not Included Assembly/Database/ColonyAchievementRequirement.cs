// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievementRequirement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace Database
{
  public abstract class ColonyAchievementRequirement
  {
    public virtual void Update()
    {
    }

    public abstract bool Success();

    public virtual bool Fail()
    {
      return false;
    }

    public abstract void Serialize(BinaryWriter writer);

    public abstract void Deserialize(IReader reader);

    public virtual string GetProgress(bool complete)
    {
      return string.Empty;
    }
  }
}
