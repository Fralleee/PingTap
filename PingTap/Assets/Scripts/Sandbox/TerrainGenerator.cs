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

  //public int textureWidth = 1024;
  //public int textureHeight = 1024;

  public float xScale = 1f;
  public float zScale = 1f;

  public Gradient gradient;

  float minTerrainHeight;
  float maxTerrainHeight;

  void Start()
  {
    mesh = new Mesh();
    GetComponent<MeshFilter>().mesh = mesh;
    CreateShape();
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
    float zMiddle = -zSize / 2;
    float xMiddle = -xSize / 2;
    for (int i = 0, z = 0; z <= zSize; z++)
    {
      float zVal = 1 - Mathf.Abs((z - zMiddle) / zMiddle);
      for (var x = 0; x <= xSize; x++)
      {
        float xVal = 1 - Mathf.Abs((x - xMiddle) / xMiddle);
        float modifier = (xVal * zVal) * 1.5f;
        float y = GetNoiseSample(x, z);
        y = Mathf.Clamp((y + modifier * 2) * modifier, 0, 1);
        vertices[i] = new Vector3(x, y, z);
        i++;
      }
    }
  }

  void CreateTriangles()
  {
    var vert = 0;
    var tris = 0;

    triangles = new int[xSize * zSize * 6];

    for (var z = 0; z < zSize; z++)
    {
      for (var x = 0; x < xSize; x++)
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
      for (var x = 0; x < xSize; x++)
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
      for (var x = 0; x < xSize; x++)
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
    float sampleX = x * xScale;
    float sampleZ = z * zScale;
    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ);
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