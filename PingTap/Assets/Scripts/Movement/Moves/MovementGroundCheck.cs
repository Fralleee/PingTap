using System;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementGroundCheck : MonoBehaviour
  {
    public bool IsGrounded { get; private set; }
    public event Action<bool, float> OnGroundChanged = delegate { };

    [SerializeField] float maxSlopeAngle = 35;

    Rigidbody rigidBody;
    CapsuleCollider capsule;

    float distToGround;

    void Awake()
    {
      rigidBody = GetComponent<Rigidbody>();
      capsule = GetComponent<CapsuleCollider>();

      distToGround = capsule.bounds.extents.y - capsule.bounds.extents.x + 0.1f;
    }

    void FixedUpdate()
    {
      GroundControl();
    }

    void GroundControl()
    {
      rigidBody.useGravity = true;
      var wasGrounded = IsGrounded;
      IsGrounded = Physics.SphereCast(transform.position, capsule.radius, -Vector3.up, out var hit, distToGround);
      if (wasGrounded != IsGrounded)
      {
        OnGroundChanged(IsGrounded, rigidBody.velocity.y);
      }

      if (!IsGrounded || rigidBody.velocity.y < -0.5f) return;

      var slopeAngle = Mathf.Abs(Vector3.Angle(hit.normal, Vector3.forward) - 90f);
      if (slopeAngle > maxSlopeAngle + 1f) return;

      rigidBody.useGravity = false;
      rigidBody.AddForce(-hit.normal * Physics.gravity.magnitude * 5f);
    }
  }
}
