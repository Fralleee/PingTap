using Fralle.Core;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class MatchState : MonoBehaviour
  {
    [HideInInspector] public MatchManager matchManager;
    [HideInInspector] public WaveManager waveManager;

    StateMachine stateMachine;

    void Awake()
    {
      matchManager = GetComponent<MatchManager>();
      waveManager = GetComponent<WaveManager>();
      stateMachine = new StateMachine();

      var matchStatePrepare = new MatchStatePrepare(waveManager, matchManager);
      var matchStateLive = new MatchStateLive(waveManager, matchManager);
      var matchStateEnd = new MatchStateEnd(waveManager, matchManager);

      stateMachine.AddTransition(matchStatePrepare, matchStateLive, () => matchManager.prepareTimer <= 0);
      stateMachine.AddTransition(matchStateLive, matchStatePrepare,
        () => matchManager.enemiesAlive == 0 && waveManager.WavesRemaining);
      stateMachine.AddTransition(matchStateLive, matchStateEnd,
        () => matchManager.enemiesAlive == 0 && !waveManager.WavesRemaining);

      stateMachine.SetState(matchStatePrepare);
      matchManager.NewState(GameState.Prepare);
    }

    void Update() => stateMachine.Tick();
  }
}