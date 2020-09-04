using UnityEngine;

namespace Fralle.AI
{
  public class AINavigation : EnemyNavigation
  {
    public Vector3 targetPosition;

    AIController aiController;

    protected new void Awake()
    {
      base.Awake();
      aiController = GetComponent<AIController>();
    }

    void Start()
    {
      SetDestination();
      aiController.IsMoving = true;
    }

    internal override void Update()
    {
      if (PathComplete())
      {
        aiController.IsMoving = false;
        FinalDestination();
      }
      base.Update();
    }

    internal override void SetDestination()
    {
      navMeshAgent.destination = targetPosition;
    }
  }
}