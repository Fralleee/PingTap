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

    Combatant combatant;
    readonly List<ShakeEvent> shakeEvents = new List<ShakeEvent>();
    Vector3 headbobPosition;
    Vector3 cameraShakePosition;
    Vector3 recoil;
    Quaternion lookRotation = Quaternion.identity;
    Quaternion recoilRotation = Quaternion.identity;
    Quaternion cameraShakeRotation = Quaternion.identity;
    float timer;

    public HeadbobConfiguration Configuration => overrideConfguration ?? defaultConfiguration;
    public float CurvePosition => Mathf.Sin(timer) * controller.currentMaxMovementSpeed * 0.1f;
    public float BobAmount => Mathf.Abs(CurvePosition);
    public void ApplyLookRotation(Quaternion quaternion) => lookRotation = quaternion;
    public void AddShakeEvent(ShakeTransformEventData data) => shakeEvents.Add(new ShakeEvent(data));

    void Awake()
    {
      combatant = controller.GetComponent<Combatant>();
      combatant.OnWeaponSwitch += OnWeaponSwitch;
      if (combatant.equippedWeapon)
        combatant.equippedWeapon.OnAttack += OnAttack;
    }

    void Update()
    {
      if (Pause || !controller.isMoving || !controller.isGrounded)
        headbobPosition = ProceduralMotion.ResetHeadbob(headbobPosition, HeadbobSmoothSpeed);
      else
        (headbobPosition, timer) = ProceduralMotion.Headbob(BobAmount * Configuration.CameraBobbingAmount * controller.movementSpeedProduct, Configuration.BobbingSpeed, timer);

      (recoil, recoilRotation) = ProceduralMotion.Recoil(recoil, recoilRotation, combatant?.equippedWeapon?.recoilSpeed ?? recoilSpeed, combatant?.equippedWeapon?.recoilRecoverTime ?? recoilRecoverTime);
      (cameraShakePosition, cameraShakeRotation) = ProceduralMotion.CameraShake(shakeEvents);

      transform.localPosition = controller.transform.position + currentOffset + headbobPosition + cameraShakePosition;
      transform.localRotation = lookRotation * recoilRotation * cameraShakeRotation;
    }

    void OnWeaponSwitch(Weapon current, Weapon old)
    {
      if (old)
        old.OnAttack -= OnAttack;
      if (current)
      {
        overrideConfguration = current.headbobConfiguration;
        current.OnAttack += OnAttack;
      }
    }

    void OnAttack(Weapon weapon, AttackAction attackAction)
    {
      recoil += new Vector3(
        Random.Range(-weapon.recoilConstraints.x, weapon.recoilConstraints.x),
        Random.Range(-weapon.recoilConstraints.y, 0),
        Random.Range(-weapon.recoilConstraints.z, weapon.recoilConstraints.z)
      ) * weapon.recoilStatMultiplier;
    }

    void OnDestroy()
    {
      if (combatant && combatant.equippedWeapon)
        combatant.equippedWeapon.OnAttack -= OnAttack;
    }
  }
}
