// Decompiled with JetBrains decompiler
// Type: Blur
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class Blur
{
  private static Material blurMaterial;

  public static RenderTexture Run(Texture2D image)
  {
    if ((Object) Blur.blurMaterial == (Object) null)
      Blur.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
    return (RenderTexture) null;
  }
}
