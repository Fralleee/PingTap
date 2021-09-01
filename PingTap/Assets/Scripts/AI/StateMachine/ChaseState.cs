using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  public abstract class ChaseState : ScriptableObject, IState<AIState>
  {
    public AIState Identifier => AIState.Chasing;

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnLogic();

    public abstract void Setup(AIBrain aiBrain);

    public ChaseState CreateInstance() => Instantiate(this);
  }
}
