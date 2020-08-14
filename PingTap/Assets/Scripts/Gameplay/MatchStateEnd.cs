using Fralle.Core.Interfaces;

namespace Fralle.Gameplay
{
  public class MatchStateEnd : IState
  {
    public void OnEnter()
    {
      GameManager.Instance.SetState(GameState.End);
      if (GameManager.Instance.playerHome.damageController.isDead)
      {
        GameManager.Instance.Defeat();
      }
      else
      {
        GameManager.Instance.Victory();
      }

    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
  }
}