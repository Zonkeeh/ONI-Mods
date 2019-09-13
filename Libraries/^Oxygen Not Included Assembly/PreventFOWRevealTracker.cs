// Decompiled with JetBrains decompiler
// Type: PreventFOWRevealTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class PreventFOWRevealTracker : KMonoBehaviour
{
  [Serialize]
  public List<int> preventFOWRevealCells;

  [OnSerializing]
  private void OnSerialize()
  {
    this.preventFOWRevealCells.Clear();
    for (int index = 0; index < Grid.VisMasks.Length; ++index)
    {
      if (Grid.PreventFogOfWarReveal[index])
        this.preventFOWRevealCells.Add(index);
    }
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    foreach (int preventFowRevealCell in this.preventFOWRevealCells)
      Grid.PreventFogOfWarReveal[preventFowRevealCell] = true;
  }
}
