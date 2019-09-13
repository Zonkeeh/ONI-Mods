// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeConverters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.AI
{
  public class AttributeConverters : KMonoBehaviour
  {
    public List<AttributeConverterInstance> converters = new List<AttributeConverterInstance>();

    public int Count
    {
      get
      {
        return this.converters.Count;
      }
    }

    protected override void OnPrefabInit()
    {
      foreach (AttributeInstance attribute in this.GetAttributes())
      {
        foreach (AttributeConverter converter in attribute.Attribute.converters)
          this.converters.Add(new AttributeConverterInstance(this.gameObject, converter, attribute));
      }
    }

    public AttributeConverterInstance Get(AttributeConverter converter)
    {
      foreach (AttributeConverterInstance converter1 in this.converters)
      {
        if (converter1.converter == converter)
          return converter1;
      }
      return (AttributeConverterInstance) null;
    }

    public AttributeConverterInstance GetConverter(string id)
    {
      foreach (AttributeConverterInstance converter in this.converters)
      {
        if (converter.converter.Id == id)
          return converter;
      }
      return (AttributeConverterInstance) null;
    }
  }
}
