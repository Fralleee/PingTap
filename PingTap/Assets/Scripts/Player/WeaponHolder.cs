using Fralle.Core;
using UnityEngine;

namespace Fralle.PingTap
{
  public class WeaponHolder : MonoBehaviour
  {
    [SerializeField] Transform cameraRig;

    PlayerCamera playerCamera;
    Vector3 horizontalOffset;
    float yRotation;
    float verticalOffset;
    float usedVerticalOffset;
    float progress;
    float forwardOffset = 0.4f;

    private void Awake()
    {
      playerCamera = cameraRig.GetComponent<PlayerCamera>();
    }

    void Start()
    {
      horizontalOffset = transform.localPosition.With(y: 0);
      verticalOffset = transform.localPosition.y;
    }

    void LateUpdate()
    {
      yRotation = cameraRig.rotation.eulerAngles.x;
      forwardOffset = 0.4f;

      if (yRotation >= 270)
      {
        yRotation -= 360;
        forwardOffset = -0.1f;
      }

      progress = Mathf.Abs(yRotation) / playerCamera.controller.clampY;
      usedVerticalOffset = Mathf.Lerp(verticalOffset, 0f, progress);
      forwardOffset = Mathf.Lerp(0f, forwardOffset, progress);


      transform.localPosition = horizontalOffset.With(y: usedVerticalOffset) + Vector3.forward * forwardOffset;
    }
  }
}
