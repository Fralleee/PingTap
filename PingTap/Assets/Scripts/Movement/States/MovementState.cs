using UnityEngine;

namespace Fralle.Movement.States
{
  public abstract class MovementState : IState
  {
    //readonly MovementRun run;
    //readonly MovementCrouch crouch;
    //readonly MovementJump jump;
    //readonly MovementHeadBob movementHeadBob;
    //readonly MovementDash dash;
    internal MonoBehaviour[] moves;

    //public MovementStateGrounded(MonoBehaviour[] moves, MovementRun run, MovementCrouch crouch, MovementJump jump, MovementDash dash, MovementHeadBob movementHeadBob)
    protected MovementState(params MonoBehaviour[] moves)
    {

      this.moves = moves;
      //this.run = run;
      //this.crouch = crouch;
      //this.jump = jump;
      //this.dash = dash;
      //this.movementHeadBob = movementHeadBob;
    }

    public virtual void OnEnter()
    {
      //run.enabled = true;
      //crouch.enabled = true;
      //jump.enabled = true;
      //dash.enabled = true;

      foreach (var move in moves)
      {
        move.enabled = true;
      }
    }

    public virtual void Tick()
    {
      //movementHeadBob.GroundedTick();
    }

    public virtual void OnExit()
    {
      //run.enabled = false;
      //crouch.enabled = false;
      //jump.enabled = false;

      foreach (var move in moves)
      {
        move.enabled = false;
      }
    }
  }
}