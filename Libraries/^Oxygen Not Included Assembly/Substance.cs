// Decompiled with JetBrains decompiler
// Type: Substance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Klei;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Substance
{
  [FormerlySerializedAs("overlayColour")]
  public Color32 conduitColour = (Color32) Color.white;
  [SerializeField]
  internal bool showInEditor = true;
  public string name;
  public SimHashes elementID;
  internal Tag nameTag;
  public Color32 colour;
  [FormerlySerializedAs("debugColour")]
  public Color32 uiColour;
  [NonSerialized]
  internal bool renderedByWorld;
  [NonSerialized]
  internal int idx;
  public Material material;
  public KAnimFile anim;
  [NonSerialized]
  internal KAnimFile[] anims;
  [NonSerialized]
  internal ElementsAudio.ElementAudioConfig audioConfig;
  [NonSerialized]
  internal MaterialPropertyBlock propertyBlock;
  [EventRef]
  public string fallingStartSound;
  [EventRef]
  public string fallingStopSound;

  public GameObject SpawnResource(
    Vector3 position,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool prevent_merge = false,
    bool forceTemperature = false,
    bool manual_activation = false)
  {
    GameObject gameObject1 = (GameObject) null;
    PrimaryElement primaryElement = (PrimaryElement) null;
    if (!prevent_merge)
    {
      int cell = Grid.PosToCell(position);
      GameObject gameObject2 = Grid.Objects[cell, 3];
      if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
      {
        Pickupable component1 = gameObject2.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          Tag tag = GameTagExtensions.Create(this.elementID);
          for (ObjectLayerListItem objectLayerListItem = component1.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
          {
            KPrefabID component2 = objectLayerListItem.gameObject.GetComponent<KPrefabID>();
            if (component2.PrefabTag == tag)
            {
              gameObject1 = component2.gameObject;
              primaryElement = component2.GetComponent<PrimaryElement>();
              temperature = SimUtil.CalculateFinalTemperature(primaryElement.Mass, primaryElement.Temperature, mass, temperature);
              position = gameObject1.transform.GetPosition();
              break;
            }
          }
        }
      }
    }
    if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
    {
      gameObject1 = GameUtil.KInstantiate(Assets.GetPrefab(this.nameTag), Grid.SceneLayer.Ore, (string) null, 0);
      primaryElement = gameObject1.GetComponent<PrimaryElement>();
      primaryElement.Mass = mass;
    }
    else
      primaryElement.Mass += mass;
    primaryElement.InternalTemperature = temperature;
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
    gameObject1.transform.SetPosition(position);
    if (!manual_activation)
      this.ActivateSubstanceGameObject(gameObject1, disease_idx, disease_count);
    return gameObject1;
  }

  public void ActivateSubstanceGameObject(GameObject obj, byte disease_idx, int disease_count)
  {
    obj.SetActive(true);
    obj.GetComponent<PrimaryElement>().AddDisease(disease_idx, disease_count, "Substances.SpawnResource");
  }

  private void SetTexture(MaterialPropertyBlock block, string texture_name)
  {
    Texture texture = this.material.GetTexture(texture_name);
    if (!((UnityEngine.Object) texture != (UnityEngine.Object) null))
      return;
    this.propertyBlock.SetTexture(texture_name, texture);
  }

  public void RefreshPropertyBlock()
  {
    if (this.propertyBlock == null)
      this.propertyBlock = new MaterialPropertyBlock();
    if (!((UnityEngine.Object) this.material != (UnityEngine.Object) null))
      return;
    this.SetTexture(this.propertyBlock, "_MainTex");
    this.propertyBlock.SetFloat("_WorldUVScale", this.material.GetFloat("_WorldUVScale"));
    if (!ElementLoader.FindElementByHash(this.elementID).IsSolid)
      return;
    this.SetTexture(this.propertyBlock, "_MainTex2");
    this.SetTexture(this.propertyBlock, "_HeightTex2");
    this.propertyBlock.SetFloat("_Frequency", this.material.GetFloat("_Frequency"));
    this.propertyBlock.SetColor("_ShineColour", this.material.GetColor("_ShineColour"));
    this.propertyBlock.SetColor("_ColourTint", this.material.GetColor("_ColourTint"));
  }

  internal AmbienceType GetAmbience()
  {
    if (this.audioConfig != null)
      return this.audioConfig.ambienceType;
    return AmbienceType.None;
  }

  internal SolidAmbienceType GetSolidAmbience()
  {
    if (this.audioConfig != null)
      return this.audioConfig.solidAmbienceType;
    return SolidAmbienceType.None;
  }

  internal string GetMiningSound()
  {
    if (this.audioConfig != null)
      return this.audioConfig.miningSound;
    return string.Empty;
  }

  internal string GetMiningBreakSound()
  {
    if (this.audioConfig != null)
      return this.audioConfig.miningBreakSound;
    return string.Empty;
  }

  internal string GetOreBumpSound()
  {
    if (this.audioConfig != null)
      return this.audioConfig.oreBumpSound;
    return string.Empty;
  }

  internal string GetFloorEventAudioCategory()
  {
    if (this.audioConfig != null)
      return this.audioConfig.floorEventAudioCategory;
    return string.Empty;
  }

  internal string GetCreatureChewSound()
  {
    if (this.audioConfig != null)
      return this.audioConfig.creatureChewSound;
    return string.Empty;
  }
}
