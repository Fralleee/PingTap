using CombatSystem.Combat.Damage;
using Fralle.Core.Extensions;
using UnityEngine;

public class Turret : MonoBehaviour
{
  public DamageController target;
  public float targetHeight;
  public Transform aimRig;
  public bool IsDeployed;
  public float range = 25f;

  void Update()
  {
    RotateTowardsTarget();
  }

  void RotateTowardsTarget()
  {
    if (!target) return;

    var targetPosition = target.transform.position.With(y: targetHeight);
    var direction = (targetPosition - aimRig.position).normalized;
    var lookRotation = Quaternion.LookRotation(direction);
    aimRig.rotation = Quaternion.Slerp(aimRig.rotation, lookRotation, Time.deltaTime * 6f);
  }

}
