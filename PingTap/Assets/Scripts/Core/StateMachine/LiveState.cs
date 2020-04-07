using UnityEngine;

namespace Fralle
{
  [CreateAssetMenu(menuName = "State/Live")]
  public class LiveState : State
  {
    public State prepareState;
    public State endState;

    public override void Enter(StateController controller)
    {
      base.Enter(controller);
      if (controller.waveManager.WavesRemaining)
      {
        controller.matchManager.enemiesAlive = controller.waveManager.NextWave();
      }
    }

    public override void Exit(StateController controller)
    {
      controller.matchManager.roundTimer = 0;
    }

    internal override void Tick(StateController controller)
    {
      controller.matchManager.roundTimer += Time.deltaTime;
    }

    internal override void Transition(StateController controller)
    {
      if (controller.matchManager.enemiesAlive == 0)
      {
        controller.SetState(controller.waveManager.WavesRemaining ? prepareState : endState);
      }
    }
  }
}