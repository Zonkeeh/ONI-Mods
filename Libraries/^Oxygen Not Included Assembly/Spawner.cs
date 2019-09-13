// Decompiled with JetBrains decompiler
// Type: Spawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class Spawner : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  public int units = 1;
  [Serialize]
  public Tag prefabTag;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    SaveGame.Instance.worldGenSpawner.AddLegacySpawner(this.prefabTag, Grid.PosToCell((KMonoBehaviour) this));
    Util.KDestroyGameObject(this.gameObject);
  }
}
