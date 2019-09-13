// Decompiled with JetBrains decompiler
// Type: WaterCubes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

public class WaterCubes : KMonoBehaviour
{
  public Material material;
  public Texture2D waveTexture;
  private GameObject cubes;

  public static WaterCubes Instance { get; private set; }

  public static void DestroyInstance()
  {
    WaterCubes.Instance = (WaterCubes) null;
  }

  protected override void OnPrefabInit()
  {
    WaterCubes.Instance = this;
  }

  public void Init()
  {
    this.cubes = Util.NewGameObject(this.gameObject, nameof (WaterCubes));
    GameObject gameObject = new GameObject();
    gameObject.name = "WaterCubesMesh";
    gameObject.transform.parent = this.cubes.transform;
    this.material.renderQueue = RenderQueues.Liquid;
    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    meshRenderer.sharedMaterial = this.material;
    meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
    meshRenderer.receiveShadows = false;
    meshRenderer.lightProbeUsage = LightProbeUsage.Off;
    meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    meshRenderer.sharedMaterial.SetTexture("_MainTex2", (Texture) this.waveTexture);
    meshFilter.sharedMesh = this.CreateNewMesh();
    meshRenderer.gameObject.layer = 0;
    meshRenderer.gameObject.transform.parent = this.transform;
    meshRenderer.gameObject.transform.SetPosition(new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Liquid)));
  }

  private Mesh CreateNewMesh()
  {
    Mesh mesh = new Mesh();
    mesh.name = nameof (WaterCubes);
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    Vector3[] vector3Array2 = new Vector3[length];
    Vector4[] vector4Array1 = new Vector4[length];
    int[] numArray1 = new int[6];
    float layerZ = Grid.GetLayerZ(Grid.SceneLayer.Liquid);
    Vector3[] vector3Array3 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, layerZ),
      new Vector3((float) Grid.WidthInCells, 0.0f, layerZ),
      new Vector3(0.0f, Grid.HeightInMeters, layerZ),
      new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, layerZ)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    Vector3[] vector3Array4 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, -1f)
    };
    Vector4[] vector4Array2 = new Vector4[4]
    {
      new Vector4(0.0f, 1f, 0.0f, -1f),
      new Vector4(0.0f, 1f, 0.0f, -1f),
      new Vector4(0.0f, 1f, 0.0f, -1f),
      new Vector4(0.0f, 1f, 0.0f, -1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    mesh.vertices = vector3Array3;
    mesh.uv = vector2Array2;
    mesh.uv2 = vector2Array2;
    mesh.normals = vector3Array4;
    mesh.tangents = vector4Array2;
    mesh.triangles = numArray2;
    mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0.0f));
    return mesh;
  }
}
