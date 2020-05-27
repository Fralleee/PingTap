using Fralle.Movement;
using UnityEngine;

namespace Fralle.AI
{
  public class EnemyWaypointNavigation : EnemyNavigation
  {
    public WaypointSchema wayPointSchema;

    Vector3 nextPosition;
    int waypointIndex;

    void Start()
    {
      if (!wayPointSchema) return;
      SetDestination();

    }

    internal override void Update()
    {
      if (PathComplete()) SetNextDestination();
      base.Update();
    }


    internal override void SetDestination()
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
  }
}