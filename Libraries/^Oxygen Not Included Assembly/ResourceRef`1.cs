// Decompiled with JetBrains decompiler
// Type: ResourceRef`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Runtime.Serialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
  [Serialize]
  private ResourceGuid guid;
  private ResourceType resource;

  public ResourceRef(ResourceType resource)
  {
    this.Set(resource);
  }

  public ResourceRef()
  {
  }

  public ResourceType Get()
  {
    return this.resource;
  }

  public void Set(ResourceType resource)
  {
    this.resource = resource;
  }

  [OnSerializing]
  private void OnSerializing()
  {
    if ((object) this.resource == null)
      this.guid = (ResourceGuid) null;
    else
      this.guid = this.resource.Guid;
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if (!(this.guid != (ResourceGuid) null))
      return;
    this.resource = Db.Get().GetResource<ResourceType>(this.guid);
    this.guid = (ResourceGuid) null;
  }
}
