// Decompiled with JetBrains decompiler
// Type: Vignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class Vignette : MonoBehaviour
{
  [SerializeField]
  private Image image;
  private Color defaultColor;
  public static Vignette Instance;

  public static void DestroyInstance()
  {
    Vignette.Instance = (Vignette) null;
  }

  private void Awake()
  {
    Vignette.Instance = this;
    this.defaultColor = this.image.color;
  }

  public void SetColor(Color color)
  {
    this.image.color = color;
  }

  public void Reset()
  {
    this.SetColor(this.defaultColor);
  }
}
