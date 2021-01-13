using Fralle.Core.Interfaces;

namespace Fralle.Gameplay
{
	public class MatchStateEnd : IState
	{
		public void OnEnter()
		{
			Managers.Instance.State.SetState(MatchState.End);
			Managers.Instance.Match.MatchOver();
		}

		public void Tick()
		{
		}

		public void OnExit()
		{
		}
	}
}
