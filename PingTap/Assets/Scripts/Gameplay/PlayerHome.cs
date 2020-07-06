using CombatSystem.Combat.Damage;
using Fralle.AI;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  [RequireComponent(typeof(DamageController))]
  public class PlayerHome : MonoBehaviour
  {
    public event Action<PlayerHome> OnDeath = delegate { };

    public DamageController damageController;

    void Awake()
    {
      damageController = GetComponent<DamageController>();
      damageController.OnDeath += HandleDeath;
      Enemy.OnEnemyReachedPowerStone += HandleEnemyReachedPowerStone;
    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      OnDeath(this);
      Destroy(gameObject);
    }

    void HandleEnemyReachedPowerStone(Enemy enemy)
    {
      damageController.TakeDamage(new DamageData()
      {
        damageAmount = enemy.damageAmount
      });
    }

    void OnDestroy()
    {
      damageController.OnDeath -= HandleDeath;
      Enemy.OnEnemyReachedPowerStone -= HandleEnemyReachedPowerStone;
    }
  }
}