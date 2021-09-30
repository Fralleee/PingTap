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
    PlayerCamera playerCamera;
    Volume abilityVolume;
    ParticleSystem speedLinesEffect;

    float defaultFov = 60f;

    public override void Setup(AbilityController ac)
    {
      abilityController = ac;
      playerController = ac.GetComponent<PlayerController>();
      rigidBody = ac.GetComponent<Rigidbody>();

      if (ac.postProcessController)
        abilityVolume = ac.postProcessController.AddProfile(postProcess);

      speedlines = Instantiate(speedlines, ac.postProcessController.transform);
      speedLinesEffect = speedlines.GetComponent<ParticleSystem>();

      playerCamera = playerController.cameraRig.GetComponent<PlayerCamera>();
      defaultFov = playerController.camera.fieldOfView;
    }

    IEnumerator StopDash()
    {
      float elapsedTime = 0f;
      float waitTime = stopTime;
      while (elapsedTime < waitTime)
      {
        playerController.camera.fieldOfView = Mathf.SmoothStep(defaultFov + addFov, defaultFov, elapsedTime / waitTime);
        abilityVolume.weight = Mathf.SmoothStep(1, 0, 1 - elapsedTime / waitTime);

        elapsedTime += Time.deltaTime;
        yield return null;
      }

      playerController.camera.fieldOfView = defaultFov;

      Reset();
    }

    public override void Perform()
    {
      base.Perform();

      isActive = true;
      playerController.isLocked = true;
      Vector3 direction = playerController.cameraRig.forward;
      if (playerController.Movement.magnitude > 0)
      {
        direction = playerController.cameraRig.TransformDirection(playerController.Movement.ToVector3());
        if (playerController.Movement.y > 0)
          direction += playerController.cameraRig.forward;
      }

      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = false;
      rigidBody.AddForce(direction * dashPower, ForceMode.VelocityChange);

      playerCamera.AddShakeEvent(cameraShake);
      abilityVolume.weight = 1;
      speedLinesEffect.Emit(100);
      playerController.camera.fieldOfView = defaultFov + addFov;

      abilityController.StartCoroutine(StopDash());
    }

    void Reset()
    {
      playerController.isLocked = false;
      rigidBody.velocity = Vector3.zero;
      rigidBody.useGravity = true;

      playerController.camera.fieldOfView = defaultFov;
      abilityVolume.weight = 0;
      isActive = false;
    }

    public override void Abort()
    {
      Reset();
    }
  }
}
