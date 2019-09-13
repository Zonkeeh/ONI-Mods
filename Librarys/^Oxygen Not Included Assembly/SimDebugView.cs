// Decompiled with JetBrains decompiler
// Type: SimDebugView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

public class SimDebugView : KMonoBehaviour
{
  public static float minimumBreathable = 0.05f;
  public static float optimallyBreathable = 1f;
  public static readonly Color[] dbColours = new Color[13]
  {
    new Color(0.0f, 0.0f, 0.0f, 0.0f),
    new Color(1f, 1f, 1f, 0.3f),
    new Color(0.7058824f, 0.8235294f, 1f, 0.2f),
    new Color(0.0f, 0.3137255f, 1f, 0.3f),
    new Color(0.7058824f, 1f, 0.7058824f, 0.5f),
    new Color(0.07843138f, 1f, 0.0f, 0.7f),
    new Color(1f, 0.9019608f, 0.7058824f, 0.9f),
    new Color(1f, 0.8235294f, 0.0f, 0.9f),
    new Color(1f, 0.7176471f, 0.3019608f, 0.9f),
    new Color(1f, 0.4156863f, 0.0f, 0.9f),
    new Color(1f, 0.7058824f, 0.7058824f, 1f),
    new Color(1f, 0.0f, 0.0f, 1f),
    new Color(1f, 0.0f, 0.0f, 1f)
  };
  private static float minMinionTemperature = 260f;
  private static float maxMinionTemperature = 310f;
  private static float minMinionPressure = 80f;
  [SerializeField]
  public Material material;
  public Material diseaseMaterial;
  public bool hideFOW;
  public const int colourSize = 4;
  private byte[] texBytes;
  private int currentFrame;
  [SerializeField]
  private Texture2D tex;
  [SerializeField]
  private GameObject plane;
  private HashedString mode;
  private SimDebugView.GameGridMode gameGridMode;
  private PathProber selectedPathProber;
  public float minTempExpected;
  public float maxTempExpected;
  public float minMassExpected;
  public float maxMassExpected;
  public float minPressureExpected;
  public float maxPressureExpected;
  public float minThermalConductivity;
  public float maxThermalConductivity;
  public float thresholdRange;
  public float thresholdOpacity;
  public SimDebugView.ColorThreshold[] temperatureThresholds;
  public SimDebugView.ColorThreshold[] heatFlowThresholds;
  public Color32[] networkColours;
  public Gradient breathableGradient;
  public Color32 unbreathableColour;
  public Color32[] toxicColour;
  public static SimDebugView Instance;
  private WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData> updateSimViewWorkItems;
  private int selectedCell;
  private Dictionary<HashedString, System.Action<SimDebugView, Texture>> dataUpdateFuncs;
  private Dictionary<HashedString, Func<SimDebugView, int, Color>> getColourFuncs;

  public SimDebugView()
  {
    Dictionary<HashedString, System.Action<SimDebugView, Texture>> dictionary1 = new Dictionary<HashedString, System.Action<SimDebugView, Texture>>();
    Dictionary<HashedString, System.Action<SimDebugView, Texture>> dictionary2 = dictionary1;
    HashedString id1 = global::OverlayModes.Temperature.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache1 = new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SimDebugView, Texture> fMgCache1 = SimDebugView.\u003C\u003Ef__mg\u0024cache1;
    dictionary2.Add(id1, fMgCache1);
    Dictionary<HashedString, System.Action<SimDebugView, Texture>> dictionary3 = dictionary1;
    HashedString id2 = global::OverlayModes.Oxygen.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache2 = new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SimDebugView, Texture> fMgCache2 = SimDebugView.\u003C\u003Ef__mg\u0024cache2;
    dictionary3.Add(id2, fMgCache2);
    Dictionary<HashedString, System.Action<SimDebugView, Texture>> dictionary4 = dictionary1;
    HashedString id3 = global::OverlayModes.Decor.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache3 = new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SimDebugView, Texture> fMgCache3 = SimDebugView.\u003C\u003Ef__mg\u0024cache3;
    dictionary4.Add(id3, fMgCache3);
    Dictionary<HashedString, System.Action<SimDebugView, Texture>> dictionary5 = dictionary1;
    HashedString id4 = global::OverlayModes.TileMode.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache4 = new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SimDebugView, Texture> fMgCache4 = SimDebugView.\u003C\u003Ef__mg\u0024cache4;
    dictionary5.Add(id4, fMgCache4);
    Dictionary<HashedString, System.Action<SimDebugView, Texture>> dictionary6 = dictionary1;
    HashedString id5 = global::OverlayModes.Disease.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache5 = new System.Action<SimDebugView, Texture>(SimDebugView.SetDisease);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SimDebugView, Texture> fMgCache5 = SimDebugView.\u003C\u003Ef__mg\u0024cache5;
    dictionary6.Add(id5, fMgCache5);
    this.dataUpdateFuncs = dictionary1;
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary7 = new Dictionary<HashedString, Func<SimDebugView, int, Color>>();
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary8 = dictionary7;
    HashedString id6 = global::OverlayModes.ThermalConductivity.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache6 = new Func<SimDebugView, int, Color>(SimDebugView.GetThermalConductivityColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache6 = SimDebugView.\u003C\u003Ef__mg\u0024cache6;
    dictionary8.Add(id6, fMgCache6);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary9 = dictionary7;
    HashedString id7 = global::OverlayModes.Temperature.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache7 = new Func<SimDebugView, int, Color>(SimDebugView.GetNormalizedTemperatureColourMode);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache7 = SimDebugView.\u003C\u003Ef__mg\u0024cache7;
    dictionary9.Add(id7, fMgCache7);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary10 = dictionary7;
    HashedString id8 = global::OverlayModes.Disease.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache8 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache8 = new Func<SimDebugView, int, Color>(SimDebugView.GetDiseaseColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache8 = SimDebugView.\u003C\u003Ef__mg\u0024cache8;
    dictionary10.Add(id8, fMgCache8);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary11 = dictionary7;
    HashedString id9 = global::OverlayModes.Decor.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache9 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache9 = new Func<SimDebugView, int, Color>(SimDebugView.GetDecorColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache9 = SimDebugView.\u003C\u003Ef__mg\u0024cache9;
    dictionary11.Add(id9, fMgCache9);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary12 = dictionary7;
    HashedString id10 = global::OverlayModes.Oxygen.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cacheA == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cacheA = new Func<SimDebugView, int, Color>(SimDebugView.GetOxygenMapColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCacheA = SimDebugView.\u003C\u003Ef__mg\u0024cacheA;
    dictionary12.Add(id10, fMgCacheA);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary13 = dictionary7;
    HashedString id11 = global::OverlayModes.Light.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cacheB == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cacheB = new Func<SimDebugView, int, Color>(SimDebugView.GetLightColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCacheB = SimDebugView.\u003C\u003Ef__mg\u0024cacheB;
    dictionary13.Add(id11, fMgCacheB);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary14 = dictionary7;
    HashedString id12 = global::OverlayModes.Radiation.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cacheC == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cacheC = new Func<SimDebugView, int, Color>(SimDebugView.GetRadiationColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCacheC = SimDebugView.\u003C\u003Ef__mg\u0024cacheC;
    dictionary14.Add(id12, fMgCacheC);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary15 = dictionary7;
    HashedString id13 = global::OverlayModes.Rooms.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cacheD == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cacheD = new Func<SimDebugView, int, Color>(SimDebugView.GetRoomsColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCacheD = SimDebugView.\u003C\u003Ef__mg\u0024cacheD;
    dictionary15.Add(id13, fMgCacheD);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary16 = dictionary7;
    HashedString id14 = global::OverlayModes.TileMode.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cacheE == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cacheE = new Func<SimDebugView, int, Color>(SimDebugView.GetTileColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCacheE = SimDebugView.\u003C\u003Ef__mg\u0024cacheE;
    dictionary16.Add(id14, fMgCacheE);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary17 = dictionary7;
    HashedString id15 = global::OverlayModes.Suit.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cacheF == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cacheF = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCacheF = SimDebugView.\u003C\u003Ef__mg\u0024cacheF;
    dictionary17.Add(id15, fMgCacheF);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary18 = dictionary7;
    HashedString id16 = global::OverlayModes.Priorities.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache10 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache10 = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache10 = SimDebugView.\u003C\u003Ef__mg\u0024cache10;
    dictionary18.Add(id16, fMgCache10);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary19 = dictionary7;
    HashedString id17 = global::OverlayModes.Crop.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache11 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache11 = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache11 = SimDebugView.\u003C\u003Ef__mg\u0024cache11;
    dictionary19.Add(id17, fMgCache11);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary20 = dictionary7;
    HashedString id18 = global::OverlayModes.Harvest.ID;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache12 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache12 = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache12 = SimDebugView.\u003C\u003Ef__mg\u0024cache12;
    dictionary20.Add(id18, fMgCache12);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary21 = dictionary7;
    HashedString gameGrid = SimDebugView.OverlayModes.GameGrid;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache13 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache13 = new Func<SimDebugView, int, Color>(SimDebugView.GetGameGridColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache13 = SimDebugView.\u003C\u003Ef__mg\u0024cache13;
    dictionary21.Add(gameGrid, fMgCache13);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary22 = dictionary7;
    HashedString stateChange = SimDebugView.OverlayModes.StateChange;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache14 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache14 = new Func<SimDebugView, int, Color>(SimDebugView.GetStateChangeColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache14 = SimDebugView.\u003C\u003Ef__mg\u0024cache14;
    dictionary22.Add(stateChange, fMgCache14);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary23 = dictionary7;
    HashedString simCheckErrorMap = SimDebugView.OverlayModes.SimCheckErrorMap;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache15 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache15 = new Func<SimDebugView, int, Color>(SimDebugView.GetSimCheckErrorMapColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache15 = SimDebugView.\u003C\u003Ef__mg\u0024cache15;
    dictionary23.Add(simCheckErrorMap, fMgCache15);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary24 = dictionary7;
    HashedString foundation = SimDebugView.OverlayModes.Foundation;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache16 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache16 = new Func<SimDebugView, int, Color>(SimDebugView.GetFoundationColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache16 = SimDebugView.\u003C\u003Ef__mg\u0024cache16;
    dictionary24.Add(foundation, fMgCache16);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary25 = dictionary7;
    HashedString fakeFloor = SimDebugView.OverlayModes.FakeFloor;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache17 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache17 = new Func<SimDebugView, int, Color>(SimDebugView.GetFakeFloorColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache17 = SimDebugView.\u003C\u003Ef__mg\u0024cache17;
    dictionary25.Add(fakeFloor, fMgCache17);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary26 = dictionary7;
    HashedString dupePassable = SimDebugView.OverlayModes.DupePassable;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache18 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache18 = new Func<SimDebugView, int, Color>(SimDebugView.GetDupePassableColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache18 = SimDebugView.\u003C\u003Ef__mg\u0024cache18;
    dictionary26.Add(dupePassable, fMgCache18);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary27 = dictionary7;
    HashedString dupeImpassable = SimDebugView.OverlayModes.DupeImpassable;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache19 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache19 = new Func<SimDebugView, int, Color>(SimDebugView.GetDupeImpassableColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache19 = SimDebugView.\u003C\u003Ef__mg\u0024cache19;
    dictionary27.Add(dupeImpassable, fMgCache19);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary28 = dictionary7;
    HashedString critterImpassable = SimDebugView.OverlayModes.CritterImpassable;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache1A == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache1A = new Func<SimDebugView, int, Color>(SimDebugView.GetCritterImpassableColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache1A = SimDebugView.\u003C\u003Ef__mg\u0024cache1A;
    dictionary28.Add(critterImpassable, fMgCache1A);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary29 = dictionary7;
    HashedString minionGroupProber = SimDebugView.OverlayModes.MinionGroupProber;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache1B == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache1B = new Func<SimDebugView, int, Color>(SimDebugView.GetMinionGroupProberColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache1B = SimDebugView.\u003C\u003Ef__mg\u0024cache1B;
    dictionary29.Add(minionGroupProber, fMgCache1B);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary30 = dictionary7;
    HashedString pathProber = SimDebugView.OverlayModes.PathProber;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache1C == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache1C = new Func<SimDebugView, int, Color>(SimDebugView.GetPathProberColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache1C = SimDebugView.\u003C\u003Ef__mg\u0024cache1C;
    dictionary30.Add(pathProber, fMgCache1C);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary31 = dictionary7;
    HashedString reserved = SimDebugView.OverlayModes.Reserved;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache1D == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache1D = new Func<SimDebugView, int, Color>(SimDebugView.GetReservedColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache1D = SimDebugView.\u003C\u003Ef__mg\u0024cache1D;
    dictionary31.Add(reserved, fMgCache1D);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary32 = dictionary7;
    HashedString allowPathFinding = SimDebugView.OverlayModes.AllowPathFinding;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache1E == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache1E = new Func<SimDebugView, int, Color>(SimDebugView.GetAllowPathFindingColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache1E = SimDebugView.\u003C\u003Ef__mg\u0024cache1E;
    dictionary32.Add(allowPathFinding, fMgCache1E);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary33 = dictionary7;
    HashedString danger = SimDebugView.OverlayModes.Danger;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache1F == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache1F = new Func<SimDebugView, int, Color>(SimDebugView.GetDangerColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache1F = SimDebugView.\u003C\u003Ef__mg\u0024cache1F;
    dictionary33.Add(danger, fMgCache1F);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary34 = dictionary7;
    HashedString minionOccupied = SimDebugView.OverlayModes.MinionOccupied;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache20 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache20 = new Func<SimDebugView, int, Color>(SimDebugView.GetMinionOccupiedColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache20 = SimDebugView.\u003C\u003Ef__mg\u0024cache20;
    dictionary34.Add(minionOccupied, fMgCache20);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary35 = dictionary7;
    HashedString pressure = SimDebugView.OverlayModes.Pressure;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache21 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache21 = new Func<SimDebugView, int, Color>(SimDebugView.GetPressureMapColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache21 = SimDebugView.\u003C\u003Ef__mg\u0024cache21;
    dictionary35.Add(pressure, fMgCache21);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary36 = dictionary7;
    HashedString tileType = SimDebugView.OverlayModes.TileType;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache22 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache22 = new Func<SimDebugView, int, Color>(SimDebugView.GetTileTypeColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache22 = SimDebugView.\u003C\u003Ef__mg\u0024cache22;
    dictionary36.Add(tileType, fMgCache22);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary37 = dictionary7;
    HashedString state = SimDebugView.OverlayModes.State;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache23 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache23 = new Func<SimDebugView, int, Color>(SimDebugView.GetStateMapColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache23 = SimDebugView.\u003C\u003Ef__mg\u0024cache23;
    dictionary37.Add(state, fMgCache23);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary38 = dictionary7;
    HashedString solidLiquid = SimDebugView.OverlayModes.SolidLiquid;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache24 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache24 = new Func<SimDebugView, int, Color>(SimDebugView.GetSolidLiquidMapColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache24 = SimDebugView.\u003C\u003Ef__mg\u0024cache24;
    dictionary38.Add(solidLiquid, fMgCache24);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary39 = dictionary7;
    HashedString mass = SimDebugView.OverlayModes.Mass;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache25 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache25 = new Func<SimDebugView, int, Color>(SimDebugView.GetMassColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache25 = SimDebugView.\u003C\u003Ef__mg\u0024cache25;
    dictionary39.Add(mass, fMgCache25);
    Dictionary<HashedString, Func<SimDebugView, int, Color>> dictionary40 = dictionary7;
    HashedString joules = SimDebugView.OverlayModes.Joules;
    // ISSUE: reference to a compiler-generated field
    if (SimDebugView.\u003C\u003Ef__mg\u0024cache26 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SimDebugView.\u003C\u003Ef__mg\u0024cache26 = new Func<SimDebugView, int, Color>(SimDebugView.GetJoulesColour);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SimDebugView, int, Color> fMgCache26 = SimDebugView.\u003C\u003Ef__mg\u0024cache26;
    dictionary40.Add(joules, fMgCache26);
    this.getColourFuncs = dictionary7;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  public static void DestroyInstance()
  {
    SimDebugView.Instance = (SimDebugView) null;
  }

  protected override void OnPrefabInit()
  {
    SimDebugView.Instance = this;
    this.material = UnityEngine.Object.Instantiate<Material>(this.material);
    this.diseaseMaterial = UnityEngine.Object.Instantiate<Material>(this.diseaseMaterial);
  }

  protected override void OnSpawn()
  {
    SimDebugViewCompositor.Instance.material.SetColor("_Color0", this.temperatureThresholds[0].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color1", this.temperatureThresholds[1].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color2", this.temperatureThresholds[2].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color3", this.temperatureThresholds[3].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color4", this.temperatureThresholds[4].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color5", this.temperatureThresholds[5].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color6", this.temperatureThresholds[6].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color7", this.temperatureThresholds[7].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color0", this.heatFlowThresholds[0].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color1", this.heatFlowThresholds[1].color);
    SimDebugViewCompositor.Instance.material.SetColor("_Color2", this.heatFlowThresholds[2].color);
    this.SetMode(global::OverlayModes.None.ID);
  }

  public void OnReset()
  {
    this.plane = SimDebugView.CreatePlane(nameof (SimDebugView), this.transform);
    this.tex = SimDebugView.CreateTexture(out this.texBytes, Grid.WidthInCells, Grid.HeightInCells);
    this.plane.GetComponent<Renderer>().sharedMaterial = this.material;
    this.plane.GetComponent<Renderer>().sharedMaterial.mainTexture = (Texture) this.tex;
    this.plane.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, -6f));
    this.SetMode(global::OverlayModes.None.ID);
  }

  public static Texture2D CreateTexture(out byte[] textureBytes, int width, int height)
  {
    textureBytes = new byte[width * height * 4];
    Texture2D texture2D = new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None);
    texture2D.name = nameof (SimDebugView);
    texture2D.wrapMode = TextureWrapMode.Clamp;
    texture2D.filterMode = FilterMode.Point;
    return texture2D;
  }

  public static GameObject CreatePlane(string layer, Transform parent)
  {
    GameObject go = new GameObject();
    go.name = "overlayViewDisplayPlane";
    go.SetLayerRecursively(LayerMask.NameToLayer(layer));
    go.transform.SetParent(parent);
    go.transform.SetPosition(Vector3.zero);
    go.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
    MeshFilter meshFilter = go.AddComponent<MeshFilter>();
    Mesh mesh = new Mesh();
    meshFilter.mesh = mesh;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    float y = 2f * (float) Grid.HeightInCells;
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3((float) Grid.WidthInCells, 0.0f, 0.0f),
      new Vector3(0.0f, y, 0.0f),
      new Vector3(Grid.WidthInMeters, y, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 2f),
      new Vector2(1f, 2f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    mesh.vertices = vector3Array2;
    mesh.uv = vector2Array2;
    mesh.triangles = numArray2;
    Vector2 vector2 = new Vector2((float) Grid.WidthInCells, y);
    mesh.bounds = new Bounds(new Vector3(0.5f * vector2.x, 0.5f * vector2.y, 0.0f), new Vector3(vector2.x, vector2.y, 0.0f));
    return go;
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.plane == (UnityEngine.Object) null)
      return;
    bool flag = this.mode != global::OverlayModes.None.ID;
    this.plane.SetActive(flag);
    SimDebugViewCompositor.Instance.Toggle(this.mode != global::OverlayModes.None.ID && !GameUtil.IsCapturingTimeLapse());
    SimDebugViewCompositor.Instance.material.SetVector("_Thresholds0", new Vector4(0.1f, 0.2f, 0.3f, 0.4f));
    SimDebugViewCompositor.Instance.material.SetVector("_Thresholds1", new Vector4(0.5f, 0.6f, 0.7f, 0.8f));
    float x = 0.0f;
    if (this.mode == global::OverlayModes.ThermalConductivity.ID || this.mode == global::OverlayModes.Temperature.ID)
      x = 1f;
    SimDebugViewCompositor.Instance.material.SetVector("_ThresholdParameters", new Vector4(x, this.thresholdRange, this.thresholdOpacity, 0.0f));
    if (!flag)
      return;
    this.UpdateData(this.tex, this.texBytes, this.mode, (byte) 192);
  }

  private static void SetDefaultBilinear(SimDebugView instance, Texture texture)
  {
    Renderer component = instance.plane.GetComponent<Renderer>();
    component.sharedMaterial = instance.material;
    component.sharedMaterial.mainTexture = (Texture) instance.tex;
    texture.filterMode = FilterMode.Bilinear;
  }

  private static void SetDefaultPoint(SimDebugView instance, Texture texture)
  {
    Renderer component = instance.plane.GetComponent<Renderer>();
    component.sharedMaterial = instance.material;
    component.sharedMaterial.mainTexture = (Texture) instance.tex;
    texture.filterMode = FilterMode.Point;
  }

  private static void SetDisease(SimDebugView instance, Texture texture)
  {
    Renderer component = instance.plane.GetComponent<Renderer>();
    component.sharedMaterial = instance.diseaseMaterial;
    component.sharedMaterial.mainTexture = (Texture) instance.tex;
    texture.filterMode = FilterMode.Bilinear;
  }

  public void UpdateData(
    Texture2D texture,
    byte[] textureBytes,
    HashedString viewMode,
    byte alpha)
  {
    System.Action<SimDebugView, Texture> fMgCache0;
    if (!this.dataUpdateFuncs.TryGetValue(viewMode, out fMgCache0))
    {
      // ISSUE: reference to a compiler-generated field
      if (SimDebugView.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SimDebugView.\u003C\u003Ef__mg\u0024cache0 = new System.Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
      }
      // ISSUE: reference to a compiler-generated field
      fMgCache0 = SimDebugView.\u003C\u003Ef__mg\u0024cache0;
    }
    fMgCache0(this, (Texture) texture);
    int min_x;
    int min_y;
    int max_x;
    int max_y;
    Grid.GetVisibleExtents(out min_x, out min_y, out max_x, out max_y);
    this.selectedPathProber = (PathProber) null;
    KSelectable selected = SelectTool.Instance.selected;
    if ((UnityEngine.Object) selected != (UnityEngine.Object) null)
      this.selectedPathProber = selected.GetComponent<PathProber>();
    this.updateSimViewWorkItems.Reset(new SimDebugView.UpdateSimViewSharedData(this, this.texBytes, viewMode, this));
    int num = 16;
    for (int y0 = min_y; y0 <= max_y; y0 += num)
    {
      int y1 = Math.Min(y0 + num - 1, max_y);
      this.updateSimViewWorkItems.Add(new SimDebugView.UpdateSimViewWorkItem(min_x, y0, max_x, y1));
    }
    this.currentFrame = Time.frameCount;
    this.selectedCell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    GlobalJobManager.Run((IWorkItemCollection) this.updateSimViewWorkItems);
    texture.LoadRawTextureData(textureBytes);
    texture.Apply();
  }

  public void SetGameGridMode(SimDebugView.GameGridMode mode)
  {
    this.gameGridMode = mode;
  }

  public SimDebugView.GameGridMode GetGameGridMode()
  {
    return this.gameGridMode;
  }

  public void SetMode(HashedString mode)
  {
    this.mode = mode;
    Game.Instance.gameObject.Trigger(1798162660, (object) mode);
  }

  public HashedString GetMode()
  {
    return this.mode;
  }

  public static Color TemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp((float) (((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected)), 0.0f, 1f)) * 171.0) / 360.0), 1f, 1f);
  }

  public static Color LiquidTemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    float num = (float) (((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected));
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp(num, 0.5f, 1f)) * 171.0) / 360.0), Mathf.Clamp(num, 0.0f, 1f), 1f);
  }

  public static Color SolidTemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp((float) (((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected)), 0.5f, 1f)) * 171.0) / 360.0), 1f, 1f);
  }

  public static Color GasTemperatureToColor(
    float temperature,
    float minTempExpected,
    float maxTempExpected)
  {
    return Color.HSVToRGB((float) ((10.0 + (1.0 - (double) Mathf.Clamp((float) (((double) temperature - (double) minTempExpected) / ((double) maxTempExpected - (double) minTempExpected)), 0.0f, 0.5f)) * 171.0) / 360.0), 1f, 1f);
  }

  public Color NormalizedTemperature(float temperature)
  {
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < this.temperatureThresholds.Length; ++index3)
    {
      if ((double) temperature <= (double) this.temperatureThresholds[index3].value)
      {
        index2 = index3;
        break;
      }
      index1 = index3;
      index2 = index3;
    }
    float a = 0.0f;
    if (index1 != index2)
      a = (float) (((double) temperature - (double) this.temperatureThresholds[index1].value) / ((double) this.temperatureThresholds[index2].value - (double) this.temperatureThresholds[index1].value));
    float t = Mathf.Min(Mathf.Max(a, 0.0f), 1f);
    return Color.Lerp(this.temperatureThresholds[index1].color, this.temperatureThresholds[index2].color, t);
  }

  public Color NormalizedHeatFlow(int cell)
  {
    int index1 = 0;
    int index2 = 0;
    float thermalComfort = GameUtil.GetThermalComfort(cell, -0.08368f);
    for (int index3 = 0; index3 < this.heatFlowThresholds.Length; ++index3)
    {
      if ((double) thermalComfort <= (double) this.heatFlowThresholds[index3].value)
      {
        index2 = index3;
        break;
      }
      index1 = index3;
      index2 = index3;
    }
    float a = 0.0f;
    if (index1 != index2)
      a = (float) (((double) thermalComfort - (double) this.heatFlowThresholds[index1].value) / ((double) this.heatFlowThresholds[index2].value - (double) this.heatFlowThresholds[index1].value));
    float t = Mathf.Min(Mathf.Max(a, 0.0f), 1f);
    Color color = Color.Lerp(this.heatFlowThresholds[index1].color, this.heatFlowThresholds[index2].color, t);
    if (Grid.Solid[cell])
      color = Color.black;
    return color;
  }

  private static bool IsInsulated(int cell)
  {
    return (Grid.Element[cell].state & Element.State.TemperatureInsulated) != Element.State.Vacuum;
  }

  private static Color GetDiseaseColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (Grid.DiseaseIdx[cell] != byte.MaxValue)
    {
      color = (Color) Db.Get().Diseases[(int) Grid.DiseaseIdx[cell]].overlayColour;
      color.a = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
    }
    else
      color.a = 0.0f;
    return color;
  }

  private static Color GetHeatFlowColour(SimDebugView instance, int cell)
  {
    return instance.NormalizedHeatFlow(cell);
  }

  private static Color GetBlack(SimDebugView instance, int cell)
  {
    return Color.black;
  }

  public static Color GetLightColour(SimDebugView instance, int cell)
  {
    Color color = new Color(0.8f, 0.7f, 0.3f, Mathf.Clamp(Mathf.Sqrt((float) (Grid.LightIntensity[cell] + LightGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0.0f, 1f));
    if (Grid.LightIntensity[cell] > 71999)
    {
      float num = (float) (((double) Grid.LightIntensity[cell] + (double) LightGridManager.previewLux[cell] - 71999.0) / 8001.0) / 10f;
      color.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, (float) ((double) Grid.CellToPos2D(cell).y / 8.0 + (double) instance.currentFrame / 32.0)) * num);
    }
    return color;
  }

  public static Color GetRadiationColour(SimDebugView instance, int cell)
  {
    Color color = new Color(0.2f, 0.9f, 0.3f, Mathf.Clamp(Mathf.Sqrt((float) (Grid.RadiationCount[cell] + RadiationGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0.0f, 1f));
    if (Grid.RadiationCount[cell] > 71999)
    {
      float num = (float) (((double) Grid.RadiationCount[cell] + (double) LightGridManager.previewLux[cell] - 71999.0) / 8001.0) / 10f;
      color.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, (float) ((double) Grid.CellToPos2D(cell).y / 8.0 + (double) instance.currentFrame / 32.0)) * num);
    }
    return color;
  }

  public static Color GetRoomsColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (Grid.IsValidCell(instance.selectedCell))
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
      if (cavityForCell != null && cavityForCell.room != null)
      {
        color = cavityForCell.room.roomType.category.color;
        color.a = 0.45f;
        if (Game.Instance.roomProber.GetCavityForCell(instance.selectedCell) == cavityForCell)
          color.a += 0.3f;
      }
    }
    return color;
  }

  public static Color GetJoulesColour(SimDebugView instance, int cell)
  {
    return Color.Lerp(Color.black, Color.red, (float) (0.5 * ((double) Grid.Element[cell].specificHeatCapacity * (double) Grid.Temperature[cell] * ((double) Grid.Mass[cell] * 1000.0)) / ((double) ElementLoader.FindElementByHash(SimHashes.SandStone).specificHeatCapacity * 294.0 * 1000000.0)));
  }

  public static Color GetNormalizedTemperatureColourMode(SimDebugView instance, int cell)
  {
    switch (Game.Instance.temperatureOverlayMode)
    {
      case Game.TemperatureOverlayModes.AbsoluteTemperature:
        return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
      case Game.TemperatureOverlayModes.AdaptiveTemperature:
        return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
      case Game.TemperatureOverlayModes.HeatFlow:
        return SimDebugView.GetHeatFlowColour(instance, cell);
      case Game.TemperatureOverlayModes.StateChange:
        return SimDebugView.GetStateChangeProximityColour(instance, cell);
      default:
        return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
    }
  }

  public static Color GetStateChangeProximityColour(SimDebugView instance, int cell)
  {
    float temperature = Grid.Temperature[cell];
    Element element = Grid.Element[cell];
    float lowTemp = element.lowTemp;
    float highTemp = element.highTemp;
    if (element.IsGas)
    {
      float maxTempExpected = Mathf.Min(lowTemp + 150f, highTemp);
      return SimDebugView.GasTemperatureToColor(temperature, lowTemp, maxTempExpected);
    }
    if (!element.IsSolid)
      return SimDebugView.TemperatureToColor(temperature, lowTemp, highTemp);
    float minTempExpected = Mathf.Max(highTemp - 150f, lowTemp);
    return SimDebugView.SolidTemperatureToColor(temperature, minTempExpected, highTemp);
  }

  public static Color GetNormalizedTemperatureColour(SimDebugView instance, int cell)
  {
    float temperature = Grid.Temperature[cell];
    return instance.NormalizedTemperature(temperature);
  }

  private static Color GetGameGridColour(SimDebugView instance, int cell)
  {
    Color color = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
    switch (instance.gameGridMode)
    {
      case SimDebugView.GameGridMode.GameSolidMap:
        color = !Grid.Solid[cell] ? Color.black : Color.white;
        break;
      case SimDebugView.GameGridMode.Lighting:
        color = Grid.LightCount[cell] > 0 || LightGridManager.previewLux[cell] > 0 ? Color.white : Color.black;
        break;
      case SimDebugView.GameGridMode.DigAmount:
        if (Grid.Element[cell].IsSolid)
        {
          color = Color.HSVToRGB(1f - Grid.Damage[cell] / (float) byte.MaxValue, 1f, 1f);
          break;
        }
        break;
      case SimDebugView.GameGridMode.DupePassable:
        color = !Grid.DupePassable[cell] ? Color.black : Color.white;
        break;
    }
    return color;
  }

  public Color32 GetColourForID(int id)
  {
    return this.networkColours[id % this.networkColours.Length];
  }

  private static Color GetThermalConductivityColour(SimDebugView instance, int cell)
  {
    bool flag = SimDebugView.IsInsulated(cell);
    Color color = Color.black;
    float num1 = instance.maxThermalConductivity - instance.minThermalConductivity;
    if (!flag && (double) num1 != 0.0)
    {
      float num2 = Mathf.Min(Mathf.Max((Grid.Element[cell].thermalConductivity - instance.minThermalConductivity) / num1, 0.0f), 1f);
      color = new Color(num2, num2, num2);
    }
    return color;
  }

  private static Color GetPressureMapColour(SimDebugView instance, int cell)
  {
    Color32 color32 = (Color32) Color.black;
    if ((double) Grid.Pressure[cell] > 0.0)
    {
      float num = Mathf.Clamp((float) (((double) Grid.Pressure[cell] - (double) instance.minPressureExpected) / ((double) instance.maxPressureExpected - (double) instance.minPressureExpected)), 0.0f, 1f) * 0.9f;
      color32 = (Color32) new Color(num, num, num, 1f);
    }
    return (Color) color32;
  }

  private static Color GetOxygenMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (!Grid.IsLiquid(cell) && !Grid.Solid[cell])
    {
      if ((double) Grid.Mass[cell] > (double) SimDebugView.minimumBreathable && (Grid.Element[cell].id == SimHashes.Oxygen || Grid.Element[cell].id == SimHashes.ContaminatedOxygen))
      {
        float time = Mathf.Clamp((Grid.Mass[cell] - SimDebugView.minimumBreathable) / SimDebugView.optimallyBreathable, 0.0f, 1f);
        color = instance.breathableGradient.Evaluate(time);
      }
      else
        color = (Color) instance.unbreathableColour;
    }
    return color;
  }

  private static Color GetTileColour(SimDebugView instance, int cell)
  {
    float num = 0.33f;
    Color color = new Color(num, num, num);
    Element element = Grid.Element[cell];
    bool flag = false;
    foreach (Tag tileOverlayFilter in Game.Instance.tileOverlayFilters)
    {
      if (element.HasTag(tileOverlayFilter))
        flag = true;
    }
    if (flag)
      color = (Color) element.substance.uiColour;
    return color;
  }

  private static Color GetTileTypeColour(SimDebugView instance, int cell)
  {
    return (Color) Grid.Element[cell].substance.uiColour;
  }

  private static Color GetStateMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    switch (Grid.Element[cell].state & Element.State.Solid)
    {
      case Element.State.Gas:
        color = Color.yellow;
        break;
      case Element.State.Liquid:
        color = Color.green;
        break;
      case Element.State.Solid:
        color = Color.blue;
        break;
    }
    return color;
  }

  private static Color GetSolidLiquidMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    switch (Grid.Element[cell].state & Element.State.Solid)
    {
      case Element.State.Liquid:
        color = Color.green;
        break;
      case Element.State.Solid:
        color = Color.blue;
        break;
    }
    return color;
  }

  private static Color GetStateChangeColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    Element element = Grid.Element[cell];
    if (!element.IsVacuum)
    {
      float num1 = Grid.Temperature[cell];
      float num2 = element.lowTemp * 0.05f;
      float a = Mathf.Abs(num1 - element.lowTemp) / num2;
      float num3 = element.highTemp * 0.05f;
      float b = Mathf.Abs(num1 - element.highTemp) / num3;
      color = Color.Lerp(Color.black, Color.red, Mathf.Max(0.0f, 1f - Mathf.Min(a, b)));
    }
    return color;
  }

  private static Color GetDecorColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (!Grid.Solid[cell])
    {
      float f = GameUtil.GetDecorAtCell(cell) / 100f;
      color = (double) f <= 0.0 ? Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(1f, 0.0f, 0.0f), Mathf.Abs(f)) : Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(0.0f, 1f, 0.0f), Mathf.Abs(f));
    }
    return color;
  }

  private static Color GetDangerColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    SimDebugView.DangerAmount dangerAmount = SimDebugView.DangerAmount.None;
    if (!Grid.Element[cell].IsSolid)
    {
      float num = 0.0f;
      if ((double) Grid.Temperature[cell] < (double) SimDebugView.minMinionTemperature)
        num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.minMinionTemperature);
      if ((double) Grid.Temperature[cell] > (double) SimDebugView.maxMinionTemperature)
        num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.maxMinionTemperature);
      if ((double) num > 0.0)
      {
        if ((double) num < 10.0)
          dangerAmount = SimDebugView.DangerAmount.VeryLow;
        else if ((double) num < 30.0)
          dangerAmount = SimDebugView.DangerAmount.Low;
        else if ((double) num < 100.0)
          dangerAmount = SimDebugView.DangerAmount.Moderate;
        else if ((double) num < 200.0)
          dangerAmount = SimDebugView.DangerAmount.High;
        else if ((double) num < 400.0)
          dangerAmount = SimDebugView.DangerAmount.VeryHigh;
        else if ((double) num > 800.0)
          dangerAmount = SimDebugView.DangerAmount.Extreme;
      }
    }
    if (dangerAmount < SimDebugView.DangerAmount.VeryHigh && (Grid.Element[cell].IsVacuum || Grid.Element[cell].IsGas && (Grid.Element[cell].id != SimHashes.Oxygen || (double) Grid.Pressure[cell] < (double) SimDebugView.minMinionPressure)))
      ++dangerAmount;
    if (dangerAmount != SimDebugView.DangerAmount.None)
      color = Color.HSVToRGB((float) ((80.0 - (double) ((float) dangerAmount / 6f) * 80.0) / 360.0), 1f, 1f);
    return color;
  }

  private static Color GetSimCheckErrorMapColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    Element element = Grid.Element[cell];
    float f1 = Grid.Mass[cell];
    float f2 = Grid.Temperature[cell];
    if (float.IsNaN(f1) || float.IsNaN(f2) || ((double) f1 > 10000.0 || (double) f2 > 10000.0))
      return Color.red;
    if (element.IsVacuum)
      color = (double) f2 == 0.0 ? ((double) f1 == 0.0 ? Color.gray : Color.blue) : Color.yellow;
    else if ((double) f2 < 10.0)
      color = Color.red;
    else if ((double) Grid.Mass[cell] < 1.0 && (double) Grid.Pressure[cell] < 1.0)
      color = Color.green;
    else if ((double) f2 > (double) element.highTemp + 3.0 && element.highTempTransition != null)
      color = Color.magenta;
    else if ((double) f2 < (double) element.lowTemp + 3.0 && element.lowTempTransition != null)
      color = Color.cyan;
    return color;
  }

  private static Color GetFakeFloorColour(SimDebugView instance, int cell)
  {
    if (Grid.FakeFloor[cell])
      return Color.cyan;
    return Color.black;
  }

  private static Color GetFoundationColour(SimDebugView instance, int cell)
  {
    if (Grid.Foundation[cell])
      return Color.white;
    return Color.black;
  }

  private static Color GetDupePassableColour(SimDebugView instance, int cell)
  {
    if (Grid.DupePassable[cell])
      return Color.green;
    return Color.black;
  }

  private static Color GetCritterImpassableColour(SimDebugView instance, int cell)
  {
    if (Grid.CritterImpassable[cell])
      return Color.yellow;
    return Color.black;
  }

  private static Color GetDupeImpassableColour(SimDebugView instance, int cell)
  {
    if (Grid.DupeImpassable[cell])
      return Color.red;
    return Color.black;
  }

  private static Color GetMinionOccupiedColour(SimDebugView instance, int cell)
  {
    if ((UnityEngine.Object) Grid.Objects[cell, 0] != (UnityEngine.Object) null)
      return Color.white;
    return Color.black;
  }

  private static Color GetMinionGroupProberColour(SimDebugView instance, int cell)
  {
    if (MinionGroupProber.Get().IsReachable(cell))
      return Color.white;
    return Color.black;
  }

  private static Color GetPathProberColour(SimDebugView instance, int cell)
  {
    if ((UnityEngine.Object) instance.selectedPathProber != (UnityEngine.Object) null && instance.selectedPathProber.GetCost(cell) != -1)
      return Color.white;
    return Color.black;
  }

  private static Color GetReservedColour(SimDebugView instance, int cell)
  {
    if (Grid.Reserved[cell])
      return Color.white;
    return Color.black;
  }

  private static Color GetAllowPathFindingColour(SimDebugView instance, int cell)
  {
    if (Grid.AllowPathfinding[cell])
      return Color.white;
    return Color.black;
  }

  private static Color GetMassColour(SimDebugView instance, int cell)
  {
    Color color = Color.black;
    if (!SimDebugView.IsInsulated(cell))
    {
      float num = Grid.Mass[cell];
      if ((double) num > 0.0)
        color = Color.HSVToRGB(1f - (float) (((double) num - (double) SimDebugView.Instance.minMassExpected) / ((double) SimDebugView.Instance.maxMassExpected - (double) SimDebugView.Instance.minMassExpected)), 1f, 1f);
    }
    return color;
  }

  public static class OverlayModes
  {
    public static readonly HashedString Mass = (HashedString) nameof (Mass);
    public static readonly HashedString Pressure = (HashedString) nameof (Pressure);
    public static readonly HashedString GameGrid = (HashedString) nameof (GameGrid);
    public static readonly HashedString ScenePartitioner = (HashedString) nameof (ScenePartitioner);
    public static readonly HashedString ConduitUpdates = (HashedString) nameof (ConduitUpdates);
    public static readonly HashedString Flow = (HashedString) nameof (Flow);
    public static readonly HashedString StateChange = (HashedString) nameof (StateChange);
    public static readonly HashedString SimCheckErrorMap = (HashedString) nameof (SimCheckErrorMap);
    public static readonly HashedString DupePassable = (HashedString) nameof (DupePassable);
    public static readonly HashedString Foundation = (HashedString) nameof (Foundation);
    public static readonly HashedString FakeFloor = (HashedString) nameof (FakeFloor);
    public static readonly HashedString CritterImpassable = (HashedString) nameof (CritterImpassable);
    public static readonly HashedString DupeImpassable = (HashedString) nameof (DupeImpassable);
    public static readonly HashedString MinionGroupProber = (HashedString) nameof (MinionGroupProber);
    public static readonly HashedString PathProber = (HashedString) nameof (PathProber);
    public static readonly HashedString Reserved = (HashedString) nameof (Reserved);
    public static readonly HashedString AllowPathFinding = (HashedString) nameof (AllowPathFinding);
    public static readonly HashedString Danger = (HashedString) nameof (Danger);
    public static readonly HashedString MinionOccupied = (HashedString) nameof (MinionOccupied);
    public static readonly HashedString TileType = (HashedString) nameof (TileType);
    public static readonly HashedString State = (HashedString) nameof (State);
    public static readonly HashedString SolidLiquid = (HashedString) nameof (SolidLiquid);
    public static readonly HashedString Joules = (HashedString) nameof (Joules);
  }

  public enum GameGridMode
  {
    GameSolidMap,
    Lighting,
    RoomMap,
    Style,
    PlantDensity,
    DigAmount,
    DupePassable,
  }

  [Serializable]
  public struct ColorThreshold
  {
    public Color color;
    public float value;
  }

  private struct UpdateSimViewSharedData
  {
    public SimDebugView instance;
    public HashedString simViewMode;
    public SimDebugView simDebugView;
    public byte[] textureBytes;

    public UpdateSimViewSharedData(
      SimDebugView instance,
      byte[] texture_bytes,
      HashedString sim_view_mode,
      SimDebugView sim_debug_view)
    {
      this.instance = instance;
      this.textureBytes = texture_bytes;
      this.simViewMode = sim_view_mode;
      this.simDebugView = sim_debug_view;
    }
  }

  private struct UpdateSimViewWorkItem : IWorkItem<SimDebugView.UpdateSimViewSharedData>
  {
    private int x0;
    private int y0;
    private int x1;
    private int y1;

    public UpdateSimViewWorkItem(int x0, int y0, int x1, int y1)
    {
      this.x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
      this.x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
      this.y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
      this.y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);
    }

    public void Run(SimDebugView.UpdateSimViewSharedData shared_data)
    {
      Func<SimDebugView, int, Color> fMgCache0;
      if (!shared_data.instance.getColourFuncs.TryGetValue(shared_data.simViewMode, out fMgCache0))
      {
        // ISSUE: reference to a compiler-generated field
        if (SimDebugView.UpdateSimViewWorkItem.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SimDebugView.UpdateSimViewWorkItem.\u003C\u003Ef__mg\u0024cache0 = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
        }
        // ISSUE: reference to a compiler-generated field
        fMgCache0 = SimDebugView.UpdateSimViewWorkItem.\u003C\u003Ef__mg\u0024cache0;
      }
      for (int y0 = this.y0; y0 <= this.y1; ++y0)
      {
        int cell1 = Grid.XYToCell(this.x0, y0);
        int cell2 = Grid.XYToCell(this.x1, y0);
        for (int index1 = cell1; index1 <= cell2; ++index1)
        {
          Color color = fMgCache0(shared_data.instance, index1);
          int index2 = index1 * 4;
          shared_data.textureBytes[index2] = (byte) ((double) Mathf.Min(color.r, 1f) * (double) byte.MaxValue);
          shared_data.textureBytes[index2 + 1] = (byte) ((double) Mathf.Min(color.g, 1f) * (double) byte.MaxValue);
          shared_data.textureBytes[index2 + 2] = (byte) ((double) Mathf.Min(color.b, 1f) * (double) byte.MaxValue);
          shared_data.textureBytes[index2 + 3] = (byte) ((double) Mathf.Min(color.a, 1f) * (double) byte.MaxValue);
        }
      }
    }
  }

  public enum DangerAmount
  {
    None = 0,
    VeryLow = 1,
    Low = 2,
    Moderate = 3,
    High = 4,
    VeryHigh = 5,
    Extreme = 6,
    MAX_DANGERAMOUNT = 6,
  }
}
