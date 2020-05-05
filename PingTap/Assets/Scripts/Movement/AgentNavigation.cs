using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentNavigation : MonoBehaviour
{
  public WaypointSchema wayPointSchema;
  [SerializeField] float stoppingDistance;
  
  Enemy enemy;
  NavMeshAgent navMeshAgent;
  Vector3 nextPosition;
  int waypointIndex;
  float defaultSpeed;

  public float currentMovementModifier = 1f;

  Dictionary<string, float> movementModifiers = new Dictionary<string, float>();

  void Awake()
  {
    enemy = GetComponent<Enemy>();
    navMeshAgent = GetComponent<NavMeshAgent>();

    defaultSpeed = navMeshAgent.speed;
  }

  void Start()
  {
    if (!wayPointSchema) return;
    nextPosition = wayPointSchema.waypoints[waypointIndex].With(y: navMeshAgent.baseOffset);
    navMeshAgent.SetDestination(nextPosition);
  }

  void Update()
  {
    if (nextPosition != Vector3.zero) CheckDestinationReached();
  }

  void SetNextDestination()
  {
    if (!navMeshAgent.enabled) return;
    waypointIndex++;
    if (waypointIndex > wayPointSchema.waypoints.Count - 1) FinalDestination();
    else
    {
      nextPosition = wayPointSchema.waypoints[waypointIndex].With(y: navMeshAgent.baseOffset);
      navMeshAgent.SetDestination(nextPosition);
    }
  }

  void FinalDestination()
  {
    navMeshAgent.isStopped = true;
    enemy?.ReachedDestination();
  }

  void CheckDestinationReached()
  {
    float distanceToTarget = Vector3.Distance(transform.position, nextPosition);
    if (distanceToTarget < stoppingDistance) SetNextDestination();
  }

  public void AddModifier(string name, float modifier)
  {
    if (movementModifiers.ContainsKey(name)) movementModifiers[name] = modifier;
    else movementModifiers.Add(name, modifier);
    currentMovementModifier = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;
    navMeshAgent.speed = defaultSpeed * currentMovementModifier;
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
    navMeshAgent.speed = defaultSpeed * currentMovementModifier;
  }
}
