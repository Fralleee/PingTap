namespace Fralle.Movement
{
  public class MovementStateDashing : IState
  {
    readonly MovementDash dash;
    public bool dashComplete;

    public MovementStateDashing(MovementDash dash)
    {
      this.dash = dash;
    }

    public void OnEnter()
    {
      dashComplete = false;
      dash.OnComplete += HandleComplete;
      dash.PerformDash();
    }

    public void Tick() { }

    public void OnExit()
    {
      dash.OnComplete -= HandleComplete;
    }

    void HandleComplete()
    {
      dashComplete = true;
    }
  }
}
