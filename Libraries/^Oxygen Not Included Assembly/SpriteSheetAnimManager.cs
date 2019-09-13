// Decompiled with JetBrains decompiler
// Type: SpriteSheetAnimManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetAnimManager : KMonoBehaviour, IRenderEveryTick
{
  private Dictionary<int, SpriteSheetAnimator> nameIndexMap = new Dictionary<int, SpriteSheetAnimator>();
  public const float SECONDS_PER_FRAME = 0.03333334f;
  [SerializeField]
  private SpriteSheet[] sheets;
  public static SpriteSheetAnimManager instance;

  public static void DestroyInstance()
  {
    SpriteSheetAnimManager.instance = (SpriteSheetAnimManager) null;
  }

  protected override void OnPrefabInit()
  {
    SpriteSheetAnimManager.instance = this;
  }

  protected override void OnSpawn()
  {
    for (int index = 0; index < this.sheets.Length; ++index)
      this.nameIndexMap[Hash.SDBMLower(this.sheets[index].name)] = new SpriteSheetAnimator(this.sheets[index]);
  }

  public void Play(string name, Vector3 pos, Vector2 size, Color32 colour)
  {
    this.Play(Hash.SDBMLower(name), pos, Quaternion.identity, size, colour);
  }

  public void Play(string name, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
  {
    this.Play(Hash.SDBMLower(name), pos, rotation, size, colour);
  }

  public void Play(int name_hash, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
  {
    this.nameIndexMap[name_hash].Play(pos, rotation, size, (Color) colour);
  }

  public void RenderEveryTick(float dt)
  {
    this.UpdateAnims(dt);
    this.Render();
  }

  public void UpdateAnims(float dt)
  {
    foreach (KeyValuePair<int, SpriteSheetAnimator> nameIndex in this.nameIndexMap)
      nameIndex.Value.UpdateAnims(dt);
  }

  public void Render()
  {
    Vector3 zero = Vector3.zero;
    foreach (KeyValuePair<int, SpriteSheetAnimator> nameIndex in this.nameIndexMap)
      nameIndex.Value.Render();
  }

  public SpriteSheetAnimator GetSpriteSheetAnimator(HashedString name)
  {
    return this.nameIndexMap[name.HashValue];
  }
}
