using UnityEngine;

namespace Fralle.FpsController.Moves
{
  public class Jumping : MonoBehaviour
  {
    PlayerController controller;
    InputController input;

    Rigidbody rigidBody;

    bool queueJump;

    void Awake()
    {
      controller = GetComponentInParent<PlayerController>();
      input = GetComponentInParent<InputController>();

      rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
      GatherInput();
    }

    void GatherInput()
    {
      if (controller.IsGrounded && input.JumpButtonDown)
      {
        queueJump = true;
      }
    }

    public void ControlledFixedUpdate()
    {
      Jump();
    }

    void Jump()
    {
      if (!queueJump) return;

      queueJump = false;
      controller.IsJumping = true;
      controller.IsGrounded = false;
      rigidBody.useGravity = true;
      rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
      rigidBody.AddForce(Vector3.up * controller.jumpStrength, ForceMode.VelocityChange);
    }
  }
}
