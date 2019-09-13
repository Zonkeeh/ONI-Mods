// Decompiled with JetBrains decompiler
// Type: SizePulse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SizePulse : MonoBehaviour
{
  public Vector2 from = Vector2.one;
  public Vector2 to = Vector2.one;
  public float multiplier = 1.25f;
  public float speed = 1f;
  public System.Action onComplete;
  public bool updateWhenPaused;
  private Vector2 cur;
  private SizePulse.State state;

  private void Start()
  {
    if (this.GetComponents<SizePulse>().Length > 1)
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    this.from = (Vector2) this.transform.localScale;
    this.cur = this.from;
    this.to = this.from * this.multiplier;
  }

  private void Update()
  {
    float t = (!this.updateWhenPaused ? Time.deltaTime : Time.unscaledDeltaTime) * this.speed;
    switch (this.state)
    {
      case SizePulse.State.Up:
        this.cur = Vector2.Lerp(this.cur, this.to, t);
        if ((double) (this.to - this.cur).sqrMagnitude < 9.99999974737875E-05)
        {
          this.cur = this.to;
          this.state = SizePulse.State.Down;
          break;
        }
        break;
      case SizePulse.State.Down:
        this.cur = Vector2.Lerp(this.cur, this.from, t);
        if ((double) (this.from - this.cur).sqrMagnitude < 9.99999974737875E-05)
        {
          this.cur = this.from;
          this.state = SizePulse.State.Finished;
          if (this.onComplete != null)
          {
            this.onComplete();
            break;
          }
          break;
        }
        break;
    }
    this.transform.localScale = new Vector3(this.cur.x, this.cur.y, 1f);
  }

  private enum State
  {
    Up,
    Down,
    Finished,
  }
}
