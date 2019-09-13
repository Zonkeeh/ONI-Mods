// Decompiled with JetBrains decompiler
// Type: Database.BuildRoomType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.IO;

namespace Database
{
  public class BuildRoomType : ColonyAchievementRequirement
  {
    private RoomType roomType;

    public BuildRoomType(RoomType roomType)
    {
      this.roomType = roomType;
    }

    public override bool Success()
    {
      foreach (Room room in Game.Instance.roomProber.rooms)
      {
        if (room.roomType == this.roomType)
          return true;
      }
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.WriteKleiString(this.roomType.Id);
    }

    public override void Deserialize(IReader reader)
    {
      this.roomType = Db.Get().RoomTypes.Get(reader.ReadKleiString());
    }

    public override string GetProgress(bool complete)
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_A_ROOM, (object) this.roomType.Name);
    }
  }
}
