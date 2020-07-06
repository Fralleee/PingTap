using UnityEngine;

namespace Fralle.FpsController.Moves
{
  public class Movement : MonoBehaviour
  {
    [SerializeField] float stopTime = 0.05f;

    PlayerController controller;
    InputController input;

    Rigidbody rigidBody;
    Transform orientation;

    Vector3 damp;

    void Awake()
    {
      controller = GetComponentInParent<PlayerController>();
      input = GetComponentInParent<InputController>();

      rigidBody = GetComponent<Rigidbody>();

      orientation = transform.Find("Orientation");
    }

    public void Move()
    {
      var desiredForce = orientation.right * input.Move.x + orientation.forward * input.Move.y;
      desiredForce = Vector3.ProjectOnPlane(desiredForce, controller.groundContactNormal).normalized;
      rigidBody.AddForce(desiredForce * controller.forwardSpeed, ForceMode.Impulse);
      StoppingForces();
    }

    void StoppingForces()
    {
      rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, Vector3.zero, ref damp, stopTime);
    }
  }
}
