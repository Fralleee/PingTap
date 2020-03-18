using UnityEngine;
using UnityEngine.AI;

public class AgentNavigation : MonoBehaviour
{
  [SerializeField] NavPoint navPoint;
  [SerializeField] float stoppingDistance;

  NavMeshAgent navMeshAgent;
  Vector3 nextPosition;


  void Awake()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
  }

  void Start()
  {
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
    Destroy(gameObject, .2f);
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
