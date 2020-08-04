using UnityEngine;

namespace Fralle.AI
{
  public class AINavigation : EnemyNavigation
  {
    public Transform target;

    void Start()
    {
      if (target) SetDestination();
    }

    internal override void Update()
    {
      if (PathComplete()) FinalDestination();
      base.Update();
    }

    internal override void SetDestination()
    {
      navMeshAgent.destination = target.position;
    }
  }
}