using Fralle.Core.Interfaces;
using UnityEngine;

namespace Fralle.Movement.States
{
  public abstract class MovementState : IState
  {
    internal MonoBehaviour[] moves;

    protected MovementState(params MonoBehaviour[] moves)
    {
      this.moves = moves;
    }

    public virtual void OnEnter()
    {
      foreach (var move in moves)
      {
        move.enabled = true;
      }
    }

    public virtual void Tick()
    {
    }

    public virtual void OnExit()
    {
      foreach (var move in moves)
      {
        move.enabled = false;
      }
    }
  }
}