using Fralle.Core;
using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
  public class PlayerRotationController : MonoBehaviour
  {
    [SerializeField] float rotateOnAngle = 35;

    RigidbodyController playerController;
    Transform orientation;
    bool doRotate;

    void Awake()
    {
      playerController = GetComponentInParent<RigidbodyController>();
    }

    private void Start()
    {
      orientation = playerController.cameraRig;
    }

    void Update()
    {
      UpdateRotation();
    }

    void UpdateRotation()
    {
      Vector3 forward = new Vector3(orientation.forward.x, 0, orientation.forward.z).normalized;
      Vector3 eulerAngles = orientation.rotation.eulerAngles.With(x: 0);
      Quaternion horizontalRotation = Quaternion.Euler(eulerAngles);

      if (playerController.isMoving)
      {
        transform.rotation = horizontalRotation;
        doRotate = false;
      }
      else if (Vector3.Angle(forward, transform.forward) > rotateOnAngle)
        doRotate = true;

      if (!doRotate)
        return;

      transform.rotation = Quaternion.Lerp(transform.rotation, horizontalRotation, Time.deltaTime * 10f);
      if (Vector3.Angle(forward, transform.forward) > 3f)
        return;

      transform.rotation = horizontalRotation;
      doRotate = false;
    }
  }
}
