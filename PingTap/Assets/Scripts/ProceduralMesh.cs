using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class ProceduralMesh : MonoBehaviour
{
  [SerializeField] int width = 20;
  [SerializeField] int height = 20;
  [SerializeField] int segments = 20;
  [SerializeField] float increaseHeight = 0.01f;
  [SerializeField] float minExtrude = 0.1f;
  [SerializeField] float maxExtrude = 1f;
  [SerializeField] bool diagonal;

  ProBuilderMesh mesh;

  public void Generate()
  {
    mesh = ShapeGenerator.GeneratePlane(PivotLocation.FirstVertex, width, height, segments, segments, Axis.Up);
    mesh.GetComponent<MeshRenderer>().sharedMaterial = BuiltinMaterials.defaultMaterial;

    if (diagonal) ExtrudeFacesDiagonal();
    else ExtrudeFaces();
  }


  void ExtrudeFaces()
  {
    var increase = 0f;
    foreach (var face in mesh.faces)
    {
      increase += increaseHeight;
      float rnd = Random.Range(minExtrude, maxExtrude * increase);
      Debug.Log(rnd);
      rnd = Mathf.Round(rnd * 100) / 100;
      Debug.Log(rnd);
      mesh.Extrude(new[] { face }, ExtrudeMethod.IndividualFaces, rnd);
    }

    mesh.ToMesh();
    mesh.Refresh();
  }

  void ExtrudeFacesDiagonal()
  {
    var increase = 0f;
    var index = 0;

    foreach (var face in mesh.faces)
    {
      if (index != 0 && index % (segments + 1) == 0) increase = index / segments * increaseHeight * segments;
      increase += increaseHeight * segments;
      float rnd = Mathf.Round(Random.Range(minExtrude, maxExtrude * increase) / 10) * 10;
      mesh.Extrude(new[] { face }, ExtrudeMethod.IndividualFaces, rnd);
      index++;
    }

    mesh.ToMesh();
    mesh.Refresh();
  }
}
