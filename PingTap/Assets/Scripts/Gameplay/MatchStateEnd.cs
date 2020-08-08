using Fralle.Core.Interfaces;

namespace Fralle.Gameplay
{
  public class MatchStateEnd : IState
  {
    public void OnEnter()
    {
      GameManager.Instance.SetState(GameState.End);
      GameManager.Instance.Victory();
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
  }
}