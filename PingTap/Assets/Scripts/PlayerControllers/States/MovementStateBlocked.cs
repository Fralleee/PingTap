using UnityEngine;

namespace Fralle.Movement.States
{
  public class MovementStateBlocked : MovementState
  {
    public MovementStateBlocked(params MonoBehaviour[] moves) : base(moves)
    {
    }
  }
}