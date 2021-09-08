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
      if (playerController.isMoving)
      {
        transform.rotation = orientation.rotation;
        doRotate = false;
      }
      else if (Vector3.Angle(orientation.forward, transform.forward) > rotateOnAngle)
      {
        doRotate = true;
      }

      if (!doRotate)
        return;

      transform.rotation = Quaternion.Lerp(transform.rotation, orientation.rotation, Time.deltaTime * 10f);
      if (Vector3.Angle(orientation.forward, transform.forward) > 3f)
        return;

      transform.rotation = orientation.rotation;
      doRotate = false;
    }
  }
}
