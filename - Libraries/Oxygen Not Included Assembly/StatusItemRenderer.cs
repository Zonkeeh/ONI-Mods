// Decompiled with JetBrains decompiler
// Type: StatusItemRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class StatusItemRenderer
{
  private Dictionary<int, int> handleTable = new Dictionary<int, int>();
  public List<StatusItemRenderer.Entry> visibleEntries = new List<StatusItemRenderer.Entry>();
  private StatusItemRenderer.Entry[] entries;
  private int entryCount;
  private Shader shader;

  public StatusItemRenderer()
  {
    this.layer = LayerMask.NameToLayer("UI");
    this.entries = new StatusItemRenderer.Entry[100];
    this.shader = Shader.Find("Klei/StatusItem");
    for (int index = 0; index < this.entries.Length; ++index)
    {
      StatusItemRenderer.Entry entry = new StatusItemRenderer.Entry();
      entry.Init(this.shader);
      this.entries[index] = entry;
    }
    this.backgroundColor = new Color32((byte) 244, (byte) 74, (byte) 71, byte.MaxValue);
    this.selectedColor = new Color32((byte) 225, (byte) 181, (byte) 180, byte.MaxValue);
    this.neutralColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    this.arrowSprite = Assets.GetSprite((HashedString) "StatusBubbleTop");
    this.backgroundSprite = Assets.GetSprite((HashedString) "StatusBubble");
    this.scale = 1f;
    Game.Instance.Subscribe(2095258329, new System.Action<object>(this.OnHighlightObject));
  }

  public int layer { get; private set; }

  public int selectedHandle { get; private set; }

  public int highlightHandle { get; private set; }

  public Color32 backgroundColor { get; private set; }

  public Color32 selectedColor { get; private set; }

  public Color32 neutralColor { get; private set; }

  public Sprite arrowSprite { get; private set; }

  public Sprite backgroundSprite { get; private set; }

  public float scale { get; private set; }

  public int GetIdx(Transform transform)
  {
    int instanceId = transform.GetInstanceID();
    int index = 0;
    if (!this.handleTable.TryGetValue(instanceId, out index))
    {
      index = this.entryCount++;
      this.handleTable[instanceId] = index;
      StatusItemRenderer.Entry entry = this.entries[index];
      entry.handle = instanceId;
      entry.transform = transform;
      this.entries[index] = entry;
    }
    return index;
  }

  public void Add(Transform transform, StatusItem status_item)
  {
    if (this.entryCount == this.entries.Length)
    {
      StatusItemRenderer.Entry[] entryArray = new StatusItemRenderer.Entry[this.entries.Length * 2];
      for (int index = 0; index < this.entries.Length; ++index)
        entryArray[index] = this.entries[index];
      for (int length = this.entries.Length; length < entryArray.Length; ++length)
        entryArray[length].Init(this.shader);
      this.entries = entryArray;
    }
    int idx = this.GetIdx(transform);
    StatusItemRenderer.Entry entry = this.entries[idx];
    entry.isBuilding = (UnityEngine.Object) transform.GetComponent<Building>() != (UnityEngine.Object) null;
    entry.Add(status_item);
    this.entries[idx] = entry;
  }

  public void Remove(Transform transform, StatusItem status_item)
  {
    int instanceId = transform.GetInstanceID();
    int idx = 0;
    if (!this.handleTable.TryGetValue(instanceId, out idx))
      return;
    StatusItemRenderer.Entry entry = this.entries[idx];
    if (entry.statusItems.Count == 0)
      return;
    entry.Remove(status_item);
    this.entries[idx] = entry;
    if (entry.statusItems.Count != 0)
      return;
    this.ClearIdx(idx);
  }

  private void ClearIdx(int idx)
  {
    StatusItemRenderer.Entry entry = this.entries[idx];
    this.handleTable.Remove(entry.handle);
    if (idx != this.entryCount - 1)
    {
      entry.Replace(this.entries[this.entryCount - 1]);
      this.entries[idx] = entry;
      this.handleTable[entry.handle] = idx;
    }
    entry = this.entries[this.entryCount - 1];
    entry.Clear();
    this.entries[this.entryCount - 1] = entry;
    --this.entryCount;
  }

  private HashedString GetMode()
  {
    if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null)
      return OverlayScreen.Instance.mode;
    return OverlayModes.None.ID;
  }

  public void MarkAllDirty()
  {
    for (int index = 0; index < this.entryCount; ++index)
      this.entries[index].MarkDirty();
  }

  public void RenderEveryTick()
  {
    this.scale = (float) (1.0 + (double) Mathf.Sin(Time.unscaledTime * 8f) * 0.100000001490116);
    Shader.SetGlobalVector("_StatusItemParameters", new Vector4(this.scale, 0.0f, 0.0f, 0.0f));
    Vector3 worldPoint1 = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
    Vector3 worldPoint2 = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, Camera.main.transform.GetPosition().z));
    this.visibleEntries.Clear();
    for (int index = 0; index < this.entryCount; ++index)
      this.entries[index].Render(this, worldPoint2, worldPoint1, this.GetMode());
  }

  public void GetIntersections(Vector2 pos, List<InterfaceTool.Intersection> intersections)
  {
    foreach (StatusItemRenderer.Entry visibleEntry in this.visibleEntries)
      visibleEntry.GetIntersection(pos, intersections, this.scale);
  }

  public void GetIntersections(Vector2 pos, List<KSelectable> selectables)
  {
    foreach (StatusItemRenderer.Entry visibleEntry in this.visibleEntries)
      visibleEntry.GetIntersection(pos, selectables, this.scale);
  }

  public void SetOffset(Transform transform, Vector3 offset)
  {
    int index = 0;
    if (!this.handleTable.TryGetValue(transform.GetInstanceID(), out index))
      return;
    this.entries[index].offset = offset;
  }

  private void OnSelectObject(object data)
  {
    int index = 0;
    if (this.handleTable.TryGetValue(this.selectedHandle, out index))
      this.entries[index].MarkDirty();
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      this.selectedHandle = gameObject.transform.GetInstanceID();
      if (!this.handleTable.TryGetValue(this.selectedHandle, out index))
        return;
      this.entries[index].MarkDirty();
    }
    else
      this.highlightHandle = -1;
  }

  private void OnHighlightObject(object data)
  {
    int index = 0;
    if (this.handleTable.TryGetValue(this.highlightHandle, out index))
    {
      StatusItemRenderer.Entry entry = this.entries[index];
      entry.MarkDirty();
      this.entries[index] = entry;
    }
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      this.highlightHandle = gameObject.transform.GetInstanceID();
      if (!this.handleTable.TryGetValue(this.highlightHandle, out index))
        return;
      StatusItemRenderer.Entry entry = this.entries[index];
      entry.MarkDirty();
      this.entries[index] = entry;
    }
    else
      this.highlightHandle = -1;
  }

  public void Destroy()
  {
    Game.Instance.Unsubscribe(-1503271301, new System.Action<object>(this.OnSelectObject));
    Game.Instance.Unsubscribe(-1201923725, new System.Action<object>(this.OnHighlightObject));
    foreach (StatusItemRenderer.Entry entry in this.entries)
    {
      entry.Clear();
      entry.FreeResources();
    }
  }

  public struct Entry
  {
    public int handle;
    public Transform transform;
    public List<StatusItem> statusItems;
    public Mesh mesh;
    public bool dirty;
    public int layer;
    public Material material;
    public Vector3 offset;
    public bool hasVisibleStatusItems;
    public bool isBuilding;

    public void Init(Shader shader)
    {
      this.statusItems = new List<StatusItem>();
      this.mesh = new Mesh();
      this.mesh.name = nameof (StatusItemRenderer);
      this.dirty = true;
      this.material = new Material(shader);
    }

    public void Render(
      StatusItemRenderer renderer,
      Vector3 camera_bl,
      Vector3 camera_tr,
      HashedString overlay)
    {
      if (DebugHandler.HideUI)
        return;
      Vector3 zero = Vector3.zero;
      if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null)
      {
        Vector3 position = this.transform.GetPosition();
        if (this.isBuilding)
        {
          Building component = this.transform.GetComponent<Building>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            position.x += (float) ((component.Def.WidthInCells - 1) % 2) / 2f;
        }
        if ((double) position.x < (double) camera_bl.x || (double) position.x > (double) camera_tr.x || ((double) position.y < (double) camera_bl.y || (double) position.y > (double) camera_tr.y))
          return;
        int cell = Grid.PosToCell(position);
        if (Grid.IsValidCell(cell) && !Grid.IsVisible(cell) || !this.transform.GetComponent<KSelectable>().IsSelectable)
          return;
        renderer.visibleEntries.Add(this);
        if (this.dirty)
        {
          int num1 = 0;
          foreach (StatusItem statusItem in this.statusItems)
          {
            if (statusItem.UseConditionalCallback(overlay, this.transform) || !(overlay != OverlayModes.None.ID) || !(statusItem.render_overlay != overlay))
              ++num1;
          }
          this.hasVisibleStatusItems = num1 != 0;
          StatusItemRenderer.Entry.MeshBuilder meshBuilder = new StatusItemRenderer.Entry.MeshBuilder(num1 + 6, this.material);
          float num2 = 0.25f;
          float z = -5f;
          Vector2 vector2 = new Vector2(0.05f, -0.05f);
          float num3 = 0.02f;
          Color32 color32_1 = new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
          Color32 color32_2 = new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 75);
          Color32 color32_3 = renderer.neutralColor;
          if (renderer.selectedHandle == this.handle || renderer.highlightHandle == this.handle)
          {
            color32_3 = renderer.selectedColor;
          }
          else
          {
            for (int index = 0; index < this.statusItems.Count; ++index)
            {
              if (this.statusItems[index].notificationType != NotificationType.Neutral)
              {
                color32_3 = renderer.backgroundColor;
                break;
              }
            }
          }
          meshBuilder.AddQuad(new Vector2(0.0f, 0.29f) + vector2, new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, (Color) color32_2);
          meshBuilder.AddQuad(new Vector2(0.0f, 0.0f) + vector2, new Vector2(num2 * (float) num1, num2), z, renderer.backgroundSprite, (Color) color32_2);
          meshBuilder.AddQuad(new Vector2(0.0f, 0.0f), new Vector2(num2 * (float) num1 + num3, num2 + num3), z, renderer.backgroundSprite, (Color) color32_1);
          meshBuilder.AddQuad(new Vector2(0.0f, 0.0f), new Vector2(num2 * (float) num1, num2), z, renderer.backgroundSprite, (Color) color32_3);
          int num4 = 0;
          for (int index = 0; index < this.statusItems.Count; ++index)
          {
            StatusItem statusItem = this.statusItems[index];
            if (statusItem.UseConditionalCallback(overlay, this.transform) || !(overlay != OverlayModes.None.ID) || !(statusItem.render_overlay != overlay))
            {
              float x = (float) ((double) num4 * (double) num2 * 2.0 - (double) num2 * (double) (num1 - 1));
              Sprite sprite = this.statusItems[index].sprite.sprite;
              meshBuilder.AddQuad(new Vector2(x, 0.0f), new Vector2(num2, num2), z, sprite, (Color) color32_1);
              ++num4;
            }
          }
          meshBuilder.AddQuad(new Vector2(0.0f, 0.29f + num3), new Vector2(0.05f + num3, 0.05f + num3), z, renderer.arrowSprite, (Color) color32_1);
          meshBuilder.AddQuad(new Vector2(0.0f, 0.29f), new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, (Color) color32_3);
          meshBuilder.End(this.mesh);
          this.dirty = false;
        }
        if (!this.hasVisibleStatusItems || !((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null))
          return;
        Graphics.DrawMesh(this.mesh, position + this.offset, Quaternion.identity, this.material, renderer.layer, GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera, 0, (MaterialPropertyBlock) null, false, false);
      }
      else
      {
        string str = "Error cleaning up status items:";
        foreach (StatusItem statusItem in this.statusItems)
          str += statusItem.Id;
        Debug.LogWarning((object) str);
      }
    }

    public void Add(StatusItem status_item)
    {
      this.statusItems.Add(status_item);
      this.dirty = true;
    }

    public void Remove(StatusItem status_item)
    {
      this.statusItems.Remove(status_item);
      this.dirty = true;
    }

    public void Replace(StatusItemRenderer.Entry entry)
    {
      this.handle = entry.handle;
      this.transform = entry.transform;
      this.offset = entry.offset;
      this.dirty = true;
      this.statusItems.Clear();
      this.statusItems.AddRange((IEnumerable<StatusItem>) entry.statusItems);
    }

    private bool Intersects(Vector2 pos, float scale)
    {
      if ((UnityEngine.Object) this.transform == (UnityEngine.Object) null)
        return false;
      Bounds bounds = this.mesh.bounds;
      Vector3 vector3 = this.transform.GetPosition() + this.offset + bounds.center;
      Vector2 vector2_1 = new Vector2(vector3.x, vector3.y);
      Vector3 size = bounds.size;
      Vector2 vector2_2 = new Vector2((float) ((double) size.x * (double) scale * 0.5), (float) ((double) size.y * (double) scale * 0.5));
      Vector2 vector2_3 = vector2_1 - vector2_2;
      Vector2 vector2_4 = vector2_1 + vector2_2;
      if ((double) pos.x >= (double) vector2_3.x && (double) pos.x <= (double) vector2_4.x && (double) pos.y >= (double) vector2_3.y)
        return (double) pos.y <= (double) vector2_4.y;
      return false;
    }

    public void GetIntersection(
      Vector2 pos,
      List<InterfaceTool.Intersection> intersections,
      float scale)
    {
      if (!this.Intersects(pos, scale) || !this.transform.GetComponent<KSelectable>().IsSelectable)
        return;
      intersections.Add(new InterfaceTool.Intersection()
      {
        component = (MonoBehaviour) this.transform.GetComponent<KSelectable>(),
        distance = -100f
      });
    }

    public void GetIntersection(Vector2 pos, List<KSelectable> selectables, float scale)
    {
      if (!this.Intersects(pos, scale))
        return;
      KSelectable component = this.transform.GetComponent<KSelectable>();
      if (!component.IsSelectable || selectables.Contains(component))
        return;
      selectables.Add(component);
    }

    public void Clear()
    {
      this.statusItems.Clear();
      this.offset = Vector3.zero;
      this.dirty = false;
    }

    public void FreeResources()
    {
      if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mesh);
        this.mesh = (Mesh) null;
      }
      if (!((UnityEngine.Object) this.material != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.material);
    }

    public void MarkDirty()
    {
      this.dirty = true;
    }

    private struct MeshBuilder
    {
      private static int[] textureIds = new int[11]
      {
        Shader.PropertyToID("_Tex0"),
        Shader.PropertyToID("_Tex1"),
        Shader.PropertyToID("_Tex2"),
        Shader.PropertyToID("_Tex3"),
        Shader.PropertyToID("_Tex4"),
        Shader.PropertyToID("_Tex5"),
        Shader.PropertyToID("_Tex6"),
        Shader.PropertyToID("_Tex7"),
        Shader.PropertyToID("_Tex8"),
        Shader.PropertyToID("_Tex9"),
        Shader.PropertyToID("_Tex10")
      };
      private Vector3[] vertices;
      private Vector2[] uvs;
      private Vector2[] uv2s;
      private int[] triangles;
      private Color32[] colors;
      private int quadIdx;
      private Material material;

      public MeshBuilder(int quad_count, Material material)
      {
        this.vertices = new Vector3[4 * quad_count];
        this.uvs = new Vector2[4 * quad_count];
        this.uv2s = new Vector2[4 * quad_count];
        this.colors = new Color32[4 * quad_count];
        this.triangles = new int[6 * quad_count];
        this.material = material;
        this.quadIdx = 0;
      }

      public void AddQuad(Vector2 center, Vector2 half_size, float z, Sprite sprite, Color color)
      {
        if (this.quadIdx == StatusItemRenderer.Entry.MeshBuilder.textureIds.Length)
          return;
        Rect rect = sprite.rect;
        Rect textureRect = sprite.textureRect;
        float num1 = textureRect.width / rect.width;
        float num2 = textureRect.height / rect.height;
        int index1 = 4 * this.quadIdx;
        this.vertices[index1] = new Vector3((center.x - half_size.x) * num1, (center.y - half_size.y) * num2, z);
        this.vertices[1 + index1] = new Vector3((center.x - half_size.x) * num1, (center.y + half_size.y) * num2, z);
        this.vertices[2 + index1] = new Vector3((center.x + half_size.x) * num1, (center.y - half_size.y) * num2, z);
        this.vertices[3 + index1] = new Vector3((center.x + half_size.x) * num1, (center.y + half_size.y) * num2, z);
        float x1 = textureRect.x / (float) sprite.texture.width;
        float y = textureRect.y / (float) sprite.texture.height;
        float num3 = textureRect.width / (float) sprite.texture.width;
        float num4 = textureRect.height / (float) sprite.texture.height;
        this.uvs[index1] = new Vector2(x1, y);
        this.uvs[1 + index1] = new Vector2(x1, y + num4);
        this.uvs[2 + index1] = new Vector2(x1 + num3, y);
        this.uvs[3 + index1] = new Vector2(x1 + num3, y + num4);
        this.colors[index1] = (Color32) color;
        this.colors[1 + index1] = (Color32) color;
        this.colors[2 + index1] = (Color32) color;
        this.colors[3 + index1] = (Color32) color;
        float x2 = (float) this.quadIdx + 0.5f;
        this.uv2s[index1] = new Vector2(x2, 0.0f);
        this.uv2s[1 + index1] = new Vector2(x2, 0.0f);
        this.uv2s[2 + index1] = new Vector2(x2, 0.0f);
        this.uv2s[3 + index1] = new Vector2(x2, 0.0f);
        int index2 = 6 * this.quadIdx;
        this.triangles[index2] = index1;
        this.triangles[1 + index2] = index1 + 1;
        this.triangles[2 + index2] = index1 + 2;
        this.triangles[3 + index2] = index1 + 2;
        this.triangles[4 + index2] = index1 + 1;
        this.triangles[5 + index2] = index1 + 3;
        this.material.SetTexture(StatusItemRenderer.Entry.MeshBuilder.textureIds[this.quadIdx], (Texture) sprite.texture);
        ++this.quadIdx;
      }

      public void End(Mesh mesh)
      {
        mesh.Clear();
        mesh.vertices = this.vertices;
        mesh.uv = this.uvs;
        mesh.uv2 = this.uv2s;
        mesh.colors32 = this.colors;
        mesh.SetTriangles(this.triangles, 0);
        mesh.RecalculateBounds();
      }
    }
  }
}
