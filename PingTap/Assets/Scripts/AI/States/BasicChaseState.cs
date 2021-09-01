using Fralle.FpsController;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  [CreateAssetMenu(menuName = "AI/States/Chase/Basic")]
  public class BasicChaseState : ChaseState
  {
    AIBrain aiBrain;
    AITargetingSystem aiTargetingSystem;
    AIController controller;

    public override void OnEnter()
    {
      controller.Speed = controller.RunSpeed;

      aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
    }

    public override void OnLogic()
    {
      controller.SetDestination(aiTargetingSystem.TargetPosition);

      if (Time.time > aiBrain.lastAlert)
        aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
    }

    public override void OnExit()
    {
      controller.Stop(controller.WalkSpeed);
    }

    public override void Setup(AIBrain aiBrain)
    {
      this.aiBrain = aiBrain;
      aiTargetingSystem = aiBrain.GetComponent<AITargetingSystem>();
      controller = aiBrain.GetComponent<AIController>();
    }
  }
}
