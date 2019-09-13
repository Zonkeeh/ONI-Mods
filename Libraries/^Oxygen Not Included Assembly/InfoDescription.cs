// Decompiled with JetBrains decompiler
// Type: InfoDescription
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class InfoDescription : KMonoBehaviour
{
  public string nameLocString = string.Empty;
  public string descriptionLocString = string.Empty;
  public string description;
  public string displayName;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (!string.IsNullOrEmpty(this.nameLocString))
      this.displayName = (string) Strings.Get(this.nameLocString);
    if (string.IsNullOrEmpty(this.descriptionLocString))
      return;
    this.description = (string) Strings.Get(this.descriptionLocString);
  }
}
