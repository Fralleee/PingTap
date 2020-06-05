using Fralle.Movement.Moves;
using System.Linq;
using UnityEngine;

namespace Fralle.Movement.States
{
  public class MovementStateDashing : MovementState
  {
    readonly MovementDash dash;
    public bool dashComplete;

    public MovementStateDashing(params MonoBehaviour[] moves) : base(moves)
    {
      dash = (MovementDash)moves.FirstOrDefault(x => x.GetType() == typeof(MovementDash));
    }

    public override void OnEnter()
    {
      base.OnEnter();

      dashComplete = false;
      dash.OnComplete += HandleComplete;
    }

    public override void Tick()
    {
      base.Tick();
    }

    public override void OnExit()
    {
      base.OnExit();

      dash.OnComplete -= HandleComplete;
    }

    void HandleComplete()
    {
      dashComplete = true;
    }
  }
}
