// Decompiled with JetBrains decompiler
// Type: SandboxSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SandboxSettings
{
  private KPrefabID entity;
  private Element element;
  private Klei.AI.Disease disease;
  private int brushSize;
  private float noiseScale;
  private float noiseDensity;
  private float mass;
  private bool instantBuild;
  public float temperature;
  public float temperatureAdditive;
  public int diseaseCount;
  public System.Action OnChangeElement;
  public System.Action OnChangeDisease;
  public System.Action OnChangeEntity;
  public System.Action OnChangeBrushSize;
  public System.Action OnChangeNoiseScale;
  public System.Action OnChangeNoiseDensity;

  public KPrefabID Entity
  {
    get
    {
      return this.entity;
    }
    set
    {
      this.entity = value;
    }
  }

  public Element Element
  {
    get
    {
      return this.element;
    }
    set
    {
      this.SelectElement(value);
    }
  }

  public Klei.AI.Disease Disease
  {
    get
    {
      return this.disease;
    }
    set
    {
      this.SelectDisease(value);
    }
  }

  public int BrushSize
  {
    get
    {
      return this.brushSize;
    }
    set
    {
      this.SetBrushSize(value);
    }
  }

  public float NoiseScale
  {
    get
    {
      return this.noiseScale;
    }
    set
    {
      this.SetNoiseScale(value);
    }
  }

  public float NoiseDensity
  {
    get
    {
      return this.noiseDensity;
    }
    set
    {
      this.SetNoiseDensity(value);
    }
  }

  public float Mass
  {
    get
    {
      return this.mass;
    }
    set
    {
      this.mass = value;
    }
  }

  public bool InstantBuild
  {
    get
    {
      return this.instantBuild;
    }
    set
    {
      this.instantBuild = value;
    }
  }

  public void SelectEntity(KPrefabID entity)
  {
    this.entity = entity;
    this.OnChangeEntity();
  }

  public void SelectElement(Element element)
  {
    this.element = element;
    this.OnChangeElement();
  }

  public void SelectDisease(Klei.AI.Disease disease)
  {
    this.disease = disease;
    this.OnChangeDisease();
  }

  public void SetBrushSize(int size)
  {
    this.brushSize = size;
    this.OnChangeBrushSize();
  }

  public void SetNoiseScale(float amount)
  {
    this.noiseScale = amount;
    this.OnChangeNoiseScale();
  }

  public void SetNoiseDensity(float amount)
  {
    this.noiseDensity = amount;
    this.OnChangeNoiseDensity();
  }
}
