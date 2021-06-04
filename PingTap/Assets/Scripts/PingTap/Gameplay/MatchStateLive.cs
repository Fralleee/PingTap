using Fralle.Core.HFSM;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class MatchStateLive : IState<MatchState>
	{
		public MatchState identifier => MatchState.Live;
		public void OnEnter()
		{
			Managers.Instance.State.SetState(MatchState.Live);
			Managers.Instance.Enemy.StartSpawner();

			EventManager.AddListener<GameOverEvent>(OnGameOver);
		}

		public void OnLogic()
		{
			Managers.Instance.Settings.WaveTimer += Time.deltaTime;
		}

		public void OnExit()
		{
			Managers.Instance.Settings.WaveTimer = 0;

			EventManager.RemoveListener<GameOverEvent>(OnGameOver);
		}

		void OnGameOver(GameOverEvent evt)
		{
			Managers.Instance.State.SetState(MatchState.End);
		}

	}
}
