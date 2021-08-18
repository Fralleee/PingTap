using Fralle.Core.AI;
using UnityEngine;

namespace Fralle.PingTap.AI
{
  public abstract class StartledState : ScriptableObject, IState<AIState>
  {
    public AIState identifier => AIState.Startled;

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnLogic();

    public abstract void Setup(AIBrain aiBrain);

    public abstract void NewOrigin(Vector3 origin);

    public StartledState CreateInstance() => Instantiate(this);
  }
}
