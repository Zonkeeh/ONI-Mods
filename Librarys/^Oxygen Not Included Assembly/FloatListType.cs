// Decompiled with JetBrains decompiler
// Type: FloatListType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

public class FloatListType : IConnectionTypeDeclaration
{
  public string Identifier
  {
    get
    {
      return "FloatList";
    }
  }

  public System.Type Type
  {
    get
    {
      return typeof (FloatList);
    }
  }

  public Color Color
  {
    get
    {
      return Color.blue;
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
