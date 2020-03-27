using UnityEngine;

namespace Fralle
{
  public class Player : MonoBehaviour
  {
    void Start()
    {
      int layer = LayerMask.NameToLayer("Self");
      gameObject.SetLayerRecursively(layer);

    }
  }
}