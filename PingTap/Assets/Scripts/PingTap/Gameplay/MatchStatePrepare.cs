using Fralle.Core.Interfaces;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class MatchStatePrepare : IState
	{
		float prepareTime;

		public void OnEnter()
		{
			Managers.Instance.State.SetState(MatchState.Prepare);
			Managers.Instance.Enemy.PrepareSpawner();

			prepareTime = Managers.Instance.Settings.prepareTimer;
		}

		public void Tick()
		{
			prepareTime -= Time.deltaTime;
			if (prepareTime <= 0)
			{
				Managers.Instance.State.SetState(MatchState.Live);
			}
		}

		public void OnExit()
		{
		}
	}
}
