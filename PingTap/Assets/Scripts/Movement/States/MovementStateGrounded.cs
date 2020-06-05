using Fralle.Movement.Moves;
using System.Linq;
using UnityEngine;

namespace Fralle.Movement.States
{
  public class MovementStateGrounded : MovementState
  {
    readonly MovementHeadBob headBob;
    public MovementStateGrounded(params MonoBehaviour[] moves) : base(moves)
    {
      headBob = (MovementHeadBob)moves.FirstOrDefault(x => x.GetType() == typeof(MovementHeadBob));
    }

    public override void OnEnter()
    {
      base.OnEnter();
    }

    public override void Tick()
    {
      base.Tick();

      headBob.GroundedTick();
    }

    public override void OnExit()
    {
      base.OnExit();
    }
  }
}
