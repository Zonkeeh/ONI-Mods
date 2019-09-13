// Decompiled with JetBrains decompiler
// Type: CharacterOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class CharacterOverlay : KMonoBehaviour
{
  private bool registered;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Register();
  }

  public void Register()
  {
    if (this.registered)
      return;
    this.registered = true;
    NameDisplayScreen.Instance.AddNewEntry(this.gameObject);
  }
}
