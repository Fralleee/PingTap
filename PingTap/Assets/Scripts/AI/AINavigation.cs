using UnityEngine;

namespace Fralle.AI
{
  public class AINavigation : EnemyNavigation
  {
    public Vector3 targetPosition;

    void Start()
    {
      SetDestination();
    }

    internal override void Update()
    {
      if (PathComplete()) FinalDestination();
      base.Update();
    }

    internal override void SetDestination()
    {
      navMeshAgent.destination = targetPosition;
    }
  }
}