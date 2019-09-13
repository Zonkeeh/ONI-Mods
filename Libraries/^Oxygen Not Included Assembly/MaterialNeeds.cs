// Decompiled with JetBrains decompiler
// Type: MaterialNeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class MaterialNeeds : KMonoBehaviour
{
  private Dictionary<Tag, float> Needs = new Dictionary<Tag, float>();
  public System.Action OnDirty;

  public static MaterialNeeds Instance { get; private set; }

  public static void DestroyInstance()
  {
    MaterialNeeds.Instance = (MaterialNeeds) null;
  }

  protected override void OnPrefabInit()
  {
    MaterialNeeds.Instance = this;
  }

  public void UpdateNeed(Tag tag, float amount)
  {
    float num = 0.0f;
    if (!this.Needs.TryGetValue(tag, out num))
      this.Needs[tag] = 0.0f;
    this.Needs[tag] = num + amount;
  }

  public float GetAmount(Tag tag)
  {
    float num = 0.0f;
    this.Needs.TryGetValue(tag, out num);
    return num;
  }

  public Dictionary<Tag, float> GetNeeds()
  {
    return this.Needs;
  }
}
