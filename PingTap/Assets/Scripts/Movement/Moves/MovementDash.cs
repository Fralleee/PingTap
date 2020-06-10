using Fralle.Core.Camera;
using Fralle.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementDash : MonoBehaviour
  {
    public event Action OnComplete = delegate { };

    [Header("Parameters")]
    [SerializeField] float power = 24f;
    [SerializeField] float stopTime = 0.25f;
    [SerializeField] float cooldown = 5f;

    [Header("Objects")]
    [SerializeField] Transform cameraRig;
    [SerializeField] ShakeTransformEventData cameraShake;
    [SerializeField] ShakeTransform cameraShakeTransform;

    PlayerInputController input;
    Rigidbody rigidBody;
    Transform orientation;
    float cooldownTimer;
    public bool isDashing;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    public void Update()
    {
      if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
      if (input.dashButtonDown && cooldownTimer <= 0) StartDashing();
    }

    void StartDashing()
    {
      if (isDashing) return;
      isDashing = true;
    }

    public void PerformDash()
    {
      cooldownTimer = cooldown;

      var direction =
        input.move.y > 0 ? cameraRig.forward :
        input.move.y < 0 ? -orientation.forward :
        input.move.x > 0 ? orientation.right :
        input.move.x < 0 ? -orientation.right :
        cameraRig.forward;

      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = false;
      rigidBody.AddForce(direction * power, ForceMode.VelocityChange);

      cameraShakeTransform.AddShakeEvent(cameraShake);
      StartCoroutine(StopDashing());
    }

    public void AbortDash()
    {
      StopAllCoroutines();
      ResetDash();
    }

    void ResetDash()
    {
      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = true;
      isDashing = false;
    }

    IEnumerator StopDashing()
    {
      yield return new WaitForSeconds(stopTime);
      ResetDash();
      OnComplete();
    }

  }
}
