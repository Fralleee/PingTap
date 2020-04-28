using Fralle.Attack;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
  public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };
  public static event Action<Enemy> OnAnyEnemyDeath = delegate { };
  public event Action<Enemy> OnDeath = delegate { };

  [HideInInspector] public AgentNavigation agentNavigation;
  [HideInInspector] public NavMeshAgent navMeshAgent;
  [HideInInspector] public Health health;

  public int damageAmount = 1;
  public WaveType waveType = WaveType.Ground;
  [SerializeField] EnemyDeathReaction enemyDeathReaction = EnemyDeathReaction.UpInTheAir;

  bool isDead;

  void Awake()
  {
    agentNavigation = GetComponent<AgentNavigation>();
    navMeshAgent = GetComponent<NavMeshAgent>();
    health = GetComponent<Health>();

    health.OnDeath += HandleDeath;
    MatchManager.OnDefeat += HandleDefeat;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.K)) EnemyDeath(true);
  }

  public void ReachedDestination()
  {
    OnEnemyReachedPowerStone(this);
    EnemyDeath(true);
  }

  void HandleDeath(Health health, Damage damage)
  {
    EnemyDeath();
  }

  void HandleDefeat(MatchManager levelManager)
  {
    Destroy(gameObject);
  }

  void EnemyDeath(bool destroyImmediately = false)
  {
    if (isDead) return;

    isDead = true;
    OnAnyEnemyDeath(this);
    OnDeath(this);

    if (destroyImmediately)
    {
      Destroy(gameObject);
      return;
    }

    gameObject.SetLayerRecursively(LayerMask.NameToLayer("Corpse"));
    if (navMeshAgent) navMeshAgent.enabled = false;

    var rigidBody = gameObject.AddComponent<Rigidbody>();
    switch (enemyDeathReaction)
    {
      case EnemyDeathReaction.UpInTheAir:
        rigidBody.AddForce(Vector3.up * Random.Range(500f, 1000f));
        rigidBody.AddTorque(Random.onUnitSphere * 360f);
        break;
      case EnemyDeathReaction.DropDead:
        rigidBody.AddTorque(Random.onUnitSphere * 90f);
        break;
    }

    Destroy(gameObject, 3f);
  }

  void OnDestroy()
  {
    health.OnDeath -= HandleDeath;
    MatchManager.OnDefeat -= HandleDefeat;
  }
}
