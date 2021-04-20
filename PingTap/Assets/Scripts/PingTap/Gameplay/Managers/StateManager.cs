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

		public static GameState GameState = GameState.Playing;
		public MatchState MatchState;
		public IState CurrentState;

		StateMachine stateMachine;
		readonly Dictionary<MatchState, IState> states = new Dictionary<MatchState, IState>();

		void Start()
		{
			SetupStateMachine();
		}

		void SetupStateMachine()
		{
			stateMachine = new StateMachine();

			states.Add(MatchState.Prepare, new MatchStatePrepare());
			states.Add(MatchState.Live, new MatchStateLive());
			states.Add(MatchState.End, new MatchStateEnd());

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
			stateMachine.SetState(states[newState]);
			MatchState = newState;
			EventManager.Broadcast(new GameStateChangeEvent(MatchState, newState));
		}

		public static void SetGameState(GameState newState)
		{
			OnGamestateChanged(newState);
			GameState = newState;
		}

	}
}
