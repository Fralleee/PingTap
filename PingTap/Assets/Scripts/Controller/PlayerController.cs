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
  [SerializeField] float maxSlopeAngle = 35;

  [Header("Debug")]
  [SerializeField] string currentVelocity;
  [SerializeField] Vector3 inputMovement;
  [SerializeField] bool isGrounded;

  new Rigidbody rigidbody;
  Transform orientation;
  bool performJump;
  float stopX;
  float stopZ;
  float distToGround;
  float capsuleRadius;

  /**
   * https://www.youtube.com/watch?v=98MBwtZU2kk&fbclid=IwAR3XuvSJQv0SuTXEn8hbPvqq62PsGLIWSUiw6QfaKlw66EGwts7QjNdCjsc
   * - Calculate forward based on hitInfo. Also other directions since we can strafe? Maybe set orientation based on this?
   * - Reformat groundangle to degress and let this be a setting, max allowed degress on slope.
   **/

  void Awake()
  {
    rigidbody = GetComponent<Rigidbody>();
    rigidbody.freezeRotation = true;

    var capsuleCollider = GetComponent<CapsuleCollider>();
    distToGround = capsuleCollider.bounds.extents.y - capsuleCollider.bounds.extents.x + 0.1f;
    capsuleRadius = capsuleCollider.radius;

    orientation = transform.Find(("Orientation"));
  }

  void Update()
  {
    GatherInputs();
  }

  void FixedUpdate()
  {
    GroundControl();
    Movement();
    LimitSpeed();
    StoppingForces();
    Jump();
  }

  void GatherInputs()
  {
    inputMovement.x = Input.GetAxisRaw("Horizontal");
    inputMovement.y = Input.GetAxisRaw("Vertical");
    performJump = Input.GetButton("Jump");
  }

  void GroundControl()
  {
    rigidbody.useGravity = true;
    isGrounded = Physics.SphereCast(transform.position, capsuleRadius, -Vector3.up, out RaycastHit hit, distToGround);
    if (!isGrounded || rigidbody.velocity.y < -0.5f) return;
    var slopeAngle = Vector3.Angle(hit.normal, Vector3.forward) - 90f;
    if (slopeAngle < maxSlopeAngle + 1f)
    {
      rigidbody.useGravity = false;
      rigidbody.AddRelativeForce(-hit.normal * Physics.gravity.magnitude * 2);
    }
  }

  void Movement()
  {
    var modifier = isGrounded ? 1 : 0.5f;
    Vector3 force = orientation.right * inputMovement.x * strafeSpeed * Time.fixedDeltaTime + orientation.forward * inputMovement.y * forwardSpeed * Time.fixedDeltaTime;
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
    if (!isGrounded || inputMovement.magnitude > 0.5f) return;
    float velocityX = Mathf.SmoothDamp(rigidbody.velocity.x, 0, ref stopX, stopTime);
    float velocityZ = Mathf.SmoothDamp(rigidbody.velocity.z, 0, ref stopZ, stopTime);
    rigidbody.velocity = new Vector3(velocityX, rigidbody.velocity.y, velocityZ);
  }

  void Jump()
  {
    if (!performJump) return;
    if (isGrounded)
    {
      rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
    }
  }
}
