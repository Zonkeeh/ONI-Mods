// Decompiled with JetBrains decompiler
// Type: Database.BuildNRoomTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.IO;

namespace Database
{
  public class BuildNRoomTypes : ColonyAchievementRequirement
  {
    private RoomType roomType;
    private int numToCreate;

    public BuildNRoomTypes(RoomType roomType, int numToCreate = 1)
    {
      this.roomType = roomType;
      this.numToCreate = numToCreate;
    }

    public override bool Success()
    {
      int num = 0;
      foreach (Room room in Game.Instance.roomProber.rooms)
      {
        if (room.roomType == this.roomType)
          ++num;
      }
      return num >= this.numToCreate;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.WriteKleiString(this.roomType.Id);
      writer.Write(this.numToCreate);
    }

    public override void Deserialize(IReader reader)
    {
      this.roomType = Db.Get().RoomTypes.Get(reader.ReadKleiString());
      this.numToCreate = reader.ReadInt32();
    }

    public override string GetProgress(bool complete)
    {
      int num = 0;
      foreach (Room room in Game.Instance.roomProber.rooms)
      {
        if (room.roomType == this.roomType)
          ++num;
      }
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_N_ROOMS, (object) this.roomType.Name, (object) (!complete ? num : this.numToCreate), (object) this.numToCreate);
    }
  }
}
