// Decompiled with JetBrains decompiler
// Type: IModule3DNodeType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using UnityEngine;

public class IModule3DNodeType : IConnectionTypeDeclaration
{
  public string Identifier
  {
    get
    {
      return "IModule3D";
    }
  }

  public System.Type Type
  {
    get
    {
      return typeof (IModule3D);
    }
  }

  public Color Color
  {
    get
    {
      return Color.magenta;
    }
  }

  public string InKnobTex
  {
    get
    {
      return "Textures/In_Knob.png";
    }
  }

  public string OutKnobTex
  {
    get
    {
      return "Textures/Out_Knob.png";
    }
  }
}
