// Decompiled with JetBrains decompiler
// Type: EventBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class EventBase : Resource
{
  public int hash;

  public EventBase(string id)
    : base(id, id)
  {
    this.hash = Hash.SDBMLower(id);
  }

  public virtual string GetDescription(EventInstanceBase ev)
  {
    return string.Empty;
  }
}
