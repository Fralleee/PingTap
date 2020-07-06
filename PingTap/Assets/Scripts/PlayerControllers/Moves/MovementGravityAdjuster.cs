using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementGravityAdjuster : MonoBehaviour
  {
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpModifier = 2f;

    PlayerInput input;

    Rigidbody rigidBody;

    void Awake()
    {
      input = GetComponentInParent<PlayerInput>();

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
      else if (rigidBody.velocity.y > 0 && !input.JumpButtonHold)
      {
        rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpModifier - 1) * Time.deltaTime;
      }
    }
  }
}
