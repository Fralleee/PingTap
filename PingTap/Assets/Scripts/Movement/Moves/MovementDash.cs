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

    [SerializeField] float dashPower = 8f;
    [SerializeField] float dashStopTime = 0.1f;
    [SerializeField] Transform cameraRig;
    [SerializeField] ShakeTransformEventData cameraShake;
    [SerializeField] ShakeTransform cameraShakeTransform;

    PlayerInputController input;

    Rigidbody rigidBody;
    Transform orientation;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();

      rigidBody = GetComponent<Rigidbody>();
      orientation = transform.Find("Orientation");
    }

    public void PerformDash()
    {
      var direction =
        input.move.y > 0 ? cameraRig.forward :
        input.move.y < 0 ? -orientation.forward :
        input.move.x > 0 ? orientation.right :
        input.move.x < 0 ? -orientation.right :
        cameraRig.forward;

      rigidBody.velocity = Vector3.zero;
      rigidBody.AddForce(direction * dashPower, ForceMode.VelocityChange);

      cameraShakeTransform.AddShakeEvent(cameraShake);
      StartCoroutine(StopDashing());
    }

    public void AbortDash()
    {
      StopAllCoroutines();
      rigidBody.velocity = Vector3.zero;
    }

    IEnumerator StopDashing()
    {
      yield return new WaitForSeconds(dashStopTime);

      rigidBody.velocity = Vector3.zero;
      OnComplete();
    }
  }
}
