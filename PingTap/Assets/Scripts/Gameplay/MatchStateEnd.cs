namespace Fralle.Gameplay
{
  public class MatchStateEnd : IState
  {
    public void OnEnter()
    {
      WaveManager.Instance.NextWave();

      MatchManager.Instance.NewState(GameState.End);
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
  }
}