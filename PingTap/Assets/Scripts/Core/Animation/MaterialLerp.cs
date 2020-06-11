using UnityEngine;

namespace Fralle.Core.Animation
{
  public class MaterialLerp : MonoBehaviour
  {
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    public Material mat5;

    public float lerptime;

    float q;
    float w;
    float e;
    float r;
    Renderer rend;

    void Start()
    {
      rend = GetComponent<Renderer>();

      rend.material = mat1;
    }

    void Update()
    {
      if (Input.GetKey(KeyCode.Q))
      {

        rend.material.Lerp(mat1, mat2, q);
        q += Time.deltaTime / lerptime;
      }

      if (Input.GetKey(KeyCode.W))
      {
        rend.material.Lerp(mat2, mat3, w);
        w += Time.deltaTime / lerptime;
      }

      if (Input.GetKey(KeyCode.E))
      {
        rend.material.Lerp(mat3, mat4, e);
        e += Time.deltaTime / lerptime;
      }

      if (Input.GetKey(KeyCode.R))
      {
        rend.material.Lerp(mat4, mat5, r);
        r += Time.deltaTime / lerptime;
      }

    }
  }
}