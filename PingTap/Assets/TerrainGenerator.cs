using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
  Mesh mesh;
  Vector3[] vertices;
  int[] triangles;
  Vector2[] uvs;
  Color[] colors;

  public int xSize = 20;
  public int zSize = 20;

  public int textureWidth = 1024;
  public int textureHeight = 1024;

  public float noise01Scale = 2F;
  public float noise01Amp = 2F;

  public float noise02Scale = 4F;
  public float noise02Amp = 4F;

  public float noise03Scale = 6F;
  public float noise03Amp = 6F;

  public Gradient gradient;

  float minTerrainHeight;
  float maxTerrainHeight;

  void Start()
  {
    mesh = new Mesh();
    GetComponent<MeshFilter>().mesh = mesh;
    CreateShape();
    //UpdateMesh();
  }

  void Update()
  {
    CreateColors();
    UpdateMesh();
  }


  void CreateShape()
  {
    CreateVertices();
    CreateTriangles();
    //CreateUvs();
  }

  void CreateVertices()
  {
    vertices = new Vector3[(xSize + 1) * (zSize + 1)];
    for (int i = 0, z = 0; z <= zSize; z++)
    {
      for (int x = 0; x <= xSize; x++)
      {
        float y = GetNoiseSample(x, z);
        vertices[i] = new Vector3(x, y, z);
        i++;
      }
    }
  }

  void CreateTriangles()
  {
    int vert = 0;
    int tris = 0;

    triangles = new int[xSize * zSize * 6];

    for (int z = 0; z < zSize; z++)
    {
      for (int x = 0; x < xSize; x++)
      {
        triangles[tris] = vert + 0;
        triangles[tris + 1] = vert + xSize + 1;
        triangles[tris + 2] = vert + 1;
        triangles[tris + 3] = vert + 1;
        triangles[tris + 4] = vert + xSize + 1;
        triangles[tris + 5] = vert + xSize + 2;
        vert++;
        tris += 6;
      }
      vert++;
    }
  }

  void CreateUvs()
  {
    uvs = new Vector2[vertices.Length];

    for (int i = 0, z = 0; z < zSize; z++)
    {
      for (int x = 0; x < xSize; x++)
      {
        uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
        i++;
      }
    }
  }

  void CreateColors()
  {
    colors = new Color[vertices.Length];
    minTerrainHeight = vertices.OrderBy(v => v.y).First().y;
    maxTerrainHeight = vertices.OrderByDescending(v => v.y).First().y;

    for (int i = 0, z = 0; z < zSize; z++)
    {
      for (int x = 0; x < xSize; x++)
      {
        float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
        colors[i] = gradient.Evaluate(height);
        i++;
      }
    }
  }

  void UpdateMesh()
  {
    mesh.Clear();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;
    mesh.colors = colors;
    mesh.RecalculateNormals();
  }

  float GetNoiseSample(int x, int z)
  {
    float noiseHeight = 0;

    float sampleX = x / noise01Scale;
    float sampleZ = z / noise01Scale;
    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
    //noiseHeight += perlinValue * noise01Amp;

    return perlinValue;
  }

  void OnDrawGizmos()
  {
    if (vertices == null) return;
    foreach (var position in vertices)
    {
      Gizmos.DrawSphere(position, .1f);
    }
  }
}
