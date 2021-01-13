namespace Fralle.Gameplay
{
	public static class Events
	{
		public static MenuEvent MenuEvent = new MenuEvent();
		public static OptionsMenuEvent OptionsMenuEvent = new OptionsMenuEvent();
		public static GameStateChangeEvent GameStateChangeEvent = new GameStateChangeEvent();
		public static GameOverEvent GameOverEvent = new GameOverEvent();
		public static LookSensitivityUpdateEvent LookSensitivityUpdateEvent = new LookSensitivityUpdateEvent();
	}

	public class GameEvent { }

	// UI Events.
	public class MenuEvent : GameEvent
	{
		public MenuEventAction MenuEventAction = MenuEventAction.Play;
	}

	public class OptionsMenuEvent : GameEvent
	{
		public bool Active;
	}

	// Gameflow Events.
	public class GameStateChangeEvent : GameEvent
	{
		public MatchState CurrentGameState;
		public MatchState NewGameState;
		public GameStateChangeEvent(MatchState currentState = MatchState.None, MatchState newState = MatchState.None)
		{
			CurrentGameState = currentState;
			NewGameState = newState;
		}
	}

	public class GameOverEvent : GameEvent
	{
		public bool Win;
		public GameOverEvent(bool isWin = false)
		{
			Win = isWin;
		}
	}

	public class LookSensitivityUpdateEvent : GameEvent
	{
		public float Value;
	}
}
