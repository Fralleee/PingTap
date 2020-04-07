﻿using Fralle;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AgentNavigation))]
[RequireComponent(typeof(DamageController))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
  public static event Action<Enemy> OnEnemyReachedPowerStone = delegate { };
  public static event Action<Enemy> OnAnyEnemyDeath = delegate { };
  public event Action<Enemy> OnDeath = delegate { };

  [HideInInspector] public AgentNavigation agentNavigation;
  [HideInInspector] public NavMeshAgent navMeshAgent;
  [HideInInspector] public DamageController damageController;

  public int damage = 1;
  public WaveType waveType = WaveType.Ground;

  [SerializeField] GameObject healthBar;
  [SerializeField] GameObject floatingNumbers;
  
  void Awake()
  {
    agentNavigation = GetComponent<AgentNavigation>();
    navMeshAgent = GetComponent<NavMeshAgent>();
    damageController = GetComponent<DamageController>();

    damageController.OnDeath += HandleDeath;
    MatchManager.OnDefeat += HandleDefeat;

    Instantiate(healthBar, transform);
    Instantiate(floatingNumbers, transform);
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

  void HandleDeath(DamageController damageController)
  {
    EnemyDeath();
  }

  void HandleDefeat(MatchManager levelManager)
  {
    Destroy(gameObject);
  }

  void EnemyDeath(bool destroyImmediately = false)
  {
    OnAnyEnemyDeath(this);
    OnDeath(this);

    if (destroyImmediately)
    {
      Destroy(gameObject);
      return;
    }

    gameObject.SetLayerRecursively(LayerMask.NameToLayer("Corpse"));
    navMeshAgent.enabled = false;
    var rigidBody = gameObject.AddComponent<Rigidbody>();
    rigidBody.AddTorque(Random.onUnitSphere * 180f);
    Destroy(gameObject, 3f);
  }

  void OnDestroy()
  {
    damageController.OnDeath -= HandleDeath;
    MatchManager.OnDefeat -= HandleDefeat;
  }
}
