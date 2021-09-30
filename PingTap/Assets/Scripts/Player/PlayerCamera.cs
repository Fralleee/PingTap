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

    void Update()
    {
      Headbob();
      Recoil();
      CameraShake();

      transform.localPosition = controller.transform.position + currentOffset + headbobPosition + cameraShakePosition;
      transform.localRotation = lookRotation * recoilRotation * cameraShakeRotation;
    }
    void Headbob()
    {
      if (Pause || !controller.isMoving || !controller.isGrounded)
        ResetHeadbob();
      else
      {
        headbobPosition = Vector3.zero;
        headbobPosition.y = BobAmount * Configuration.CameraBobbingAmount * controller.movementSpeedProduct;

        timer += Configuration.BobbingSpeed * Time.deltaTime;
        if (timer > Mathf.PI * 2)
          timer -= Mathf.PI * 2;
      }
    }

    void Recoil()
    {
      Quaternion toRotation = Quaternion.Euler(recoil.y, recoil.x, recoil.z);
      recoilRotation = Quaternion.RotateTowards(recoilRotation, toRotation, recoilSpeed * Time.deltaTime);
      recoil = Vector3.Lerp(recoil, Vector3.zero, recoilRecoverTime * Time.deltaTime);
    }

    void CameraShake()
    {
      Vector3 positionOffset = Vector3.zero;
      Vector3 rotationOffset = Vector3.zero;

      for (int i = shakeEvents.Count - 1; i != -1; i--)
      {
        ShakeEvent shakeEvent = shakeEvents[i];
        shakeEvent.Update();

        if (shakeEvent.Target == ShakeTransformEventData.TargetTransform.Position)
          positionOffset += shakeEvent.noise;
        else
          rotationOffset += shakeEvent.noise;

        if (!shakeEvent.IsAlive())
          shakeEvents.RemoveAt(i);
      }

      cameraShakePosition = positionOffset;
      cameraShakeRotation = Quaternion.Euler(rotationOffset);
    }

    void ResetHeadbob()
    {
      timer = 0;
      headbobPosition = Vector3.Lerp(headbobPosition, Vector3.zero, Time.deltaTime * HeadbobSmoothSpeed);
    }

    public void SetupRecoil(float recoilSpeed, float recoilRecoverTime)
    {
      this.recoilSpeed = recoilSpeed;
      this.recoilRecoverTime = recoilRecoverTime;
    }

    public void AddRecoil(Vector3 vector3)
    {
      recoil += vector3;
    }

    public void AddShakeEvent(ShakeTransformEventData data)
    {
      shakeEvents.Add(new ShakeEvent(data));
    }

    public void AddShakeEvent(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, ShakeTransformEventData.TargetTransform target)
    {
      ShakeTransformEventData data = ScriptableObject.CreateInstance<ShakeTransformEventData>();
      data.Init(amplitude, frequency, duration, blendOverLifetime, target);

      AddShakeEvent(data);
    }


  }
}
