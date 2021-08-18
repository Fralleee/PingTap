using UnityEngine;

namespace Fralle.PingTap
{
  public class HeadbobAdjuster : MonoBehaviour
  {
    [SerializeField] HeadbobConfiguration configuration;
    HeadbobCameraTransformer headbob;

    void Start()
    {
      if (configuration)
      {
        headbob = GetComponentInParent<HeadbobCameraTransformer>();
        if (headbob)
          headbob.overrideConfguration = configuration;
      }

    }

    void OnDestroy()
    {
      if (headbob)
        headbob.overrideConfguration = null;
    }
  }
}
