using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
  [Header("Movement")]
  [SerializeField] float forwardSpeed = 40f;
  [SerializeField] float strafeSpeed = 30f;
  [SerializeField] float maxSpeed = 5f;
  [SerializeField] float stopTime = 0.1f;
  [SerializeField] float jumpStrength = 4f;

  [Header("Mouse Look")]
  [SerializeField] float mouseSensitivity = 50f;
  [SerializeField] float mouseLookSmooth = 0f;

  [Header("Debug")]
  [SerializeField] Vector3 currentVelocity;
  [SerializeField] Vector3 inputMovement;
  [SerializeField] Vector3 inputMouseLook;
  [SerializeField] float currentRotationX;
  [SerializeField] float currentRotationY;

  new Rigidbody rigidbody;
  new Camera camera;
  bool performJump;
  float stopX;
  float stopZ;
  float mouseLookDampX;
  float mouseLookDampY;
  float distToGround;

  bool isGrounded
  {
    get
    {
      return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
  }


  void Awake()
  {
    rigidbody = GetComponent<Rigidbody>();
    camera = GetComponentInChildren<Camera>();
    distToGround = GetComponent<Collider>().bounds.extents.y;


    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
    GatherInputs();
    Movement();
    Jump();
  }

  void FixedUpdate()
  {
    LimitSpeed();
    StoppingForces();
  }

  void LateUpdate()
  {
    MouseLook();
  }

  void GatherInputs()
  {
    inputMovement.x = Input.GetAxis("Horizontal") * strafeSpeed;
    inputMovement.y = Input.GetAxis("Vertical") * forwardSpeed;

    inputMouseLook.x -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
    inputMouseLook.y += Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;

    performJump = Input.GetButtonDown("Jump");
  }

  void Movement()
  {
    rigidbody.AddRelativeForce(inputMovement.x * Time.deltaTime, 0, inputMovement.y * Time.deltaTime, ForceMode.VelocityChange);
  }

  void LimitSpeed()
  {
    Vector3 horizontalMovement = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
    if (horizontalMovement.magnitude > maxSpeed)
    {
      horizontalMovement = horizontalMovement.normalized * maxSpeed;
      rigidbody.velocity = new Vector3(horizontalMovement.x, rigidbody.velocity.y, horizontalMovement.z);
    }
  }

  void StoppingForces()
  {
    float velocityX = Mathf.SmoothDamp(rigidbody.velocity.x, 0, ref stopX, stopTime);
    float velocityZ = Mathf.SmoothDamp(rigidbody.velocity.z, 0, ref stopZ, stopTime);
    rigidbody.velocity = new Vector3(velocityX, rigidbody.velocity.y, velocityZ);
  }

  void MouseLook()
  {
    inputMouseLook.x = Mathf.Clamp(inputMouseLook.x, -90, 90);

    currentRotationX = Mathf.SmoothDamp(currentRotationX, inputMouseLook.x, ref mouseLookDampX, mouseLookSmooth);
    currentRotationY = Mathf.SmoothDamp(currentRotationY, inputMouseLook.y, ref mouseLookDampY, mouseLookSmooth);

    camera.transform.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
    transform.localRotation = Quaternion.Euler(0, currentRotationY, 0);
  }

  void Jump()
  {
    if (performJump)
    {
      performJump = false;
      if (isGrounded)
      {
        rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
      }
    }
  }
}
