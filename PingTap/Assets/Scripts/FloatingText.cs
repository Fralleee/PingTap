using UnityEngine;

namespace Fralle
{
  public class FloatingText : MonoBehaviour
  {
    [SerializeField] float destroyTime = 2f;
    [SerializeField] Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);

    void Start()
    {
      Destroy(gameObject, destroyTime);
      transform.localPosition += new Vector3(
        Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
        Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
        Random.Range(-randomizeIntensity.z, randomizeIntensity.z)
      );
    }

  }
}