using Fralle.Core;
using UnityEngine;

namespace Fralle.PingTap
{
  public partial class HeadbobWeaponHolderTransformer : LocalTransformer, IPositioner, IRotator
  {
    HeadbobCameraTransformer headbobMaster;
    PlayerController playerController;
    Combatant combatant;

    Vector3 currentPosition = Vector3.zero;
    Quaternion currentRotation = Quaternion.identity;
    Vector3 localAxis = new Vector3(0, 1, 0);

    void Awake()
    {
      headbobMaster = GetComponentInParent<HeadbobCameraTransformer>();
      playerController = GetComponentInParent<PlayerController>();
      combatant = GetComponentInParent<Combatant>();

      combatant.OnWeaponSwitch += OnWeaponSwitch;
      if (combatant.EquippedWeapon != null)
        OnWeaponSwitch(combatant.EquippedWeapon, null);
    }

    public Vector3 GetPosition() => currentPosition;
    public Quaternion GetRotation() => currentRotation;
    public override void Calculate()
    {
      if (headbobMaster.Pause || !playerController.IsMoving || !playerController.IsGrounded)
      {
        Reset();
        return;
      }

      PerformBob();
    }

    void PerformBob()
    {
      currentPosition = Vector3.zero;
      currentRotation = Quaternion.identity;

      currentPosition.y = headbobMaster.BobAmount * headbobMaster.Configuration.WeaponBobbingAmount;

      float angleChanges = currentRotation.eulerAngles.y + headbobMaster.CurvePosition * headbobMaster.Configuration.WeaponRotationAmount;
      currentRotation = Quaternion.AngleAxis(angleChanges, localAxis);
    }
    void Reset()
    {
      currentPosition = Vector3.Lerp(currentPosition, Vector3.zero, Time.deltaTime * headbobMaster.SmoothSpeed);
      currentRotation = Quaternion.Lerp(currentRotation, Quaternion.identity, Time.deltaTime * headbobMaster.SmoothSpeed);
    }

    void OnWeaponSwitch(Weapon newWeapon, Weapon oldWeapon)
    {
      if (oldWeapon != null)
        oldWeapon.OnActiveWeaponActionChanged -= OnWeaponActionChanged;

      if (newWeapon != null)
        newWeapon.OnActiveWeaponActionChanged += OnWeaponActionChanged;
    }
    void OnWeaponActionChanged(Status status)
    {
      headbobMaster.Pause = status == Status.Firing;
    }
  }
}
