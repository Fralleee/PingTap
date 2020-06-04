using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementAirControl : MonoBehaviour
  {
    [HideInInspector] public float startSpeed;

    [SerializeField] float forwardSpeed = 100f;
    [SerializeField] float strafeSpeed = 75f;
    [SerializeField] float maxSpeed = 5f;

    PlayerInputController input;
    PlayerMovement playerMovement;

    Rigidbody rigidBody;
    Transform orientation;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();
      playerMovement = GetComponentInParent<PlayerMovement>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    void FixedUpdate()
    {
      Movement();
      LimitSpeed();
    }

    void Movement()
    {
      var force = orientation.right * input.move.x * strafeSpeed * Time.deltaTime + orientation.forward * input.move.y * forwardSpeed * Time.deltaTime;
      rigidBody.AddForce(force, ForceMode.VelocityChange);
    }

    void LimitSpeed()
    {
      var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
      if (horizontalMovement.magnitude < (maxSpeed + startSpeed)) return;

      horizontalMovement = horizontalMovement.normalized * (maxSpeed + startSpeed);
      rigidBody.velocity = new Vector3(horizontalMovement.x, rigidBody.velocity.y, horizontalMovement.z);

      //if (playerMovement.debug) playerMovement.debugUi.SetVelocityText(rigidBody.velocity.With(y: 0).magnitude);
    }
  }
}
