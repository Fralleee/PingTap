using Fralle.Movement.Moves;

namespace Fralle.Movement.States
{
  public class MovementStateGrounded : IState
  {
    readonly MovementRun run;
    readonly MovementCrouch crouch;
    readonly MovementJump jump;
    readonly MovementHeadBob movementHeadBob;

    public MovementStateGrounded(MovementRun run, MovementCrouch crouch, MovementJump jump, MovementHeadBob movementHeadBob)
    {
      this.run = run;
      this.crouch = crouch;
      this.jump = jump;
      this.movementHeadBob = movementHeadBob;
    }

    public void OnEnter()
    {
      run.enabled = true;
      crouch.enabled = true;
      jump.enabled = true;
    }

    public void Tick()
    {
      movementHeadBob.GroundedTick();
    }

    public void OnExit()
    {
      run.enabled = false;
      crouch.enabled = false;
      jump.enabled = false;
    }
  }
}
