using UnityEngine;

namespace Fralle.PingTap.AI
{
  public enum AIState
  {
    None,
    Wandering,
    Searching,
    Startled,
    Chasing,
    Battling
  }

  static class AIStateExtensions
  {
    public static Color Color(this AIState state)
    {
      return state switch
      {
        AIState.Wandering => UnityEngine.Color.green,
        AIState.Searching => UnityEngine.Color.cyan,
        AIState.Startled => UnityEngine.Color.blue,
        AIState.Chasing => UnityEngine.Color.yellow,
        AIState.Battling => UnityEngine.Color.red,
        _ => UnityEngine.Color.gray,
      };
    }
  }
}
