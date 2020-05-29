using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement
{
  public class MovementRun : MonoBehaviour
  {
    [SerializeField] float forwardSpeed = 100f;
    [SerializeField] float strafeSpeed = 75f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float stopTime = 0.05f;

    PlayerMovement movement;
    PlayerInputController input;

    Rigidbody rigidBody;
    Transform orientation;

    float stopX;
    float stopZ;

    void Awake()
    {
      movement = GetComponentInParent<PlayerMovement>();
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    void Update()
    {
      if (!movement.enabled) return;
    }

    void FixedUpdate()
    {
      if (!movement.enabled) return;

      Movement();
      LimitSpeed();
      StoppingForces();
    }

    void Movement()
    {
      var modifier = movement.groundCheck.IsGrounded ? 1 : 0.5f;
      var force = orientation.right * input.move.x * strafeSpeed * Time.deltaTime + orientation.forward * input.move.y * forwardSpeed * Time.deltaTime;
      rigidBody.AddForce(force * modifier, ForceMode.VelocityChange);
      movement.Movement(force * modifier);
    }

    void LimitSpeed()
    {
      if (movement.state == PlayerMovementState.Dashing) return;

      var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
      if (!(horizontalMovement.magnitude > maxSpeed)) return;
      horizontalMovement = horizontalMovement.normalized * maxSpeed;
      rigidBody.velocity = new Vector3(horizontalMovement.x, rigidBody.velocity.y, horizontalMovement.z);
    }

    void StoppingForces()
    {
      if (movement.state == PlayerMovementState.Dashing) return;
      if (!movement.groundCheck.IsGrounded || input.move.magnitude > 0.5f) return;

      var velocityX = Mathf.SmoothDamp(rigidBody.velocity.x, 0, ref stopX, stopTime);
      var velocityZ = Mathf.SmoothDamp(rigidBody.velocity.z, 0, ref stopZ, stopTime);
      rigidBody.velocity = new Vector3(velocityX, rigidBody.velocity.y, velocityZ);
    }
  }
}
