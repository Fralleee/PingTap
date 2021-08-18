using Fralle.AbilitySystem;
using Fralle.Core;
using Fralle.Core.CameraControls;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.PingTap
{
  [CreateAssetMenu(menuName = "Abilities/Dash")]
  public class DashAbility : ActiveAbility
  {
    [Header("Settings")]
    [SerializeField] float stopTime = 0.25f;
    [SerializeField] float dashPower = 35;

    [Header("Effects")]
    [SerializeField] ShakeTransformEventData cameraShake;
    [SerializeField] VolumeProfile postProcess;
    [SerializeField] GameObject speedlines;
    [SerializeField] float addFov = 10f;

    AbilityController abilityController;
    PlayerController playerController;
    Rigidbody rigidBody;
    Transform orientation;
    ShakeTransformer cameraShakeTransform;
    Volume abilityVolume;
    GameObject abilityVolumeGo;
    ParticleSystem speedLinesEffect;

    float defaultFov = 60f;

    public override void Setup(AbilityController ac)
    {
      abilityController = ac;
      playerController = ac.GetComponent<PlayerController>();
      rigidBody = ac.GetComponentInChildren<Rigidbody>();
      orientation = rigidBody.transform.Find("Orientation");

      if (ac.postProcessController)
        abilityVolume = ac.postProcessController.AddProfile(postProcess);

      speedlines = Instantiate(speedlines, ac.postProcessController.transform);
      speedLinesEffect = speedlines.GetComponent<ParticleSystem>();

      cameraShakeTransform = playerController.Camera.GetComponentInParent<ShakeTransformer>();
      defaultFov = playerController.Camera.fieldOfView;
    }

    IEnumerator StopDash()
    {
      float elapsedTime = 0f;
      float waitTime = stopTime;
      while (elapsedTime < waitTime)
      {
        playerController.Camera.fieldOfView = Mathf.SmoothStep(defaultFov + addFov, defaultFov, elapsedTime / waitTime);
        abilityVolume.weight = Mathf.SmoothStep(1, 0, 1 - (elapsedTime / waitTime));

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      playerController.Camera.fieldOfView = defaultFov;

      Reset();
    }

    public override void Perform()
    {
      base.Perform();

      IsActive = true;
      playerController.IsLocked = true;
      Vector3 direction = playerController.CameraRig.forward;
      if (playerController.Movement.magnitude > 0)
      {
        direction = orientation.TransformDirection(playerController.Movement.ToVector3());
        if (playerController.Movement.y > 0)
          direction += playerController.CameraRig.forward;
      }

      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = false;
      rigidBody.AddForce(direction * dashPower, ForceMode.VelocityChange);

      cameraShakeTransform.AddShakeEvent(cameraShake);
      abilityVolume.weight = 1;
      speedLinesEffect.Emit(100);
      playerController.Camera.fieldOfView = defaultFov + addFov;

      abilityController.StartCoroutine(StopDash());
    }

    void Reset()
    {
      playerController.IsLocked = false;
      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = true;

      playerController.Camera.fieldOfView = defaultFov;
      abilityVolume.weight = 0;
      IsActive = false;
    }

    public override void Abort()
    {
      Reset();
    }

    void OnDestroy()
    {
      Destroy(abilityVolumeGo);
    }
  }
}
