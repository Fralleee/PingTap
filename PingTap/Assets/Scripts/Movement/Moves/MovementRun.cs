using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementRun : MonoBehaviour
  {
    [HideInInspector] public MovementHeadBob movementHeadBob;

    [SerializeField] float forwardSpeed = 100f;
    [SerializeField] float strafeSpeed = 75f;
    //[SerializeField] float maxSpeed = 5f;
    [SerializeField] float stopTime = 0.05f;

    PlayerInputController input;

    Rigidbody rigidBody;
    Transform orientation;

    float stopX;
    float stopZ;
    float externalSpeedModifier = 1f;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    void FixedUpdate()
    {
      Movement();
      StoppingForces();
    }

    void Movement()
    {
      var force = orientation.right * input.move.x * strafeSpeed * Time.deltaTime + orientation.forward * input.move.y * forwardSpeed * Time.deltaTime;
      rigidBody.AddForce(force * externalSpeedModifier, ForceMode.VelocityChange);
      if (movementHeadBob) movementHeadBob.HandleMovement(force, externalSpeedModifier);
    }

    void StoppingForces()
    {
      var velocityX = Mathf.SmoothDamp(rigidBody.velocity.x, 0, ref stopX, stopTime);
      var velocityZ = Mathf.SmoothDamp(rigidBody.velocity.z, 0, ref stopZ, stopTime);
      rigidBody.velocity = new Vector3(velocityX, rigidBody.velocity.y, velocityZ);
    }
  }
}
