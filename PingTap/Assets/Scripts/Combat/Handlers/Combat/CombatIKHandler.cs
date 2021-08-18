using Fralle.Core;
using System;
using System.Linq;

namespace Fralle.PingTap
{
  [Serializable]
  public class CombatIKHandler
  {
    public bool enabled;
    public bool useLeftHand = true;

    Combatant combatant;
    HandIK[] leftHandIks;
    HandIK[] rightHandIks;
    HandIKTarget[] leftHandIKTargets;
    HandIKTarget[] rightHandIKTargets;

    public void Setup(Combatant combatant)
    {
      this.combatant = combatant;
      this.combatant.OnWeaponSwitch += HandleWeaponSwitch;

      HandIK[] handIKs = this.combatant.GetComponentsInChildren<HandIK>();
      HandIKTarget[] handIKTargets = this.combatant.GetComponentsInChildren<HandIKTarget>();

      leftHandIks = handIKs.Where(x => x.hand == Hand.Left).ToArray();
      rightHandIks = handIKs.Where(x => x.hand == Hand.Right).ToArray();
      leftHandIKTargets = handIKTargets.Where(x => x.hand == Hand.Left).ToArray();
      rightHandIKTargets = handIKTargets.Where(x => x.hand == Hand.Right).ToArray();

      if (combatant.EquippedWeapon != null)
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
      if (useLeftHand && combatant.EquippedWeapon.leftHandGrip)
      {
        foreach (HandIK ik in leftHandIks)
          ik.Toggle();

        foreach (HandIKTarget target in leftHandIKTargets)
          target.Target(combatant.EquippedWeapon.leftHandGrip);
      }

      if (combatant.EquippedWeapon.rightHandGrip)
      {
        foreach (HandIK ik in rightHandIks)
          ik.Toggle();

        foreach (HandIKTarget target in rightHandIKTargets)
          target.Target(combatant.EquippedWeapon.rightHandGrip);
      }
    }

    void HandleWeaponSwitch(Weapon newWeapon, Weapon oldWeapon)
    {
      SetupIK();
    }
  }
}
