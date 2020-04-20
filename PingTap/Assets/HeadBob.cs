using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
  [SerializeField] Transform headBob;

  [Header("Running bob")]
  [SerializeField] float transitionSpeed = 10f;
  [SerializeField] float bobSpeed = 10f;
  [SerializeField] float bobAmount = 0.005f;

  [Header("Jumping bob")]
  [SerializeField] float jumpBobPower = 0.015f;
  [SerializeField] float bounceBackPower = 0.5f;
  [SerializeField] float bounceBackTimeMultiplier = 3f;

  PlayerController playerController;
  float timer = Mathf.PI / 2;
  float velocityY;
  float bounceBackVelocityY;
  float jumpTimer = Mathf.PI / 2;

  readonly float bounceBackThreshold = 0.0001f;

  void Awake()
  {
    playerController = GetComponent<PlayerController>();
    playerController.OnMovement += HandleMovement;
    playerController.OnJump += HandleJump;
    playerController.OnGroundHit += HandleGroundHit;
  }

  void FixedUpdate()
  {
    velocityY = playerController.rigidbody.velocity.y * jumpBobPower;
    if (Mathf.Abs(bounceBackVelocityY) > bounceBackThreshold)
    {
      jumpTimer += Time.deltaTime * bounceBackTimeMultiplier;
      bounceBackVelocityY = Mathf.Cos(jumpTimer) * bounceBackVelocityY;
      velocityY = bounceBackVelocityY;
    }
    else if (Mathf.Abs(velocityY) > bounceBackThreshold)
    {
      jumpTimer += Time.deltaTime;
      velocityY = -Mathf.Cos(jumpTimer) * velocityY;
    }
  }

  void HandleGroundHit(Collision collision)
  {
    jumpTimer = 0.33f;
    bounceBackVelocityY = bounceBackPower * -collision.impulse.y;
  }

  void HandleJump(Rigidbody rigidbody)
  {
    jumpTimer = 0f;
    bounceBackVelocityY = 0f;
  }

  void HandleMovement(Vector3 movement)
  {
    Vector3 newPosition;
    if (movement.magnitude > 0)
    {
      float actualBobAmount = bobAmount * movement.magnitude;
      timer += bobSpeed * Time.deltaTime;
      newPosition = new Vector3(Mathf.Cos(timer) * actualBobAmount, Mathf.Abs((Mathf.Sin(timer) * actualBobAmount)), 0);
    }
    else
    {
      timer = Mathf.PI / 2;
      newPosition = Vector3.Lerp(headBob.localPosition, Vector3.zero, transitionSpeed * Time.deltaTime);
    }

    headBob.localPosition = newPosition.With(Mathf.Abs(velocityY) > bounceBackThreshold 
      ? Mathf.Lerp(headBob.localPosition.y, velocityY, transitionSpeed * Time.deltaTime) 
      : Mathf.Lerp(headBob.localPosition.y, newPosition.y + velocityY, transitionSpeed * Time.deltaTime));

    if (timer > Mathf.PI * 2) timer = 0;
  }
}
