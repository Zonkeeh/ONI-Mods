// Decompiled with JetBrains decompiler
// Type: ChoreGroupManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;

[SerializationConfig(MemberSerialization.OptIn)]
public class ChoreGroupManager : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  private List<Tag> defaultForbiddenTagsList = new List<Tag>();
  [Serialize]
  private Dictionary<Tag, int> defaultChorePermissions = new Dictionary<Tag, int>();
  public static ChoreGroupManager instance;

  public static void DestroyInstance()
  {
    ChoreGroupManager.instance = (ChoreGroupManager) null;
  }

  public List<Tag> DefaultForbiddenTagsList
  {
    get
    {
      return this.defaultForbiddenTagsList;
    }
  }

  public Dictionary<Tag, int> DefaultChorePermission
  {
    get
    {
      return this.defaultChorePermissions;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    ChoreGroupManager.instance = this;
    this.ConvertOldVersion();
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
    {
      if (!this.defaultChorePermissions.ContainsKey(resource.Id.ToTag()))
        this.defaultChorePermissions.Add(resource.Id.ToTag(), 2);
    }
  }

  private void ConvertOldVersion()
  {
    foreach (Tag defaultForbiddenTags in this.defaultForbiddenTagsList)
    {
      if (!this.defaultChorePermissions.ContainsKey(defaultForbiddenTags))
        this.defaultChorePermissions.Add(defaultForbiddenTags, -1);
      this.defaultChorePermissions[defaultForbiddenTags] = 0;
    }
    this.defaultForbiddenTagsList.Clear();
  }
}
