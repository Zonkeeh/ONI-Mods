// Decompiled with JetBrains decompiler
// Type: NotificationExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public static class NotificationExtensions
{
  public static string ReduceMessages(this List<Notification> notifications, bool countNames = true)
  {
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    foreach (Notification notification in notifications)
    {
      int num = 0;
      if (!dictionary.TryGetValue(notification.NotifierName, out num))
        dictionary[notification.NotifierName] = 0;
      dictionary[notification.NotifierName] = num + 1;
    }
    string str = string.Empty;
    foreach (KeyValuePair<string, int> keyValuePair in dictionary)
    {
      if (countNames)
        str = str + "\n" + keyValuePair.Key + "(" + (object) keyValuePair.Value + ")";
      else
        str = str + "\n" + keyValuePair.Key;
    }
    return str;
  }
}
