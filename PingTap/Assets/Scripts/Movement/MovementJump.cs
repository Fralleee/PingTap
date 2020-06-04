using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement
{
  public class MovementJump : MonoBehaviour
  {
    [SerializeField] float jumpStrength = 8f;

    PlayerInputController input;

    Rigidbody rigidBody;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
      Jump();
    }

    void Jump()
    {
      if (!input.jumpButtonDown) return;

      rigidBody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
    }
  }
}
