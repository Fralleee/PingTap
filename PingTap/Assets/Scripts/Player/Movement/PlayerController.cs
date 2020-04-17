using UnityEngine;
using UnityEngine.Rendering;

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
  [SerializeField] GameObject debugUI;
  [SerializeField] bool debugMode;

  MovementDebugUI movementDebugUi;

  Vector3 inputMovement;
  CapsuleCollider capsule;
  new Rigidbody rigidbody;
  Transform orientation;

  bool isGrounded;
  bool jumpButtonDown;
  bool jumpButtonHold;

  float stopX;
  float stopZ;
  float distToGround;
  float capsuleRadius;

  void Awake()
  {
    capsule = GetComponent<CapsuleCollider>();

    rigidbody = GetComponent<Rigidbody>();
    rigidbody.freezeRotation = true;

    var capsuleCollider = GetComponent<CapsuleCollider>();
    distToGround = capsuleCollider.bounds.extents.y - capsuleCollider.bounds.extents.x + 0.1f;

    capsuleRadius = capsuleCollider.radius;

    orientation = transform.Find(("Orientation"));

    InitializeDebug();
  }

  void Update()
  {
    GatherInputs();
    Jump();
  }

  void FixedUpdate()
  {
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

  void Movement()
  {
    float modifier = isGrounded ? 1 : 0.5f;
    Vector3 force = orientation.right * inputMovement.x * strafeSpeed * Time.deltaTime + orientation.forward * inputMovement.y * forwardSpeed * Time.deltaTime;
    rigidbody.AddForce(force * modifier, ForceMode.VelocityChange);

    if (debugMode) movementDebugUi.SetVelocityText(new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude);
  }

  void LimitSpeed()
  {
    var horizontalMovement = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
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

  void InitializeDebug()
  {
    var ui = new GameObject("UI");
    ui.transform.parent = transform;
    GameObject activeDebugUI = Instantiate(debugUI, ui.transform);
    movementDebugUi = activeDebugUI.GetComponentInChildren<MovementDebugUI>();
  }

  void OnCollisionStay(Collision collision)
  {
    rigidbody.useGravity = true;
    if (!isGrounded && !(rigidbody.velocity.y < 0.1)) return;
    isGrounded = false;

    for (var i = 0; i < collision.contactCount; i++)
    {
      ContactPoint contact = collision.GetContact(i);
      if (!(contact.point.y < (transform.position.y - (capsule.radius + 0.199f)))) continue;

      isGrounded = true;
      if (debugMode) movementDebugUi.SetGroundedText(isGrounded, isGrounded ? contact.otherCollider.transform.name : "");

      float slopeAngle = Mathf.Abs(Vector3.Angle(contact.normal, Vector3.forward) - 90f);
      if (debugMode) movementDebugUi.SetSlopeAngleText(slopeAngle);
      if (slopeAngle > maxSlopeAngle + 1f) break;

      rigidbody.useGravity = false;
      rigidbody.AddForce(-contact.normal * Physics.gravity.magnitude * 5f);

      break;
    }
  }

  void OnCollisionExit()
  {
    rigidbody.useGravity = true;
  }

  #region Old code
  // # FixedUpdate
  //void GroundControl()
  //{
  //rigidbody.useGravity = true;
  //isGrounded = Physics.SphereCast(transform.position, capsuleRadius, -Vector3.up, out RaycastHit hit, distToGround);

  //if (debugMode) movementDebugUi.SetGroundedText(isGrounded, isGrounded ? hit.transform.name : "");
  //if (!isGrounded || rigidbody.velocity.y < -0.5f) return;

  //float slopeAngle = Mathf.Abs(Vector3.Angle(hit.normal, Vector3.forward) - 90f);
  //if (debugMode) movementDebugUi.SetSlopeAngleText(slopeAngle);
  //if (slopeAngle > maxSlopeAngle + 1f) return;

  //rigidbody.useGravity = false;
  //rigidbody.AddForce(-hit.normal * Physics.gravity.magnitude * 5f);
  //}
  #endregion

}
