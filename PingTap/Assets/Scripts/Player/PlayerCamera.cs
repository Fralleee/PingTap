using Fralle.Core;
using Fralle.Core.CameraControls;
using Fralle.FpsController;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
  public class PlayerCamera : CameraController, ICameraRotator
  {
    [FoldoutGroup("References")] public Transform weaponCamera;
    [FoldoutGroup("References")] public Transform weaponHolder;

    [FoldoutGroup("Headbob")] [SerializeField] HeadbobConfiguration defaultConfiguration;
    [FoldoutGroup("Headbob")] public HeadbobConfiguration overrideConfguration;
    [FoldoutGroup("Headbob")] public float HeadbobSmoothSpeed = 10f;
    [FoldoutGroup("Headbob")] public bool Pause;

    [FoldoutGroup("Recoil")] [ReadOnly] public float recoilSpeed = 15f;
    [FoldoutGroup("Recoil")] [ReadOnly] public float recoilRecoverTime = 10f;

    readonly List<ShakeEvent> shakeEvents = new List<ShakeEvent>();

    Vector3 headbobPosition;
    Vector3 cameraShakePosition;
    Vector3 recoil;
    Quaternion lookRotation;
    Quaternion recoilRotation;
    Quaternion cameraShakeRotation;
    float timer;

    public HeadbobConfiguration Configuration => overrideConfguration ?? defaultConfiguration;
    public float CurvePosition => Mathf.Sin(timer) * controller.currentMaxMovementSpeed * 0.1f;
    public float BobAmount => Mathf.Abs(CurvePosition);
    public void ApplyLookRotation(Quaternion quaternion) => lookRotation = quaternion;
    public void AddRecoil(Vector3 vector3) => recoil += vector3;
    public void AddShakeEvent(ShakeTransformEventData data) => shakeEvents.Add(new ShakeEvent(data));
    public void SetupRecoil(float recoilSpeed, float recoilRecoverTime)
    {
      this.recoilSpeed = recoilSpeed;
      this.recoilRecoverTime = recoilRecoverTime;
    }

    void Update()
    {
      if (Pause || !controller.isMoving || !controller.isGrounded)
        headbobPosition = ProceduralMotion.ResetHeadbob(headbobPosition, HeadbobSmoothSpeed);
      else
        (headbobPosition, timer) = ProceduralMotion.Headbob(BobAmount * Configuration.CameraBobbingAmount * controller.movementSpeedProduct, Configuration.BobbingSpeed, timer);

      (recoil, recoilRotation) = ProceduralMotion.Recoil(recoil, recoilRotation, recoilSpeed, recoilRecoverTime);
      (cameraShakePosition, cameraShakeRotation) = ProceduralMotion.CameraShake(shakeEvents);

      transform.localPosition = controller.transform.position + currentOffset + headbobPosition + cameraShakePosition;
      transform.localRotation = lookRotation * recoilRotation * cameraShakeRotation;
    }

  }
}
