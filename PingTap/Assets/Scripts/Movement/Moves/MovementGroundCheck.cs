using System;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementGroundCheck : MonoBehaviour
  {
    public bool IsGrounded { get; private set; }
    public event Action<bool, float> OnGroundChanged = delegate { };

    [SerializeField] float maxSlopeAngle = 35;
    [SerializeField] float maxWalkableSlopeAngle = 45;

    PlayerMovement playerMovement;

    Rigidbody rigidBody;
    CapsuleCollider capsule;

    float distToGround;

    void Awake()
    {
      playerMovement = GetComponentInParent<PlayerMovement>();

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
      IsGrounded = Physics.SphereCast(transform.position, capsule.radius - Physics.defaultContactOffset, Vector3.down, out var hit, distToGround);

      if (wasGrounded != IsGrounded)
      {
        OnGroundChanged(IsGrounded, rigidBody.velocity.y);
        if (playerMovement.debug) playerMovement.debugUi.SetGroundedText(IsGrounded, IsGrounded ? hit.transform.name : "");
      }

      if (IsGrounded)
      {
        var slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
        if (playerMovement.debug) playerMovement.debugUi.SetSlopeAngleText(slopeAngle);
        if (slopeAngle > maxWalkableSlopeAngle) rigidBody.AddForce(Physics.gravity * 3f);
        if (slopeAngle > maxSlopeAngle + 1f) return;
      }
      else if (!IsGrounded || rigidBody.velocity.y < -0.5f) return;

      rigidBody.useGravity = false;
      rigidBody.AddForce(-hit.normal * Physics.gravity.magnitude * 5f);
    }
  }
}
