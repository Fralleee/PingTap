using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Movement
{
  public class HeadBob : MonoBehaviour
  {
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] AnimationCurve blendOverLifetime = new AnimationCurve();

    [Header("Running bob")]
    [SerializeField] float transitionSpeed = 10f;
    [SerializeField] float bobSpeed = 10f;
    [SerializeField] float bobAmount = 0.005f;

    [Header("Jumping bob")]
    [SerializeField] float jumpBobPower = 0.015f;

    [SerializeField] float bounceBackPower = 0.5f;
    [SerializeField] float bounceBackTimeMultiplier = 3f;

    PlayerMovement movement;
    float timer = Mathf.PI / 2;
    float velocityY;
    float bounceBackVelocityY;
    const float BounceBackThreshold = 0.0001f;
    float jumpTimer = Mathf.PI / 2;


    void Awake()
    {
      movement = GetComponentInParent<PlayerMovement>();
      movement.OnMovement += HandleMovement;
      movement.OnGroundChanged += HandleGroundHit;
    }

    void FixedUpdate()
    {
      velocityY = !movement.groundCheck.IsGrounded ? rigidBody.velocity.y * jumpBobPower : 0;

      if (Mathf.Abs(bounceBackVelocityY) > BounceBackThreshold)
      {
        jumpTimer += Time.deltaTime * bounceBackTimeMultiplier;
        bounceBackVelocityY = Mathf.Cos(jumpTimer) * bounceBackVelocityY;
        velocityY = bounceBackVelocityY;
      }
      else if (Mathf.Abs(velocityY) > BounceBackThreshold)
      {
        jumpTimer += Time.deltaTime;
        velocityY = -Mathf.Cos(jumpTimer) * velocityY;
      }
    }

    void HandleGroundHit(bool touchedGround, float velocity)
    {
      if (touchedGround)
      {
        jumpTimer = 0.33f;
        var clampedVelocity = Mathf.Clamp(velocity, -30, -10);
        bounceBackVelocityY = bounceBackPower * clampedVelocity;
      }
      else
      {
        jumpTimer = 0f;
        bounceBackVelocityY = 0f;
      }
    }

    void HandleMovement(Vector3 movement)
    {
      Vector3 newPosition;
      if (movement.magnitude > 0)
      {
        var agePercent = 1.0f - ((timer * 0.5f) / Mathf.PI);
        var actualBobAmount = bobAmount * movement.magnitude * blendOverLifetime.Evaluate(agePercent);
        timer += bobSpeed * Time.deltaTime;
        newPosition = new Vector3(Mathf.Cos(timer) * actualBobAmount, Mathf.Abs((Mathf.Sin(timer) * actualBobAmount)), 0);
      }
      else
      {
        timer = Mathf.PI / 2;
        newPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, transitionSpeed * Time.deltaTime);
      }

      transform.localPosition = newPosition.With(y: Mathf.Abs(velocityY) > BounceBackThreshold ?
        Mathf.Lerp(transform.localPosition.y, velocityY, transitionSpeed * Time.deltaTime) :
        Mathf.Lerp(transform.localPosition.y, newPosition.y + velocityY, transitionSpeed * Time.deltaTime));

      if (timer > Mathf.PI * 2) timer = 0;
    }

    void OnDestroy()
    {
      movement.OnMovement -= HandleMovement;
      movement.OnGroundChanged -= HandleGroundHit;
    }
  }
}