using Fralle.Movement.Moves;
using System.Linq;
using UnityEngine;

namespace Fralle.Movement.States
{
  public class MovementStateAirborne : MovementState
  {
    readonly Rigidbody rigidBody;
    readonly MovementAirControl airControl;
    readonly MovementHeadBob headBob;

    public MovementStateAirborne(Rigidbody rb, params MonoBehaviour[] moves) : base(moves)
    {
      rigidBody = rb;

      airControl = (MovementAirControl)moves.FirstOrDefault(x => x.GetType() == typeof(MovementAirControl));
      headBob = (MovementHeadBob)moves.FirstOrDefault(x => x.GetType() == typeof(MovementHeadBob));
    }

    public override void OnEnter()
    {
      base.OnEnter();

      var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
      airControl.startSpeed = horizontalMovement.magnitude;
    }

    public override void Tick()
    {
      base.Tick();

      headBob.AirborneTick();
    }

    public override void OnExit()
    {
      base.OnExit();

      airControl.startSpeed = 0f;
      airControl.enabled = false;
    }
  }
}
