using Fralle.Core.AI;
using Fralle.PingTap.AI;

namespace Fralle.PingTap
{
	public class BattleState : IState<AIState>
	{
		public AIState identifier => AIState.Battling;

		public void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public void OnLogic()
		{
			throw new System.NotImplementedException();
		}
	}
}
