using Fralle.Core.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Gameplay
{
  public class StateManager : MonoBehaviour
  {
    public static event Action<GameState> OnGamestateChanged = delegate { };

    [SerializeField] bool startGame;

    public static GameState GameState = GameState.Playing;
    public MatchState MatchState;
    public IState<MatchState> CurrentState;

    StateMachine<MatchState> stateMachine;
    readonly HashSet<IState<MatchState>> states = new HashSet<IState<MatchState>>();

    void Start()
    {
      SetupStateMachine();
    }

    void SetupStateMachine()
    {
      stateMachine = new StateMachine<MatchState>();

      states.Add(new MatchStatePrepare());
      states.Add(new MatchStateLive());
      states.Add(new MatchStateEnd());

      if (startGame)
      {
        SetState(MatchState.Prepare);
      }
    }

    void Update()
    {
      stateMachine.OnLogic();
    }

    public void SetState(MatchState newState)
    {
      IState<MatchState> state = states.FirstOrDefault(x => x.Identifier == newState);
      stateMachine.SetState(state);
      MatchState = state.Identifier;
      EventManager.Broadcast(new GameStateChangeEvent(MatchState, newState));
    }

    public static void SetGameState(GameState newState)
    {
      OnGamestateChanged(newState);
      GameState = newState;
    }

  }
}
