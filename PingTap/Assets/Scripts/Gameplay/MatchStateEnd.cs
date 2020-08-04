using Fralle.Core.Interfaces;

namespace Fralle.Gameplay
{
  public class MatchStateEnd : IState
  {
    public void OnEnter()
    {
      //MatchManager.Instance.NewState(GameState.End);
      //MatchManager.Instance.Victory();
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
  }
}