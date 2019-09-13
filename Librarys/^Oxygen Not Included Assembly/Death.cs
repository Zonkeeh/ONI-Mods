// Decompiled with JetBrains decompiler
// Type: Death
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Death : Resource
{
  public string preAnim;
  public string loopAnim;
  public string sound;
  public string description;

  public Death(
    string id,
    ResourceSet parent,
    string name,
    string description,
    string pre_anim,
    string loop_anim)
    : base(id, parent, name)
  {
    this.preAnim = pre_anim;
    this.loopAnim = loop_anim;
    this.description = description;
  }
}
