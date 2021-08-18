using Fralle.Core;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap
{
  [Serializable]
  public class DamageDeathHandler
  {
    DamageController damageController;
    NavMeshAgent navMeshAgent;
    Animator animator;

    public void Setup(DamageController damageController)
    {
      this.damageController = damageController;
      this.damageController.OnDeath += HandleDeath;

      navMeshAgent = this.damageController.GetComponentInChildren<NavMeshAgent>();
      animator = this.damageController.GetComponentInChildren<Animator>();
    }

    public void Clean()
    {
      damageController.OnDeath -= HandleDeath;
    }

    void HandleDeath(DamageController dc, DamageData dd)
    {
      if (navMeshAgent)
        navMeshAgent.enabled = false;
      if (animator)
        animator.enabled = false;

      foreach (IDisableOnDeath disableOnDeath in dc.GetComponentsInChildren<IDisableOnDeath>())
        disableOnDeath.enabled = false;

      if (dc.gameObject.TryGetComponent(out CapsuleCollider targetCollider))
        GameObject.Destroy(targetCollider);

      dc.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Corpse"));
      GameObject.Destroy(dc.gameObject, 3f);
    }
  }
}
