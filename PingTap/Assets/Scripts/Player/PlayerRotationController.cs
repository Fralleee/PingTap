using UnityEngine;

namespace Fralle.PingTap
{
  public class PlayerRotationController : MonoBehaviour
  {
    [SerializeField] float rotateOnAngle = 35;

    FpsController.PlayerController playerController;
    bool doRotate;

    void Awake()
    {
      playerController = GetComponentInParent<FpsController.PlayerController>();
    }

    void Update()
    {
      UpdateRotation();
    }

    void UpdateRotation()
    {
      if (playerController.isMoving)
      {
        transform.rotation = playerController.Orientation;
        doRotate = false;
      }
      else if (Quaternion.Angle(transform.rotation, playerController.Orientation) > rotateOnAngle)
        doRotate = true;

      if (!doRotate)
        return;

      transform.rotation = Quaternion.Lerp(transform.rotation, playerController.Orientation, Time.deltaTime * 10f);
      if (Quaternion.Angle(transform.rotation, playerController.Orientation) > 3f)
        return;

      transform.rotation = playerController.Orientation;
      doRotate = false;
    }
  }
}
