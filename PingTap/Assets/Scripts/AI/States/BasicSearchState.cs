using Fralle.FpsController;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  [CreateAssetMenu(menuName = "AI/States/Search/Basic")]
  public class BasicSearchState : SearchState
  {
    AIController controller;

    float searchDistance = 10f;

    public override void OnEnter()
    {
      controller.Speed = controller.runSpeed;
    }

    public override void OnLogic()
    {
      if (controller.RemainingDistance > 0.5f)
        return;

      controller.SetRandomDestination(searchDistance);
    }

    public override void OnExit()
    {
      controller.Stop(controller.walkSpeed);
    }

    public override void Setup(AIBrain aiBrain)
    {
      controller = aiBrain.GetComponent<AIController>();
    }
  }
}
