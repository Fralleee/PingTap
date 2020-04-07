using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle
{
  public abstract class State : ScriptableObject
  {
    public GameState gameState;

    public void UpdateState(StateController controller)
    {
      Tick(controller);
      Transition(controller);
    }

    public virtual void Enter(StateController controller)
    {
      controller.matchManager.gameState = gameState;
    }

    public abstract void Exit(StateController controller);

    internal abstract void Tick(StateController controller);

    internal abstract void Transition(StateController controller);
  }
}