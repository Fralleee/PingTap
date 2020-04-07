using System;
using System.Collections;
using System.Collections.Generic;
using Fralle;
using UnityEngine;

public class Nexus : MonoBehaviour
{
  public static event Action<Nexus> OnDeath = delegate { };

  DamageController damageController;

  void Awake()
  {
    damageController = GetComponent<DamageController>();
    damageController.OnDeath += HandleDeath;
    Enemy.OnEnemyReachedPowerStone += HandleEnemyReachedPowerStone;
  }

  void HandleDeath(DamageController damageController)
  {
    Destroy(gameObject);
  }

  void HandleEnemyReachedPowerStone(Enemy enemy)
  {
    damageController.TakeDamage(enemy.damage);
  }

  void OnDestroy()
  {
    OnDeath(this);
    damageController.OnDeath -= HandleDeath;
    Enemy.OnEnemyReachedPowerStone -= HandleEnemyReachedPowerStone;
  }
}
