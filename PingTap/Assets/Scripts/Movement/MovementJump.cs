using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement
{
  public class MovementJump : MonoBehaviour
  {
    [SerializeField] float jumpStrength = 8f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpModifier = 2f;

    PlayerMovement movement;
    PlayerInputController input;

    Rigidbody rigidBody;

    void Awake()
    {
      movement = GetComponentInParent<PlayerMovement>();
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
      Jump();
    }

    void FixedUpdate()
    {
      GravityAdjuster();
    }

    void Jump()
    {
      if (!input.jumpButtonDown) return;
      if (!movement.groundCheck.IsGrounded) return;

      rigidBody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
      movement.Jump(Vector3.up * jumpStrength);
    }

    void GravityAdjuster()
    {
      if (movement.groundCheck.IsGrounded) return;
      if (movement.state == PlayerMovementState.Dashing) return;

      if (rigidBody.velocity.y < 0)
      {
        rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
      }
      else if (rigidBody.velocity.y > 0 && !input.jumpButtonHold)
      {
        rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpModifier - 1) * Time.deltaTime;
      }
    }
  }
}
