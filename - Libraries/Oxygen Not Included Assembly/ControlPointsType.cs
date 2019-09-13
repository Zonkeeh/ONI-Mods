// Decompiled with JetBrains decompiler
// Type: ControlPointsType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

public class ControlPointsType : IConnectionTypeDeclaration
{
  public string Identifier
  {
    get
    {
      return "ControlPoints";
    }
  }

  public System.Type Type
  {
    get
    {
      return typeof (ControlPointList);
    }
  }

  public Color Color
  {
    get
    {
      return Color.green;
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
