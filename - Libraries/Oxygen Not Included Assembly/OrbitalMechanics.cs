// Decompiled with JetBrains decompiler
// Type: OrbitalMechanics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class OrbitalMechanics : KMonoBehaviour, IRenderEveryTick
{
  [SerializeField]
  private OrbitalMechanics.OrbitData[] orbitData;
  [SerializeField]
  private bool applyOverrides;
  [SerializeField]
  [Range(0.0f, 100f)]
  private float overridePercent;
  [SerializeField]
  private GameObject[] orbitingObjects;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.orbitingObjects = (GameObject[]) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Rebuild();
  }

  public void RenderEveryTick(float dt)
  {
    if (this.orbitData == null || this.orbitingObjects == null)
      return;
    float time = GameClock.Instance.GetTime();
    for (int index = 0; index < this.orbitingObjects.Length; ++index)
    {
      OrbitalMechanics.OrbitData data = this.orbitData[index];
      bool behind;
      Vector3 pos = this.CalculatePos(ref data, time, out behind);
      pos.y -= 0.5f;
      Vector3 position = pos;
      position.x = Camera.main.ViewportToWorldPoint(pos).x;
      position.y = Camera.main.ViewportToWorldPoint(pos).y;
      bool flag = !data.rotatesBehind || !behind;
      GameObject orbitingObject = this.orbitingObjects[index];
      if ((UnityEngine.Object) orbitingObject != (UnityEngine.Object) null)
      {
        orbitingObject.transform.SetPosition(position);
        orbitingObject.transform.localScale = Vector3.one * Camera.main.orthographicSize / data.distance;
        if (orbitingObject.activeSelf != flag)
          orbitingObject.SetActive(flag);
      }
    }
  }

  [ContextMenu("Rebuild")]
  private void Rebuild()
  {
    if (this.orbitingObjects != null)
    {
      foreach (GameObject orbitingObject in this.orbitingObjects)
        Util.KDestroyGameObject(orbitingObject);
      this.orbitingObjects = (GameObject[]) null;
    }
    if (this.orbitData == null || this.orbitData.Length <= 0)
      return;
    float time = GameClock.Instance.GetTime();
    this.orbitingObjects = new GameObject[this.orbitData.Length];
    for (int index = 0; index < this.orbitData.Length; ++index)
    {
      OrbitalMechanics.OrbitData data = this.orbitData[index];
      bool behind;
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) data.prefabTag), this.CalculatePos(ref data, time, out behind));
      gameObject.SetActive(true);
      this.orbitingObjects[index] = gameObject;
    }
  }

  private Vector3 CalculatePos(
    ref OrbitalMechanics.OrbitData data,
    float time,
    out bool behind)
  {
    float num1 = data.periodInCycles * 600f;
    float f = (float) ((!this.applyOverrides ? (double) time / (double) num1 - (double) (int) ((double) time / (double) num1) : (double) this.overridePercent / 100.0) * 2.0 * 3.14159274101257);
    float num2 = 0.5f * data.radiusScale;
    Vector3 vector3_1 = new Vector3(0.5f, data.yGridPercent, 0.0f);
    Vector3 vector3_2 = new Vector3(Mathf.Cos(f), 0.0f, Mathf.Sin(f));
    behind = (double) vector3_2.z > (double) data.behindZ;
    Vector3 vector3_3 = Quaternion.Euler(data.angle, 0.0f, 0.0f) * (vector3_2 * num2);
    Vector3 vector3_4 = vector3_1 + vector3_3;
    vector3_4.z = data.renderZ;
    return vector3_4;
  }

  [Serializable]
  private struct OrbitData
  {
    public string prefabTag;
    public float periodInCycles;
    public float yGridPercent;
    public float angle;
    public float radiusScale;
    public bool rotatesBehind;
    public float behindZ;
    public Vector3 scale;
    public float distance;
    public float renderZ;
  }
}
