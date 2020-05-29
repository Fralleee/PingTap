namespace Fralle.Gameplay
{
  public class MatchStateEnd : IState
  {
    readonly WaveManager waveManager;
    readonly MatchManager matchManager;

    public MatchStateEnd(WaveManager waveManager, MatchManager matchManager)
    {
      this.waveManager = waveManager;
      this.matchManager = matchManager;
    }

    public void OnEnter()
    {
      waveManager.NextWave();

      matchManager.NewState(GameState.End);
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
  }
}