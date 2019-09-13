// Decompiled with JetBrains decompiler
// Type: RetireColonyUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public static class RetireColonyUtility
{
  private static char[] invalidCharacters = "<>:\"\\/|?*.".ToCharArray();
  private static Encoding[] attempt_encodings = new Encoding[3]
  {
    (Encoding) new UTF8Encoding(false, true),
    (Encoding) new UnicodeEncoding(false, true, true),
    Encoding.ASCII
  };
  private const int FILE_IO_RETRY_ATTEMPTS = 5;

  public static bool SaveColonySummaryData()
  {
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    string str1 = System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
    if (!Directory.Exists(str1))
      Directory.CreateDirectory(str1);
    string path2 = RetireColonyUtility.StripInvalidCharacters(SaveGame.Instance.BaseName);
    string str2 = System.IO.Path.Combine(str1, path2);
    if (!Directory.Exists(str2))
      Directory.CreateDirectory(str2);
    string path = System.IO.Path.Combine(str2, path2 + ".json");
    string s = JsonConvert.SerializeObject((object) RetireColonyUtility.GetCurrentColonyRetiredColonyData());
    bool flag = false;
    int num = 0;
    while (!flag)
    {
      if (num < 5)
      {
        try
        {
          Thread.Sleep(num * 100);
          using (FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
          {
            flag = true;
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            fileStream.Write(bytes, 0, bytes.Length);
          }
        }
        catch (Exception ex)
        {
          Debug.LogWarningFormat("SaveColonySummaryData failed attempt {0}: {1}", (object) (num + 1), (object) ex.ToString());
        }
        ++num;
      }
      else
        break;
    }
    return flag;
  }

  public static RetiredColonyData GetCurrentColonyRetiredColonyData()
  {
    MinionAssignablesProxy[] minions = new MinionAssignablesProxy[Components.MinionAssignablesProxy.Count];
    for (int index = 0; index < minions.Length; ++index)
      minions[index] = Components.MinionAssignablesProxy[index];
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, ColonyAchievementStatus> achievement in SaveGame.Instance.GetComponent<ColonyAchievementTracker>().achievements)
    {
      if (achievement.Value.success)
        stringList.Add(achievement.Key);
    }
    BuildingComplete[] buildingCompletes = new BuildingComplete[Components.BuildingCompletes.Count];
    for (int index = 0; index < buildingCompletes.Length; ++index)
      buildingCompletes[index] = Components.BuildingCompletes[index];
    return new RetiredColonyData(SaveGame.Instance.BaseName, GameClock.Instance.GetCycle(), System.DateTime.Now.ToShortDateString(), stringList.ToArray(), minions, buildingCompletes);
  }

  private static RetiredColonyData LoadRetiredColony(
    string file,
    bool skipStats,
    Encoding enc)
  {
    RetiredColonyData retiredColonyData = new RetiredColonyData();
    using (FileStream fileStream = File.Open(file, FileMode.Open))
    {
      using (StreamReader streamReader = new StreamReader((Stream) fileStream, enc))
      {
        using (JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) streamReader))
        {
          string empty = string.Empty;
          List<string> stringList = new List<string>();
          List<Tuple<string, int>> tupleList1 = new List<Tuple<string, int>>();
          List<RetiredColonyData.RetiredDuplicantData> retiredDuplicantDataList = new List<RetiredColonyData.RetiredDuplicantData>();
          List<RetiredColonyData.RetiredColonyStatistic> retiredColonyStatisticList = new List<RetiredColonyData.RetiredColonyStatistic>();
          while (jsonReader.Read())
          {
            JsonToken tokenType1 = jsonReader.TokenType;
            if (tokenType1 == JsonToken.PropertyName)
              empty = jsonReader.Value.ToString();
            if (tokenType1 == JsonToken.String && empty == "colonyName")
              retiredColonyData.colonyName = jsonReader.Value.ToString();
            if (tokenType1 == JsonToken.String && empty == "date")
              retiredColonyData.date = jsonReader.Value.ToString();
            if (tokenType1 == JsonToken.Integer && empty == "cycleCount")
              retiredColonyData.cycleCount = int.Parse(jsonReader.Value.ToString());
            if (tokenType1 == JsonToken.String && empty == "achievements")
              stringList.Add(jsonReader.Value.ToString());
            if (tokenType1 == JsonToken.StartObject && empty == "Duplicants")
            {
              string str1 = (string) null;
              RetiredColonyData.RetiredDuplicantData retiredDuplicantData = new RetiredColonyData.RetiredDuplicantData();
              retiredDuplicantData.accessories = new Dictionary<string, string>();
label_31:
              while (jsonReader.Read())
              {
                tokenType1 = jsonReader.TokenType;
                switch (tokenType1)
                {
                  case JsonToken.PropertyName:
                    str1 = jsonReader.Value.ToString();
                    break;
                  case JsonToken.EndObject:
                    goto label_32;
                }
                if (str1 == "name" && tokenType1 == JsonToken.String)
                  retiredDuplicantData.name = jsonReader.Value.ToString();
                if (str1 == "age" && tokenType1 == JsonToken.Integer)
                  retiredDuplicantData.age = int.Parse(jsonReader.Value.ToString());
                if (str1 == "skillPointsGained" && tokenType1 == JsonToken.Integer)
                  retiredDuplicantData.skillPointsGained = int.Parse(jsonReader.Value.ToString());
                if (str1 == "accessories")
                {
                  string key = (string) null;
                  while (jsonReader.Read())
                  {
                    tokenType1 = jsonReader.TokenType;
                    switch (tokenType1)
                    {
                      case JsonToken.PropertyName:
                        key = jsonReader.Value.ToString();
                        break;
                      case JsonToken.EndObject:
                        goto label_31;
                    }
                    if (key != null && jsonReader.Value != null && tokenType1 == JsonToken.String)
                    {
                      string str2 = jsonReader.Value.ToString();
                      retiredDuplicantData.accessories.Add(key, str2);
                    }
                  }
                }
              }
label_32:
              retiredDuplicantDataList.Add(retiredDuplicantData);
            }
            if (tokenType1 == JsonToken.StartObject && empty == "buildings")
            {
              string str = (string) null;
              string a = (string) null;
              int b = 0;
              while (jsonReader.Read())
              {
                tokenType1 = jsonReader.TokenType;
                switch (tokenType1)
                {
                  case JsonToken.PropertyName:
                    str = jsonReader.Value.ToString();
                    break;
                  case JsonToken.EndObject:
                    goto label_42;
                }
                if (str == "first" && tokenType1 == JsonToken.String)
                  a = jsonReader.Value.ToString();
                if (str == "second" && tokenType1 == JsonToken.Integer)
                  b = int.Parse(jsonReader.Value.ToString());
              }
label_42:
              Tuple<string, int> tuple = new Tuple<string, int>(a, b);
              tupleList1.Add(tuple);
            }
            if (tokenType1 == JsonToken.StartObject && empty == "Stats")
            {
              if (!skipStats)
              {
                string str1 = (string) null;
                RetiredColonyData.RetiredColonyStatistic retiredColonyStatistic = new RetiredColonyData.RetiredColonyStatistic();
                List<Tuple<float, float>> tupleList2 = new List<Tuple<float, float>>();
                while (jsonReader.Read())
                {
                  JsonToken tokenType2 = jsonReader.TokenType;
                  switch (tokenType2)
                  {
                    case JsonToken.PropertyName:
                      str1 = jsonReader.Value.ToString();
                      break;
                    case JsonToken.EndObject:
                      goto label_67;
                  }
                  if (str1 == "id" && tokenType2 == JsonToken.String)
                    retiredColonyStatistic.id = jsonReader.Value.ToString();
                  if (str1 == "name" && tokenType2 == JsonToken.String)
                    retiredColonyStatistic.name = jsonReader.Value.ToString();
                  if (str1 == "nameX" && tokenType2 == JsonToken.String)
                    retiredColonyStatistic.nameX = jsonReader.Value.ToString();
                  if (str1 == "nameY" && tokenType2 == JsonToken.String)
                    retiredColonyStatistic.nameY = jsonReader.Value.ToString();
                  if (str1 == "value" && tokenType2 == JsonToken.StartObject)
                  {
                    string str2 = (string) null;
                    float a = 0.0f;
                    float b = 0.0f;
                    while (jsonReader.Read())
                    {
                      JsonToken tokenType3 = jsonReader.TokenType;
                      switch (tokenType3)
                      {
                        case JsonToken.PropertyName:
                          str2 = jsonReader.Value.ToString();
                          break;
                        case JsonToken.EndObject:
                          goto label_65;
                      }
                      if (str2 == "first" && (tokenType3 == JsonToken.Float || tokenType3 == JsonToken.Integer))
                        a = float.Parse(jsonReader.Value.ToString());
                      if (str2 == "second" && (tokenType3 == JsonToken.Float || tokenType3 == JsonToken.Integer))
                        b = float.Parse(jsonReader.Value.ToString());
                    }
label_65:
                    Tuple<float, float> tuple = new Tuple<float, float>(a, b);
                    tupleList2.Add(tuple);
                  }
                }
label_67:
                retiredColonyStatistic.value = tupleList2.ToArray();
                retiredColonyStatisticList.Add(retiredColonyStatistic);
              }
              else
                break;
            }
          }
          retiredColonyData.Duplicants = retiredDuplicantDataList.ToArray();
          retiredColonyData.Stats = retiredColonyStatisticList.ToArray();
          retiredColonyData.achievements = stringList.ToArray();
          retiredColonyData.buildings = tupleList1;
        }
      }
    }
    return retiredColonyData;
  }

  public static RetiredColonyData[] LoadRetiredColonies(bool skipStats = false)
  {
    List<RetiredColonyData> retiredColonyDataList = new List<RetiredColonyData>();
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    string path = System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    foreach (string directory in Directory.GetDirectories(System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName())))
    {
      foreach (string file in Directory.GetFiles(directory))
      {
        if (file.EndsWith(".json"))
        {
          for (int index = 0; index < RetireColonyUtility.attempt_encodings.Length; ++index)
          {
            Encoding attemptEncoding = RetireColonyUtility.attempt_encodings[index];
            try
            {
              RetiredColonyData retiredColonyData = RetireColonyUtility.LoadRetiredColony(file, skipStats, attemptEncoding);
              if (retiredColonyData != null)
              {
                if (retiredColonyData.colonyName == null)
                  throw new Exception("data.colonyName was null");
                retiredColonyDataList.Add(retiredColonyData);
                break;
              }
              break;
            }
            catch (Exception ex)
            {
              Debug.LogWarningFormat("LoadRetiredColonies failed load {0} [{1}]: {2}", (object) attemptEncoding, (object) file, (object) ex.ToString());
            }
          }
        }
      }
    }
    return retiredColonyDataList.ToArray();
  }

  public static string[] LoadColonySlideshowFiles(string colonyName)
  {
    string path = System.IO.Path.Combine(System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName()), RetireColonyUtility.StripInvalidCharacters(colonyName));
    List<string> stringList = new List<string>();
    if (Directory.Exists(path))
    {
      foreach (string file in Directory.GetFiles(path))
      {
        if (file.EndsWith(".png"))
          stringList.Add(file);
      }
    }
    else
      Debug.LogWarningFormat("LoadColonySlideshow path does not exist or is not directory [{0}]", (object) path);
    return stringList.ToArray();
  }

  public static Sprite[] LoadColonySlideshow(string colonyName)
  {
    string path = System.IO.Path.Combine(System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName()), RetireColonyUtility.StripInvalidCharacters(colonyName));
    List<Sprite> spriteList = new List<Sprite>();
    if (Directory.Exists(path))
    {
      foreach (string file in Directory.GetFiles(path))
      {
        if (file.EndsWith(".png"))
        {
          Texture2D texture2D = new Texture2D(512, 768);
          texture2D.filterMode = FilterMode.Point;
          texture2D.LoadImage(File.ReadAllBytes(file));
          spriteList.Add(Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float) texture2D.width, (float) texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect));
        }
      }
    }
    else
      Debug.LogWarningFormat("LoadColonySlideshow path does not exist or is not directory [{0}]", (object) path);
    return spriteList.ToArray();
  }

  public static Sprite LoadRetiredColonyPreview(string colonyName)
  {
    string path = System.IO.Path.Combine(System.IO.Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName()), RetireColonyUtility.StripInvalidCharacters(colonyName));
    List<string> stringList = new List<string>();
    if (Directory.Exists(path))
    {
      foreach (string file in Directory.GetFiles(path))
      {
        if (file.EndsWith(".png"))
          stringList.Add(file);
      }
    }
    else
      Debug.LogWarningFormat("LoadColonyPreview path does not exist or is not directory [{0}]", (object) path);
    if (stringList.Count <= 0)
      return (Sprite) null;
    Texture2D texture2D = new Texture2D(512, 768);
    texture2D.LoadImage(File.ReadAllBytes(stringList[stringList.Count - 1]));
    return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float) texture2D.width, (float) texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
  }

  public static Sprite LoadColonyPreview(string savePath, string colonyName)
  {
    string path = System.IO.Path.ChangeExtension(savePath, ".png");
    if (File.Exists(path))
    {
      try
      {
        Texture2D texture2D = new Texture2D(512, 768);
        texture2D.LoadImage(File.ReadAllBytes(path));
        return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float) texture2D.width, (float) texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("failed to load preview image!? " + (object) ex));
      }
    }
    return RetireColonyUtility.LoadRetiredColonyPreview(colonyName);
  }

  public static string StripInvalidCharacters(string source)
  {
    foreach (char invalidCharacter in RetireColonyUtility.invalidCharacters)
      source = source.Replace(invalidCharacter, '_');
    source = source.Trim();
    return source;
  }
}
