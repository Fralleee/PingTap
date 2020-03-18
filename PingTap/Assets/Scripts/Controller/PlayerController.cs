using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
  [Header("Movement")]
  [SerializeField] float forwardSpeed = 100f;
  [SerializeField] float strafeSpeed = 75f;
  [SerializeField] float maxSpeed = 5f;
  [SerializeField] float stopTime = 0.05f;
  [SerializeField] float maxSlopeAngle = 35;


  [Header("Jumping")]
  [SerializeField] float jumpStrength = 8f;
  [SerializeField] float fallMultiplier = 2.5f;
  [SerializeField] float lowJumpModifier = 2f;

  [Header("Debug")]
  [SerializeField] string currentVelocity;
  [SerializeField] Vector3 inputMovement;
  [SerializeField] bool isGrounded;
  [SerializeField] float currentSlopeAngle;


  new Rigidbody rigidbody;
  Transform orientation;
  bool jumpButtonDown;
  bool jumpButtonHold;
  float stopX;
  float stopZ;
  float distToGround;
  float capsuleRadius;

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
    Jump();
  }

  void FixedUpdate()
  {
    GroundControl();
    Movement();
    LimitSpeed();
    StoppingForces();
    GravityAdjuster();
  }


  void GatherInputs()
  {
    inputMovement.x = Input.GetAxisRaw("Horizontal");
    inputMovement.y = Input.GetAxisRaw("Vertical");
    jumpButtonDown = Input.GetButtonDown("Jump");
    jumpButtonHold = Input.GetButton("Jump");
  }

  void GroundControl()
  {
    rigidbody.useGravity = true;
    isGrounded = Physics.SphereCast(transform.position, capsuleRadius, -Vector3.up, out RaycastHit hit, distToGround);
    if (!isGrounded || rigidbody.velocity.y < -0.5f) return;

    var slopeAngle = Vector3.Angle(hit.normal, Vector3.forward) - 90f;
    currentSlopeAngle = slopeAngle;
    if (slopeAngle < maxSlopeAngle + 1f)
    {
      rigidbody.useGravity = false;
      rigidbody.AddRelativeForce(-hit.normal * Physics.gravity.magnitude * 2);
    }
  }

  void Movement()
  {
    var modifier = isGrounded ? 1 : 0.5f;
    Vector3 force = orientation.right * inputMovement.x * strafeSpeed * Time.deltaTime + orientation.forward * inputMovement.y * forwardSpeed * Time.deltaTime;
    rigidbody.AddForce(force * modifier, ForceMode.VelocityChange);
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
    if (!jumpButtonDown) return;
    if (isGrounded)
    {
      rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
    }
  }

  void GravityAdjuster()
  {
    if (isGrounded) return;
    if (rigidbody.velocity.y < 0)
    {
      rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }
    else if (rigidbody.velocity.y > 0 && !jumpButtonHold)
    {
      rigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpModifier - 1) * Time.deltaTime;
    }
  }
}
