using Fralle.Core.AI;
using Fralle.PingTap.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	public abstract class AIPersonality : ScriptableObject
	{
		public abstract void Load(AIBrain aiBrain, StateMachine<AIState> stateMachine);
		public abstract AIPersonality CreateInstance();
	}
}
