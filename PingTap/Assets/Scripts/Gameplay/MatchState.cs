using Fralle.Core;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchState : MonoBehaviour
  {
    StateMachine stateMachine;

    void Awake()
    {
      stateMachine = new StateMachine();

      var matchStatePrepare = new MatchStatePrepare();
      var matchStateLive = new MatchStateLive();
      var matchStateEnd = new MatchStateEnd();

      stateMachine.AddTransition(matchStatePrepare, matchStateLive, () => MatchManager.Instance.prepareTimer <= 0);
      stateMachine.AddTransition(matchStateLive, matchStatePrepare,
        () => MatchManager.Instance.enemiesAlive == 0 && WaveManager.Instance.WavesRemaining);
      stateMachine.AddTransition(matchStateLive, matchStateEnd,
        () => MatchManager.Instance.enemiesAlive == 0 && !WaveManager.Instance.WavesRemaining);

      stateMachine.SetState(matchStatePrepare);
      MatchManager.Instance.NewState(GameState.Prepare);
    }

    void Update() => stateMachine.Tick();
  }
}