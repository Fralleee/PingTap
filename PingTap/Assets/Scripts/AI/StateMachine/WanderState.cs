using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  public abstract class WanderState : ScriptableObject, IState<AIState>
  {
    public AIState identifier => AIState.Wandering;

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnLogic();

    public abstract void Setup(AIBrain aiBrain);

    public WanderState CreateInstance() => Instantiate(this);
  }
}
