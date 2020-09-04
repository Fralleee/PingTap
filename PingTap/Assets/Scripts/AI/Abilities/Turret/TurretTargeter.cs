using CombatSystem.Combat.Damage;
using System.Linq;
using UnityEngine;

namespace Fralle.Abilities.Turret
{
  public class TurretTargeter : MonoBehaviour
  {
    [SerializeField] float scanRate = 2f;
    [SerializeField] float rayCastRate = 0.5f;
    [SerializeField] LayerMask layerMask = new LayerMask();

    TurretController turret;

    float currentScanTime;
    float currentRayCastTime;

    void Awake()
    {
      turret = GetComponent<TurretController>();
    }

    void Update()
    {
      HandleCooldowns();
      LookForTarget();
      RaycastTarget();
    }

    void HandleCooldowns()
    {
      if (currentScanTime > 0) currentScanTime -= Time.deltaTime;
      if (currentRayCastTime > 0) currentRayCastTime -= Time.deltaTime;
    }

    void LookForTarget()
    {
      if (turret.target || currentScanTime > 0) return;

      currentScanTime = scanRate;
      var colliders = Physics.OverlapSphere(transform.position, turret.range, layerMask).ToList();
      if (colliders.Count > 0)
      {
        colliders.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude);
        foreach (var col in colliders)
        {
          if (Physics.Raycast(transform.position, col.transform.position - transform.position, out RaycastHit hitInfo, turret.range, layerMask))
          {
            var target = hitInfo.transform.gameObject;
            turret.target = target.GetComponent<DamageController>();
            turret.targetHeight = target.GetComponent<Collider>().bounds.center.y;
            currentScanTime = 0;
            break;
          }
        }
      }
    }

    void RaycastTarget()
    {
      if (!turret.target) return;
      if (currentRayCastTime > 0) return;

      currentRayCastTime = rayCastRate;
      if (Physics.Raycast(turret.aimRig.position, turret.target.transform.position - turret.aimRig.position, out RaycastHit hitInfo, turret.range))
      {
        if (hitInfo.transform != turret.target.transform) turret.target = null;
      }
      else turret.target = null;
    }

  }
}