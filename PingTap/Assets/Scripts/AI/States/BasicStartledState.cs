using Fralle.FpsController;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.PingTap.AI
{
  [CreateAssetMenu(menuName = "AI/States/Startled/Basic")]
  public class BasicStartledState : StartledState
  {
    AIBrain aiBrain;
    AIController controller;

    public override void OnEnter()
    {
      controller.speed = controller.runSpeed;
    }

    public override void OnLogic()
    {
    }

    public override void OnExit()
    {
      controller.Stop(controller.walkSpeed);
    }

    public override void Setup(AIBrain aiBrain)
    {
      this.aiBrain = aiBrain;
      controller = aiBrain.GetComponent<AIController>();
    }

    public override void NewOrigin(Vector3 origin)
    {
      if (NavMesh.SamplePosition(origin, out NavMeshHit navMeshHit, 5f, -1))
        controller.SetDestination(navMeshHit.position);

      aiBrain.AlertOthers(origin, AIState.Startled);
    }
  }
}
