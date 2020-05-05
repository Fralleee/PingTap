﻿using Fralle.AI;
using Fralle.Attack.Offense;
using System;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class Nexus : MonoBehaviour
  {
    public event Action<Nexus> OnDeath = delegate { };

    public Health health;

    void Awake()
    {
      health = GetComponent<Health>();
      health.OnDeath += HandleDeath;
      Enemy.OnEnemyReachedPowerStone += HandleEnemyReachedPowerStone;
    }

    void HandleDeath(Health health, Damage damage)
    {
      Destroy(gameObject);
    }

    void HandleEnemyReachedPowerStone(Enemy enemy)
    {
      health.TakeDamage(new Damage()
      {
        damageAmount = enemy.damageAmount
      });
    }

    void OnDestroy()
    {
      OnDeath(this);
      health.OnDeath -= HandleDeath;
      Enemy.OnEnemyReachedPowerStone -= HandleEnemyReachedPowerStone;
    }
  }
}