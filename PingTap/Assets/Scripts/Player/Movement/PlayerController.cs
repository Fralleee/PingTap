using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
  public event Action<Vector3> OnMovement = delegate { };
  public event Action<bool> OnGroundChanged = delegate { };

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
  public new Rigidbody rigidbody;
  Transform orientation;

  bool isGrounded;
  bool jumpButtonDown;
  bool jumpButtonHold;

  float stopX;
  float stopZ;
  float distToGround;

  void Awake()
  {
    capsule = GetComponent<CapsuleCollider>();

    rigidbody = GetComponent<Rigidbody>();
    rigidbody.freezeRotation = true;

    distToGround = capsule.bounds.extents.y - capsule.bounds.extents.x + 0.1f;

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

  void Movement()
  {
    float modifier = isGrounded ? 1 : 0.5f;
    Vector3 force = orientation.right * inputMovement.x * strafeSpeed * Time.deltaTime + orientation.forward * inputMovement.y * forwardSpeed * Time.deltaTime;
    rigidbody.AddForce(force * modifier, ForceMode.VelocityChange);
    OnMovement(force * modifier);

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
    if (!isGrounded) return;

    rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
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

  void GroundControl()
  {
    rigidbody.useGravity = true;
    bool wasGrounded = isGrounded;
    isGrounded = Physics.SphereCast(transform.position, capsule.radius, -Vector3.up, out RaycastHit hit, distToGround);
    if(wasGrounded != isGrounded) OnGroundChanged(isGrounded);

    if (debugMode) movementDebugUi.SetGroundedText(isGrounded, isGrounded ? hit.transform.name : "");
    if (!isGrounded || rigidbody.velocity.y < -0.5f) return;

    float slopeAngle = Mathf.Abs(Vector3.Angle(hit.normal, Vector3.forward) - 90f);
    if (debugMode) movementDebugUi.SetSlopeAngleText(slopeAngle);
    if (slopeAngle > maxSlopeAngle + 1f) return;

    rigidbody.useGravity = false;
    rigidbody.AddForce(-hit.normal * Physics.gravity.magnitude * 5f);
  }

}
