using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public class AgentNavigation : MonoBehaviour
{
  public WaypointSchema wayPointSchema;
  [SerializeField] float stoppingDistance;

  Enemy enemy;
  NavMeshAgent navMeshAgent;
  Vector3 nextPosition;
  int waypointIndex;

  void Awake()
  {
    enemy = GetComponent<Enemy>();
    navMeshAgent = GetComponent<NavMeshAgent>();
  }

  void Start()
  {
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
    enemy.ReachedDestination();
  }

  void CheckDestinationReached()
  {
    float distanceToTarget = Vector3.Distance(transform.position, nextPosition);
    if (distanceToTarget < stoppingDistance) SetNextDestination();
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(0, 0, 1, 1F);
    Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    if (nextPosition != Vector3.zero) Gizmos.DrawLine(transform.position, nextPosition);
  }

}
