using Fralle.FpsController;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  [CreateAssetMenu(menuName = "AI/States/Search/Basic")]
  public class BasicSearchState : SearchState
  {
    AIBrain aiBrain;
    AIController controller;

    float searchDistance = 10f;

    public override void OnEnter()
    {
      controller.Speed = controller.RunSpeed;
    }

    public override void OnLogic()
    {
      if (controller.RemainingDistance > 0.5f)
        return;

      controller.SetRandomDestination(searchDistance);
    }

    public override void OnExit()
    {
      controller.Stop(controller.WalkSpeed);
    }

    public override void Setup(AIBrain aiBrain)
    {
      this.aiBrain = aiBrain;
      controller = aiBrain.GetComponent<AIController>();
    }
  }
}
