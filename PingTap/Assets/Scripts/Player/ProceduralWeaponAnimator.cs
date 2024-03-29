using Fralle.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fralle.PingTap
{
  public class ProceduralWeaponAnimator : MonoBehaviour
  {
    [FoldoutGroup("Jumpbob")] [SerializeField] float jumpBobClampMomentum = 0.2f;
    [FoldoutGroup("Jumpbob")] [SerializeField] float jumpBobSmoothSpeed = 20f;
    [FoldoutGroup("Jumpbob")] [SerializeField] float jumpBobResetSpeed = 10f;
    [FoldoutGroup("Jumpbob")] [SerializeField] float jumpBobRotationMagnitude = 20f;
    [FoldoutGroup("Jumpbob")] [SerializeField] float jumpBobJumpMagnitude = 0.0005f;
    [FoldoutGroup("Jumpbob")] [SerializeField] float jumpBobFallMagnitude = 0.002f;

    [FoldoutGroup("Sway")] [SerializeField] float swaySmoothRotation = 10f;
    [FoldoutGroup("Sway")] [SerializeField] float swayLookRotationAmount = 1.6f;
    [FoldoutGroup("Sway")] [SerializeField] float swayMaxLookRotation = 3.2f;

    [FoldoutGroup("Tilt")] [SerializeField] float tiltSmoothSpeed = 10f;
    [FoldoutGroup("Tilt")] [SerializeField] float tiltRotationAmount = 3.2f;
    [FoldoutGroup("Tilt")] [SerializeField] float tiltMaxRotation = 5f;

    PlayerCamera playerCamera;
    FpsController.PlayerController controller;
    Combatant combatant;
    Vector3 headbobPosition;
    Vector3 jumpbobPosition;
    Quaternion headbobRotation;
    Quaternion jumpbobRotation;
    Quaternion swayRotation;
    Quaternion tiltRotation;

    float momentum;

    void Start()
    {
      playerCamera = GetComponentInParent<PlayerCamera>();
      controller = playerCamera.controller;
      combatant = controller.GetComponent<Combatant>();
      combatant.OnWeaponSwitch += OnWeaponSwitch;
      if (combatant.equippedWeapon)
        OnWeaponSwitch(combatant.equippedWeapon, null);
    }

    void Update()
    {
      ResetMomentum();
      Headbob();
      Jumpbob();
      Tilt();
      Sway();

      transform.localPosition = headbobPosition + jumpbobPosition;
      transform.localRotation = headbobRotation * jumpbobRotation * tiltRotation * swayRotation;
    }

    void Headbob()
    {
      if (playerCamera.Pause || !controller.isMoving || !controller.isGrounded)
        (headbobPosition, headbobRotation) = ProceduralMotion.ResetHeadbob(headbobPosition, headbobRotation, playerCamera.HeadbobSmoothSpeed);
      else
      {
        float bobAmount = playerCamera.BobAmount * playerCamera.Configuration.WeaponBobbingAmount * controller.movementSpeedProduct;
        float angleChanges = headbobRotation.eulerAngles.y + playerCamera.CurvePosition * playerCamera.Configuration.WeaponRotationAmount;
        (headbobPosition, headbobRotation) = ProceduralMotion.Headbob(bobAmount, angleChanges);
      }
    }

    void Jumpbob()
    {

      if (controller.rigidBody.velocity.y > 0) // Jumping
      {
        momentum -= controller.rigidBody.velocity.y * jumpBobJumpMagnitude;
      }
      else // Falling
      {
        momentum -= controller.rigidBody.velocity.y * jumpBobFallMagnitude;
      }

      momentum = Mathf.Clamp(momentum, -jumpBobClampMomentum, jumpBobClampMomentum);
      jumpbobPosition = Vector3.Lerp(jumpbobPosition, jumpbobPosition.With(y: momentum), Time.deltaTime * jumpBobSmoothSpeed);
      jumpbobRotation = Quaternion.AngleAxis(-momentum * jumpBobRotationMagnitude, new Vector3(1, 0, 0));
    }

    void Tilt()
    {
      if (controller.isMoving)
      {
        float horizontalAmount = Mathf.Clamp(-controller.Movement.x * tiltRotationAmount, -tiltMaxRotation, tiltMaxRotation);
        float verticalAmount = Mathf.Clamp(-controller.Movement.y * tiltRotationAmount, -tiltMaxRotation, tiltMaxRotation);
        Quaternion rotation = Quaternion.Euler(new Vector3(verticalAmount, 0f, horizontalAmount));
        tiltRotation = Quaternion.Lerp(tiltRotation, Quaternion.identity * rotation, Time.deltaTime * tiltSmoothSpeed);
      }
      else
        tiltRotation = Quaternion.Lerp(tiltRotation, Quaternion.identity, Time.deltaTime * tiltSmoothSpeed);
    }

    void Sway()
    {
      float lookAmountX = Mathf.Clamp(controller.MouseLook.x * swayLookRotationAmount, -swayMaxLookRotation, swayMaxLookRotation);
      float lookAmountY = Mathf.Clamp(controller.MouseLook.y * swayLookRotationAmount, -swayMaxLookRotation, swayMaxLookRotation);

      Quaternion finalRotation = Quaternion.Euler(new Vector3(lookAmountY, lookAmountX, 0f));
      swayRotation = Quaternion.Slerp(swayRotation, finalRotation, Time.deltaTime * swaySmoothRotation);
    }

    void ResetMomentum()
    {
      if (controller.isGrounded)
        momentum = Mathf.Lerp(momentum, 0f, Time.deltaTime * jumpBobResetSpeed);
    }

    void OnWeaponSwitch(Weapon newWeapon, Weapon oldWeapon)
    {
      if (oldWeapon)
        oldWeapon.OnActiveWeaponActionChanged -= OnWeaponActionChanged;

      if (newWeapon)
        newWeapon.OnActiveWeaponActionChanged += OnWeaponActionChanged;
    }

    void OnWeaponActionChanged(Status status)
    {
      playerCamera.Pause = status == Status.Firing;
    }
  }
}
