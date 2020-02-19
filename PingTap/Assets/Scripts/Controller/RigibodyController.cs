using UnityEngine;

public class RigibodyController : MonoBehaviour
{

  public float Speed = 5f;
  public float JumpHeight = 2f;
  public float GroundDistance = 0.2f;
  public float DashDistance = 5f;
  public LayerMask Ground;

  private Rigidbody rigidBody;
  private Vector3 input = Vector3.zero;
  private bool isGrounded = true;
  private Transform groundChecker;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    groundChecker = transform.GetChild(0);
  }

  void Update()
  {
    isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);


    input = Vector3.zero;
    input.x = Input.GetAxis("Horizontal");
    input.z = Input.GetAxis("Vertical");
    if (input != Vector3.zero)
      transform.forward = input;

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      rigidBody.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }
    if (Input.GetButtonDown("Dash"))
    {
      Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rigidBody.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rigidBody.drag + 1)) / -Time.deltaTime)));
      rigidBody.AddForce(dashVelocity, ForceMode.VelocityChange);
    }
  }


  void FixedUpdate()
  {
    rigidBody.MovePosition(rigidBody.position + input * Speed * Time.fixedDeltaTime);
  }
}