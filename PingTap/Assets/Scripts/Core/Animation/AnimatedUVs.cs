using Fralle.Core.Enums;
using UnityEngine;

namespace Fralle.Core.Animation
{
  public class AnimatedUVs : MonoBehaviour
  {
    [SerializeField] float scrollSpeed = 0.25F;
    [SerializeField] Axis direction;
    new Renderer renderer;
    static readonly int MainTex = Shader.PropertyToID("_MainTex");

    void Start()
    {
      renderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
      float offset = Time.time * scrollSpeed;
      renderer.material.SetTextureOffset(MainTex, new Vector2(direction == Axis.X ? offset : 0, direction == Axis.Y ? offset : 0));
    }

  }
}