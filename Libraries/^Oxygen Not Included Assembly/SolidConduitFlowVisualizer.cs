// Decompiled with JetBrains decompiler
// Type: SolidConduitFlowVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SolidConduitFlowVisualizer
{
  private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);
  private static int BLOB_SOUND_COUNT = 7;
  private HashSet<int> insulatedCells = new HashSet<int>();
  private int highlightedCell = -1;
  private Color32 highlightColour = (Color32) new Color(0.2f, 0.2f, 0.2f, 0.2f);
  private SolidConduitFlow flowManager;
  private string overlaySound;
  private bool showContents;
  private double animTime;
  private int layer;
  private List<SolidConduitFlowVisualizer.AudioInfo> audioInfo;
  private Game.ConduitVisInfo visInfo;
  private SolidConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;
  private SolidConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;
  private SolidConduitFlowVisualizer.Tuning tuning;

  public SolidConduitFlowVisualizer(
    SolidConduitFlow flow_manager,
    Game.ConduitVisInfo vis_info,
    string overlay_sound,
    SolidConduitFlowVisualizer.Tuning tuning)
  {
    this.flowManager = flow_manager;
    this.visInfo = vis_info;
    this.overlaySound = overlay_sound;
    this.tuning = tuning;
    this.movingBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
    this.staticBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
  }

  public void FreeResources()
  {
    this.movingBallMesh.Cleanup();
    this.staticBallMesh.Cleanup();
  }

  private float CalculateMassScale(float mass)
  {
    return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, (float) (((double) mass - (double) this.visInfo.overlayMassScaleRange.x) / ((double) this.visInfo.overlayMassScaleRange.y - (double) this.visInfo.overlayMassScaleRange.x)));
  }

  private Color32 GetContentsColor(Element element, Color32 default_color)
  {
    if (element == null)
      return default_color;
    Color conduitColour = (Color) element.substance.conduitColour;
    conduitColour.a = 128f;
    return (Color32) conduitColour;
  }

  private Color32 GetBackgroundColor(float insulation_lerp)
  {
    if (this.showContents)
      return Color32.Lerp(this.visInfo.overlayTint, this.visInfo.overlayInsulatedTint, insulation_lerp);
    return Color32.Lerp(this.visInfo.tint, this.visInfo.insulatedTint, insulation_lerp);
  }

  public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
  {
    GridArea visibleArea = GridVisibleArea.GetVisibleArea();
    Vector2I vector2I1 = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
    Vector2I vector2I2 = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
    this.animTime += (double) Time.deltaTime;
    if (trigger_audio)
    {
      if (this.audioInfo == null)
        this.audioInfo = new List<SolidConduitFlowVisualizer.AudioInfo>();
      for (int index = 0; index < this.audioInfo.Count; ++index)
      {
        SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[index];
        audioInfo.distance = float.PositiveInfinity;
        audioInfo.position = Vector3.zero;
        audioInfo.blobCount = (audioInfo.blobCount + 1) % SolidConduitFlowVisualizer.BLOB_SOUND_COUNT;
        this.audioInfo[index] = audioInfo;
      }
    }
    Vector3 position = CameraController.Instance.transform.GetPosition();
    Element element = (Element) null;
    if (this.tuning.renderMesh)
    {
      float z1 = 0.0f;
      if (this.showContents)
        z1 = 1f;
      float w = (float) ((int) (this.animTime / (1.0 / (double) this.tuning.framesPerSecond)) % (int) this.tuning.spriteCount) * (1f / this.tuning.spriteCount);
      this.movingBallMesh.Begin();
      this.movingBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
      this.movingBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
      this.movingBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z1, w));
      this.movingBallMesh.SetVector("_Highlight", new Vector4((float) this.highlightColour.r / (float) byte.MaxValue, (float) this.highlightColour.g / (float) byte.MaxValue, (float) this.highlightColour.b / (float) byte.MaxValue, 0.0f));
      this.staticBallMesh.Begin();
      this.staticBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
      this.staticBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
      this.staticBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z1, 0.0f));
      this.staticBallMesh.SetVector("_Highlight", new Vector4((float) this.highlightColour.r / (float) byte.MaxValue, (float) this.highlightColour.g / (float) byte.MaxValue, (float) this.highlightColour.b / (float) byte.MaxValue, 0.0f));
      for (int idx = 0; idx < this.flowManager.GetSOAInfo().NumEntries; ++idx)
      {
        Vector2I xy1 = Grid.CellToXY(this.flowManager.GetSOAInfo().GetCell(idx));
        if (!(xy1 < vector2I1) && !(xy1 > vector2I2))
        {
          SolidConduitFlow.Conduit conduit = this.flowManager.GetSOAInfo().GetConduit(idx);
          SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this.flowManager);
          SolidConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(this.flowManager);
          bool flag = lastFlowInfo.direction != SolidConduitFlow.FlowDirection.None;
          if (flag)
          {
            int cell = conduit.GetCell(this.flowManager);
            int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
            Vector2I xy2 = Grid.CellToXY(cell);
            Vector2I xy3 = Grid.CellToXY(cellFromDirection);
            Vector2 pos = (Vector2) xy2;
            if (cell != -1)
              pos = Vector2.Lerp(new Vector2((float) xy2.x, (float) xy2.y), new Vector2((float) xy3.x, (float) xy3.y), lerp_percent);
            Color backgroundColor = (Color) this.GetBackgroundColor(Mathf.Lerp(!this.insulatedCells.Contains(cell) ? 0.0f : 1f, !this.insulatedCells.Contains(cellFromDirection) ? 0.0f : 1f, lerp_percent));
            Vector2I uvbl = new Vector2I(0, 0);
            Vector2I uvtl = new Vector2I(0, 1);
            Vector2I uvbr = new Vector2I(1, 0);
            Vector2I uvtr = new Vector2I(1, 1);
            float highlight = 0.0f;
            if (this.showContents)
            {
              if (flag != initialContents.pickupableHandle.IsValid())
                this.movingBallMesh.AddQuad(pos, (Color32) backgroundColor, this.tuning.size, 0.0f, 0.0f, uvbl, uvtl, uvbr, uvtr);
            }
            else
            {
              element = (Element) null;
              if (Grid.PosToCell(new Vector3(pos.x + SolidConduitFlowVisualizer.GRID_OFFSET.x, pos.y + SolidConduitFlowVisualizer.GRID_OFFSET.y, 0.0f)) == this.highlightedCell)
                highlight = 1f;
            }
            Color32 contentsColor = this.GetContentsColor(element, (Color32) backgroundColor);
            float num = 1f;
            this.movingBallMesh.AddQuad(pos, contentsColor, this.tuning.size * num, 1f, highlight, uvbl, uvtl, uvbr, uvtr);
            if (trigger_audio)
              this.AddAudioSource(conduit, position);
          }
          if (initialContents.pickupableHandle.IsValid() && !flag)
          {
            int cell = conduit.GetCell(this.flowManager);
            Vector2 xy2 = (Vector2) Grid.CellToXY(cell);
            float insulation_lerp = !this.insulatedCells.Contains(cell) ? 0.0f : 1f;
            Vector2I uvbl = new Vector2I(0, 0);
            Vector2I uvtl = new Vector2I(0, 1);
            Vector2I uvbr = new Vector2I(1, 0);
            Vector2I uvtr = new Vector2I(1, 1);
            float highlight = 0.0f;
            Color backgroundColor = (Color) this.GetBackgroundColor(insulation_lerp);
            float num = 1f;
            if (this.showContents)
            {
              this.staticBallMesh.AddQuad(xy2, (Color32) backgroundColor, this.tuning.size * num, 0.0f, 0.0f, uvbl, uvtl, uvbr, uvtr);
            }
            else
            {
              element = (Element) null;
              if (cell == this.highlightedCell)
                highlight = 1f;
            }
            Color32 contentsColor = this.GetContentsColor(element, (Color32) backgroundColor);
            this.staticBallMesh.AddQuad(xy2, contentsColor, this.tuning.size * num, 1f, highlight, uvbl, uvtl, uvbr, uvtr);
          }
        }
      }
      this.movingBallMesh.End(z, this.layer);
      this.staticBallMesh.End(z, this.layer);
    }
    if (!trigger_audio)
      return;
    this.TriggerAudio();
  }

  public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
  {
    this.showContents = show_contents;
    this.layer = !show_contents || !move_to_overlay_layer ? 0 : LayerMask.NameToLayer("MaskedOverlay");
  }

  private void AddAudioSource(SolidConduitFlow.Conduit conduit, Vector3 camera_pos)
  {
    using (new KProfiler.Region(nameof (AddAudioSource), (UnityEngine.Object) null))
    {
      UtilityNetwork network = this.flowManager.GetNetwork(conduit);
      if (network == null)
        return;
      Vector3 posCcc = Grid.CellToPosCCC(conduit.GetCell(this.flowManager), Grid.SceneLayer.Building);
      float num = Vector3.SqrMagnitude(posCcc - camera_pos);
      bool flag = false;
      for (int index = 0; index < this.audioInfo.Count; ++index)
      {
        SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[index];
        if (audioInfo.networkID == network.id)
        {
          if ((double) num < (double) audioInfo.distance)
          {
            audioInfo.distance = num;
            audioInfo.position = posCcc;
            this.audioInfo[index] = audioInfo;
          }
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      this.audioInfo.Add(new SolidConduitFlowVisualizer.AudioInfo()
      {
        networkID = network.id,
        position = posCcc,
        distance = num,
        blobCount = 0
      });
    }
  }

  private void TriggerAudio()
  {
    if (SpeedControlScreen.Instance.IsPaused)
      return;
    CameraController instance1 = CameraController.Instance;
    int num1 = 0;
    List<SolidConduitFlowVisualizer.AudioInfo> audioInfoList = new List<SolidConduitFlowVisualizer.AudioInfo>();
    for (int index = 0; index < this.audioInfo.Count; ++index)
    {
      if (instance1.IsVisiblePos(this.audioInfo[index].position))
      {
        audioInfoList.Add(this.audioInfo[index]);
        ++num1;
      }
    }
    for (int index = 0; index < audioInfoList.Count; ++index)
    {
      SolidConduitFlowVisualizer.AudioInfo audioInfo = audioInfoList[index];
      if ((double) audioInfo.distance != double.PositiveInfinity)
      {
        EventInstance instance2 = SoundEvent.BeginOneShot(this.overlaySound, audioInfo.position);
        int num2 = (int) instance2.setParameterValue("blobCount", (float) audioInfo.blobCount);
        int num3 = (int) instance2.setParameterValue("networkCount", (float) num1);
        SoundEvent.EndOneShot(instance2);
      }
    }
  }

  public void SetInsulated(int cell, bool insulated)
  {
    if (insulated)
      this.insulatedCells.Add(cell);
    else
      this.insulatedCells.Remove(cell);
  }

  public void SetHighlightedCell(int cell)
  {
    this.highlightedCell = cell;
  }

  [Serializable]
  public class Tuning
  {
    public bool renderMesh;
    public float size;
    public float spriteCount;
    public float framesPerSecond;
    public Texture2D backgroundTexture;
    public Texture2D foregroundTexture;
  }

  private class ConduitFlowMesh
  {
    private List<Vector3> positions = new List<Vector3>();
    private List<Vector4> uvs = new List<Vector4>();
    private List<int> triangles = new List<int>();
    private List<Color32> colors = new List<Color32>();
    private Mesh mesh;
    private Material material;
    private int quadIndex;

    public ConduitFlowMesh()
    {
      this.mesh = new Mesh();
      this.mesh.name = "ConduitMesh";
      this.material = new Material(Shader.Find("Klei/ConduitBall"));
    }

    public void AddQuad(
      Vector2 pos,
      Color32 color,
      float size,
      float is_foreground,
      float highlight,
      Vector2I uvbl,
      Vector2I uvtl,
      Vector2I uvbr,
      Vector2I uvtr)
    {
      float num = size * 0.5f;
      this.positions.Add(new Vector3(pos.x - num, pos.y - num, 0.0f));
      this.positions.Add(new Vector3(pos.x - num, pos.y + num, 0.0f));
      this.positions.Add(new Vector3(pos.x + num, pos.y - num, 0.0f));
      this.positions.Add(new Vector3(pos.x + num, pos.y + num, 0.0f));
      this.uvs.Add(new Vector4((float) uvbl.x, (float) uvbl.y, is_foreground, highlight));
      this.uvs.Add(new Vector4((float) uvtl.x, (float) uvtl.y, is_foreground, highlight));
      this.uvs.Add(new Vector4((float) uvbr.x, (float) uvbr.y, is_foreground, highlight));
      this.uvs.Add(new Vector4((float) uvtr.x, (float) uvtr.y, is_foreground, highlight));
      this.colors.Add(color);
      this.colors.Add(color);
      this.colors.Add(color);
      this.colors.Add(color);
      this.triangles.Add(this.quadIndex * 4);
      this.triangles.Add(this.quadIndex * 4 + 1);
      this.triangles.Add(this.quadIndex * 4 + 2);
      this.triangles.Add(this.quadIndex * 4 + 2);
      this.triangles.Add(this.quadIndex * 4 + 1);
      this.triangles.Add(this.quadIndex * 4 + 3);
      ++this.quadIndex;
    }

    public void SetTexture(string id, Texture2D texture)
    {
      this.material.SetTexture(id, (Texture) texture);
    }

    public void SetVector(string id, Vector4 data)
    {
      this.material.SetVector(id, data);
    }

    public void Begin()
    {
      this.positions.Clear();
      this.uvs.Clear();
      this.triangles.Clear();
      this.colors.Clear();
      this.quadIndex = 0;
    }

    public void End(float z, int layer)
    {
      this.mesh.Clear();
      this.mesh.SetVertices(this.positions);
      this.mesh.SetUVs(0, this.uvs);
      this.mesh.SetColors(this.colors);
      this.mesh.SetTriangles(this.triangles, 0, false);
      Graphics.DrawMesh(this.mesh, new Vector3(SolidConduitFlowVisualizer.GRID_OFFSET.x, SolidConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
    }

    public void Cleanup()
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.mesh);
      this.mesh = (Mesh) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.material);
      this.material = (Material) null;
    }
  }

  private struct AudioInfo
  {
    public int networkID;
    public int blobCount;
    public float distance;
    public Vector3 position;
  }
}
