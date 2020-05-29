using Fralle.Core.Camera;
using Fralle.Player;
using System.Collections;
using UnityEngine;

namespace Fralle.Movement
{
  public class MovementDash : MonoBehaviour
  {
    [SerializeField] float dashPower = 8f;
    [SerializeField] float dashStopTime = 0.1f;
    [SerializeField] Transform cameraRig;
    [SerializeField] ShakeTransformEventData cameraShake;
    [SerializeField] ShakeTransform cameraShakeTransform;

    PlayerMovement movement;
    PlayerInputController input;

    Rigidbody rigidBody;
    Transform orientation;

    void Awake()
    {
      movement = GetComponentInParent<PlayerMovement>();
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    void Update()
    {
      Dash();
    }

    void Dash()
    {
      if (movement.state != PlayerMovementState.Ready || !input.dashButtonDown) return;

      var direction =
        input.move.y > 0 ? cameraRig.forward :
        input.move.y < 0 ? -orientation.forward :
        input.move.x > 0 ? orientation.right :
        input.move.x < 0 ? -orientation.right :
        cameraRig.forward;

      rigidBody.velocity = Vector3.zero;
      rigidBody.AddForce(direction * dashPower, ForceMode.VelocityChange);

      movement.state = PlayerMovementState.Dashing;
      movement.Dash(direction * dashPower);

      cameraShakeTransform.AddShakeEvent(cameraShake);

      StartCoroutine(StopDashing());
    }

    IEnumerator StopDashing()
    {
      yield return new WaitForSeconds(dashStopTime);

      if (movement.state != PlayerMovementState.Dashing) yield break;

      rigidBody.velocity = Vector3.zero;
      movement.state = PlayerMovementState.Ready;
    }
  }
}
