namespace Fralle.Movement
{
  public class MovementStateGrounded : IState
  {
    readonly MovementRun run;
    readonly MovementCrouch crouch;
    readonly MovementJump jump;
    readonly HeadBob headBob;

    public MovementStateGrounded(MovementRun run, MovementCrouch crouch, MovementJump jump, HeadBob headBob)
    {
      this.run = run;
      this.crouch = crouch;
      this.jump = jump;
      this.headBob = headBob;
    }

    public void OnEnter()
    {
      run.enabled = true;
      crouch.enabled = true;
      jump.enabled = true;
    }

    public void Tick()
    {
      headBob.GroundedTick();
    }

    public void OnExit()
    {
      run.enabled = false;
      crouch.enabled = false;
      jump.enabled = false;
    }
  }
}
