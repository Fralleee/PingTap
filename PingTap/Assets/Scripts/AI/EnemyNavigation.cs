using Fralle.Attack.Offense;
using Fralle.Movement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
  public class EnemyNavigation : MonoBehaviour
  {
    public WaypointSchema wayPointSchema;
    public float currentMovementModifier = 1f;

    Enemy enemy;
    NavMeshAgent navMeshAgent;

    Vector3 nextPosition;
    int waypointIndex;
    float stopTime;
    float movementSpeed;
    readonly Dictionary<string, float> movementModifiers = new Dictionary<string, float>();

    void Awake()
    {
      enemy = GetComponent<Enemy>();
      enemy.OnDeath += HandleDeath;

      navMeshAgent = GetComponent<NavMeshAgent>();
      movementSpeed = navMeshAgent.speed;
    }

    void Start()
    {
      if (!wayPointSchema) return;
      SetDestination();

    }

    void Update()
    {
      if (PathComplete()) SetNextDestination();
      if (stopTime > 0f)
      {
        stopTime = Mathf.Clamp(stopTime - Time.deltaTime, 0, float.MaxValue);
        if (stopTime <= 0) RemoveModifier("DamageTaken");
      }
    }

    void SetDestination()
    {
      nextPosition = wayPointSchema.waypoints[waypointIndex];
      navMeshAgent.destination = nextPosition;
    }

    void SetNextDestination()
    {
      if (!navMeshAgent.enabled) return;

      waypointIndex++;
      if (waypointIndex > wayPointSchema.waypoints.Count - 1) FinalDestination();
      else SetDestination();
    }

    void FinalDestination()
    {
      navMeshAgent.isStopped = false;
      if (enemy != null) enemy.ReachedDestination();
    }

    public void AddModifier(string name, float modifier)
    {
      if (movementModifiers.ContainsKey(name)) movementModifiers[name] = modifier;
      else movementModifiers.Add(name, modifier);
      currentMovementModifier = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;

      SetSpeed(movementSpeed * currentMovementModifier);
    }

    public void RemoveModifier(string name)
    {
      movementModifiers.Remove(name);
      if (movementModifiers.Count > 0)
      {
        float value = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;
        currentMovementModifier = value;
      }
      else currentMovementModifier = 1f;

      SetSpeed(movementSpeed * currentMovementModifier);
    }

    public void SetSpeed(float speed)
    {
      navMeshAgent.speed = speed;
    }

    public void StopMovement(float time)
    {
      AddModifier("DamageTaken", 0);
      stopTime = time;
    }

    void HandleDeath(Damage damage)
    {
      if (navMeshAgent) navMeshAgent.enabled = false;
    }

    bool PathComplete()
    {
      if (!(Vector3.Distance(navMeshAgent.destination, transform.position) <= navMeshAgent.stoppingDistance)) return false;
      return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f;
    }

    void OnDisable()
    {
      enemy.OnDeath -= HandleDeath;
    }
  }
}