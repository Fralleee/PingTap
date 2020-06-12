using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementHeadBob : MonoBehaviour
  {
    [SerializeField] Rigidbody rigidBody = null;
    [SerializeField] AnimationCurve blendOverLifetime = new AnimationCurve();

    [Header("Running bob")]
    [SerializeField] float transitionSpeed = 10f;
    [SerializeField] float bobSpeed = 10f;
    [SerializeField] float bobAmount = 0.005f;

    [Header("Jumping bob")]
    [SerializeField] float jumpBobPower = 0.015f;

    [SerializeField] float bounceBackPower = 0.5f;
    [SerializeField] float bounceBackTimeMultiplier = 3f;


    Vector3 newPosition;
    float timer = Mathf.PI / 2;
    float velocityY;
    float bounceBackVelocityY;
    const float BounceBackThreshold = 0.0001f;
    float jumpTimer = Mathf.PI / 2;


    public void GroundedTick()
    {
      velocityY = 0;
      InternalTick();
    }

    public void AirborneTick()
    {
      velocityY = rigidBody.velocity.y * jumpBobPower;
      InternalTick();
    }

    void InternalTick()
    {
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

      transform.localPosition = newPosition.With(y: Mathf.Abs(velocityY) > BounceBackThreshold ?
        Mathf.Lerp(transform.localPosition.y, velocityY, transitionSpeed * Time.deltaTime) :
        Mathf.Lerp(transform.localPosition.y, newPosition.y + velocityY, transitionSpeed * Time.deltaTime));

      if (timer > Mathf.PI * 2) timer = 0;
    }

    public void HandleGroundHit(bool touchedGround, float velocity)
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

    public void HandleMovement(Vector3 movementInput, float percentageOfMaxSpeed)
    {
      if (movementInput.magnitude > 0)
      {
        var agePercent = 1.0f - ((timer * 0.5f) / Mathf.PI);
        var actualBobAmount = bobAmount * movementInput.magnitude * blendOverLifetime.Evaluate(agePercent);
        timer += bobSpeed * percentageOfMaxSpeed * Time.deltaTime;
        newPosition = new Vector3(Mathf.Cos(timer) * actualBobAmount, Mathf.Abs((Mathf.Sin(timer) * actualBobAmount)), 0);
      }
      else
      {
        timer = Mathf.PI / 2;
        newPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, transitionSpeed * Time.deltaTime);
      }

    }
  }
}