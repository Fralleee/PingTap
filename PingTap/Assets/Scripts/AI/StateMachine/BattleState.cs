using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap.AI
{
	public abstract class BattleState : ScriptableObject, IState<AIState>
	{
		public AIState identifier => AIState.Battling;

		public abstract void OnEnter();

		public abstract void OnExit();

		public abstract void OnLogic();

		public abstract void Setup(AIBrain aiBrain);

		public BattleState CreateInstance() => Instantiate(this);
	}
}
