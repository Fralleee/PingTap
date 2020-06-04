using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementGravityAdjuster : MonoBehaviour
  {
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpModifier = 2f;

    PlayerInputController input;

    Rigidbody rigidBody;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
      GravityAdjuster();
    }

    void GravityAdjuster()
    {
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
