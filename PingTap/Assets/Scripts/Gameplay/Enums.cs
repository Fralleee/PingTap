namespace Fralle.Gameplay
{
  public enum MenuEventAction
  {
    Play,
    ReturnToIntroMenu
  }

  public enum GameState
  {
    MainMenu,
    PauseMenu,
    Playing
  }

  public enum MatchState
  {
    None,
    Prepare,
    Live,
    End
  }

  public enum ObjectiveProgressType
  {
    None,
    Amount,
    Time
  }
}
