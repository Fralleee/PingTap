using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public class AgentNavigation : MonoBehaviour
{
  [SerializeField] NavPoint navPoint;
  [SerializeField] float stoppingDistance;

  Enemy enemy;
  NavMeshAgent navMeshAgent;
  Vector3 nextPosition;

  void Awake()
  {
    enemy = GetComponent<Enemy>();
    navMeshAgent = GetComponent<NavMeshAgent>();
  }

  void Start()
  {
    if (navPoint == null)
    {
      var navPointObject = GameObject.Find("NavPoint 1");
      if (navPointObject) navPoint = navPointObject.GetComponent<NavPoint>();
    }

    if (navPoint)
    {
      nextPosition = navPoint.transform.position.With(y: navMeshAgent.baseOffset);
      navMeshAgent.SetDestination(nextPosition);
    }
    else Debug.LogWarning("NavMeshAgent is missing NavPoint");
  }

  void Update()
  {
    if (navPoint != null) CheckDestinationReached();
  }

  void SetNextDestination()
  {
    var nextPoint = navPoint.GetComponent<NavPoint>().GetNearestPoint(transform.position);
    if (nextPoint == null) FinalDestination();
    else
    {
      navPoint = nextPoint;
      nextPosition = nextPoint.transform.position.With(y: navMeshAgent.baseOffset);
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
    if (navPoint) Gizmos.DrawLine(transform.position, nextPosition);
  }

}
