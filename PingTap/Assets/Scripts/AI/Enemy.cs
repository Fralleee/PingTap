using Fralle;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


[RequireComponent(typeof(DamageController))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
  public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };
  public event Action<Enemy> OnDeath = delegate { };

  NavMeshAgent navMeshAgent;
  DamageController damageController;

  void Awake()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    damageController = GetComponent<DamageController>();
    damageController.OnDeath += EnemyDeath;
  }

  public void ReachedDestination()
  {
    OnEnemyReachedPowerStone(this);
    EnemyDeath();
  }

  void EnemyDeath()
  {
    OnDeath(this);
    navMeshAgent.enabled = false;
    var rigidBody = gameObject.AddComponent<Rigidbody>();
    rigidBody.AddTorque(Random.onUnitSphere * 180f);
    Destroy(gameObject, 3f);
  }
}
