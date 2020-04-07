using UnityEngine;

namespace Fralle
{
  [CreateAssetMenu(menuName = "State/End")]
  public class EndState : State
  {
    public override void Enter(StateController controller)
    {
      base.Enter(controller);
      controller.waveManager.NextWave();
    }

    public override void Exit(StateController controller) { }

    internal override void Tick(StateController controller) { }

    internal override void Transition(StateController controller) { }
  }
}