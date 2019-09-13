// Decompiled with JetBrains decompiler
// Type: UIRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class UIRotator : KMonoBehaviour
{
  public float minRotationSpeed = 1f;
  public float maxRotationSpeed = 1f;
  public float rotationSpeed = 1f;

  protected override void OnPrefabInit()
  {
    this.rotationSpeed = Random.Range(this.minRotationSpeed, this.maxRotationSpeed);
  }

  private void Update()
  {
    this.GetComponent<RectTransform>().Rotate(0.0f, 0.0f, this.rotationSpeed * Time.unscaledDeltaTime);
  }
}
