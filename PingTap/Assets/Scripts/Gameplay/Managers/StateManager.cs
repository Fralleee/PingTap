using Fralle.Core.Infrastructure;
using Fralle.Core.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class StateManager : MonoBehaviour
	{
		public GameState gameState;
		public IState currentState;

		StateMachine stateMachine;
		readonly Dictionary<GameState, IState> States = new Dictionary<GameState, IState>();

		void Awake()
		{
			SetupStateMachine();
		}

		void SetupStateMachine()
		{
			stateMachine = new StateMachine();

			States.Add(GameState.Prepare, new MatchStatePrepare());
			States.Add(GameState.Live, new MatchStateLive());
			States.Add(GameState.End, new MatchStateEnd());

			SetState(GameState.Prepare);
		}

		void Update()
		{
			stateMachine.Tick();
		}

		public void SetState(GameState newState)
		{
			stateMachine.SetState(States[newState]);
			gameState = newState;
			EventManager.Broadcast(new GameStateChangeEvent(gameState, newState));
		}

	}
}
