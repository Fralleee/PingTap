using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementLimitSpeed : MonoBehaviour
  {
    [SerializeField] float maxSpeed = 7f;

    Rigidbody rigidBody;
    float externalSpeedModifier = 1f;

    void Awake()
    {
      rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
      LimitSpeed();
    }

    void LimitSpeed()
    {
      var actualMaxSpeed = maxSpeed * externalSpeedModifier;
      var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
      if (!(horizontalMovement.magnitude > actualMaxSpeed)) return;
      horizontalMovement = horizontalMovement.normalized * actualMaxSpeed;
      rigidBody.velocity = new Vector3(horizontalMovement.x, rigidBody.velocity.y, horizontalMovement.z);
    }

    public void SetExternalSpeedModifier(float modifier)
    {
      externalSpeedModifier = Mathf.Clamp(modifier, 0, float.MaxValue);
    }
  }
}
