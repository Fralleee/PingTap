using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Attack
{
  public class Melee : WeaponAction
  {
    public float meleeRadius = 2f;

    [SerializeField] float swingTime = 0.25f;
    [SerializeField] float recoveryTime = 1f;

    bool swinging;
    float activeSwingTime;
    float activeRecoveryTime;

    readonly Vector3 swingPosition = new Vector3(0.4f, 0, 0.75f);
    readonly Quaternion swingRotation = Quaternion.Euler(0, -160f, 0);

    public override void Fire()
    {
      PerformMelee();
    }

    void LateUpdate()
    {
      Animate();
    }

    void Animate()
    {
      if (activeSwingTime > 0)
      {
        activeSwingTime -= Time.deltaTime;
        if (swinging)
        {
          float delta = -(Mathf.Cos(Mathf.PI * (activeSwingTime / swingTime)) - 1f) / 2f;
          transform.localPosition = Vector3.Lerp(swingPosition, Vector3.zero, delta);
          transform.localRotation = Quaternion.Lerp(swingRotation, Quaternion.identity, delta);
        }
        else
        {
          float delta = -(Mathf.Cos(Mathf.PI * (activeSwingTime / swingTime)) - 1f) / 2f;
          transform.localPosition = Vector3.Lerp(Vector3.zero, swingPosition, delta);
          transform.localRotation = Quaternion.Lerp(Quaternion.identity, swingRotation, delta);
          if (activeSwingTime <= 0) weapon.ChangeWeaponAction(WeaponStatus.Ready);
        }
      }
      else if (activeRecoveryTime > 0)
      {
        activeRecoveryTime -= Time.deltaTime;
        swinging = false;
        if (activeRecoveryTime <= 0) activeSwingTime = swingTime;
      }
    }

    void PerformMelee()
    {
      activeSwingTime = swingTime;
      activeRecoveryTime = recoveryTime;
      swinging = true;
      weapon.ChangeWeaponAction(WeaponStatus.Melee);

      IEnumerable<Health> targets = GetTargets();
      foreach (Health target in targets)
      {
        if (!TargetInArc(target)) continue;

        var damage = new Damage()
        {
          damageAmount = Damage,
          player = player,
          element = element,
          effects = damageEffects.Select(x => x.Setup(player, Damage)).ToArray(),
          hitAngle = Vector3.Angle((transform.position - target.transform.position).normalized,
            target.transform.forward),
          position = target.transform.position
        };
        target.ReceiveAttack(damage);
      }
    }

    bool TargetInArc(Component target)
    {
      Vector3 vectorToCollider = (target.transform.position - transform.position).normalized;
      return Vector3.Dot(vectorToCollider, transform.forward) > 0;
    }

    IEnumerable<Health> GetTargets()
    {
      Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRadius);
      return colliders.Select(x => x.GetComponentInParent<Health>())
        .Where(x => x != null).Distinct();
    }
  }
}