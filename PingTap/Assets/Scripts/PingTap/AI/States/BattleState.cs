using Fralle.Core.HFSM;
using Fralle.PingTap.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
