using Fralle.FpsController;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  [CreateAssetMenu(menuName = "AI/States/Wander/Basic")]
  public class BasicWanderState : WanderState
  {
    AIBrain aiBrain;
    AIController controller;

    float wanderDistance = 10f;

    public override void OnEnter()
    {
      controller.Speed = controller.walkSpeed;
    }

    public override void OnLogic()
    {
      if (controller.RemainingDistance > 0.5f)
        return;

      controller.SetRandomDestination(wanderDistance);
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
  }
}
