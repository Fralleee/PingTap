namespace Fralle.Gameplay
{
	public enum MenuEventAction
	{
		Play,
		ReturnToIntroMenu
	}

	public enum GameState
	{
		MenuActive,
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
