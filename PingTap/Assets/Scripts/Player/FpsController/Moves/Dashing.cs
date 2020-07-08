using Fralle.Core.Camera;
using System;
using System.Collections;
using UnityEngine;

namespace Fralle.FpsController.Moves
{
  public class Dashing : MonoBehaviour
  {
    public event Action OnDashStart = delegate { };
    public event Action OnDashEnd = delegate { };

    [SerializeField] float stopTime = 0.25f;
    [SerializeField] Transform cameraRig = null;
    [SerializeField] ShakeTransformEventData cameraShake = null;
    [SerializeField] ShakeTransform cameraShakeTransform = null;

    PlayerController controller;
    InputController input;

    Rigidbody rigidBody;
    Transform orientation;

    float cooldownTimer;

    bool queueDash;

    void Awake()
    {
      controller = GetComponentInParent<PlayerController>();
      input = GetComponentInParent<InputController>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    void Update()
    {
      if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
      if (input.DashButtonDown && cooldownTimer <= 0) queueDash = true;
    }

    public void Dash()
    {
      if (!queueDash || controller.IsDashing) return;

      OnDashStart();
      queueDash = false;
      controller.IsDashing = true;
      Perform();
    }

    void Perform()
    {
      cooldownTimer = controller.dashCooldown;

      var direction =
        input.Move.y > 0 ? cameraRig.forward :
        input.Move.y < 0 ? -orientation.forward :
        input.Move.x > 0 ? orientation.right :
        input.Move.x < 0 ? -orientation.right :
        cameraRig.forward;

      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = false;
      rigidBody.AddForce(direction * controller.dashPower, ForceMode.VelocityChange);

      if (cameraShakeTransform && cameraShake)
      {
        cameraShakeTransform.AddShakeEvent(cameraShake);
      }

      StartCoroutine(StopDashing());
    }

    public void Abort()
    {
      StopAllCoroutines();
      Reset();
    }

    void Reset()
    {
      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = true;
      controller.IsDashing = false;
      OnDashEnd();
    }

    IEnumerator StopDashing()
    {
      yield return new WaitForSeconds(stopTime);
      Reset();
    }

  }
}
