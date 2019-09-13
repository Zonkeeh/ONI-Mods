// Decompiled with JetBrains decompiler
// Type: PrimaryElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class PrimaryElement : KMonoBehaviour, ISaveLoadable
{
  public static float MAX_MASS = 100000f;
  private static readonly Tag[] metalTags = new Tag[2]
  {
    GameTags.Metal,
    GameTags.RefinedMetal
  };
  private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>((System.Action<PrimaryElement, object>) ((component, data) => component.OnSplitFromChunk(data)));
  private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>((System.Action<PrimaryElement, object>) ((component, data) => component.OnAbsorb(data)));
  public PrimaryElement.GetTemperatureCallback getTemperatureCallback;
  public PrimaryElement.SetTemperatureCallback setTemperatureCallback;
  private PrimaryElement diseaseRedirectTarget;
  private bool useSimDiseaseInfo;
  public const float DefaultChunkMass = 400f;
  [Serialize]
  [HashedEnum]
  public SimHashes ElementID;
  private float _units;
  [Serialize]
  [SerializeField]
  private float _Temperature;
  [Serialize]
  [NonSerialized]
  public bool KeepZeroMassObject;
  [Serialize]
  private HashedString diseaseID;
  [Serialize]
  private int diseaseCount;
  private HandleVector<int>.Handle diseaseHandle;
  public float MassPerUnit;
  [NonSerialized]
  private Element _Element;
  [NonSerialized]
  public System.Action<PrimaryElement> onDataChanged;
  [NonSerialized]
  private bool forcePermanentDiseaseContainer;

  public PrimaryElement()
  {
    // ISSUE: reference to a compiler-generated field
    if (PrimaryElement.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      PrimaryElement.\u003C\u003Ef__mg\u0024cache0 = new PrimaryElement.GetTemperatureCallback(PrimaryElement.OnGetTemperature);
    }
    // ISSUE: reference to a compiler-generated field
    this.getTemperatureCallback = PrimaryElement.\u003C\u003Ef__mg\u0024cache0;
    // ISSUE: reference to a compiler-generated field
    if (PrimaryElement.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      PrimaryElement.\u003C\u003Ef__mg\u0024cache1 = new PrimaryElement.SetTemperatureCallback(PrimaryElement.OnSetTemperature);
    }
    // ISSUE: reference to a compiler-generated field
    this.setTemperatureCallback = PrimaryElement.\u003C\u003Ef__mg\u0024cache1;
    this._units = 1f;
    this.diseaseHandle = HandleVector<int>.InvalidHandle;
    this.MassPerUnit = 1f;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  public void SetUseSimDiseaseInfo(bool use)
  {
    this.useSimDiseaseInfo = use;
  }

  [Serialize]
  public float Units
  {
    get
    {
      return this._units;
    }
    set
    {
      this._units = value;
    }
  }

  public float Temperature
  {
    get
    {
      return this.getTemperatureCallback(this);
    }
    set
    {
      this.SetTemperature(value);
    }
  }

  public float InternalTemperature
  {
    get
    {
      return this._Temperature;
    }
    set
    {
      this._Temperature = value;
    }
  }

  [OnSerializing]
  private void OnSerializing()
  {
    this._Temperature = this.Temperature;
    this.SanitizeMassAndTemperature();
    this.diseaseID.HashValue = 0;
    this.diseaseCount = 0;
    if (this.useSimDiseaseInfo)
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      if (Grid.DiseaseIdx[cell] == byte.MaxValue)
        return;
      this.diseaseID = Db.Get().Diseases[(int) Grid.DiseaseIdx[cell]].id;
      this.diseaseCount = Grid.DiseaseCount[cell];
    }
    else
    {
      if (!this.diseaseHandle.IsValid())
        return;
      DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
      if (header.diseaseIdx == byte.MaxValue)
        return;
      this.diseaseID = Db.Get().Diseases[(int) header.diseaseIdx].id;
      this.diseaseCount = header.diseaseCount;
    }
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if (this.ElementID == (SimHashes) 351109216)
      this.ElementID = SimHashes.Creature;
    this.SanitizeMassAndTemperature();
    float temperature = this._Temperature;
    if (float.IsNaN(temperature) || float.IsInfinity(temperature) || ((double) temperature < 0.0 || 10000.0 < (double) temperature))
    {
      DeserializeWarnings.Instance.PrimaryElementTemperatureIsNan.Warn(string.Format("{0} has invalid temperature of {1}. Resetting temperature.", (object) this.name, (object) this.Temperature), (GameObject) null);
      temperature = this.Element.defaultValues.temperature;
    }
    this._Temperature = temperature;
    this.Temperature = temperature;
    if (this.Element == null)
      DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(this.name + "Primary element has no element.", (GameObject) null);
    if ((double) this.Mass < 0.0)
    {
      DebugUtil.DevLogError((UnityEngine.Object) this.gameObject, "deserialized ore with less than 0 mass. Error! Destroying");
      Util.KDestroyGameObject(this.gameObject);
    }
    else
    {
      if (this.onDataChanged != null)
        this.onDataChanged(this);
      byte index = Db.Get().Diseases.GetIndex(this.diseaseID);
      if (index == byte.MaxValue || this.diseaseCount <= 0)
      {
        if (!this.diseaseHandle.IsValid())
          return;
        GameComps.DiseaseContainers.Remove(this.gameObject);
        this.diseaseHandle.Clear();
      }
      else if (this.diseaseHandle.IsValid())
      {
        DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
        header.diseaseIdx = index;
        header.diseaseCount = this.diseaseCount;
        GameComps.DiseaseContainers.SetHeader(this.diseaseHandle, header);
      }
      else
        this.diseaseHandle = GameComps.DiseaseContainers.Add(this.gameObject, index, this.diseaseCount);
    }
  }

  protected override void OnLoadLevel()
  {
    base.OnLoadLevel();
  }

  private void SanitizeMassAndTemperature()
  {
    if ((double) this._Temperature <= 0.0)
    {
      DebugUtil.DevLogErrorFormat((UnityEngine.Object) this.gameObject, "{0} is attempting to serialize a temperature of <= 0K. Resetting to default.", (object) this.gameObject.name);
      this._Temperature = this.Element.defaultValues.temperature;
    }
    if ((double) this.Mass <= (double) PrimaryElement.MAX_MASS)
      return;
    DebugUtil.DevLogErrorFormat((UnityEngine.Object) this.gameObject, "{0} is attempting to serialize very large mass {1}. Resetting to default.", (object) this.gameObject.name, (object) this.Mass);
    this.Mass = this.Element.defaultValues.mass;
  }

  public float Mass
  {
    get
    {
      return this.Units * this.MassPerUnit;
    }
    set
    {
      this.SetMass(value);
      if (this.onDataChanged == null)
        return;
      this.onDataChanged(this);
    }
  }

  private void SetMass(float mass)
  {
    if (((double) mass > (double) PrimaryElement.MAX_MASS || (double) mass < 0.0) && this.ElementID != SimHashes.Regolith)
      DebugUtil.DevLogErrorFormat((UnityEngine.Object) this.gameObject, "{0} is getting an abnormal mass set {1}.", (object) this.gameObject.name, (object) this.Mass);
    mass = Mathf.Clamp(mass, 0.0f, PrimaryElement.MAX_MASS);
    this.Units = mass / this.MassPerUnit;
    if ((double) this.Units <= 0.0 && !this.KeepZeroMassObject)
      Util.KDestroyGameObject(this.gameObject);
    else if (!this.KeepZeroMassObject && (double) this.Units <= 0.0)
      throw new ArgumentException("Invalid mass");
  }

  private void SetTemperature(float temperature)
  {
    if (float.IsNaN(temperature) || float.IsInfinity(temperature))
    {
      DebugUtil.LogErrorArgs((UnityEngine.Object) this.gameObject, (object) ("Invalid temperature [" + (object) temperature + "]"));
    }
    else
    {
      if ((double) temperature <= 0.0)
        KCrashReporter.Assert(false, "Tried to set PrimaryElement.Temperature to a value <= 0");
      this.setTemperatureCallback(this, temperature);
    }
  }

  public void SetMassTemperature(float mass, float temperature)
  {
    this.SetMass(mass);
    this.SetTemperature(temperature);
  }

  public Element Element
  {
    get
    {
      if (this._Element == null)
        this._Element = ElementLoader.FindElementByHash(this.ElementID);
      return this._Element;
    }
  }

  public byte DiseaseIdx
  {
    get
    {
      if ((bool) ((UnityEngine.Object) this.diseaseRedirectTarget))
        return this.diseaseRedirectTarget.DiseaseIdx;
      byte num = byte.MaxValue;
      if (this.useSimDiseaseInfo)
      {
        int cell = Grid.PosToCell(this.transform.GetPosition());
        num = Grid.DiseaseIdx[cell];
      }
      else if (this.diseaseHandle.IsValid())
        num = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseIdx;
      return num;
    }
  }

  public int DiseaseCount
  {
    get
    {
      if ((bool) ((UnityEngine.Object) this.diseaseRedirectTarget))
        return this.diseaseRedirectTarget.DiseaseCount;
      int num = 0;
      if (this.useSimDiseaseInfo)
      {
        int cell = Grid.PosToCell(this.transform.GetPosition());
        num = Grid.DiseaseCount[cell];
      }
      else if (this.diseaseHandle.IsValid())
        num = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseCount;
      return num;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GameComps.InfraredVisualizers.Add(this.gameObject);
    this.Subscribe<PrimaryElement>(1335436905, PrimaryElement.OnSplitFromChunkDelegate);
    this.Subscribe<PrimaryElement>(-2064133523, PrimaryElement.OnAbsorbDelegate);
  }

  protected override void OnSpawn()
  {
    Attributes attributes = this.GetAttributes();
    if (attributes == null)
      return;
    foreach (AttributeModifier attributeModifier in this.Element.attributeModifiers)
      attributes.Add(attributeModifier);
  }

  public void ForcePermanentDiseaseContainer(bool force_on)
  {
    if (force_on)
    {
      if (!this.diseaseHandle.IsValid())
        this.diseaseHandle = GameComps.DiseaseContainers.Add(this.gameObject, byte.MaxValue, 0);
    }
    else if (this.diseaseHandle.IsValid() && this.DiseaseIdx == byte.MaxValue)
    {
      GameComps.DiseaseContainers.Remove(this.gameObject);
      this.diseaseHandle.Clear();
    }
    this.forcePermanentDiseaseContainer = force_on;
  }

  protected override void OnCleanUp()
  {
    GameComps.InfraredVisualizers.Remove(this.gameObject);
    if (this.diseaseHandle.IsValid())
    {
      GameComps.DiseaseContainers.Remove(this.gameObject);
      this.diseaseHandle.Clear();
    }
    base.OnCleanUp();
  }

  public void SetElement(SimHashes element_id)
  {
    this.ElementID = element_id;
    this.UpdateTags();
  }

  public void UpdateTags()
  {
    if (this.ElementID == (SimHashes) 0)
    {
      Debug.Log((object) "UpdateTags() Primary element 0", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      KPrefabID component = this.GetComponent<KPrefabID>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      List<Tag> tagList = new List<Tag>();
      Element element = this.Element;
      tagList.Add(GameTagExtensions.Create(element.id));
      foreach (Tag oreTag in element.oreTags)
        tagList.Add(oreTag);
      if (component.HasAnyTags(PrimaryElement.metalTags))
        tagList.Add(GameTags.StoredMetal);
      foreach (Tag tag in tagList)
        component.AddTag(tag, false);
    }
  }

  public void ModifyDiseaseCount(int delta, string reason)
  {
    if ((bool) ((UnityEngine.Object) this.diseaseRedirectTarget))
      this.diseaseRedirectTarget.ModifyDiseaseCount(delta, reason);
    else if (this.useSimDiseaseInfo)
    {
      SimMessages.ModifyDiseaseOnCell(Grid.PosToCell((KMonoBehaviour) this), byte.MaxValue, delta);
    }
    else
    {
      if (delta == 0 || !this.diseaseHandle.IsValid() || (GameComps.DiseaseContainers.ModifyDiseaseCount(this.diseaseHandle, delta) > 0 || this.forcePermanentDiseaseContainer))
        return;
      this.Trigger(-1689370368, (object) false);
      GameComps.DiseaseContainers.Remove(this.gameObject);
      this.diseaseHandle.Clear();
    }
  }

  public void AddDisease(byte disease_idx, int delta, string reason)
  {
    if (delta == 0)
      return;
    if ((bool) ((UnityEngine.Object) this.diseaseRedirectTarget))
      this.diseaseRedirectTarget.AddDisease(disease_idx, delta, reason);
    else if (this.useSimDiseaseInfo)
      SimMessages.ModifyDiseaseOnCell(Grid.PosToCell((KMonoBehaviour) this), disease_idx, delta);
    else if (this.diseaseHandle.IsValid())
    {
      if (GameComps.DiseaseContainers.AddDisease(this.diseaseHandle, disease_idx, delta) > 0)
        return;
      GameComps.DiseaseContainers.Remove(this.gameObject);
      this.diseaseHandle.Clear();
    }
    else
    {
      if (delta <= 0)
        return;
      this.diseaseHandle = GameComps.DiseaseContainers.Add(this.gameObject, disease_idx, delta);
      this.Trigger(-1689370368, (object) true);
      this.Trigger(-283306403, (object) null);
    }
  }

  private static float OnGetTemperature(PrimaryElement primary_element)
  {
    return primary_element._Temperature;
  }

  private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    Debug.Assert(!float.IsNaN(temperature));
    if ((double) temperature <= 0.0)
      DebugUtil.LogErrorArgs((UnityEngine.Object) primary_element.gameObject, (object) (primary_element.gameObject.name + " has a temperature of zero which has always been an error in my experience."));
    primary_element._Temperature = temperature;
  }

  private void OnSplitFromChunk(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
      return;
    float percent = this.Units / (this.Units + pickupable.PrimaryElement.Units);
    SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(pickupable.PrimaryElement, percent);
    this.AddDisease(percentOfDisease.idx, percentOfDisease.count, "PrimaryElement.SplitFromChunk");
    pickupable.PrimaryElement.ModifyDiseaseCount(-percentOfDisease.count, "PrimaryElement.SplitFromChunk");
  }

  private void OnAbsorb(object data)
  {
    Pickupable pickupable = (Pickupable) data;
    if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
      return;
    this.AddDisease(pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, "PrimaryElement.OnAbsorb");
  }

  private void SetDiseaseVisualProvider(GameObject visualizer)
  {
    HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(this.gameObject);
    if (!(handle != HandleVector<int>.InvalidHandle))
      return;
    DiseaseContainer payload = GameComps.DiseaseContainers.GetPayload(handle);
    payload.visualDiseaseProvider = visualizer;
    GameComps.DiseaseContainers.SetPayload(handle, ref payload);
  }

  public void RedirectDisease(GameObject target)
  {
    this.SetDiseaseVisualProvider(target);
    this.diseaseRedirectTarget = !(bool) ((UnityEngine.Object) target) ? (PrimaryElement) null : target.GetComponent<PrimaryElement>();
    Debug.Assert((UnityEngine.Object) this.diseaseRedirectTarget != (UnityEngine.Object) this, (object) "Disease redirect target set to myself");
  }

  public delegate float GetTemperatureCallback(PrimaryElement primary_element);

  public delegate void SetTemperatureCallback(PrimaryElement primary_element, float temperature);
}
