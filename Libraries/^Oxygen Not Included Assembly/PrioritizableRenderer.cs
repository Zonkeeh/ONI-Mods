// Decompiled with JetBrains decompiler
// Type: PrioritizableRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PrioritizableRenderer
{
  private Mesh mesh;
  private int layer;
  private Material material;
  private int prioritizableCount;
  private Vector3[] vertices;
  private Vector2[] uvs;
  private int[] triangles;
  private List<Prioritizable> prioritizables;
  private PrioritizeTool tool;

  public PrioritizableRenderer()
  {
    this.layer = LayerMask.NameToLayer("UI");
    Shader shader = Shader.Find("Klei/Prioritizable");
    Texture2D texture = Assets.GetTexture("priority_overlay_atlas");
    this.material = new Material(shader);
    this.material.SetTexture(Shader.PropertyToID("_MainTex"), (Texture) texture);
    this.prioritizables = new List<Prioritizable>();
    this.mesh = new Mesh();
    this.mesh.name = "Prioritizables";
    this.mesh.MarkDynamic();
  }

  public PrioritizeTool currentTool
  {
    get
    {
      return this.tool;
    }
    set
    {
      this.tool = value;
    }
  }

  public void Cleanup()
  {
    this.material = (Material) null;
    this.vertices = (Vector3[]) null;
    this.uvs = (Vector2[]) null;
    this.prioritizables = (List<Prioritizable>) null;
    this.triangles = (int[]) null;
    Object.DestroyImmediate((Object) this.mesh);
    this.mesh = (Mesh) null;
  }

  public void RenderEveryTick()
  {
    using (new KProfiler.Region(nameof (PrioritizableRenderer), (Object) null))
    {
      if ((Object) GameScreenManager.Instance == (Object) null || (Object) SimDebugView.Instance == (Object) null || SimDebugView.Instance.GetMode() != OverlayModes.Priorities.ID)
        return;
      this.prioritizables.Clear();
      Vector2I min;
      Vector2I max;
      Grid.GetVisibleExtents(out min, out max);
      int height = max.y - min.y;
      int width = max.x - min.x;
      Extents extents = new Extents(min.x, min.y, width, height);
      List<ScenePartitionerEntry> gathered_entries = new List<ScenePartitionerEntry>();
      GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.prioritizableObjects, gathered_entries);
      foreach (ScenePartitionerEntry partitionerEntry in gathered_entries)
      {
        Prioritizable prioritizable = (Prioritizable) partitionerEntry.obj;
        if ((Object) prioritizable != (Object) null && prioritizable.showIcon && (prioritizable.IsPrioritizable() && this.tool.IsActiveLayer(this.tool.GetFilterLayerFromGameObject(prioritizable.gameObject))))
          this.prioritizables.Add(prioritizable);
      }
      if (this.prioritizableCount != this.prioritizables.Count)
      {
        this.prioritizableCount = this.prioritizables.Count;
        this.vertices = new Vector3[4 * this.prioritizableCount];
        this.uvs = new Vector2[4 * this.prioritizableCount];
        this.triangles = new int[6 * this.prioritizableCount];
      }
      if (this.prioritizableCount == 0)
        return;
      for (int index1 = 0; index1 < this.prioritizables.Count; ++index1)
      {
        Prioritizable prioritizable = this.prioritizables[index1];
        Vector3 vector3 = Vector3.zero;
        KAnimControllerBase component = prioritizable.GetComponent<KAnimControllerBase>();
        vector3 = !((Object) component != (Object) null) ? prioritizable.transform.GetPosition() : component.GetWorldPivot();
        vector3.x += prioritizable.iconOffset.x;
        vector3.y += prioritizable.iconOffset.y;
        Vector2 vector2 = new Vector2(0.2f, 0.3f) * prioritizable.iconScale;
        float z = -5f;
        int index2 = 4 * index1;
        this.vertices[index2] = new Vector3(vector3.x - vector2.x, vector3.y - vector2.y, z);
        this.vertices[1 + index2] = new Vector3(vector3.x - vector2.x, vector3.y + vector2.y, z);
        this.vertices[2 + index2] = new Vector3(vector3.x + vector2.x, vector3.y - vector2.y, z);
        this.vertices[3 + index2] = new Vector3(vector3.x + vector2.x, vector3.y + vector2.y, z);
        float num1 = 0.1f;
        PrioritySetting masterPriority = prioritizable.GetMasterPriority();
        float num2 = -1f;
        if (masterPriority.priority_class >= PriorityScreen.PriorityClass.high)
          num2 += 9f;
        if (masterPriority.priority_class >= PriorityScreen.PriorityClass.topPriority)
          num2 = num2;
        float num3 = num2 + (float) masterPriority.priority_value;
        float x = num1 * num3;
        float y = 0.0f;
        float num4 = num1;
        float num5 = 1f;
        this.uvs[index2] = new Vector2(x, y);
        this.uvs[1 + index2] = new Vector2(x, y + num5);
        this.uvs[2 + index2] = new Vector2(x + num4, y);
        this.uvs[3 + index2] = new Vector2(x + num4, y + num5);
        int index3 = 6 * index1;
        this.triangles[index3] = index2;
        this.triangles[1 + index3] = index2 + 1;
        this.triangles[2 + index3] = index2 + 2;
        this.triangles[3 + index3] = index2 + 2;
        this.triangles[4 + index3] = index2 + 1;
        this.triangles[5 + index3] = index2 + 3;
      }
      this.mesh.Clear();
      this.mesh.vertices = this.vertices;
      this.mesh.uv = this.uvs;
      this.mesh.SetTriangles(this.triangles, 0);
      this.mesh.RecalculateBounds();
      Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, this.material, this.layer, GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera, 0, (MaterialPropertyBlock) null, false, false);
    }
  }
}
