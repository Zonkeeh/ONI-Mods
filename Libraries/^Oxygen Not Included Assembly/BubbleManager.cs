// Decompiled with JetBrains decompiler
// Type: BubbleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : KMonoBehaviour, ISim33ms, IRenderEveryTick
{
  private List<BubbleManager.Bubble> bubbles = new List<BubbleManager.Bubble>();
  public static BubbleManager instance;

  public static void DestroyInstance()
  {
    BubbleManager.instance = (BubbleManager) null;
  }

  protected override void OnPrefabInit()
  {
    BubbleManager.instance = this;
  }

  public void SpawnBubble(
    Vector2 position,
    Vector2 velocity,
    SimHashes element,
    float mass,
    float temperature)
  {
    this.bubbles.Add(new BubbleManager.Bubble()
    {
      position = position,
      velocity = velocity,
      element = element,
      temperature = temperature,
      mass = mass
    });
  }

  public void Sim33ms(float dt)
  {
    ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList1 = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
    ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList2 = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
    foreach (BubbleManager.Bubble bubble in this.bubbles)
    {
      bubble.position += bubble.velocity * dt;
      bubble.elapsedTime += dt;
      int cell = Grid.PosToCell(bubble.position);
      if (!Grid.IsVisiblyInLiquid(bubble.position) || Grid.Element[cell].id == bubble.element)
        pooledList2.Add(bubble);
      else
        pooledList1.Add(bubble);
    }
    foreach (BubbleManager.Bubble bubble in (List<BubbleManager.Bubble>) pooledList2)
      SimMessages.AddRemoveSubstance(Grid.PosToCell(bubble.position), bubble.element, CellEventLogger.Instance.FallingWaterAddToSim, bubble.mass, bubble.temperature, byte.MaxValue, 0, true, -1);
    this.bubbles.Clear();
    this.bubbles.AddRange((IEnumerable<BubbleManager.Bubble>) pooledList1);
    pooledList2.Recycle();
    pooledList1.Recycle();
  }

  public void RenderEveryTick(float dt)
  {
    ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.PooledList pooledList = ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.Allocate();
    SpriteSheetAnimator spriteSheetAnimator = SpriteSheetAnimManager.instance.GetSpriteSheetAnimator((HashedString) "liquid_splash1");
    foreach (BubbleManager.Bubble bubble in this.bubbles)
    {
      SpriteSheetAnimator.AnimInfo animInfo = new SpriteSheetAnimator.AnimInfo()
      {
        frame = spriteSheetAnimator.GetFrameFromElapsedTimeLooping(bubble.elapsedTime),
        elapsedTime = bubble.elapsedTime,
        pos = new Vector3(bubble.position.x, bubble.position.y, 0.0f),
        rotation = Quaternion.identity,
        size = Vector2.one,
        colour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
      };
      pooledList.Add(animInfo);
    }
    pooledList.Recycle();
  }

  private struct Bubble
  {
    public Vector2 position;
    public Vector2 velocity;
    public float elapsedTime;
    public int frame;
    public SimHashes element;
    public float temperature;
    public float mass;
  }
}
