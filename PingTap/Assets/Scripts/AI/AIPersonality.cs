using Fralle.Core.AI;
using Fralle.PingTap.AI;
using UnityEngine;

namespace Fralle.PingTap
{
	public abstract class AIPersonality : ScriptableObject
	{
		public abstract AIPersonality CreateInstance();
		public abstract void Load(AIBrain aiBrain, StateMachine<AIState> stateMachine);
		public abstract void Alert(Vector3 position, AIState alertState);
	}
}
