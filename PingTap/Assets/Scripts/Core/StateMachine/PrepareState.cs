using UnityEngine;

namespace Fralle
{
  [CreateAssetMenu(menuName = "State/Prepare")]
  public class PrepareState : State
  {
    public State liveState;

    public override void Enter(StateController controller)
    {
      base.Enter(controller);
      controller.matchManager.prepareTimer = controller.matchManager.prepareTime;
    }

    public override void Exit(StateController controller)
    {
    }

    internal override void Tick(StateController controller)
    {
      controller.matchManager.prepareTimer -= Time.deltaTime;
    }

    internal override void Transition(StateController controller)
    {
      if (controller.matchManager.prepareTimer <= 0) controller.SetState(liveState);
    }
  }
}