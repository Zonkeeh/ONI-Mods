// Decompiled with JetBrains decompiler
// Type: GameAudioSheets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GameAudioSheets : AudioSheets
{
  private HashSet<HashedString> validFileNames = new HashSet<HashedString>();
  private Dictionary<HashedString, HashSet<HashedString>> animsNotAllowedToPlaySpeech = new Dictionary<HashedString, HashSet<HashedString>>();
  private static GameAudioSheets _Instance;

  public static GameAudioSheets Get()
  {
    if ((Object) GameAudioSheets._Instance == (Object) null)
      GameAudioSheets._Instance = Resources.Load<GameAudioSheets>(nameof (GameAudioSheets));
    return GameAudioSheets._Instance;
  }

  public override void Initialize()
  {
    this.validFileNames.Add((HashedString) "game_triggered");
    foreach (KAnimFile animAsset in Assets.instance.AnimAssets)
    {
      if (!((Object) animAsset == (Object) null))
        this.validFileNames.Add((HashedString) animAsset.name);
    }
    base.Initialize();
    foreach (AudioSheet sheet in this.sheets)
    {
      foreach (AudioSheet.SoundInfo soundInfo in sheet.soundInfos)
      {
        if (soundInfo.Type == "MouthFlapSoundEvent" || soundInfo.Type == "VoiceSoundEvent")
        {
          HashSet<HashedString> hashedStringSet = (HashSet<HashedString>) null;
          if (!this.animsNotAllowedToPlaySpeech.TryGetValue((HashedString) soundInfo.File, out hashedStringSet))
          {
            hashedStringSet = new HashSet<HashedString>();
            this.animsNotAllowedToPlaySpeech[(HashedString) soundInfo.File] = hashedStringSet;
          }
          hashedStringSet.Add((HashedString) soundInfo.Anim);
        }
      }
    }
  }

  protected override AnimEvent CreateSoundOfType(
    string type,
    string file_name,
    string sound_name,
    int frame,
    float min_interval)
  {
    SoundEvent soundEvent = (SoundEvent) null;
    bool flag = true;
    if (sound_name.Contains(":disable_camera_position_scaling"))
    {
      sound_name = sound_name.Replace(":disable_camera_position_scaling", string.Empty);
      flag = false;
    }
    if (type == "FloorSoundEvent")
      soundEvent = (SoundEvent) new FloorSoundEvent(file_name, sound_name, frame);
    else if (type == "SoundEvent" || type == "LoopingSoundEvent")
    {
      bool is_looping = type == "LoopingSoundEvent";
      string[] strArray = sound_name.Split(':');
      sound_name = strArray[0];
      soundEvent = new SoundEvent(file_name, sound_name, frame, true, is_looping, min_interval, false);
      for (int index = 1; index < strArray.Length; ++index)
      {
        if (strArray[index] == "IGNORE_PAUSE")
          soundEvent.ignorePause = true;
        else
          Debug.LogWarning((object) (sound_name + " has unknown parameter " + strArray[index]));
      }
    }
    else if (type == "LadderSoundEvent")
      soundEvent = (SoundEvent) new LadderSoundEvent(file_name, sound_name, frame);
    else if (type == "LaserSoundEvent")
      soundEvent = (SoundEvent) new LaserSoundEvent(file_name, sound_name, frame, min_interval);
    else if (type == "HatchDrillSoundEvent")
      soundEvent = (SoundEvent) new HatchDrillSoundEvent(file_name, sound_name, frame, min_interval);
    else if (type == "CreatureChewSoundEvent")
      soundEvent = (SoundEvent) new CreatureChewSoundEvent(file_name, sound_name, frame, min_interval);
    else if (type == "BuildingDamageSoundEvent")
      soundEvent = (SoundEvent) new BuildingDamageSoundEvent(file_name, sound_name, frame);
    else if (type == "WallDamageSoundEvent")
      soundEvent = (SoundEvent) new WallDamageSoundEvent(file_name, sound_name, frame, min_interval);
    else if (type == "RemoteSoundEvent")
      soundEvent = (SoundEvent) new RemoteSoundEvent(file_name, sound_name, frame, min_interval);
    else if (type == "VoiceSoundEvent" || type == "LoopingVoiceSoundEvent")
      soundEvent = (SoundEvent) new VoiceSoundEvent(file_name, sound_name, frame, type == "LoopingVoiceSoundEvent");
    else if (type == "MouthFlapSoundEvent")
      soundEvent = (SoundEvent) new MouthFlapSoundEvent(file_name, sound_name, frame, false);
    else if (type == "MainMenuSoundEvent")
      soundEvent = (SoundEvent) new MainMenuSoundEvent(file_name, sound_name, frame);
    else if (type == "CreatureVariationSoundEvent")
      soundEvent = (SoundEvent) new CreatureVariationSoundEvent(file_name, sound_name, frame, true, type == "LoopingSoundEvent", min_interval, false);
    else if (type == "CountedSoundEvent")
      soundEvent = (SoundEvent) new CountedSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
    else if (type == "SculptingSoundEvent")
      soundEvent = (SoundEvent) new SculptingSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
    else if (type == "PhonoboxSoundEvent")
      soundEvent = (SoundEvent) new PhonoboxSoundEvent(file_name, sound_name, frame, min_interval);
    if (soundEvent != null)
      soundEvent.shouldCameraScalePosition = flag;
    return (AnimEvent) soundEvent;
  }

  public bool IsAnimAllowedToPlaySpeech(KAnim.Anim anim)
  {
    HashSet<HashedString> hashedStringSet = (HashSet<HashedString>) null;
    if (this.animsNotAllowedToPlaySpeech.TryGetValue((HashedString) anim.animFile.name, out hashedStringSet))
      return !hashedStringSet.Contains(anim.hash);
    return true;
  }

  private class SingleAudioSheetLoader : AsyncLoader
  {
    public AudioSheet sheet;
    public string text;
    public string name;

    public override void Run()
    {
      this.sheet.soundInfos = new ResourceLoader<AudioSheet.SoundInfo>(this.text, this.name).resources.ToArray();
    }
  }

  private class GameAudioSheetLoader : GlobalAsyncLoader<GameAudioSheets.GameAudioSheetLoader>
  {
    public override void CollectLoaders(List<AsyncLoader> loaders)
    {
      foreach (AudioSheet sheet in GameAudioSheets.Get().sheets)
        loaders.Add((AsyncLoader) new GameAudioSheets.SingleAudioSheetLoader()
        {
          sheet = sheet,
          text = sheet.asset.text,
          name = sheet.asset.name
        });
    }

    public override void Run()
    {
    }
  }
}
