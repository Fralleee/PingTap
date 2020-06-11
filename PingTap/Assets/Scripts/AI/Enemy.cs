using Fralle.AI.Spawning;
using Fralle.Attack.Offense;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using Fralle.Player;
using System;
using UnityEngine;

namespace Fralle.AI
{
  [RequireComponent(typeof(Health))]
  public class Enemy : MonoBehaviour
  {
    public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };
    public static event Action<Enemy> OnAnyEnemyDeath = delegate { };
    public event Action<Damage> OnDeath = delegate { };

    [HideInInspector] public EnemyNavigation enemyNavigation;
    [HideInInspector] public Health health;

    [Header("General")]
    public int damageAmount = 1;
    public WaveType waveType = WaveType.Ground;

    public bool IsDead { get; private set; }
    public PlayerMain KilledByPlayer { get; private set; }

    void Awake()
    {
      enemyNavigation = GetComponent<EnemyNavigation>();
      health = GetComponent<Health>();

      health.OnDeath += HandleDeath;
      health.OnDamageTaken += HandleDamageTaken;
      MatchManager.OnDefeat += HandleDefeat;
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.K)) Death(null, true);
    }

    public void ReachedDestination()
    {
      OnEnemyReachedPowerStone(this);
      Death(null, true);
    }

    void HandleDamageTaken(Health hp, Damage damage)
    {
      enemyNavigation.StopMovement(0.1f);
    }

    void HandleDeath(Health hp, Damage damage)
    {
      Death(damage);
    }

    void HandleDefeat(PlayerStats stats)
    {
      Destroy(gameObject);
    }

    void Death(Damage damage, bool destroyImmediately = false)
    {
      if (IsDead) return;

      IsDead = true;
      KilledByPlayer = damage?.player;

      DeathEvents(damage);
      DeathVisuals(destroyImmediately);
    }

    void DeathEvents(Damage damage)
    {
      OnAnyEnemyDeath(this);
      OnDeath(damage);
    }

    void DeathVisuals(bool destroyImmediately)
    {
      if (destroyImmediately)
      {
        Destroy(gameObject);
        return;
      }

      gameObject.SetLayerRecursively(LayerMask.NameToLayer("Corpse"));

      Destroy(gameObject, 3f);
    }

    void OnDestroy()
    {
      health.OnDeath -= HandleDeath;
      MatchManager.OnDefeat -= HandleDefeat;
    }
  }
}
