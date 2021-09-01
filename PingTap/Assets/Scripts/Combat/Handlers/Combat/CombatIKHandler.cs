using Fralle.Core;
using System;
using System.Linq;
using UnityEngine.Serialization;

namespace Fralle.PingTap
{
  [Serializable]
  public class CombatIKHandler
  {
    [FormerlySerializedAs("Enabled")] public bool enabled;
    [FormerlySerializedAs("UseLeftHand")] public bool useLeftHand = true;

    Combatant combatant;
    HandIK[] leftHandIKs;
    HandIK[] rightHandIKs;
    HandIKTarget[] leftHandIKTargets;
    HandIKTarget[] rightHandIKTargets;

    public void Setup(Combatant combatant)
    {
      this.combatant = combatant;
      this.combatant.OnWeaponSwitch += HandleWeaponSwitch;

      HandIK[] handIKs = this.combatant.GetComponentsInChildren<HandIK>();
      HandIKTarget[] handIKTargets = this.combatant.GetComponentsInChildren<HandIKTarget>();

      leftHandIKs = handIKs.Where(x => x.hand == Hand.Left).ToArray();
      rightHandIKs = handIKs.Where(x => x.hand == Hand.Right).ToArray();
      leftHandIKTargets = handIKTargets.Where(x => x.hand == Hand.Left).ToArray();
      rightHandIKTargets = handIKTargets.Where(x => x.hand == Hand.Right).ToArray();

      if (combatant.equippedWeapon != null)
        SetupIK();
      else
      {
        foreach (HandIK toggleIK in handIKs)
          toggleIK.Toggle(false);
      }
    }

    public void Clean()
    {
      combatant.OnWeaponSwitch -= HandleWeaponSwitch;
    }

    void SetupIK()
    {
      if (useLeftHand && combatant.equippedWeapon.leftHandGrip)
      {
        foreach (HandIK ik in leftHandIKs)
          ik.Toggle();

        foreach (HandIKTarget target in leftHandIKTargets)
          target.Target(combatant.equippedWeapon.leftHandGrip);
      }

      if (!combatant.equippedWeapon.rightHandGrip)
        return;

      foreach (HandIK ik in rightHandIKs)
        ik.Toggle();

      foreach (HandIKTarget target in rightHandIKTargets)
        target.Target(combatant.equippedWeapon.rightHandGrip);
    }

    void HandleWeaponSwitch(Weapon newWeapon, Weapon oldWeapon)
    {
      SetupIK();
    }
  }
}
