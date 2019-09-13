// Decompiled with JetBrains decompiler
// Type: UICurvePath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class UICurvePath : KMonoBehaviour
{
  public bool loop = true;
  public float initialAlpha = 0.5f;
  public float moveSpeed = 0.1f;
  public Transform startPoint;
  public Transform endPoint;
  public Transform controlPointStart;
  public Transform controlPointEnd;
  public Image sprite;
  public bool animateScale;
  public Vector3 initialScale;
  private float startDelay;
  private float tick;
  private Vector3 A;
  private Vector3 B;
  private Vector3 C;
  private Vector3 D;

  protected override void OnSpawn()
  {
    this.Init();
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.OnResize();
    this.startDelay = (float) UnityEngine.Random.Range(0, 8);
  }

  private void OnResize()
  {
    this.A = this.startPoint.position;
    this.B = this.controlPointStart.position;
    this.C = this.controlPointEnd.position;
    this.D = this.endPoint.position;
  }

  protected override void OnCleanUp()
  {
    ScreenResize.Instance.OnResize -= new System.Action(this.OnResize);
    base.OnCleanUp();
  }

  private void Update()
  {
    this.startDelay -= Time.unscaledDeltaTime;
    this.sprite.gameObject.SetActive((double) this.startDelay < 0.0);
    if ((double) this.startDelay > 0.0)
      return;
    this.tick += Time.unscaledDeltaTime * this.moveSpeed;
    this.sprite.transform.position = this.DeCasteljausAlgorithm(this.tick);
    this.sprite.SetAlpha(Mathf.Min(this.sprite.color.a + this.tick / 2f, 1f));
    if (this.animateScale)
    {
      float num = Mathf.Min(this.sprite.transform.localScale.x + Time.unscaledDeltaTime * this.moveSpeed, 1f);
      this.sprite.transform.localScale = new Vector3(num, num, 1f);
    }
    if (!this.loop || (double) this.tick <= 1.0)
      return;
    this.Init();
  }

  private void Init()
  {
    this.sprite.transform.position = this.startPoint.position;
    this.tick = 0.0f;
    if (this.animateScale)
      this.sprite.transform.localScale = this.initialScale;
    this.sprite.SetAlpha(this.initialAlpha);
  }

  private void OnDrawGizmos()
  {
    if (!Application.isPlaying)
    {
      this.A = this.startPoint.position;
      this.B = this.controlPointStart.position;
      this.C = this.controlPointEnd.position;
      this.D = this.endPoint.position;
    }
    Gizmos.color = Color.white;
    Vector3 vector3 = this.A;
    float num1 = 0.02f;
    int num2 = Mathf.FloorToInt(1f / num1);
    for (int index = 1; index <= num2; ++index)
      vector3 = this.DeCasteljausAlgorithm((float) index * num1);
    Gizmos.color = Color.green;
  }

  private Vector3 DeCasteljausAlgorithm(float t)
  {
    float num = 1f - t;
    Vector3 vector3_1 = num * this.A + t * this.B;
    Vector3 vector3_2 = num * this.B + t * this.C;
    Vector3 vector3_3 = num * this.C + t * this.D;
    Vector3 vector3_4 = num * vector3_1 + t * vector3_2;
    Vector3 vector3_5 = num * vector3_2 + t * vector3_3;
    return num * vector3_4 + t * vector3_5;
  }
}
