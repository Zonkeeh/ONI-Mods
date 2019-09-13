// Decompiled with JetBrains decompiler
// Type: EventInstanceBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
  [Serialize]
  public int frame;
  [Serialize]
  public int eventHash;
  public EventBase ev;

  public EventInstanceBase(EventBase ev)
  {
    this.frame = GameClock.Instance.GetFrame();
    this.eventHash = ev.hash;
    this.ev = ev;
  }

  public override string ToString()
  {
    string str = "[" + this.frame.ToString() + "] ";
    if (this.ev != null)
      return str + this.ev.GetDescription(this);
    return str + "Unknown event";
  }
}
