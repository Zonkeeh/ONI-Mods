// Decompiled with JetBrains decompiler
// Type: TechInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class TechInstance
{
  public ResearchPointInventory progressInventory = new ResearchPointInventory();
  public Tech tech;
  private bool complete;

  public TechInstance(Tech tech)
  {
    this.tech = tech;
  }

  public bool IsComplete()
  {
    return this.complete;
  }

  public void Purchased()
  {
    if (this.complete)
      return;
    this.complete = true;
  }

  public TechInstance.SaveData Save()
  {
    string[] array1 = new string[this.progressInventory.PointsByTypeID.Count];
    this.progressInventory.PointsByTypeID.Keys.CopyTo(array1, 0);
    float[] array2 = new float[this.progressInventory.PointsByTypeID.Count];
    this.progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
    return new TechInstance.SaveData()
    {
      techId = this.tech.Id,
      complete = this.complete,
      inventoryIDs = array1,
      inventoryValues = array2
    };
  }

  public void Load(TechInstance.SaveData save_data)
  {
    this.complete = save_data.complete;
    for (int index = 0; index < save_data.inventoryIDs.Length; ++index)
      this.progressInventory.AddResearchPoints(save_data.inventoryIDs[index], save_data.inventoryValues[index]);
  }

  public struct SaveData
  {
    public string techId;
    public bool complete;
    public string[] inventoryIDs;
    public float[] inventoryValues;
  }
}
