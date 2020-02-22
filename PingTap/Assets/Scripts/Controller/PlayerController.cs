using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
  [Header("Movement")]
  [SerializeField] float forwardSpeed = 100f;
  [SerializeField] float strafeSpeed = 75f;
  [SerializeField] float maxSpeed = 5f;
  [SerializeField] float stopTime = 0.05f;
  [SerializeField] float jumpStrength = 8f;

  [Header("Debug")]
  [SerializeField] string currentVelocity;
  [SerializeField] Vector3 inputMovement;

  new Rigidbody rigidbody;
  Transform orientation;
  bool performJump;
  float stopX;
  float stopZ;
  float distToGround;

  bool IsGrounded => Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);

  void Awake()
  {
    rigidbody = GetComponent<Rigidbody>();
    distToGround = GetComponent<Collider>().bounds.extents.y;
    orientation = transform.Find(("Orientation"));
  }

  void Update()
  {
    GatherInputs();
    Movement();
  }

  void FixedUpdate()
  {
    Jump();
    LimitSpeed();
    StoppingForces();
  }

  void GatherInputs()
  {
    inputMovement.x = Input.GetAxisRaw("Horizontal");
    inputMovement.y = Input.GetAxisRaw("Vertical");
    performJump = Input.GetButton("Jump");
  }

  void Movement()
  {
    var modifier = IsGrounded ? 1 : 0.5f;
    Vector3 force = orientation.right * inputMovement.x * strafeSpeed * Time.deltaTime + orientation.forward * inputMovement.y * forwardSpeed * Time.deltaTime;
    rigidbody.AddRelativeForce(force * modifier, ForceMode.VelocityChange);
    currentVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude.ToString("##.0");
  }

  void LimitSpeed()
  {
    Vector3 horizontalMovement = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
    if (!(horizontalMovement.magnitude > maxSpeed)) return;
    horizontalMovement = horizontalMovement.normalized * maxSpeed;
    rigidbody.velocity = new Vector3(horizontalMovement.x, rigidbody.velocity.y, horizontalMovement.z);
  }

  void StoppingForces()
  {
    if (!IsGrounded || inputMovement.magnitude > 0.5f) return;
    float velocityX = Mathf.SmoothDamp(rigidbody.velocity.x, 0, ref stopX, stopTime);
    float velocityZ = Mathf.SmoothDamp(rigidbody.velocity.z, 0, ref stopZ, stopTime);
    rigidbody.velocity = new Vector3(velocityX, rigidbody.velocity.y, velocityZ);
  }

  void Jump()
  {
    if (!performJump) return;
    if (IsGrounded)
    {
      rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
    }
  }
}
