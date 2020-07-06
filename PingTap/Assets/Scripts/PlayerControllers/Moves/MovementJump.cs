using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementJump : MonoBehaviour
  {
    [SerializeField] float jumpStrength = 8f;

    PlayerInput input;

    Rigidbody rigidBody;

    void Awake()
    {
      input = GetComponentInParent<PlayerInput>();

      rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
      Jump();
    }

    void Jump()
    {
      if (!input.JumpButtonDown) return;

      rigidBody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
    }
  }
}
