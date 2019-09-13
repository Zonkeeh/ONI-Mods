// Decompiled with JetBrains decompiler
// Type: ElementsAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class ElementsAudio
{
  private static ElementsAudio _instance;
  private ElementsAudio.ElementAudioConfig[] elementAudioConfigs;

  public static ElementsAudio Instance
  {
    get
    {
      if (ElementsAudio._instance == null)
        ElementsAudio._instance = new ElementsAudio();
      return ElementsAudio._instance;
    }
  }

  public void LoadData(
    ElementsAudio.ElementAudioConfig[] elements_audio_configs)
  {
    this.elementAudioConfigs = elements_audio_configs;
  }

  public ElementsAudio.ElementAudioConfig GetConfigForElement(SimHashes id)
  {
    if (this.elementAudioConfigs != null)
    {
      for (int index = 0; index < this.elementAudioConfigs.Length; ++index)
      {
        if (this.elementAudioConfigs[index].elementID == id)
          return this.elementAudioConfigs[index];
      }
    }
    return (ElementsAudio.ElementAudioConfig) null;
  }

  public class ElementAudioConfig : Resource
  {
    public AmbienceType ambienceType = AmbienceType.None;
    public SolidAmbienceType solidAmbienceType = SolidAmbienceType.None;
    public string miningSound = string.Empty;
    public string miningBreakSound = string.Empty;
    public string oreBumpSound = string.Empty;
    public string floorEventAudioCategory = string.Empty;
    public string creatureChewSound = string.Empty;
    public SimHashes elementID;
  }
}
