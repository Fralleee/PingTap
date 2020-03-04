using UnityEngine;

public enum Axis
{
  X,
  Y
}

public class AnimatedUVs : MonoBehaviour
{
  [SerializeField] float scrollSpeed = 0.25F;
  [SerializeField] Axis direction;
  new Renderer renderer;

  void Start()
  {
    renderer = GetComponent<Renderer>();
  }

  void LateUpdate()
  {
    float offset = Time.time * scrollSpeed;
    renderer.material.SetTextureOffset("_MainTex", new Vector2(direction == Axis.X ? offset : 0, direction == Axis.Y ? offset : 0));
  }

}