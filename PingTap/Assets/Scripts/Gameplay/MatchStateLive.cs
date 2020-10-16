using Fralle.Core.Interfaces;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class MatchStateLive : IState
	{
		public void OnEnter()
		{
			Managers.Instance.State.SetState(GameState.Live);
			Managers.Instance.Enemy.StartSpawner();

			EventManager.AddListener<GameOverEvent>(OnGameOver);
		}

		public void Tick()
		{
			Managers.Instance.Settings.waveTimer += Time.deltaTime;
		}

		public void OnExit()
		{
			Managers.Instance.Settings.waveTimer = 0;

			EventManager.RemoveListener<GameOverEvent>(OnGameOver);
		}

		void OnGameOver(GameOverEvent evt)
		{
			Managers.Instance.State.SetState(GameState.End);
		}

	}
}
