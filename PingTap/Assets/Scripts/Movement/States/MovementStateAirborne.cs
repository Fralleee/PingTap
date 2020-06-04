using UnityEngine;

namespace Fralle.Movement
{
  public class MovementStateAirborne : IState
  {
    readonly MovementAirControl airControl;
    readonly MovementGravityAdjuster gravityAdjuster;
    readonly HeadBob headBob;
    readonly Rigidbody rigidBody;

    public MovementStateAirborne(MovementAirControl airControl, MovementGravityAdjuster gravityAdjuster, HeadBob headBob, Rigidbody rigidBody)
    {
      this.airControl = airControl;
      this.gravityAdjuster = gravityAdjuster;
      this.headBob = headBob;
      this.rigidBody = rigidBody;
    }

    public void OnEnter()
    {
      gravityAdjuster.enabled = true;
      airControl.enabled = true;

      var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
      airControl.startSpeed = horizontalMovement.magnitude;
    }

    public void Tick()
    {
      headBob.AirborneTick();
    }

    public void OnExit()
    {
      airControl.startSpeed = 0f;
      airControl.enabled = false;
      gravityAdjuster.enabled = false;
    }
  }
}
