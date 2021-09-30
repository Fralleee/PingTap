using UnityEngine;

namespace Fralle.PingTap
{
  public class HeadbobAdjuster : MonoBehaviour
  {
    [SerializeField] HeadbobConfiguration configuration;
    PlayerCamera playerCamera;

    void Start()
    {
      if (!configuration)
        return;

      playerCamera = GetComponentInParent<PlayerCamera>();
    }

    public void Activate()
    {
      if (playerCamera && configuration)
        playerCamera.overrideConfguration = configuration;
    }

    void OnDestroy()
    {
      if (playerCamera)
        playerCamera.overrideConfguration = null;
    }
  }
}
