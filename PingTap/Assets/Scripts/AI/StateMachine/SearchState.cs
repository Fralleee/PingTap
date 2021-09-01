using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  public abstract class SearchState : ScriptableObject, IState<AIState>
  {
    public AIState Identifier => AIState.Searching;

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnLogic();

    public abstract void Setup(AIBrain aiBrain);

    public SearchState CreateInstance() => Instantiate(this);
  }
}
