using Fralle.FpsController;
using UnityEngine;

namespace Fralle
{
  public class PlayerRotationController : MonoBehaviour
  {
    [SerializeField] float rotateOnAngle = 35;
    [SerializeField] Transform orientation;

    RigidbodyController playerController;
    bool doRotate;

    void Awake()
    {
      playerController = GetComponentInParent<RigidbodyController>();
    }

    void Update()
    {
      UpdateRotation();
    }

    void UpdateRotation()
    {
      if (playerController.IsMoving)
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
