using Fralle.PingTap;
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
      if (currentScanTime > 0)
        currentScanTime -= Time.deltaTime;
      if (currentRayCastTime > 0)
        currentRayCastTime -= Time.deltaTime;
    }

    void LookForTarget()
    {
      if (turret.Target || currentScanTime > 0)
        return;

      currentScanTime = scanRate;
      Collider[] colliders = Physics.OverlapSphere(transform.position, turret.Range, layerMask);
      if (colliders.Length <= 0)
        return;

      foreach (Collider col in colliders.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude))
      {
        if (!Physics.Raycast(transform.position, col.transform.position - transform.position, out RaycastHit hitInfo,
          turret.Range, layerMask))
          continue;

        GameObject target = hitInfo.transform.gameObject;
        turret.Target = target.GetComponent<DamageController>();
        turret.TargetHeight = target.GetComponent<Collider>().bounds.center.y;
        currentScanTime = 0;
        break;
      }
    }

    void RaycastTarget()
    {
      if (!turret.Target)
        return;
      if (currentRayCastTime > 0)
        return;

      currentRayCastTime = rayCastRate;
      if (Physics.Raycast(turret.AimRig.position, turret.Target.transform.position - turret.AimRig.position, out RaycastHit hitInfo, turret.Range))
      {
        if (hitInfo.transform != turret.Target.transform)
          turret.Target = null;
      }
      else
        turret.Target = null;
    }

  }
}
