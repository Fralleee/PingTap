using Fralle.Core.Infrastructure;
using Fralle.Core.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class StateManager : MonoBehaviour
	{
		public static event Action<GameState> OnGamestateChanged = delegate { };

		[SerializeField] bool startGame;

		public static GameState gameState = GameState.Playing;
		public MatchState matchState;
		public IState currentState;

		StateMachine stateMachine;
		readonly Dictionary<MatchState, IState> States = new Dictionary<MatchState, IState>();

		void Start()
		{
			SetupStateMachine();
		}

		void SetupStateMachine()
		{
			stateMachine = new StateMachine();

			States.Add(MatchState.Prepare, new MatchStatePrepare());
			States.Add(MatchState.Live, new MatchStateLive());
			States.Add(MatchState.End, new MatchStateEnd());

			if (startGame)
			{
				SetState(MatchState.Prepare);
			}
		}

		void Update()
		{
			stateMachine.Tick();
		}

		public void SetState(MatchState newState)
		{
			stateMachine.SetState(States[newState]);
			matchState = newState;
			EventManager.Broadcast(new GameStateChangeEvent(matchState, newState));
		}

		public static void SetGameState(GameState newState)
		{
			OnGamestateChanged(newState);
			gameState = newState;
		}

	}
}
